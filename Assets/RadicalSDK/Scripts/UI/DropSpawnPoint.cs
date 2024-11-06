using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Radical
{
    
    public class DropSpawnPoint : RadicalDroppable
    {
        public DropCharacter dropCharacter;
        RadicalSpawnPoint spawnPoint;

        private void Start()
        {
            icon = GetComponent<RawImage>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            GameObject droppedSpawnPoint = eventData.pointerDrag;
            if (droppedSpawnPoint.TryGetComponent(out SpawnPointSelectButton spawnPointSelect))
            {
                currentTexture = spawnPointSelect.texture;
                icon.texture = currentTexture;
                Vector3 position = spawnPointSelect.spawnPoint == null ? new Vector3() : spawnPointSelect.spawnPoint.transform.position;
                dropCharacter.AssignSpawnPoint(position);
            }
        }
    }
}