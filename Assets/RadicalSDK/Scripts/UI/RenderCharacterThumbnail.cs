using System.Collections.Generic;
using UnityEngine;

public class RenderCharacterThumbnail : MonoBehaviour
{
    public Color[] backgroundColors;
    [SerializeField] int resolution = 512;
    Camera cam;

    public void Start()
    {
        cam = GetComponent<Camera>();   
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Renders each selectable character and returns the renderings as textures
    /// </summary>
    /// <param name="characters"></param>
    /// <returns></returns>
    public List<Texture2D> RenderCharacters(HashSet<GameObject> characters, out List<GameObject> characterList)
    {
        gameObject.SetActive(true);
        List<Texture2D> response = new List<Texture2D>();
        characterList = new List<GameObject>();
        int i = 0;
        foreach (GameObject character in characters)
        {
            GameObject clone = Instantiate(character, Vector3.zero, Quaternion.identity);
            RenderTexture rt = new RenderTexture(resolution, resolution, 0)
            {
                antiAliasing = 8,
                name = character.name
            };
            cam.targetTexture = rt;
            RenderTexture.active = rt;
            cam.Render();
            Texture2D icon = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
            icon.ReadPixels(new Rect(0, 0, resolution, resolution), 0,0);
            icon.Apply();
            response.Add(icon);
            characterList.Add(character);
            clone.SetActive(false);
            Destroy(clone);
            cam.backgroundColor = backgroundColors[i];
            i++;
        }

        cam.targetTexture = null;
        Destroy(gameObject);
        return response;
    }
}
