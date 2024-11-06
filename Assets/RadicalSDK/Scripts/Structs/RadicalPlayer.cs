using System;
using UnityEngine;


namespace Radical
{

    [Serializable]
    public class RadicalPlayer
    {

        // Custom Configuration
        public string playerID;
        public string userName;
        public GameObject playerPrefab;
        public bool isBaking;
        public bool isPlayer;
        /// <summary>
        /// The instantiated GameObject from the playerPrefab 
        /// </summary>
        [HideInInspector] public RadicalCharacterAnimator player;
        [HideInInspector] public GameObject gameObject;
        //Recording
        
        [HideInInspector] public AnimationClip animationClip;
        [HideInInspector] public bool isRegistered;
        [HideInInspector] public int startFrame; // the timestamp of the frame which the character receives when being instantiated
        [HideInInspector] public Vector3 spawnPosition;
        //public bool characterExists { get { return avatar != null; } }

        public RadicalPlayer(string playerID, string userName)
        {
            this.playerID = playerID;
            this.userName = userName;
            isPlayer = true;
        }

        public RadicalPlayer(JPlayer playerFromList)
        {
            playerID = playerFromList.attendeeId;
            userName = playerFromList.userName;
            isPlayer = playerFromList.isPlayer;
            //this.playerPrefab = prefab;
        }

        public override string ToString()
        {
            return "Player: " + playerID;
        }
    }
}