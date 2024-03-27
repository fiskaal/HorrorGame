using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.Shared {

    [CustomEditor(typeof(HFPS_CharacterAction))]
    public class HFPS_CharacterActionEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_CharacterAction hfpsCharAct;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsCharAct = (HFPS_CharacterAction)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty startTypeRef = serializedObject.FindProperty("startOptions.startType");
            SerializedProperty startEndTypeRef = serializedObject.FindProperty("startOptions.startEndType");
            SerializedProperty startUse = serializedObject.FindProperty("startOptions.startUse");

            SerializedProperty startState = serializedObject.FindProperty("startOptions.startState");
            SerializedProperty startPosition = serializedObject.FindProperty("startOptions.startPosition");

            SerializedProperty handler = serializedObject.FindProperty("userOptions.handler");

            SerializedProperty actionType = serializedObject.FindProperty("userOptions.actionType");
            SerializedProperty actionAttribute = serializedObject.FindProperty("userOptions.actionAttribute");
            SerializedProperty useType = serializedObject.FindProperty("userOptions.useType");
            SerializedProperty endTypeRef = serializedObject.FindProperty("userOptions.endType");

            SerializedProperty teleportTypeRef = serializedObject.FindProperty("teleportType");
            SerializedProperty transferTypeRef = serializedObject.FindProperty("transferType");
            SerializedProperty saveState = serializedObject.FindProperty("saveState");
            SerializedProperty loadState = serializedObject.FindProperty("loadState");
            SerializedProperty transferAction = serializedObject.FindProperty("transferAction");
            SerializedProperty teleportTrans = serializedObject.FindProperty("teleportTrans");
            SerializedProperty transferActionNameRef = serializedObject.FindProperty("transferActionName");
            SerializedProperty levelNameRef = serializedObject.FindProperty("levelName");

            SerializedProperty interactTypeRef = serializedObject.FindProperty("interactType");
            SerializedProperty interactObj = serializedObject.FindProperty("references.interactObj");
            SerializedProperty interactCol = serializedObject.FindProperty("references.interactCol");
            SerializedProperty dynamObj = serializedObject.FindProperty("references.dynamObj");
            SerializedProperty actionParent = serializedObject.FindProperty("references.actionParent");
            SerializedProperty endTrigger = serializedObject.FindProperty("references.endTrigger");

            SerializedProperty interactFront = serializedObject.FindProperty("references.interactFront");
            SerializedProperty interactBack = serializedObject.FindProperty("references.interactBack");

            SerializedProperty lookLock = serializedObject.FindProperty("userOptions.lookLock");
            SerializedProperty moveLock = serializedObject.FindProperty("userOptions.moveLock");
            SerializedProperty lookOptions = serializedObject.FindProperty("lookOptions");

            SerializedProperty actionsStart = serializedObject.FindProperty("actionsStart");
            SerializedProperty actionsEnd = serializedObject.FindProperty("actionsEnd");

            SerializedProperty actionsFrontStart = serializedObject.FindProperty("actionsFrontStart");
            SerializedProperty actionsFrontEnd = serializedObject.FindProperty("actionsFrontEnd");
            SerializedProperty actionsBackStart = serializedObject.FindProperty("actionsBackStart");
            SerializedProperty actionsBackEnd = serializedObject.FindProperty("actionsBackEnd");

            SerializedProperty xInputRef = serializedObject.FindProperty("playerOptions.xInput");
            SerializedProperty xLimitRef = serializedObject.FindProperty("playerOptions.xLimit");
            SerializedProperty xLimitDir = serializedObject.FindProperty("playerOptions.xLimitDir");

            SerializedProperty yInputRef = serializedObject.FindProperty("playerOptions.yInput");
            SerializedProperty yLimitRef = serializedObject.FindProperty("playerOptions.yLimit");
            SerializedProperty yLimitDir = serializedObject.FindProperty("playerOptions.yLimitDir");

            SerializedProperty jumpInput = serializedObject.FindProperty("playerOptions.jumpInput");
            SerializedProperty stateInput = serializedObject.FindProperty("playerOptions.stateInput");

            SerializedProperty leanInput = serializedObject.FindProperty("playerOptions.leanInput");
            SerializedProperty leanLock = serializedObject.FindProperty("playerOptions.leanLock");

            SerializedProperty sprintInput = serializedObject.FindProperty("playerOptions.sprintInput");
            SerializedProperty sprintLock = serializedObject.FindProperty("playerOptions.sprintLock");

            SerializedProperty zoomInput = serializedObject.FindProperty("playerOptions.zoomInput");
            SerializedProperty zoomLock = serializedObject.FindProperty("playerOptions.zoomLock");

            SerializedProperty endTrigger_Front = serializedObject.FindProperty("references.endTrigger_Front");
            SerializedProperty endTrigger_Back = serializedObject.FindProperty("references.endTrigger_Back");

            SerializedProperty exitBlock_Front = serializedObject.FindProperty("references.exitBlock_Front");
            SerializedProperty exitBlock_Back = serializedObject.FindProperty("references.exitBlock_Back");

            SerializedProperty itemUseLock = serializedObject.FindProperty("userOptions.itemDisplay.itemUseLock");
            SerializedProperty itemZoomLock = serializedObject.FindProperty("userOptions.itemDisplay.itemZoomLock");
            SerializedProperty itemSwitchLock = serializedObject.FindProperty("userOptions.itemDisplay.itemSwitchLock");
            SerializedProperty itemDispEnter = serializedObject.FindProperty("userOptions.itemDisplay.itemDispEnter");
            SerializedProperty itemDispExit = serializedObject.FindProperty("userOptions.itemDisplay.itemDispExit");
            
            SerializedProperty displaySet = serializedObject.FindProperty("hfpsUI.displaySet");

            SerializedProperty gameUIStateStartRef = serializedObject.FindProperty("hfpsUI.gameUIStateStart");
            SerializedProperty gameUIStateEndRef = serializedObject.FindProperty("hfpsUI.gameUIStateEnd");

            SerializedProperty pauseStateStartRef = serializedObject.FindProperty("hfpsUI.pauseStateStart");
            SerializedProperty pauseStateEnd = serializedObject.FindProperty("hfpsUI.pauseStateEnd");

            SerializedProperty inventoryStateStart = serializedObject.FindProperty("hfpsUI.inventoryStateStart");
            SerializedProperty inventoryStateEnd = serializedObject.FindProperty("hfpsUI.inventoryStateEnd");

            SerializedProperty saveStateStart = serializedObject.FindProperty("hfpsUI.saveStateStart");
            SerializedProperty saveStateEnd = serializedObject.FindProperty("hfpsUI.saveStateEnd");

            SerializedProperty loadStateStart = serializedObject.FindProperty("hfpsUI.loadStateStart");
            SerializedProperty loadStateEnd = serializedObject.FindProperty("hfpsUI.loadStateEnd");

            SerializedProperty audioType = serializedObject.FindProperty("audioOptions.audioType");
            SerializedProperty clip = serializedObject.FindProperty("audioOptions.clip");

            SerializedProperty extensions = serializedObject.FindProperty("extensions");

            SerializedProperty startActionEvents = serializedObject.FindProperty("startActionEvents");
            SerializedProperty actionEvents = serializedObject.FindProperty("actionEvents");
            SerializedProperty actionStopEvents = serializedObject.FindProperty("actionStopEvents");

            SerializedProperty inputType = serializedObject.FindProperty("inputType");
            
            SerializedProperty gameManager = serializedObject.FindProperty("auto.gameManager");
            SerializedProperty refs = serializedObject.FindProperty("auto.refs");
            SerializedProperty uiCont = serializedObject.FindProperty("auto.uiCont");
            
            SerializedProperty actionBar = serializedObject.FindProperty("autoExtensions.actionBar");
            SerializedProperty enterDirection = serializedObject.FindProperty("auto.enterDirection");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/Utility Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != Components Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/DM_CharAct_Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Character Action", "HeaderText_Small");

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

            hfpsCharAct.tabs = GUILayout.SelectionGrid(hfpsCharAct.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            if(hfpsCharAct.tabs == 0){

                EditorGUILayout.Space();

                hfpsCharAct.startOpts = GUILayout.Toggle(hfpsCharAct.startOpts, "Start Options", GUI.skin.button);

                if(hfpsCharAct.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The type of start this action will use." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(startTypeRef, true);

                    if(hfpsCharAct.startOptions.startType == HFPS_CharacterAction.StartType.AutoStart){

                        EditorGUILayout.PropertyField(startEndTypeRef, true);
                        EditorGUILayout.PropertyField(startUse, true);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Uses a delay before action auto starts IF true." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsCharAct.startOptions.useStartDelay = EditorGUILayout.Toggle("Use Start Delay?", hfpsCharAct.startOptions.useStartDelay);    

                        if(hfpsCharAct.startOptions.useStartDelay){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Delay used to auto start the action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsCharAct.startOptions.startDelay = EditorGUILayout.FloatField("Start Delay", hfpsCharAct.startOptions.startDelay);

                        }//useStartDelay

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Delay used to end the auto start action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsCharAct.startOptions.startEndWait = EditorGUILayout.FloatField("Start End Wait", hfpsCharAct.startOptions.startEndWait);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Player state to start off at." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(startState, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Start position used on auto start action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(startPosition, true);

                    }//startType = auto start

                }//startOpts

                EditorGUILayout.Space();

                hfpsCharAct.genOpts = GUILayout.Toggle(hfpsCharAct.genOpts, "General Options", GUI.skin.button);

                if(hfpsCharAct.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The type of action used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(actionType, true);
                    
                    #if COMPONENTS_PRESENT

                        if(hfpsCharAct.userOptions.actionType != HFPS_CharacterAction.ActionType.TwoSided && hfpsCharAct.userOptions.actionType != HFPS_CharacterAction.ActionType.PushObject){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Attribute applied to action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(actionAttribute, true);

                        }//actionType != two sided
                    
                    #endif

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets use type." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(useType, true);

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.PushObject){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets use type." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(endTypeRef, true);

                        if(hfpsCharAct.userOptions.endType == HFPS_CharacterAction.EndType.Input){

                            if(!hfpsCharAct.extensions.actionBarSettings.useActionBar){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "End Type is set to Input but Action Bar is NOT enabled on this action." + "\n", MessageType.Warning);

                            }//useActionBar

                        }//endType = input

                    }//actionType = push object

                    if(hfpsCharAct.userOptions.actionAttribute == HFPS_CharacterAction.ActionAttribute.Teleport){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The type of teleportation to use." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(teleportTypeRef, true);

                        if(hfpsCharAct.teleportType == HFPS_CharacterAction.Teleport_Type.Local){

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "The character action this action will transfer to." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(transferAction, true);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The transform / position used when teleporting to this character action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(teleportTrans, true);

                        }//teleportType = local

                        if(hfpsCharAct.teleportType == HFPS_CharacterAction.Teleport_Type.Level){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The type of transfer to occur" + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(transferTypeRef, true);

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "If Active a pre scene load save will occur." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(saveState, true);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The type of HFPS scene load to use." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(loadState, true);

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "The transform / position used when teleporting to this character action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(teleportTrans, true);

                            if(hfpsCharAct.transferType == HFPS_CharacterAction.Transfer_Type.ActionToLevel){

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "The name of the action in the new scene to transfer this action to (i.e name listed in Actions component)" + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(transferActionNameRef, new GUIContent("Action Name"), true);

                                if(hfpsCharAct.transferActionName == ""){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("Transfer Type > Action To Level requirs an Action Name.", MessageType.Warning);

                                    EditorGUILayout.Space();

                                }//levelName = null

                            }//transferType = action to level

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The name of the new level to load." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(levelNameRef, true);

                            if(hfpsCharAct.levelName == ""){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("Teleport Type > Level requirs a Level Name.", MessageType.Warning);

                                EditorGUILayout.Space();

                            }//levelName = null

                        }//teleportType = level

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The wait time before teleport begins." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsCharAct.teleportWait = EditorGUILayout.FloatField("Teleport Wait", hfpsCharAct.teleportWait);

                    }//actionType = teleport

                    #if PUZZLER_PRESENT

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Checks the complete state of a Puzzler Handler if TRUE." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsCharAct.userOptions.linkComplete = EditorGUILayout.Toggle("Link Complete State?", hfpsCharAct.userOptions.linkComplete);

                        if(hfpsCharAct.userOptions.linkComplete){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Puzzler Handler to check complete state from." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(handler, true);

                        }//linkComplete

                    #endif

                }//genOpts

                EditorGUILayout.Space();

                hfpsCharAct.interOpts = GUILayout.Toggle(hfpsCharAct.interOpts, "Interaction Options", GUI.skin.button);

                if(hfpsCharAct.interOpts){

                    EditorGUILayout.Space();

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.General){
                    
                        #if COMPONENTS_PRESENT
                    
                            if(!hfpsCharAct.extensions.actionBarSettings.useActionBar && !hfpsCharAct.extensions.subActionSettings.useSubActions && hfpsCharAct.userOptions.actionAttribute != HFPS_CharacterAction.ActionAttribute.Teleport){

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "How the interaction trigger object will be handled." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(interactTypeRef, true);
                                
                                if(hfpsCharAct.interactType == HFPS_CharacterAction.InteractType.Disable){
                                
                                    EditorGUILayout.Space();
                                
                                }//interacType = disable
                                
                                if(hfpsCharAct.interactType == HFPS_CharacterAction.InteractType.ReEnable){

                                    if(showTips){
                                    
                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "The wait time before the interaction object is re-activated." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsCharAct.userOptions.interactObjWait = EditorGUILayout.FloatField("InteractObj Wait", hfpsCharAct.userOptions.interactObjWait);

                                    EditorGUILayout.Space();
                                
                                }//interactType = re-enable

                            //!useActionBar, !useSubActions & actionAttribute != teleport
                            } else {

                                if(hfpsCharAct.extensions.actionBarSettings.useActionBar && !hfpsCharAct.extensions.subActionSettings.useSubActions){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Action Bar is active, Interact Type will be handled automatically." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//useActionBar

                                if(!hfpsCharAct.extensions.actionBarSettings.useActionBar && hfpsCharAct.extensions.subActionSettings.useSubActions){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Sub Actions is active, Interact Type will be handled automatically." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//useSubActions

                                if(hfpsCharAct.extensions.actionBarSettings.useActionBar && hfpsCharAct.extensions.subActionSettings.useSubActions){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Action Bar / Sub Actions is active, Interact Type will be handled automatically." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//useActionBar & useSubActions

                            }//!useActionBar, !useSubActions & actionAttribute != teleport
                        
                        #else 
                        
                            if(!hfpsCharAct.extensions.actionBarSettings.useActionBar){
                            
                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "How the interaction trigger object will be handled." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(interactTypeRef, true);
                                
                                if(hfpsCharAct.interactType == HFPS_CharacterAction.InteractType.Disable){
                                
                                    EditorGUILayout.Space();
                                
                                }//interacType = disable
                                
                                if(hfpsCharAct.interactType == HFPS_CharacterAction.InteractType.ReEnable){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "The wait time before the interaction object is re-activated." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsCharAct.userOptions.interactObjWait = EditorGUILayout.FloatField("Interact Wait", hfpsCharAct.userOptions.interactObjWait);

                                    EditorGUILayout.Space();
                                
                                }//interactType = re-enable
                            
                            //!useActionBar
                            } else {
                            
                                if(hfpsCharAct.extensions.actionBarSettings.useActionBar){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Action Bar is active, Interact Type will be handled automatically." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//useActionBar
                            
                            }//!useActionBar
                        
                        #endif

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The interaction object used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactObj, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The interaction collider used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactCol, true);

                    }//actionType = general

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.TwoSided){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The front interaction object used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactFront, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The back interaction object used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactBack, true);

                    }//actionType = two sided

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.EnterObject){
                    
                        if(!hfpsCharAct.extensions.actionBarSettings.useActionBar){
                            
                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "How the interaction trigger object will be handled." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(interactTypeRef, true);
                            
                            if(hfpsCharAct.interactType == HFPS_CharacterAction.InteractType.Disable){
                                
                                EditorGUILayout.Space();
                                
                            }//interacType = disable
                            
                            if(hfpsCharAct.interactType == HFPS_CharacterAction.InteractType.ReEnable){

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "The wait time before the interaction object is re-activated." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                hfpsCharAct.userOptions.interactObjWait = EditorGUILayout.FloatField("Interact Wait", hfpsCharAct.userOptions.interactObjWait);

                                EditorGUILayout.Space();
                            
                            }//interactType = re-enable
                            
                        //!useActionBar
                        } else {
                            
                            if(hfpsCharAct.extensions.actionBarSettings.useActionBar){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Action Bar is active, Interact Type will be handled automatically." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//useActionBar
                            
                        }//!useActionBar

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The interaction collider used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactCol, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The dynamic object used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(dynamObj, new GUIContent("Dynamic Object"), true);

                    }//actionType = general

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.PushObject){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The interaction object used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactObj, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The interaction collider used for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactCol, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Parent transform that holds this action (if used)" + "\n", MessageType.Info);

                        }//showTips

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(actionParent, true);
                        EditorGUILayout.PropertyField(endTrigger, true);

                    }//actionType = push object

                }//interOpts

                EditorGUILayout.Space();

                hfpsCharAct.lookOpts = GUILayout.Toggle(hfpsCharAct.lookOpts, "Look Options", GUI.skin.button);

                if(hfpsCharAct.lookOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets player look use during this action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(lookLock, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Look update options used for this action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(lookOptions, true);

                }//lookOpts

                EditorGUILayout.Space();

                hfpsCharAct.moveOpts = GUILayout.Toggle(hfpsCharAct.moveOpts, "Move Options", GUI.skin.button);

                if(hfpsCharAct.moveOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets player movement input type for after action finishes." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(moveLock, true);

                    EditorGUILayout.Space();

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.TwoSided){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "End action trigger for front of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(endTrigger_Front, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "End action trigger for back of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(endTrigger_Back, true);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Movement blocking collider for front of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(exitBlock_Front, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Movement blocking collider for front of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(exitBlock_Back, true);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Start actions for front enter of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(actionsFrontStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "End actions for front exit of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(actionsFrontEnd, true);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Start actions for back enter of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(actionsBackStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "End actions for back exit of action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(actionsBackEnd, true);

                    }//actionType = two sided

                    EditorGUILayout.Space();

                    if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.General | hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.EnterObject | hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.PushObject){

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Start actions for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(actionsStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "End actions for this action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(actionsEnd, true);

                    }//actionType = general or enter object
                    
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Delays end actions if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    hfpsCharAct.userOptions.delayActionsEnd = EditorGUILayout.Toggle("Delay Actions End?", hfpsCharAct.userOptions.delayActionsEnd);
                    
                    if(hfpsCharAct.userOptions.delayActionsEnd){
                    
                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Delay time for end actions." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                    
                        hfpsCharAct.userOptions.actionEndWait = EditorGUILayout.FloatField("Action End Wait", hfpsCharAct.userOptions.actionEndWait);

                    }//delayActionsEnd
                    
                }//moveOpts

                EditorGUILayout.Space();

                hfpsCharAct.playOpts = GUILayout.Toggle(hfpsCharAct.playOpts, "Player Options", GUI.skin.button);

                if(hfpsCharAct.playOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE handles mouse display based on input type (i.e show / hide)" + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsCharAct.playerOptions.showMouse = EditorGUILayout.Toggle("Show Mouse?", hfpsCharAct.playerOptions.showMouse);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "If TRUE adjust the character controllers with during action." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsCharAct.userOptions.adjustCharCont = EditorGUILayout.Toggle("Adjust Char Cont", hfpsCharAct.userOptions.adjustCharCont);

                    if(hfpsCharAct.userOptions.adjustCharCont){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The radius to apply to the character controller." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsCharAct.userOptions.radius = EditorGUILayout.FloatField("Radius", hfpsCharAct.userOptions.radius);

                    }//adjustCharCont
                    
                    #if COMPONENTS_PRESENT

                        EditorGUILayout.Space();

                        if(hfpsCharAct.userOptions.moveLock == HFPS_CharacterAction.LockSettings.Unlock){

                            hfpsCharAct.userOptions.adjustMoveSpeed = EditorGUILayout.Toggle("Adjust Move Speed", hfpsCharAct.userOptions.adjustMoveSpeed);

                            if(hfpsCharAct.userOptions.adjustMoveSpeed){

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "The move speed applied to the player." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                hfpsCharAct.userOptions.moveSpeed = EditorGUILayout.FloatField("Move Speed", hfpsCharAct.userOptions.moveSpeed);

                            }//adjustMoveSpeed

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Sets if X input is locked during action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(xInputRef, true);

                            if(hfpsCharAct.playerOptions.xInput != HFPS_CharacterAction.Input_Lock.Lock){

                                EditorGUILayout.PropertyField(xLimitRef, true);

                                if(hfpsCharAct.playerOptions.xLimit == HFPS_CharacterAction.Input_Limit.Limit){

                                    EditorGUILayout.PropertyField(xLimitDir, new GUIContent("X Limit Direction"), true);

                                }//xLimit = limit

                            }//xInput != locked

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Sets if Y input is locked during action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(yInputRef, true);

                            if(hfpsCharAct.playerOptions.yInput != HFPS_CharacterAction.Input_Lock.Lock){

                                EditorGUILayout.PropertyField(yLimitRef, true);

                                if(hfpsCharAct.playerOptions.yLimit == HFPS_CharacterAction.Input_Limit.Limit){

                                    EditorGUILayout.PropertyField(yLimitDir, new GUIContent("Y Limit Direction"), true);

                                }//yLimit = limit

                            }//yInput != locked

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Sets if Jump input is locked during action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(jumpInput, true);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Sets if Character State input is locked during action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(stateInput, true);

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Sets if sprint input is locked during action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(sprintInput, true);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Sets if sprint input is kept locked during action." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(sprintLock, true);

                            EditorGUILayout.Space();

                        }//moveLock = unlock

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets if lean input is locked during action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(leanInput, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets if lean input is kept locked during action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(leanLock, true);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets if zoom input is locked during action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(zoomInput, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets if zoom input is kept locked during action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(zoomLock, true);
                    
                    #endif

                }//playOpts

                EditorGUILayout.Space();

                hfpsCharAct.itemOpts = GUILayout.Toggle(hfpsCharAct.itemOpts, "Item Options", GUI.skin.button);

                if(hfpsCharAct.itemOpts){
                
                    #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets if item switch input is locked during action." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(itemSwitchLock, true);
                        
                    #endif
                    
                    #if COMPONENTS_PRESENT

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
                    
                    #endif

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The item switch action used for action start." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemDispEnter, new GUIContent("Item Display Enter"), true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The item switch action used for action exit." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(itemDispExit, new GUIContent("Item Display Exit"), true);

                }//itemOpts

                EditorGUILayout.Space();

                hfpsCharAct.uiOpts = GUILayout.Toggle(hfpsCharAct.uiOpts, "UI Options", GUI.skin.button);

                if(hfpsCharAct.uiOpts){
                
                    if(hfpsCharAct.hfpsUI.gameUIStateStart == HFPS_CharacterAction.Action_State.Nothing && hfpsCharAct.hfpsUI.gameUIStateEnd == HFPS_CharacterAction.Action_State.Nothing){
                    
                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Will use display set assigned on UI Controller instead of hiding / showing the entire Game UI" + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                        
                        hfpsCharAct.hfpsUI.useDisplaySet = EditorGUILayout.Toggle("Use Display Set?", hfpsCharAct.hfpsUI.useDisplaySet);

                        if(hfpsCharAct.hfpsUI.useDisplaySet){

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Name of the display set group to show / hide." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(displaySet, true);
                    
                        }//useDisplaySet
                    
                    }//gameUIStateStart & gameUIStateEnd = nothing
                    
                    if(!hfpsCharAct.hfpsUI.useDisplaySet){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets the Game UI active state on action start." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(gameUIStateStartRef, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets the Game UI active state on action end." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(gameUIStateEndRef, true);
                    
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

                    if(hfpsCharAct.hfpsUI.pauseStateStart != HFPS_CharacterAction.Action_State.Disable){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Sets the save button in pause UI interaction state on action start." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(saveStateStart, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets the save button in pause UI interaction state on action end." + "\n", MessageType.Info);

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
                
                #if COMPONENTS_PRESENT

                    EditorGUILayout.Space();

                    hfpsCharAct.audioOpts = GUILayout.Toggle(hfpsCharAct.audioOpts, "Audio Options", GUI.skin.button);

                    if(hfpsCharAct.audioOpts){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Type of audio to play the assigned clip to." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(audioType, true);

                        if(hfpsCharAct.audioOptions.audioType != HFPS_CharacterAction.AudioType.None){

                            if(hfpsCharAct.audioOptions.audioType == HFPS_CharacterAction.AudioType.Music){

                                hfpsCharAct.audioOptions.keepAmbience = EditorGUILayout.Toggle("Keep Ambience?", hfpsCharAct.audioOptions.keepAmbience);

                            }//audioType = music

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Clip played when action is started." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(clip, true);

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Plays clip at a custom volume if TRUE." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsCharAct.audioOptions.customVolume = EditorGUILayout.Toggle("Custom Volume?", hfpsCharAct.audioOptions.customVolume);

                            if(hfpsCharAct.audioOptions.customVolume){

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Volume used for clip play." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                hfpsCharAct.audioOptions.audioVolume = EditorGUILayout.FloatField("Audio Volume", hfpsCharAct.audioOptions.audioVolume);

                            }//customVolume

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Switches the audio immediate IF true." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsCharAct.audioOptions.immediate = EditorGUILayout.Toggle("Immediate?", hfpsCharAct.audioOptions.immediate);

                        }//audioType != none

                    }//audioOpts
                
                #endif

                EditorGUILayout.Space();

                hfpsCharAct.extOpts = GUILayout.Toggle(hfpsCharAct.extOpts, "Extensions Options", GUI.skin.button);

                if(hfpsCharAct.extOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(extensions, true);

                }//extOpts

            }//tabs = user options

            if(hfpsCharAct.tabs == 1){

                EditorGUILayout.Space();

                if(hfpsCharAct.startOptions.startType == HFPS_CharacterAction.StartType.AutoStart){

                    if(hfpsCharAct.startOptions.startEndType == HFPS_CharacterAction.StartEndType.Auto | hfpsCharAct.startOptions.startEndType == HFPS_CharacterAction.StartEndType.Manual){

                        EditorGUILayout.PropertyField(startActionEvents, true);

                    }//startEndType = auto or manual

                }//startType = auto

                EditorGUILayout.PropertyField(actionEvents, true);
                EditorGUILayout.PropertyField(actionStopEvents, true);

            }//tabs = events

            if(hfpsCharAct.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(inputType, true);

                EditorGUILayout.PropertyField(gameManager, true);
                EditorGUILayout.PropertyField(refs, new GUIContent("References"), true);
                EditorGUILayout.PropertyField(uiCont, new GUIContent("UI Controller"), true);

                if(hfpsCharAct.extensions.actionBarSettings.useActionBar){

                    EditorGUILayout.PropertyField(actionBar, true);

                }//useActionBar

                if(hfpsCharAct.userOptions.actionType == HFPS_CharacterAction.ActionType.TwoSided){

                    EditorGUILayout.PropertyField(enterDirection, true);

                }//actionType = two sided

                EditorGUILayout.Space();

                if(hfpsCharAct.userOptions.adjustCharCont){

                    hfpsCharAct.auto.oldRadius = EditorGUILayout.FloatField("Old Radius", hfpsCharAct.auto.oldRadius);

                }//adjustCharCont

                hfpsCharAct.auto.currentItem = EditorGUILayout.IntField("Current Item", hfpsCharAct.auto.currentItem);

                EditorGUILayout.Space();

                if(hfpsCharAct.lookOptions.updateLookCaps){

                    if(hfpsCharAct.lookOptions.updateLookX){

                        hfpsCharAct.auto.minXOld = EditorGUILayout.FloatField("Min X Old", hfpsCharAct.auto.minXOld);
                        hfpsCharAct.auto.maxXOld = EditorGUILayout.FloatField("Max X Old", hfpsCharAct.auto.maxXOld);

                    }//updateLookX

                    if(hfpsCharAct.lookOptions.updateLookY){

                        hfpsCharAct.auto.minYOld = EditorGUILayout.FloatField("Min Y Old", hfpsCharAct.auto.minYOld);
                        hfpsCharAct.auto.maxYOld = EditorGUILayout.FloatField("Max Y Old", hfpsCharAct.auto.maxYOld);

                    }//updateLookY

                }//updateLookCaps

                EditorGUILayout.Space();

                hfpsCharAct.auto.startActionLocked = EditorGUILayout.Toggle("StartAction Locked?", hfpsCharAct.auto.startActionLocked);
                hfpsCharAct.auto.actionActive = EditorGUILayout.Toggle("Action Active?", hfpsCharAct.auto.actionActive);
                hfpsCharAct.auto.buffLock = EditorGUILayout.Toggle("Buff Lock", hfpsCharAct.auto.buffLock);
                hfpsCharAct.auto.locked = EditorGUILayout.Toggle("Locked?", hfpsCharAct.auto.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsCharAct);

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


    }//HFPS_CharacterActionEditor


}//namespace