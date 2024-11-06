
using UnityEngine;

namespace Radical
{
    public class RadicalSpawnPoint : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] int resolution = 512;
        void Start()
        {
            UIManager.AddSpawnPoint(this);
        }

        public Texture2D GetIcon()
        {

            Texture2D icon = new Texture2D(resolution, resolution, TextureFormat.RGB24, false); 
            RenderTexture rt = new RenderTexture(resolution, resolution, 0)
            {
                antiAliasing = 8
            };
            RenderTexture.active = rt;
            cam.targetTexture = rt;
            cam.Render();
            cam.targetTexture = null;
            Destroy(cam.gameObject);
            icon.ReadPixels(new Rect(0,0, resolution, resolution), 0, 0);
            icon.Apply();
            return icon;
        }
    }
}
