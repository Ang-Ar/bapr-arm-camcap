using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    [System.Serializable]
    public struct BakedFrameData
    {
        public Vector3 root_t;
        public Quaternion Head_r,
            LeftArm_r,
            LeftFoot_r,
            LeftForeArm_r,
            LeftHand_r,
            LeftHandThumb1_r,
            LeftHandThumb2_r,
            LeftHandThumb3_r,
            LeftHandIndex1_r,
            LeftHandIndex2_r,
            LeftHandIndex3_r,
            LeftHandMiddle1_r,
            LeftHandMiddle2_r,
            LeftHandMiddle3_r,
            LeftHandRing1_r,
            LeftHandRing2_r,
            LeftHandRing3_r,
            LeftHandPinky1_r,
            LeftHandPinky2_r,
            LeftHandPinky3_r,
            LeftLeg_r,
            LeftShoulder_r,
            LeftUpLeg_r,
            Neck_r,
            RightArm_r,
            RightFoot_r,
            RightForeArm_r,
            RightHand_r,
            RightHandThumb1_r,
            RightHandThumb2_r,
            RightHandThumb3_r,
            RightHandIndex1_r,
            RightHandIndex2_r,
            RightHandIndex3_r,
            RightHandMiddle1_r,
            RightHandMiddle2_r,
            RightHandMiddle3_r,
            RightHandRing1_r,
            RightHandRing2_r,
            RightHandRing3_r,
            RightHandPinky1_r,
            RightHandPinky2_r,
            RightHandPinky3_r,
            
            RightLeg_r,
            RightShoulder_r,
            RightUpLeg_r,
            Spine1_r,
            Spine2_r,
            Spine_r,
            root_r;

        public int frame;
        public override string ToString()
        {
            return $"R: {root_r}, T:  {root_t}, Index: {RightHandIndex1_r}";
        }
        // One frame data ready to be stored as a Unity Asset
        //public BakedFrameData Bake(FrameData frameData)
        //{
        //    float[] p = frameData.root_t;
        //    Vector3 rootPosition = new Vector3(p[0], p[1], p[2]);
        //    return new BakedFrameData //FIXME: We are converting to quaternion twice, might be faster to read the bake info right before the bone is rotated
        //    {
        //        Head_r          = toQuaternion(frameData.Head_r),
        //        LeftArm_r       = toQuaternion(frameData.LeftArm_r),
        //        LeftFoot_r      = toQuaternion(frameData.LeftFoot_r),
        //        LeftForeArm_r   = toQuaternion(frameData.LeftForeArm_r),
        //        LeftHand_r      = toQuaternion(frameData.LeftHand_r),
        //        LeftLeg_r       = toQuaternion(frameData.LeftLeg_r),
        //        LeftShoulder_r  = toQuaternion(frameData.LeftShoulder_r),
        //        LeftUpLeg_r     = toQuaternion(frameData.LeftUpLeg_r),
        //        Neck_r          = toQuaternion(frameData.Neck_r),
        //        RightArm_r      = toQuaternion(frameData.RightArm_r),
        //        RightFoot_r     = toQuaternion(frameData.RightFoot_r),
        //        RightForeArm_r  = toQuaternion(frameData.RightForeArm_r),
        //        RightHand_r     = toQuaternion(frameData.RightHand_r),
        //        RightLeg_r      = toQuaternion(frameData.RightLeg_r),
        //        RightShoulder_r = toQuaternion(frameData.RightShoulder_r),
        //        RightUpLeg_r    = toQuaternion(frameData.RightUpLeg_r),
        //        Spine1_r        = toQuaternion(frameData.Spine1_r),
        //        Spine2_r        = toQuaternion(frameData.Spine2_r),
        //        Spine_r         = toQuaternion(frameData.Spine_r),
        //        root_r          = toQuaternion(frameData.root_r),

        //        root_t          = rootPosition,
        //        frame = frameData.frame// [0]
        //    };
        //}

        //public float[] GetFaceData()
        //{
        //    return faceData.ToArray();
        //    return new float[]
        //    {
        //        faceData.browDownLeft * 100f,
        //        faceData.browDownRight * 100f,
        //        faceData.browInnerUp * 100f,
        //        faceData.browOuterUpLeft * 100f,
        //        faceData.browOuterUpRight * 100f,
        //        faceData.cheekPuff * 100f,
        //        faceData.cheekSquintLeft * 100f,
        //        faceData.cheekSquintRight * 100f,
        //        faceData.eyeBlinkLeft * 100f,
        //        faceData.eyeBlinkRight * 100f,
        //        faceData.eyeLookDownLeft * 100f,
        //        faceData.eyeLookDownRight * 100f,
        //        faceData.eyeLookInLeft * 100f,
        //        faceData.eyeLookInRight * 100f,
        //        faceData.eyeLookOutLeft * 100f,
        //        faceData.eyeLookOutRight * 100f,
        //        faceData.eyeLookUpLeft * 100f,
        //        faceData.eyeLookUpRight * 100f,
        //        faceData.eyeSquintLeft * 100f,
        //        faceData.eyeSquintRight * 100f,
        //        faceData.eyeWideLeft * 100f,
        //        faceData.eyeWideRight * 100f,
        //        faceData.jawForward * 100f,
        //        faceData.jawLeft * 100f,
        //        faceData.jawOpen * 100f,
        //        faceData.jawRight * 100f,
        //        faceData.mouthClose * 100f,
        //        faceData.mouthDimpleLeft * 100f,
        //        faceData.mouthDimpleRight * 100f,
        //        faceData.mouthFrownLeft * 100f,
        //        faceData.mouthFrownRight * 100f,
        //        faceData.mouthFunnel * 100f,
        //        faceData.mouthLeft * 100f,
        //        faceData.mouthLowerDownLeft * 100f,
        //        faceData.mouthLowerDownRight * 100f,
        //        faceData.mouthPressLeft * 100f,
        //        faceData.mouthPressRight * 100f,
        //        faceData.mouthPucker * 100f,
        //        faceData.mouthRight * 100f,
        //        faceData.mouthRollLower * 100f,
        //        faceData.mouthRollUpper * 100f,
        //        faceData.mouthShrugLower * 100f,
        //        faceData.mouthShrugUpper * 100f,
        //        faceData.mouthSmileLeft * 100f,
        //        faceData.mouthSmileRight * 100f,
        //        faceData.mouthStretchLeft * 100f,
        //        faceData.mouthStretchRight * 100f,
        //        faceData.mouthUpperUpLeft * 100f,
        //        faceData.mouthUpperUpRight * 100f,
        //        faceData.noseSneerLeft * 100f,
        //        faceData.noseSneerRight * 100f,
        //        faceData.tongueOut * 100f
        //    };
        //}

        public Quaternion GetBoneRotation(string boneName)
        {
            return boneName switch
            {
                "Head" => Head_r,
                "LeftArm" => LeftArm_r,
                "LeftFoot" => LeftFoot_r,
                "LeftForeArm" => LeftForeArm_r,
                "LeftHand" => LeftHand_r,

                "LeftHandIndex1" => LeftHandIndex1_r,
                "LeftHandIndex2" => LeftHandIndex2_r,
                "LeftHandIndex3" => LeftHandIndex3_r,
                "LeftHandMiddle1" => LeftHandMiddle1_r,
                "LeftHandMiddle2" => LeftHandMiddle2_r,
                "LeftHandMiddle3" => LeftHandMiddle3_r,
                "LeftHandRing1" => LeftHandRing1_r,
                "LeftHandRing2" => LeftHandRing2_r,
                "LeftHandRing3" => LeftHandRing3_r,
                "LeftHandThumb1" => LeftHandThumb1_r,
                "LeftHandThumb2" => LeftHandThumb2_r,
                "LeftHandThumb3" => LeftHandThumb3_r,
                "LeftHandPinky1" => LeftHandPinky1_r,
                "LeftHandPinky2" => LeftHandPinky2_r,
                "LeftHandPinky3" => LeftHandPinky3_r,
                "LeftLeg" => LeftLeg_r,
                "LeftShoulder" => LeftShoulder_r,
                "LeftUpLeg" => LeftUpLeg_r,
                "Neck" => Neck_r,
                "RightArm" => RightArm_r,
                "RightFoot" => RightFoot_r,
                "RightForeArm" => RightForeArm_r,
                "RightHand" => RightHand_r,
                "RightHandIndex1" => RightHandIndex1_r,
                "RightHandIndex2" => RightHandIndex2_r,
                "RightHandIndex3" => RightHandIndex3_r,
                "RightHandMiddle1" => RightHandMiddle1_r,
                "RightHandMiddle2" => RightHandMiddle2_r,
                "RightHandMiddle3" => RightHandMiddle3_r,
                "RightHandRing1" => RightHandRing1_r,
                "RightHandRing2" => RightHandRing2_r,
                "RightHandRing3" => RightHandRing3_r,
                "RightHandThumb1" => RightHandThumb1_r,
                "RightHandThumb2" => RightHandThumb2_r,
                "RightHandThumb3" => RightHandThumb3_r,
                "RightHandPinky1" => RightHandPinky1_r,
                "RightHandPinky2" => RightHandPinky2_r,
                "RightHandPinky3" => RightHandPinky3_r,
                "RightLeg" => RightLeg_r,
                "RightShoulder" => RightShoulder_r,
                "RightUpLeg" => RightUpLeg_r,
                "Spine1" => Spine1_r,
                "Spine2" => Spine2_r,
                "Spine" => Spine_r,
                "Hips" => root_r,

                //case  "root": return root_r;
                _ => throw new System.MissingFieldException("Unknown Bone: " + boneName),
            };
        }

        //Quaternion toQuaternion(float[] rotationValues)
        //{
        //    float x = Mathf.Round(rotationValues[1] * 10000000f) / 10000000f;
        //    float y = -Mathf.Round(rotationValues[2] * 10000000f) / 10000000f;
        //    float z = -Mathf.Round(rotationValues[3] * 10000000f) / 10000000f;
        //    float w = Mathf.Round(rotationValues[0] * 10000000f) / 10000000f;
        //    return new Quaternion(x, y, z, w);
        //}
    }
}