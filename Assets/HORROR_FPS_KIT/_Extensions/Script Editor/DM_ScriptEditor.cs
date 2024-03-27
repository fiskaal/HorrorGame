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

    public class DM_ScriptEditor : EditorWindow {


    ///////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////

    //////////////////////////
    ///
    ///     CORE SAVE DATA
    ///
    //////////////////////////


        [Serializable]
        public class Saved_Content {

            public string name;
            public string path;
            public string content;

            public List<Saved_Edit> edits = new List<Saved_Edit>();

        }//Saved_Content

        [Serializable]
        public class Saved_Edit {

            public string editName;
            public bool present;
            public bool origPresent;
            public bool removed;

        }//Saved_Edit


    //////////////////////////
    ///
    ///     DEBUG SAVE DATA
    ///
    //////////////////////////


        [Serializable]
        public class Content_Saved {

            public string name;
            public string editName;
            public string path;
            public string content;
            public bool present;
            public bool origPresent;
            public bool removed;

        }//Content_Saved


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Update_Type {

            Add = 0,
            Remove = 1,

        }//Update_Type


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

        
        GUISkin oldSkin;

        private static DM_ScriptEditor window;
        private static Vector2 windowsSize = new Vector2(400, 600);

        public DM_ScriptEdit_Library editLibrary;
        public DM_ScriptEdit_Library curLibrary;

        public string scriptPath;
        public string scriptContent;

        private static DM_Version dmVersion;
        private static string versionName = "ScriptEditor Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        public Update_Type updateType;

        public List<Saved_Content> savedContent = new List<Saved_Content>();
        public List<Saved_Content> savedContentRemove = new List<Saved_Content>();

        public List<Content_Saved> saved = new List<Content_Saved>();
        public List<Content_Saved> savedRemoved = new List<Content_Saved>();

        Vector2 scrollPos;
        Vector2 scrollPos2;
        public bool editsDone;
        public bool savedChecked;
        public bool savedRemChecked;
        private bool barShowing = false;

        public int tabs;
        public int statusTabs;
        
        
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


        [MenuItem("Tools/Dizzy Media/Extensions/Project/Script Editor", false , 13)]
        private static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
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

            ScriptEdit_Screen();

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        private void ScriptEdit_Screen(){
            
            serializedObject.Update();
            
            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/DM Utility Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != IWC Skin

            }//oldSkin == null
            
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t0 = (Texture)Resources.Load("EditorContent/ScriptEditor/ScriptEditor_Header");

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));

            EditorGUI.BeginChangeCheck();

            SerializedProperty savedContentRef = serializedObject.FindProperty("savedContent");

            SerializedProperty editLibraryRef = serializedObject.FindProperty("editLibrary");

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

                    EditorGUILayout.PropertyField(editLibraryRef, true);

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    tabs = GUILayout.SelectionGrid(tabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[5].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[6].local }, 2);

                    EditorGUILayout.Space();

                    if(tabs == 0){

                        if(editLibrary != null){
                            
                            if(curLibrary != null){
                                
                                if(curLibrary != editLibrary){
                                
                                    curLibrary = editLibrary;
                                
                                    Notifications_Clear();
                                    
                                }//curLibrary != editLibrary
                                
                            //curLibrary != null
                            } else {
                             
                                if(curLibrary != editLibrary){
                                
                                    curLibrary = editLibrary;
                                    
                                }//curLibrary != editLibrary
                                
                            }//curLibrary != null

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[10].local + editLibrary.content.name, EditorStyles.label);
                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[11].local + editLibrary.content.version, EditorStyles.label);
                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[12].local + editLibrary.content.library.Count, EditorStyles.label);

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[13].local, EditorStyles.label);

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        //editLibrary != null
                        } else {

                            EditorGUILayout.HelpBox("\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local + "\n", MessageType.Error);

                            //if(saved.Count > 0){

                                //Notifications_Clear();

                            //}//saved.Count > 0

                        }//editLibrary != null

                        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        if(editLibrary != null){

                            EditorGUILayout.Space();

                            if(editLibrary.content.library.Count > 0){

                                for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                                    if(editLibrary.content.library[l].template.content.Count > 1){

                                        EditorGUILayout.LabelField(" - " + editLibrary.content.library[l].name + " " + "( " + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[16].local + " )", EditorStyles.label);

                                    //template.content.Count > 1
                                    } else {

                                        EditorGUILayout.LabelField(" - " + editLibrary.content.library[l].name + " " + "( " + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[15].local + " )", EditorStyles.label);

                                    }//template.content.Count > 1

                                }//for l library

                            }//library.Count > 0

                        }//editLibrary != null

                        EditorGUILayout.Space();

                        EditorGUILayout.EndScrollView(); 

                    }//tabs = Library 

                    if(tabs == 1){

                        if(editLibrary != null){
                            
                            if(curLibrary != null){
                                
                                if(editLibrary != curLibrary){
                                
                                    curLibrary = editLibrary;
                                
                                    Notifications_Clear();
                                    
                                }//editLibrary != curLibrary
                                
                            //curLibrary != null
                            } else {
                             
                                if(curLibrary != editLibrary){
                                
                                    curLibrary = editLibrary;
                                    
                                }//curLibrary != editLibrary
                                
                            }//curLibrary != null 

                            statusTabs = GUILayout.SelectionGrid(statusTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[7].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[8].local }, 2);

                            EditorGUILayout.Space();

                            if(statusTabs == 0){

                                if(savedContent.Count > 0 | savedContentRemove.Count > 0){

                                    EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[14].local, EditorStyles.label);

                                    EditorGUILayout.Space();

                                }//savedContent.Count > 0 or savedContentRemove.Count > 0

                            }//statusTabs = general

                            scrollPos2 = GUILayout.BeginScrollView(scrollPos2, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            //EditorGUILayout.Space();

                            //EditorGUILayout.PropertyField(savedContentRef, true);

                            EditorGUILayout.Space();

                            if(statusTabs == 0){

                                if(savedContent.Count > 0 | savedContentRemove.Count > 0){

                                    if(!savedChecked){

                                        if(updateType == Update_Type.Add){

                                            if(savedContent.Count > 0){

                                                for(int sc = 0; sc < savedContent.Count; ++sc ) {

                                                    int presentCount = 0;

                                                    for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                                                        if(editLibrary.content.library[l].name == savedContent[sc].name){

                                                            EditorGUILayout.LabelField(" - " + savedContent[sc].name + " " + "( " + savedContent[sc].edits.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[16].local + " )", EditorStyles.label);

                                                            if(savedContent[sc].edits.Count > 0){

                                                                for(int sce = 0; sce < savedContent[sc].edits.Count; ++sce ) {

                                                                    if(savedContent[sc].edits[sce].present){

                                                                        presentCount += 1;

                                                                    }//present

                                                                }//for sce edits

                                                            }//edits.Count > 0

                                                            if(presentCount > 0){
                                                                
                                                                GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

                                                                Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconG");
                                                                Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconR");
                                                                
                                                                EditorGUILayout.BeginHorizontal();

                                                                EditorGUILayout.LabelField(" - " + savedContent[sc].name + " " + "( " + presentCount + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + " )", EditorStyles.label);
                                                                
                                                                if(presentCount == editLibrary.content.library[l].template.content.Count){
                                                                
                                                                    GUILayout.Label(t2, "checkIcon");
                                                                    
                                                                //presentCount = template.content.Count
                                                                } else {
                                                                    
                                                                    GUILayout.Label(t3, "checkIcon");
                                                                    
                                                                }//presentCount = template.content.Count
                                                                
                                                                EditorGUILayout.EndHorizontal();
                                                                
                                                                GUI.skin = oldSkin;

                                                            }//presentCount > 0

                                                        }//name = name

                                                    }//for l library.Count

                                                    EditorGUILayout.Space();

                                                }//for sc savedContent

                                            }//savedContent.Count > 0

                                        }//updateType = add

                                        if(updateType == Update_Type.Remove){

                                            if(savedContentRemove.Count > 0){

                                                for(int sc = 0; sc < savedContentRemove.Count; ++sc ) {

                                                    int presentCount = 0;

                                                    for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                                                        if(editLibrary.content.library[l].name == savedContentRemove[sc].name){

                                                            EditorGUILayout.LabelField(" - " + savedContentRemove[sc].name + " " + "( " + savedContentRemove[sc].edits.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[16].local + " )", EditorStyles.label);

                                                            if(savedContentRemove[sc].edits.Count > 0){

                                                                for(int sce = 0; sce < savedContentRemove[sc].edits.Count; ++sce ) {

                                                                    if(savedContentRemove[sc].edits[sce].present){

                                                                        presentCount += 1;

                                                                    }//present

                                                                }//for sce edits

                                                            }//edits.Count > 0

                                                            if(presentCount > 0){
                                                                
                                                                GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

                                                                Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconG");
                                                                Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconR");
                                                            
                                                                EditorGUILayout.BeginHorizontal();

                                                                EditorGUILayout.LabelField(" - " + savedContentRemove[sc].name + " " + "( " + presentCount + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + " )", EditorStyles.label);
                                                                
                                                                if(presentCount == editLibrary.content.library[l].template.content.Count){
                                                                
                                                                    GUILayout.Label(t2, "checkIcon");
                                                                    
                                                                //presentCount = template.content.Count
                                                                } else {
                                                                    
                                                                    GUILayout.Label(t3, "checkIcon");
                                                                    
                                                                }//presentCount = template.content.Count
                                                                
                                                                EditorGUILayout.EndHorizontal();
                                                                
                                                                GUI.skin = oldSkin;

                                                            }//presentCount > 0

                                                        }//name = name

                                                    }//for l library.Count

                                                    EditorGUILayout.Space();

                                                }//for sc savedContentRemove

                                            }//savedContentRemove.Count > 0

                                        }//updateType = remove

                                    //!savedChecked
                                    } else {

                                        if(updateType == Update_Type.Add){

                                            for(int sc = 0; sc < savedContent.Count; ++sc ) {

                                                int presentCount = 0;

                                                for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                                                    if(editLibrary.content.library[l].name == savedContent[sc].name){

                                                        if(savedContent[sc].edits.Count > 0){

                                                            for(int sce = 0; sce < savedContent[sc].edits.Count; ++sce ) {

                                                                if(savedContent[sc].edits[sce].present){

                                                                    presentCount += 1;

                                                                }//present

                                                            }//for sce edits

                                                        }//edits.Count > 0

                                                        if(presentCount > 0){
                                                            
                                                            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

                                                            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconG");
                                                            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconR");
                                                            
                                                            EditorGUILayout.BeginHorizontal();

                                                            EditorGUILayout.LabelField(" - " + savedContent[sc].name + " " + "( " + presentCount + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + " )", EditorStyles.label);
                                                            
                                                            if(presentCount == editLibrary.content.library[l].template.content.Count){
                                                                
                                                                GUILayout.Label(t2, "checkIcon");
                                                                    
                                                            //presentCount = template.content.Count
                                                            } else {
                                                                    
                                                                GUILayout.Label(t3, "checkIcon");
                                                                    
                                                            }//presentCount = template.content.Count
                                                            
                                                            EditorGUILayout.EndHorizontal();

                                                            GUI.skin = oldSkin;

                                                        }//presentCount > 0

                                                    }//name = name

                                                }//for l library.Count

                                                EditorGUILayout.Space();

                                            }//for sc savedContent

                                        }//updateType = add

                                        if(updateType == Update_Type.Remove){

                                            for(int sc = 0; sc < savedContentRemove.Count; ++sc ) {

                                                int presentCount = 0;

                                                for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                                                    if(editLibrary.content.library[l].name == savedContentRemove[sc].name){

                                                        if(savedContentRemove[sc].edits.Count > 0){

                                                            for(int sce = 0; sce < savedContentRemove[sc].edits.Count; ++sce ) {

                                                                if(savedContentRemove[sc].edits[sce].present){

                                                                    presentCount += 1;

                                                                }//present

                                                            }//for sce edits

                                                        }//edits.Count > 0

                                                        if(presentCount > 0){
                                                            
                                                            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

                                                            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconG");
                                                            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_CheckIconR");

                                                            EditorGUILayout.LabelField(" - " + savedContentRemove[sc].name + " " + "( " + presentCount + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local + editLibrary.content.library[l].template.content.Count + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + " )", EditorStyles.label);
                                                            
                                                            if(presentCount == editLibrary.content.library[l].template.content.Count){
                                                                
                                                                GUILayout.Label(t2, "checkIcon");
                                                                    
                                                            //presentCount = template.content.Count
                                                            } else {
                                                                    
                                                                GUILayout.Label(t3, "checkIcon");
                                                                    
                                                            }//presentCount = template.content.Count
                                                            
                                                            EditorGUILayout.EndHorizontal();

                                                            GUI.skin = oldSkin;

                                                        }//presentCount > 0

                                                    }//name = name

                                                }//for l library.Count

                                                EditorGUILayout.Space();

                                            }//for sc savedContentRemove

                                        }//updateType = remove

                                    }//!savedChecked

                                //savedContent.Count > 0 or savedContentRemove.Count > 0
                                } else {

                                    EditorGUILayout.HelpBox("\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local + "\n", MessageType.Warning);

                                }//savedContent.Count > 0 or savedContentRemove.Count > 0

                            }//statusTabs = general

                            if(statusTabs == 1){

                                if(saved.Count > 0 | savedRemoved.Count > 0){

                                    if(!savedChecked){

                                        if(updateType == Update_Type.Add){

                                            for(int s = 0; s < saved.Count; ++s ) {

                                                if(!saved[s].present){

                                                    EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local + "\n", MessageType.Info);

                                                //!present
                                                } else {

                                                    EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local + "\n", MessageType.Info);

                                                }//!present

                                            }//for s saved

                                        }//updateType = add

                                        if(updateType == Update_Type.Remove){

                                            for(int sr = 0; sr < savedRemoved.Count; ++sr ) {

                                                if(!savedRemoved[sr].removed){

                                                    if(!savedRemoved[sr].origPresent){

                                                        EditorGUILayout.HelpBox("\n" + savedRemoved[sr].name + " > " + savedRemoved[sr].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + "\n", MessageType.Error);

                                                    //!origPresent
                                                    } else {

                                                        EditorGUILayout.HelpBox("\n" + savedRemoved[sr].name + " > " + savedRemoved[sr].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local + "\n", MessageType.Info);

                                                    }//!origPresent

                                                //removed
                                                } else {

                                                    EditorGUILayout.HelpBox("\n" + savedRemoved[sr].name + " > " + savedRemoved[sr].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[7].local + "\n", MessageType.Info);

                                                }//removed

                                            }//for sr savedRemoved

                                        }//updateType = remove

                                    //!savedChecked
                                    } else {

                                        if(updateType == Update_Type.Add){

                                            for(int s = 0; s < saved.Count; ++s ) {

                                                if(!saved[s].present){

                                                    EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local + "\n", MessageType.Error);

                                                //!present
                                                } else {

                                                    EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + "\n", MessageType.Info);

                                                }//!present

                                            }//for s saved

                                        }//updateType = add

                                        if(updateType == Update_Type.Remove){

                                            for(int sr = 0; sr < savedRemoved.Count; ++sr ) {

                                                if(!savedRemoved[sr].present){

                                                    EditorGUILayout.HelpBox("\n" + savedRemoved[sr].name + " > " + savedRemoved[sr].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local + "\n", MessageType.Info);

                                                //!present
                                                } else {

                                                    EditorGUILayout.HelpBox("\n" + savedRemoved[sr].name + " > " + savedRemoved[sr].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + "\n", MessageType.Error);

                                                }//!present

                                            }//for sr savedRemoved

                                        }//updateType = remove

                                    }//!savedChecked

                                //saved.Count > 0
                                } else {

                                    EditorGUILayout.HelpBox("\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local + "\n", MessageType.Warning);

                                }//saved.Count > 0

                            }//statusTabs = debug

                            EditorGUILayout.EndScrollView();

                        //editLibrary != null
                        } else {

                            EditorGUILayout.HelpBox("\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local + "\n", MessageType.Error);

                            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.EndScrollView();

                        }//editLibrary != null

                    }//tabs = status

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    if(editLibrary != null){

                        if(editsDone){

                            GUI.enabled = false;

                        //editsDone
                        } else {

                            GUI.enabled = true;

                        }//editsDone

                    //editLibrary != null
                    } else {

                        GUI.enabled = false;

                    }//editLibrary != null

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[1].local)){

                            Scipts_Update(Update_Type.Add);

                        }//DisplayDialog

                    }//Button

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[1].local)){

                            Scipts_Update(Update_Type.Remove);

                        }//DisplayDialog

                    }//Button

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();

                    if(updateType == Update_Type.Add){

                        if(saved.Count > 0){

                            if(editsDone){

                                GUI.enabled = true;

                            //editsDone
                            } else {

                                GUI.enabled = false;

                            }//editsDone

                        //saved.Count > 0
                        } else {

                            GUI.enabled = false;

                        }//saved.Count > 0

                    }//updateType = add

                    if(updateType == Update_Type.Remove){

                        if(savedRemoved.Count > 0){

                            if(editsDone){

                                GUI.enabled = true;

                            //editsDone
                            } else {

                                GUI.enabled = false;

                            }//editsDone

                        //savedRemoved.Count > 0
                        } else {

                            GUI.enabled = false;

                        }//savedRemoved.Count > 0

                    }//updateType = remove

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)){

                        Scripts_Check();

                    }//Button

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[4].local)){

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

            if(!EditorApplication.isCompiling){

                if(editsDone && barShowing){

                    barShowing = false;
                    EditorUtility.ClearProgressBar();

                }//defineChanged & barShowing

            }//isCompiling

            EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

        }//ScriptEdit_Screen


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    ///////////////////////////////////////


        private void Scipts_Update(Update_Type newType){

            updateType = (Update_Type)newType;

            int countEdit = 0;
            int countLib = 0;
            int countTemp = 0;
            int countSave = 0;
            editsDone = false;

            savedContent = new List<Saved_Content>();
            savedContentRemove = new List<Saved_Content>();

            saved = new List<Content_Saved>();
            savedRemoved = new List<Content_Saved>();

            if(editLibrary != null){

                //ADD 

                if(newType == Update_Type.Add){

                    if(editLibrary.content.library.Count > 0){

                        for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                            countLib += 1;
                            countTemp = 0;

                            if(editLibrary.content.library[l].template != null){

                                scriptPath = File_Find(editLibrary.content.library[l].name);

                                if(scriptPath != ""){

                                    scriptContent = File.ReadAllText(scriptPath);

                                    for(int c = 0; c < editLibrary.content.library[l].template.content.Count; ++c ) {

                                        countTemp += 1;
                                        countEdit = 0;

                                        if(!scriptContent.Contains(editLibrary.content.library[l].template.content[c].edit)){

                                            if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].original)){

                                                scriptContent = scriptContent.Replace(editLibrary.content.library[l].template.content[c].original, editLibrary.content.library[l].template.content[c].edit);


                                                //CORE SAVE DATA

                                                Saved_Content tempSaveContent = new Saved_Content();
                                                Saved_Edit tempSaveEdit = new Saved_Edit();

                                                tempSaveContent.name = editLibrary.content.library[l].name;
                                                tempSaveContent.path = scriptPath;
                                                tempSaveContent.content = scriptContent;

                                                tempSaveEdit.editName = editLibrary.content.library[l].template.content[c].name;

                                                if(savedContent.Count > 0){

                                                    for(int sc = 0; sc < savedContent.Count; ++sc ) {

                                                        if(savedContent[sc].name == tempSaveContent.name){

                                                            if(savedContent[sc].edits.Count > 0){

                                                                for(int sce = 0; sce < savedContent[sc].edits.Count; ++sce ) {

                                                                    if(savedContent[sc].edits[sce].editName != tempSaveEdit.editName){

                                                                        countEdit += 1;

                                                                        if(countEdit == savedContent[sc].edits.Count){

                                                                            savedContent[sc].edits.Add(tempSaveEdit);

                                                                            break;

                                                                        }//countEdit = edits.Count

                                                                    }//editName != editName

                                                                }//for sce edits

                                                            }//edits.Count > 0

                                                        //name = name
                                                        } else {

                                                            if(sc == savedContent.Count - 1){

                                                                tempSaveContent.edits.Add(tempSaveEdit);

                                                                if(!savedContent.Contains(tempSaveContent)){

                                                                    savedContent.Add(tempSaveContent);

                                                                }//!Contains

                                                            }//sc = savedContent.Count

                                                        }//name = name

                                                    }//savedContent.Count > 0

                                                //savedContent.Count > 0
                                                } else {

                                                    tempSaveContent.edits.Add(tempSaveEdit);

                                                    if(!savedContent.Contains(tempSaveContent)){

                                                        savedContent.Add(tempSaveContent);

                                                    }//!Contains

                                                }//savedContent.Count > 0


                                                //DEBUG SAVE DATA

                                                Content_Saved tempSave = new Content_Saved();

                                                tempSave.name = editLibrary.content.library[l].name;
                                                tempSave.editName = editLibrary.content.library[l].template.content[c].name;
                                                tempSave.path = scriptPath;
                                                tempSave.content = scriptContent;

                                                if(!saved.Contains(tempSave)){

                                                    saved.Add(tempSave);

                                                }//!Contains

                                                editsDone = true;

                                               //Debug.Log("Saved Count = " + saved.Count);

                                            }//contains original

                                        //!contains edit
                                        } else {


                                            //CORE SAVE DATA

                                            Saved_Content tempSaveContent = new Saved_Content();
                                            Saved_Edit tempSaveEdit = new Saved_Edit();

                                            tempSaveContent.name = editLibrary.content.library[l].name;

                                            tempSaveEdit.editName = editLibrary.content.library[l].template.content[c].name;
                                            tempSaveEdit.present = true;

                                            if(savedContent.Count > 0){

                                                for(int sc = 0; sc < savedContent.Count; ++sc ) {

                                                    if(savedContent[sc].name == tempSaveContent.name){

                                                        if(savedContent[sc].edits.Count > 0){

                                                            for(int sce = 0; sce < savedContent[sc].edits.Count; ++sce ) {

                                                                if(savedContent[sc].edits[sce].editName != tempSaveEdit.editName){

                                                                    countEdit += 1;

                                                                    if(countEdit == savedContent[sc].edits.Count){

                                                                        savedContent[sc].edits.Add(tempSaveEdit);

                                                                        break;

                                                                    }//countEdit = edits.Count

                                                                //editName != editName
                                                                } else {

                                                                    savedContent[sc].edits[sce].present = true;

                                                                    break;

                                                                }//editName != editName

                                                            }//for sce edits

                                                        }//edits.Count > 0

                                                    //name = name
                                                    } else {

                                                        if(sc == savedContent.Count - 1){

                                                            tempSaveContent.edits.Add(tempSaveEdit);

                                                            if(!savedContent.Contains(tempSaveContent)){

                                                                savedContent.Add(tempSaveContent);

                                                            }//!Contains

                                                        }//sc = savedContent.Count

                                                    }//name = name

                                                }//savedContent.Count > 0

                                            //savedContent.Count > 0
                                            } else {

                                                tempSaveContent.edits.Add(tempSaveEdit);

                                                if(!savedContent.Contains(tempSaveContent)){

                                                    savedContent.Add(tempSaveContent);

                                                }//!Contains

                                            }//savedContent.Count > 0


                                            //DEBUG SAVE DATA

                                            Content_Saved tempSave = new Content_Saved();

                                            tempSave.name = editLibrary.content.library[l].name;
                                            tempSave.editName = editLibrary.content.library[l].template.content[c].name;
                                            tempSave.present = true;

                                            if(!saved.Contains(tempSave)){

                                                saved.Add(tempSave);

                                            }//!Contains

                                            //Debug.Log("Saved Present Count = " + saved.Count);

                                        }//!contains edit

                                        //Debug.Log("countLib = " + countLib);
                                        //Debug.Log("countTemp = " + countTemp);

                                        if(countLib == editLibrary.content.library.Count && countTemp == editLibrary.content.library[l].template.content.Count){

                                            for(int s = 0; s < saved.Count; ++s ) {

                                                if(!saved[s].present){

                                                    File.WriteAllText(saved[s].path, saved[s].content);

                                                }//!present

                                                countSave += 1;

                                                if(countSave == saved.Count){
                                                    EditorUtility.DisplayProgressBar(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[8].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[9].local, 0.5f);

                                                    barShowing = true;
                                                    editsDone = true;

                                                    AssetDatabase.Refresh();

                                                }//countSave = saved.Count

                                            }//for s saved

                                        }//count = content.Count

                                    }//for c content

                                }//scriptPath != ""

                            //template != null
                            } else {

                                Debug.Log("Template missing = " + editLibrary.content.library[l].name);

                            }//template != null

                        }//for l library

                    }//library.Count > 0

                }//newType = add


                //REMOVE

                if(newType == Update_Type.Remove){

                    if(editLibrary.content.library.Count > 0){

                        for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                            countLib += 1;
                            countTemp = 0;

                            if(editLibrary.content.library[l].template != null){

                                if(editLibrary.content.library[l].template.removable == DM_ScriptEdit_Template.Removable.Yes){

                                    scriptPath = File_Find(editLibrary.content.library[l].name);

                                    if(scriptPath != ""){

                                        scriptContent = File.ReadAllText(scriptPath);

                                        for(int c = 0; c < editLibrary.content.library[l].template.content.Count; ++c ) {

                                            countTemp += 1;

                                            if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].edit)){

                                                scriptContent = scriptContent.Replace(editLibrary.content.library[l].template.content[c].edit, editLibrary.content.library[l].template.content[c].original);


                                                //CORE SAVE DATA

                                                Saved_Content tempSaveContent = new Saved_Content();
                                                Saved_Edit tempSaveEdit = new Saved_Edit();

                                                tempSaveContent.name = editLibrary.content.library[l].name;
                                                tempSaveContent.path = scriptPath;
                                                tempSaveContent.content = scriptContent;

                                                tempSaveEdit.editName = editLibrary.content.library[l].template.content[c].name;

                                                if(savedContentRemove.Count > 0){

                                                    for(int sc = 0; sc < savedContentRemove.Count; ++sc ) {

                                                        if(savedContentRemove[sc].name == tempSaveContent.name){

                                                            if(savedContentRemove[sc].edits.Count > 0){

                                                                for(int sce = 0; sce < savedContentRemove[sc].edits.Count; ++sce ) {

                                                                    if(savedContentRemove[sc].edits[sce].editName != tempSaveEdit.editName){

                                                                        countEdit += 1;

                                                                        if(countEdit == savedContentRemove[sc].edits.Count){

                                                                            savedContentRemove[sc].edits.Add(tempSaveEdit);

                                                                            break;

                                                                        }//countEdit = edits.Count

                                                                    }//editName != editName

                                                                }//for sce edits

                                                            }//edits.Count > 0

                                                        //name = name
                                                        } else {

                                                            if(sc == savedContentRemove.Count - 1){

                                                                tempSaveContent.edits.Add(tempSaveEdit);

                                                                if(!savedContentRemove.Contains(tempSaveContent)){

                                                                    savedContentRemove.Add(tempSaveContent);

                                                                }//!Contains

                                                            }//sc = savedContentRemove.Count

                                                        }//name = name

                                                    }//savedContentRemove.Count > 0

                                                //savedContentRemove.Count > 0
                                                } else {

                                                    tempSaveContent.edits.Add(tempSaveEdit);

                                                    if(!savedContentRemove.Contains(tempSaveContent)){

                                                        savedContentRemove.Add(tempSaveContent);

                                                    }//!Contains

                                                }//savedContentRemove.Count > 0


                                                //DEBUG SAVE DATA

                                                Content_Saved tempSave = new Content_Saved();

                                                tempSave.name = editLibrary.content.library[l].name;
                                                tempSave.editName = editLibrary.content.library[l].template.content[c].name;
                                                tempSave.path = scriptPath;
                                                tempSave.content = scriptContent;
                                                tempSave.removed = true;

                                                if(!savedRemoved.Contains(tempSave)){

                                                    savedRemoved.Add(tempSave);

                                                }//!Contains

                                                editsDone = true;

                                                //Debug.Log("SavedRemoved Count = " + savedRemoved.Count);

                                            //contains edit
                                            } else {


                                                //CORE SAVE DATA

                                                Saved_Content tempSaveContent = new Saved_Content();
                                                Saved_Edit tempSaveEdit = new Saved_Edit();

                                                tempSaveContent.name = editLibrary.content.library[l].name;

                                                tempSaveEdit.editName = editLibrary.content.library[l].template.content[c].name;
                                                tempSaveEdit.present = true;

                                                if(savedContentRemove.Count > 0){

                                                    for(int sc = 0; sc < savedContentRemove.Count; ++sc ) {

                                                        if(savedContentRemove[sc].name == tempSaveContent.name){

                                                            if(savedContentRemove[sc].edits.Count > 0){

                                                                for(int sce = 0; sce < savedContentRemove[sc].edits.Count; ++sce ) {

                                                                    if(savedContentRemove[sc].edits[sce].editName != tempSaveEdit.editName){

                                                                        countEdit += 1;

                                                                        if(countEdit == savedContentRemove[sc].edits.Count){

                                                                            savedContentRemove[sc].edits.Add(tempSaveEdit);

                                                                            break;

                                                                        }//countEdit = edits.Count

                                                                    //editName != editName
                                                                    } else {

                                                                        savedContentRemove[sc].edits[sce].present = true;

                                                                        break;

                                                                    }//editName != editName

                                                                }//for sce edits

                                                            }//edits.Count > 0

                                                        //name = name
                                                        } else {

                                                            if(sc == savedContentRemove.Count - 1){

                                                                tempSaveContent.edits.Add(tempSaveEdit);

                                                                if(!savedContentRemove.Contains(tempSaveContent)){

                                                                    savedContentRemove.Add(tempSaveContent);

                                                                }//!Contains

                                                            }//sc = savedContentRemove.Count

                                                        }//name = name

                                                    }//savedContentRemove.Count > 0

                                                //savedContentRemove.Count > 0
                                                } else {

                                                    tempSaveContent.edits.Add(tempSaveEdit);

                                                    if(!savedContentRemove.Contains(tempSaveContent)){

                                                        savedContentRemove.Add(tempSaveContent);

                                                    }//!Contains

                                                }//savedContentRemove.Count > 0


                                                //DEBUG SAVE DATA

                                                Content_Saved tempSave = new Content_Saved();

                                                tempSave.name = editLibrary.content.library[l].name;
                                                tempSave.editName = editLibrary.content.library[l].template.content[c].name;

                                                if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].original)){

                                                    tempSave.origPresent = true;

                                                }//contains original

                                                tempSave.removed = false;

                                                if(!savedRemoved.Contains(tempSave)){

                                                    savedRemoved.Add(tempSave);

                                                }//!Contains

                                                //Debug.Log("SavedRemoved NOT Present Count = " + savedRemoved.Count);

                                            }//contains edit

                                            //Debug.Log("countLib = " + countLib);
                                            //Debug.Log("countTemp = " + countTemp);

                                            if(countLib == editLibrary.content.library.Count && countTemp == editLibrary.content.library[l].template.content.Count){

                                                for(int s = 0; s < savedRemoved.Count; ++s ) {

                                                    if(savedRemoved[s].removed){

                                                        File.WriteAllText(savedRemoved[s].path, savedRemoved[s].content);

                                                    }//removed

                                                    countSave += 1;

                                                    if(countSave == savedRemoved.Count){
                                                        EditorUtility.DisplayProgressBar(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[8].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[9].local, 0.5f);

                                                        barShowing = true;
                                                        editsDone = true;

                                                        AssetDatabase.Refresh();

                                                    }//countSave = savedRemoved.Count

                                                }//for s savedRemoved

                                            }//count = content.Count

                                        }//for c content

                                    }//scriptPath != ""

                                }//removable = yes

                            //template != null
                            } else {

                                Debug.Log("Template missing = " + editLibrary.content.library[l].name);

                            }//template != null

                        }//for l library

                    }//library.Count > 0

                }//newType = remove

                tabs = 1;
                statusTabs = 0;

            }//editLibrary != null

        }//Scipts_Update

        private void Scripts_Check(){

            if(editLibrary != null){


                //ADD

                if(updateType == Update_Type.Add){

                    if(editLibrary.content.library.Count > 0){

                        for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                            if(editLibrary.content.library[l].template != null){

                                scriptPath = File_Find(editLibrary.content.library[l].name);

                                if(scriptPath != ""){

                                    scriptContent = File.ReadAllText(scriptPath);

                                    for(int c = 0; c < editLibrary.content.library[l].template.content.Count; ++c ) {

                                        if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].edit)){


                                            //CORE SAVE DATA

                                            for(int sc = 0; sc < savedContent.Count; ++sc ) {

                                                if(savedContent[sc].name == editLibrary.content.library[l].name){

                                                    if(savedContent[sc].edits.Count > 0){

                                                        for(int sce = 0; sce < savedContent[sc].edits.Count; ++sce ) {

                                                            if(savedContent[sc].edits[sce].editName == editLibrary.content.library[l].template.content[c].name){

                                                                savedContent[sc].edits[sce].present = true;

                                                            }//editName != editName

                                                        }//for sce edits

                                                    }//edits.Count > 0

                                                }//name = name

                                            }//for sc savedContent


                                            //DEBUG SAVE DATA

                                            for(int s = 0; s < saved.Count; ++s ) {

                                                if(saved[s].name == editLibrary.content.library[l].name){

                                                    saved[s].present = true;

                                                }//name = name

                                            }//for s saved

                                        //contains edit
                                        } else {


                                            //CORE SAVE DATA

                                            for(int sc = 0; sc < savedContent.Count; ++sc ) {

                                                if(savedContent[sc].name == editLibrary.content.library[l].name){

                                                    if(savedContent[sc].edits.Count > 0){

                                                        for(int sce = 0; sce < savedContent[sc].edits.Count; ++sce ) {

                                                            if(savedContent[sc].edits[sce].editName == editLibrary.content.library[l].template.content[c].name){

                                                                savedContent[sc].edits[sce].present = false;

                                                            }//editName != editName

                                                        }//for sce edits

                                                    }//edits.Count > 0

                                                }//name = name

                                            }//for sc savedContent


                                            //DEBUG SAVE DATA

                                            for(int s = 0; s < saved.Count; ++s ) {

                                                if(saved[s].name == editLibrary.content.library[l].name){

                                                    saved[s].present = false;

                                                }//name = name

                                            }//for s saved

                                        }//contains edit

                                    }//for c content

                                }//scriptPath != ""

                            }//template != null

                        }//for l library

                    }//library.Count > 0

                }//updateType = add


                //REMOVE

                if(updateType == Update_Type.Remove){

                    if(editLibrary.content.library.Count > 0){

                        for(int l = 0; l < editLibrary.content.library.Count; ++l ) {

                            if(editLibrary.content.library[l].template != null){

                                scriptPath = File_Find(editLibrary.content.library[l].name);

                                if(scriptPath != ""){

                                    scriptContent = File.ReadAllText(scriptPath);

                                    for(int c = 0; c < editLibrary.content.library[l].template.content.Count; ++c ) {

                                        if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].edit)){


                                            //CORE SAVE DATA

                                            for(int sc = 0; sc < savedContentRemove.Count; ++sc ) {

                                                if(savedContentRemove[sc].name == editLibrary.content.library[l].name){

                                                    if(savedContentRemove[sc].edits.Count > 0){

                                                        for(int sce = 0; sce < savedContentRemove[sc].edits.Count; ++sce ) {

                                                            if(savedContentRemove[sc].edits[sce].editName == editLibrary.content.library[l].template.content[c].name){

                                                                savedContentRemove[sc].edits[sce].present = true;

                                                            }//editName != editName

                                                        }//for sce edits

                                                    }//edits.Count > 0

                                                }//name = name

                                            }//for sc savedContentRemove


                                            //DEBUG SAVE DATA

                                            for(int s = 0; s < savedRemoved.Count; ++s ) {

                                                if(savedRemoved[s].name == editLibrary.content.library[l].name){

                                                    savedRemoved[s].present = true;

                                                }//name = name

                                            }//for s savedRemoved

                                        //contains original
                                        } else {


                                            //CORE SAVE DATA

                                            for(int sc = 0; sc < savedContentRemove.Count; ++sc ) {

                                                if(savedContentRemove[sc].name == editLibrary.content.library[l].name){

                                                    if(savedContentRemove[sc].edits.Count > 0){

                                                        for(int sce = 0; sce < savedContentRemove[sc].edits.Count; ++sce ) {

                                                            if(savedContentRemove[sc].edits[sce].editName == editLibrary.content.library[l].template.content[c].name){

                                                                savedContentRemove[sc].edits[sce].present = false;

                                                            }//editName != editName

                                                        }//for sce edits

                                                    }//edits.Count > 0

                                                }//name = name

                                            }//for sc savedContentRemove


                                            //DEBUG SAVE DATA

                                            for(int s = 0; s < savedRemoved.Count; ++s ) {

                                                if(savedRemoved[s].name == editLibrary.content.library[l].name){

                                                    savedRemoved[s].present = false;

                                                }//name = name

                                            }//for s savedRemoved

                                        }//contains edit

                                    }//for c content

                                }//scriptPath != ""

                            }//template != null

                        }//for l library

                    }//library.Count > 0

                }//updateType = remove

            }//editLibrary != null

            savedChecked = true;

        }//Scripts_Check

        private void Notifications_Clear(){

            savedContent = new List<Saved_Content>();
            savedContentRemove = new List<Saved_Content>();

            saved = new List<Content_Saved>();  
            savedRemoved = new List<Content_Saved>();

            editsDone = false;
            savedChecked = false;

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

                    if(dmMenusLocData.dictionary[d].asset == "Script Editor"){

                        menusLocDataSlot = d;

                        //Debug.Log("Loc Data Slot = " + menusLocDataSlot);

                    }//asset = IWC

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

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[18].local);

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

                                window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Script Editor Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Puzzler Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Script Editor Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
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

                Debug.Log(fileName + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[19].local);

            }//results > 0

            return "";

        }//File_Find

        private void OnDestroy() {

            window = null;
            verNumb = "";

            if(barShowing){

                barShowing = false;
                EditorUtility.ClearProgressBar();

            }//barShowing

        }//OnDestroy


    }//DM_ScriptEditor
    
    
}//namespace

#endif
