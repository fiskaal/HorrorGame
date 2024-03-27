using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.Shared {

    [CustomEditor(typeof(DM_ActionBar))]
    public class DM_ActionBarEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        DM_ActionBar actBar;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            actBar = (DM_ActionBar)target;

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

            Texture2D t = (Texture2D)Resources.Load("EditorContent/DM_Utility-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Action Bar", "HeaderText");

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

            actBar.actBarTabs = GUILayout.SelectionGrid(actBar.actBarTabs, new string[] { "User Options", "References", "Auto/Debug"}, 3);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            SerializedProperty pauseInput = serializedObject.FindProperty("pauseInput");
            SerializedProperty pauseBackInput = serializedObject.FindProperty("pauseBackInput");

            SerializedProperty actions = serializedObject.FindProperty("actions");
            SerializedProperty events = serializedObject.FindProperty("events");
            SerializedProperty holder = serializedObject.FindProperty("holder");

            SerializedProperty curDevice = serializedObject.FindProperty("curDevice");
            SerializedProperty auto = serializedObject.FindProperty("auto");

            if(actBar.actBarTabs == 0){

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Click the toggles below to show the options for each section." + "\n", MessageType.Info);

                }//showTips

                EditorGUILayout.Space();

                actBar.startOpts = GUILayout.Toggle(actBar.startOpts, "Start Options", GUI.skin.button);

                if(actBar.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Creates an instance reference that can be used by other scripts." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips 

                    actBar.createInstance = EditorGUILayout.Toggle("Create Instance?", actBar.createInstance);

                }//startOpts

                EditorGUILayout.Space();

                actBar.inputOpts = GUILayout.Toggle(actBar.inputOpts, "Input Options", GUI.skin.button);

                if(actBar.inputOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Detects pause input to lock / hide action bar if actions are active." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    actBar.detectPause = EditorGUILayout.Toggle("Detect Pause?", actBar.detectPause);

                    if(actBar.detectPause){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Input to detect for pause." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(pauseInput, true);

                    }//detectPause
                    
                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Detects pause back input to lock / hide action bar if actions are active." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    actBar.detectPauseBack = EditorGUILayout.Toggle("Detect Pause Back?", actBar.detectPauseBack);

                    if(actBar.detectPauseBack){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Input to detect for pause back." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(pauseBackInput, true);

                    }//detectPauseBack

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The wait time before input detection." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    actBar.inputWait = EditorGUILayout.FloatField("Input Wait", actBar.inputWait);

                }//inputOpts

            }//actBarTabs = user options

            if(actBar.actBarTabs == 1){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "The parent holder object used." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips 

                EditorGUILayout.PropertyField(holder, true);

                if(showTips){

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "The base & sub action references used when displaying actions." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips 

                EditorGUILayout.PropertyField(actions, true);

            }//actBarTabs = references

            if(actBar.actBarTabs == 2){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("Displays debug logs in the console if ON.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.LabelField("Debug Notifications", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                actBar.debugInt = GUILayout.Toolbar(actBar.debugInt, new string[] { "OFF", "ON" });

                if(actBar.debugInt == 0){

                    actBar.useDebug = false;

                }//debugInt == 0

                if(actBar.debugInt == 1){

                    actBar.useDebug = true;

                }//debugInt == 1

                GUILayout.Space(15);

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(curDevice, true);

                EditorGUILayout.PropertyField(auto, true);

            }//actBarTabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(actBar);

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


    }//DM_ActionBarEditor


}//namespace