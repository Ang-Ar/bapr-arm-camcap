using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    /// <summary>
    /// Serializable player list that contains the information relevant to the Unity Plugin
    /// </summary>
    [Serializable]
    public class PlayerList
    {
        public JPlayer[] players;
    }
}
