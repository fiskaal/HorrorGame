using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_Possessed))]
    public class HFPS_PossessedEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_Possessed hfpsPoss;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsPoss = (HFPS_Possessed)target;

        }//OnEnable    

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty targetTypeRef = serializedObject.FindProperty("targetType");
            SerializedProperty playerTag = serializedObject.FindProperty("playerTag");

            SerializedProperty moveType = serializedObject.FindProperty("moveType");
            SerializedProperty moveTrans = serializedObject.FindProperty("moveTrans");
            SerializedProperty startMove = serializedObject.FindProperty("startMove");

            SerializedProperty destroyCall = serializedObject.FindProperty("destroyCall");
            SerializedProperty destroyType = serializedObject.FindProperty("destroyType");

            SerializedProperty events = serializedObject.FindProperty("events");

            SerializedProperty target = serializedObject.FindProperty("target");
            SerializedProperty moveState = serializedObject.FindProperty("moveState");

            var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};

            if(oldSkin == null){

                if(oldSkin != Resources.Load("EditorContent/Components Skin") as GUISkin){

                    oldSkin = GUI.skin;

                    //Debug.Log("Old Skin Name " + GUI.skin.name);

                }//oldSkin != Components Skin

            }//oldSkin == null

            GUI.skin = Resources.Load("EditorContent/Components Skin") as GUISkin;

            Texture2D t = (Texture2D)Resources.Load("EditorContent/Components-Editor-Icon");
            Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
            Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");

            GUILayout.BeginHorizontal("Possessed", "HeaderText");

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

            hfpsPoss.tabs = GUILayout.SelectionGrid(hfpsPoss.tabs, new string[] { "User Options", "Events", "Auto/Debug"}, 3);

            if(hfpsPoss.tabs == 0){

                EditorGUILayout.Space();

                hfpsPoss.startOpts = GUILayout.Toggle(hfpsPoss.startOpts, "Start Options", GUI.skin.button);

                if(hfpsPoss.startOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "If TRUE possession auto starts on scene start or instantiation." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsPoss.autoStart = EditorGUILayout.Toggle("Auto Start?", hfpsPoss.autoStart);

                    if(hfpsPoss.autoStart){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "If TRUE possession start is delayed." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsPoss.useStartDelay = EditorGUILayout.Toggle("Use Start Delay?", hfpsPoss.useStartDelay);

                        if(hfpsPoss.useStartDelay){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The wait time before possession starts." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsPoss.startDelay = EditorGUILayout.FloatField("Start Delay", hfpsPoss.startDelay);

                        }//useStartDelay

                    }//autoStart

                }//startOpts

                EditorGUILayout.Space();

                hfpsPoss.targetOpts = GUILayout.Toggle(hfpsPoss.targetOpts, "Target Options", GUI.skin.button);

                if(hfpsPoss.targetOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The type of target to use, custom requires manual target assignment via action call." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(targetTypeRef, true);

                    if(hfpsPoss.targetType == HFPS_Possessed.Target_Type.Player){

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The tag used by the player." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(playerTag, true);

                    }//targetType = player

                }//targetOpts

                EditorGUILayout.Space();

                hfpsPoss.moveOpts = GUILayout.Toggle(hfpsPoss.moveOpts, "Movement Options", GUI.skin.button);

                if(hfpsPoss.moveOpts){

                    EditorGUILayout.Space();

                    hfpsPoss.moveTabs = GUILayout.SelectionGrid(hfpsPoss.moveTabs, new string[] { "General", "Start", "Movement"}, 3);

                    if(hfpsPoss.moveTabs == 0){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The type of movement this possession will use." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(moveType, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The transform that is moved." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(moveTrans, true);

                    }//moveTabs = general

                    if(hfpsPoss.moveTabs == 1){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "If TRUE possession moves to an initial move point before moving towards the target." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsPoss.useStartMove = EditorGUILayout.Toggle("Use Start Move?", hfpsPoss.useStartMove);

                        if(hfpsPoss.useStartMove){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The initial move point." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(startMove, true);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The wait time after initial movement is done, before moving towards target." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsPoss.startMoveFinishDelay = EditorGUILayout.FloatField("StartMove FinishDelay", hfpsPoss.startMoveFinishDelay);

                        }//useStartMove

                    }//moveTabs = start

                    if(hfpsPoss.moveTabs == 2){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The speed in which the object is moved." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsPoss.moveSpeed = EditorGUILayout.FloatField("Move Speed", hfpsPoss.moveSpeed);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "If TRUE uses rotation movement." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsPoss.useRotation = EditorGUILayout.Toggle("Use Rotation?", hfpsPoss.useRotation);

                        if(hfpsPoss.useRotation){

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "The speed in which the object is rotated." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsPoss.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", hfpsPoss.rotationSpeed);

                        }//useRotation

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The max distance the possession can check for movement." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsPoss.moveRange_Max = EditorGUILayout.FloatField("Move Range Max", hfpsPoss.moveRange_Max);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The stop distance used for distance from possession to target." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsPoss.moveRange_Stop = EditorGUILayout.FloatField("Move Range Stop", hfpsPoss.moveRange_Stop);

                    }//moveTabs = movement

                }//moveOpts

                EditorGUILayout.Space();

                hfpsPoss.destOpts = GUILayout.Toggle(hfpsPoss.destOpts, "Destroy Options", GUI.skin.button);

                if(hfpsPoss.destOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Sets how destroy is called." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(destroyCall, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The type of destroy to use." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(destroyType, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The wait time before destroy occurs." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    hfpsPoss.destroyWait = EditorGUILayout.FloatField("Destroy Wait", hfpsPoss.destroyWait);

                }//destOpts

            }//tabs = user options

            if(hfpsPoss.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(events, true);

            }//tabs = events

            if(hfpsPoss.tabs == 2){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(target, true);
                EditorGUILayout.PropertyField(moveState, true);

                EditorGUILayout.Space();

                hfpsPoss.canMove = EditorGUILayout.Toggle("Can Move?", hfpsPoss.canMove);

                hfpsPoss.moveTo_Distance = EditorGUILayout.FloatField("Move To Distance", hfpsPoss.moveTo_Distance);

                hfpsPoss.moving = EditorGUILayout.Toggle("Moving?", hfpsPoss.moving);
                hfpsPoss.stopped = EditorGUILayout.Toggle("Stopped?", hfpsPoss.stopped);

                EditorGUILayout.Space();

                hfpsPoss.locked = EditorGUILayout.Toggle("Locked?", hfpsPoss.locked);

            }//tabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsPoss);

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


    }//HFPS_PossessedEditor


}//namespace