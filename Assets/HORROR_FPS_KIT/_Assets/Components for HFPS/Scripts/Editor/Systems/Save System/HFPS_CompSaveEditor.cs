using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_CompSave))]
    public class HFPS_CompSaveEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_CompSave compSave;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            compSave = (HFPS_CompSave)target;

        }//OnEnable

        public override void OnInspectorGUI() { 

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15);

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/Components Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != DM Utility Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/Components Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/Components-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Components Save", "HeaderText_Small");

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

            compSave.tabs = GUILayout.SelectionGrid(compSave.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            SerializedProperty itemsSaveType = serializedObject.FindProperty("itemsSaveType");
            SerializedProperty objSaveType = serializedObject.FindProperty("objSaveType");

            SerializedProperty compDataName = serializedObject.FindProperty("compDataName");

            SerializedProperty events = serializedObject.FindProperty("events");

            SerializedProperty saveData = serializedObject.FindProperty("saveData");
            SerializedProperty compVersionRef = serializedObject.FindProperty("compVersion");

            EditorGUILayout.Space(); 

            if(compSave.tabs == 0){

                if(showTips){

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Click the toggles below to show the options for each section." + "\n", MessageType.Info);

                }//showTips

                EditorGUILayout.Space();

                compSave.startOpts = GUILayout.Toggle(compSave.startOpts, "Start Options", GUI.skin.button);

                if(compSave.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("Transfer Save System from scene to scene.", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    compSave.dontDestroy = EditorGUILayout.Toggle("Dont Destroy?", compSave.dontDestroy);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("Creates an instance refernce which can be used by other scripts (i.e HFPS_CompSave.instance)", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    compSave.createInstance = EditorGUILayout.Toggle("Create Instance?", compSave.createInstance);

                    EditorGUILayout.Space();

                }//startOpts

                EditorGUILayout.Space();

                compSave.saveOpts = GUILayout.Toggle(compSave.saveOpts, "Save Options", GUI.skin.button);

                if(compSave.saveOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("Initial Wait time before StartInit begins.", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    compSave.initWait = EditorGUILayout.FloatField("Init Wait", compSave.initWait);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("Loads Components data on start if TRUE.", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    compSave.useStartLoad = EditorGUILayout.Toggle("Use Start Load?", compSave.useStartLoad);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("Uses Obfuscation on Components save data if TRUE.", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    compSave.secureSave = EditorGUILayout.Toggle("Secure Save?", compSave.secureSave);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("Creates a debug Components save file if TRUE (Used in combination with secure save)", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    compSave.debugJSON = EditorGUILayout.Toggle("Debug JSON?", compSave.debugJSON);

                }//saveOpts

                EditorGUILayout.Space();

                compSave.itemsOpts = GUILayout.Toggle(compSave.itemsOpts, "Items Options", GUI.skin.button);

                if(compSave.itemsOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("The type of save to be used for items.", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemsSaveType, true);

                }//itemsOpts

                EditorGUILayout.Space();

                compSave.objOpts = GUILayout.Toggle(compSave.objOpts, "Objectives Options", GUI.skin.button);

                if(compSave.objOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("The type of save to be used for objectives.", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(objSaveType, new GUIContent("Objectives Save"), true);

                }//objOpts

                EditorGUILayout.Space();

                compSave.fileOpts = GUILayout.Toggle(compSave.fileOpts, "File Options", GUI.skin.button);

                if(compSave.fileOpts){

                    EditorGUILayout.Space();

                    compSave.fileTabs = GUILayout.SelectionGrid(compSave.fileTabs, new string[] { "Components", "HFPS"}, 2);

                    if(compSave.fileTabs == 0){

                        if(compSave.compVersion != null){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Components Version Detected!" + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Fill in the file names below to correctly save files." + "\n" + "Use the buttons below to clear files if you need to." + "\n", MessageType.Info);

                            }//showTips

                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(compDataName, new GUIContent("Data Name"), true);

                            EditorGUILayout.Space();

                            compSave.resetOnPlayStop = EditorGUILayout.Toggle("Reset Data on Stop?", compSave.resetOnPlayStop);

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.Space();

                            #if UNITY_EDITOR

                                if(EditorApplication.isPlaying){

                                   GUI.enabled = false; 

                                //isPlaying
                                } else {

                                    if(compSave.compDataName == ""){

                                        GUI.enabled = false;

                                    //compDataName == null
                                    } else {

                                        GUI.enabled = true; 

                                    }//compDataName == null

                                }//isPlaying

                            #endif

                            if(GUILayout.Button("Clear Components Data")){

                                CompData_Clear();

                            }//Button

                            GUILayout.Space(5);

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        //compVersion != null
                        } else {

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Components Save uses Components Version to keep track of the last Components version save files were saved in." + "\n" + "\n" + "Use the button below to catch the Components Version."  + "\n", MessageType.Warning);

                            EditorGUILayout.Space();

                            if(GUILayout.Button("Catch Components Version")){

                                compSave.Version_Find();

                            }//Button

                        }//compVersion != null

                    }//fileTabs = components

                    if(compSave.fileTabs == 1){

                        GUILayout.Space(20);

                        compSave.clearSavesOnPlayStop = EditorGUILayout.Toggle("Clear Saves on Stop?", compSave.clearSavesOnPlayStop);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Use the button below to clear HFPS save files (in editor only)" + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        #if UNITY_EDITOR

                            if(EditorApplication.isPlaying){

                                GUI.enabled = false; 

                            //isPlaying
                            } else {

                                GUI.enabled = true; 

                            }//isPlaying

                        #endif

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        EditorGUILayout.Space();

                        if(GUILayout.Button("Clear HFPS Saves")){

                            Saves_Clear();

                        }//Button

                        GUILayout.Space(5);

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    }//fileTabs = HFPS

                }//fileOpts

            }//tabs = user options

            if(compSave.tabs == 1){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These events are triggered when save or load is called." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(events, true);

                EditorGUILayout.Space();

            }//tabs = events

            if(compSave.tabs == 2){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("Displays debug logs in the console if ON.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.LabelField("Debug Notifications", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                compSave.debugInt = GUILayout.Toolbar(compSave.debugInt, new string[] { "OFF", "ON" });

                if(compSave.debugInt == 0){

                    compSave.useDebug = false;

                }//debugInt == 0

                if(compSave.debugInt == 1){

                    compSave.useDebug = true;

                }//debugInt == 1

                GUILayout.Space(15);

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(compVersionRef, true);
                EditorGUILayout.PropertyField(saveData, true);
                compSave.loading = EditorGUILayout.Toggle("Loading?", compSave.loading);
                compSave.locked = EditorGUILayout.Toggle("Locked?", compSave.locked);

            }//tabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(compSave);

                if(!EditorApplication.isPlaying){

                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                }//!isPlaying

            }//changed

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

        }//OnInspectorGUI


    //////////////////////////
    //
    //      EDITOR ACTIONS
    //
    //////////////////////////


        public void CompData_Clear(){

            compSave.Reset_CompData(false);

        }//CompData_Clear

        public void Saves_Clear(){

            compSave.Reset_Saves();

        }//Saves_Clear


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


    }//HFPS_CompSaveEditor


}//namespace