using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConeLimiter))]
public class ConeLimiterInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ConeLimiter target = this.target as ConeLimiter;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("save cone axis"))
        {
            Undo.RecordObject(target, "save cone axis");
            target.coneAxis = target.constrained.localRotation;
            Debug.Log($"Saved cone axis (local Unity Eulers: {target.coneAxis.eulerAngles})");
        }
        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();
    }
}
