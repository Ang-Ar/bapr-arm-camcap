using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationPlayback))]
public class AnimationPlaybackInspector : Editor
{
    public override void OnInspectorGUI()
    {
        AnimationPlayback target = (AnimationPlayback)this.target;
        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (! Application.isPlaying)
        {
            GUILayout.Label("Enter play mode to enable playback");
            return;
        }

        GUILayout.Label(string.Format("Animation Playback {0}", target.IsActive ? "ON" : "OFF"));
        if (GUILayout.Button(target.IsActive ? "Deactivate" : "Activate"))
        {
            if (target.IsActive) target.Deactivate();
            else target.Activate();
        }
    }
}
