using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_PlayerMan))]
    public class HFPS_PlayerManEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_PlayerMan hfpsPlayMan;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsPlayMan = (HFPS_PlayerMan)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty slowDownUse = serializedObject.FindProperty("slowDownUse");

            SerializedProperty references = serializedObject.FindProperty("references");
            SerializedProperty meleeConts = serializedObject.FindProperty("meleeConts");
            SerializedProperty weaponConts = serializedObject.FindProperty("weaponConts");
            SerializedProperty throwConts = serializedObject.FindProperty("throwConts");
            SerializedProperty dmWeapConts = serializedObject.FindProperty("dmWeapConts");

            SerializedProperty enemHold = serializedObject.FindProperty("enemHold");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/Components Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != Components Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/Components Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/Components-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Player Manager", "HeaderText_Small");

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

            EditorGUILayout.BeginVertical();

            hfpsPlayMan.tabs = GUILayout.SelectionGrid(hfpsPlayMan.tabs, new string[] { "User Options", "References", "Auto"}, 3);

            if(hfpsPlayMan.tabs == 0){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Sets how movement slow down occurs when zooming." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(slowDownUse, true);

            }//tabs = user options

            if(hfpsPlayMan.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(references, new GUIContent("HFPS References"), true);

                EditorGUILayout.Space();
                
                if(GUILayout.Button("Catch Weapons")){
                
                    hfpsPlayMan.Weapons_Catch();
                
                }//button
                
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(meleeConts, true);
                EditorGUILayout.PropertyField(weaponConts, true);
                
                #if HFPS_THROWABLES_PRESENT
                
                    EditorGUILayout.PropertyField(throwConts, true);
                
                #endif
                
                #if HFPS_WEAPONS_PRESENT
                
                    EditorGUILayout.PropertyField(dmWeapConts, true);
                
                #endif

            }//tabs = references

            if(hfpsPlayMan.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(enemHold, new GUIContent("Enemies Holder"), true);
                hfpsPlayMan.isHiding = EditorGUILayout.Toggle("Is Hiding?", hfpsPlayMan.isHiding);

            }//tabs = auto

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsPlayMan);

                if(!EditorApplication.isPlaying){

                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                }//!isPlaying

            }//changed

            EditorGUILayout.Space();

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


    }//HFPS_PlayerManEditor


}//namespace