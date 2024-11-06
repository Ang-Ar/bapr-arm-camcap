using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Radical
{
    public class RadicalDraggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public float targetScale = 0.33f;
        public int index;
        public Texture2D texture;

        public TextMeshProUGUI label;
        protected RawImage image;

        public virtual void AssignTexture(Texture2D texture, GameObject associatedGameObject, int index)
        {
            this.index = index;
            this.texture = texture;
            image = GetComponent<RawImage>();
            image.texture = texture;
            name = texture.name;
            label.text = associatedGameObject.name;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            GameObject clone = Instantiate(gameObject); // create a clone and place it at the exact position where this instance was
            clone.GetComponent<RadicalDraggable>().SetColor(Color.white);
            Transform t = clone.transform; // creating the clone is necessary, bc. the dragged obj is global in Unity
            t.SetParent(transform.parent);
            t.SetSiblingIndex(index);
            transform.SetParent(transform.root);
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            image.raycastTarget = false; // so it doesn't interfere with the raycast of the drop target
        }

        public void SetColor(Color color)
        {
            if (image == null)
                image = GetComponent<RawImage>();
            image.color = color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetColor(Color.cyan);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            SetColor(Color.white);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Mouse.current.position.value;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Destroy(gameObject);
        }
    }
}
