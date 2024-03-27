using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_RadialDetect))]
    public class HFPS_RadialDetectEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_RadialDetect hfpsRadial;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsRadial = (HFPS_RadialDetect)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty radialAttributeRef = serializedObject.FindProperty("radialAttribute");
            SerializedProperty playerTag = serializedObject.FindProperty("playerTag");
            SerializedProperty npcTag = serializedObject.FindProperty("npcTag");

            SerializedProperty tempDest = serializedObject.FindProperty("tempDest");

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

            GUILayout.BeginHorizontal("Radial Detect", "HeaderText");

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

            hfpsRadial.tabs = GUILayout.SelectionGrid(hfpsRadial.tabs, new string[] { "User Options"}, 1);

            if(hfpsRadial.tabs == 0){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "The type of detection used." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(radialAttributeRef, true);

                if(hfpsRadial.radialAttribute == HFPS_RadialDetect.RadialAttribute.OnlyPlayer | hfpsRadial.radialAttribute == HFPS_RadialDetect.RadialAttribute.Everything){

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The tag used for the player." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(playerTag, true);

                }//radialAttribute = only player or everything

                if(hfpsRadial.radialAttribute == HFPS_RadialDetect.RadialAttribute.OnlyNPC | hfpsRadial.radialAttribute == HFPS_RadialDetect.RadialAttribute.Everything){

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The tag used for the player." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(npcTag, true);

                }//radialAttribute = only npc or everything

                if(hfpsRadial.radialAttribute != HFPS_RadialDetect.RadialAttribute.None){

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The amount of damage to send." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsRadial.damageAmount = EditorGUILayout.IntField("Damage Amount", hfpsRadial.damageAmount);

                }//radialAttribute != none

            }//tabs = user options

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsRadial);

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


    }//HFPS_RadialDetectEditor


}//namespace