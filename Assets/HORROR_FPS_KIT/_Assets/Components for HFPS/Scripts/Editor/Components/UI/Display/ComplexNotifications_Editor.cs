using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(ComplexNotifications))]
    public class ComplexNotifications_Editor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        ComplexNotifications compNotif;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            compNotif = (ComplexNotifications)target;

        }//OnEnable

        public override void OnInspectorGUI() { 

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15);

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

            GUILayout.BeginHorizontal("Complex Notifications", "HeaderText_Small");

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

            compNotif.tabs = GUILayout.SelectionGrid(compNotif.tabs, new string[] { "User Options", "References", "Auto"}, 3);

            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            SerializedProperty notifications = serializedObject.FindProperty("notifications");
            SerializedProperty audSource = serializedObject.FindProperty("audSource");
            
            SerializedProperty tempTexts = serializedObject.FindProperty("tempTexts");

            if(compNotif.tabs == 0){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Setup your notifications below using the options available." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips 

                EditorGUILayout.PropertyField(notifications, true);

            }//tabs = User Options

            if(compNotif.tabs == 1){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Fill in the references for the components/systems that you want to use." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(audSource, true);

            }//tabs = References

            if(compNotif.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                compNotif.curNotif = EditorGUILayout.IntField("Cur Notification", compNotif.curNotif);
                EditorGUILayout.PropertyField(tempTexts, true);
                
                EditorGUILayout.Space();

                compNotif.tempWait = EditorGUILayout.FloatField("Temp Wait", compNotif.tempWait);
                compNotif.tempHideWait = EditorGUILayout.FloatField("Temp Hide Wait", compNotif.tempHideWait);
                compNotif.tempVolume = EditorGUILayout.FloatField("Temp Volume", compNotif.tempVolume);
                compNotif.locked = EditorGUILayout.Toggle("Locked?", compNotif.locked);

            }//tabs = Auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(compNotif);

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


    }//ComplexNotifications_Editor


}//namespace