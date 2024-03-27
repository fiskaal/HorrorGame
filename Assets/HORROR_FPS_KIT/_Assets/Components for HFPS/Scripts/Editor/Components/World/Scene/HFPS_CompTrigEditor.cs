using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_CompTrig))]
    public class HFPS_CompTrigEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_CompTrig compTrig;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            compTrig = (HFPS_CompTrig)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty triggerTypeRef = serializedObject.FindProperty("triggerType");
            SerializedProperty healUse = serializedObject.FindProperty("healUse");
            SerializedProperty useTypeRef = serializedObject.FindProperty("useType");
            SerializedProperty trigger = serializedObject.FindProperty("trigger");
            SerializedProperty hfpsPlayerAttention = serializedObject.FindProperty("hfpsPlayerAttention");
            SerializedProperty hfpsPossessed = serializedObject.FindProperty("hfpsPossessed");
            SerializedProperty hfpsDestroyable = serializedObject.FindProperty("hfpsDestroyable");
            SerializedProperty detectTag = serializedObject.FindProperty("detectTag");
            SerializedProperty unlockTypeRef = serializedObject.FindProperty("unlockType");

            SerializedProperty hideTypeRef = serializedObject.FindProperty("hideType");
            SerializedProperty timeCheckType = serializedObject.FindProperty("timeCheckType");

            SerializedProperty pauseStateEnter = serializedObject.FindProperty("pauseStateEnter");
            SerializedProperty pauseStateExit = serializedObject.FindProperty("pauseStateExit");

            SerializedProperty inventoryStateEnter = serializedObject.FindProperty("inventoryStateEnter");
            SerializedProperty inventoryStateExit = serializedObject.FindProperty("inventoryStateExit");

            SerializedProperty saveStateEnter = serializedObject.FindProperty("saveStateEnter");
            SerializedProperty saveStateExit = serializedObject.FindProperty("saveStateExit");

            SerializedProperty loadStateEnter = serializedObject.FindProperty("loadStateEnter");
            SerializedProperty loadStateExit = serializedObject.FindProperty("loadStateExit");

            SerializedProperty onManualTrigger = serializedObject.FindProperty("onManualTrigger");
            SerializedProperty onTriggerEnter = serializedObject.FindProperty("onTriggerEnter");
            SerializedProperty onTriggerExit = serializedObject.FindProperty("onTriggerExit");
            SerializedProperty onHeal = serializedObject.FindProperty("onHeal");
            SerializedProperty onReset = serializedObject.FindProperty("onReset");
            SerializedProperty refsCatch = serializedObject.FindProperty("refsCatch");
            SerializedProperty timedEvents = serializedObject.FindProperty("timedEvents");

            SerializedProperty refs = serializedObject.FindProperty("refs");

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

            GUILayout.BeginHorizontal("Component Trigger", "headerText_Small");

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

            compTrig.tabs = GUILayout.SelectionGrid(compTrig.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            if(compTrig.tabs == 0){

                EditorGUILayout.Space();

                compTrig.genOpts = GUILayout.Toggle(compTrig.genOpts, "General Options", GUI.skin.button);

                if(compTrig.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets how many times this this trigger works." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(useTypeRef, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The type of trigger that will occur" + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(triggerTypeRef, true);

                    if(compTrig.useType == HFPS_CompTrig.Use_Type.Manual){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Uses a delay before trigger Manaul events if TRUE" + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        compTrig.useTriggerDelay = EditorGUILayout.Toggle("Use Trigger Delay?", compTrig.useTriggerDelay);

                        if(compTrig.useTriggerDelay){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The wait time before the Manaul event will be triggered." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            compTrig.triggerDelay = EditorGUILayout.FloatField("Trigger Delay", compTrig.triggerDelay);

                        }//useTriggerDelay

                    }//useType = manual

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerHeal && compTrig.useType != HFPS_CompTrig.Use_Type.Manual){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Sets when heal is activated." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(healUse, true);

                    }//triggerType = player heal & useType != manual

                    if(compTrig.useType == HFPS_CompTrig.Use_Type.Single | compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The tag for the player." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(detectTag, true);

                    }//useType = single or multi

                    if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                        if(compTrig.triggerType != HFPS_CompTrig.Trigger_Type.PlayerHide){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The type of trigger unlock to occur (if any)" + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(unlockTypeRef, true);

                            if(compTrig.unlockType == HFPS_CompTrig.Unlock_Type.Delayed){

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "The wait time before unlock occurs." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                compTrig.unlockDelay = EditorGUILayout.FloatField("Unlock Delay", compTrig.unlockDelay);

                            }//unlockType = delayed

                        }//triggerType != player hide

                    }//useType = multi

                    if(compTrig.triggerType != HFPS_CompTrig.Trigger_Type.PlayerHide){

                        if(compTrig.useType == HFPS_CompTrig.Use_Type.Single){

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "The trigger used." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(trigger, true);

                        }//useType = single

                    //triggerType != player hide
                    } else {

                        if(compTrig.useType == HFPS_CompTrig.Use_Type.Single | compTrig.hideType == HFPS_CompTrig.HideType.TimeLimited){

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "The trigger used." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(trigger, true);

                        }//hideType = time limited

                    }//triggerType != player hide

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.Destroyable){

                        if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                            EditorGUILayout.Space();

                        }//useType = multi

                        EditorGUILayout.PropertyField(hfpsDestroyable, new GUIContent("Destroyable"), true);

                    }//triggerType = destroyable

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerAttention){

                        if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                            EditorGUILayout.Space();

                        }//useType = multi

                        EditorGUILayout.PropertyField(hfpsPlayerAttention, new GUIContent("Player Attention"), true);

                    }//triggerType = player attention

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerHide){

                        if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                            EditorGUILayout.Space();

                        }//useType = multi

                        EditorGUILayout.PropertyField(hideTypeRef, true);

                        if(compTrig.hideType == HFPS_CompTrig.HideType.TimeLimited){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "How time will be checked." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(timeCheckType, true);

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Time before trigger area activates." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            compTrig.activeTime = EditorGUILayout.FloatField("Active Time", compTrig.activeTime);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Multiplier used for reducing active time." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            compTrig.countDownMulti = EditorGUILayout.FloatField("Count Down Multi", compTrig.countDownMulti);

                            EditorGUILayout.Space();

                            if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                                if(showTips){

                                    EditorGUILayout.HelpBox("\n" + "Auto resets after a period of time if TRUE." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                compTrig.autoReset = EditorGUILayout.Toggle("Auto Reset?", compTrig.autoReset);

                                if(compTrig.autoReset){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "Time to wait before resetting trigger." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    compTrig.resetWait = EditorGUILayout.FloatField("Reset Wait", compTrig.resetWait);

                                }//autoReset

                            }//useType = multi

                        }//hideType = time limited

                    }//triggerType = player hide

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.Possessed){

                        if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                            EditorGUILayout.Space();

                        }//useType = multi

                        EditorGUILayout.PropertyField(hfpsPossessed, new GUIContent("Possessed"), true);

                    }//triggerType = possessed

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.Destroyable | compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerDamage){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The amount of damage to send." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        compTrig.damageAmount = EditorGUILayout.IntField("Damage Amount", compTrig.damageAmount);

                    }//triggerType = destroyable or player damage

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.Save){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Is this save occuring right before a new scene is loaded?" + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        compTrig.preLoadScene = EditorGUILayout.Toggle("Pre Load Scene?", compTrig.preLoadScene);

                    }//triggerType = save

                    if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.ScreenEvent){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Category slot for screen events." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        compTrig.category = EditorGUILayout.IntField("Category", compTrig.category);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Event slots for screen events." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        compTrig.eventSlot = EditorGUILayout.IntField("Event Slot", compTrig.eventSlot);

                    }//triggerType = screen event

                }//genOpts

                EditorGUILayout.Space();

                compTrig.uiOpts = GUILayout.Toggle(compTrig.uiOpts, "UI Options", GUI.skin.button);

                if(compTrig.uiOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the pause input lock state on trigger enter." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(pauseStateEnter, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets the pause input lock state on trigger exit." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(pauseStateExit, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the inventory input lock state on trigger enter." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(inventoryStateEnter, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets the inventory input lock state on trigger exit." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(inventoryStateExit, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the save button in pause UI interaction state on trigger enter." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(saveStateEnter, true);

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the save button in pause UI interaction state on trigger exit." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(saveStateExit, true);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets the load button in pause UI interaction state on trigger enter" + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(loadStateEnter, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Sets the load button in pause UI interaction state on trigger exit." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(loadStateExit, true);

                }//uiOpts

            }//tabs = user options

            if(compTrig.tabs == 1){

                EditorGUILayout.Space();

                if(compTrig.useType == HFPS_CompTrig.Use_Type.Manual){

                    EditorGUILayout.PropertyField(onManualTrigger, true);

                }//useType = manual

                if(compTrig.useType != HFPS_CompTrig.Use_Type.Manual){

                    EditorGUILayout.PropertyField(onTriggerEnter, true);
                    EditorGUILayout.PropertyField(onTriggerExit, true);

                }//useType != manual

                if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerHeal){

                    EditorGUILayout.PropertyField(onHeal, true);

                }//triggerType = player heal

                if(compTrig.useType == HFPS_CompTrig.Use_Type.Multi){

                    if(compTrig.triggerType != HFPS_CompTrig.Trigger_Type.PlayerHide){

                        EditorGUILayout.PropertyField(onReset, true);

                    }//triggerType != player hide

                }//useType = multi

                if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerHide){

                    EditorGUILayout.PropertyField(timedEvents, true);

                }//triggerType = player hide

                EditorGUILayout.PropertyField(refsCatch, new GUIContent("References Catch"), true);

            }//tabs = events

            if(compTrig.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(refs, new GUIContent("References"), true);

                if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerHeal){

                    compTrig.healing = EditorGUILayout.Toggle("Healing?", compTrig.healing);

                }//triggerType = player heal

                if(compTrig.triggerType == HFPS_CompTrig.Trigger_Type.PlayerHide && compTrig.hideType == HFPS_CompTrig.HideType.TimeLimited){

                    compTrig.countDownActive = EditorGUILayout.Toggle("Count Down Active?", compTrig.countDownActive);
                    compTrig.currentTime = EditorGUILayout.FloatField("Current Time", compTrig.currentTime);

                }//triggerType = player hide

                compTrig.isInside = EditorGUILayout.Toggle("Is Inside?", compTrig.isInside);
                compTrig.locked = EditorGUILayout.Toggle("Locked?", compTrig.locked);

            }//tabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(compTrig);

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


    }//HFPS_CompTrigEditor


}//namespace