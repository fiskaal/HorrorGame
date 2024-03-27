using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.Shared {

    [CustomEditor(typeof(HFPS_References))]
    public class HFPS_ReferencesEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_References hfpsRefs;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsRefs = (HFPS_References)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty characterController = serializedObject.FindProperty("characterController");
            SerializedProperty playCont = serializedObject.FindProperty("playCont");
            SerializedProperty playerFunct = serializedObject.FindProperty("playerFunct");
            SerializedProperty healthManager = serializedObject.FindProperty("healthManager");
            SerializedProperty mouseLook = serializedObject.FindProperty("mouseLook");
            SerializedProperty jsEffects = serializedObject.FindProperty("jsEffects");
            SerializedProperty itemSwitcher = serializedObject.FindProperty("itemSwitcher");
            SerializedProperty playerRigid = serializedObject.FindProperty("playerRigid");

            SerializedProperty audioFader = serializedObject.FindProperty("audioFader");
            SerializedProperty playerMan = serializedObject.FindProperty("playerMan");
            SerializedProperty screenEvents = serializedObject.FindProperty("screenEvents");
            SerializedProperty subActionsHandler = serializedObject.FindProperty("subActionsHandler");

            SerializedProperty charAction = serializedObject.FindProperty("charAction");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/DM Utility Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != Components Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/DM_References-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("References", "HeaderText");

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

            hfpsRefs.tabs = GUILayout.SelectionGrid(hfpsRefs.tabs, new string[] { "User Options", "Helpers", "Auto"}, 3);

            if(hfpsRefs.tabs == 0){

                EditorGUILayout.Space();

                hfpsRefs.hfpsOpts = GUILayout.Toggle(hfpsRefs.hfpsOpts, "HFPS", GUI.skin.button);

                if(hfpsRefs.hfpsOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(characterController, true);
                    EditorGUILayout.PropertyField(playCont, new GUIContent("Player Controller"), true);
                    EditorGUILayout.PropertyField(playerFunct, new GUIContent("Player Functions"), true);
                    EditorGUILayout.PropertyField(playerRigid, true);
                    EditorGUILayout.PropertyField(healthManager, true);
                    EditorGUILayout.PropertyField(mouseLook, true);
                    EditorGUILayout.PropertyField(jsEffects, new GUIContent("Jumpscare Effects"), true);
                    EditorGUILayout.PropertyField(itemSwitcher, true);

                }//hfpsOpts
                
                #if COMPONENTS_PRESENT

                    EditorGUILayout.Space();

                    hfpsRefs.compOpts = GUILayout.Toggle(hfpsRefs.compOpts, "Components", GUI.skin.button);

                    if(hfpsRefs.compOpts){

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(audioFader, true);
                        EditorGUILayout.PropertyField(playerMan, new GUIContent("Player Manager"), true);
                        EditorGUILayout.PropertyField(screenEvents, true);
                        EditorGUILayout.PropertyField(subActionsHandler, true);

                    }//compOpts
                
                #endif

                EditorGUILayout.Space();

            }//tabs = user options

            if(hfpsRefs.tabs == 1){

                EditorGUILayout.Space();

                hfpsRefs.helpCompRefs = GUILayout.Toggle(hfpsRefs.helpCompRefs, "References", GUI.skin.button);

                if(hfpsRefs.helpCompRefs){

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Use the button below to catch reference scripts." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                    if(GUILayout.Button("Catch References")){

                        if(EditorUtility.DisplayDialog("Update References?", "You are about catch script references, this will overwrite any existing references." + "\n" + "\n" + "Are you sure?", "Yes", "No")){

                            hfpsRefs.References_Catch();

                        }//DisplayDialog

                    }//button

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                //helpCompRefs
                } else {

                    EditorGUILayout.Space();

                }//helpCompRefs

            }//tabs = helpers

            if(hfpsRefs.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();
                
                #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_VENDOR_PRESENT)

                    EditorGUILayout.PropertyField(charAction, true);

                    EditorGUILayout.Space();

                #else
                
                    EditorGUILayout.HelpBox("\n" + "No automatic values to track." + "\n", MessageType.Warning);

                    EditorGUILayout.Space();
                
                #endif
    
            }//tabs = auto

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsRefs);

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


    }//HFPS_ReferencesEditor


}//namespace