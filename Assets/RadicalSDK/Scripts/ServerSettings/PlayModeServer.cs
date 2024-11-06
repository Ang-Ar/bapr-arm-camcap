using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Radical
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [ExecuteInEditMode]
    public class PlayModeServer : ServerBase
    {
        [Tooltip("Drag as many humanoid rigged characters into this list as you want. After connecting to the server, you will be able to pick the active characters from this list. If left empty, you will only be able to pick the default character.")]
        public List<GameObject> characterPool;

        private void Awake()
        {
            m_instance = this;
        }

        protected override void addPlayer(JPlayer player)
        {
            GetComponent<UIManager>().ReOpenMenu();
        }

        public List<GameObject> GetCharacterPool()
        {
            List<GameObject> response = new List<GameObject>() { defaultCharacter };
            response.AddRange(characterPool);
            return response;
        }

        public void AssignCharacter(int index, string name)
        {
            if (!m_Players.ContainsKey(name))
            {
                print($"Character {name} was not in dict, this shouldn't happen");
                return;
            }
            m_Players[name].playerPrefab = characterPool[index];
        }
        public override void ReadPlayerList(PlayerList incomingPlayers)
        {
            //TODO: Fails if list is empty (not possible, right?)
            // the initial connection to the server triggers receiving a list of players that are currently connected to the stream
            //string namedList = CrudeJson.ToNamedList(list, "players");
            //print("Formatted: " + namedList);
            //PlayerList incomingPlayers = JsonUtility.FromJson<PlayerList>(namedList); //All players in the initial attendee-list
            if (incomingPlayers == null || incomingPlayers.players == null) 
            {
                print("Player list was empty");
                return;
            }
            allCharacters = new List<RadicalPlayer>();
            m_Players = new Dictionary<string, RadicalPlayer>();
            int length = incomingPlayers.players.Length;
            for (int i = 0; i < length; i++)
            {
                JPlayer incomingPlayer = incomingPlayers.players[i];
                addPlayer(incomingPlayer);
            }
        }

    }
}