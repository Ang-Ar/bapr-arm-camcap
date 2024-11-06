using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Radical
{
    public class DropCharacter : RadicalDroppable, IDropHandler
    {
        //[HideInInspector] public GameObject assignedCharacter;
        //public TextMeshProUGUI label;
        //public int index = 0; // 0 => defaultCharacter
        //public GameObject selectedPrefab;

        //RawImage icon;
        // RenderTexture currentTexture;
        public TextMeshProUGUI label;

        [HideInInspector] public Vector3 spawnPosition;
        public void Init(string userName, GameObject prefab, Texture2D texture)
        {
            icon = GetComponent<RawImage>();
            icon.texture = texture;
            selectedPrefab = prefab;
            currentTexture = texture;
            label.text = userName;
        }
        public override void OnDrop(PointerEventData eventData)
        {
            GameObject droppedCharacter = eventData.pointerDrag;
            if (droppedCharacter.TryGetComponent(out CharacterSelectButton character))
            {
                selectedPrefab = character.characterPrefab;
                currentTexture = character.texture;
                icon.texture = currentTexture;
            }
        }

        public void AssignSpawnPoint(Vector3 spawnPosition)
        {
            this.spawnPosition = spawnPosition;
        }
    }
}