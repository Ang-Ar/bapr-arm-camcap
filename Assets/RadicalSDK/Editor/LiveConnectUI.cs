using System;
using UnityEditor;
using UnityEngine;

namespace Radical
{
    [CustomEditor(typeof(LiveConnector))]
    public class LiveConnectUI : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector GUI for the other serialized properties
            base.OnInspectorGUI();
            
            GUI.enabled = true;
            
            serializedObject.Update();

            // Load the logo PNG image from Assets
            Texture2D logoImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/RadicalSDK/Editor/radical-logowith-t_text_2x.png");
            GUILayout.BeginHorizontal();
            // here the size of the logo in the editor can be adjusted
            float boxLogoMaxWidth = EditorGUIUtility.currentViewWidth;
            float boxLogoMaxHeight = logoImage.height * boxLogoMaxWidth / logoImage.width;
            GUIStyle style = new GUIStyle(GUI.skin.box)
            {
                // there seems to be some padding to left I cannot get rid of so counter it with some padding to the right
                padding = new RectOffset(0, 25, 10, 10)
            };
            //style.CalcHeight(logoImage, EditorGUIUtility.currentViewWidth);
            GUILayout.Box(logoImage, style, GUILayout.MaxWidth(boxLogoMaxWidth), GUILayout.Height(boxLogoMaxHeight));
            //GUILayout.Box(logoImage, style, GUILayout.MaxWidth(boxLogoMaxWidth), GUILayout.ExpandHeight(false));
            GUILayout.EndHorizontal();
            GUILayout.Label("Version 1.6.1");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("radicalRoomID"), new GUIContent("Room ID: "));
            // The account key needs to be hidden behind a password field. For that we grab the property but change the string value directly using a password field
            SerializedProperty accountKeyProperty = serializedObject.FindProperty("accountKey");
            accountKeyProperty.stringValue = EditorGUILayout.PasswordField("Account Key:", accountKeyProperty.stringValue);
            SerializedProperty passwordProperty = serializedObject.FindProperty("password");
            passwordProperty.stringValue = EditorGUILayout.PasswordField("Password (optional):", passwordProperty.stringValue);
            SerializedProperty useOverrideWebSocketURLProperty = serializedObject.FindProperty("useOverrideWebSocketURL");
            useOverrideWebSocketURLProperty.boolValue = EditorGUILayout.Toggle("Websocket URL (optional)", useOverrideWebSocketURLProperty.boolValue);
            if (useOverrideWebSocketURLProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("overrideWebsocketURL"), new GUIContent("Websocket URL:"));
            }
            //Debug.Log("Is exportable: " + ServerBase.isExportableScene);
            if (!ServerBase.isExportableScene)
                EditorModeServerSetup();
            // A good UI tool to show messages like connection status or -errors would be a help box
            //if (editorModeServer == null) editorModeServer = GameObject.FindObjectOfType<EditorModeServer>();

            // Apply any changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }

        void EditorModeServerSetup()
        {
            
            if (EditorModeServer.messageSeverity != MessageSeverity.None)
            {
                // Define a GUIStyle with a smaller icon size
                GUIStyle customStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    fixedHeight = 0,
                    padding = new RectOffset(8, 0, 0, 0)
                };

                // Use the custom GUIStyle to display a selfmade HelpBox
                EditorGUILayout.BeginVertical(customStyle);
                EditorGUILayout.LabelField(new GUIContent(EditorModeServer.message, getMessageIcon()), EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndVertical();

                // The following is what we tried to replace by all of the above
                // EditorGUILayout.HelpBox(editorModeServer.message, editorModeServer.messageType);
            }
            LiveConnector liveConnector = target as LiveConnector;
            bool isFetching = liveConnector.isConnected;
            string label = isFetching ? "Fetching..." : "Fetch Players";
            GUI.enabled = !isFetching; // Disable the button if connected
            if (!UnityEngine.Application.isPlaying && GUILayout.Button(label))
            {
                EditorModeServer.message = "";
                EditorModeServer.messageSeverity = MessageSeverity.None;
                if (MainThreadUtil.Instance == null)
                {
                    
                    MainThreadUtil.Instance = FindFirstObjectByType<MainThreadUtil>();
                }
                try
                {
                    liveConnector.ConnectInEditorMode();
                }
                catch (Exception e)
                {
                    Debug.Log("Reason\n" + e);
                    Debug.LogError("Server connection failed.");
                }
            }
            GUI.enabled = true; // Re-enable the button
                                //EditorGUILayout.PropertyField(serializedObject.FindProperty("serverConfiguration"));

        }
        Texture getMessageIcon()
        {

            switch (EditorModeServer.messageSeverity)
            {
                case MessageSeverity.Info: return EditorGUIUtility.IconContent("console.infoicon.sml").image;
                case MessageSeverity.Warning: return EditorGUIUtility.IconContent("console.warnicon.sml").image;
                case MessageSeverity.Error: return EditorGUIUtility.IconContent("console.erroricon.sml").image;
                default: return null;
            }
        }
    }
}