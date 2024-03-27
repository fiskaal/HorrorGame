using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.IO;

using DizzyMedia.HFPS_Components;
using DizzyMedia.Version;

namespace DizzyMedia.Extension {

    public class Comp_SysCheck : EditorWindow {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

        GUISkin oldSkin;

        private static Comp_SysCheck window;
        private static Vector2 windowsSize = new Vector2(430, 500);

        private static DM_Version dmVersion;
        private static string versionName = "SysCheck Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        private static DM_Version compVersion;
        private static string compVersionName = "Components Version";
        private static string compVerNumb = "";
        private static bool compVersCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        private bool useDebug = false;
        private int debugInt = 0;

        private bool useAdvDebug = false;
        private int advDebugInt = 0;

        private bool systemsChecked;
        private bool foundCompData = false;
        private string saveValidStatus = "";
        private string compSaveStatus = "";

        private static HFPS_CompSave compSave;

        Vector2 scrollPos;
        Vector2 save_ScrollPos;

        int tabs;
        int tabsSaves;


    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Helpers/Systems/Systems Check", false , 0)]
        private static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<Comp_SysCheck>(false, "Systems Check" + " v" + verNumb, true);
                window.maxSize = window.minSize = windowsSize;

            }//dmVersion == null

            if(dmMenusLocData == null){

                languageLock = false;
                DM_LocDataFind();

            //dmMenusLocData = null
            } else {

                language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

            }//dmMenusLocData = null

        }//OpenWizard

        public void OpenWizard_Single(){

            OpenWizard();

        }//OpenWizard_Single

        private void OnGUI() {

            CheckForSystems();
            SystemsCheck_Screen();

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        private void SystemsCheck_Screen(){

            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};

            EditorGUI.BeginChangeCheck();

            ScriptableObject target = this;
            SerializedObject soTar = new SerializedObject(target);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            language = (DM_InternEnums.Language)EditorGUILayout.EnumPopup("Language", language); 

            if(dmMenusLocData != null){

                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[0].local)) {

                    Language_Save();

                }//Button

            }//dmMenusLocData != null

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if(dmMenusLocData != null){

                if(verNumb == "Unknown"){

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[0].texts[0].text, MessageType.Info);

                //verNumb == "Unknown"
                } else {

                    EditorGUILayout.BeginHorizontal();

                    if(compVersion != null){

                        EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local + " v" + compVerNumb, style); 

                    //compVersion != null
                    } else {

                        compVersCheckStatic = false;
                        Version_FindStatic_NoWindow(); 

                    }//compVersion != null

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.Space();

                    tabs = GUILayout.SelectionGrid(tabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].local}, 2);

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if(tabs == 0){

                        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Info); 

                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                            SystemsCheck_Reset(true); 

                        }//Button

                        GUILayout.Space(20);

                        EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local, EditorStyles.centeredGreyMiniLabel);

                        EditorGUILayout.Space();

                        debugInt = GUILayout.Toolbar(debugInt, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local });

                        if(debugInt == 0){

                            useDebug = false;

                        }//debugInt == 0

                        if(debugInt == 1){

                            useDebug = true;

                        }//debugInt == 1

                        if(useDebug){

                            GUILayout.Space(20);

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            advDebugInt = GUILayout.Toolbar(advDebugInt, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local });

                            if(advDebugInt == 0){

                                useAdvDebug = false;

                            }//advDebugInt == 0

                            if(advDebugInt == 1){

                                useAdvDebug = true;

                            }//advDebugInt == 1

                        }//useDebug

                        EditorGUILayout.Space();

                    }//tabs = intro

                    if(tabs == 1){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[0].text, MessageType.Info); 

                        EditorGUILayout.Space();

                        tabsSaves = GUILayout.SelectionGrid(tabsSaves, new string[] {dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[1].local}, 2);

                        EditorGUILayout.Space();

                        if(tabsSaves == 0){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[1].text, MessageType.Info);

                            GUILayout.Space(15);

                            if(oldSkin == null){

                                if(oldSkin != Resources.Load("EditorContent/Components Skin") as GUISkin){

                                    oldSkin = GUI.skin;

                                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                                }//oldSkin != Components Skin

                            }//oldSkin == null

                            GUI.skin = Resources.Load("EditorContent/Components Skin") as GUISkin;

                            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconG");
                            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconR");

                            GUILayout.BeginHorizontal();

                            if(compSave != null){

                                EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[0].local, EditorStyles.boldLabel);

                                GUILayout.Label(t2, "checkIcon");

                            //compSave != null
                            } else {

                                EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[0].local, EditorStyles.boldLabel);

                                GUILayout.Label(t3, "checkIcon");

                            }//compSave != null

                            EditorGUILayout.EndHorizontal();

                            GUILayout.Space(5);

                            GUI.skin = oldSkin;

                            EditorGUILayout.Space();

                            if(compSave != null){

                                GUI.enabled = false;

                            //compSave != null
                            } else {

                                GUI.enabled = true;

                            }//compSave != null

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[3].local)){

                                CheckForSystems_Button();

                            }//Button

                            EditorGUILayout.Space();

                        }//tabsSaves = systems

                        if(tabsSaves == 1){

                            if(compSave == null){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[2].text, MessageType.Error);

                            //compSave = null
                            } else {

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[3].text, MessageType.Info);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                save_ScrollPos = GUILayout.BeginScrollView(save_ScrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                GUILayout.Space(5);

                                if(compSave.compDataName == ""){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[4].text, MessageType.Error);

                                //compDataName == null
                                } else {

                                    if(saveValidStatus == ""){

                                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[5].text, MessageType.Warning);

                                    //saveValidStatus = null
                                    } else {

                                        if(saveValidStatus == "Valid"){

                                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[6].text, MessageType.Info);

                                            EditorGUILayout.Space();

                                            if(compSaveStatus == "Valid"){

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[7].text, MessageType.Info);

                                            }//compSaveStatus = Valid

                                        }//saveValidStatus = Valid 

                                        if(saveValidStatus == "NotValid"){

                                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[8].text, MessageType.Error);

                                            EditorGUILayout.Space();

                                            if(compSaveStatus == "Valid"){

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[7].text, MessageType.Info);

                                            }//compSaveStatus = Valid

                                            if(compSaveStatus == "NotValid"){

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[9].text, MessageType.Error);

                                            }//compSaveStatus = Valid

                                        }//saveValidStatus = NotValid

                                        if(saveValidStatus == "NoFiles"){

                                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[10].text, MessageType.Info);

                                        }//saveValidStatus = NoFiles 

                                    }//saveValidStatus = null

                                }//compDataName == null

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndScrollView();                        

                                GUILayout.BeginHorizontal();

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[1].local)){

                                    Saves_Check();

                                }//Button

                                GUILayout.Space(5);

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[2].local)){

                                    Saves_Clear();

                                }//Button

                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.Space();

                            }//compSave = null

                        }//tabsSaves = save files check

                    }//tabs = save

                }//verNumb == "Unknown"

            //dmMenusLocData != null 
            } else {

                if(!languageLock){

                    DM_LocDataFind();

                }//!languageLock 

            }//dmMenusLocData != null

            if(EditorGUI.EndChangeCheck()){

                soTar.ApplyModifiedProperties();

            }//EndChangeCheck

        }//SystemsCheck_Screen


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    ///////////////////////////////////////

    /////////////////////////////////
    //
    //     SYSTEMS
    //
    /////////////////////////////////


        private void CheckForSystems(){

            if(!systemsChecked){

                if(compSave == null){

                    var newCompSave = GameObject.FindObjectOfType<HFPS_CompSave>();

                    if(newCompSave != null){

                        compSave = newCompSave;

                    }//newCompSave != null

                }//compSave == null

                systemsChecked = true;

            }//!systemsChecked

        }//CheckForSystems

        private void CheckForSystems_Button(){

            if(compSave == null){

                var newCompSave = GameObject.FindObjectOfType<HFPS_CompSave>();

                if(newCompSave != null){

                    compSave = newCompSave;

                }//newCompSave != null

            }//compSave == null

            if(compSave == null){

                Debug.Log("No Systems to catch");

            }//compSave = null

        }//CheckForSystems_Button

        private void SystemsCheck_Reset(bool showDebug){

            systemsChecked = false; 
            foundCompData = false;

            saveValidStatus = "";
            compSaveStatus = "";

            if(showDebug){

                Debug.Log("Systems Checker Reset");

            }//showDebug

        }//SystemsCheck_Reset


    /////////////////////////////////
    ///
    ///     SAVE ACTIONS
    ///
    /////////////////////////////////


        private void Saves_Check(){


            foundCompData = false;


    /////////////////
    //
    //     COMP DATA
    //
    /////////////////


            string tempCompName = "/" + compSave.compDataName;
            string tempPath = Application.persistentDataPath + tempCompName;

            Comp_SaveData saveData = new Comp_SaveData();

            if(File.Exists(tempPath)) {

                foundCompData = true;

                if(compSave.secureSave){

                    byte[] catchByte = File.ReadAllBytes(Application.persistentDataPath + tempCompName);
                    string tempCatch = StringObfuscator.Parse(catchByte);

                    saveData = JsonUtility.FromJson<Comp_SaveData>(tempCatch);

                    if(saveData.compVersion == compVersion.version){

                        compSaveStatus = "Valid";

                    //compVersion = version
                    } else {

                        compSaveStatus = "NotValid";

                    }//compVersion = version

                //secureSave
                } else {

                    saveData = JsonUtility.FromJson<Comp_SaveData>(File.ReadAllText(Application.persistentDataPath + tempCompName));

                    if(saveData.compVersion == compVersion.version){

                        compSaveStatus = "Valid";

                    //compVersion = version
                    } else {

                        compSaveStatus = "NotValid";

                    }//compVersion = version

                }//secureSave

                if(useDebug){

                    Debug.Log("Components Data Found");

                }//useDebug

            //File.Exists
            } else {

                if(useDebug){

                    Debug.Log("No Components Data Found");

                }//useDebug

            }//File.Exists

            if(!foundCompData){

                saveValidStatus = "NoFiles";

            }//!foundCompData

            if(compSaveStatus == "Valid"){

                saveValidStatus = "Valid";

            }//compSaveStatus = valid

            if(compSaveStatus == "NotValid"){

                saveValidStatus = "NotValid";

            }//compSaveStatus = NotValid

        }//Saves_Check

        private void Saves_Clear(){

            compSave.Reset_CompData(false);

            foundCompData = false;

            saveValidStatus = "";
            compSaveStatus = "";

            Saves_Check();

        }//Saves_Clear



    //////////////////////////////////////
    ///
    ///     LANGUAGE ACTIONS
    ///
    //////////////////////////////////////


        public static void DM_LocDataFind(){

            if(dmMenusLocData == null){

                //Debug.Log("Find Start");

                //AssetDatabase.Refresh();

                string[] results;
                DM_MenusLocData tempMenusLocData = ScriptableObject.CreateInstance<DM_MenusLocData>();

                results = AssetDatabase.FindAssets(menusLocDataName);

                if(results.Length > 0){

                    foreach(string guid in results){

                        if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                            tempMenusLocData = AssetDatabase.LoadAssetAtPath<DM_MenusLocData>(AssetDatabase.GUIDToAssetPath(guid));

                            if(tempMenusLocData != null){

                                dmMenusLocData = tempMenusLocData;

                                if(dmMenusLocData != null){

                                    if(!languageLock){

                                        languageLock = true;

                                        Language_Check();

                                    }//!languageLock

                                }//dmMenusLocData != null

                            }//tempMenusLocData != null

                            //Debug.Log("Menus Loc Data Found");

                        }//file.exists

                    }//foreach guid

                }//results.Length > 0

            //dmMenusLocData = null
            } else {

                if(!languageLock){

                    languageLock = true;

                    language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

                }//!languageLock

            }//dmMenusLocData = null

        }//DM_LocDataFind

        public static void Language_Check(){

            if(dmMenusLocData != null){

                for(int d = 0; d < dmMenusLocData.dictionary.Count; d++){

                    if(dmMenusLocData.dictionary[d].asset == "Systems Check"){

                        menusLocDataSlot = d;

                        //Debug.Log("Loc Data Slot = " + menusLocDataSlot);

                    }//asset = system check

                }//for d dictionary

                language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

            }//dmMenusLocData != null

        }//Language_Check

        public void Language_Save(){

            if(dmMenusLocData != null){

                if((int)dmMenusLocData.currentLanguage != (int)language){

                    dmMenusLocData.currentLanguage = (DM_InternEnums.Language)(int)language;

                }//currentLanguage != language

            }//dmMenusLocData != null

            Debug.Log("Language Saved");

        }//Language_Save


    //////////////////////////////////////
    ///
    ///     VERSION ACTIONS
    ///
    //////////////////////////////////////


        public static void Version_FindStatic(){

            if(!versionCheckStatic){

                versionCheckStatic = true;

                AssetDatabase.Refresh();

                string[] results;
                DM_Version tempVersion = ScriptableObject.CreateInstance<DM_Version>();

                results = AssetDatabase.FindAssets(versionName);

                if(results.Length > 0){

                    foreach(string guid in results){

                        if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                            tempVersion = AssetDatabase.LoadAssetAtPath<DM_Version>(AssetDatabase.GUIDToAssetPath(guid));

                            if(tempVersion != null){

                                dmVersion = tempVersion;
                                verNumb = dmVersion.version;

                                window = GetWindow<Comp_SysCheck>(false, "Systems Check" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Systems Check Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<Comp_SysCheck>(false, "Systems Check" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Systems Check Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Systems Check Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<Comp_SysCheck>(false, "Systems Check" + " v" + verNumb, true);
                    window.maxSize = window.minSize = windowsSize;

                }//results.Length > 0

            }//!versionCheckStatic

        }//Version_FindStatic

        private static void Version_FindStatic_NoWindow(){

            if(!compVersCheckStatic){

                compVersCheckStatic = true;

                AssetDatabase.Refresh();

                string[] results;
                DM_Version tempVersion = ScriptableObject.CreateInstance<DM_Version>();

                results = AssetDatabase.FindAssets(compVersionName);

                if(results.Length > 0){

                    foreach(string guid in results){

                        if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                            tempVersion = AssetDatabase.LoadAssetAtPath<DM_Version>(AssetDatabase.GUIDToAssetPath(guid));

                            if(tempVersion != null){

                                compVersion = tempVersion;
                                compVerNumb = compVersion.version;

                            //tempVersion != null
                            } else {

                                if(compVerNumb == ""){

                                    compVerNumb = "Unknown";

                                }//compVerNumb = null

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Components Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    compVerNumb = "Unknown";

                }//results.Length > 0

            }//!compVersCheckStatic

        }//Version_FindStatic_NoWindow


    //////////////////////////////////////
    ///
    ///     EXTRA ACTIONS
    ///
    ///////////////////////////////////////


        private void OnDestroy() {

            window = null;
            verNumb = "";

        }//OnDestroy


    }//Comp_SysCheck


}//namespace