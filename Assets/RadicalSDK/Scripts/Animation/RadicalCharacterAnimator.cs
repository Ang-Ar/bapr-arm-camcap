using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Radical
{
    /// <summary>
    /// Not compatible with custom characters, create custom characters with the EditorModeServer on the Controller prefab
    /// </summary>
    public class RadicalCharacterAnimator : MonoBehaviour
    {
        #region Armature
        Transform Root;
        [Tooltip("This is the parent of the hip bone, which is usually the entire armature")]
        public Transform armature;
        //[HideInInspector] public Transform Head;
        //[HideInInspector] public Transform Neck;
        //[HideInInspector] public Transform Spine;
        //[HideInInspector] public Transform Spine1;
        //[HideInInspector] public Transform Spine2;

        ////[Header("Left Arm")]
        //[HideInInspector] public Transform LeftShoulder;
        //[HideInInspector] public Transform LeftArm;
        //[HideInInspector] public Transform LeftForeArm;
        //[HideInInspector] public Transform LeftHand;

        ////[Header("Right Arm")]
        //[HideInInspector] public Transform RightShoulder;
        //[HideInInspector] public Transform RightArm;
        //[HideInInspector] public Transform RightForeArm;
        //[HideInInspector] public Transform RightHand;

        ////[Header("Left Leg")]
        //[HideInInspector] public Transform LeftUpLeg;
        //[HideInInspector] public Transform LeftLeg;
        //[HideInInspector] public Transform LeftFoot;

        ////[Header("Right Leg")]
        //[HideInInspector] public Transform RightUpLeg;
        //[HideInInspector] public Transform RightLeg;
        //[HideInInspector] public Transform RightFoot;

        [HideInInspector] public bool isNativeCharacter; // if it's the original character or one that has the same armature, we don't need helpers
        #endregion
        
        [HideInInspector] public bool isBaking;
        [HideInInspector] public bool isFaceRigged;
        int[] blendShapeIndexMatrix;
        SkinnedMeshRenderer body;
        SkinnedMeshRenderer face;
        Dictionary<string, Bone> rig; //Store a bone struct that can store the rest rotation of each bone

        List<BakedFrameData> bakedFrames; //Collect baked frames to store them in the AnimationPlaybackObject
        List<BakedFaceData> bakedFaces;
        
        string animationSaveName;

        protected FrameData nextFrameData; //Buffer the last frame data, because we cannot apply it from the thread that manages the websocket
        protected FaceData nextFaceData;

        private RetargetType retargetType;
        
        Vector3 retargetHipDelta; // the hip's position is in world space, so we need to compensate for retargetted characters of different heights
        

        protected virtual void Awake()
        {
            //print("Starting " + name);
            isNativeCharacter = true;
            BindSkeletton();
            tryStoreBlendShapeMatrix(gameObject);
            nextFrameData.hasBeenProcessed = true; // don't start trying to process before the 1. frame has been received
            nextFaceData.hasBeenProcessed = true;
        }

        public void StartBaking(string animationName)
        {
            isBaking = true;
            animationSaveName = animationName;
            bakedFrames = new List<BakedFrameData>();
            bakedFaces = new List<BakedFaceData>();
        }

        /// <summary>
        /// Store the framedata, so we can pose the character on next fixed update
        /// </summary>
        /// <param name="frameData"></param>
        public void AddFrameData(FrameData frameData, FaceData faceData)
        {
            //print($"Adding frame data for {name} : {frameData}");
            nextFrameData = frameData;
            nextFaceData = faceData;
            if (isBaking)
            {
                bake(frameData, faceData);
            }
        }

        protected virtual void FixedUpdate()
        {
            // Read the pose from the frame data in the buffer
            setPose();
            SetFacialExpression(nextFaceData);
            //TODO: Baking needs to read all frames in the buffer, here we potentially occasionally loose some
        }
        public void SetPose(BakedFrameData pose)
        {
            pose.root_t.x *= -1f;
            Root.localPosition = pose.root_t;
            foreach (var bone in rig)
            {
                Quaternion rotation = pose.GetBoneRotation(bone.Key);
                bone.Value.ApplyRotation(rotation);
            }
        }

        void bake(FrameData frame, FaceData face) 
        {
            if (frame.frame == 0 || frame.root_t == null)
            {
                return; //empty frame
            }
            bakedFrames.Add(frame.Bake());
            if (face.frame > 0)
                bakedFaces.Add(face.Bake());
        }

        #region Set the Pose according to the frame data

        void setPose()
        {
            // set the position/rotation of every bone
            if (nextFrameData.hasBeenProcessed) return;

            setRootTransforms(nextFrameData);
            foreach (var bone in rig)
            {
                float[] rotation = nextFrameData.GetBoneRotation(bone.Key);
                if (rotation == null) return; //sometimes the package is incomplete, FIXME: Skip data for baking?
                bone.Value.ApplyRotation(rotation);
            }
            nextFrameData.hasBeenProcessed = true;
        }

        void setRootTransforms(FrameData frameData)
        {
            // Convert the root specific transform data to Vector and Quaternion
            float[] positions = frameData.root_t;
            float x = -positions[0]; //Invert x direction
            float y = positions[1];
            float z = positions[2];
            Vector3 position = new Vector3(x, y, z) + retargetHipDelta;
            Root.localPosition = position;
        }
#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            storeBakedAnimation();
        }

        protected void storeBakedAnimation()
        {
            if (isBaking) //If this character has baked data, save it to a ScriptableObject
            {
                AnimationPlaybackObject playbackObject = ScriptableObject.CreateInstance<AnimationPlaybackObject>();
                playbackObject.playerID = name;
                refineAnimation(out BakedFrameData[] l_bakedFrames, out BakedFaceData[] l_bakedFaces);

                playbackObject.animation = l_bakedFrames;

                if (l_bakedFaces != null)
                {
                    playbackObject.facialAnimation = l_bakedFaces;
                    playbackObject.hasFace = true;
                }
                string filepath = $"Assets/RadicalSDK/RadicalCharacterAnimation/{animationSaveName}_animation.asset";
                print("Saved to " + filepath.Split('.')[0]);
                EditorUtility.SetDirty(playbackObject);
                AssetDatabase.CreateAsset(playbackObject, filepath); //FIXME: Allow mutliple saved animations per player(name) ?
                AssetDatabase.SaveAssets();
                

            }
        }

        void refineAnimation(out BakedFrameData[] l_bakedFrames, out BakedFaceData[] l_bakedFaces)
        {
            int frames = bakedFrames.Count;
            int faces = bakedFaces.Count;
            int startFrame = bakedFrames[0].frame;
            int endFrame = bakedFrames[frames - 1].frame;
            int expectedFrames = endFrame - startFrame + 1;
            if (expectedFrames < 0) // frame overflow (e.g started at 800, ended 200)
            {
                l_bakedFrames = bakedFrames.ToArray();
                l_bakedFaces = bakedFaces.Count > 0 ? bakedFaces.ToArray() : null;
                return;
            }
            if (frames == expectedFrames)
                print(name + ": No dropped frames");

            {
                
                l_bakedFrames = bakedFrames.ToArray();
                l_bakedFaces = isFaceRigged ? bakedFaces.ToArray() : null;
                return;
            }
            int length = frames;
            int lastFrame = bakedFrames[0].frame - 1;
            l_bakedFrames = new BakedFrameData[expectedFrames];
            int offset = 0;
            
            for (int i = 0; i < length; i++)
            {
                BakedFrameData bakedFrame = bakedFrames[i];
                print(bakedFrame);
                int currentFrame = bakedFrame.frame;
                if (currentFrame < lastFrame)
                {
                    offset += currentFrame - lastFrame;
                    l_bakedFrames[i + offset] = bakedFrames[i];
                    lastFrame = currentFrame + offset;
                }
                else if (currentFrame == lastFrame)
                {
                    lastFrame = currentFrame;
                }
                else
                    lastFrame = currentFrame;
            } //TODO: Refine the face as well
            
            l_bakedFaces = bakedFaces.ToArray();
        }
#endif

        #endregion

        #region Facial Animation
        public void SetFacialExpression(FaceData faceData)
        {
            if (!isFaceRigged || faceData.hasBeenProcessed) return;

            float[] blendShapeValues = nextFaceData.ToArray();
            setFacialExpression(blendShapeValues);
            nextFaceData.hasBeenProcessed = true;
        }

        void setFacialExpression(float[] blendShapeValues)
        {
            if (!isFaceRigged) return;
            int length = blendShapeValues.Length;
            for (int i = 0; i < length; i++)
            {
                int index = blendShapeIndexMatrix[i];
                //for (int j = 0; j < morphTargets.Length; j++)
                //{
                //    morphTargets[j].SetBlendShapeWeight(index, blendShapeValues[i]);
                //}
                face.SetBlendShapeWeight(index, blendShapeValues[i]);
            }
        }

        //public void SwitchCharacter(Dictionary<string, Transform> boneDict, Transform newCharacter)
        //{
        //    StartCoroutine(WaitForSwitchingCharacter(boneDict, newCharacter));
        //}

        //IEnumerator WaitForSwitchingCharacter(Dictionary<string, Transform> boneDict, Transform newCharacter)
        //{
        //    yield return new WaitForEndOfFrame(); // make sure the stream does not interfere with the bone placement
        //    float y = boneDict["Hips"].position.y - 0.967506f; //TODO: Make sure hips are always called hips, On changing the radical character, we need to change this value
        //    retargetHipDelta = new Vector3(0, y, 0);
        //    int length = boneDict.Count;
        //    BoneSwitch[] boneSwitches = new BoneSwitch[length];
        //    transform.position = newCharacter.position;
        //    //Transform parent = boneDict["Hips"].parent;
        //    //Root.SetParent(parent, true);
        //    int i = 0;
        //    foreach (var entry in rig)
        //    {
        //        string key = entry.Key;
        //        Bone bone = entry.Value;
                
        //        //bm.position = bone.bone.position;
        //        boneSwitches[i] = new BoneSwitch(bone, boneDict[key]);
        //        bone.bone.name += "_Radical";
        //        //Transform bm2 = Instantiate(bmrmp, boneDict[key]).transform;
        //        i++;
        //        //bm2.position = bone.bone.position;
        //        //Transform radicalBone = bone.bone;
        //        //Transform targetBone = 
        //        //radicalBone.position = targetBone.position;
        //        //targetBone.SetParent(radicalBone, true);
        //    }
        //     for (i = 0; i < length; i++)
        //    {
        //        boneSwitches[i].Reparent();
        //    }
        //    SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        //    length = meshes.Length;
        //    for (i = 0; i < length; i++)
        //    {
        //        Destroy(meshes[i].gameObject);
        //    }
        //    tryStoreBlendShapeMatrix(newCharacter.gameObject);
        //}

        public void SetFacialExpression(BakedFaceData faceData)
        {
            //if (!isFaceRigged || faceData.hasBeenProcessed) return;

            float[] blendShapeValues = faceData.ToArray();
           setFacialExpression(blendShapeValues);
        }
        protected void tryStoreBlendShapeMatrix(GameObject character)
        {
            int length;
            //List<SkinnedMeshRenderer> l_morphTargets = new List<SkinnedMeshRenderer>();
            if (!character.TryGetComponent<SkinnedMeshRenderer>(out _))
            {
                SkinnedMeshRenderer[] smr = character.GetComponentsInChildren<SkinnedMeshRenderer>();
                length = smr.Length;
                
                for (int i = 0; i < length; i++)
                {
                    Mesh l_mesh = smr[i].sharedMesh;
                    if (l_mesh.blendShapeCount == 53)
                    {
                        face = smr[i];
                        break;
                    }
                    //l_morphTargets.Add(smr[i]);
                    else
                        body = smr[i]; //if the character is split into more than 1 mesh, it keeps getting re-assigned, should not be a problem though
                }
            }
            //if (l_morphTargets.Count == 0)
            if (face == null)
            {
                isFaceRigged = false;
                return;
            }
            //else if (body == null) // body and face are the same mesh
            //    body = face;

            //print("Getting blendshapes for " + name + " from " + face.name);
            //Mesh mesh = skinnedMesh.sharedMesh;
            
            string[] names = {
                "browDownLeft",
                "browDownRight",
                "browInnerUp",
                "browOuterUpLeft",
                "browOuterUpRight",
                "cheekPuff",
                "cheekSquintLeft",
                "cheekSquintRight",
                "eyeBlinkLeft",
                "eyeBlinkRight",
                "eyeLookDownLeft",
                "eyeLookDownRight",
                "eyeLookInLeft",
                "eyeLookInRight",
                "eyeLookOutLeft",
                "eyeLookOutRight",
                "eyeLookUpLeft",
                "eyeLookUpRight",
                "eyeSquintLeft",
                "eyeSquintRight",
                "eyeWideLeft",
                "eyeWideRight",
                "jawForward",
                "jawLeft",
                "jawOpen",
                "jawRight",
                "mouthClose",
                "mouthDimpleLeft",
                "mouthDimpleRight",
                "mouthFrownLeft",
                "mouthFrownRight",
                "mouthFunnel",
                "mouthLeft",
                "mouthLowerDownLeft",
                "mouthLowerDownRight",
                "mouthPressLeft",
                "mouthPressRight",
                "mouthPucker",
                "mouthRight",
                "mouthRollLower",
                "mouthRollUpper",
                "mouthShrugLower",
                "mouthShrugUpper",
                "mouthSmileLeft",
                "mouthSmileRight",
                "mouthStretchLeft",
                "mouthStretchRight",
                "mouthUpperUpLeft",
                "mouthUpperUpRight",
                "noseSneerLeft",
                "noseSneerRight",
                "tongueOut"
            };
            length = names.Length;


            //try
            {
                blendShapeIndexMatrix = new int[length];
                //int morphtargets = l_morphTargets.Count;


                Mesh mesh = face.sharedMesh;

                for (int j = 0; j < length; j++)
                {
                    string l_name = names[j];
                    int index = mesh.GetBlendShapeIndex(l_name);
                    if (index < 0)
                    {
                        print($"{mesh.name} is missing blendshape {l_name}, removing from facial animation.");
                        face = null;
                        isFaceRigged = false;
                        return;
                    }
                    blendShapeIndexMatrix[j] = index;
                }
                isFaceRigged = true;
                //if (l_morphTargets.Count > 0) //TODO: this fails if the list only contains null entries
                //{
                //    //morphTargets = l_morphTargets.OfType<SkinnedMeshRenderer>().ToArray();// Select(o => null).
                //    isFaceRigged = true;
                //}
                //else
                //    isFaceRigged = false;
            }
            //if this fails somehow:
            //catch
            //{
                //print("Getting face rig failed for: " + mesh.name);
            //    isFaceRigged = false;
            //}
        }
        //public void SetFacialExpression(FaceData faceData)
        //{
        //    float[] blendShapeValues = faceData.ToArray();
        //    int length = blendShapeValues.Length;
        //    for (int i = 0; i < length; i++)
        //    {
        //        int index = blendShapeIndexMatrix[i];
        //        skinnedMesh.SetBlendShapeWeight(index, blendShapeValues[i]);
        //    }
        //}
        #endregion

        #region Editor
        public void BindSkeletton()
        {
            //could only be used to animate a character with all bones of the default radical character (None can be missing)
            //print("Binding");
            //armature = transform;//.GetChild(0);
            //print("Binding " + name);
            if (armature == null)
                armature = transform.Find("Root"); //specific for the driver
            Root = armature.Find("Hips");

            Dictionary<string, Transform> tmpBoneDict = new Dictionary<string, Transform>
            {
                {"LeftShoulder",        armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder")},
                {"LeftArm",             armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm")},
                {"LeftForeArm",         armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm")},
                {"LeftHand",            armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand")},

                {"LeftHandIndex1",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandIndex1")},
                {"LeftHandIndex2",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandIndex1/LeftHandIndex2")},
                {"LeftHandIndex3",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandIndex1/LeftHandIndex2/LeftHandIndex3")},

                {"LeftHandMiddle1",     armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandMiddle1")},
                {"LeftHandMiddle2",     armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandMiddle1/LeftHandMiddle2")},
                {"LeftHandMiddle3",     armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandMiddle1/LeftHandMiddle2/LeftHandMiddle3")},

                {"LeftHandRing1",       armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandRing1")},
                {"LeftHandRing2",       armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandRing1/LeftHandRing2")},
                {"LeftHandRing3",       armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandRing1/LeftHandRing2/LeftHandRing3")},

                {"LeftHandPinky1",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandPinky1")},
                {"LeftHandPinky2",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandPinky1/LeftHandPinky2")},
                {"LeftHandPinky3",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandPinky1/LeftHandPinky2/LeftHandPinky3")},

                {"LeftHandThumb1",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandThumb1")},
                {"LeftHandThumb2",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandThumb1/LeftHandThumb2")},
                {"LeftHandThumb3",      armature.Find("Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand/LeftHandThumb1/LeftHandThumb2/LeftHandThumb3")},

                {"LeftUpLeg",           armature.Find("Hips/LeftUpLeg")},
                {"LeftLeg",             armature.Find("Hips/LeftUpLeg/LeftLeg")},
                {"LeftFoot",            armature.Find("Hips/LeftUpLeg/LeftLeg/LeftFoot") },

                {"RightShoulder",       armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder")},
                {"RightArm",            armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm")},
                {"RightForeArm",        armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm")},
                {"RightHand",           armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand")},

                {"RightHandIndex1",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandIndex1")},
                {"RightHandIndex2",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandIndex1/RightHandIndex2")},
                {"RightHandIndex3",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandIndex1/RightHandIndex2/RightHandIndex3")},

                {"RightHandMiddle1",    armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandMiddle1")},
                {"RightHandMiddle2",    armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandMiddle1/RightHandMiddle2")},
                {"RightHandMiddle3",    armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandMiddle1/RightHandMiddle2/RightHandMiddle3")},

                {"RightHandRing1",      armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandRing1")},
                {"RightHandRing2",      armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandRing1/RightHandRing2")},
                {"RightHandRing3",      armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandRing1/RightHandRing2/RightHandRing3")},

                {"RightHandPinky1",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandPinky1")},
                {"RightHandPinky2",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandPinky1/RightHandPinky2")},
                {"RightHandPinky3",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandPinky1/RightHandPinky2/RightHandPinky3")},

                {"RightHandThumb1",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandThumb1")},
                {"RightHandThumb2",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandThumb1/RightHandThumb2")},
                {"RightHandThumb3",     armature.Find("Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/RightHandThumb1/RightHandThumb2/RightHandThumb3")},

                {"RightUpLeg",          armature.Find("Hips/RightUpLeg")},
                {"RightLeg",            armature.Find("Hips/RightUpLeg/RightLeg")},
                {"RightFoot",           armature.Find("Hips/RightUpLeg/RightLeg/RightFoot") },

                {"Hips",                armature.Find("Hips") },
                {"Spine",               armature.Find("Hips/Spine")},
                {"Spine1",              armature.Find("Hips/Spine/Spine1")},
                {"Spine2",              armature.Find("Hips/Spine/Spine1/Spine2")},
                {"Neck",                armature.Find("Hips/Spine/Spine1/Spine2/Neck")},
                {"Head",                armature.Find("Hips/Spine/Spine1/Spine2/Neck/Head")},
            };
            convertRig(tmpBoneDict);
        }

        protected void convertRig(Dictionary<string, Transform> boneDict)
        {
            rig = new Dictionary<string, Bone>();
            foreach (var item in boneDict)
            {
                if (item.Value == null)
                {
                    print(name + ": I'm missing " + item.Key);
                }
                else
                {
                    string l_name = item.Key;
                    rig.Add(l_name, new Bone(item.Value, l_name));
                }
            }
        }
        #endregion

    }
}
