using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_AudioFader))]
    public class HFPS_AudioFaderEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_AudioFader audFade;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            audFade = (HFPS_AudioFader)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty ambience = serializedObject.FindProperty("ambience");
            SerializedProperty music = serializedObject.FindProperty("music");

            SerializedProperty fadeType = serializedObject.FindProperty("fadeType");
            SerializedProperty curSource = serializedObject.FindProperty("curSource");
            SerializedProperty tempClip = serializedObject.FindProperty("tempClip");

            SerializedProperty oldAmbClip = serializedObject.FindProperty("oldAmbClip");
            SerializedProperty oldMusicClip = serializedObject.FindProperty("oldMusicClip");

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

            GUILayout.BeginHorizontal("Audio Fader", "HeaderText");

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

            audFade.tabs = GUILayout.SelectionGrid(audFade.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(audFade.tabs == 0){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Multiplier applied to fade time." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                audFade.fadeMulti = EditorGUILayout.FloatField("Fade Multiplier", audFade.fadeMulti);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Audio source reference for ambience source." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(ambience, true);

                if(showTips){

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Audio source reference for music source." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(music, true);

            }//tabs = user options

            if(audFade.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(fadeType, true);
                EditorGUILayout.PropertyField(curSource, true);
                EditorGUILayout.PropertyField(tempClip, true);

                audFade.tempVolume = EditorGUILayout.FloatField("Temp Volume", audFade.tempVolume);
                audFade.tempKeep = EditorGUILayout.Toggle("Temp Keep?", audFade.tempKeep);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(oldAmbClip, true);
                EditorGUILayout.PropertyField(oldMusicClip, true);

                EditorGUILayout.Space();

                audFade.oldAmbVolume = EditorGUILayout.FloatField("Old Amb Volume", audFade.oldAmbVolume);
                audFade.oldMusicVolume = EditorGUILayout.FloatField("Old Music Volume", audFade.oldMusicVolume);

                EditorGUILayout.Space();

                audFade.isFading = EditorGUILayout.Toggle("Is Fading?", audFade.isFading);

                EditorGUILayout.Space();

                audFade.ambienceIsPlaying = EditorGUILayout.Toggle("Ambience Is Playing?", audFade.ambienceIsPlaying);
                audFade.musicIsPlaying = EditorGUILayout.Toggle("Music Is Playing?", audFade.musicIsPlaying);

                EditorGUILayout.Space();

                audFade.ambienceWasPlaying = EditorGUILayout.Toggle("Ambience Was Playing?", audFade.ambienceWasPlaying);
                audFade.musicWasPlaying = EditorGUILayout.Toggle("Music Was Playing?", audFade.musicWasPlaying);

                EditorGUILayout.Space();

                audFade.ambienceDoneFading = EditorGUILayout.Toggle("Ambience Done Fading?", audFade.ambienceDoneFading);
                audFade.musicDoneFading = EditorGUILayout.Toggle("Music Done Fading?", audFade.musicDoneFading);

                EditorGUILayout.Space();

                audFade.revert = EditorGUILayout.Toggle("Revert?", audFade.revert);
                audFade.locked = EditorGUILayout.Toggle("Locked?", audFade.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(audFade);

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


    }//HFPS_AudioFaderEditor


}//namespace