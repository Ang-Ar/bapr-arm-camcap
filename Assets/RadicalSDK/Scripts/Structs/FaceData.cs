using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    public struct FaceData
    {
        public float browDownLeft;
        public float browDownRight;
        public float browInnerUp;
        public float browOuterUpLeft;
        public float browOuterUpRight;
        public float cheekPuff;
        public float cheekSquintLeft;
        public float cheekSquintRight;
        public float eyeBlinkLeft;
        public float eyeBlinkRight;
        public float eyeLookDownLeft;
        public float eyeLookDownRight;
        public float eyeLookInLeft;
        public float eyeLookInRight;
        public float eyeLookOutLeft;
        public float eyeLookOutRight;
        public float eyeLookUpLeft;
        public float eyeLookUpRight;
        public float eyeSquintLeft;
        public float eyeSquintRight;
        public float eyeWideLeft;
        public float eyeWideRight;
        public float jawForward;
        public float jawLeft;
        public float jawOpen;
        public float jawRight;
        public float mouthClose;
        public float mouthDimpleLeft;
        public float mouthDimpleRight;
        public float mouthFrownLeft;
        public float mouthFrownRight;
        public float mouthFunnel;
        public float mouthLeft;
        public float mouthLowerDownLeft;
        public float mouthLowerDownRight;
        public float mouthPressLeft;
        public float mouthPressRight;
        public float mouthPucker;
        public float mouthRight;
        public float mouthRollLower;
        public float mouthRollUpper;
        public float mouthShrugLower;
        public float mouthShrugUpper;
        public float mouthSmileLeft;
        public float mouthSmileRight;
        public float mouthStretchLeft;
        public float mouthStretchRight;
        public float mouthUpperUpLeft;
        public float mouthUpperUpRight;
        public float noseSneerLeft;
        public float noseSneerRight;
        public float tongueOut;

        public bool hasBeenProcessed; // Flag to store whether the bundle has been processed
        public int frame;
        public BakedFaceData Bake()
        {
            return new BakedFaceData()
            {
                browDownLeft = Mathf.Round(browDownLeft * 10000) / 100,
                browDownRight = Mathf.Round(browDownRight * 10000) / 100,
                browInnerUp = Mathf.Round(browInnerUp * 10000) / 100,
                browOuterUpLeft = Mathf.Round(browOuterUpLeft * 10000) / 100,
                browOuterUpRight = Mathf.Round(browOuterUpRight * 10000) / 100,
                cheekPuff = Mathf.Round(cheekPuff * 10000) / 100,
                cheekSquintLeft = Mathf.Round(cheekSquintLeft * 10000) / 100,
                cheekSquintRight = Mathf.Round(cheekSquintRight * 10000) / 100,
                eyeBlinkLeft = Mathf.Round(eyeBlinkLeft * 10000) / 100,
                eyeBlinkRight = Mathf.Round(eyeBlinkRight * 10000) / 100,
                eyeLookDownLeft = Mathf.Round(eyeLookDownLeft * 10000) / 100,
                eyeLookDownRight = Mathf.Round(eyeLookDownRight * 10000) / 100,
                eyeLookInLeft = Mathf.Round(eyeLookInLeft * 10000) / 100,
                eyeLookInRight = Mathf.Round(eyeLookInRight * 10000) / 100,
                eyeLookOutLeft = Mathf.Round(eyeLookOutLeft * 10000) / 100,
                eyeLookOutRight = Mathf.Round(eyeLookOutRight * 10000) / 100,
                eyeLookUpLeft = Mathf.Round(eyeLookUpLeft * 10000) / 100,
                eyeLookUpRight = Mathf.Round(eyeLookUpRight * 10000) / 100,
                eyeSquintLeft = Mathf.Round(eyeSquintLeft * 10000) / 100,
                eyeSquintRight = Mathf.Round(eyeSquintRight * 10000) / 100,
                eyeWideLeft = Mathf.Round(eyeWideLeft * 10000) / 100,
                eyeWideRight = Mathf.Round(eyeWideRight * 10000) / 100,
                jawForward = Mathf.Round(jawForward * 10000) / 100,
                jawLeft = Mathf.Round(jawLeft * 10000) / 100,
                jawOpen = Mathf.Round(jawOpen * 10000) / 100,
                jawRight = Mathf.Round(jawRight * 10000) / 100,
                mouthClose = Mathf.Round(mouthClose * 10000) / 100,
                mouthDimpleLeft = Mathf.Round(mouthDimpleLeft * 10000) / 100,
                mouthDimpleRight = Mathf.Round(mouthDimpleRight * 10000) / 100,
                mouthFrownLeft = Mathf.Round(mouthFrownLeft * 10000) / 100,
                mouthFrownRight = Mathf.Round(mouthFrownRight * 10000) / 100,
                mouthFunnel = Mathf.Round(mouthFunnel * 10000) / 100,
                mouthLeft = Mathf.Round(mouthLeft * 10000) / 100,
                mouthLowerDownLeft = Mathf.Round(mouthLowerDownLeft * 10000) / 100,
                mouthLowerDownRight = Mathf.Round(mouthLowerDownRight * 10000) / 100,
                mouthPressLeft = Mathf.Round(mouthPressLeft * 10000) / 100,
                mouthPressRight = Mathf.Round(mouthPressRight * 10000) / 100,
                mouthPucker = Mathf.Round(mouthPucker * 10000) / 100,
                mouthRight = Mathf.Round(mouthRight * 10000) / 100,
                mouthRollLower = Mathf.Round(mouthRollLower * 10000) / 100,
                mouthRollUpper = Mathf.Round(mouthRollUpper * 10000) / 100,
                mouthShrugLower = Mathf.Round(mouthShrugLower * 10000) / 100,
                mouthShrugUpper = Mathf.Round(mouthShrugUpper * 10000) / 100,
                mouthSmileLeft = Mathf.Round(mouthSmileLeft * 10000) / 100,
                mouthSmileRight = Mathf.Round(mouthSmileRight * 10000) / 100,
                mouthStretchLeft = Mathf.Round(mouthStretchLeft * 10000) / 100,
                mouthStretchRight = Mathf.Round(mouthStretchRight * 10000) / 100,
                mouthUpperUpLeft = Mathf.Round(mouthUpperUpLeft * 10000) / 100,
                mouthUpperUpRight = Mathf.Round(mouthUpperUpRight * 10000) / 100,
                noseSneerLeft = Mathf.Round(noseSneerLeft * 10000) / 100,
                noseSneerRight = Mathf.Round(noseSneerRight * 10000) / 100,
                tongueOut = Mathf.Round(tongueOut * 10000) / 100,
                
            };
        }

        public float[] ToArray()
        {
            return new float[]
            {
                browDownLeft * 100f,
                browDownRight * 100f,
                browInnerUp * 100f,
                browOuterUpLeft * 100f,
                browOuterUpRight * 100f,
                cheekPuff * 100f,
                cheekSquintLeft * 100f,
                cheekSquintRight * 100f,
                eyeBlinkLeft * 100f,
                eyeBlinkRight * 100f,
                eyeLookDownLeft * 100f,
                eyeLookDownRight * 100f,
                eyeLookInLeft * 100f,
                eyeLookInRight * 100f,
                eyeLookOutLeft * 100f,
                eyeLookOutRight * 100f,
                eyeLookUpLeft * 100f,
                eyeLookUpRight * 100f,
                eyeSquintLeft * 100f,
                eyeSquintRight * 100f,
                eyeWideLeft * 100f,
                eyeWideRight * 100f,
                jawForward * 100f,
                jawLeft * 100f,
                jawOpen * 100f,
                jawRight * 100f,
                mouthClose * 100f,
                mouthDimpleLeft * 100f,
                mouthDimpleRight * 100f,
                mouthFrownLeft * 100f,
                mouthFrownRight * 100f,
                mouthFunnel * 100f,
                mouthLeft * 100f,
                mouthLowerDownLeft * 100f,
                mouthLowerDownRight * 100f,
                mouthPressLeft * 100f,
                mouthPressRight * 100f,
                mouthPucker * 100f,
                mouthRight * 100f,
                mouthRollLower * 100f,
                mouthRollUpper * 100f,
                mouthShrugLower * 100f,
                mouthShrugUpper * 100f,
                mouthSmileLeft * 100f,
                mouthSmileRight * 100f,
                mouthStretchLeft * 100f,
                mouthStretchRight * 100f,
                mouthUpperUpLeft * 100f,
                mouthUpperUpRight * 100f,
                noseSneerLeft * 100f,
                noseSneerRight * 100f,
                tongueOut * 100f
            };
        }

        public override string ToString()
        {
            if (browDownLeft == 0)
                return "Empty face data";
            return "Smile: " + mouthSmileLeft;
        }
    }
}