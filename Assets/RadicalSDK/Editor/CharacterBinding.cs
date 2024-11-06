using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Radical
{
    [CustomEditor(typeof(RadicalCharacterAnimator))]
    public class CharacterBinding : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            RadicalCharacterAnimator character = target as RadicalCharacterAnimator;

            ////if (GUILayout.Button("Bind Skeletton"))
            ////{
            ////    //[Tooltip("Some Rings are stored multiple times because of same model, different real world materials.")]
            ////    character.ConvertToRadicalCharacter();
            ////}
            //GUILayout.Space(10);
            //GUILayout.Label("Animation");
            //string label = character.isRecording ? "Stop Recording" : "Record Animation";
            //if (GUILayout.Button(label))
            //{
            //    //[Tooltip("Some Rings are stored multiple times because of same model, different real world materials.")]
            //    character.RecordAnimation();
            //}
        }
    }
}