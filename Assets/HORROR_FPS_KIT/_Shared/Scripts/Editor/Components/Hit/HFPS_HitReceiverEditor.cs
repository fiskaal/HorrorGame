using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

#if COMPONENTS_PRESENT

    using DizzyMedia.HFPS_Components;

#endif

namespace DizzyMedia.Shared {

    [CustomEditor(typeof(HFPS_HitReceiver))]
    public class HFPS_HitReceiverEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_HitReceiver hfpsHitRec;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsHitRec = (HFPS_HitReceiver)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty onHit = serializedObject.FindProperty("onHit");
            SerializedProperty onRayHit = serializedObject.FindProperty("onRayHit");

            SerializedProperty messages = serializedObject.FindProperty("messages");

            SerializedProperty destroyable = serializedObject.FindProperty("destroyable");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/DM Utility Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != DM Utility Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/DM Utility Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/DM_Utility-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Hit Receiver", "HeaderText");

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

            hfpsHitRec.tabs = GUILayout.SelectionGrid(hfpsHitRec.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            if(hfpsHitRec.tabs == 0){

                EditorGUILayout.Space();

                hfpsHitRec.startOpts = GUILayout.Toggle(hfpsHitRec.startOpts, "Start Options", GUI.skin.button);

                if(hfpsHitRec.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE uses an initial unlock wait." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsHitRec.useDelayUnlock = EditorGUILayout.Toggle("Use Delay Unlock?", hfpsHitRec.useDelayUnlock);

                    if(hfpsHitRec.useDelayUnlock){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before unlock occurs." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsHitRec.unlockDelay = EditorGUILayout.FloatField("Unlock Delay", hfpsHitRec.unlockDelay);

                    }//useDelayUnlock

                }//startOpts
                
                #if COMPONENTS_PRESENT

                    EditorGUILayout.Space();

                    hfpsHitRec.genOpts = GUILayout.Toggle(hfpsHitRec.genOpts, "General Options", GUI.skin.button);

                    if(hfpsHitRec.genOpts){

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(destroyable, true);

                    }//genOpts
                
                #endif

                EditorGUILayout.Space();

                hfpsHitRec.messOpts = GUILayout.Toggle(hfpsHitRec.messOpts, "Messages Options", GUI.skin.button);

                if(hfpsHitRec.messOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(messages, true);

                }//messOpts

            }//tabs = user options

            if(hfpsHitRec.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(onHit, true);
                EditorGUILayout.PropertyField(onRayHit, true);

            }//tabs = events

            if(hfpsHitRec.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                hfpsHitRec.locked = EditorGUILayout.Toggle("Locked?", hfpsHitRec.locked);

            }//tabs = auto 

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsHitRec);

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


    }//HFPS_HitReceiverEditor


}//namespace