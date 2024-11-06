using System;
using System.Collections.Generic;
using UnityEngine;

namespace Radical
{
    public class ServerBase : MonoBehaviour
    {
        [Tooltip("This character will be assigned to all players by default, leave empty to us the default radical character.")]
        public GameObject defaultCharacter; // the character that gets assigned if none was chosen
        [HideInInspector] public Dictionary<string, RadicalPlayer> m_Players;
        [HideInInspector] public List<RadicalPlayer> allCharacters; // we need this as a list type for the UI override
        [HideInInspector] public LiveConnector connector;
        public static bool isExportableScene = true; // the UI buttons are necessary only for the editor mode scenes
        public static ServerBase m_instance;

        public virtual void ReadPlayerList(PlayerList playerList)
        {
            throw new NotImplementedException("Don't use Server Base directly");
        }

        public static ServerBase GetServer()
        {
            //print("Returning " + m_instance.GetType());
            return m_instance;
        }

        //public virtual void showMessage(string message, MessageSeverity severity, int time)
        //{
        //    throw new NotImplementedException("Don't use Server Base directly");
        //}
        
        protected virtual void addPlayer(JPlayer player)
        {

        }
        public void ConnectPlayer(string playerData)
        {
            JPlayer player = JsonUtility.FromJson<JPlayer>(playerData);
            addPlayer(player);  
        }

       

        public void DisconnectPlayer(string playerInfo)
        {
            print("Player left: " + playerInfo);
            //UnityEditor.EditorUtility.SetDirty(this);
        }
    }
}
