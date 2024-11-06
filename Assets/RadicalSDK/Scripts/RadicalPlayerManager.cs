using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Radical
{
    public class RadicalPlayerManager : MonoBehaviour
    {
        [Tooltip("Please do not change this.")]
        public GameObject radicalCharacterPrefab;
        [Tooltip("Please do not change this.")]
        public GameObject radicalCharacterDriver;
        
        Dictionary<string, RadicalCharacterAnimator> m_Characters = new Dictionary<string, RadicalCharacterAnimator>(); // Store characters by user ID
        public static bool isCharacterPresent;
        static RadicalPlayerManager m_instance;
        GameObject defaultCharacter;

        private void Awake()
        {
            Application.targetFrameRate = 30; // Live stream and Playback are set up with 30fps
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                Debug.LogError(m_instance.name + ": There can only be one player manager in the scene!");
            }

            m_Characters = new Dictionary<string, RadicalCharacterAnimator>();
        }

        public void ReceiveFramedata(string message)
        {
            if (!message.Contains("frame_data")) return;
            string attendee = CrudeJson.GetFieldStringValue(message, "attendeeId");
            string framedataField = CrudeJson.GetField(message, "frame_data");
            FrameData frameData = JsonUtility.FromJson<FrameData>(framedataField);
            //print(attendee);

            if (m_Characters.TryGetValue(attendee, out RadicalCharacterAnimator characterAnimator))// ContainsKey(attendee))
            {
                //RadicalCharacterAnimator characterAnimator = m_Characters[attendee];
                int frame = characterAnimator.isBaking ? CrudeJson.GetFieldIntValue(message, "timestamp") : 0;

                frameData.frame = frame;
                FaceData faceData;
                if (message.Contains("frame_data_face"))
                {
                    string faceDataField = CrudeJson.GetField(message, "frame_data_face");
                    //print(faceDataField);
                    faceData = JsonUtility.FromJson<FaceData>(faceDataField);
                    faceData.frame = frame;
                }
                else
                    faceData = new FaceData();
                characterAnimator.AddFrameData(frameData, faceData);
            }
        }

        public void ReceiveFramedata(FrameData frameData, FaceData faceData, string attendee)
        {
            if (m_Characters.TryGetValue(attendee, out RadicalCharacterAnimator characterAnimator))// ContainsKey(attendee))
            {
                //RadicalCharacterAnimator characterAnimator = m_Characters[attendee];
                //print("Found player for prediction result: " + attendee);
                characterAnimator.AddFrameData(frameData, faceData);
            }
            else
                print("Uknown player: " + attendee);
        }

        public void SwitchCharacter(RadicalCharacterAnimator targetCharacter, string playerID)
        {
            if (string.IsNullOrEmpty(playerID))
            {
                foreach (var entry in m_Characters)
                {
                    m_Characters[playerID] = targetCharacter;
                }
            }
            m_Characters[playerID] = targetCharacter;
        }

        public void OnPlayerConnect(string message)
        {
            //TODO: ability to pick Character for newly connected players
            JPlayer playerFromList = JsonUtility.FromJson<JPlayer>(message);
            //print("User name: " + playerFromList.userName);
            if (playerFromList.isPlayer)// || m_Characters.ContainsKey(playerFromList.attendeeId))) // player already registered
                OnPlayerConnect(playerFromList);
            //RadicalPlayer player;
            //Dictionary<string, RadicalPlayer> predefinedPlayers = GetComponent<EditorModeServer>().m_Players;
            //// if the player was not in the dictionary, create a new one with blank settings
            //if (predefinedPlayers == null || !predefinedPlayers.TryGetValue(playerFromList.attendeeId, out player))
            //{
            //    player = new RadicalPlayer(playerFromList);
            //}

            //CreateCharacter(player);
        }

        public void OnPlayerConnect(JPlayer jPlayer)
        {
            if (m_Characters.ContainsKey(jPlayer.attendeeId)) return; // player already registered

            //RadicalPlayer player = new RadicalPlayer(jPlayer);
            Dictionary<string, RadicalPlayer> predefinedPlayers = GetComponent<EditorModeServer>().m_Players;
            // if the player was not in the dictionary, create a new one with blank settings
            if (predefinedPlayers == null || !predefinedPlayers.TryGetValue(jPlayer.attendeeId, out RadicalPlayer player))
            {
                player = new RadicalPlayer(jPlayer); // we're not in 
            }
            CreateCharacter(player);
        }

        public void CreateCharacter(RadicalPlayer player)
        {
            string userID = player.playerID;
            print("Creating User: " + player);
            GameObject prefab = player.playerPrefab;
            
            float x = m_Characters.Count * 1.5f;
            Vector3 position = player.spawnPosition == Vector3.zero ? new Vector3(x, 0, 0) : player.spawnPosition;
            RadicalCharacterAnimator characterAnimator;

            if (prefab == null) // manually assigned player in the editor
            {
                GameObject source = defaultCharacter == null ? radicalCharacterPrefab : defaultCharacter;
                player.playerPrefab = source;
                if (!source.TryGetComponent<RadicalCharacterAnimator>(out _))
                {
                    characterAnimator = createCustomPlayer(ref player, position);
                }
                else
                {
                    GameObject clone = Instantiate(source);
                    clone.name = userID;
                    characterAnimator = clone.GetComponent<RadicalCharacterAnimator>();
                    player.gameObject = clone;
                    clone.transform.position = position;
                }
            }
            else if (!prefab.TryGetComponent<RadicalCharacterAnimator>(out _))
            {
                characterAnimator = createCustomPlayer(ref player, position);
            }

            else
            {
                GameObject clone;
                if (string.IsNullOrEmpty(prefab.scene.name))
                {
                    clone = Instantiate(player.playerPrefab);
                    clone.transform.position = position;
                }
                else
                {
                    clone = prefab;
                }

                clone.name = userID;
                characterAnimator = clone.GetComponent<RadicalCharacterAnimator>();
                player.gameObject = clone;
            }
            //print("Player is baking: " + player.isBaking);
            if (player.isBaking) 
            {
                characterAnimator.StartBaking(player.playerID);
            }
            m_Characters.Add(userID, characterAnimator);
        }

        public static RadicalAnimationDriver CreateAnimationDriver(string playerID, GameObject target)
        {
            // For non-radical humanoid characters we need a dummy armature to convert the pose from the stream to a humanoid pose in Unity
            // called by all the AnimationPlayback instances on load
            GameObject radicalCharacterDriver = m_instance.radicalCharacterDriver;
            GameObject driver = Instantiate(radicalCharacterDriver);
            driver.name = playerID + "_Driver1";
            
            m_instance.deleteMesh(driver);
            
            RadicalAnimationDriver animationDriver = driver.GetComponent<RadicalAnimationDriver>();
            
            return animationDriver;
        }

        public void deleteMesh(GameObject armature)
        {
            // Delete the mesh from the driver, since we only need the transform data, not the deformed mesh
            SkinnedMeshRenderer[] meshes = armature.GetComponentsInChildren<SkinnedMeshRenderer>();
            int length = meshes.Length;
            for (int i = 0; i < length; i++)
            {
                Destroy(meshes[i].gameObject);
            }
        }

        RadicalAnimationDriver createCustomPlayer(ref RadicalPlayer player, Vector3 position)
        {
            // Create a humanoid character driver to convert the baked animation to humanoid poses at runtime
            GameObject character = player.playerPrefab;
            GameObject instance;
            if (string.IsNullOrEmpty(character.scene.name)) // if the character is already in the scene, don't instantiate it
            {
                instance = Instantiate(character);
                instance.transform.position = position;
            }
            else
            {
                //print("Custom character in scene, using: " + character.name);
                instance = character;
            }

            instance.name = player.playerID;
            player.gameObject = instance;
            GameObject driver = Instantiate(radicalCharacterDriver);
            driver.name = character.name + "_Driver";
            RadicalAnimationDriver animationDriver = driver.GetComponent<RadicalAnimationDriver>();
            deleteMesh(driver); // no need for the mesh if the character only drives another avatar, but we cannot delete it in the editor as that breaks the humanoid conversion
            animationDriver.name = player.playerID + "_AnimationDriver"; //TODO: We could probably animate all custom characters with one driver
            
            animationDriver.Init(instance);
            return animationDriver;
        }

        public static Dictionary<string, RadicalCharacterAnimator> GetPlayers()
        {
            return m_instance.m_Characters;
        }

        public static RadicalPlayerManager GetRadicalPlayerManager()
        {
            return m_instance;
        }

        public void ReadPlayerList(string message)
        {
            // the initial connection to the server triggers receiving a list of players that are currently connected to the stream
            string namedList = CrudeJson.ToNamedList(message, "players");
            PlayerList incomingPlayers = JsonUtility.FromJson<PlayerList>(namedList);
            if (TryGetComponent(out UIManager uiManager))
            {
                uiManager.PopulateCharacterScrollViews(incomingPlayers);
            }
            
            else
                GetComponent<ServerBase>().ReadPlayerList(incomingPlayers);
        }
        /// <summary>
        /// Called by the start button
        /// </summary>
        /// <param name="incomingPlayers"></param>
        /// <param name="droppedCharacters">List of prefabs that have been assigned per drag and drop</param>
        public void CreatePlayers(PlayerList incomingPlayers, List<DropCharacter> droppedCharacters)
        {
            JPlayer[] registeredPlayers = incomingPlayers.players;
            int length = registeredPlayers.Length;
            int arrayPosition = 0;
            for (int i = 0; i < length; i++)
            {
                //print($"{i}: Is player {registeredPlayers[i].userName}: {registeredPlayers[i].isPlayer}");
                if (!registeredPlayers[i].isPlayer) continue;
                RadicalPlayer player = new RadicalPlayer(registeredPlayers[i]);
                DropCharacter dc = droppedCharacters[arrayPosition];
                player.spawnPosition = dc.spawnPosition;
                player.playerPrefab = dc.selectedPrefab;
                CreateCharacter(player);
                arrayPosition++;
            }
        }

        public void CreatePlayers(PlayerList incomingPlayers)
        {
            List<RadicalPlayer> playerSettings = GetComponent<ServerBase>().allCharacters;
            Dictionary<string, RadicalPlayer> predefinedPlayers = toDict(playerSettings);
            int length = incomingPlayers.players.Length;
            for (int i = 0; i < length; i++)
            {
                JPlayer incomingPlayer = incomingPlayers.players[i];
                if (!incomingPlayer.isPlayer) continue;
                RadicalPlayer player;
                if (!predefinedPlayers.TryGetValue(incomingPlayer.attendeeId, out player))
                {
                    player = new RadicalPlayer(incomingPlayer);
                }
                CreateCharacter(player);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void SetDefaultCharacter(GameObject defaultCharacter)
        {
            if (defaultCharacter == null)
                this.defaultCharacter = radicalCharacterPrefab;
            else
                this.defaultCharacter = defaultCharacter;
        }

        Dictionary<string, RadicalPlayer> toDict(List<RadicalPlayer> players)
        {
            // we cannot manage the player list as a dictionary, bc. Unity cannot serialize it (show it in the inspector)
            // so we make a dict from the list to quickly check if a player has been configured or not
            Dictionary<string, RadicalPlayer> result = new Dictionary<string, RadicalPlayer>();
            int length = players.Count;
            if (players == null || length == 0)
            {
                return result;
            }
            for (int i = 0; i < length; i++)
            {
                RadicalPlayer player = players[i];
                //print("Adding to dict: " + player.playerID);
                string id = player.playerID;
                result.Add(id, player);
            }
            return result;
        }
    }
}