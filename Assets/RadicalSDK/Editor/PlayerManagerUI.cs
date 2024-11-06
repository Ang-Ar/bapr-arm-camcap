using UnityEditor;
using UnityEngine;

namespace Radical
{
    [CustomEditor(typeof(RadicalPlayerManager))]
    public class PlayerManagerUI : Editor
    {
        float inspectorWidth; // needs to be global bc. GetControlRect returns the correct width only after the tab has been resized (else 1)
        public override bool RequiresConstantRepaint() => true;
        //public override void OnInspectorGUI()
        //{
        //    DrawDefaultInspector();

        //    GUILayout.Space(10);
        //    RadicalPlayerManager playerManager = target as RadicalPlayerManager;

        //    //if (GUILayout.Button("Add Character"))
        //    //{
        //    //    playerManager.AddCharacter();
        //    //}
        //    GUILayout.Space(10);
        //    // Access the serialized property for the list of RadicalPlayers
        //    SerializedProperty radicalPlayersProperty = serializedObject.FindProperty("allCharacters");

        //    // Draw a header label for the list
        //    EditorGUILayout.LabelField("Radical Players");

        //    // Draw a separator line
        //    EditorGUILayout.Separator();
        //    Rect area = EditorGUILayout.GetControlRect(true); // get the area of the inspector tab
        //    setWidth(area);
        //    // Draw the table header
        //    EditorGUILayout.BeginHorizontal();

        //    float playerIDWidth = inspectorWidth * 0.3f;
        //    float prefabWidth = inspectorWidth * 0.5f;
        //    float bakeWidth = inspectorWidth * 0.15f;

        //    EditorGUILayout.LabelField("Player ID", GUILayout.Width(playerIDWidth));
        //    EditorGUILayout.LabelField("Player Prefab", GUILayout.Width(prefabWidth));
        //    EditorGUILayout.LabelField("Is Baking", GUILayout.Width(bakeWidth));
        //    EditorGUILayout.EndHorizontal();

        //    // Draw each element in the list
        //    int length = radicalPlayersProperty.arraySize;
        //    for (int i = 0; i < length; i++)
        //    {
        //        SerializedProperty elementProperty = radicalPlayersProperty.GetArrayElementAtIndex(i);

        //        SerializedProperty playerIDProperty = elementProperty.FindPropertyRelative("playerID");
        //        SerializedProperty playerPrefabProperty = elementProperty.FindPropertyRelative("playerPrefab");
        //        SerializedProperty isBakingProperty = elementProperty.FindPropertyRelative("isBaking");

        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.PropertyField(playerIDProperty, GUIContent.none, GUILayout.Width(playerIDWidth));
        //        EditorGUILayout.PropertyField(playerPrefabProperty, GUIContent.none, GUILayout.Width(prefabWidth));
        //        EditorGUILayout.PropertyField(isBakingProperty, GUIContent.none, GUILayout.Width(bakeWidth));
        //        EditorGUILayout.EndHorizontal();
        //    }

        //    // Apply any changes to the serialized object
        //    serializedObject.ApplyModifiedProperties();
        //}


        void setWidth(Rect rect)
        {
            // get the absolute width of an element based on the tab's width and its relative size
            float width = rect.width;
            if (width > 1)
                inspectorWidth = width;
        }
    }
}