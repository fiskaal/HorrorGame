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

using HFPS.Systems;

namespace DizzyMedia.Extension {

    public class HFPS_FloatingIconHelper : EditorWindow {

        
    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////
        
        
        [Serializable]
        public class Object_Temp {
            
            public GameObject newObject;
            public bool canAdd;
            
        }//Object_Temp
        

    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static HFPS_FloatingIconHelper window;
        private static Vector2 windowsSize = new Vector2(400, 450);

        private static DM_Version dmVersion;
        private static string versionName = "FloatingIconHelper Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;
        
        public FloatingIconManager floatingIconManager;
        private List<Object_Temp> objectsTemp = new List<Object_Temp>();

        Vector2 scrollPos;
        Vector2 scrollPos2;
        
        
    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/HFPS/Floating Icon Helper", false , 13)]
        private static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<HFPS_FloatingIconHelper>(false, "Floating Icon Helper" + " v" + verNumb, true);
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

            FloatingIcon_Screen();

        }//OnGUI
        

    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        private void FloatingIcon_Screen(){

            EditorGUI.BeginChangeCheck();

            ScriptableObject target = this;
            SerializedObject soTar = new SerializedObject(target);

            SerializedProperty floatingIconManagerRef = soTar.FindProperty("floatingIconManager");

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            language = (DM_InternEnums.Language)EditorGUILayout.EnumPopup("Language", language); 

            if(dmMenusLocData != null){

                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[0].local)) {

                    Language_Save();

                }//Button

            }//dmMenusLocData != null

            EditorGUILayout.EndHorizontal();

            if(dmMenusLocData != null){

                if(verNumb == "Unknown"){

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[0].texts[0].text, MessageType.Error);

                //verNumb == "Unknown"
                } else {
                    
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Info);
                    
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.PropertyField(floatingIconManagerRef, true);
                    
                    EditorGUILayout.Space();
                    
                    if(objectsTemp.Count == 0){

                        EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, EditorStyles.centeredGreyMiniLabel);

                        EditorGUILayout.Space();

                        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        if(Selection.gameObjects.Length > 0){

                            for(int i = 0; i < Selection.gameObjects.Length; i++){

                                if(Selection.gameObjects[i].GetComponent<MeshRenderer>() != null){

                                    EditorGUILayout.HelpBox(Selection.gameObjects[i].name + " | " + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local, MessageType.Info);

                                //MeshRenderer != null
                                } else {

                                    EditorGUILayout.HelpBox(Selection.gameObjects[i].name + " | " + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local + "\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local, MessageType.Error);

                                }//MeshRenderer != null

                            }//for i Selection

                        //Selection.Length > 0
                        } else {

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local, MessageType.Warning);

                        }//Selection.Length > 0

                        EditorGUILayout.Space();

                        EditorGUILayout.EndScrollView();
                        
                    //objectsTemp.Count = 0
                    } else {
                        
                        EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local, EditorStyles.centeredGreyMiniLabel);

                        EditorGUILayout.Space();
                        
                        scrollPos2 = GUILayout.BeginScrollView(scrollPos2, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        if(objectsTemp.Count > 0){

                            for(int i = 0; i < objectsTemp.Count; i++){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + i + " " + objectsTemp[i].newObject.name + " | " + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[7].local + objectsTemp[i].canAdd, MessageType.Info);

                            }//for i objectsTemp

                        }//objectsTemp.Count > 0

                        EditorGUILayout.Space();

                        EditorGUILayout.EndScrollView();
                        
                    }//objectsTemp.Count = 0

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();
                    
                    if(Selection.gameObjects.Length > 0){
                        
                        GUI.enabled = true;
                        
                    //Selection.Length > 0
                    } else {
                        
                        GUI.enabled = false;
                        
                    }//Selection.Length > 0
                    
                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                        Objects_Catch();

                    }//Button
                    
                    if(objectsTemp.Count > 0){
                        
                        GUI.enabled = true;
                        
                    //objectsTemp.Count > 0
                    } else {
                        
                        GUI.enabled = false;
                        
                    }//objectsTemp.Count > 0
                    
                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                        Objects_Clear();

                    }//Button

                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.Space();
                    
                    if(objectsTemp.Count > 0 && floatingIconManager != null){
                        
                        GUI.enabled = true;
                        
                    //floatingIconManager != null
                    } else {
                        
                        GUI.enabled = false;
                        
                    }//floatingIconManager != null
                    
                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)){

                        Objects_AddToFloatingIcons();

                    }//Button

                    EditorGUILayout.Space();
                    
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
            
        }//FloatingIcon_Screen
        
        
    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    ///////////////////////////////////////
        
        
        private void Objects_Catch(){
            
            bool present = false;
            
            if(Selection.gameObjects.Length > 0){
             
                for(int i = 0; i < Selection.gameObjects.Length; i++){
                 
                    if(Selection.gameObjects[i].GetComponent<MeshRenderer>() != null){
                        
                        Object_Temp newTempObject = new Object_Temp();
                        
                        newTempObject.newObject = Selection.gameObjects[i];
                        newTempObject.canAdd = true;
                        
                        if(!Objects_Check(newTempObject)){
                                
                            objectsTemp.Add(newTempObject);
                            
                            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[8].local + newTempObject.newObject.name);
                                
                        }//object not present

                    }//MeshRenderer != null
                    
                }//for i Selection
                
            }//Selection.Length > 0
            
        }//Objects_Catch
        
        private void Objects_Clear(){
            
            objectsTemp.Clear();
            
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[9].local);
            
        }//Objects_Clear
        
        private void Objects_AddToFloatingIcons(){
            
            if(floatingIconManager != null){
                
                for(int ot = 0; ot < objectsTemp.Count; ot++){
             
                    if(!Icons_Check(objectsTemp[ot])){

                        floatingIconManager.FloatingIcons.Add(objectsTemp[ot].newObject);
                        
                        Debug.Log(objectsTemp[ot].newObject.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[10].local);

                    //icon object not present
                    } else {

                        Debug.Log(objectsTemp[ot].newObject.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[11].local);

                    }//icon object not present
                    
                }//for ot objectsTemp
                
            }//floatingIconManager != null
            
        }//Objects_AddToFloatingIcons
        
        private bool Objects_Check(Object_Temp newTempObject){
            
            if(objectsTemp.Count > 0){
            
                for(int ot = 0; ot < objectsTemp.Count; ot++){

                    if(objectsTemp[ot].newObject == newTempObject.newObject){

                        return true;

                    }//newObject = newObject

                }//for ot objectsTemp
                
            //objectsTemp.Count > 0
            } else {
                
                return false;
                
            }//objectsTemp.Count > 0
            
            return false;
            
        }//Objects_Check
        
        private bool Icons_Check(Object_Temp tempObject){
            
            bool present = false;

            if(floatingIconManager.FloatingIcons.Count > 0){
                    
                for(int fli = 0; fli < floatingIconManager.FloatingIcons.Count; fli++){
                 
                    if(floatingIconManager.FloatingIcons[fli] == tempObject.newObject){
                        
                        return true;

                    }//object == newObject
                    
                }//for fli floatingIcons
                    
            //FloatingIcons.Count > 0
            } else {
                    
                return false;
                    
            }//FloatingIcons.Count > 0

            return false;
            
        }//Icons_Check
        
        
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

                    if(dmMenusLocData.dictionary[d].asset == "Floating Icon Helper"){

                        menusLocDataSlot = d;

                        //Debug.Log("Loc Data Slot = " + menusLocDataSlot);

                    }//asset = Floating Icon Helper

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

                                window = GetWindow<HFPS_FloatingIconHelper>(false, "Floating Icon Helper" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Floating Icon Helper Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<HFPS_FloatingIconHelper>(false, "Floating Icon Helper" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Puzzler Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Floating Icon Helper Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<HFPS_FloatingIconHelper>(false, "Floating Icon Helper" + " v" + verNumb, true);
                    window.maxSize = window.minSize = windowsSize;

                }//results.Length > 0

            }//!versionCheckStatic

        }//Version_FindStatic


    //////////////////////////////////////
    ///
    ///     EXTRA ACTIONS
    ///
    ///////////////////////////////////////


        public string File_Find(string fileName){

            string[] results = new string[0];

            results = AssetDatabase.FindAssets(fileName + " t:script");

            if(results.Length > 0){

                UnityEngine.Object[] scripts = new UnityEngine.Object[results.Length];

                for(int r = 0; r < results.Length; r++) {

                    scripts[r] = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(results[r]), typeof(UnityEngine.Object));

                }//for r results

                if(scripts.Length > 0){

                    for(int s = 0; s < scripts.Length; s++) {

                        if(scripts[s].name == fileName){

                            //Debug.Log(scripts[s].name + " Found!");

                            return AssetDatabase.GUIDToAssetPath(results[s]);

                        //name = tempName
                        } else {

                            //Debug.Log(scripts[s].name + " Not Correct File Found!");

                        }//name = tempName

                    }//for s scripts

                }//scripts.Length > 0

            //results > 0
            } else {

                Debug.Log(fileName + " Not Found!");

            }//results > 0

            return "";

        }//File_Find

        private void OnDestroy() {

            window = null;
            verNumb = "";

        }//OnDestroy


    }//HFPS_FloatingIconHelper
    
    
}//namespace

#endif