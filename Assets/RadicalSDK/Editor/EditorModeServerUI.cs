using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Radical
{
    [CustomEditor(typeof(EditorModeServer))]
    public class EditorModeServerUI : Editor
    {
        float inspectorWidth;
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            ServerBase.isExportableScene = false;
            //GUILayout.Space(10);
            //RadicalPlayerManager playerManager = target as RadicalPlayerManager;

            //if (GUILayout.Button("Add Character"))
            //{
            //    playerManager.AddCharacter();
            //}
            //GUILayout.Space(10);
            // Access the serialized property for the list of RadicalPlayers
            EditorGUILayout.LabelField("Radical Players");
            SerializedProperty radicalPlayersProperty = serializedObject.FindProperty("allCharacters");

            // Draw a header label for the list


            // Draw a separator line
            //EditorGUILayout.Separator();
            Rect area = EditorGUILayout.GetControlRect(true); // get the area of the inspector tab
            setWidth(area);
            // Draw the table header
            EditorGUILayout.BeginHorizontal();

            float playerIDWidth = inspectorWidth * 0.3f;
            float prefabWidth = inspectorWidth * 0.5f;
            float bakeWidth = inspectorWidth * 0.15f;

            //EditorGUILayout.LabelField("Player ID", GUILayout.Width(playerIDWidth));
            EditorGUILayout.LabelField("Username", GUILayout.Width(playerIDWidth));
            EditorGUILayout.LabelField("Player Prefab", GUILayout.Width(prefabWidth));
            EditorGUILayout.LabelField("Is Baking", GUILayout.Width(bakeWidth));
            EditorGUILayout.EndHorizontal();

            // Draw each element in the list
            int length = radicalPlayersProperty.arraySize;
            for (int i = 0; i < length; i++)
            {
                SerializedProperty elementProperty = radicalPlayersProperty.GetArrayElementAtIndex(i);

                //SerializedProperty playerIDProperty = elementProperty.FindPropertyRelative("playerID");
                SerializedProperty playerIDProperty = elementProperty.FindPropertyRelative("userName");
                SerializedProperty playerPrefabProperty = elementProperty.FindPropertyRelative("playerPrefab");
                SerializedProperty isBakingProperty = elementProperty.FindPropertyRelative("isBaking");

                EditorGUILayout.BeginHorizontal(); //FIXME: According to Unity this looks for element 9 in an array of size 9
                EditorGUILayout.LabelField(playerIDProperty.stringValue, GUILayout.Width(playerIDWidth));
                EditorGUILayout.PropertyField(playerPrefabProperty, GUIContent.none, GUILayout.Width(prefabWidth));
                EditorGUILayout.PropertyField(isBakingProperty, GUIContent.none, GUILayout.Width(bakeWidth));
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Clear List"))
            {
                (target as EditorModeServer).ClearPlayerList();
            }
            // Apply any changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }


        void setWidth(Rect rect)
        {
            // get the absolute width of an element based on the tab's width and its relative size
            float width = rect.width;
            if (width > 1)
                inspectorWidth = width;
        }
    }
}