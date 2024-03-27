using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using HFPS.Player;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_DualWield))]
    public class HFPS_DualWieldEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_DualWield hfpsDualWield;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsDualWield = (HFPS_DualWield)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty incompIDs = serializedObject.FindProperty("incompIDs");

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

            GUILayout.BeginHorizontal("Dual Wield", "HeaderText");

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

            hfpsDualWield.tabs = GUILayout.SelectionGrid(hfpsDualWield.tabs, new string[] { "User Options", "Helpers"}, 2);

            if(hfpsDualWield.tabs == 0){

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "If TRUE this item can be used with dual wield." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                hfpsDualWield.dualWield = EditorGUILayout.Toggle("Can Dual Wield?", hfpsDualWield.dualWield);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Incompatible item ID's with this item." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(incompIDs, new GUIContent("Incompatible ID's"), true);

            }//tabs = user options

            if(hfpsDualWield.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("\n" + "You can use the helpers below to make automatic updates to your player." + "\n", MessageType.Info);

                EditorGUILayout.Space();

                #if COMPONENTS_PRESENT

                    #if UNITY_EDITOR

                        if(EditorApplication.isPlaying){

                            GUI.enabled = false;

                        //isPlaying
                        } else {

                            GUI.enabled = true;

                        }//isPlaying

                    #endif

                    if(GUILayout.Button("Add THIS To Dual Wield List")){

                        DualWield_Add(true);

                    }//button

                    EditorGUILayout.Space();

                    if(GUILayout.Button("Add ALL To Dual Wield List")){

                        DualWield_Add(false);

                    }//button

                #else 

                    EditorGUILayout.HelpBox("\n" + "Components is NOT active!" + "\n", MessageType.Error);

                #endif

            }//tabs = helpers

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsDualWield);

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


        private void DualWield_Add(bool single){

            #if COMPONENTS_PRESENT

                if(single){

                    bool present = false;

                    var itemSwitch = FindObjectsOfType<ItemSwitcher>();

                    if(itemSwitch != null){

                        if(itemSwitch[0].dualWields.Count > 0){

                            if(!itemSwitch[0].dualWields.Contains(hfpsDualWield)){

                                itemSwitch[0].dualWields.Add(hfpsDualWield);

                                present = false;

                            //!Contains
                            } else {

                                present = true;

                            }//!Contains

                        //dualWields.Count > 0
                        } else {

                            itemSwitch[0].dualWields.Add(hfpsDualWield);

                            present = false;

                        }//dualWields.Count > 0

                        if(!present){

                            Debug.Log(hfpsDualWield.name + " Added to Dual Wield List");

                        //!present
                        } else {

                            Debug.Log(hfpsDualWield.name + " Already Present");

                        }//!present

                    }//itemSwitch != null

                //single
                } else {

                    bool updateList = false;

                    var itemSwitch = FindObjectsOfType<ItemSwitcher>();

                    if(itemSwitch != null){

                        if(itemSwitch[0].ItemList.Count != itemSwitch[0].dualWields.Count){

                            updateList = true;

                        //ItemList.Count != dualWields.Count
                        } else {

                            int trueCount = 0;

                            for(int i = 0; i < itemSwitch[0].ItemList.Count; i++){

                                if(itemSwitch[0].ItemList[i].name == itemSwitch[0].dualWields[i].name){

                                    trueCount += 1;

                                }//name == name

                            }//for i ItemList

                            if(trueCount == itemSwitch[0].ItemList.Count){

                                Debug.Log("Dual Wield List already matches Item List");

                            //trueCount = ItemList.Count
                            } else {

                                updateList = true;

                            }//trueCount = ItemList.Count

                        }//ItemList.Count != dualWields.Count

                        if(updateList){

                            Debug.Log("Dual Wield List does NOT match Item List, Updating...");

                            itemSwitch[0].dualWields = new List<HFPS_DualWield>();

                            for(int i = 0; i < itemSwitch[0].ItemList.Count; i++){

                                if(itemSwitch[0].ItemList[i].GetComponent<HFPS_DualWield>()){

                                    if(!itemSwitch[0].dualWields.Contains(itemSwitch[0].ItemList[i].GetComponent<HFPS_DualWield>())){

                                        itemSwitch[0].dualWields.Add(itemSwitch[0].ItemList[i].GetComponent<HFPS_DualWield>());

                                        Debug.Log(itemSwitch[0].ItemList[i].name + " Added to Dual Wield List");

                                    }//!Contains

                                }//HFPS_DualWield

                            }//for i playerItems

                        }//updateList

                    }//itemSwitch != null

                }//single

            #endif

        }//DualWield_Add


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


    }//HFPS_DualWieldEditor


}//namespace