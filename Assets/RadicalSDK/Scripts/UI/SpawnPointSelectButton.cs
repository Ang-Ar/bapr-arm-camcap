using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Radical
{
    
    public class SpawnPointSelectButton : RadicalDraggable
    {
        [HideInInspector] public RadicalSpawnPoint spawnPoint;
        private void Start()
        {
            texture = GetComponent<RawImage>().mainTexture as Texture2D;
        }

        public void Init(RadicalSpawnPoint spawnPoint, int index)
        {
            this.spawnPoint = spawnPoint;
            this.index = index;
            image = GetComponent<RawImage>();
            texture = spawnPoint.GetIcon(); 
            image.texture = texture;

            name = spawnPoint.name;
            label.text = spawnPoint.name;
        }
    }
}
