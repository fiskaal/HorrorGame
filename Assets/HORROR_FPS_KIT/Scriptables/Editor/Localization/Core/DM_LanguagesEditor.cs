using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Extension;

[CustomEditor(typeof(DM_Languages))]
public class DM_LanguagesEditor : Editor {


//////////////////////////
//
//      EDITOR DISPLAY
//
//////////////////////////
    
    
    DM_Languages dmLangs;
    
    private void OnEnable() {
        
        dmLangs = (DM_Languages)target;
        
    }//OnEnable
    
    public override void OnInspectorGUI() { 
    
        serializedObject.Update();
    
        EditorGUI.BeginChangeCheck();

        SerializedProperty languageTypeRef = serializedObject.FindProperty("languageType");
        SerializedProperty languagesRef = serializedObject.FindProperty("languages");
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(languageTypeRef, true);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(languagesRef, true);
        
        if(dmLangs.languageType != DM_Languages.Language_Type.Core){

            #if DM_LOCALIZATION

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUILayout.BeginHorizontal();

                if(GUILayout.Button("Add Languages")) {

                    Launch_LocUpdater();

                }//Button

                EditorGUILayout.EndHorizontal();
        
            #endif
        
        }//languageType != core
        
        if(EditorGUI.EndChangeCheck()){

            serializedObject.ApplyModifiedProperties();

        }//EndChangeCheck

        if(GUI.changed){

            EditorUtility.SetDirty(dmLangs);

        }//changed
        
    }//OnInspectorGUI
    
    
//////////////////////////
//
//      EDITOR ACTIONS
//
//////////////////////////

    #if DM_LOCALIZATION
    
        public void Launch_LocUpdater(){

            Diaries_LocUpdater window = (Diaries_LocUpdater)EditorWindow.GetWindow<Diaries_LocUpdater>(false, "Localization Updater", true);
            window.OpenWizard_Single();

            window.useType = Diaries_LocUpdater.Use_Type.Language;

            if(window.languages == null){

                window.languages = dmLangs;

            }//languages = null

        }//Launch_LocUpdater
    
    #endif

}//DM_LanguagesEditor
