using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JointLimits))]
public class JointLimitsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        JointLimits target = this.target as JointLimits;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("save constrained rest pose"))
        {
            Undo.RecordObject(target, "save rest pose (constrained)");
            target.restPose = target.constrained.localRotation;
            Debug.Log($"Saved rest pose of constrained object (local Unity Eulers) {target.restPose.eulerAngles}");
        }
        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();
    }
}
