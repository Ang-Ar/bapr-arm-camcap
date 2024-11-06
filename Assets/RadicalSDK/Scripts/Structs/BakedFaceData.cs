using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    [System.Serializable]
    public struct BakedFaceData
    {
        public float browDownLeft,
                browDownRight,
                browInnerUp,
                browOuterUpLeft,
                browOuterUpRight,
                cheekPuff,
                cheekSquintLeft,
                cheekSquintRight,
                eyeBlinkLeft,
                eyeBlinkRight,
                eyeLookDownLeft,
                eyeLookDownRight,
                eyeLookInLeft,
                eyeLookInRight,
                eyeLookOutLeft,
                eyeLookOutRight,
                eyeLookUpLeft,
                eyeLookUpRight,
                eyeSquintLeft,
                eyeSquintRight,
                eyeWideLeft,
                eyeWideRight,
                jawForward,
                jawLeft,
                jawOpen,
                jawRight,
                mouthClose,
                mouthDimpleLeft,
                mouthDimpleRight,
                mouthFrownLeft,
                mouthFrownRight,
                mouthFunnel,
                mouthLeft,
                mouthLowerDownLeft,
                mouthLowerDownRight,
                mouthPressLeft,
                mouthPressRight,
                mouthPucker,
                mouthRight,
                mouthRollLower,
                mouthRollUpper,
                mouthShrugLower,
                mouthShrugUpper,
                mouthSmileLeft,
                mouthSmileRight,
                mouthStretchLeft,
                mouthStretchRight,
                mouthUpperUpLeft,
                mouthUpperUpRight,
                noseSneerLeft,
                noseSneerRight,
                tongueOut;
        public int frame;
        public BakedFaceData(FaceData faceData)
        {
            browDownLeft        = faceData.browDownLeft;
            browDownRight       = faceData.browDownRight;
            browInnerUp         = faceData.browInnerUp;
            browOuterUpLeft     = faceData.browOuterUpLeft;
            browOuterUpRight    = faceData.browOuterUpRight;
            cheekPuff           = faceData.cheekPuff;
            cheekSquintLeft     = faceData.cheekSquintLeft;
            cheekSquintRight    = faceData.cheekSquintRight;
            eyeBlinkLeft        = faceData.eyeBlinkLeft;
            eyeBlinkRight       = faceData.eyeBlinkRight;
            eyeLookDownLeft     = faceData.eyeLookDownLeft;
            eyeLookDownRight    = faceData.eyeLookDownRight;
            eyeLookInLeft       = faceData.eyeLookInLeft;
            eyeLookInRight      = faceData.eyeLookInRight;
            eyeLookOutLeft      = faceData.eyeLookOutLeft;
            eyeLookOutRight     = faceData.eyeLookOutRight;
            eyeLookUpLeft       = faceData.eyeLookUpLeft;
            eyeLookUpRight      = faceData.eyeLookUpRight;
            eyeSquintLeft       = faceData.eyeSquintLeft;
            eyeSquintRight      = faceData.eyeSquintRight;
            eyeWideLeft         = faceData.eyeWideLeft;
            eyeWideRight        = faceData.eyeWideRight;
            jawForward          = faceData.jawForward;
            jawLeft             = faceData.jawLeft;
            jawOpen             = faceData.jawOpen;
            jawRight            = faceData.jawRight;
            mouthClose          = faceData.mouthClose;
            mouthDimpleLeft     = faceData.mouthDimpleLeft;
            mouthDimpleRight    = faceData.mouthDimpleRight;
            mouthFrownLeft      = faceData.mouthFrownLeft;
            mouthFrownRight     = faceData.mouthFrownRight;
            mouthFunnel         = faceData.mouthFunnel;
            mouthLeft           = faceData.mouthLeft;
            mouthLowerDownLeft  = faceData.mouthLowerDownLeft;
            mouthLowerDownRight = faceData.mouthLowerDownRight;
            mouthPressLeft      = faceData.mouthPressLeft;
            mouthPressRight     = faceData.mouthPressRight;
            mouthPucker         = faceData.mouthPucker;
            mouthRight          = faceData.mouthRight;
            mouthRollLower      = faceData.mouthRollLower;
            mouthRollUpper      = faceData.mouthRollUpper;
            mouthShrugLower     = faceData.mouthShrugLower;
            mouthShrugUpper     = faceData.mouthShrugUpper;
            mouthSmileLeft      = faceData.mouthSmileLeft;
            mouthSmileRight     = faceData.mouthSmileRight;
            mouthStretchLeft    = faceData.mouthStretchLeft;
            mouthStretchRight   = faceData.mouthStretchRight;
            mouthUpperUpLeft    = faceData.mouthUpperUpLeft;
            mouthUpperUpRight   = faceData.mouthUpperUpRight;
            noseSneerLeft       = faceData.noseSneerLeft;
            noseSneerRight      = faceData.noseSneerRight;
            tongueOut           = faceData.tongueOut;
            frame               = faceData.frame;
        }

        public float[] ToArray()
        {
            return new float[]
            {
                browDownLeft,
                browDownRight,
                browInnerUp,
                browOuterUpLeft,
                browOuterUpRight,
                cheekPuff,
                cheekSquintLeft,
                cheekSquintRight,
                eyeBlinkLeft,
                eyeBlinkRight,
                eyeLookDownLeft,
                eyeLookDownRight,
                eyeLookInLeft,
                eyeLookInRight,
                eyeLookOutLeft,
                eyeLookOutRight,
                eyeLookUpLeft,
                eyeLookUpRight,
                eyeSquintLeft,
                eyeSquintRight,
                eyeWideLeft,
                eyeWideRight,
                jawForward,
                jawLeft,
                jawOpen,
                jawRight,
                mouthClose,
                mouthDimpleLeft,
                mouthDimpleRight,
                mouthFrownLeft,
                mouthFrownRight,
                mouthFunnel,
                mouthLeft,
                mouthLowerDownLeft,
                mouthLowerDownRight,
                mouthPressLeft,
                mouthPressRight,
                mouthPucker,
                mouthRight,
                mouthRollLower,
                mouthRollUpper,
                mouthShrugLower,
                mouthShrugUpper,
                mouthSmileLeft,
                mouthSmileRight,
                mouthStretchLeft,
                mouthStretchRight,
                mouthUpperUpLeft,
                mouthUpperUpRight,
                noseSneerLeft,
                noseSneerRight,
                tongueOut
            };
        }

        public float GetBlendShapeValue(string name)
        {
            float blendShapeValue;
            switch (name)
            {
                case "browDownLeft":        blendShapeValue = browDownLeft; break;
                case "browDownRight":       blendShapeValue = browDownRight; break;
                case "browInnerUp":         blendShapeValue = browInnerUp; break;
                case "browOuterUpLeft":     blendShapeValue = browOuterUpLeft; break;
                case "browOuterUpRight":    blendShapeValue = browOuterUpRight; break;
                case "cheekPuff":           blendShapeValue = cheekPuff; break;
                case "cheekSquintLeft":     blendShapeValue = cheekSquintLeft; break;
                case "cheekSquintRight":    blendShapeValue = cheekSquintRight; break;
                case "eyeBlinkLeft":        blendShapeValue = eyeBlinkLeft; break;
                case "eyeBlinkRight":       blendShapeValue = eyeBlinkRight; break;
                case "eyeLookDownLeft":     blendShapeValue = eyeLookDownLeft; break;
                case "eyeLookDownRight":    blendShapeValue = eyeLookDownRight; break;
                case "eyeLookInLeft":       blendShapeValue = eyeLookInLeft; break;
                case "eyeLookInRight":      blendShapeValue = eyeLookInRight; break;
                case "eyeLookOutLeft":      blendShapeValue = eyeLookOutLeft; break;
                case "eyeLookOutRight":     blendShapeValue = eyeLookOutRight; break;
                case "eyeLookUpLeft":       blendShapeValue = eyeLookUpLeft; break;
                case "eyeLookUpRight":      blendShapeValue = eyeLookUpRight; break;
                case "eyeSquintLeft":       blendShapeValue = eyeSquintLeft; break;
                case "eyeSquintRight":      blendShapeValue = eyeSquintRight; break;
                case "eyeWideLeft":         blendShapeValue = eyeWideLeft; break;
                case "eyeWideRight":        blendShapeValue = eyeWideRight; break;
                case "jawForward":          blendShapeValue = jawForward; break;
                case "jawLeft":             blendShapeValue = jawLeft; break;
                case "jawOpen":             blendShapeValue = jawOpen; break;
                case "jawRight":            blendShapeValue = jawRight; break;
                case "mouthClose":          blendShapeValue = mouthClose; break;
                case "mouthDimpleLeft":     blendShapeValue = mouthDimpleLeft; break;
                case "mouthDimpleRight":    blendShapeValue = mouthDimpleRight; break;
                case "mouthFrownLeft":      blendShapeValue = mouthFrownLeft; break;
                case "mouthFrownRight":     blendShapeValue = mouthFrownRight; break;
                case "mouthFunnel":         blendShapeValue = mouthFunnel; break;
                case "mouthLeft":           blendShapeValue = mouthLeft; break;
                case "mouthLowerDownLeft":  blendShapeValue = mouthLowerDownLeft; break;
                case "mouthLowerDownRight": blendShapeValue = mouthLowerDownRight; break;
                case "mouthPressLeft":      blendShapeValue = mouthPressLeft; break;
                case "mouthPressRight":     blendShapeValue = mouthPressRight; break;
                case "mouthPucker":         blendShapeValue = mouthPucker; break;
                case "mouthRight":          blendShapeValue = mouthRight; break;
                case "mouthRollLower":      blendShapeValue = mouthRollLower; break;
                case "mouthRollUpper":      blendShapeValue = mouthRollUpper; break;
                case "mouthShrugLower":     blendShapeValue = mouthShrugLower; break;
                case "mouthShrugUpper":     blendShapeValue = mouthShrugUpper; break;
                case "mouthSmileLeft":      blendShapeValue = mouthSmileLeft; break;
                case "mouthSmileRight":     blendShapeValue = mouthSmileRight; break;
                case "mouthStretchLeft":    blendShapeValue = mouthStretchLeft; break;
                case "mouthStretchRight":   blendShapeValue = mouthStretchRight; break;
                case "mouthUpperUpLeft":    blendShapeValue = mouthUpperUpLeft; break;
                case "mouthUpperUpRight":   blendShapeValue = mouthUpperUpRight; break;
                case "noseSneerLeft":       blendShapeValue = noseSneerLeft; break;
                case "noseSneerRight":      blendShapeValue = noseSneerRight; break;
                case "tongueOut":           blendShapeValue = tongueOut; break;
                default: throw new System.NotImplementedException("Unknown blend shape");
            }
            return blendShapeValue * 100f;
        }
    }
}