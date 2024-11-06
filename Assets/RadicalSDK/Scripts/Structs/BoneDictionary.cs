using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    public class BoneNaming
    {
        public Dictionary<HumanBodyBones, string> boneMapping { get { return getBoneMapping(); } }

        //public Dictionary<string, Transform> GetBonesAsList(RadicalCharacterAnimator animator)
        //{
        //    return new Dictionary<string, Transform>
        //{
        //    {"Head", animator.Head},
        //    {"Neck", animator.Neck},

        //    {"LeftArm", animator.LeftArm},
        //    {"LeftFoot",animator.LeftFoot },
        //    {"LeftForeArm", animator.LeftForeArm},
        //    {"LeftHand", animator.LeftHand},
        //    {"LeftLeg", animator.LeftLeg},
        //    {"LeftShoulder", animator.LeftShoulder},
        //    {"LeftUpLeg", animator.LeftUpLeg},

        //    {"RightArm", animator.RightArm},
        //    {"RightFoot", animator.RightFoot},
        //    {"RightForeArm", animator.RightForeArm},
        //    {"RightHand", animator.RightHand},
        //    {"RightLeg", animator.RightLeg},
        //    {"RightShoulder", animator.RightShoulder},
        //    {"RightUpLeg", animator.RightUpLeg},
        //    {"Spine1", animator.Spine1},
        //    {"Spine2", animator.Spine2},
        //    {"Spine", animator.Spine},
        //    //{"Root", animator.Root},

        //    //{"NeckDummy", animator.NeckDummy},
        //    //{"RightShoulderDummy", animator.RightShoulderDummy},
        //    //{"RightUpLegDummy", animator.RightUpLegDummy},
        //    //{"LeftShoulderDummy", animator.LeftShoulderDummy},
        //    //{"LeftUpLegDummy", animator.LeftUpLegDummy},
        //    //{"SpineDummy", animator.SpineDummy},
        //};
        //}

        Dictionary<HumanBodyBones, string> getBoneMapping()
        {
            //Hardcoded relation between Unity's HumanBodyBones and RADiCAL naming convention
            return new Dictionary<HumanBodyBones, string>
        {
            //{ HumanBodyBones.Hips, "Root" },

            { HumanBodyBones.LeftUpperLeg, "LeftUpLeg" },
            { HumanBodyBones.LeftLowerLeg, "LeftLeg" },
            { HumanBodyBones.LeftFoot, "LeftFoot" },
            { HumanBodyBones.LeftShoulder, "LeftShoulder" },
            { HumanBodyBones.LeftUpperArm, "LeftArm" },
            { HumanBodyBones.LeftLowerArm, "LeftForeArm" },
            { HumanBodyBones.LeftHand, "LeftHand" },

            { HumanBodyBones.RightUpperLeg, "RightUpLeg" },
            { HumanBodyBones.RightLowerLeg, "RightLeg" },
            { HumanBodyBones.RightFoot, "RightFoot" },
            { HumanBodyBones.RightShoulder, "RightShoulder" },
            { HumanBodyBones.RightUpperArm, "RightArm" },
            { HumanBodyBones.RightLowerArm, "RightForeArm" },
            { HumanBodyBones.RightHand, "RightHand" },

            { HumanBodyBones.Spine, "Spine" },
            { HumanBodyBones.Chest, "Spine1" },
            { HumanBodyBones.UpperChest, "Spine2" },
            { HumanBodyBones.Neck, "Neck" },
            { HumanBodyBones.Head, "Head" }
        };
        }
    }
}