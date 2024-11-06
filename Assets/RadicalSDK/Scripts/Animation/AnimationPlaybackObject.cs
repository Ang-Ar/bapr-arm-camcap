
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    // Store frame data in an array, one pose per frame
    public class AnimationPlaybackObject : ScriptableObject
    {
        public string playerID;
        [HideInInspector] public BakedFrameData[] animation;
        [HideInInspector] public BakedFaceData[] facialAnimation;
        [HideInInspector] public float frameDuration;
        [HideInInspector] public bool hasFace;
        //[HideInInspector] public int startFrame;
    }
}