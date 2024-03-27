using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using DizzyMedia.Shared;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_Actions))]
    public class HFPS_ActionsEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_Actions hfpsActs;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsActs = (HFPS_Actions)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty tempAction = serializedObject.FindProperty("tempAction");
            SerializedProperty actionsRef = serializedObject.FindProperty("actions");
            SerializedProperty tempType = serializedObject.FindProperty("tempType");
            SerializedProperty tempName = serializedObject.FindProperty("auto.tempName");

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

            GUILayout.BeginHorizontal("Actions", "HeaderText");

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

            hfpsActs.tabs = GUILayout.SelectionGrid(hfpsActs.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(hfpsActs.tabs == 0){

                EditorGUILayout.Space();

                hfpsActs.genOpts = GUILayout.Toggle(hfpsActs.genOpts, "General Options", GUI.skin.button);

                if(hfpsActs.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Waits before initializing start if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsActs.useStartDelay = EditorGUILayout.Toggle("Use Start Delay?", hfpsActs.useStartDelay);

                    if(hfpsActs.useStartDelay){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before start is called." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsActs.startWait = EditorGUILayout.FloatField("Start Wait", hfpsActs.startWait);

                    }//useStartDelay

                }//genOpts

                EditorGUILayout.Space();

                hfpsActs.actOpts = GUILayout.Toggle(hfpsActs.actOpts, "Actions Options", GUI.skin.button);

                if(hfpsActs.actOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Camera states which can be called to." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(actionsRef, true);

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();

                    #if UNITY_EDITOR

                        if(EditorApplication.isPlaying){

                           GUI.enabled = false; 

                        //isPlaying
                        } else {

                            GUI.enabled = true;

                        }//isPlaying

                    #endif

                    if(GUILayout.Button("Find Actions")) {

                        Actions_Find();

                    }//Button

                    #if UNITY_EDITOR

                        if(EditorApplication.isPlaying){

                           GUI.enabled = false; 

                        //isPlaying
                        } else {

                            if(hfpsActs.actions.Count > 0){

                                GUI.enabled = true;

                            //actions.Count > 0
                            } else {

                                GUI.enabled = false;

                            }//actions.Count > 0

                        }//isPlaying

                    #endif

                    if(GUILayout.Button("Clear Actions")) {

                        Actions_Clear();

                    }//Button

                    EditorGUILayout.EndHorizontal();

                    GUI.enabled = true;

                }//actOpts

            }//tabs = user options

            if(hfpsActs.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(tempAction, true);
                EditorGUILayout.PropertyField(tempType, true);

                EditorGUILayout.Space();

                hfpsActs.autoFoldShow = EditorGUILayout.Foldout(hfpsActs.autoFoldShow, "Auto");

                if(hfpsActs.autoFoldShow){

                    EditorGUILayout.PropertyField(tempName, new GUIContent("ActionName"), true);
                    hfpsActs.auto.tempRadius = EditorGUILayout.FloatField("Old Radius", hfpsActs.auto.tempRadius);
                    hfpsActs.auto.tempItem = EditorGUILayout.IntField("Current Item", hfpsActs.auto.tempItem);

                    EditorGUILayout.Space();

                    hfpsActs.auto.tempPauseLock = EditorGUILayout.Toggle("Pause Lock", hfpsActs.auto.tempPauseLock);
                    hfpsActs.auto.tempInvLock = EditorGUILayout.Toggle("Inventory Lock", hfpsActs.auto.tempInvLock);
                    hfpsActs.auto.tempSaveState = EditorGUILayout.Toggle("Save State", hfpsActs.auto.tempSaveState);
                    hfpsActs.auto.tempLoadState = EditorGUILayout.Toggle("Load State", hfpsActs.auto.tempLoadState);

                    EditorGUILayout.Space();

                    hfpsActs.auto.tempLockMoveX = EditorGUILayout.Toggle("Lock Move X", hfpsActs.auto.tempLockMoveX);
                    hfpsActs.auto.tempLockMoveY = EditorGUILayout.Toggle("Lock Move Y", hfpsActs.auto.tempLockMoveY);
                    hfpsActs.auto.tempLockJump = EditorGUILayout.Toggle("Lock Jump", hfpsActs.auto.tempLockJump);
                    hfpsActs.auto.tempLockStateInput = EditorGUILayout.Toggle("Lock State Input", hfpsActs.auto.tempLockStateInput);

                    hfpsActs.auto.tempLockLean = EditorGUILayout.Toggle("Lock Lean", hfpsActs.auto.tempLockLean);
                    hfpsActs.auto.tempLockZoom = EditorGUILayout.Toggle("Lock Zoom", hfpsActs.auto.tempLockZoom);
                    hfpsActs.auto.tempLockWeapZoom = EditorGUILayout.Toggle("Lock Weap Zoom", hfpsActs.auto.tempLockWeapZoom);
                    hfpsActs.auto.tempLockItemUse = EditorGUILayout.Toggle("Lock Item Use", hfpsActs.auto.tempLockItemUse);
                    hfpsActs.auto.tempLockItemSwitch = EditorGUILayout.Toggle("Lock Item Switch", hfpsActs.auto.tempLockItemSwitch);

                    EditorGUILayout.Space();

                    hfpsActs.locked = EditorGUILayout.Toggle("Locked?", hfpsActs.locked);

                }//autoFoldShow

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsActs);

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


        public void Actions_Find(){

            var allMonoBehaviours = FindObjectsOfType<HFPS_CharacterAction>(true);

            if(allMonoBehaviours.Length > 0){

                Debug.Log("Actions Found");

                for(int i = 0; i < allMonoBehaviours.Length; i++){

                    HFPS_Actions.Action tempAction = new HFPS_Actions.Action();

                    tempAction.action = allMonoBehaviours[i];

                    if(hfpsActs.actions.Count > 0){

                        if(!Action_Check(tempAction.action)){

                            hfpsActs.actions.Add(tempAction);

                            //Debug.Log(tempAction.action.name + " Not Present");

                        //!Action_Check
                        } else {

                            //Debug.Log(tempAction.action.name + " Present");

                        }//!Action_Check

                    //actions.Count > 0
                    } else {

                        hfpsActs.actions.Add(tempAction);

                    }//actions.Count > 0

                }//for i allMonoBehaviours

                EditorUtility.SetDirty(hfpsActs);

            }//allMonoBehaviours.Length > 0

        }//Actions_Find

        private bool Action_Check(HFPS_CharacterAction newAction){

            for(int a = 0; a < hfpsActs.actions.Count; a++){

                if(hfpsActs.actions[a].action == newAction){

                    return true;

                }//action != newAction

            }//for a actions

            return false;

        }//Action_Check

        public void Actions_Clear(){

            hfpsActs.actions.Clear();

            hfpsActs.actions = new List<HFPS_Actions.Action>();

            EditorUtility.SetDirty(hfpsActs);

        }//Actions_Clear


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


    }//HFPS_ActionsEditor


}//namespace