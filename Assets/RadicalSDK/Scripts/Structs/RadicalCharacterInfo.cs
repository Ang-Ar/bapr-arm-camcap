using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    [Serializable]
    public struct RadicalCharacterInfo
    {
        public string playerID;
        public bool use;
        public GameObject playerPrefab;
        public bool recordAnimation;
        public AnimationClip animationClip;
        [HideInInspector] public GameObject player;

    }
}