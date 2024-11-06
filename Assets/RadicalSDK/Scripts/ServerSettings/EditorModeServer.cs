using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Radical
{

#if UNITY_EDITOR
    [InitializeOnLoad]
    [ExecuteInEditMode]
#endif  

    public class EditorModeServer : ServerBase 
    {
        private void Awake()
        {
            m_instance = this;

            //EditorSceneManager.sceneOpened += OnOpenSceneEditor; //TODO: We don't need the EditorSceneManager functions anymore?
#if UNITY_EDITOR
            EditorSceneManager.activeSceneChanged += setInstance;
#endif
        }

        void Start()
        {
            connector = GetComponent<LiveConnector>();
            if (defaultCharacter != null)
            {
                GetComponent<RadicalPlayerManager>().SetDefaultCharacter(defaultCharacter);
            }
        }

        //TODO: Clear player list on reloading scene


        public override void ReadPlayerList(PlayerList incomingPlayers)
        {
            //TODO: Fails if list is empty (not possible, right?)
            //if (Application.isPlaying) return;
            // the initial connection to the server triggers receiving a list of players that are currently connected to the stream
            //string namedList = CrudeJson.ToNamedList(list, "players");
            //print("Formatted: " + namedList);
            //PlayerList incomingPlayers = JsonUtility.FromJson<PlayerList>(namedList); //All players in the initial attendee-list
            //print("Reading player list");
            if (!Application.isPlaying) // received a new list by fetching, overwrite the old one, if exists
                allCharacters = new List<RadicalPlayer>();
            m_Players = new Dictionary<string, RadicalPlayer>();

            if (Application.isPlaying)
                GetComponent<RadicalPlayerManager>().CreatePlayers(incomingPlayers);
            else
            {
                //Add the players in the inspector
                int length = incomingPlayers.players.Length;
                for (int i = 0; i < length; i++)
                {
                    JPlayer incomingPlayer = incomingPlayers.players[i];
                    //print("userName: " + incomingPlayer.userName);
                    addPlayer(incomingPlayer);
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        protected override void addPlayer(JPlayer player)
        {
            if (!player.isPlayer) return;

            string playerID = player.attendeeId;
            
            if (!m_Players.ContainsKey(playerID))
            {
                string attendee = player.attendeeId;
                int startIndex = Mathf.Max(attendee.Length - 6, 0);
                string id = player.attendeeId.Substring(startIndex);
                string userName = $"{player.userName} ({id})"; //TODO: Store pure usernames, so we can recall their settings
                RadicalPlayer newPlayer = new RadicalPlayer(playerID, userName);
                GameObject prefab = defaultCharacter == null 
                    ? GetComponent<RadicalPlayerManager>().radicalCharacterPrefab 
                    : defaultCharacter;
                
                newPlayer.playerPrefab = prefab;
                m_Players.Add(playerID, newPlayer);
                allCharacters.Add(newPlayer);
                //print("Added at runtime" + player.userName);
            }
        }

        public void ReadPlayerList(string playerList)
        {
            //TODO: Fails if list is empty (not possible, right?)
            if (Application.isPlaying) return;
            // the initial connection to the server triggers receiving a list of players that are currently connected to the stream
            string namedList = CrudeJson.ToNamedList(playerList, "players");
            //print("Formatted: " + namedList);
            PlayerList incomingPlayers = JsonUtility.FromJson<PlayerList>(namedList); //All players in the initial attendee-list
            ReadPlayerList(incomingPlayers);
        }

        public void ConnectNewPlayer(string playerInfo)
        {
            //print("New player: " + playerInfo);
            JPlayer player = JsonUtility.FromJson<JPlayer>(playerInfo);
            addPlayer(player);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        #region Editor UI

        //[HideInInspector]
        public static string message;
        //[HideInInspector]
        public static bool isFetchingPlayers;
        //[HideInInspector]
        public static MessagePriority currentPriority;
        //[HideInInspector] 
        public static MessageSeverity messageSeverity;
        public bool isConnected { get { return getIsConnected(); } }

        private void OnEnable() //this gets called on scene rebuild as well
        {
            m_instance = this;
        }

        public void ClearPlayerList()
        {
            m_Players?.Clear();
            allCharacters?.Clear();
            //EditorUtility.SetDirty(this);
        }

        private void setInstance(Scene arg0, Scene arg1)
        {
            //print("Scene Changed");
            m_instance = this;
        }

        private bool getIsConnected()
        {
            if (connector == null)
            {
                connector = GetComponent<LiveConnector>();
            }
            return connector.isConnected;
        }
        
        async void ShowMessageTimer(int delay)
        {
            if (delay < 0) //show until next message replaces the content
                return;
            await Task.Delay(delay);
            currentPriority = MessagePriority.None;
        }

        public void ShowErrorMessage(string _message, MessagePriority priority)
        {
            //message = "Sorry, we can’t give you access for one of three reasons:\n\n(1) check whether the Live room has external streaming permissions - streaming is available only if the room owner has a Professional account\n\n(2) check whether you’ve added the correct Room ID\n\n(3) check whether you’ve added the correct Account Key - you can find yours through Settings on our website.";// _message;
            if (priority > currentPriority)
            {
                message = _message;
                currentPriority = priority;
                ShowMessageTimer(10000);
            }
            else
                print("Additional Error:" + _message);
            
            messageSeverity = MessageSeverity.Error;
        }


        #endregion
    }
}