using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_PlayerAttention))]
    public class HFPS_PlayerAttentionEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_PlayerAttention playAttent;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            playAttent = (HFPS_PlayerAttention)target;

        }//OnEnable


        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty useType = serializedObject.FindProperty("useType");
            SerializedProperty attentionTypeRef = serializedObject.FindProperty("attentionType");
            SerializedProperty attentionStopTypeRef = serializedObject.FindProperty("attentionStopType");

            SerializedProperty jumpInput = serializedObject.FindProperty("jumpInput");
            SerializedProperty stateInput = serializedObject.FindProperty("stateInput");

            SerializedProperty leanInput = serializedObject.FindProperty("leanInput");
            SerializedProperty sprintInput = serializedObject.FindProperty("sprintInput");
            SerializedProperty zoomInput = serializedObject.FindProperty("zoomInput");

            SerializedProperty itemInputLock = serializedObject.FindProperty("itemInputLock");
            SerializedProperty itemUseLock = serializedObject.FindProperty("itemUseLock");
            SerializedProperty itemZoomLock = serializedObject.FindProperty("itemZoomLock");
            SerializedProperty itemDispStart = serializedObject.FindProperty("itemDispStart");
            SerializedProperty itemDispEnd = serializedObject.FindProperty("itemDispEnd");

            SerializedProperty actions = serializedObject.FindProperty("actions");
            SerializedProperty startState = serializedObject.FindProperty("startState");
            SerializedProperty endState = serializedObject.FindProperty("endState");
            SerializedProperty playerUnlockRef = serializedObject.FindProperty("playerUnlock");
            SerializedProperty sceneAction = serializedObject.FindProperty("sceneAction");
            SerializedProperty actionBarSettings = serializedObject.FindProperty("actionBarSettings");
            SerializedProperty events = serializedObject.FindProperty("events");
            SerializedProperty inputType = serializedObject.FindProperty("inputType");
            SerializedProperty refs = serializedObject.FindProperty("refs");
            SerializedProperty actionBar = serializedObject.FindProperty("actionBar");
            
            SerializedProperty displaySet = serializedObject.FindProperty("displaySet");

            SerializedProperty gameUIStateStart = serializedObject.FindProperty("gameUIStateStart");
            SerializedProperty gameUIStateEnd = serializedObject.FindProperty("gameUIStateEnd");

            SerializedProperty pauseStateStartRef = serializedObject.FindProperty("pauseStateStart");
            SerializedProperty pauseStateEnd = serializedObject.FindProperty("pauseStateEnd");

            SerializedProperty inventoryStateStart = serializedObject.FindProperty("inventoryStateStart");
            SerializedProperty inventoryStateEnd = serializedObject.FindProperty("inventoryStateEnd");

            SerializedProperty saveStateStart = serializedObject.FindProperty("saveStateStart");
            SerializedProperty saveStateEnd = serializedObject.FindProperty("saveStateEnd");

            SerializedProperty loadStateStart = serializedObject.FindProperty("loadStateStart");
            SerializedProperty loadStateEnd = serializedObject.FindProperty("loadStateEnd");

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

            GUILayout.BeginHorizontal("Player Attention", "headerText_Small");

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

            playAttent.tabs = GUILayout.SelectionGrid(playAttent.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            if(playAttent.tabs == 0){

                EditorGUILayout.Space();

                playAttent.genOpts = GUILayout.Toggle(playAttent.genOpts, "General Options", GUI.skin.button);

                if(playAttent.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "How this attention will be used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(useType, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Type of attention that will be used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(attentionTypeRef, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets how the attention stops." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(attentionStopTypeRef, true);

                    if(playAttent.attentionStopType == HFPS_PlayerAttention.AttentionStop_Type.Auto){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The time in which the attention lasts for." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        playAttent.attentionTime = EditorGUILayout.FloatField("Attention Time", playAttent.attentionTime);

                    }//attentionStopType = stop

                }//genOpts

                EditorGUILayout.Space();

                playAttent.moveOpts = GUILayout.Toggle(playAttent.moveOpts, "Move Options", GUI.skin.button);

                if(playAttent.moveOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Actions used for this attention." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(actions, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets how player unlock occurs." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(playerUnlockRef, true);

                    if(playAttent.playerUnlock == HFPS_PlayerAttention.Player_Unlock.Auto){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before player unlock occurs." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        playAttent.unlockWait = EditorGUILayout.FloatField("Unlock Wait", playAttent.unlockWait);

                    }//playerUnlock = auto

                }//moveOpts

                EditorGUILayout.Space();

                playAttent.playOpts = GUILayout.Toggle(playAttent.playOpts, "Player Options", GUI.skin.button);

                if(playAttent.playOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE handles mouse display based on input type (i.e show / hide)" + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    playAttent.showMouse = EditorGUILayout.Toggle("Show Mouse?", playAttent.showMouse);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets if Jump input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(jumpInput, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets if lean input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(leanInput, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets if sprint input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(sprintInput, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets if Character State input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(stateInput, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets if zoom input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(zoomInput, true);

                }//playOpts

                EditorGUILayout.Space();

                playAttent.itemOpts = GUILayout.Toggle(playAttent.itemOpts, "Item Options", GUI.skin.button);

                if(playAttent.itemOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets if item switch input is locked during attention." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemInputLock, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets if item use input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemUseLock, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets if item zoom input is locked during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemZoomLock, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The item switch action used for attention start." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemDispStart, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The item switch action used for attention end." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemDispEnd, true);

                }//itemOpts

                EditorGUILayout.Space();

                playAttent.uiOpts = GUILayout.Toggle(playAttent.uiOpts, "UI Options", GUI.skin.button);

                if(playAttent.uiOpts){

                    if(playAttent.gameUIStateStart == HFPS_PlayerAttention.Action_State.Nothing && playAttent.gameUIStateEnd == HFPS_PlayerAttention.Action_State.Nothing){
                    
                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Will use display set assigned on UI Controller instead of hiding / showing the entire Game UI" + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                        
                        playAttent.useDisplaySet = EditorGUILayout.Toggle("Use Display Set?", playAttent.useDisplaySet);

                        if(playAttent.useDisplaySet){

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Name of the display set group to show / hide." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(displaySet, true);
                    
                        }//useDisplaySet
                    
                    }//gameUIStateStart & gameUIStateEnd = nothing

                    if(!playAttent.useDisplaySet){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets the Game UI active state on action start." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(gameUIStateStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets the Game UI active state on action end." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(gameUIStateEnd, true);
                    
                    }//!useDisplayState
                    
                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the pause input lock state on action start." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(pauseStateStartRef, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets the pause input lock state on action end." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(pauseStateEnd, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the inventory input lock state on action start." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(inventoryStateStart, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets the inventory input lock state on action end." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(inventoryStateEnd, true);

                    if(playAttent.pauseStateStart != HFPS_PlayerAttention.Action_State.Disable){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets the save button in pause UI interaction state on attention start." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(saveStateStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets the save button in pause UI interaction state on attention end." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(saveStateEnd, true);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets the load button in pause UI interaction state on action start." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(loadStateStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets the load button in pause UI interaction state on action end." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(loadStateEnd, true);

                    }//pauseStateStart != disable

                }//uiOpts

                EditorGUILayout.Space();

                playAttent.extOpts = GUILayout.Toggle(playAttent.extOpts, "Extensions", GUI.skin.button);

                if(playAttent.extOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE uses a Scene Action during attention." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    playAttent.useSceneAction = EditorGUILayout.Toggle("Use Scene Action?", playAttent.useSceneAction);

                    if(playAttent.useSceneAction){

                        EditorGUILayout.PropertyField(sceneAction, true);

                    }//useSceneAction

                    if(playAttent.attentionType == HFPS_PlayerAttention.Attention_Type.Input){

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(actionBarSettings, true);

                    }//attentionType = input

                }//extOpts

            }//tabs = user options

            if(playAttent.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(events, true);

            }//tabs = events

            if(playAttent.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(inputType, true);
                EditorGUILayout.PropertyField(refs, new GUIContent("References"), true);

                if(playAttent.attentionType == HFPS_PlayerAttention.Attention_Type.Input){

                    EditorGUILayout.PropertyField(actionBar, true);
                    playAttent.actBarShowing = EditorGUILayout.Toggle("ActionBar Showing?", playAttent.actBarShowing);

                }//attentionType = input

                playAttent.currentItem = EditorGUILayout.IntField("Current Item", playAttent.currentItem);
                playAttent.locked = EditorGUILayout.Toggle("Locked?", playAttent.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(playAttent);

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


    }//HFPS_PlayerAttentionEditor


}//namespace