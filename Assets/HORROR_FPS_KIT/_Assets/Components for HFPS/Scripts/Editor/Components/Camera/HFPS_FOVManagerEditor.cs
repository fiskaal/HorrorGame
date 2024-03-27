using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_FOVManager))]
    public class HFPS_FOVManagerEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_FOVManager fovMan;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            fovMan = (HFPS_FOVManager)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty states = serializedObject.FindProperty("states");
            SerializedProperty cameras = serializedObject.FindProperty("cameras");

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

            GUILayout.BeginHorizontal("FOV Manager", "HeaderText");

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

            fovMan.tabs = GUILayout.SelectionGrid(fovMan.tabs, new string[] { "User Options", "References", "Auto/Debug"}, 3);

            if(fovMan.tabs == 0){

                EditorGUILayout.Space();

                fovMan.genOpts = GUILayout.Toggle(fovMan.genOpts, "General Options", GUI.skin.button);

                if(fovMan.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Global multipliers applied to FOV actions if states are not used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    fovMan.globalZoomMulti = EditorGUILayout.FloatField("Global Zoom Multi", fovMan.globalZoomMulti);
                    fovMan.globalUnzoomMulti = EditorGUILayout.FloatField("Global UnZoom Multi", fovMan.globalUnzoomMulti);

                }//genOpts

                EditorGUILayout.Space();

                fovMan.statesOpts = GUILayout.Toggle(fovMan.statesOpts, "States Options", GUI.skin.button);

                if(fovMan.statesOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Camera states which can be called to." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(states, true);

                }//statesOpts

            }//tabs = user options

            if(fovMan.tabs == 1){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "All cameras currently used by the player." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(cameras, true);

            }//tabs = references

            if(fovMan.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                fovMan.tempWait = EditorGUILayout.FloatField("Temp Wait", fovMan.tempWait);
                fovMan.tempFOV = EditorGUILayout.FloatField("Temp FOV", fovMan.tempFOV);
                fovMan.oldFOV = EditorGUILayout.FloatField("Old FOV", fovMan.oldFOV);
                fovMan.tempZoomMulti = EditorGUILayout.FloatField("Temp Zoom Multi", fovMan.tempZoomMulti);

                EditorGUILayout.Space();

                fovMan.zoomIN = EditorGUILayout.Toggle("Zoom IN?", fovMan.zoomIN);
                fovMan.zoomOUT = EditorGUILayout.Toggle("Zoom OUT?", fovMan.zoomOUT);
                fovMan.zooming = EditorGUILayout.Toggle("Zooming?", fovMan.zooming);

                EditorGUILayout.Space();

                fovMan.locked = EditorGUILayout.Toggle("Locked?", fovMan.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(fovMan);

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


    }//HFPS_FOVManagerEditor


}//namespace