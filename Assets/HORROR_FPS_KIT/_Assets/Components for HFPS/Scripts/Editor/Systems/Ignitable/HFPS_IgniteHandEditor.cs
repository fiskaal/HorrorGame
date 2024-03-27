using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_IgniteHand))]
    public class HFPS_IgniteHandEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_IgniteHand igniteHand;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            igniteHand = (HFPS_IgniteHand)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty detectTag = serializedObject.FindProperty("detectTag");

            SerializedProperty source = serializedObject.FindProperty("source");
            SerializedProperty igniteClip = serializedObject.FindProperty("igniteClip");

            SerializedProperty tempIgnite = serializedObject.FindProperty("tempIgnite");

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

            GUILayout.BeginHorizontal("Ignite Handler", "HeaderText");

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

            igniteHand.tabs = GUILayout.SelectionGrid(igniteHand.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(igniteHand.tabs == 0){

                EditorGUILayout.Space();

                igniteHand.genOpts = GUILayout.Toggle(igniteHand.genOpts, "General Options", GUI.skin.button);

                if(igniteHand.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The detection tag used for ignitables." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(detectTag, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The ignite rate applied to ignitable times." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    igniteHand.igniteRate = EditorGUILayout.FloatField("Ignite Rate", igniteHand.igniteRate);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The ignite multiplier applied to the ignite rate." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    igniteHand.igniteMulti = EditorGUILayout.FloatField("Ignite Multiplier", igniteHand.igniteMulti);

                }//genOpts

                EditorGUILayout.Space();

                igniteHand.audioOpts = GUILayout.Toggle(igniteHand.audioOpts, "Audio Options", GUI.skin.button);

                if(igniteHand.audioOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE plays a sound during ignite." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    igniteHand.useIgniteSound = EditorGUILayout.Toggle("Use Ignite Sound?", igniteHand.useIgniteSound);

                    if(igniteHand.useIgniteSound){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Source used for playing the ignite sound." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(source, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Clip used for playing the ignite sound." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(igniteClip, true);

                    }//useIgniteSound

                }//audioOpts

            }//tabs = user options

            if(igniteHand.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(tempIgnite, true);

                EditorGUILayout.Space();

                igniteHand.isLighting = EditorGUILayout.Toggle("Is Lighting?", igniteHand.isLighting);
                igniteHand.tempIgniteTime = EditorGUILayout.FloatField("Temp Ignite Time", igniteHand.tempIgniteTime);

                EditorGUILayout.Space();

                igniteHand.locked = EditorGUILayout.Toggle("Locked?", igniteHand.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(igniteHand);

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


    }//HFPS_IgniteHandEditor


}//namespace