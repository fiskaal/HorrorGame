using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [CustomEditor(typeof(HFPS_Destroyable))]
    public class HFPS_DestroyableEditor : Editor {


    //////////////////////////
    //
    //      EDITOR DISPLAY
    //
    //////////////////////////


        HFPS_Destroyable hfpsDest;
        GUISkin oldSkin;

        public bool showTips;

        private void OnEnable() {

            hfpsDest = (HFPS_Destroyable)target;

        }//OnEnable

        public override void OnInspectorGUI() {

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Space(15); 

            EditorGUI.BeginChangeCheck();

            SerializedProperty parent = serializedObject.FindProperty("parent");
            SerializedProperty collider = serializedObject.FindProperty("collider");

            SerializedProperty damageTypeRef = serializedObject.FindProperty("damageType");
            SerializedProperty breakTypeRef = serializedObject.FindProperty("breakType");

            SerializedProperty healthTypeRef = serializedObject.FindProperty("healthType");

            SerializedProperty anim = serializedObject.FindProperty("anim");
            SerializedProperty damageClip = serializedObject.FindProperty("damageClip");
            SerializedProperty breakClip = serializedObject.FindProperty("breakClip");

            SerializedProperty explodeAttributeRef = serializedObject.FindProperty("explodeAttribute");
            SerializedProperty radial = serializedObject.FindProperty("radial");

            SerializedProperty breakStages = serializedObject.FindProperty("breakStages");
            SerializedProperty destroyTypeRef = serializedObject.FindProperty("destroyType");
            SerializedProperty soundLibrary = serializedObject.FindProperty("soundLibrary");

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

            GUILayout.BeginHorizontal("Destroyable", "HeaderText");

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

            hfpsDest.tabs = GUILayout.SelectionGrid(hfpsDest.tabs, new string[] { "User Options", "Auto/Debug"}, 2);

            if(hfpsDest.tabs == 0){

                EditorGUILayout.Space();

                hfpsDest.genOpts = GUILayout.Toggle(hfpsDest.genOpts, "General Options", GUI.skin.button);

                if(hfpsDest.genOpts){

                    EditorGUILayout.Space();

                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The type of damage that will occur." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(damageTypeRef, true);

                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The type of break that will occur." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(breakTypeRef, true);

                    if(hfpsDest.damageType != HFPS_Destroyable.DamageType.None && hfpsDest.breakType != HFPS_Destroyable.BreakType.None && hfpsDest.breakType != HFPS_Destroyable.BreakType.Invincible){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "If TRUE this object will be affected by explodable objects with radial damage." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        hfpsDest.affectedByExplosions = EditorGUILayout.Toggle("Affected By Explosions?", hfpsDest.affectedByExplosions);

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Parent object of main model." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(parent, true);

                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Collider of destroyable." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(collider, true);

                    }//breakType != invincible

                }//genOpts

                if(hfpsDest.damageType == HFPS_Destroyable.DamageType.Break | hfpsDest.damageType == HFPS_Destroyable.DamageType.Explode){

                    if(hfpsDest.breakType != HFPS_Destroyable.BreakType.None){

                        if(hfpsDest.breakType != HFPS_Destroyable.BreakType.Invincible){

                            EditorGUILayout.Space();

                            hfpsDest.healthOpts = GUILayout.Toggle(hfpsDest.healthOpts, "Health Options", GUI.skin.button);

                            if(hfpsDest.healthOpts){

                                EditorGUILayout.Space();

                                if(showTips){

                                    EditorGUILayout.HelpBox("\n" + "The type of health used." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(healthTypeRef, true);

                                if(hfpsDest.healthType == HFPS_Destroyable.HealthType.Health){

                                    EditorGUILayout.Space();

                                    if(showTips){

                                        EditorGUILayout.HelpBox("\n" + "Minimum health destroyable will use." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsDest.minHealth = EditorGUILayout.IntField("Min Health", hfpsDest.minHealth);

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "Maximum health destroyable will use." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsDest.maxHealth = EditorGUILayout.IntField("Max Health", hfpsDest.maxHealth);

                                }//healthType = health

                            }//healthOpts

                        }//breakType != invincible

                        EditorGUILayout.Space();

                        hfpsDest.animOpts = GUILayout.Toggle(hfpsDest.animOpts, "Animation Options", GUI.skin.button);

                        if(hfpsDest.animOpts){

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "If TRUE uses an animation for the set actions below." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            hfpsDest.useAnimation = EditorGUILayout.Toggle("Use Animation?", hfpsDest.useAnimation);

                            if(hfpsDest.useAnimation){

                                EditorGUILayout.PropertyField(anim, true);

                                if(hfpsDest.healthType == HFPS_Destroyable.HealthType.Health | hfpsDest.breakType == HFPS_Destroyable.BreakType.Invincible){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "The animation used when object receives damage." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    EditorGUILayout.PropertyField(damageClip, true);

                                }//healthType = health or breakType = invincible

                                if(hfpsDest.breakType != HFPS_Destroyable.BreakType.None && hfpsDest.breakType != HFPS_Destroyable.BreakType.Invincible){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "The animation used when object breaks." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    EditorGUILayout.PropertyField(breakClip, true);

                                }//breakType != invincible

                            }//useAnimation

                        }//animOpts

                        if(hfpsDest.breakType != HFPS_Destroyable.BreakType.Invincible && hfpsDest.damageType == HFPS_Destroyable.DamageType.Explode){

                            EditorGUILayout.Space();

                            hfpsDest.explOpts = GUILayout.Toggle(hfpsDest.explOpts, "Explosion Options", GUI.skin.button);

                            if(hfpsDest.explOpts){

                                EditorGUILayout.Space();

                                if(showTips){

                                    EditorGUILayout.HelpBox("\n" + "The type of explosion that occurs." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(explodeAttributeRef, true);

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "The wait time before explosion occurs (used with multi stages)" + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                hfpsDest.explodeWait = EditorGUILayout.FloatField("Explode Wait", hfpsDest.explodeWait);

                                if(hfpsDest.explodeAttribute == HFPS_Destroyable.ExplodeAttribute.Radial){

                                    EditorGUILayout.Space();

                                    if(showTips){

                                        EditorGUILayout.HelpBox("\n" + "Radial trigger used for explosion." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    EditorGUILayout.PropertyField(radial, true);

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "Trigger size for explosion." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsDest.radialSize = EditorGUILayout.FloatField("Radial Size", hfpsDest.radialSize);

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "Multiplier used for radial size increase over time." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsDest.radialMulti = EditorGUILayout.FloatField("Radial Multiplier", hfpsDest.radialMulti);

                                }//explodeAttribute = radial

                            }//explOpts

                        }//damageType = explode

                        if(hfpsDest.breakType != HFPS_Destroyable.BreakType.Invincible){

                            EditorGUILayout.Space();

                            hfpsDest.stageOpts = GUILayout.Toggle(hfpsDest.stageOpts, "Stage Options", GUI.skin.button);

                            if(hfpsDest.stageOpts){

                                EditorGUILayout.Space();

                                if(showTips){

                                    EditorGUILayout.HelpBox("\n" + "Stages used for break actions." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(breakStages, true);

                            }//stageOpts

                            EditorGUILayout.Space();

                            hfpsDest.destOpts = GUILayout.Toggle(hfpsDest.destOpts, "Destroy Options", GUI.skin.button);

                            if(hfpsDest.destOpts){

                                EditorGUILayout.Space();

                                if(showTips){

                                    EditorGUILayout.HelpBox("\n" + "The type of destroy used." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(destroyTypeRef, true);

                                if(hfpsDest.destroyType == HFPS_Destroyable.DestroyType.AfterTime){

                                    if(showTips){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.HelpBox("\n" + "The wait time before destroy occurs." + "\n", MessageType.Info);

                                        EditorGUILayout.Space();

                                    }//showTips

                                    hfpsDest.destroyWait = EditorGUILayout.FloatField("Destroy Wait", hfpsDest.destroyWait);

                                }//destroyType = after time

                            }//destOpts

                        }//breakType != invincible

                    }//breakType != none and = invincible

                }//breakType != none

                EditorGUILayout.Space();

                hfpsDest.soundOpts = GUILayout.Toggle(hfpsDest.soundOpts, "Sounds Options", GUI.skin.button);

                if(hfpsDest.soundOpts){

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(soundLibrary, true);

                }//soundOpts

            }//tabs = user options

            if(hfpsDest.tabs == 1){

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();

                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.Space();

                hfpsDest.tempSound = EditorGUILayout.IntField("Temp Sound", hfpsDest.tempSound);
                hfpsDest.curStage = EditorGUILayout.IntField("Cur Stage", hfpsDest.curStage);
                hfpsDest.curHealth = EditorGUILayout.IntField("Cur Health", hfpsDest.curHealth);

                EditorGUILayout.Space();

                hfpsDest.updateRadial = EditorGUILayout.Toggle("Update Radial?", hfpsDest.updateRadial);
                hfpsDest.locked = EditorGUILayout.Toggle("Locked?", hfpsDest.locked);

            }//tabs = auto

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()){

                serializedObject.ApplyModifiedProperties();

            }//EndChangeCheck

            if(GUI.changed){

                EditorUtility.SetDirty(hfpsDest);

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


    }//HFPS_DestroyableEditor


}//namespace