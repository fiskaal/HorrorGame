using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.Utility {

    [CustomEditor(typeof(DM_ForwardDetect))]
    public class DM_ForwardDetectEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        DM_ForwardDetect dmForDetect;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            dmForDetect = (DM_ForwardDetect)target;

        }//OnEnable

        public override void OnInspectorGUI() { 

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15);

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/DM Utility Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != DM Utility Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/DM_ForwardDir-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Forward Detect", "HeaderText");

            GUILayout.Label(t, "headerIcon");

            GUILayout.FlexibleSpace();

            if(!showTips){

                if(GUILayout.Button(t2, "infoIcon")){

                    ShowTips_Check();

                }//Button

            }//!showTips

            if(showTips){

                if(GUILayout.Button(t3, "infoIcon")){

                    ShowTips_Check();

                }//Button

            }//showTips

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUI.skin = oldSkin;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            dmForDetect.tabs = GUILayout.SelectionGrid(dmForDetect.tabs, new string[] { "User Options", "Auto/Gizmos"}, 2);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            SerializedProperty gizmoColor = serializedObject.FindProperty("gizmoColor");

            SerializedProperty forwardDir = serializedObject.FindProperty("forwardDir");
            SerializedProperty forwardType = serializedObject.FindProperty("forwardType");
            SerializedProperty forwardAngle = serializedObject.FindProperty("forwardAngle");

            if(dmForDetect.tabs == 0){

                if(showTips){

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("Allows direction detection if TRUE.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                dmForDetect.detect = EditorGUILayout.Toggle("Detect?", dmForDetect.detect);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("Direction of the forward you want to use.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(forwardDir, new GUIContent("Forward Direction"), true);

                if(showTips){

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("Type of facing direcition to use.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(forwardType, true);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("The angle that's allowed for detection.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(forwardAngle, true);

            }//tabs = user options

            if(dmForDetect.tabs == 1){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("Displays gizmos if ON.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.LabelField("Gizmos", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                dmForDetect.gizmosInt = GUILayout.Toolbar(dmForDetect.gizmosInt, new string[] { "OFF", "ON" });

                if(dmForDetect.gizmosInt == 0){

                    dmForDetect.useGizmos = false;

                }//gizmosInt == 0

                if(dmForDetect.gizmosInt == 1){

                    dmForDetect.useGizmos = true;

                }//gizmosInt == 1

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(gizmoColor, true);
                dmForDetect.rayDist = EditorGUILayout.FloatField("Ray Distance", dmForDetect.rayDist);

            }//tabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(dmForDetect);

                if(!EditorApplication.isPlaying){

                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                }//!isPlaying

            }//changed

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

        }//OnInspectorGUI


    //////////////////////////
    //
    //      TIPS ACTIONS
    //
    //////////////////////////


        public void ShowTips_Check(){

            if(showTips){

                showTips = false;

            //showTips
            } else {

                showTips = true;

            }//showTips

        }//ShowTips_Check


    }//DM_ForwardDetectEditor


}//namespace