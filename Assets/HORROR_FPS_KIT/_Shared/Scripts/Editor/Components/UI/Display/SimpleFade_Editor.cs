using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.Shared {

    [CustomEditor(typeof(SimpleFade))]
    public class SimpleFade_Editor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        SimpleFade simpFade;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            simpFade = (SimpleFade)target;

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

            GUILayout.BeginHorizontal("Simple Fade", "HeaderText");

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

            simpFade.simpFadeTabs = GUILayout.SelectionGrid(simpFade.simpFadeTabs, new string[] { "User Options", "References", "Auto"}, 3);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            SerializedProperty initState = serializedObject.FindProperty("initState");
            SerializedProperty animType = serializedObject.FindProperty("animType");
            SerializedProperty audClip = serializedObject.FindProperty("audClip");

            SerializedProperty fadeInString = serializedObject.FindProperty("fadeInString");
            SerializedProperty fadeOutString = serializedObject.FindProperty("fadeOutString");

            SerializedProperty fadeAnim = serializedObject.FindProperty("fadeAnim");
            SerializedProperty audSource = serializedObject.FindProperty("audSource");

            if(simpFade.simpFadeTabs == 0){

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Click the toggles below to show the options for each section." + "\n", MessageType.Info);

                }//showTips

                EditorGUILayout.Space();

                simpFade.startOpts = GUILayout.Toggle(simpFade.startOpts, "Start Options", GUI.skin.button);

                if(simpFade.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The initial state of the fade on start." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(initState, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Uses a delay before the system starts if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.Space();

                    simpFade.initWait = EditorGUILayout.Toggle("Init Wait?", simpFade.initWait);

                    if(simpFade.initWait){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before the system starts." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        simpFade.waitTime = EditorGUILayout.FloatField("Wait Time", simpFade.waitTime);

                    }//initWait

                }//startOpts

                EditorGUILayout.Space();

                simpFade.animOpts = GUILayout.Toggle(simpFade.animOpts, "Animation Options", GUI.skin.button);

                if(simpFade.animOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The type of animation trigger." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(animType, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The animation name used to fade in." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(fadeInString, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The animation name used to fade out." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(fadeOutString, true);

                }//animOpts

                EditorGUILayout.Space();

                simpFade.soundOpts = GUILayout.Toggle(simpFade.soundOpts, "Sound Options", GUI.skin.button);

                if(simpFade.soundOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Uses a sound effect when the fade occurs." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    simpFade.useFadeSound = EditorGUILayout.Toggle("Use Fade Sound?", simpFade.useFadeSound);

                    if(simpFade.useFadeSound){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The sound effect played when fadding occurs." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(audClip, true);
                        simpFade.audVol = EditorGUILayout.FloatField("Audio Volume", simpFade.audVol);

                    }//useFadeSound

                }//soundOpts

            }//simpFadeTabs = user options

            if(simpFade.simpFadeTabs == 1){

                EditorGUILayout.Space();

                simpFade.audioRefs = GUILayout.Toggle(simpFade.audioRefs, "Audio", GUI.skin.button);

                if(simpFade.audioRefs){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The AudioSource used for fade sounds." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(audSource, new GUIContent("Audio Source"), true);

                }//audioRefs

                EditorGUILayout.Space();

                simpFade.dispRefs = GUILayout.Toggle(simpFade.dispRefs, "Display", GUI.skin.button);

                if(simpFade.dispRefs){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The animator used for fades." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(fadeAnim, true);

                }//dispRefs

            }//simpFadeTabs = references

            if(simpFade.simpFadeTabs == 2){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("Displays debug logs in the console if ON.", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.LabelField("Debug Notifications", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                simpFade.debugInt = GUILayout.Toolbar(simpFade.debugInt, new string[] { "OFF", "ON" });

                if(simpFade.debugInt == 0){

                    simpFade.useDebug = false;

                }//debugInt == 0

                if(simpFade.debugInt == 1){

                    simpFade.useDebug = true;

                }//debugInt == 1

                GUILayout.Space(15);

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                simpFade.init = EditorGUILayout.Toggle("Init", simpFade.init);
                simpFade.locked = EditorGUILayout.Toggle("Locked?", simpFade.locked);

            }//simpFadeTabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(simpFade);

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


    }//SimpleFade_Editor


}//namespace