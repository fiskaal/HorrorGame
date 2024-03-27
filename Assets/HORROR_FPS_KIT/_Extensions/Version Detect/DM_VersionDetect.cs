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

    public class DM_VersionDetect : EditorWindow {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Content_Check {

            public string version;
            public List<Content_Data> data;

        }//Content_Check

        [Serializable]
        public class Content_Data {

            public string name;
            public int count;

        }//Content_Data


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static DM_VersionDetect window;
        private static Vector2 windowsSize = new Vector2(400, 450);

        public DM_VersDetect_Library versLibrary;

        public string scriptPath;
        public string scriptContent;

        private static DM_Version dmVersion;
        private static string versionName = "VersDetect Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        public List<Content_Check> checkData = new List<Content_Check>();

        Vector2 scrollPos;
        public bool savedChecked;
        private string tempSavedAsset = "";
        private string tempSavedVersion = "";


    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/Version/Version Detect", false , 15)]
        private static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<DM_VersionDetect>(false, "Version Detect" + " v" + verNumb, true);
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

            VersionDetect_Screen();

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        private void VersionDetect_Screen(){

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t0 = (Texture)Resources.Load("EditorContent/VersionDetect/VersionDetect_Header");

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));

            EditorGUI.BeginChangeCheck();

            ScriptableObject target = this;
            SerializedObject soTar = new SerializedObject(target);

            SerializedProperty versLibraryRef = soTar.FindProperty("versLibrary");

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

                    EditorGUILayout.PropertyField(versLibraryRef, new GUIContent("Version Library"), true);

                    EditorGUILayout.Space();

                    if(versLibrary != null){

                        if(versLibrary.content.version != ""){

                            EditorGUILayout.HelpBox("\n" + versLibrary.content.asset + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local + " (" + versLibrary.content.version + ") " + "\n", MessageType.Info);

                        //version != null
                        } else {

                            EditorGUILayout.HelpBox("\n" + versLibrary.content.asset + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local + "\n", MessageType.Info);

                        }//version != null

                    //versLibrary != null
                    } else {

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, MessageType.Error);

                    }//versLibrary != null

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                    EditorGUILayout.Space();

                    if(tempSavedAsset != ""){

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local + tempSavedAsset, MessageType.Info);

                    }//tempSavedAsset != null

                    if(tempSavedVersion == ""){

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local, MessageType.Error);

                    //tempSavedVersion == null
                    } else {

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local + tempSavedVersion, MessageType.Info);

                    }//tempSavedVersion == null

                    EditorGUILayout.Space();

                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    if(versLibrary != null){

                        if(tempSavedVersion == ""){

                            GUI.enabled = true;

                        //tempSavedVersion = null
                        } else {

                            GUI.enabled = false;

                        }//tempSavedVersion = null

                    //versLibrary != null
                    } else {

                        GUI.enabled = false;

                    }//versLibrary != null

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                        Scripts_Check();

                    }//Button

                    if(tempSavedVersion != ""){

                        GUI.enabled = true;

                    //tempSavedVersion != ""
                    } else {

                        GUI.enabled = false;

                    }//tempSavedVersion != ""

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                        Notifications_Clear();

                    }//Button

                    EditorGUILayout.EndHorizontal();

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

        }//VersionDetect_Screen


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    ///////////////////////////////////////


        private void Scripts_Check(){

            checkData = new List<Content_Check>();

            bool done = false;
            tempSavedAsset = "";
            tempSavedVersion = "";

            if(versLibrary != null){

                tempSavedAsset = versLibrary.content.asset;

                if(versLibrary.content.versions.Count > 0){

                    for(int v = 0; v < versLibrary.content.versions.Count; ++v ) {

                        Content_Check tempCheck = new Content_Check();
                        tempCheck.version = versLibrary.content.versions[v].name;
                        tempCheck.data = new List<Content_Data>();

                        if(versLibrary.content.versions[v].library.Count > 0){

                            for(int l = 0; l < versLibrary.content.versions[v].library.Count; ++l ) {

                                Content_Data tempData = new Content_Data();
                                tempData.name = versLibrary.content.versions[v].library[l].name;

                                if(versLibrary.content.versions[v].library[l].template != null){

                                    scriptPath = File_Find(versLibrary.content.versions[v].library[l].name);

                                    if(scriptPath != ""){

                                        scriptContent = File.ReadAllText(scriptPath);

                                        if(versLibrary.content.versions[v].library[l].template.content.Count > 0){

                                            for(int c = 0; c < versLibrary.content.versions[v].library[l].template.content.Count; ++c ) {

                                                if(scriptContent.Contains(versLibrary.content.versions[v].library[l].template.content[c].text)){

                                                    tempData.count += 1;

                                                    //Debug.Log("TempDataCount = " + tempData.count);
                                                    //Debug.Log("ContentCount = " + versLibrary.content.versions[v].library[l].template.content.Count);
                                                    //Debug.Log("Found " + versLibrary.content.versions[v].library[l].name + " " + versLibrary.content.versions[v].library[l].template.content[c].name);

                                                    if(tempData.count == versLibrary.content.versions[v].library[l].template.content.Count){

                                                        //Debug.Log("Data Added");
                                                        tempCheck.data.Add(tempData);

                                                        if(tempCheck.data.Count == versLibrary.content.versions[v].library.Count){

                                                            //Debug.Log("Check Added");
                                                            checkData.Add(tempCheck);

                                                        }//data.Count = library.Count

                                                        if(checkData.Count == versLibrary.content.versions.Count){

                                                            //Debug.Log("Early Done");
                                                            done = true;

                                                        }//checkData.Count = versions.Count

                                                    }//tempData.count = content.Count

                                                //Contains
                                                } else {

                                                    //Debug.Log("Data Added");
                                                    tempCheck.data.Add(tempData);

                                                    if(tempCheck.data.Count == versLibrary.content.versions[v].library.Count){

                                                        //Debug.Log("Check Added");
                                                        checkData.Add(tempCheck);

                                                    }//data.Count = library.Count

                                                    if(checkData.Count == versLibrary.content.versions.Count){

                                                        //Debug.Log("Early Done");
                                                        done = true;

                                                    }//checkData.Count = versions.Count

                                                }//Contains

                                            }//for c content

                                        }//content.Count > 0

                                    }//scriptPath != null

                                }//template != null

                            }//for l library

                        }//library.Count > 0

                    }//for v versions

                }//versions.Count > 0

                if(done){

                    //Debug.Log("Done");

                    if(checkData.Count > 0){

                        for(int ch = 0; ch < checkData.Count; ++ch ) {

                            if(checkData[ch].data.Count > 0){

                                for(int chd = 0; chd < checkData[ch].data.Count; ++chd ) {

                                    if(checkData[ch].data.Count == versLibrary.content.versions[ch].library.Count){

                                        if(checkData[ch].data[chd].count == versLibrary.content.versions[ch].library[chd].template.content.Count){

                                            tempSavedVersion = versLibrary.content.version + versLibrary.content.versions[ch].name;

                                        }//count = content.Count

                                    }//data.Count = library.Count

                                }//for chd checkData.data

                            }//data.Count > 0

                        }//for ch checkData

                    }//checkData.Count > 0

                }//done

            }//versLibrary != null

        }//Scripts_Check

        private void Notifications_Clear(){

            tempSavedAsset = "";
            tempSavedVersion = "";
            checkData = new List<Content_Check>();

        }//Notifications_Clear


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

                    if(dmMenusLocData.dictionary[d].asset == "Version Detect"){

                        menusLocDataSlot = d;

                        //Debug.Log("Loc Data Slot = " + menusLocDataSlot);

                    }//asset = Version Detect

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

                                window = GetWindow<DM_VersionDetect>(false, "Version Detect" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Version Detect Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<DM_VersionDetect>(false, "Version Detect" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Puzzler Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Version Detect Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<DM_VersionDetect>(false, "Version Detect" + " v" + verNumb, true);
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


    }//DM_VersionDetect
    
    
}//namespace

#endif
