using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.Extension {

    [CustomEditor(typeof(DM_WeaponCreator_Template))]
    public class DM_WeaponCreator_TemplateEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        DM_WeaponCreator_Template weapCreateTemp;

        private void OnEnable() {

            weapCreateTemp = (DM_WeaponCreator_Template)target;

        }//OnEnable

        public override void OnInspectorGUI() { 

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            SerializedProperty weaponName = serializedObject.FindProperty("weaponName");
            SerializedProperty weaponTypeRef = serializedObject.FindProperty("weaponType");

            SerializedProperty armsPrefab = serializedObject.FindProperty("armsPrefab");

            SerializedProperty candleSettings = serializedObject.FindProperty("candleSettings");
            SerializedProperty flashlightSettings = serializedObject.FindProperty("flashlightSettings");
            SerializedProperty lanternSettings = serializedObject.FindProperty("lanternSettings");
            SerializedProperty lighterSettings = serializedObject.FindProperty("lighterSettings");
            SerializedProperty meleeSettings = serializedObject.FindProperty("meleeSettings");
            SerializedProperty gunSettings = serializedObject.FindProperty("gunSettings");

            //SerializedProperty animationEvents = serializedObject.FindProperty("animationEvents");
            SerializedProperty animationSoundEvents = serializedObject.FindProperty("animationSoundEvents");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(weaponName, true);
            EditorGUILayout.PropertyField(weaponTypeRef, true);

            EditorGUILayout.PropertyField(armsPrefab, true);

            EditorGUILayout.Space();

            if(weapCreateTemp.weaponType == HFPS_WeaponCreator.Weapon_Type.Candle){

                EditorGUILayout.PropertyField(candleSettings, true);

            }//weaponType = candle

            if(weapCreateTemp.weaponType == HFPS_WeaponCreator.Weapon_Type.Flashlight){

                EditorGUILayout.PropertyField(flashlightSettings, true);

                EditorGUILayout.PropertyField(animationSoundEvents, true);

            }//weaponType = flashlight

            if(weapCreateTemp.weaponType == HFPS_WeaponCreator.Weapon_Type.Lantern){

                EditorGUILayout.PropertyField(lanternSettings, true);

            }//weaponType = lantern

            if(weapCreateTemp.weaponType == HFPS_WeaponCreator.Weapon_Type.Lighter){

                EditorGUILayout.PropertyField(lighterSettings, true);

            }//weaponType = lighter

            if(weapCreateTemp.weaponType == HFPS_WeaponCreator.Weapon_Type.Melee){

                EditorGUILayout.PropertyField(meleeSettings, true);

            }//weaponType = melee

            if(weapCreateTemp.weaponType == HFPS_WeaponCreator.Weapon_Type.Gun){

                EditorGUILayout.PropertyField(gunSettings, true);

                EditorGUILayout.PropertyField(animationSoundEvents, true);

            }//weaponType = gun

            //EditorGUILayout.PropertyField(animationEvents, true);

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(weapCreateTemp);

            }//changed

        }//OnInspectorGUI    


    }//DM_WeaponCreator_TemplateEditor
    

}//namespace
