using UnityEditor;
using UnityEngine;

namespace Radical
{
    public class RadicalAnimationRecorder : MonoBehaviour
    {
        //Dictionary<string, RadicalPlayer> m_Characters = new Dictionary<string, RadicalPlayer>();
        //GameObjectRecorder recorder;
        AnimationPlaybackObject playbackObject;
        string playerName;
        public void Init(string _playerName)
        {
            playerName = _playerName;
            playbackObject = ScriptableObject.CreateInstance<AnimationPlaybackObject>();
        }

        public void UnRegisterCharacter(RadicalPlayer player, bool saveRecording) { }

        public void StopRecording()
        {
            //AnimationClip targetClip = createAnimationClip();
            //recorder.SaveToClip(targetClip); //TODO: This will overwrite any animation at any time the bake is initialized
            ////string originalName = player.playerPrefab == null ? "default" : player.playerPrefab.name;
            //string filename = targetClip.name + "_animation.anim";
            //AssetDatabase.CreateAsset(targetClip, "Assets/RadicalCharacterAnimation/" + filename);
            //AssetDatabase.SaveAssets();
            //Destroy(recorder);
            //print($"Saved animation for {playerName} to {filename}");
        }



        private void OnApplicationQuit()
        {
            StopRecording();
        }
    }
}
