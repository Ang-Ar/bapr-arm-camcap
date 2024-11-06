using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Radical
{
    public class UIManager : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject playerThumbnailPrefab;
        public GameObject dropTargetPrefab;
        public GameObject spawnPointSelect;
        
        [Header("Scroll views")]
        public Transform characterScrollViewContents;
        public Transform playerScrollViewContents;
        public Transform spawnPointScrollViewContents;
        public GameObject scrollViews; // parent of the subsequent UI
        GameObject canvas;
        [Header("UI Elements")]
        public TMP_InputField roomIDField;
        public Button startButton;

        PlayerList playerList;
        RenderCharacterThumbnail thumbnailRenderer;
        List<DropCharacter> dropCharacters;
        List<RadicalSpawnPoint> spawnPoints;
        static UIManager m_instance;

        private void Awake()
        {
            thumbnailRenderer = FindObjectOfType<RenderCharacterThumbnail>(true);
            startButton.enabled = false;
            spawnPoints = new List<RadicalSpawnPoint>();

            m_instance = this;
        }

        private void Start()
        {
            canvas = characterScrollViewContents.transform.root.gameObject;
            canvas.SetActive(true);
            scrollViews.SetActive(false);
            
#if UNITY_EDITOR
            //Debug
            string roomID = GetComponent<LiveConnector>().radicalRoomID;
            if (!string.IsNullOrEmpty(roomID))
            {
                roomIDField.SetTextWithoutNotify(roomID);
                //GetComponent<LiveConnector>().Connect(); //HACK: DEBUG, remove
            }
#endif
        }

         public void PopulateCharacterScrollViews(PlayerList players)
         {
            createCharacterSelection(out Texture2D defaultTexture);
            createPlayerList(players, defaultTexture);
            createSpawnPointSelection();
         }

        void createPlayerList(PlayerList players, Texture2D defaultTexture)
        {
            GameObject defaultCharacter = GetComponent<PlayModeServer>().defaultCharacter;
            dropCharacters = new List<DropCharacter>();
            scrollViews.SetActive(true);
            
            int length = players.players.Length;

            for (int i = 0; i < length; i++)
            {
                JPlayer player = players.players[i];
                if (!player.isPlayer) continue;
                GameObject scrollViewElement = Instantiate(dropTargetPrefab, playerScrollViewContents);
                DropCharacter dropCharacter = scrollViewElement.GetComponentInChildren<DropCharacter>();
                dropCharacter.Init(player.userName, defaultCharacter, defaultTexture);
                dropCharacters.Add(dropCharacter);
            }
            playerList = players;
        }

        void createSpawnPointSelection()
        {
            int length = spawnPoints.Count;
            for (int i = 0; i < length; i++)
            {
                RadicalSpawnPoint spawnPoint = spawnPoints[i]; 
                GameObject scrollViewElement = Instantiate(spawnPointSelect, spawnPointScrollViewContents);
                SpawnPointSelectButton dragSpawnPoint = scrollViewElement.GetComponentInChildren<SpawnPointSelectButton>();
                dragSpawnPoint.Init(spawnPoint, i + 1); //first one already exists
            }
        }

        void createCharacterSelection(out Texture2D defaultCharacterThumbnail)
        {
            PlayModeServer characterManager = GetComponent<PlayModeServer>();
            if (characterManager.defaultCharacter == null)
            {
                characterManager.defaultCharacter = GetComponent<RadicalPlayerManager>().radicalCharacterPrefab;
            }
            HashSet<GameObject> characterPool = new HashSet<GameObject> //these 2 characters are always present (though they may be the same)
            {
                characterManager.defaultCharacter,
                GetComponent<RadicalPlayerManager>().radicalCharacterPrefab
            };
            characterPool.UnionWith(characterManager.characterPool);
            
            List<Texture2D> textures = thumbnailRenderer.RenderCharacters(characterPool, out List<GameObject> sortedCharacterPool);
            int length = textures.Count;
            defaultCharacterThumbnail = textures[0];
            for (int i = 0; i < length; i++)
            {
                GameObject thumbnail = Instantiate(playerThumbnailPrefab);
                thumbnail.transform.parent = characterScrollViewContents;
                CharacterSelectButton selectButton = thumbnail.GetComponent<CharacterSelectButton>();
                selectButton.AssignTexture(textures[i], sortedCharacterPool[i], i);
            }
            startButton.enabled = true;
        }
        public static void AddSpawnPoint(RadicalSpawnPoint spawnPoint)
        {
            m_instance.spawnPoints.Add(spawnPoint);
        }
        #region UI

        public void INP_OnEndEditRoomID(string roomID)
        {
            //print("Room id length: " + roomID.Length);
            if (roomID.Length < 36)
            {
                ErrorReport.ShowMessage("The room ID is too short.", MessagePriority.WrongRoomID);
            }
            else if (roomID.Length > 36)
            {
                ErrorReport.ShowMessage("The room ID is too long.", MessagePriority.WrongRoomID);
            }
            else
            {
                if (TryGetComponent(out LiveConnector connector))
                {
                    ErrorReport.ShowMessage("", MessagePriority.None); // hides the error message
                    connector.radicalRoomID = roomID;
                    connector.Connect();
                }
                else
                {
                    throw new NotImplementedException("There is no radical server prefab in the scene.");
                    //ErrorReport.ShowMessage("ERROR: There is no RadicalServerConnector prefab in this scene.", MessageSeverity.Warning
                }
            }
        }

        public void INP_OnBeginEditRoomID()
        {
            ErrorReport.ShowMessage("", MessagePriority.None); // hides the error message
        }

        public void BTN_CloseErrorMessage()
        {
            //RadicalMessage. GetComponent<ServerBase>().ShowEMessage(MessagePriority.None, MessageSeverity.None);
        }

        public void BTN_AddPlayer()
        {
            GetComponent<RadicalPlayerManager>().CreatePlayers(playerList, dropCharacters);
        }

        public void BTN_Start()
        {
            //print("Pressed button start");
            GetComponent<RadicalPlayerManager>().CreatePlayers(playerList, dropCharacters);
            roomIDField.gameObject.SetActive(false);

            //Destroy(playerScrollViewContents.parent.gameObject); // we don't need the canvas anymore
            //Destroy(playerScrollViewContents.root.gameObject); // we don't need the canvas anymore
        }

        internal void ReOpenMenu()
        {
            canvas.SetActive(true);
        }
        #endregion
    }
}