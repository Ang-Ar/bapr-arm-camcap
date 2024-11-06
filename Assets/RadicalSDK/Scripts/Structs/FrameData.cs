using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    // To be deserialized from frame data
    public struct FrameData
    {
        public float[] Head_r;
        public float[] LeftArm_r;
        public float[] LeftFoot_r;
        public float[] LeftForeArm_r;
        public float[] LeftHand_r;
        public float[] LeftHandIndex1_r;
        public float[] LeftHandIndex2_r;
        public float[] LeftHandIndex3_r;
        public float[] LeftHandMiddle1_r;
        public float[] LeftHandMiddle2_r;
        public float[] LeftHandMiddle3_r;
        public float[] LeftHandRing1_r;
        public float[] LeftHandRing2_r;
        public float[] LeftHandRing3_r;
        public float[] LeftHandPinky1_r;
        public float[] LeftHandPinky2_r;
        public float[] LeftHandPinky3_r;
        public float[] LeftHandThumb1_r;
        public float[] LeftHandThumb2_r;
        public float[] LeftHandThumb3_r;
        public float[] LeftLeg_r;
        public float[] LeftShoulderDummy_r;
        public float[] LeftShoulder_r;
        public float[] LeftUpLegDummy_r;
        public float[] LeftUpLeg_r;
        public float[] NeckDummy_r;
        public float[] Neck_r;
        public float[] RightArm_r;
        public float[] RightFoot_r;
        public float[] RightForeArm_r;
        public float[] RightHand_r;
        public float[] RightHandIndex1_r;
        public float[] RightHandIndex2_r;
        public float[] RightHandIndex3_r;
        public float[] RightHandMiddle1_r;
        public float[] RightHandMiddle2_r;
        public float[] RightHandMiddle3_r;
        public float[] RightHandRing1_r;
        public float[] RightHandRing2_r;
        public float[] RightHandRing3_r;
        public float[] RightHandPinky1_r;
        public float[] RightHandPinky2_r;
        public float[] RightHandPinky3_r;
        public float[] RightHandThumb1_r;
        public float[] RightHandThumb2_r;
        public float[] RightHandThumb3_r;
        public float[] RightLeg_r;
        public float[] RightShoulderDummy_r;
        public float[] RightShoulder_r;
        public float[] RightUpLegDummy_r;
        public float[] RightUpLeg_r;
        public float[] Spine1_r;
        public float[] Spine2_r;
        public float[] SpineDummy_r;
        public float[] Spine_r;
        public float[] root_r;

        public float[] root_t;

        public string attendeeId;
        public int frame;

        public bool hasBeenProcessed; // Flag to store whether the bundle has been processed
        public BakedFrameData Bake()
        {
            float[] p = root_t;
            //Vector3 rootPosition = new Vector3(-p[0], p[1], p[2]); //TODO: Replace next line
            Vector3 rootPosition = new Vector3(p[0], p[1], p[2]);
            return new BakedFrameData //FIXME: We are converting to quaternion twice, might be faster to read the bake info right before the bone is rotated
            {
                Head_r = toQuaternion(Head_r),
                LeftArm_r = toQuaternion(LeftArm_r),
                LeftFoot_r = toQuaternion(LeftFoot_r),
                LeftForeArm_r = toQuaternion(LeftForeArm_r),
                LeftHand_r = toQuaternion(LeftHand_r),

                LeftHandThumb1_r = toQuaternion(RightHandThumb1_r),
                LeftHandThumb2_r = toQuaternion(LeftHandThumb2_r),
                LeftHandThumb3_r = toQuaternion(LeftHandThumb3_r),

                LeftHandIndex1_r = toQuaternion(LeftHandIndex1_r),
                LeftHandIndex2_r = toQuaternion(LeftHandIndex2_r),
                LeftHandIndex3_r = toQuaternion(LeftHandIndex3_r),

                LeftHandMiddle1_r = toQuaternion(LeftHandMiddle1_r),
                LeftHandMiddle2_r = toQuaternion(LeftHandMiddle2_r),
                LeftHandMiddle3_r = toQuaternion(LeftHandMiddle3_r),

                LeftHandRing1_r = toQuaternion(LeftHandRing1_r),
                LeftHandRing2_r = toQuaternion(LeftHandRing2_r),
                LeftHandRing3_r = toQuaternion(LeftHandRing3_r),

                LeftHandPinky1_r = toQuaternion(LeftHandPinky1_r),
                LeftHandPinky2_r = toQuaternion(LeftHandPinky2_r),
                LeftHandPinky3_r = toQuaternion(LeftHandPinky3_r),

                LeftLeg_r = toQuaternion(LeftLeg_r),
                LeftShoulder_r = toQuaternion(LeftShoulder_r),
                LeftUpLeg_r = toQuaternion(LeftUpLeg_r),
                Neck_r = toQuaternion(Neck_r),
                RightArm_r = toQuaternion(RightArm_r),
                RightFoot_r = toQuaternion(RightFoot_r),
                RightForeArm_r = toQuaternion(RightForeArm_r),
                
                RightHand_r = toQuaternion(RightHand_r),
                RightHandThumb1_r = toQuaternion(RightHandThumb1_r),
                RightHandThumb2_r = toQuaternion(RightHandThumb2_r),
                RightHandThumb3_r = toQuaternion(RightHandThumb3_r),

                RightHandIndex1_r = toQuaternion(RightHandIndex1_r),
                RightHandIndex2_r = toQuaternion(RightHandIndex2_r),
                RightHandIndex3_r = toQuaternion(RightHandIndex3_r),

                RightHandMiddle1_r = toQuaternion(RightHandMiddle1_r),
                RightHandMiddle2_r = toQuaternion(RightHandMiddle2_r),
                RightHandMiddle3_r = toQuaternion(RightHandMiddle3_r),

                RightHandRing1_r = toQuaternion(RightHandRing1_r),
                RightHandRing2_r = toQuaternion(RightHandRing2_r),
                RightHandRing3_r = toQuaternion(RightHandRing3_r),

                RightHandPinky1_r = toQuaternion(RightHandPinky1_r),
                RightHandPinky2_r = toQuaternion(RightHandPinky2_r),
                RightHandPinky3_r = toQuaternion(RightHandPinky3_r),

                RightLeg_r = toQuaternion(RightLeg_r),
                RightShoulder_r = toQuaternion(RightShoulder_r),
                RightUpLeg_r = toQuaternion(RightUpLeg_r),
                Spine1_r = toQuaternion(Spine1_r),
                Spine2_r = toQuaternion(Spine2_r),
                Spine_r = toQuaternion(Spine_r),
                root_r = toQuaternion(root_r),

                root_t = rootPosition,
                frame = frame// [0]
            };
        }
        public float[] GetBoneRotation(string boneName)
        {
            return boneName switch
            {
                "Head" => Head_r,
                "LeftArm" => LeftArm_r,
                "LeftFoot" => LeftFoot_r,
                "LeftForeArm" => LeftForeArm_r,

                "LeftHand" => LeftHand_r,
                "LeftHandThumb1" => LeftHandThumb1_r,
                "LeftHandThumb2" => LeftHandThumb2_r,
                "LeftHandThumb3" => LeftHandThumb3_r,
                "LeftHandIndex1" => LeftHandIndex1_r,
                "LeftHandIndex2" => LeftHandIndex2_r,
                "LeftHandIndex3" => LeftHandIndex3_r,
                "LeftHandMiddle1" => LeftHandMiddle1_r,
                "LeftHandMiddle2" => LeftHandMiddle2_r,
                "LeftHandMiddle3" => LeftHandMiddle3_r,
                "LeftHandRing1" => LeftHandRing1_r,
                "LeftHandRing2" => LeftHandRing2_r,
                "LeftHandRing3" => LeftHandRing3_r,
                "LeftHandPinky1" => LeftHandPinky1_r,
                "LeftHandPinky2" => LeftHandPinky2_r,
                "LeftHandPinky3" => LeftHandPinky3_r,
            
                "LeftLeg" => LeftLeg_r,
                "LeftShoulderDummy" => LeftShoulderDummy_r,
                "LeftShoulder" => LeftShoulder_r,
                "LeftUpLegDummy" => LeftUpLegDummy_r,
                "LeftUpLeg" => LeftUpLeg_r,
                "NeckDummy" => NeckDummy_r,
                "Neck" => Neck_r,
                "RightArm" => RightArm_r,
                "RightFoot" => RightFoot_r,
                "RightForeArm" => RightForeArm_r,

                "RightHand" => RightHand_r,
                "RightHandThumb1" => RightHandThumb1_r,
                "RightHandThumb2" => RightHandThumb2_r,
                "RightHandThumb3" => RightHandThumb3_r,
                "RightHandIndex1" => RightHandIndex1_r,
                "RightHandIndex2" => RightHandIndex2_r,
                "RightHandIndex3" => RightHandIndex3_r,
                "RightHandMiddle1" => RightHandMiddle1_r,
                "RightHandMiddle2" => RightHandMiddle2_r,
                "RightHandMiddle3" => RightHandMiddle3_r,
                "RightHandRing1" => RightHandRing1_r,
                "RightHandRing2" => RightHandRing2_r,
                "RightHandRing3" => RightHandRing3_r,
                "RightHandPinky1" => RightHandPinky1_r,
                "RightHandPinky2" => RightHandPinky2_r,
                "RightHandPinky3" => RightHandPinky3_r,

                "RightLeg" => RightLeg_r,
                "RightShoulderDummy" => RightShoulderDummy_r,
                "RightShoulder" => RightShoulder_r,
                "RightUpLegDummy" => RightUpLegDummy_r,
                "RightUpLeg" => RightUpLeg_r,
                "Spine1" => Spine1_r,
                "Spine2" => Spine2_r,
                "SpineDummy" => SpineDummy_r,
                "Spine" => Spine_r,
                "Hips" => root_r,
                //case  "root": return root_r;
                _ => throw new System.MissingFieldException("Unknown Bone: " + boneName),
            };
        }

        public override string ToString()
        {
            if (root_r == null)
                return "Empty frame data";
            return $"frame:{frame}, root: {root_r[0]}, {root_t[0]}";
        }
        Quaternion toQuaternion(float[] rotationValues)
        {
            float precision = 1000000f;
            float x = Mathf.Round(rotationValues[1] * precision) / precision;
            float y = -Mathf.Round(rotationValues[2] * precision) / precision;
            float z = -Mathf.Round(rotationValues[3] * precision) / precision;
            float w = Mathf.Round(rotationValues[0] * precision) / precision;
            return new Quaternion(x, y, z, w);
        }
    }
}


//public struct FrameData
//{
//    public Quaternion Head_r;
//    public Quaternion LeftArm_r;
//    public Quaternion LeftFoot_r;
//    public Quaternion LeftForeArm_r;
//    public Quaternion LeftHand_r;
//    public Quaternion LeftLeg_r;
//    public Quaternion LeftShoulder_r;
//    public Quaternion LeftUpLeg_r;
//    public Quaternion NeckDummy_r;
//    public Quaternion Neck_r;
//    public Quaternion RightArm_r;
//    public Quaternion RightFoot_r;
//    public Quaternion RightForeArm_r;
//    public Quaternion RightHand_r;
//    public Quaternion RightLeg_r;
//    public Quaternion RightShoulder_r;
//    public Quaternion RightUpLeg_r;
//    public Quaternion Spine1_r;
//    public Quaternion Spine2_r;
//    public Quaternion Spine_r;
//    public Quaternion root_r;

//    public Vector3 root_t;

//    public string attendeeId;
//    public float[] timestamp;

    //Dummies (don't need them at all?)
    //public Quaternion SpineDummy_r;
    //public Quaternion RightUpLegDummy_r;
    //public Quaternion LeftShoulderDummy_r;
    //public Quaternion LeftUpLegDummy_r;
    //public Quaternion RightShoulderDummy_r;
//}

