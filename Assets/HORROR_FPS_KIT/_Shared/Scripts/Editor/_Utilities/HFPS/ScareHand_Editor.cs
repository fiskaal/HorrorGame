using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.Utility {

    [CustomEditor(typeof(ScareHand))]
    public class ScareHand_Editor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        ScareHand scareHand;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            scareHand = (ScareHand)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty useTypeRef = serializedObject.FindProperty("useType");
            SerializedProperty jumpscareEffectsRef = serializedObject.FindProperty("jumpscareEffects");

            SerializedProperty cameraShakeRef = serializedObject.FindProperty("cameraShake");
            SerializedProperty shakePreset = serializedObject.FindProperty("shakePreset");
            SerializedProperty PositionInfluence = serializedObject.FindProperty("PositionInfluence");
            SerializedProperty RotationInfluence = serializedObject.FindProperty("RotationInfluence");

            SerializedProperty JumpscareSound = serializedObject.FindProperty("JumpscareSound");
            SerializedProperty ScaredBreath = serializedObject.FindProperty("ScaredBreath");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/DM Utility Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != DM Utility Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/DM_ScareHand-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Scare Handler", "HeaderText");

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

            scareHand.tabs = GUILayout.SelectionGrid(scareHand.tabs, new string[] { "User Options", "Auto/Debug"}, 2);        

            if(scareHand.tabs == 0){

                EditorGUILayout.Space();

                scareHand.genOpts = GUILayout.Toggle(scareHand.genOpts, "General Options", GUI.skin.button);

                if(scareHand.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets how many times this Scare Handler can be used / called to." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(useTypeRef, true);

                    scareHand.disableArmsLock = EditorGUILayout.Toggle("Disable Arms Lock?", scareHand.disableArmsLock);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE delays screen effects and shake." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    scareHand.delayFxNshake = EditorGUILayout.Toggle("Delay FX & Shake?", scareHand.delayFxNshake);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The wait time before screen effects and shake start." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    scareHand.delayFxWait = EditorGUILayout.FloatField("Delay FX Wait", scareHand.delayFxWait);

                }//genOpts

                EditorGUILayout.Space();

                scareHand.effOpts = GUILayout.Toggle(scareHand.effOpts, "Effects Options", GUI.skin.button);

                if(scareHand.effOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets if screen effects should be used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(jumpscareEffectsRef, true);

                    if(scareHand.jumpscareEffects == ScareHand.JSEffects.On){

                        EditorGUILayout.Space();

                        scareHand.chromaticAberrationAmount = EditorGUILayout.FloatField("Chromatic Aberration Amount", scareHand.chromaticAberrationAmount);
                        scareHand.vignetteAmount = EditorGUILayout.FloatField("Vignette Amount", scareHand.vignetteAmount);
                        scareHand.effectsTime = EditorGUILayout.FloatField("Effects Time", scareHand.effectsTime);
                        scareHand.scaredBreathTime = EditorGUILayout.FloatField("Scared Breath Time", scareHand.scaredBreathTime);

                    }//jumpscareEffects = on

                }//effOpts

                EditorGUILayout.Space();

                scareHand.shakeOpts = GUILayout.Toggle(scareHand.shakeOpts, "Shake Options", GUI.skin.button);

                if(scareHand.shakeOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets if screen shake should be used." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(cameraShakeRef, true);

                    if(scareHand.cameraShake == ScareHand.Camera_Shake.On){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "If TRUE screen shake uses a defined preset, if FALSE screen shake uses custom values below." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        scareHand.shakeByPreset = EditorGUILayout.Toggle("Shake By Preset?", scareHand.shakeByPreset);

                        if(scareHand.shakeByPreset){

                            EditorGUILayout.PropertyField(shakePreset, true);

                        //shakeByPreset
                        } else {

                            EditorGUILayout.Space();

                            scareHand.magnitude = EditorGUILayout.FloatField("Magnitude", scareHand.magnitude);
                            scareHand.roughness = EditorGUILayout.FloatField("Roughness", scareHand.roughness);
                            scareHand.startTime = EditorGUILayout.FloatField("Start Time", scareHand.startTime);
                            scareHand.durationTime = EditorGUILayout.FloatField("Duration Time", scareHand.durationTime);

                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(PositionInfluence, true);
                            EditorGUILayout.PropertyField(RotationInfluence, true);

                        }//shakeByPreset

                    }//cameraShake = on

                }//shakeOpts

                EditorGUILayout.Space();

                scareHand.audioOpts = GUILayout.Toggle(scareHand.audioOpts, "Audio Options", GUI.skin.button);

                if(scareHand.audioOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE plays audio sounds below." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    scareHand.useJumpscareSound = EditorGUILayout.Toggle("Use Jumpscare Sound?", scareHand.useJumpscareSound);

                    if(scareHand.useJumpscareSound){

                        EditorGUILayout.PropertyField(JumpscareSound, true);
                        EditorGUILayout.PropertyField(ScaredBreath, true);

                        scareHand.scareVolume = EditorGUILayout.FloatField("Scare Volume", scareHand.scareVolume);

                    }//useJumpscareSound

                }//audioOpts

            }//tabs = user options

            if(scareHand.tabs == 1){

                if(scareHand.useType == ScareHand.Use_Type.SingleUse){

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    scareHand.isPlayed = EditorGUILayout.Toggle("isPlayed?", scareHand.isPlayed);

                //useType = single use
                } else {

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "There are no automatic values being handled with the current settings." + "\n", MessageType.Info);

                }//useType = single use

            }//tabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(scareHand);

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


    }//ScareHand_Editor


}//namespace
