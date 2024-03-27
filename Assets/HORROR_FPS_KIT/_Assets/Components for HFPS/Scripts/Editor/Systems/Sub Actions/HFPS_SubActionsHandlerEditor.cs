using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_SubActionsHandler))]
    public class HFPS_SubActionsHandlerEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_SubActionsHandler subActsHand;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            subActsHand = (HFPS_SubActionsHandler)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty refs = serializedObject.FindProperty("refs");

            SerializedProperty subActions = serializedObject.FindProperty("subActions");
            SerializedProperty actionInputs = serializedObject.FindProperty("actionInputs");

            SerializedProperty inputTypeRef = serializedObject.FindProperty("inputType");

            SerializedProperty playerInput = serializedObject.FindProperty("playerInput");

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

            GUILayout.BeginHorizontal("Sub Actions Handler", "HeaderText_Small");

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

            subActsHand.tabs = GUILayout.SelectionGrid(subActsHand.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(subActsHand.tabs == 0){

                EditorGUILayout.Space();

                subActsHand.genOpts = GUILayout.Toggle(subActsHand.genOpts, "General Options", GUI.skin.button);

                if(subActsHand.genOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(refs, new GUIContent("References"), true);

                }//genOpts

                EditorGUILayout.Space();

                subActsHand.actOpts = GUILayout.Toggle(subActsHand.actOpts, "Actions Options", GUI.skin.button);

                if(subActsHand.actOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sub action options for each action used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(subActions, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Inputs used for sub actions." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(actionInputs, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE uses a delay before sub action starts." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    subActsHand.useActionDelay = EditorGUILayout.Toggle("Use Action Delay?", subActsHand.useActionDelay);

                    if(subActsHand.useActionDelay){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before the sub action starts." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        subActsHand.actionDelay = EditorGUILayout.FloatField("Action Delay", subActsHand.actionDelay);

                    }//useActionDelay

                }//actOpts

                EditorGUILayout.Space();

                subActsHand.inputOpts = GUILayout.Toggle(subActsHand.inputOpts, "Input Options", GUI.skin.button);

                if(subActsHand.inputOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Type of input detection to use." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(inputTypeRef, true);

                    if(subActsHand.inputType == HFPS_SubActionsHandler.Input_Type.Hold){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Input press time before action occurs." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        subActsHand.holdTime = EditorGUILayout.FloatField("Hold Time", subActsHand.holdTime);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Multiplier used for input hold time." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        subActsHand.holdMulti = EditorGUILayout.FloatField("Hold Multi", subActsHand.holdMulti);

                    }//inputType = hold

                }//inputOpts

            }//tabs = user options

            if(subActsHand.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(playerInput, true);

                EditorGUILayout.Space();

                subActsHand.curAction = EditorGUILayout.IntField("Current Action", subActsHand.curAction);
                subActsHand.tempInt = EditorGUILayout.IntField("TempInt", subActsHand.tempInt);

                EditorGUILayout.Space();

                subActsHand.isHolding = EditorGUILayout.Toggle("Is Holding?", subActsHand.isHolding);
                subActsHand.curHoldTime = EditorGUILayout.FloatField("Cur Hold Time", subActsHand.curHoldTime);
                subActsHand.tempFill = EditorGUILayout.FloatField("Temp Fill", subActsHand.tempFill);

                EditorGUILayout.Space();

                subActsHand.locked = EditorGUILayout.Toggle("Locked?", subActsHand.locked);
                subActsHand.lockDelay = EditorGUILayout.FloatField("Lock Delay", subActsHand.lockDelay);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(subActsHand);

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


    }//HFPS_SubActionsHandlerEditor


}//namespace