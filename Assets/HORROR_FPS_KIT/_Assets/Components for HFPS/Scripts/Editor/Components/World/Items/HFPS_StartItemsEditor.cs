using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_StartItems))]
    public class HFPS_StartItemsEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_StartItems hfpsStart;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsStart = (HFPS_StartItems)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty weapons = serializedObject.FindProperty("weapons");
            SerializedProperty items = serializedObject.FindProperty("items");
            SerializedProperty itemsRemove = serializedObject.FindProperty("itemsRemove");

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

            GUILayout.BeginHorizontal("Start Items", "HeaderText");

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

            hfpsStart.tabs = GUILayout.SelectionGrid(hfpsStart.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(hfpsStart.tabs == 0){

                EditorGUILayout.Space();

                hfpsStart.startOpts = GUILayout.Toggle(hfpsStart.startOpts, "Start Options", GUI.skin.button);

                if(hfpsStart.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE uses a delay before adding start items." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsStart.addDelay = EditorGUILayout.Toggle("Add Delay?", hfpsStart.addDelay);

                    if(hfpsStart.addDelay){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before start items are added." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsStart.delayWait = EditorGUILayout.FloatField("Delay Wait", hfpsStart.delayWait);

                    }//addDelay

                }//startOpts

                EditorGUILayout.Space();

                hfpsStart.itemOpts = GUILayout.Toggle(hfpsStart.itemOpts, "Item Options", GUI.skin.button);

                if(hfpsStart.itemOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Clears the players inventory on start if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsStart.clearInventory = EditorGUILayout.Toggle("Clear Inventory?", hfpsStart.clearInventory);

                    EditorGUILayout.Space();

                    hfpsStart.addItemsOpts = GUILayout.Toggle(hfpsStart.addItemsOpts, "Add Items", GUI.skin.button);

                    if(hfpsStart.addItemsOpts){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Weapon Item options for start items addition." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(weapons, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Item options for start items addition." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(items, true);

                    }//addItemsOpts

                    if(!hfpsStart.clearInventory){

                        EditorGUILayout.Space();

                        hfpsStart.removeItemsOpts = GUILayout.Toggle(hfpsStart.removeItemsOpts, "Remove Items", GUI.skin.button);

                        if(hfpsStart.removeItemsOpts){

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Items to remove on start." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(itemsRemove, new GUIContent("Items"), true);

                        }//removeItemsOpts

                    }//!clearInventory

                }//itemOpts

            }//tabs = user options

            if(hfpsStart.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                hfpsStart.present = EditorGUILayout.Toggle("Present?", hfpsStart.present);
                hfpsStart.locked = EditorGUILayout.Toggle("Locked?", hfpsStart.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsStart);

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


    }//HFPS_StartItemsEditor


}//namespace