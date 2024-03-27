using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_Ignitable))]
    public class HFPS_IgnitableEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_Ignitable ignitable;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            ignitable = (HFPS_Ignitable)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty igniteTypeRef = serializedObject.FindProperty("igniteType");
            SerializedProperty trigger = serializedObject.FindProperty("trigger");

            SerializedProperty showOnLit = serializedObject.FindProperty("showOnLit");

            SerializedProperty itemID = serializedObject.FindProperty("itemID");

            SerializedProperty interactTypeRef = serializedObject.FindProperty("interactType");
            SerializedProperty itemTypeRef = serializedObject.FindProperty("itemType");
            SerializedProperty itemUseType = serializedObject.FindProperty("itemUseType");

            SerializedProperty selectText = serializedObject.FindProperty("selectText");
            SerializedProperty emptyText = serializedObject.FindProperty("emptyText");
            SerializedProperty wrongItemText = serializedObject.FindProperty("wrongItemText");

            SerializedProperty audSource = serializedObject.FindProperty("audSource");
            SerializedProperty audClip = serializedObject.FindProperty("audClip");

            SerializedProperty onCorrectItem = serializedObject.FindProperty("onCorrectItem");
            SerializedProperty onIncorrectItem = serializedObject.FindProperty("onIncorrectItem");

            SerializedProperty onLit = serializedObject.FindProperty("onLit");
            SerializedProperty onLitLoad = serializedObject.FindProperty("onLitLoad");

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

            GUILayout.BeginHorizontal("Ignitable", "HeaderText");

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

            ignitable.tabs = GUILayout.SelectionGrid(ignitable.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            if(ignitable.tabs == 0){

                EditorGUILayout.Space();

                ignitable.genOpts = GUILayout.Toggle(ignitable.genOpts, "General Options", GUI.skin.button);

                if(ignitable.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The type of ignite interaction to use." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(igniteTypeRef, true);

                    if(ignitable.igniteType == HFPS_Ignitable.Ignite_Type.Trigger){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Collider reference used for interaction." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(trigger, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Time before ignite / lit occurs." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        ignitable.igniteTime = EditorGUILayout.FloatField("Ignite Time", ignitable.igniteTime);

                    }//igniteType = trigger

                    if(ignitable.igniteType == HFPS_Ignitable.Ignite_Type.Manual){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Collider reference used for interaction." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(trigger, true);

                        if(ignitable.gameObject.GetComponent<InteractEvents>() == null || ignitable.gameObject.GetComponent<UIObjectInfo>() == null){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Ignite Type > Manual requires you to make a manual call to Interaction_Init" + "\n" + "\n" + "Use the button below to update this ignitables interaction." + "\n", MessageType.Warning);

                            EditorGUILayout.Space();

                            if(GUILayout.Button("Update Interaction")){

                                if(EditorUtility.DisplayDialog("Update Interaction Type?", "You are about to update the setup on this ignitable interaction for MANUAL usage." + "\n" + "\n" + "Are you sure?", "Yes", "No")){

                                    Interaction_Update();

                                }//DisplayDialog

                            }//Button

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        }//InteractEvents = null & UIObjectInfo = null

                    }//igniteType = manual

                }//genOpts

                EditorGUILayout.Space();

                ignitable.igniteOpts = GUILayout.Toggle(ignitable.igniteOpts, "Ignite Options", GUI.skin.button);

                if(ignitable.igniteOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Objects that are shown when ignite is called." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(showOnLit, true);

                }//igniteOpts

                if(ignitable.igniteType == HFPS_Ignitable.Ignite_Type.Manual){

                    EditorGUILayout.Space();

                    ignitable.itemOpts = GUILayout.Toggle(ignitable.itemOpts, "Item Options", GUI.skin.button);

                    if(ignitable.itemOpts){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Requires an item for interaction IF true." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        ignitable.requireItem = EditorGUILayout.Toggle("Require Item?", ignitable.requireItem);

                        if(ignitable.requireItem){

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "The item to detect for interaction." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(itemID, true);

                        }//requireItem

                    }//itemOpts

                    if(ignitable.requireItem){

                        EditorGUILayout.Space();

                        ignitable.interactOpts = GUILayout.Toggle(ignitable.interactOpts, "Interaction Options", GUI.skin.button);

                        if(ignitable.interactOpts){

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Checks how the player will interact with the ignitable." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(interactTypeRef, true);
                            EditorGUILayout.PropertyField(itemTypeRef, true);

                            if(ignitable.itemType == HFPS_Ignitable.Item_Type.Regular){

                                EditorGUILayout.PropertyField(itemUseType, true);

                            }//itemType = regular

                            if(ignitable.itemType == HFPS_Ignitable.Item_Type.Switcher){

                                if(!ignitable.detectItemShowing){

                                    EditorGUILayout.PropertyField(itemUseType, true);

                                }//detectItemShowing

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Requires switcher item to be showing to trigger interaction IF true." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                ignitable.detectItemShowing = EditorGUILayout.Toggle("Detect Item Showing?", ignitable.detectItemShowing);

                                if(ignitable.detectItemShowing){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "The switcher slot to be detected (i.e 0, 1, 2, etc.)" + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    ignitable.switcherSlot = EditorGUILayout.IntField("Switcher Slot", ignitable.switcherSlot);

                                }//detectItemShowing

                            }//itemType = switcher

                            if(ignitable.detectItemShowing && ignitable.interactType == HFPS_Ignitable.Interact_Type.OpenInventory && ignitable.itemType == HFPS_Ignitable.Item_Type.Switcher){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Detect Item Show does not work with Open Inventory interaction, the interaction will use Auto Detect instead." + "\n", MessageType.Warning);

                            }//detectItemShowing & interactType = open inventory

                            EditorGUILayout.Space();

                            if(ignitable.interactType == HFPS_Ignitable.Interact_Type.OpenInventory){

                                if(ignitable.itemType == HFPS_Ignitable.Item_Type.Regular){

                                    if(showTips){

                                        EditorGUILayout.HelpBox("\n" + "Text to be used when displaying inventory select." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    EditorGUILayout.PropertyField(selectText, true);

                                }//itemType = regular

                                if(ignitable.itemType == HFPS_Ignitable.Item_Type.Switcher){

                                    if(!ignitable.detectItemShowing){

                                        if(showTips){

                                            EditorGUILayout.HelpBox("\n" + "Text to be used when displaying inventory select." + "\n", MessageType.Info);

                                            EditorGUILayout.Space();

                                        }//showTips

                                        EditorGUILayout.PropertyField(selectText, true);

                                    }//detectItemShowing

                                }//itemType = switcher

                            }//interactType = open inventory

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Text to be used when player does not have the item(s) required." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(emptyText, true);

                            if(ignitable.itemType == HFPS_Ignitable.Item_Type.Switcher){

                                if(ignitable.detectItemShowing){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "Text to be used when the required switcher item is not showing." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    EditorGUILayout.PropertyField(wrongItemText, true);

                                }//detectItemShowing

                            }//itemType = switcher

                        }//interactOpts

                    }//requireItem

                }//igniteType = manual

                EditorGUILayout.Space();

                ignitable.soundOpts = GUILayout.Toggle(ignitable.soundOpts, "Sound Options", GUI.skin.button);

                if(ignitable.soundOpts){

                    EditorGUILayout.Space();

                    ignitable.useLitSound = EditorGUILayout.Toggle("Use Lit Sound?", ignitable.useLitSound);

                    if(ignitable.useLitSound){

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(audSource, true);
                        EditorGUILayout.PropertyField(audClip, true);

                    }//useLitSound

                }//soundOpts

            }//tabs = user options

            if(ignitable.tabs == 1){

                EditorGUILayout.Space();

                if(ignitable.igniteType == HFPS_Ignitable.Ignite_Type.Manual){

                    if(ignitable.requireItem){

                        EditorGUILayout.PropertyField(onCorrectItem, true);

                        if(ignitable.itemType == HFPS_Ignitable.Item_Type.Switcher){

                            if(ignitable.detectItemShowing){

                                EditorGUILayout.PropertyField(onIncorrectItem, true);

                            }//detectItemShowing

                        }//itemType = switcher

                    }//requireItem

                }//igniteType = manual

                EditorGUILayout.PropertyField(onLit, true);
                EditorGUILayout.PropertyField(onLitLoad, true);

            }//tabs = events

            if(ignitable.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                ignitable.isLit = EditorGUILayout.Toggle("Is Lit?", ignitable.isLit);
                ignitable.locked = EditorGUILayout.Toggle("Locked?", ignitable.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(ignitable);

                if(!EditorApplication.isPlaying){

                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                }//!isPlaying

            }//changed

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

        }//OnInspectorGUI


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    ///////////////////////////////////////


        private void Interaction_Update(){

            if(ignitable.gameObject.GetComponent<InteractEvents>() == null){

                InteractEvents tempEvents = ignitable.gameObject.AddComponent<InteractEvents>();

                if(tempEvents != null){

                    Debug.Log("Interact Events Created");

                    tempEvents.InteractType = InteractEvents.Type.Event;
                    tempEvents.RepeatMode = InteractEvents.Repeat.MoreTimes;

                    tempEvents.InteractEvent = new UnityEvent();

                    UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvents.InteractEvent, ignitable.Interaction_Init);

                    Debug.Log("Interact Events Updated");

                }//tempEvents != null

            //InteractEvents = null
            } else {

                Debug.Log("Interaction Events already present!");

            }//InteractEvents = null

            if(ignitable.gameObject.GetComponent<UIObjectInfo>() == null){

                UIObjectInfo tempInfo = ignitable.gameObject.AddComponent<UIObjectInfo>();

                if(tempInfo != null){

                    Debug.Log("UIObjectInfo Added");

                    tempInfo.ObjectTitle = "";
                    tempInfo.UseText = "Light";
                    tempInfo.DynamicTrue = "";
                    tempInfo.DynamicFalse = "";

                    Debug.Log("UIObjectInfo Updated");

                }//tempInfo != null

            //UIObjectInfo = null
            } else {

                Debug.Log("UIObjectInfo already present!");

            }//UIObjectInfo = null

            ignitable.gameObject.tag = "Untagged";
            ignitable.gameObject.layer = 9;

            Debug.Log("Ignitable Tag and Layer Updated");

        }//Interaction_Update


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


    }//HFPS_IgnitableEditor


}//namespace