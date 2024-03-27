using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(DM_MenusLocData))]
public class DM_MenusLocDataEditor : Editor {


//////////////////////////
//
//      EDITOR DISPLAY
//
//////////////////////////
    
    
    DM_MenusLocData menusLocData;
    
    private void OnEnable() {
        
        menusLocData = (DM_MenusLocData)target;
        
    }//OnEnable
    
    public override void OnInspectorGUI() { 
    
        serializedObject.Update();
    
        EditorGUI.BeginChangeCheck();

        SerializedProperty currentLanguage = serializedObject.FindProperty("currentLanguage");
        SerializedProperty dictionary = serializedObject.FindProperty("dictionary");
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(currentLanguage, true);
        EditorGUILayout.PropertyField(dictionary, true);
        
        if(EditorGUI.EndChangeCheck()){

            serializedObject.ApplyModifiedProperties();

        }//EndChangeCheck

        if(GUI.changed){

            EditorUtility.SetDirty(menusLocData);

        }//changed
        
    }//OnInspectorGUI

}//DM_MenusLocDataEditor
