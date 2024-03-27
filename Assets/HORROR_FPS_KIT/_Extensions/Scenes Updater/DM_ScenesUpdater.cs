#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.IO;

using DizzyMedia.Version;

namespace DizzyMedia.Extension {

    public class DM_ScenesUpdater : EditorWindow {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static DM_ScenesUpdater window;
        private static Vector2 windowsSize = new Vector2(400, 400);

        private static DM_Version dmVersion;
        private static string versionName = "ScenesUpdater Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        public DM_ScenesUpdater_Template template;

        public List<SceneAsset> sceneAssets = new List<SceneAsset>();

        bool scenesCaught = false;
        Vector2 scrollPos;
        
        
    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        private SerializedObject serializedObject;

        private void OnEnable() {
        
            serializedObject = new SerializedObject(this);
        
        }//OnEnable


    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/Project/Scenes Updater", false , 13)]
        public static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<DM_ScenesUpdater>(false, "Scenes Updater" + " v" + verNumb, true);
                //window.maxSize = window.minSize = windowsSize;

            }//dmVersion == null

            if(dmMenusLocData == null){

                languageLock = false;
                DM_LocDataFind();

            //dmMenusLocData = null
            } else {

                language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

            }//dmMenusLocData = null

        }//OpenWizard

        private void OnGUI() {

            ScenesUpdater_Screen();

            if(!scenesCaught){

                Scenes_Catch(false);

            }//!scenesCaught

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        public void ScenesUpdater_Screen(){
            
            serializedObject.Update();

            var style = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter};

            EditorGUI.BeginChangeCheck();

            SerializedProperty sceneAssetsRef = serializedObject.FindProperty("sceneAssets");
            SerializedProperty templateRef = serializedObject.FindProperty("template");

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

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Info);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(templateRef, new GUIContent("Template"), true);

                    EditorGUILayout.Space();

                    GUILayout.Label(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, EditorStyles.boldLabel);

                    EditorGUILayout.Space();

                    scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                    EditorGUILayout.PropertyField(sceneAssetsRef, new GUIContent("Scenes"), true);

                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    if(template != null){

                        GUI.enabled = true;

                    //template != null
                    } else {

                        GUI.enabled = false;

                    }//template != null

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)) {

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[1].local)){

                            Scenes_Save();

                        }//DisplayDialog

                    }//button

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)) {

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[1].local)){

                            int option = EditorUtility.DisplayDialogComplex(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].message,
                                dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].buttons[0].local,
                                dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].buttons[1].local,
                                dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].buttons[2].local);

                                switch(option) {

                                    //Add to
                                    case 0:    

                                        Scenes_Load(option);

                                        //Debug.Log("Add to scenes");
                                        break;

                                    //Clear
                                    case 1:    

                                        Scenes_Load(option);

                                        //Debug.Log("Clear list");
                                        break;

                                    //cancel
                                    case 2:

                                        //Debug.Log("Cancel");
                                        break;

                                    default:

                                        Debug.LogError("Unrecognized option.");
                                        break;

                                }//switch option

                        }//DisplayDialog 

                    }//button

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(5);

                    EditorGUILayout.BeginHorizontal();

                    GUI.enabled = true;

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)) {

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].buttons[1].local)){

                            Scenes_Catch(true);

                        }//DisplayDialog

                    }//button

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(5);

                    EditorGUILayout.BeginHorizontal();

                    if(sceneAssets.Count > 0){

                        GUI.enabled = true;

                    //sceneAssets.Count > 0
                    } else {

                        GUI.enabled = false;

                    }//sceneAssets.Count > 0

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[4].local)) {

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[4].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[4].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[4].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[4].buttons[1].local)){

                            BuildSettings_Update();

                        }//DisplayDialog

                    }//button

                    GUI.enabled = true;

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[5].local)) {

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[5].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[5].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[5].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[5].buttons[1].local)){

                            BuildSettings_Clear();

                        }//DisplayDialog 

                    }//button

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                }//verNumb == "Unknown"

            //dmMenusLocData != null 
            } else {

                if(!languageLock){

                    DM_LocDataFind();

                }//!languageLock 

            }//dmMenusLocData != null

            EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

        }//ScenesUpdater_Screen


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    //////////////////////////////////////

    /////////////////////////////////
    //
    //     SCENES
    //
    /////////////////////////////////


        public void Scenes_Catch(bool debug){

            if(EditorBuildSettings.scenes.Length > 0){

                sceneAssets = new List<SceneAsset>();

                foreach(var scene in EditorBuildSettings.scenes){

                    SceneAsset tempScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

                    sceneAssets.Add(tempScene);

                }//foreach scene

                scenesCaught = true;

                if(debug){

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local);

                }//debug

            //scenes.Length > 0
            } else {

                if(debug){

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local);

                }//debug

            }//scenes.Length > 0

        }//Scenes_Catch

        public void Scenes_Save(){

            if(template != null){

                template.scenes = new List<SceneAsset>();

                for(int i = 0; i < sceneAssets.Count; i++){

                    template.scenes.Add(sceneAssets[i]);

                }//for i sceneAssets

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local);

                AssetDatabase.SaveAssets();

                EditorUtility.SetDirty(template);

            }//template != null

        }//Scenes_Save

        public void Scenes_Load(int loadType){

            if(template != null){

                if(template.scenes.Count > 0){

                    if(loadType == 0){

                        for(int i = 0; i < template.scenes.Count; i++){

                            if(!Scene_Check(template.scenes[i])){

                                sceneAssets.Add(template.scenes[i]);

                            }//scene not present

                        }//for i scenes

                    }//loadType = add to

                    if(loadType == 1){

                        sceneAssets = new List<SceneAsset>();

                        OnGUI();

                        for(int i = 0; i < template.scenes.Count; i++){

                            sceneAssets.Add(template.scenes[i]);

                        }//for i scenes

                        OnGUI();

                    }//loadType = clear

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local);

                }//scenes.Count > 0

            //template != null
            } else {

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local);

            }//template != null

        }//Scenes_Load

        public bool Scene_Check(SceneAsset newScene){

            if(sceneAssets.Count > 0){

                for(int i = 0; i < sceneAssets.Count; i++){

                    if(sceneAssets[i].name == newScene.name){

                        return true;

                    }//name = name

                }//for i sceneAssets

            }//sceneAssets.Count > 0

            return false;

        }//Scene_Check


    /////////////////////////////////
    //
    //     BUILD SETTINGS
    //
    /////////////////////////////////


        public void BuildSettings_Update() {

            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            foreach(var sceneAsset in sceneAssets) {

                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);

                if (!string.IsNullOrEmpty(scenePath))

                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));

            }//foreach sceneAsset

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

        }//BuildSettings_Update

        public void BuildSettings_Clear(){

            EditorBuildSettings.scenes = new EditorBuildSettingsScene[0];

        }//BuildSettings_Clear


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

                    if(dmMenusLocData.dictionary[d].asset == "Scenes Updater"){

                        menusLocDataSlot = d;

                        //Debug.Log("Loc Data Slot = " + menusLocDataSlot);

                    }//asset = scenes updater

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

                                window = GetWindow<DM_ScenesUpdater>(false, "Scenes Updater" + " v" + verNumb, true);
                                //window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Scenes Updater Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<DM_ScenesUpdater>(false, "Scenes Updater" + " v" + verNumb, true);
                                //window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Scenes Updater Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Scenes Updater Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<DM_ScenesUpdater>(false, "Scenes Updater" + " v" + verNumb, true);
                    //window.maxSize = window.minSize = windowsSize;

                }//results.Length > 0

            }//!versionCheckStatic

        }//Version_FindStatic


    //////////////////////////////////////
    ///
    ///     EXTRA ACTIONS
    ///
    ///////////////////////////////////////


        private void OnDestroy() {

            window = null;
            verNumb = "";
            
            if(serializedObject != null){
            
                serializedObject.Dispose();
            
            }//serializedObject != null

        }//OnDestroy


    }//DM_ScenesUpdater
    
    
}//namespace

#endif
