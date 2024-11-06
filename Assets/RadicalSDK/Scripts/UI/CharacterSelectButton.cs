using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Radical
{
    public class CharacterSelectButton : RadicalDraggable, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject characterPrefab;

        public override void AssignTexture(Texture2D texture, GameObject character, int index)
        {
            this.characterPrefab = character;
            base.AssignTexture(texture, character, index);  
        }

        

       
    }
}