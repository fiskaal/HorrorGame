#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

using DizzyMedia.Version;

namespace DizzyMedia.Extension {

    public class DM_RagdollCreator : EditorWindow {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    //////////////////////////////////////


        [System.Serializable]
        public class HumanParts {

            public Transform Pelvis;

            [Space]

            public Transform LeftHips;
            public Transform LeftKnee;
            public Transform LeftFoot;

            [Space]

            public Transform RightHips;
            public Transform RightKnee;
            public Transform RightFoot;

            [Space]

            public Transform LeftArm;
            public Transform LeftElbow;
            public Transform LeftHand;

            [Space]

            public Transform RightArm;
            public Transform RightElbow;
            public Transform RightHand;

            [Space]

            public Transform Chest;

            [Space]

            public Transform Head;

        }//HumanParts

        [System.Serializable]
        public class RagdollSettings {

            [Space]

            public float totalWeight = 60;
            public bool createEnds = true;

            [Space]

            public bool enabledState = true;
            public bool isTrigger;
            public bool isKinematic;
            public bool useGravity = true;

            [Space]

            public float rigidDrag = 0.3f;
            public float rigidAngularDrag = 0.3f;

            [Space]

            public CollisionDetectionMode collisionMode;

        }//RagdollSettings

        public struct WeightCalculator {

            public readonly float Pelvis;
            public readonly float Hip;
            public readonly float Knee;
            public readonly float Foot;
            public readonly float Arm;
            public readonly float Elbow;
            public readonly float Hand;
            public readonly float Chest;
            public readonly float Head;

            public WeightCalculator(float totalWeight, bool withEnds) {

                Pelvis = totalWeight * 0.20f;
                Chest = totalWeight * 0.20f;
                Head = totalWeight * 0.05f;

                if(withEnds) {

                    Hip = totalWeight * 0.20f / 2f;
                    Knee = totalWeight * 0.15f / 2f;
                    Foot = totalWeight * 0.05f / 2f;

                    Arm = totalWeight * 0.08f / 2f;
                    Elbow = totalWeight * 0.05f / 2f;
                    Hand = totalWeight * 0.02f / 2f;

                //withEnds
                } else {

                    Hip = totalWeight * 0.20f / 2f;
                    Knee = totalWeight * 0.20f / 2f;
                    Foot = 0f;

                    Arm = totalWeight * 0.08f / 2f;
                    Elbow = totalWeight * 0.07f / 2f;
                    Hand = 0f;

                }//withEnds

                float checkSum =

                    Pelvis +
                    Hip * 2f +
                    Knee * 2f +
                    Foot * 2f +
                    Arm * 2f +
                    Elbow * 2f +
                    Hand * 2f +
                    Chest +
                    Head;

                if(Mathf.Abs(totalWeight - checkSum) > Mathf.Epsilon){

                    Debug.LogError("totalWeight != checkSum (" + totalWeight.ToString() + ", " + checkSum.ToString() + ")");

                }//abs

            }//WeightCalculator

        }//WeightCalculator


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static DM_RagdollCreator window;
        private static Vector2 windowsSize = new Vector2(400, 600);

        public HumanParts humanParts;
        private List<Transform> humanPartsTemp = new List<Transform>();

        public RagdollSettings ragdollSettings;

        private static DM_Version dmVersion;
        private static string versionName = "RagdollCreator Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        Animator tempAnim;
        public Vector3 playDir;
        public Transform rootTrans;

        public bool ragdollCreated;

        bool useDebug;
        int debugInt;

        int wizardTabCount;
        int configTypeInt;
        Vector2 scrollPos;


    //////////////////////////////////////
    ///
    ///     MENU
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/Character/Ragdoll Creator", false, 13)]
        public static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<DM_RagdollCreator>(false, "Ragdoll Creator" + " v" + verNumb, true);
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


    /////////////////////////////////////////////////////////////////////////////
    ///
    ///     EDITOR GUI
    ///
    //////////////////////////////////////////////////////////////////////////////


        private void OnGUI() {

            Ragdoll_Screen();

        }//OnGUI

        private void Ragdoll_Screen(){

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t0 = (Texture)Resources.Load("EditorContent/RagdollCreator/RagdollCreator_Header");

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));

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

                    wizardTabCount = GUILayout.SelectionGrid(wizardTabCount, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].local}, 2);

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if(wizardTabCount == 0){

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Info);

                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, EditorStyles.centeredGreyMiniLabel);

                        EditorGUILayout.Space();

                        configTypeInt = GUILayout.SelectionGrid(configTypeInt, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].local}, 2);

                        EditorGUILayout.Space();

                        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        if(configTypeInt == 0){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[0].text, MessageType.Info);

                        }//configTypeInt == 0

                        if(configTypeInt == 1){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[0].text, MessageType.Info);

                        }//configTypeInt == 1

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        if(configTypeInt == 0){

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[0].local, EditorStyles.centeredGreyMiniLabel);

                            if(humanPartsTemp.Count < 1){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[1].text, MessageType.Info);

                            }//humanPartsTemp.Count < 1

                            EditorGUILayout.Space();

                            if(humanPartsTemp.Count == 0){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[2].text, MessageType.Info);

                            }//humanPartsTemp.Count == 0

                            if(humanPartsTemp.Count != 0 && humanPartsTemp.Count < 15){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[3].text, MessageType.Info);

                            }//humanPartsTemp.Count != 0 & humanPartsTemp.Count < 15

                            if(humanPartsTemp.Count == 15){

                                if(!ragdollCreated){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[4].text, MessageType.Info);

                                //!ragdollCreated
                                } else {

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[5].text, MessageType.Info);

                                }//!ragdollCreated

                            }//humanPartsTemp.Count == 15

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[1].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            ScriptableObject target = this;
                            SerializedObject soTar = new SerializedObject(target);
                            SerializedProperty humanParts = soTar.FindProperty("humanParts");
                            SerializedProperty ragdollSettings = soTar.FindProperty("ragdollSettings");

                            EditorGUILayout.PropertyField(humanParts, true);
                            soTar.ApplyModifiedProperties();

                            EditorGUILayout.Space();

                            if(humanPartsTemp.Count > 0){

                                EditorGUILayout.Space();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[2].local, EditorStyles.centeredGreyMiniLabel);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(ragdollSettings, true);
                                soTar.ApplyModifiedProperties();

                                EditorGUILayout.Space();

                            }//humanPartsTemp.Count > 0

                        }//configTypeInt = 0

                        if(configTypeInt == 1){

                            if(Selection.gameObjects.Length == 0){

                                if(tempAnim != null){

                                    tempAnim = null;

                                }//tempAnim != null

                                if(rootTrans != null){

                                    rootTrans = null;

                                }//rootTrans = null

                            }//Selection = 0

                            if(Selection.gameObjects.Length == 1){

                                foreach(GameObject tempObjs in Selection.gameObjects){

                                    if(tempObjs.GetComponent<Animator>() != null && tempObjs.GetComponent<Animator>().avatar != null){

                                        tempAnim = tempObjs.GetComponent<Animator>();

                                    //Animator != null
                                    } else {

                                        if(tempAnim != null){

                                            tempAnim = null;

                                        }//tempAnim != null

                                        if(rootTrans != null){

                                            rootTrans = null;

                                        }//rootTrans = null

                                    }//Animator != null

                                }//foreach tempObjs

                            }//Selection = 1

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].singleValues[0].local, EditorStyles.centeredGreyMiniLabel);

                            if(tempAnim != null){

                                if(humanPartsTemp.Count < 1 && Selection.gameObjects.Length == 1){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[2].text, MessageType.Info);

                                }//humanPartsTemp.Count < 1

                                if(Selection.gameObjects.Length == 2){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[3].text, MessageType.Info);

                                }//Selection.length = 2

                            //tempAnim != null
                            } else {

                                if(humanPartsTemp.Count < 1){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[1].text, MessageType.Info);

                                }//humanPartsTemp.Count < 1

                                if(humanPartsTemp.Count != 0 && humanPartsTemp.Count < 15){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[5].text, MessageType.Info);

                                }//Count != 0 && Count < 15

                            }//tempAnim != null

                            EditorGUILayout.Space();

                            if(humanPartsTemp.Count == 0){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[4].text, MessageType.Info);

                            }//humanPartsTemp.Count == 0

                            if(humanPartsTemp.Count != 0 && humanPartsTemp.Count < 15){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[5].text, MessageType.Info);

                            }//humanPartsTemp.Count != 0 & humanPartsTemp.Count < 15

                            if(humanPartsTemp.Count == 15){

                                if(!ragdollCreated){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[6].text, MessageType.Info);

                                //!ragdollCreated
                                } else {

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[7].text, MessageType.Info);

                                }//!ragdollCreated

                            }//humanPartsTemp.Count == 15

                            EditorGUILayout.Space();

                            if(humanPartsTemp.Count > 0){

                                ScriptableObject target = this;
                                SerializedObject soTar = new SerializedObject(target);
                                SerializedProperty ragdollSettings = soTar.FindProperty("ragdollSettings");

                                EditorGUILayout.Space();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].singleValues[1].local, EditorStyles.centeredGreyMiniLabel);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(ragdollSettings, true);
                                soTar.ApplyModifiedProperties();

                                EditorGUILayout.Space();

                            }//humanPartsTemp.Count > 0

                            EditorGUILayout.Space();

                        }//configTypeInt = 1

                        EditorGUILayout.Space();

                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        if(configTypeInt == 0){

                            if(humanPartsTemp.Count > 0){

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[0].local)) {

                                    CreateRagdoll();

                                }//Button

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[1].local)) {

                                    RegSet_New();

                                }//Button

                                EditorGUILayout.Space();

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[2].local)) {

                                    ClearHumanoidParts();

                                }//Button

                                if(ragdollCreated){

                                    GUI.enabled = true;

                                //ragdollCreated
                                } else {

                                    GUI.enabled = false;

                                }//ragdollCreated

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[3].local)) {

                                    ClearRagdoll();

                                }//Button

                            //humanPartsTemp.Count > 0
                            } else {

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[4].local)) {

                                    UploadHumanoidParts();

                                }//Button

                            }//humanPartsTemp.Count > 0

                        }//configTypeInt = 0

                        if(configTypeInt == 1){

                            if(humanPartsTemp.Count == 0){

                                if(Selection.gameObjects.Length == 1){

                                    if(tempAnim != null){

                                        GUI.enabled = true;

                                    //tempAnim != null
                                    } else {

                                        GUI.enabled = false;

                                    }//tempAnim != null

                                //Selection.length = 1
                                } else {

                                    GUI.enabled = false;

                                }//Selection.length = 1

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[2].local)) {

                                    GrabHumanoidParts();

                                }//Button

                                if(humanPartsTemp.Count > 0){

                                    GUI.enabled = true;

                                //humanPartsTemp.Count > 0
                                } else {

                                    GUI.enabled = false;

                                }//humanPartsTemp.Count > 0

                                EditorGUILayout.Space();

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[3].local)) {

                                    ClearHumanoidParts();

                                }//Button

                                if(ragdollCreated){

                                    GUI.enabled = true;

                                //ragdollCreated
                                } else {

                                    GUI.enabled = false;

                                }//ragdollCreated

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[0].local)) {

                                    ClearRagdoll();

                                }//Button

                            //humanPartsTemp.count = 0
                            } else {

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[0].local)) {

                                    CreateRagdoll();

                                }//Button

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[1].local)) {

                                    RegSet_New();

                                }//Button

                                EditorGUILayout.Space();

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[3].local)) {

                                    ClearHumanoidParts();

                                }//Button

                                if(ragdollCreated){

                                    GUI.enabled = true;

                                //ragdollCreated
                                } else {

                                    GUI.enabled = false;

                                }//ragdollCreated

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[4].local)) {

                                    ClearRagdoll();

                                }//Button

                            }//humanPartsTemp.count = 0

                        }//configTypeInt = 1

                        EditorGUILayout.Space();

                    }//wizardTabCount = ragdoll

                    if(wizardTabCount == 1){

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].texts[0].text, MessageType.Info);

                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].toggles[0].header, EditorStyles.centeredGreyMiniLabel);

                        EditorGUILayout.Space();

                        debugInt = GUILayout.SelectionGrid(debugInt, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].toggles[0].valFalse, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].toggles[0].valTrue}, 2);

                        if(debugInt == 0){

                            useDebug = false;

                        }//debugInt == 0

                        if(debugInt == 1){

                            useDebug = true;

                        }//debugInt == 1

                        EditorGUILayout.Space();

                    }//wizardTabCount = debug

                }//verNumb == "Unknown"

            //dmMenusLocData != null 
            } else {

                if(!languageLock){

                    DM_LocDataFind();

                }//!languageLock 

            }//dmMenusLocData != null

        }//Ragdoll_Screen


    /////////////////////////////////////////////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    //////////////////////////////////////////////////////////////////////////////


        void GrabHumanoidParts(){

            if(rootTrans == null){

                rootTrans = tempAnim.transform;

            }//rootTrans = null

            humanParts.Pelvis = tempAnim.GetBoneTransform(HumanBodyBones.Hips);

            humanParts.LeftHips = tempAnim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            humanParts.RightHips = tempAnim.GetBoneTransform(HumanBodyBones.RightUpperLeg);

            humanParts.LeftKnee = tempAnim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            humanParts.RightKnee = tempAnim.GetBoneTransform(HumanBodyBones.RightLowerLeg);

            humanParts.RightFoot = tempAnim.GetBoneTransform(HumanBodyBones.RightFoot);
            humanParts.LeftFoot = tempAnim.GetBoneTransform(HumanBodyBones.LeftFoot);

            humanParts.LeftArm = tempAnim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            humanParts.RightArm = tempAnim.GetBoneTransform(HumanBodyBones.RightUpperArm);

            humanParts.LeftElbow = tempAnim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            humanParts.RightElbow = tempAnim.GetBoneTransform(HumanBodyBones.RightLowerArm);

            humanParts.LeftHand = tempAnim.GetBoneTransform(HumanBodyBones.LeftHand);
            humanParts.RightHand = tempAnim.GetBoneTransform(HumanBodyBones.RightHand);

            humanParts.Chest = tempAnim.GetBoneTransform(HumanBodyBones.Chest);

            humanParts.Head = tempAnim.GetBoneTransform(HumanBodyBones.Head);


            humanPartsTemp = new List<Transform>();

            if(humanParts.Pelvis != null){

                humanPartsTemp.Add(humanParts.Pelvis);

            //Pelvis != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Pelvis");

                }//useDebug

            }//Pelvis != null

            if(humanParts.LeftHips != null){

                humanPartsTemp.Add(humanParts.LeftHips);

            //LeftHips != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Hips");

                }//useDebug

            }//LeftHips != null

            if(humanParts.LeftKnee != null){

                humanPartsTemp.Add(humanParts.LeftKnee);

            //LeftKnee != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Knee");

                }//useDebug

            }//LeftKnee != null

            if(humanParts.LeftFoot != null){

                humanPartsTemp.Add(humanParts.LeftFoot);

            //LeftFoot != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Foot");

                }//useDebug

            }//LeftFoot != null

            if(humanParts.RightHips != null){

                humanPartsTemp.Add(humanParts.RightHips);

            //RightHips != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Hips");

                }//useDebug

            }//RightHips != null

            if(humanParts.RightKnee != null){

                humanPartsTemp.Add(humanParts.RightKnee);

                    //RightKnee != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Knee");

                }//useDebug

            }//RightKnee != null

            if(humanParts.RightFoot != null){

                humanPartsTemp.Add(humanParts.RightFoot);

            //RightFoot != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Foot");

                }//useDebug

            }//RightFoot != null

            if(humanParts.LeftArm != null){

                humanPartsTemp.Add(humanParts.LeftArm);

            //LeftArm != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Arm");

                }//useDebug

            }//LeftArm != null

            if(humanParts.LeftElbow != null){

                humanPartsTemp.Add(humanParts.LeftElbow);

            //LeftElbow != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Elbow");

                }//useDebug

            }//LeftElbow != null

            if(humanParts.LeftHand != null){

                humanPartsTemp.Add(humanParts.LeftHand);

            //LeftHand != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Hand");

                }//useDebug

            }//LeftHand != null

            if(humanParts.RightArm != null){

                humanPartsTemp.Add(humanParts.RightArm);

            //RightArm != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Arm");

                }//useDebug

            }//RightArm != null

            if(humanParts.RightElbow != null){

                humanPartsTemp.Add(humanParts.RightElbow);

            //RightElbow != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Elbow");

                }//useDebug

            }//RightElbow != null

            if(humanParts.RightHand != null){

                humanPartsTemp.Add(humanParts.RightHand);

            //RightHand != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Hand");

                }//useDebug

            }//RightHand != null

            if(humanParts.Chest != null){

                humanPartsTemp.Add(humanParts.Chest);

            //Chest != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Middle Spine");

                }//useDebug

            }//Chest != null

            if(humanParts.Head != null){

                humanPartsTemp.Add(humanParts.Head);

            //Head != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Head");

                }//useDebug

            }//Head != null

            if(useDebug){

                Debug.Log("Body Parts Grabbed");

            }//useDebug

        }//GrabHumanoidParts

        void UploadHumanoidParts(){

            if(rootTrans == null){

                rootTrans = tempAnim.transform;

            }//rootTrans = null

            humanPartsTemp = new List<Transform>();

            if(humanParts.Pelvis != null){

                humanPartsTemp.Add(humanParts.Pelvis);

            //Pelvis != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Pelvis");

                }//useDebug

            }//Pelvis != null

            if(humanParts.LeftHips != null){

                humanPartsTemp.Add(humanParts.LeftHips);

            //LeftHips != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Hips");

                }//useDebug

            }//LeftHips != null

            if(humanParts.LeftKnee != null){

                humanPartsTemp.Add(humanParts.LeftKnee);

            //LeftKnee != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Knee");

                }//useDebug

            }//LeftKnee != null

            if(humanParts.LeftFoot != null){

                humanPartsTemp.Add(humanParts.LeftFoot);

            //LeftFoot != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Foot");

                }//useDebug

            }//LeftFoot != null

            if(humanParts.RightHips != null){

                humanPartsTemp.Add(humanParts.RightHips);

            //RightHips != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Hips");

                }//useDebug

            }//RightHips != null

            if(humanParts.RightKnee != null){

                humanPartsTemp.Add(humanParts.RightKnee);

                    //RightKnee != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Knee");

                }//useDebug

            }//RightKnee != null

            if(humanParts.RightFoot != null){

                humanPartsTemp.Add(humanParts.RightFoot);

            //RightFoot != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Foot");

                }//useDebug

            }//RightFoot != null

            if(humanParts.LeftArm != null){

                humanPartsTemp.Add(humanParts.LeftArm);

            //LeftArm != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Arm");

                }//useDebug

            }//LeftArm != null

            if(humanParts.LeftElbow != null){

                humanPartsTemp.Add(humanParts.LeftElbow);

            //LeftElbow != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Elbow");

                }//useDebug

            }//LeftElbow != null

            if(humanParts.LeftHand != null){

                humanPartsTemp.Add(humanParts.LeftHand);

            //LeftHand != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Left Hand");

                }//useDebug

            }//LeftHand != null

            if(humanParts.RightArm != null){

                humanPartsTemp.Add(humanParts.RightArm);

            //RightArm != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Arm");

                }//useDebug

            }//RightArm != null

            if(humanParts.RightElbow != null){

                humanPartsTemp.Add(humanParts.RightElbow);

            //RightElbow != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Elbow");

                }//useDebug

            }//RightElbow != null

            if(humanParts.RightHand != null){

                humanPartsTemp.Add(humanParts.RightHand);

            //RightHand != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Right Hand");

                }//useDebug

            }//RightHand != null

            if(humanParts.Chest != null){

                humanPartsTemp.Add(humanParts.Chest);

            //Chest != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Middle Spine");

                }//useDebug

            }//Chest != null

            if(humanParts.Head != null){

                humanPartsTemp.Add(humanParts.Head);

            //Head != null
            } else {

                if(useDebug){

                    Debug.Log("Transform Missing - Head");

                }//useDebug

            }//Head != null

            if(humanPartsTemp.Count > 0){

                if(useDebug){

                    Debug.Log("Body Parts Uploaded");

                }//useDebug

            //humanPartsTemp.Count > 0
            } else {

                if(useDebug){

                    Debug.Log("Body Parts NOT Uploaded");

                }//useDebug

            }//humanPartsTemp.Count > 0

        }//UploadHumanoidParts

        void ClearHumanoidParts(){

            if(rootTrans != null){

                rootTrans = null;

            }//rootTrans = null

            if(humanPartsTemp.Count > 0){

                humanPartsTemp.Clear();

                if(useDebug){

                    Debug.Log("Body Parts Cleared");

                }//useDebug

            //humanPartsTemp.Count > 0
            } else {

                if(useDebug){

                    Debug.Log("No Body Parts To Clear");

                }//useDebug

            }//humanPartsTemp.Count > 0

            HumanParts tempHumanParts = new HumanParts();

            humanParts = tempHumanParts;

        }//ClearHumanoidParts

        public void RegSet_New(){

            RagdollSettings tempRagSettings = new RagdollSettings();

            tempRagSettings.totalWeight = 60;
            tempRagSettings.rigidDrag = 0.3f;
            tempRagSettings.rigidAngularDrag = 0.3f;
            tempRagSettings.useGravity = true;
            tempRagSettings.createEnds = true;

            ragdollSettings = tempRagSettings;

            if(useDebug){

                Debug.Log("Ragdoll Settings Reset");

            }//useDebug

        }//RegSet_New


    /////////////////////////////////////////////////////////////////////////////
    ///
    ///     RAGDOLL ACTIONS (Create, Remove, Etc.)
    ///
    //////////////////////////////////////////////////////////////////////////////


        void CreateRagdoll(){

            if(!ragdollCreated){

                var weight = new WeightCalculator(ragdollSettings.totalWeight, ragdollSettings.createEnds);

                playDir = GetPlayerDirection();



    ///
    ///
    ///     BODY PART - PELVIS
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - PELVIS
    ///
    //////////////////////////////////////////


                BoxCollider tempColPelvis = humanParts.Pelvis.gameObject.AddComponent<BoxCollider>();
                humanParts.Pelvis.gameObject.AddComponent<Rigidbody>();

                Vector3 pelvisSize = new Vector3(0.32f, 0.31f, 0.3f);
                Vector3 pelvisCenter = new Vector3(00f, 0.06f, -0.01f);

                tempColPelvis.size = Abs(humanParts.Pelvis.InverseTransformVector(pelvisSize));
                tempColPelvis.center = humanParts.Pelvis.InverseTransformVector(pelvisCenter);

                tempColPelvis.isTrigger = ragdollSettings.isTrigger;
                tempColPelvis.enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY RIGIDBODY UPDATE - PELVIS
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_Pelvis = humanParts.Pelvis.GetComponent<Rigidbody>();

                tempRigid_Pelvis.isKinematic = ragdollSettings.isKinematic;
                tempRigid_Pelvis.useGravity = ragdollSettings.useGravity;

                tempRigid_Pelvis.mass = weight.Pelvis;

                tempRigid_Pelvis.drag = ragdollSettings.rigidDrag;
                tempRigid_Pelvis.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_Pelvis.collisionDetectionMode = ragdollSettings.collisionMode;


    ///
    ///
    ///     BODY PART - CHEST
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - CHEST
    ///
    //////////////////////////////////////////


                BoxCollider tempColChest = humanParts.Chest.gameObject.AddComponent<BoxCollider>();
                humanParts.Chest.gameObject.AddComponent<Rigidbody>();
                humanParts.Chest.gameObject.AddComponent<CharacterJoint>();

                humanParts.Chest.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.Chest.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                Vector3 chestSize = new Vector3(0.34f, 0.34f, 0.28f);

                float y = (pelvisSize.y + chestSize.y) / 2f + pelvisCenter.y;
                y -= humanParts.Chest.position.y - humanParts.Pelvis.position.y;

                tempColChest.size = Abs(humanParts.Chest.InverseTransformVector(chestSize));
                tempColChest.center = humanParts.Chest.InverseTransformVector(new Vector3(0f, y, -0.03f));

                tempColChest.isTrigger = ragdollSettings.isTrigger;
                tempColChest.enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - CHEST
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_Chest = humanParts.Chest.GetComponent<Rigidbody>();

                tempRigid_Chest.isKinematic = ragdollSettings.isKinematic;
                tempRigid_Chest.useGravity = ragdollSettings.useGravity;

                tempRigid_Chest.mass = weight.Chest;

                tempRigid_Chest.drag = ragdollSettings.rigidDrag;
                tempRigid_Chest.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_Chest.collisionDetectionMode = ragdollSettings.collisionMode;


    //////////////////////////////////////////
    ///
    ///     BODY PART JOINT UPDATE - CHEST
    ///
    //////////////////////////////////////////


                var chestJoint = humanParts.Chest.GetComponent<CharacterJoint>();

                ConfigureJointParams(chestJoint, humanParts.Chest, humanParts.Pelvis.GetComponent<Rigidbody>(), rootTrans.right, rootTrans.forward);
                ConfigureJointLimits(chestJoint, -45f, 20f, 20f, 20f);


    ///
    ///
    ///     BODY PART - HEAD
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - HEAD
    ///
    //////////////////////////////////////////


                SphereCollider tempColHead = humanParts.Head.gameObject.AddComponent<SphereCollider>();
                humanParts.Head.gameObject.AddComponent<Rigidbody>();
                humanParts.Head.gameObject.AddComponent<CharacterJoint>();

                humanParts.Head.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.Head.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                tempColHead.radius = 0.1f;
                tempColHead.center = humanParts.Head.InverseTransformVector(new Vector3(0f, 0.09f, 0.03f));

                tempColHead.isTrigger = ragdollSettings.isTrigger;
                tempColHead.enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - HEAD
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_Head = humanParts.Head.GetComponent<Rigidbody>();

                tempRigid_Head.isKinematic = ragdollSettings.isKinematic;
                tempRigid_Head.useGravity = ragdollSettings.useGravity;

                tempRigid_Head.mass = weight.Head;

                tempRigid_Head.drag = ragdollSettings.rigidDrag;
                tempRigid_Head.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_Head.collisionDetectionMode = ragdollSettings.collisionMode;


    //////////////////////////////////////////
    ///
    ///     BODY PART JOINT UPDATE - HEAD
    ///
    //////////////////////////////////////////


                var headJoint = humanParts.Head.GetComponent<CharacterJoint>();

                ConfigureJointParams(headJoint, humanParts.Head, humanParts.Chest.GetComponent<Rigidbody>(), rootTrans.right, rootTrans.forward);
                ConfigureJointLimits(headJoint, -45f, 20f, 20f, 20f);



    ///////////////////////// LEFT LOWER SIDE OF BODY SETUP /////////////////////////////////////////////




    ///
    ///
    ///     BODY PART - LeftHips
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - LeftHips
    ///
    //////////////////////////////////////////


                humanParts.LeftHips.gameObject.AddComponent<CapsuleCollider>();
                humanParts.LeftHips.gameObject.AddComponent<Rigidbody>();
                humanParts.LeftHips.gameObject.AddComponent<CharacterJoint>();

                humanParts.LeftHips.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.LeftHips.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.LeftHips.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.LeftHips.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - LeftHips
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_LeftHips = humanParts.LeftHips.GetComponent<Rigidbody>();

                tempRigid_LeftHips.isKinematic = ragdollSettings.isKinematic;
                tempRigid_LeftHips.useGravity = ragdollSettings.useGravity;

                tempRigid_LeftHips.mass = weight.Hip;

                tempRigid_LeftHips.drag = ragdollSettings.rigidDrag;
                tempRigid_LeftHips.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_LeftHips.collisionDetectionMode = ragdollSettings.collisionMode;


    ///
    ///
    ///     BODY PART - LeftKnee
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - LeftKnee
    ///
    //////////////////////////////////////////


                humanParts.LeftKnee.gameObject.AddComponent<CapsuleCollider>();
                humanParts.LeftKnee.gameObject.AddComponent<Rigidbody>();
                humanParts.LeftKnee.gameObject.AddComponent<CharacterJoint>();

                humanParts.LeftKnee.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.LeftKnee.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.LeftKnee.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.LeftKnee.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - LeftKnee
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_LeftKnee = humanParts.LeftKnee.GetComponent<Rigidbody>();

                tempRigid_LeftKnee.isKinematic = ragdollSettings.isKinematic;
                tempRigid_LeftKnee.useGravity = ragdollSettings.useGravity;

                tempRigid_LeftKnee.mass = weight.Knee;

                tempRigid_LeftKnee.drag = ragdollSettings.rigidDrag;
                tempRigid_LeftKnee.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_LeftKnee.collisionDetectionMode = ragdollSettings.collisionMode;



                if(ragdollSettings.createEnds){

    ///
    ///
    ///     BODY PART - LeftFoot
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - LeftFoot
    ///
    //////////////////////////////////////////


                    humanParts.LeftFoot.gameObject.AddComponent<BoxCollider>();
                    humanParts.LeftFoot.gameObject.AddComponent<Rigidbody>();
                    humanParts.LeftFoot.gameObject.AddComponent<CharacterJoint>();

                    humanParts.LeftFoot.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                    humanParts.LeftFoot.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                    humanParts.LeftFoot.GetComponent<BoxCollider>().isTrigger = ragdollSettings.isTrigger;
                    humanParts.LeftFoot.GetComponent<BoxCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - LeftFoot
    ///
    //////////////////////////////////////////


                    Rigidbody tempRigid_LeftFoot = humanParts.LeftFoot.GetComponent<Rigidbody>();

                    tempRigid_LeftFoot.isKinematic = ragdollSettings.isKinematic;
                    tempRigid_LeftFoot.useGravity = ragdollSettings.useGravity;

                    tempRigid_LeftFoot.mass = weight.Foot;

                    tempRigid_LeftFoot.drag = ragdollSettings.rigidDrag;
                    tempRigid_LeftFoot.angularDrag = ragdollSettings.rigidAngularDrag;

                    tempRigid_LeftFoot.collisionDetectionMode = ragdollSettings.collisionMode;


                }//createEnds

                ConfigureRagdollForLimb(humanParts.LeftHips, humanParts.LeftKnee, humanParts.LeftFoot, ragdollSettings.createEnds, false);
                ConfigureLegsJoints(humanParts.LeftHips, humanParts.LeftKnee, humanParts.LeftFoot, ragdollSettings.createEnds);


    ///////////////////////// LEFT UPPER SIDE OF BODY SETUP /////////////////////////////////////////////




    ///
    ///
    ///     BODY PART - LeftArm
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - LeftArm
    ///
    //////////////////////////////////////////


                humanParts.LeftArm.gameObject.AddComponent<CapsuleCollider>();
                humanParts.LeftArm.gameObject.AddComponent<Rigidbody>();
                humanParts.LeftArm.gameObject.AddComponent<CharacterJoint>();

                humanParts.LeftArm.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.LeftArm.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.LeftArm.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.LeftArm.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - LeftArm
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_LeftArm = humanParts.LeftArm.GetComponent<Rigidbody>();

                tempRigid_LeftArm.isKinematic = ragdollSettings.isKinematic;
                tempRigid_LeftArm.useGravity = ragdollSettings.useGravity;

                tempRigid_LeftArm.mass = weight.Arm;

                tempRigid_LeftArm.drag = ragdollSettings.rigidDrag;
                tempRigid_LeftArm.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_LeftArm.collisionDetectionMode = ragdollSettings.collisionMode;


    ///
    ///
    ///     BODY PART - LeftElbow
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - LeftElbow
    ///
    //////////////////////////////////////////


                humanParts.LeftElbow.gameObject.AddComponent<CapsuleCollider>();
                humanParts.LeftElbow.gameObject.AddComponent<Rigidbody>();
                humanParts.LeftElbow.gameObject.AddComponent<CharacterJoint>();

                humanParts.LeftElbow.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.LeftElbow.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.LeftElbow.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.LeftElbow.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - LeftElbow
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_LeftElbow = humanParts.LeftElbow.GetComponent<Rigidbody>();

                tempRigid_LeftElbow.isKinematic = ragdollSettings.isKinematic;
                tempRigid_LeftElbow.useGravity = ragdollSettings.useGravity;

                tempRigid_LeftElbow.mass = weight.Elbow;

                tempRigid_LeftElbow.drag = ragdollSettings.rigidDrag;
                tempRigid_LeftElbow.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_LeftElbow.collisionDetectionMode = ragdollSettings.collisionMode;


                if(ragdollSettings.createEnds){

    ///
    ///
    ///     BODY PART - LeftHand
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - LeftHand
    ///
    //////////////////////////////////////////


                    humanParts.LeftHand.gameObject.AddComponent<BoxCollider>();
                    humanParts.LeftHand.gameObject.AddComponent<Rigidbody>();
                    humanParts.LeftHand.gameObject.AddComponent<CharacterJoint>();

                    humanParts.LeftHand.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                    humanParts.LeftHand.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                    humanParts.LeftHand.GetComponent<BoxCollider>().isTrigger = ragdollSettings.isTrigger;
                    humanParts.LeftHand.GetComponent<BoxCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - LeftHand
    ///
    //////////////////////////////////////////


                    Rigidbody tempRigid_LeftHand = humanParts.LeftHand.GetComponent<Rigidbody>();

                    tempRigid_LeftHand.isKinematic = ragdollSettings.isKinematic;
                    tempRigid_LeftHand.useGravity = ragdollSettings.useGravity;

                    tempRigid_LeftHand.mass = weight.Hand;

                    tempRigid_LeftHand.drag = ragdollSettings.rigidDrag;
                    tempRigid_LeftHand.angularDrag = ragdollSettings.rigidAngularDrag;

                    tempRigid_LeftHand.collisionDetectionMode = ragdollSettings.collisionMode;

                }//createEnds

                ConfigureRagdollForLimb(humanParts.LeftArm, humanParts.LeftElbow, humanParts.LeftHand, ragdollSettings.createEnds, true);
                ConfigureHandJoints(humanParts.LeftArm, humanParts.LeftElbow, humanParts.LeftHand, true, ragdollSettings.createEnds);



    ///////////////////////// EDITING HERE /////////////////////////////////////////////



    ///////////////////////// RIGHT LOWER SIDE OF BODY SETUP /////////////////////////////////////////////




    ///
    ///
    ///     BODY PART - RightHips
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - RightHips
    ///
    //////////////////////////////////////////


                humanParts.RightHips.gameObject.AddComponent<CapsuleCollider>();
                humanParts.RightHips.gameObject.AddComponent<Rigidbody>();
                humanParts.RightHips.gameObject.AddComponent<CharacterJoint>();

                humanParts.RightHips.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.RightHips.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.RightHips.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.RightHips.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - RightHips
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_RightHips = humanParts.RightHips.GetComponent<Rigidbody>();

                tempRigid_RightHips.isKinematic = ragdollSettings.isKinematic;
                tempRigid_RightHips.useGravity = ragdollSettings.useGravity;

                tempRigid_RightHips.mass = weight.Hip;

                tempRigid_RightHips.drag = ragdollSettings.rigidDrag;
                tempRigid_RightHips.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_RightHips.collisionDetectionMode = ragdollSettings.collisionMode;


    ///
    ///
    ///     BODY PART - RightKnee
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - RightKnee
    ///
    //////////////////////////////////////////


                humanParts.RightKnee.gameObject.AddComponent<CapsuleCollider>();
                humanParts.RightKnee.gameObject.AddComponent<Rigidbody>();
                humanParts.RightKnee.gameObject.AddComponent<CharacterJoint>();

                humanParts.RightKnee.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.RightKnee.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.RightKnee.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.RightKnee.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - RightKnee
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_RightKnee = humanParts.RightKnee.GetComponent<Rigidbody>();

                tempRigid_RightKnee.isKinematic = ragdollSettings.isKinematic;
                tempRigid_RightKnee.useGravity = ragdollSettings.useGravity;

                tempRigid_RightKnee.mass = weight.Knee;

                tempRigid_RightKnee.drag = ragdollSettings.rigidDrag;
                tempRigid_RightKnee.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_RightKnee.collisionDetectionMode = ragdollSettings.collisionMode;


                if(ragdollSettings.createEnds){

    ///
    ///
    ///     BODY PART - RightFoot
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - RightFoot
    ///
    //////////////////////////////////////////


                    humanParts.RightFoot.gameObject.AddComponent<BoxCollider>();
                    humanParts.RightFoot.gameObject.AddComponent<Rigidbody>();
                    humanParts.RightFoot.gameObject.AddComponent<CharacterJoint>();

                    humanParts.RightFoot.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                    humanParts.RightFoot.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                    humanParts.RightFoot.GetComponent<BoxCollider>().isTrigger = ragdollSettings.isTrigger;
                    humanParts.RightFoot.GetComponent<BoxCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - RightFoot
    ///
    //////////////////////////////////////////


                    Rigidbody tempRigid_RightFoot = humanParts.RightFoot.GetComponent<Rigidbody>();

                    tempRigid_RightFoot.isKinematic = ragdollSettings.isKinematic;
                    tempRigid_RightFoot.useGravity = ragdollSettings.useGravity;

                    tempRigid_RightFoot.mass = weight.Foot;

                    tempRigid_RightFoot.drag = ragdollSettings.rigidDrag;
                    tempRigid_RightFoot.angularDrag = ragdollSettings.rigidAngularDrag;

                    tempRigid_RightFoot.collisionDetectionMode = ragdollSettings.collisionMode;

                }//createEnds


                ConfigureRagdollForLimb(humanParts.RightHips, humanParts.RightKnee, humanParts.RightFoot, ragdollSettings.createEnds, false);
                ConfigureLegsJoints(humanParts.RightHips, humanParts.RightKnee, humanParts.RightFoot, ragdollSettings.createEnds);


    ///////////////////////// RIGHT UPPER SIDE OF BODY SETUP /////////////////////////////////////////////




    ///
    ///
    ///     BODY PART - RightArm
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - RightArm
    ///
    //////////////////////////////////////////


                humanParts.RightArm.gameObject.AddComponent<CapsuleCollider>();
                humanParts.RightArm.gameObject.AddComponent<Rigidbody>();
                humanParts.RightArm.gameObject.AddComponent<CharacterJoint>();

                humanParts.RightArm.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.RightArm.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.RightArm.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.RightArm.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - RightArm
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_RightArm = humanParts.RightArm.GetComponent<Rigidbody>();

                tempRigid_RightArm.isKinematic = ragdollSettings.isKinematic;
                tempRigid_RightArm.useGravity = ragdollSettings.useGravity;

                tempRigid_RightArm.mass = weight.Arm;

                tempRigid_RightArm.drag = ragdollSettings.rigidDrag;
                tempRigid_RightArm.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_RightArm.collisionDetectionMode = ragdollSettings.collisionMode;


    ///
    ///
    ///     BODY PART - RightElbow
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - RightElbow
    ///
    //////////////////////////////////////////


                humanParts.RightElbow.gameObject.AddComponent<CapsuleCollider>();
                humanParts.RightElbow.gameObject.AddComponent<Rigidbody>();
                humanParts.RightElbow.gameObject.AddComponent<CharacterJoint>();

                humanParts.RightElbow.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                humanParts.RightElbow.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                humanParts.RightElbow.GetComponent<CapsuleCollider>().isTrigger = ragdollSettings.isTrigger;
                humanParts.RightElbow.GetComponent<CapsuleCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - RightElbow
    ///
    //////////////////////////////////////////


                Rigidbody tempRigid_RightElbow = humanParts.RightElbow.GetComponent<Rigidbody>();

                tempRigid_RightElbow.isKinematic = ragdollSettings.isKinematic;
                tempRigid_RightElbow.useGravity = ragdollSettings.useGravity;

                tempRigid_RightElbow.mass = weight.Elbow;

                tempRigid_RightElbow.drag = ragdollSettings.rigidDrag;
                tempRigid_RightElbow.angularDrag = ragdollSettings.rigidAngularDrag;

                tempRigid_RightElbow.collisionDetectionMode = ragdollSettings.collisionMode;


                if(ragdollSettings.createEnds){


    ///
    ///
    ///     BODY PART - RightHand
    ///
    ///


    //////////////////////////////////////////
    ///
    ///     BODY PART CREATE - RightHand
    ///
    //////////////////////////////////////////


                    humanParts.RightHand.gameObject.AddComponent<BoxCollider>();
                    humanParts.RightHand.gameObject.AddComponent<Rigidbody>();
                    humanParts.RightHand.gameObject.AddComponent<CharacterJoint>();

                    humanParts.RightHand.gameObject.GetComponent<CharacterJoint>().enablePreprocessing = false;
                    humanParts.RightHand.gameObject.GetComponent<CharacterJoint>().enableProjection = true;

                    humanParts.RightHand.GetComponent<BoxCollider>().isTrigger = ragdollSettings.isTrigger;
                    humanParts.RightHand.GetComponent<BoxCollider>().enabled = ragdollSettings.enabledState;


    //////////////////////////////////////////
    ///
    ///     BODY PART RIGIDBODY UPDATE - RightHand
    ///
    //////////////////////////////////////////


                    Rigidbody tempRigid_RightHand = humanParts.RightHand.GetComponent<Rigidbody>();

                    tempRigid_RightHand.isKinematic = ragdollSettings.isKinematic;
                    tempRigid_RightHand.useGravity = ragdollSettings.useGravity;

                    tempRigid_RightHand.mass = weight.Hand;

                    tempRigid_RightHand.drag = ragdollSettings.rigidDrag;
                    tempRigid_RightHand.angularDrag = ragdollSettings.rigidAngularDrag;

                    tempRigid_RightHand.collisionDetectionMode = ragdollSettings.collisionMode;

                }//createEnds


                ConfigureRagdollForLimb(humanParts.RightArm, humanParts.RightElbow, humanParts.RightHand, ragdollSettings.createEnds, true);
                ConfigureHandJoints(humanParts.RightArm, humanParts.RightElbow, humanParts.RightHand, false, ragdollSettings.createEnds);

                ragdollCreated = true;

                if(useDebug){

                    Debug.Log("Ragdoll Created on " + rootTrans.gameObject.name);

                }//useDebug

            //pelvis Rigidbody != null
            } else {

                if(useDebug){

                    Debug.Log("Ragdoll Already Created");

                }//useDebug

            }//pelvis Rigidbody != null

        }//CreateRagdoll

        void ClearRagdoll(){

            foreach(var component in humanParts.Pelvis.GetComponentsInChildren<Collider>()){

                GameObject.DestroyImmediate(component);

            }//foreach component

            foreach(var component in humanParts.Pelvis.GetComponentsInChildren<CharacterJoint>()){

                GameObject.DestroyImmediate(component);

            }//foreach component

            foreach(var component in humanParts.Pelvis.GetComponentsInChildren<Rigidbody>()){

                GameObject.DestroyImmediate(component);

            }//foreach component

            ragdollCreated = false;

            if(useDebug){

                Debug.Log("Ragdoll Cleared on " + rootTrans.gameObject.name);

            }//useDebug

        }//ClearRagdoll


    //////////////////////////////////////////////////////////////////////////////
    ///
    ///     RAGDOLL CREATION TOOLS
    ///
    //////////////////////////////////////////////////////////////////////////////


        static void ConfigureJointParams(CharacterJoint joint, Transform part, Rigidbody anchor, Vector3 axis, Vector3 swingAxis) {

            joint.connectedBody = anchor;
            joint.axis = part.InverseTransformDirection(axis);
            joint.swingAxis = part.InverseTransformDirection(swingAxis);

        }//ConfigureJointParams

        static void ConfigureJointLimits(CharacterJoint joint, float lowTwist, float highTwist, float swing1, float swing2) {

            //if (lowTwist > highTwist)

                //throw new ArgumentException("wrong limitation: lowTwist > highTwist");

            var twistLimitSpring = joint.twistLimitSpring;
            joint.twistLimitSpring = twistLimitSpring;

            var swingLimitSpring = joint.swingLimitSpring;
            joint.swingLimitSpring = swingLimitSpring;

            var lowTwistLimit = joint.lowTwistLimit;
            lowTwistLimit.limit = lowTwist;
            joint.lowTwistLimit = lowTwistLimit;

            var highTwistLimit = joint.highTwistLimit;
            highTwistLimit.limit = highTwist;
            joint.highTwistLimit = highTwistLimit;

            var swing1Limit = joint.swing1Limit;
            swing1Limit.limit = swing1;
            joint.swing1Limit = swing1Limit;

            var swing2Limit = joint.swing2Limit;
            swing2Limit.limit = swing2;
            joint.swing2Limit = swing2Limit;

        }//ConfigureJointLimits

        static void ConfigureRagdollForLimb(Transform limbUpper, Transform limbLower, Transform tip, bool createEnds, bool arms) {

            float totalLength = limbUpper.InverseTransformPoint(tip.position).magnitude;

            CapsuleCollider upperCapsule = limbUpper.gameObject.GetComponent<CapsuleCollider>();

            var boneEndPos = limbUpper.InverseTransformPoint(limbLower.position);

            upperCapsule.direction = GetXyzDirection(limbLower.localPosition);

            upperCapsule.radius = totalLength * 0.12f;
            upperCapsule.height = boneEndPos.magnitude;
            upperCapsule.center = Vector3.Scale(boneEndPos, Vector3.one * 0.5f);

            CapsuleCollider endCapsule = limbLower.GetComponent<CapsuleCollider>();

            if(arms){

                boneEndPos = limbLower.InverseTransformPoint(tip.position);

            }//arms

            endCapsule.direction = GetXyzDirection(boneEndPos);
            endCapsule.radius = totalLength * 0.12f;
            endCapsule.height = boneEndPos.magnitude;
            endCapsule.center = Vector3.Scale(boneEndPos, Vector3.one * 0.5f);

            if(createEnds) {

                boneEndPos = GetLongestTransform(tip).position;
                boneEndPos = tip.InverseTransformPoint(boneEndPos);

                Vector3 tipDir = GetXyzDirectionV(boneEndPos);
                Vector3 tipSides = (tipDir - Vector3.one) * -1;
                Vector3 boxSize = tipDir * boneEndPos.magnitude * 1.3f + tipSides * totalLength * 0.2f;

                BoxCollider tipBox = tip.gameObject.GetComponent<BoxCollider>();
                tipBox.size = boxSize;

                float halfTipLength = boneEndPos.magnitude / 2f;
                tipBox.center = Vector3.Scale(boneEndPos.normalized, Vector3.one * halfTipLength);

            }//createEnds

        }//ConfigureRagdollForLimb

        void ConfigureHandJoints(Transform arm, Transform elbow, Transform hand, bool leftHand, bool createEnds) {

            var dirUpper = elbow.position - arm.position;
            var dirLower = hand.position - elbow.position;
            var dirHand = GetLongestTransform(hand).position - hand.position;

            var armJoint = arm.GetComponent<CharacterJoint>();
            var elbowJoint = elbow.GetComponent<CharacterJoint>();
            var handJoint = hand.GetComponent<CharacterJoint>();

            var armRigid = arm.GetComponent<Rigidbody>();
            var elbowRigid = elbow.GetComponent<Rigidbody>();
            var chestRigid = humanParts.Chest.GetComponent<Rigidbody>();

            if(leftHand) {

                ConfigureJointLimits(armJoint, -100f, 30f, 100f, 45f);
                ConfigureJointLimits(elbowJoint, -120f, 0f, 10f, 90f);

                if(createEnds) {

                    ConfigureJointLimits(handJoint, -90f, 90f, 90f, 45f);

                }//createEnds

                dirUpper = -dirUpper;
                dirLower = -dirLower;
                dirHand = -dirHand;

            //leftHand	
            } else {

                ConfigureJointLimits(armJoint, -30f, 100f, 100f, 45f);
                ConfigureJointLimits(elbowJoint, 0f, 120f, 10f, 90f);

                if(createEnds) {

                    ConfigureJointLimits(handJoint, -90f, 90f, 90f, 45f);

                }//createEnds

            }//lefthand

            var upU = Vector3.Cross(playDir, dirUpper);
            var upL = Vector3.Cross(playDir, dirLower);
            var upH = Vector3.Cross(playDir, dirHand);

            ConfigureJointParams(armJoint, arm, chestRigid, upU, playDir);
            ConfigureJointParams(elbowJoint, elbow, armRigid, upL, playDir);

            if(createEnds) {

                ConfigureJointParams(handJoint, hand, elbowRigid, upH, playDir);

            }//createEnds

        }//ConfigureHandJoints

        void ConfigureLegsJoints(Transform hip, Transform knee, Transform foot, bool createEnds) {
            
            var hipJoint = hip.GetComponent<CharacterJoint>();
            var kneeJoint = knee.GetComponent<CharacterJoint>();
            var footJoint = foot.GetComponent<CharacterJoint>();

            var pelvisRigid = humanParts.Pelvis.GetComponent<Rigidbody>();
            var hipRigid = hip.GetComponent<Rigidbody>();

            var kneeRigid = knee.GetComponent<Rigidbody>();

            ConfigureJointParams(hipJoint, hip, pelvisRigid, rootTrans.right, rootTrans.forward);
            ConfigureJointParams(kneeJoint, knee, hipRigid, rootTrans.right, rootTrans.forward);

            ConfigureJointLimits(hipJoint, -10f, 120f, 90f, 20f);
            ConfigureJointLimits(kneeJoint, -120f, 0f, 10f, 20f);

            if (createEnds) {

                ConfigureJointParams(footJoint, foot, kneeRigid, rootTrans.right, rootTrans.forward);
                ConfigureJointLimits(footJoint, -70f, 70f, 45f, 20f);

            }//createEnds

        }//ConfigureLegsJoints

        private Vector3 Abs(Vector3 v) {

            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

        }//Abs

        public Vector3 GetPlayerDirection(){

            Vector3 leftKnee = humanParts.LeftKnee.position - humanParts.Pelvis.position;
            Vector3 rightKnee = humanParts.RightKnee.position - humanParts.Pelvis.position;

            return Vector3.Cross(leftKnee, rightKnee).normalized;

        }//GetPlayerDirection

        static int GetXyzDirection(Vector3 node) {

            float x = Mathf.Abs(node.x);
            float y = Mathf.Abs(node.y);
            float z = Mathf.Abs(node.z);

            if (x > y & x > z)		// x is the bigest
                    return 0;

            if (y > x & y > z)		// y is the bigest
                    return 1;

            // z is the bigest
            return 2;

        }//GetXyzDirection

        static Vector3 GetXyzDirectionV(Vector3 node) {

            var d = GetXyzDirection(node);

            switch(d) {

                case 0: return Vector3.right;
                case 1: return Vector3.up;
                case 2: return Vector3.forward;

            }//switch

            throw new InvalidOperationException();

        }//GetXyzDirectionV

        private static Transform GetLongestTransform(Transform limb) {

            float longestF = -1;
            Transform longestT = null;

            // find the farest object that attached to 'limb'
            foreach (Transform t in limb.GetComponentsInChildren<Transform>()) {

                float length = (limb.position - t.position).sqrMagnitude;

                if(length > longestF) {

                    longestF = length;
                    longestT = t;

                }//length > longestF

            }//foreach limb

            return longestT;

        }//GetLongestTransform


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

                    if(dmMenusLocData.dictionary[d].asset == "Ragdoll Creator"){

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

                                window = GetWindow<DM_RagdollCreator>(false, "Ragdoll Creator" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Ragdoll Creator Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<DM_RagdollCreator>(false, "Ragdoll Creator" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Puzzler Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Ragdoll Creator Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<DM_RagdollCreator>(false, "Ragdoll Creator" + " v" + verNumb, true);
                    window.maxSize = window.minSize = windowsSize;

                }//results.Length > 0

            }//!versionCheckStatic

        }//Version_FindStatic

        private void OnDestroy() {

            window = null;
            verNumb = "";

        }//OnDestroy


    }//DM_RagdollCreator
    
    
}//namespace

#endif
