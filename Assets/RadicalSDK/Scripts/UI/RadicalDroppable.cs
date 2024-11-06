using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Radical
{
    public class RadicalDroppable : MonoBehaviour, IDropHandler
    {
        public GameObject selectedPrefab;

        protected Texture2D currentTexture;
        protected RawImage icon;


        public virtual void OnDrop(PointerEventData eventData)
        {

        }
    }
}
