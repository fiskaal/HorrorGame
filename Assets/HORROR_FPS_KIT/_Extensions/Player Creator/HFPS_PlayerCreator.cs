#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using System.IO;

#if COMPONENTS_PRESENT

    using DizzyMedia.HFPS_Components;
    using DizzyMedia.Shared;

#endif

using DizzyMedia.Version;

using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.Extension {

    public class HFPS_PlayerCreator : EditorWindow {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    //////////////////////////////////////


        [System.Serializable]
        public class Create_Options {

            public bool customPosition;
            public Transform createPos;

            public bool destroyChar;

            public bool createOpts;
            public bool createTypeOptions;
            public bool createPosOptions;
            public bool createDestOpts;
            public bool gameOpts;

        }//Create_Options

        [System.Serializable]
        public class Game_Options {

            public bool updGameMan = true;

            public bool gameManOpts;

        }//Game_Options

        [System.Serializable]
        public class PlayerCreate_Options {

            [Layer] public int limbVisibleLayer = 11;
            [Layer] public int invisibleLayer = 12;

            public List<GameObject> invisibleParts = new List<GameObject>();

            public List<GameObject> activateLimbs = new List<GameObject>();
            public List<GameObject> deactivateLimbs = new List<GameObject>();

            public List<BodyAnimator.SpineBone> spineBones = new List<BodyAnimator.SpineBone>();

            public bool playRefs;
            public bool bodyAnimOpts;

        }//PlayerCreate_Options

        [System.Serializable]
        public class Ragdoll_Options {

            public bool addDeathParent = true;
            public bool fixHeadCollider = false;
            public bool autoHeadFix = false;

            public Vector3 headCenter = new Vector3(-8.11303e-07f, 11.35702f, 11.58915f);
            public float headRadius = 11.91596f;
            public float headHeight = 37.54185f;
            public Direction headDirection = Direction.ZAxis;

            public bool ragdollOpts;
            public bool ragdollHeadOpts;

            public BodyAnimator tempBodyAnim;
            public Animator charAnim;
            public Transform pelvis;
            public Collider tempCollider;
            public bool animChecked;

        }//Ragdoll_Options

        [System.Serializable]
        public class Update_Options {

            public bool featOpts;
            public bool playOpts;
            public bool proneOpts;
            public bool zoomOpts;

            public bool audioFadeOpts;
            public bool dualWieldOpts;
            public bool fovOpts;
            public bool matOpts;
            public bool playManOpts;
            public bool screenEventsOpts;
            public bool subActsOpts;

        }//Update_Options

        [System.Serializable]
        public class AddOrUpdate_Options {

            public bool addAudioFader;
            public bool addDualWield;
            public bool addFOVMan;
            public bool addMaterialCont;
            public bool addPlayerMan;
            public bool addReferences;
            public bool addScreenEvents;
            public bool addSubActions;
            public bool updateProne;
            public bool updateZoom;

        }//AddOrUpdate_Options

        [System.Serializable]
        public class AudioFader_Options {

            public float fadeMulti;

            public AudioFade_Sources sources = new AudioFade_Sources();

        }//AudioFader_Options

        [System.Serializable]
        public class AudioFade_Sources {

            [Space]

            public AudioSource ambience;
            public AudioSource music;

            [Header("Auto")]

            public string ambName;
            public string musicName;

        }//AudioFade_Source

        [System.Serializable]
        public class DualWield_Item {

            public string name;
            public int flags;
            public bool canDualWield;

            public List<string> options = new List<string>();
            public List<string> selectedOptions = new List<string>();

        }//DualWield_Item

        [System.Serializable]
        public class FOV_Options {

            public float globalZoomMulti;
            public float globalUnZoomMulti;

            public bool autoFindCameras;
            public List<Camera> cameras = new List<Camera>();

            #if COMPONENTS_PRESENT

                public List<HFPS_FOVManager.Camera_State> states = new List<HFPS_FOVManager.Camera_State>();

            #endif

        }//FOV_Options

        [System.Serializable]
        public class FOV_Auto {

            public bool camOpts;
            public bool genOpts;
            public bool statesOpts;

        }//FOV_Auto

        [System.Serializable]
        public class MaterialCont_Options {

            #if COMPONENTS_PRESENT

                public List<HFPS_MaterialCont.BodyParts> bodyParts = new List<HFPS_MaterialCont.BodyParts>();

            #endif

        }//MaterialCont_Options

        [System.Serializable]
        public class PlayerMan_Options {

            #if COMPONENTS_PRESENT

                public HFPS_PlayerMan.SlowDown slowDownUse;

                public List<HFPS_PlayerMan.Melee_Cont> meleeConts = new List<HFPS_PlayerMan.Melee_Cont>();
                public List<HFPS_PlayerMan.Weapon_Cont> weaponConts = new List<HFPS_PlayerMan.Weapon_Cont>();

            #endif

        }//PlayerMan_Options

        [System.Serializable]
        public class PlayerMan_Auto {

            public bool genOpts;
            public bool meleeOpts;
            public bool weapOpts;

        }//PlayerMan_Auto

        [System.Serializable]
        public class Prone_Options {

            public bool enableProne;
            public float proneSpeed;
            public Vector3 proneCenter;

        }//Prone_Options

        [System.Serializable]
        public class Prone_Auto {

            public bool basicSettings;
            public bool contFeatures;
            public bool contAdjust;

        }//Prone_Auto

        [System.Serializable]
        public class SubActions_Options {

            #if COMPONENTS_PRESENT

                public List<HFPS_SubActionsHandler.Sub_Action> subActions = new List<HFPS_SubActionsHandler.Sub_Action>();
                public List<HFPS_SubActionsHandler.Action_Input> actionInputs = new List<HFPS_SubActionsHandler.Action_Input>();

                public bool useActionDelay;
                public float actionDelay;

                public HFPS_SubActionsHandler.Input_Type inputType;
                public float holdTime;
                public float holdMulti;

            #endif

        }//SubActions_Options

        [System.Serializable]
        public class SubActions_Auto {

            public bool actOpts;
            public bool inputOpts;

        }//SubActions_Auto

        [System.Serializable]
        public class ScrollPositions {

            public Vector2 playOpts_scrollPos;
            public Vector2 featOpts_scrollPos;
            public Vector2 notifNothing_scrollPos;
            public Vector2 dualWield_scrollPos;
            public Vector2 subActs_scrollPos;
            public Vector2 fov_scrollPos;
            public Vector2 matCont_scrollPos;
            public Vector2 playMan_scrollPos;
            public Vector2 prone_scrollPos;
            public Vector2 audFade_scrollPos;
            public Vector2 createGen_scrollPos;
            public Vector2 createCompGen_scrollPos;
            public Vector2 gameOpts_scrollPos;
            public Vector2 createPlayOpts_scrollPos;
            public Vector2 ragdollOpts_scrollPos;
            public Vector2 createOpts_scrollPos;
            public Vector2 createCustom_scrollPos;

        }//ScrollPositions


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Use_Type {

            Create = 0,
            Update = 1,

        }//Use_Type

        public enum Character_Type {

            Generic = 0,
            ComponentsGeneric = 1,
            Custom = 2,

        }//Character_Type

        public enum Create_Type {

            AOrB = 0,
            C = 1,

        }//Create_Type

        public enum Direction {

            XAxis = 0,
            YAxis = 1,
            ZAxis = 2,

        }//Direction


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static HFPS_PlayerCreator window;
        private static Vector2 windowsSize = new Vector2(400, 600);

        public Use_Type useType;
        public Character_Type characterType;
        public Create_Type createType;

        public GameObject character;
        public GameObject playerLayout;
        public AnimatorController playerAnimator;

        #if COMPONENTS_PRESENT

            public DM_PlayerCreator_Template template;

        #endif

        private static DM_Version dmVersion;
        private static string versionName = "PlayerCreator Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        public Create_Options createOptions;
        public Game_Options gameOptions;
        public PlayerCreate_Options playerCreateOptions;
        public Ragdoll_Options ragdollOptions;

        public Update_Options updateOptions;
        public AddOrUpdate_Options addOrUpdateOptions;
        public AudioFader_Options audioFaderOptions;
        public List<DualWield_Item> dualWieldItems = new List<DualWield_Item>();
        public FOV_Options fovOptions;
        public FOV_Auto fovAuto;
        public MaterialCont_Options matContOptions;
        public PlayerMan_Options playerManOptions;
        public PlayerMan_Auto playerManAuto;
        public Prone_Options proneOptions;
        public Prone_Auto proneAuto;
        public SubActions_Options subActionsOptions;
        public SubActions_Auto subActionsAuto;

        #if COMPONENTS_PRESENT

            public PlayerFunctions.Zoom_Type zoomType;

        #endif

        public ScrollPositions scrollPositions;

        #if COMPONENTS_PRESENT

            private bool playerIsHFPS;
            private bool playerChecked;

        #endif

        public List<GameObject> playerItems = new List<GameObject>();

        private bool initDefaults;
        private int tabs;


    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/HFPS/HFPS Player Creator", false , 13)]
        public static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<HFPS_PlayerCreator>(false, "Player Creator" + " v" + verNumb, true);
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

        private void OnGUI() {

            PlayerCreator_Screen();

            if(!initDefaults){

                initDefaults = true;
                Player_CreateDefaults(false);

            }//!initDefaults

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        public void PlayerCreator_Screen(){

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t0 = (Texture)Resources.Load("EditorContent/PlayerCreator/PlayerCreator_Header");

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));

            EditorGUI.BeginChangeCheck();

            ScriptableObject target = this;
            SerializedObject soTar = new SerializedObject(target);

            SerializedProperty useTypeRef = soTar.FindProperty("useType");
            SerializedProperty characterTypeRef = soTar.FindProperty("characterType");
            SerializedProperty createTypeRef = soTar.FindProperty("createType");

            SerializedProperty characterRef = soTar.FindProperty("character");
            SerializedProperty templateRef = soTar.FindProperty("template");

            SerializedProperty createPos = soTar.FindProperty("createOptions.createPos");

            SerializedProperty playerLayoutRef = soTar.FindProperty("playerLayout");
            SerializedProperty playerAnimatorRef = soTar.FindProperty("playerAnimator");

            SerializedProperty limbVisibleLayer = soTar.FindProperty("playerCreateOptions.limbVisibleLayer");
            SerializedProperty invisibleLayer = soTar.FindProperty("playerCreateOptions.invisibleLayer");
            SerializedProperty invisibleParts = soTar.FindProperty("playerCreateOptions.invisibleParts");
            SerializedProperty activateLimbs = soTar.FindProperty("playerCreateOptions.activateLimbs");
            SerializedProperty deactivateLimbs = soTar.FindProperty("playerCreateOptions.deactivateLimbs");
            SerializedProperty spineBones = soTar.FindProperty("playerCreateOptions.spineBones");

            SerializedProperty headCenter = soTar.FindProperty("ragdollOptions.headCenter");
            SerializedProperty headDirection = soTar.FindProperty("ragdollOptions.headDirection");

            SerializedProperty sources = soTar.FindProperty("audioFaderOptions.sources");

            SerializedProperty cameras = soTar.FindProperty("fovOptions.cameras");
            SerializedProperty camStates = soTar.FindProperty("fovOptions.states");

            SerializedProperty bodyParts = soTar.FindProperty("matContOptions.bodyParts");

            SerializedProperty slowDownUse = soTar.FindProperty("playerManOptions.slowDownUse");
            SerializedProperty meleeConts = soTar.FindProperty("playerManOptions.meleeConts");
            SerializedProperty weaponConts = soTar.FindProperty("playerManOptions.weaponConts");

            SerializedProperty proneCenter = soTar.FindProperty("proneOptions.proneCenter");

            SerializedProperty zoomType = soTar.FindProperty("zoomType");

            SerializedProperty subActionsRef = soTar.FindProperty("subActionsOptions.subActions");
            SerializedProperty actionInputs = soTar.FindProperty("subActionsOptions.actionInputs");
            SerializedProperty inputType = soTar.FindProperty("subActionsOptions.inputType");

            //SerializedProperty dualWieldItemsRef = soTar.FindProperty("dualWieldItems");

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

                    EditorGUILayout.PropertyField(useTypeRef, true);


    //////////////////////////////////////
    ///
    ///     PLAYER CREATE
    ///
    //////////////////////////////////////


                    if(useType == Use_Type.Create){

                        EditorGUILayout.PropertyField(characterTypeRef, true);

                        if(characterType == Character_Type.Generic){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(!createOptions.createOpts && !createOptions.gameOpts){

                                scrollPositions.createGen_scrollPos = GUILayout.BeginScrollView(scrollPositions.createGen_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!gameOpts

                            if(!createOptions.gameOpts){

                                createOptions.createOpts = GUILayout.Toggle(createOptions.createOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local, GUI.skin.button);

                            }//!gameOpts

                            if(!createOptions.createOpts){

                                GUILayout.Space(5);

                                createOptions.gameOpts = GUILayout.Toggle(createOptions.gameOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local, GUI.skin.button);

                            }//createOpts

                        }//characterType = generic

                        if(characterType == Character_Type.ComponentsGeneric){

                            #if COMPONENTS_PRESENT

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                if(!createOptions.createOpts && !createOptions.gameOpts){

                                    scrollPositions.createCompGen_scrollPos = GUILayout.BeginScrollView(scrollPositions.createCompGen_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                }//!gameOpts

                                if(!createOptions.gameOpts){

                                    createOptions.createOpts = GUILayout.Toggle(createOptions.createOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local, GUI.skin.button);

                                }//!gameOpts

                                if(!createOptions.createOpts){

                                    GUILayout.Space(5);

                                    createOptions.gameOpts = GUILayout.Toggle(createOptions.gameOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local, GUI.skin.button);

                                }//createOpts

                            #else

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local, MessageType.Error);

                            #endif

                        }//characterType = components generic

                        if(characterType == Character_Type.Custom){

                            EditorGUILayout.PropertyField(characterRef, true);

                            EditorGUILayout.Space();

                            if(character == null){

                                if(ragdollOptions.animChecked){

                                    Ragdoll_Reset();

                                }//ragdollOptions.animChecked

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, MessageType.Warning);

                            //character = null
                            } else {

                                if(!ragdollOptions.animChecked){

                                    if(ragdollOptions.charAnim == null){

                                        if(character.GetComponent<Animator>() != null){

                                            ragdollOptions.charAnim = character.GetComponent<Animator>();
                                            ragdollOptions.pelvis = ragdollOptions.charAnim.GetBoneTransform(HumanBodyBones.Hips);
                                            ragdollOptions.tempCollider = ragdollOptions.pelvis.GetComponent<Collider>();

                                            ragdollOptions.animChecked = true;

                                        //animator != null
                                        } else {

                                            ragdollOptions.animChecked = true;

                                        }//animator != null

                                    }//ragdollOptions.charAnim == null

                                //!ragdollOptions.animChecked
                                } else {

                                    if(ragdollOptions.charAnim != null){

                                        EditorGUILayout.Space();

                                        if(ragdollOptions.tempCollider != null){

                                            EditorGUILayout.HelpBox(character.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local, MessageType.Info);

                                        //ragdollOptions.tempCollider != null
                                        } else {

                                            EditorGUILayout.HelpBox(character.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local, MessageType.Warning);

                                        }//ragdollOptions.tempCollider != null

                                    //ragdollOptions.charAnim != null
                                    } else {

                                        EditorGUILayout.HelpBox(character.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local, MessageType.Error);

                                    }//ragdollOptions.charAnim != null

                                }//!ragdollOptions.animChecked

                            }//character = null

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(!createOptions.createOpts && !createOptions.gameOpts && !updateOptions.playOpts && !ragdollOptions.ragdollOpts){

                                scrollPositions.createCustom_scrollPos = GUILayout.BeginScrollView(scrollPositions.createCustom_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!gameOpts & !playOpts


    //////////////////////////////////////
    ///
    ///     PLAYER CREATE - MAIN BUTTONS
    ///
    //////////////////////////////////////


                            if(character != null){

                                if(!createOptions.gameOpts && !updateOptions.playOpts && !ragdollOptions.ragdollOpts){

                                    createOptions.createOpts = GUILayout.Toggle(createOptions.createOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local, GUI.skin.button);

                                }//!gameOpts

                                if(!createOptions.createOpts && !updateOptions.playOpts && !ragdollOptions.ragdollOpts){

                                    GUILayout.Space(5);

                                    createOptions.gameOpts = GUILayout.Toggle(createOptions.gameOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local, GUI.skin.button);

                                }//!playOpts

                                if(!createOptions.createOpts && !createOptions.gameOpts && !ragdollOptions.ragdollOpts){

                                    GUILayout.Space(5);

                                    updateOptions.playOpts = GUILayout.Toggle(updateOptions.playOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local, GUI.skin.button);

                                }//!gameOpts

                                if(ragdollOptions.animChecked){

                                    if(!createOptions.createOpts && !createOptions.gameOpts && !updateOptions.playOpts){

                                        GUILayout.Space(5);

                                        ragdollOptions.ragdollOpts = GUILayout.Toggle(ragdollOptions.ragdollOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[4].local, GUI.skin.button);

                                    }//!gameOpts & !playOpts

                                }//ragdollOptions.animChecked

                            }//character != null

                        }//characterType = custom


    //////////////////////////////////////
    ///
    ///     PLAYER CREATE - LOWER BUTTONS
    ///
    //////////////////////////////////////


                        if(!createOptions.createOpts && !createOptions.gameOpts && !updateOptions.playOpts && !ragdollOptions.ragdollOpts){

                            if(characterType == Character_Type.ComponentsGeneric){

                                #if COMPONENTS_PRESENT

                                    EditorGUILayout.EndScrollView();

                                #endif

                            //characterType = components generic
                            } else {

                                 EditorGUILayout.EndScrollView();

                            }//characterType = components generic

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(characterType == Character_Type.ComponentsGeneric){

                                #if COMPONENTS_PRESENT

                                    GUI.enabled = true;

                                #else

                                    GUI.enabled = false;

                                #endif

                            }//characterType = components generic

                            if(characterType == Character_Type.Custom){

                                if(character != null && playerLayout != null && playerAnimator != null){

                                    GUI.enabled = true;

                                //character, playerLayout & playerAnimator != null
                                } else {

                                    GUI.enabled = false;

                                }//character, playerLayout & playerAnimator != null

                            }//characterType = custom

                            if(characterType != Character_Type.ComponentsGeneric){

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[5].local)){

                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[1].local)){

                                        Player_Create();

                                    }//DisplayDialog                            

                                }//button

                                GUILayout.Space(5);

                                GUI.enabled = true;

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[6].local)){

                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[1].local)){

                                        Player_CreateDefaults(true);

                                    }//DisplayDialog 

                                }//button

                                EditorGUILayout.Space();

                            //characterType != components generic
                            } else {

                                #if COMPONENTS_PRESENT

                                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[5].local)){

                                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[1].local)){

                                            Player_Create();

                                        }//DisplayDialog                            

                                    }//button

                                    GUILayout.Space(5);

                                    GUI.enabled = true;

                                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[6].local)){

                                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[1].local)){

                                            Player_CreateDefaults(true);

                                        }//DisplayDialog 

                                    }//button

                                    EditorGUILayout.Space();

                                #endif

                            }//characterType != components generic

                        }//!gameOpts & !playOpts & !ragdollOpts


    //////////////////////////////////////
    ///
    ///     CREATE OPTIONS
    ///
    //////////////////////////////////////


                        if(createOptions.createOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[1].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.createOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.createOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            if(characterType != Character_Type.Custom){

                                if(!createOptions.createPosOptions && !createOptions.createDestOpts){

                                    EditorGUILayout.Space();

                                    createOptions.createTypeOptions = GUILayout.Toggle(createOptions.createTypeOptions, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[7].local, GUI.skin.button);

                                    if(createOptions.createTypeOptions){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(createTypeRef, new GUIContent("HFPS Version 1.6.3"), true);

                                    }//createTypeOptions

                                }//!createPosOptions & !createDestOpts

                            }//characterType != custom

                            if(!createOptions.createTypeOptions && !createOptions.createDestOpts){

                                EditorGUILayout.Space();

                                createOptions.createPosOptions = GUILayout.Toggle(createOptions.createPosOptions, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[8].local, GUI.skin.button);

                                if(createOptions.createPosOptions){

                                    EditorGUILayout.Space();

                                    createOptions.customPosition = EditorGUILayout.Toggle("Custom Create Position?", createOptions.customPosition);

                                    if(createOptions.customPosition){

                                        EditorGUILayout.PropertyField(createPos, new GUIContent("Create Position"), true);

                                    }//customPosition

                                }//createPosOptions

                            }//!createTypeOptions & !createDestOpts

                            if(characterType == Character_Type.Custom){

                                if(!createOptions.createPosOptions && !createOptions.createTypeOptions){

                                    EditorGUILayout.Space();

                                    createOptions.createDestOpts = GUILayout.Toggle(createOptions.createDestOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[9].local, GUI.skin.button);

                                    if(createOptions.createDestOpts){

                                        EditorGUILayout.Space();

                                        createOptions.destroyChar = EditorGUILayout.Toggle("Destroy Character?", createOptions.destroyChar);

                                    }//createDestOpts

                                }//!createPosOptions & !createTypeOptions

                            }//characterType = custom

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//createOpts


    //////////////////////////////////////
    ///
    ///     GAME OPTIONS
    ///
    //////////////////////////////////////


                        if(createOptions.gameOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[2].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.gameOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gameOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            gameOptions.gameManOpts = GUILayout.Toggle(gameOptions.gameManOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[10].local, GUI.skin.button);

                            if(gameOptions.gameManOpts){

                                EditorGUILayout.Space();

                                gameOptions.updGameMan = EditorGUILayout.Toggle("Update Game Manager?", gameOptions.updGameMan);

                            }//gameManOpts

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//gameOpts


    //////////////////////////////////////
    ///
    ///     PLAYER OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.playOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[3].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.createPlayOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.createPlayOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            if(!playerCreateOptions.bodyAnimOpts){

                                playerCreateOptions.playRefs = GUILayout.Toggle(playerCreateOptions.playRefs, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[11].local, GUI.skin.button);

                            }//!bodyAnimOpts

                            if(playerCreateOptions.playRefs){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(playerLayoutRef, true);
                                EditorGUILayout.PropertyField(playerAnimatorRef, true);

                            }//playRefs

                            if(!playerCreateOptions.playRefs){

                                GUILayout.Space(5);

                                playerCreateOptions.bodyAnimOpts = GUILayout.Toggle(playerCreateOptions.bodyAnimOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[12].local, GUI.skin.button);

                            }//!playRefs

                            if(playerCreateOptions.bodyAnimOpts){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(limbVisibleLayer, true);
                                EditorGUILayout.PropertyField(invisibleLayer, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(invisibleParts, true);
                                EditorGUILayout.PropertyField(activateLimbs, true);
                                EditorGUILayout.PropertyField(deactivateLimbs, true);
                                EditorGUILayout.PropertyField(spineBones, true);

                            }//bodyAnimOpts

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//playOpts


    //////////////////////////////////////
    ///
    ///     RAGDOLL OPTIONS
    ///
    //////////////////////////////////////


                        if(ragdollOptions.ragdollOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[4].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.ragdollOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.ragdollOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            ragdollOptions.ragdollHeadOpts = GUILayout.Toggle(ragdollOptions.ragdollHeadOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[13].local, GUI.skin.button);

                            if(ragdollOptions.ragdollHeadOpts){

                                EditorGUILayout.Space();

                                ragdollOptions.addDeathParent = EditorGUILayout.Toggle("Add Death Parent?", ragdollOptions.addDeathParent);
                                ragdollOptions.fixHeadCollider = EditorGUILayout.Toggle("Fix Head Collider?", ragdollOptions.fixHeadCollider);

                                if(ragdollOptions.fixHeadCollider){

                                    ragdollOptions.autoHeadFix = EditorGUILayout.Toggle("Auto Head Fix?", ragdollOptions.autoHeadFix);

                                    if(!ragdollOptions.autoHeadFix){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(headCenter, true);

                                        ragdollOptions.headRadius = EditorGUILayout.FloatField("Head Radius", ragdollOptions.headRadius);
                                        ragdollOptions.headHeight = EditorGUILayout.FloatField("Head Height", ragdollOptions.headHeight);

                                        EditorGUILayout.PropertyField(headDirection, true);

                                    }//!autoHeadFix

                                }//fixHeadCollider

                            }//ragdollHeadOpts

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(!ragdollOptions.ragdollHeadOpts){

                                EditorGUILayout.BeginHorizontal();

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[14].local)){

                                    Launch_RagdollCreator();

                                }//button

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[15].local)){

                                    Ragdoll_Reset();

                                }//button

                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.Space();

                            }//!ragdollHeadOpts

                            EditorGUILayout.EndVertical();

                        }//ragdollOpts

                    }//useType = create


    //////////////////////////////////////
    ///
    ///     PLAYER UPDATE
    ///
    //////////////////////////////////////


                    if(useType == Use_Type.Update){

                        #if COMPONENTS_PRESENT

                        EditorGUILayout.PropertyField(characterRef, true);
                        EditorGUILayout.PropertyField(templateRef, true);

                        if(character == null){

                            playerIsHFPS = false;
                            playerChecked = false;

                            if(dualWieldItems.Count > 0){

                                dualWieldItems = new List<DualWield_Item>();

                            }//dualWieldItems.Count > 0

                            if(playerItems.Count > 0){

                                playerItems = new List<GameObject>();

                            }//playerItems.Count > 0

                        //character != null
                        } else {

                            if(!playerChecked){

                                if(Player_Check()){

                                    playerIsHFPS = true;

                                    if(playerItems.Count == 0){

                                        PlayerItems_Catch();

                                    }//playerItems.Count = 0

                                //player check
                                } else {

                                    playerIsHFPS = false;

                                }//player check

                            }//!playerChecked

                        }//character = null


    //////////////////////////////////////
    ///
    ///     PLAYER UPDATE - MAIN BUTTONS
    ///
    //////////////////////////////////////


                        EditorGUILayout.Space();

                        if(!updateOptions.featOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            updateOptions.playOpts = GUILayout.Toggle(updateOptions.playOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[0].local, GUI.skin.button);

                        }//

                        if(!updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            GUILayout.Space(5);

                            updateOptions.featOpts = GUILayout.Toggle(updateOptions.featOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[1].local, GUI.skin.button);

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        }//

                        if(!updateOptions.playOpts && !updateOptions.featOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            scrollPositions.notifNothing_scrollPos = GUILayout.BeginScrollView(scrollPositions.notifNothing_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            if(!addOrUpdateOptions.addAudioFader && !addOrUpdateOptions.addDualWield && !addOrUpdateOptions.addFOVMan && !addOrUpdateOptions.addMaterialCont && !addOrUpdateOptions.addPlayerMan && !addOrUpdateOptions.updateProne && !addOrUpdateOptions.addScreenEvents && !addOrUpdateOptions.addSubActions && !addOrUpdateOptions.addReferences && !addOrUpdateOptions.updateZoom){

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[0].local, MessageType.Error);

                            }

                        }

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.addAudioFader){

                                GUILayout.Space(5);

                                updateOptions.audioFadeOpts = GUILayout.Toggle(updateOptions.audioFadeOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[2].local, GUI.skin.button);

                            }//addOrUpdateOptions.addAudioFader

                        }//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.addDualWield){

                                GUILayout.Space(5);

                                updateOptions.dualWieldOpts = GUILayout.Toggle(updateOptions.dualWieldOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[3].local, GUI.skin.button);

                            }//addOrUpdateOptions.addDualWield

                        }//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.addFOVMan){

                                GUILayout.Space(5);

                                updateOptions.fovOpts = GUILayout.Toggle(updateOptions.fovOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[4].local, GUI.skin.button);

                            }//addOrUpdateOptions.addFOVMan

                        }//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.addMaterialCont){

                                GUILayout.Space(5);

                                updateOptions.matOpts = GUILayout.Toggle(updateOptions.matOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[5].local, GUI.skin.button);

                            }//addOrUpdateOptions.addMaterialCont

                        }//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.addPlayerMan){

                                GUILayout.Space(5);

                                updateOptions.playManOpts = GUILayout.Toggle(updateOptions.playManOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[6].local, GUI.skin.button);

                            }//addOrUpdateOptions.addPlayerMan

                        }//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.updateProne){

                                GUILayout.Space(5);

                                updateOptions.proneOpts = GUILayout.Toggle(updateOptions.proneOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[7].local, GUI.skin.button);

                            }//addOrUpdateOptions.updateProne

                        }//

                        //if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.proneOpts && !updateOptions.playManOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            //if(addOrUpdateOptions.addScreenEvents){

                                //GUILayout.Space(5);

                                //updateOptions.screenEventsOpts = GUILayout.Toggle(updateOptions.screenEventsOpts, "Screen Events Options", GUI.skin.button);

                            //}//addOrUpdateOptions.addScreenEvents

                        //}//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.proneOpts && !updateOptions.playManOpts && !updateOptions.screenEventsOpts && !updateOptions.zoomOpts){

                            if(addOrUpdateOptions.addSubActions){

                                GUILayout.Space(5);

                                updateOptions.subActsOpts = GUILayout.Toggle(updateOptions.subActsOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[8].local, GUI.skin.button);

                            }//addOrUpdateOptions.addSubActions

                        }//

                        if(!updateOptions.featOpts && !updateOptions.playOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts){

                            if(addOrUpdateOptions.updateZoom){

                                GUILayout.Space(5);

                                updateOptions.zoomOpts = GUILayout.Toggle(updateOptions.zoomOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[9].local, GUI.skin.button);

                            }//addOrUpdateOptions.updateZoom

                        }//


    //////////////////////////////////////
    ///
    ///     PLAYER UPDATE - LOWER BUTTONS
    ///
    //////////////////////////////////////


                        if(!updateOptions.playOpts && !updateOptions.featOpts && !updateOptions.audioFadeOpts && !updateOptions.dualWieldOpts && !updateOptions.fovOpts && !updateOptions.matOpts && !updateOptions.playManOpts && !updateOptions.proneOpts && !updateOptions.screenEventsOpts && !updateOptions.subActsOpts && !updateOptions.zoomOpts){

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(character != null){

                                if(character != null && playerIsHFPS){

                                    if(!addOrUpdateOptions.addAudioFader && !addOrUpdateOptions.addDualWield && !addOrUpdateOptions.addFOVMan && !addOrUpdateOptions.addMaterialCont && !addOrUpdateOptions.addPlayerMan && !addOrUpdateOptions.updateProne && !addOrUpdateOptions.addScreenEvents && !addOrUpdateOptions.addSubActions && !addOrUpdateOptions.addReferences && !addOrUpdateOptions.updateZoom){

                                        GUI.enabled = false;

                                    } else {

                                        GUI.enabled = true;

                                    }

                                //character != null
                                } else {

                                    GUI.enabled = false;

                                }//character != null

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[21].local)){

                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].buttons[1].local)){

                                        Player_Update();

                                    }//DisplayDialog

                                }//button

                            //character != null
                            } else {

                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[22].local)){

                                    Player_Catch();

                                }//button

                            }//character != null

                            GUILayout.Space(5);

                            if(character != null){

                                if(playerIsHFPS){

                                    if(template != null){

                                        GUI.enabled = true;

                                    //template != null
                                    } else {

                                        GUI.enabled = false;

                                    }//template != null

                                //playerIsHFPS
                                } else {

                                    GUI.enabled = false;

                                }//playerIsHFPS

                            //character != null
                            } else {

                                GUI.enabled = false;

                            }//character != null

                            EditorGUILayout.BeginHorizontal();

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[23].local)){

                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].buttons[1].local)){

                                    Template_Save();

                                }//DisplayDialog

                            }//button

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[24].local)){

                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].buttons[1].local)){

                                    Template_Load();

                                }//DisplayDialog

                            }//button

                            EditorGUILayout.EndHorizontal();

                            GUILayout.Space(5);

                            GUI.enabled = true;

                            EditorGUILayout.BeginHorizontal();

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[25].local)){

                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[3].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[3].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[3].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[3].buttons[1].local)){

                                    Player_UpdateDefaults();

                                }//DisplayDialog

                            }//button

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                        }//all buttons off


    //////////////////////////////////////
    ///
    ///     PLAYER OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.playOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[0].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.playOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.playOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[1].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            EditorGUILayout.BeginVertical();

                            addOrUpdateOptions.updateProne = GUILayout.Toggle(addOrUpdateOptions.updateProne, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[26].local, GUI.skin.button);

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[2].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            EditorGUILayout.BeginVertical();

                            addOrUpdateOptions.updateZoom = GUILayout.Toggle(addOrUpdateOptions.updateZoom, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[27].local, GUI.skin.button);

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[28].local)){

                                PlayerUpdate_AddState(true);

                            }//button

                            GUILayout.Space(5);

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[29].local)){

                                PlayerUpdate_AddState(false);

                            }//button

                            EditorGUILayout.Space();

                        }//playOpts


    //////////////////////////////////////
    ///
    ///     FEATURES OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.featOpts){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[1].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.featOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.featOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[3].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            EditorGUILayout.BeginVertical();

                            addOrUpdateOptions.addScreenEvents = GUILayout.Toggle(addOrUpdateOptions.addScreenEvents, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[30].local, GUI.skin.button);

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[4].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            EditorGUILayout.BeginHorizontal();

                            addOrUpdateOptions.addDualWield = GUILayout.Toggle(addOrUpdateOptions.addDualWield, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[31].local, GUI.skin.button);
                            addOrUpdateOptions.addSubActions = GUILayout.Toggle(addOrUpdateOptions.addSubActions, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[32].local, GUI.skin.button);

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[5].local, EditorStyles.centeredGreyMiniLabel);

                            EditorGUILayout.Space();

                            EditorGUILayout.BeginHorizontal();

                            addOrUpdateOptions.addAudioFader = GUILayout.Toggle(addOrUpdateOptions.addAudioFader, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[33].local, GUI.skin.button);
                            addOrUpdateOptions.addFOVMan = GUILayout.Toggle(addOrUpdateOptions.addFOVMan, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[34].local, GUI.skin.button);

                            EditorGUILayout.EndHorizontal();

                            GUILayout.Space(5);

                            EditorGUILayout.BeginHorizontal();

                            addOrUpdateOptions.addMaterialCont = GUILayout.Toggle(addOrUpdateOptions.addMaterialCont, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[35].local, GUI.skin.button);
                            addOrUpdateOptions.addPlayerMan = GUILayout.Toggle(addOrUpdateOptions.addPlayerMan, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[36].local, GUI.skin.button);

                            EditorGUILayout.EndHorizontal();

                            GUILayout.Space(5);

                            EditorGUILayout.BeginHorizontal();

                            addOrUpdateOptions.addReferences = GUILayout.Toggle(addOrUpdateOptions.addReferences, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[37].local, GUI.skin.button);

                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.Space();

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.EndVertical();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[28].local)){

                                FeaturesUpdate_AllState(true);

                            }//button

                            GUILayout.Space(5);

                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[29].local)){

                                FeaturesUpdate_AllState(false);

                            }//button

                            EditorGUILayout.Space();

                        }//featOpts


    //////////////////////////////////////
    ///
    ///     AUDIO FADER OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.audioFadeOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[2].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.audFade_scrollPos = GUILayout.BeginScrollView(scrollPositions.audFade_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            audioFaderOptions.fadeMulti = EditorGUILayout.FloatField("Fade Multiplier", audioFaderOptions.fadeMulti);

                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(sources, true);

                            EditorGUILayout.Space();

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//audioFadeOpts


    //////////////////////////////////////
    ///
    ///     DUAL WIELD OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.dualWieldOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[3].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            //EditorGUILayout.PropertyField(dualWieldItemsRef, true);

                            scrollPositions.dualWield_scrollPos = GUILayout.BeginScrollView(scrollPositions.dualWield_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            if(dualWieldItems.Count > 0){

                                EditorGUILayout.Space();

                                for(int dwi = 0; dwi < dualWieldItems.Count; dwi++){

                                    EditorGUILayout.BeginHorizontal();

                                    dualWieldItems[dwi].flags = EditorGUILayout.MaskField(dualWieldItems[dwi].name, dualWieldItems[dwi].flags, dualWieldItems[dwi].options.ToArray());

                                    for(int dwio = 0; dwio < dualWieldItems[dwi].options.Count; dwio++){

                                        if(dualWieldItems[dwi].flags == 0 && dualWieldItems[dwi].selectedOptions.Count > 0){

                                            dualWieldItems[dwi].selectedOptions.Clear();

                                        }//flags = 0 & selectedOptions.Count > 0

                                        if ((dualWieldItems[dwi].flags & (1 << dwio )) == (1 << dwio)) {

                                            if(!dualWieldItems[dwi].selectedOptions.Contains(dualWieldItems[dwi].options[dwio])){

                                                dualWieldItems[dwi].selectedOptions.Add(dualWieldItems[dwi].options[dwio]);

                                            }//!contains

                                        }//flags

                                    }//for dwio options

                                    GUILayout.Space(10);

                                    dualWieldItems[dwi].canDualWield = GUILayout.Toggle(dualWieldItems[dwi].canDualWield, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[10].local, GUI.skin.button);

                                    EditorGUILayout.EndHorizontal();

                                }//for dwi dualWieldItems

                            //dualWieldItems.Count > 0
                            } else {

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[6].local, MessageType.Error);

                            }//dualWieldItems.Count > 0

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//dualWieldOpts


    //////////////////////////////////////
    ///
    ///     FOV OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.fovOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[4].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.fov_scrollPos = GUILayout.BeginScrollView(scrollPositions.fov_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            if(!fovAuto.genOpts && !fovAuto.statesOpts){

                                fovAuto.camOpts = GUILayout.Toggle(fovAuto.camOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[11].local, GUI.skin.button);

                            }//!genOpts & !statesOpts

                            if(fovAuto.camOpts){

                                EditorGUILayout.Space();

                                fovOptions.autoFindCameras = EditorGUILayout.Toggle("Auto Find Cameras?", fovOptions.autoFindCameras);

                                EditorGUILayout.Space();

                                if(!fovOptions.autoFindCameras){

                                    EditorGUILayout.PropertyField(cameras, true);

                                //!autoFindCameras
                                } else {

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[7].local, MessageType.Info);

                                }//!autoFindCameras

                            }//camOpts

                            if(!fovAuto.camOpts && !fovAuto.statesOpts){

                                EditorGUILayout.Space();

                                fovAuto.genOpts = GUILayout.Toggle(fovAuto.genOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[12].local, GUI.skin.button);

                            }//!camOpts & !statesOpts

                            if(fovAuto.genOpts){

                                EditorGUILayout.Space();

                                fovOptions.globalZoomMulti = EditorGUILayout.FloatField("Global Zoom Multi", fovOptions.globalZoomMulti);
                                fovOptions.globalUnZoomMulti = EditorGUILayout.FloatField("Global UnZoom Multi", fovOptions.globalUnZoomMulti);

                            }//genOpts

                            if(!fovAuto.camOpts && !fovAuto.genOpts){

                                EditorGUILayout.Space();

                                fovAuto.statesOpts = GUILayout.Toggle(fovAuto.statesOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[13].local, GUI.skin.button);

                            }//!camOpts & !genOpts

                            if(fovAuto.statesOpts){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(camStates, new GUIContent("States"), true);

                            }//statesOpts

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//fovOpts


    //////////////////////////////////////
    ///
    ///     MATERIAL OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.matOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[5].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.matCont_scrollPos = GUILayout.BeginScrollView(scrollPositions.matCont_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(bodyParts, true);

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//matOpts


    //////////////////////////////////////
    ///
    ///     PLAYER MANAGER OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.playManOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[6].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.playMan_scrollPos = GUILayout.BeginScrollView(scrollPositions.playMan_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            if(!playerManAuto.meleeOpts && !playerManAuto.weapOpts){

                                playerManAuto.genOpts = GUILayout.Toggle(playerManAuto.genOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[12].local, GUI.skin.button);

                            }//!meleeOpts & !weapOpts

                            if(playerManAuto.genOpts){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(slowDownUse, true);

                            }//genOpts

                            if(!playerManAuto.genOpts && !playerManAuto.weapOpts){

                                EditorGUILayout.Space();

                                playerManAuto.meleeOpts = GUILayout.Toggle(playerManAuto.meleeOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[14].local, GUI.skin.button);

                            }//!genOpts & !weapOpts

                            if(playerManAuto.meleeOpts){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeConts, true);

                            }//meleeOpts

                            if(!playerManAuto.meleeOpts && !playerManAuto.genOpts){

                                EditorGUILayout.Space();

                                playerManAuto.weapOpts = GUILayout.Toggle(playerManAuto.weapOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[15].local, GUI.skin.button);

                            }//!meleeOpts & !genOpts

                            if(playerManAuto.weapOpts){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(weaponConts, true);

                            }//weapOpts

                            EditorGUILayout.Space();

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//playManOpts


    //////////////////////////////////////
    ///
    ///     PRONE OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.proneOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[7].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.prone_scrollPos = GUILayout.BeginScrollView(scrollPositions.prone_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            if(!proneAuto.contFeatures && !proneAuto.contAdjust){

                                proneAuto.basicSettings = GUILayout.Toggle(proneAuto.basicSettings, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[16].local, GUI.skin.button);

                            }//!contFeatures & !contAdjust

                            if(proneAuto.basicSettings){

                                EditorGUILayout.Space();

                                proneOptions.proneSpeed = EditorGUILayout.FloatField("Prone Speed", proneOptions.proneSpeed);

                            }//basicSettings

                            if(!proneAuto.basicSettings && !proneAuto.contAdjust){

                                EditorGUILayout.Space();

                                proneAuto.contFeatures = GUILayout.Toggle(proneAuto.contFeatures, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[17].local, GUI.skin.button);

                            }//!basicSettings & !contAdjust

                            if(proneAuto.contFeatures){

                                EditorGUILayout.Space();

                                proneOptions.enableProne = EditorGUILayout.Toggle("Enable Prone?", proneOptions.enableProne);

                            }//contFeatures

                            if(!proneAuto.basicSettings && !proneAuto.contFeatures){

                                EditorGUILayout.Space();

                                proneAuto.contAdjust = GUILayout.Toggle(proneAuto.contAdjust, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[18].local, GUI.skin.button);

                            }//!basicSettings & !contFeatures

                            if(proneAuto.contAdjust){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(proneCenter, true);

                            }//contAdjust

                            EditorGUILayout.Space();

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//proneOpts


    //////////////////////////////////////
    ///
    ///     SCREEN EVENTS OPTIONS
    ///
    //////////////////////////////////////


                        //if(updateOptions.screenEventsOpts){

                            //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            //EditorGUILayout.HelpBox("\n" + "Screen Events area" + "\n", MessageType.Info);

                            //EditorGUILayout.Space();

                        //}//screenEventsOpts


    //////////////////////////////////////
    ///
    ///     SUB ACTIONS OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.subActsOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[8].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            scrollPositions.subActs_scrollPos = GUILayout.BeginScrollView(scrollPositions.subActs_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            EditorGUILayout.Space();

                            if(!subActionsAuto.inputOpts){

                                subActionsAuto.actOpts = GUILayout.Toggle(subActionsAuto.actOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[19].local, GUI.skin.button);

                            }//!inputOpts

                            if(subActionsAuto.actOpts){

                                EditorGUILayout.Space();

                                subActionsOptions.useActionDelay = EditorGUILayout.Toggle("Use Action Delay?", subActionsOptions.useActionDelay);
                                subActionsOptions.actionDelay = EditorGUILayout.FloatField("Action Delay", subActionsOptions.actionDelay);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(subActionsRef, true);

                            }//actOpts

                            if(!subActionsAuto.actOpts){

                                EditorGUILayout.Space();

                                subActionsAuto.inputOpts = GUILayout.Toggle(subActionsAuto.inputOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[20].local, GUI.skin.button);

                            }//!actOpts

                            if(subActionsAuto.inputOpts){

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(inputType, true);
                                subActionsOptions.holdTime = EditorGUILayout.FloatField("Hold Time", subActionsOptions.holdTime);
                                subActionsOptions.holdMulti = EditorGUILayout.FloatField("Hold Multi", subActionsOptions.holdMulti);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(actionInputs, true);

                            }//inputOpts

                            EditorGUILayout.Space();

                            EditorGUILayout.EndScrollView();

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.EndVertical();

                        }//subActsOpts


    //////////////////////////////////////
    ///
    ///     ZOOM OPTIONS
    ///
    //////////////////////////////////////


                        if(updateOptions.zoomOpts){

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[9].text, MessageType.Info);

                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(zoomType, true);

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local, MessageType.Error);

                            EditorGUILayout.Space();

                            EditorGUILayout.EndVertical();

                        }//zoomOpts

                        #else 

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local, MessageType.Error);

                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        #endif

                    }//useType = update

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

        }//PlayerCreator_Screen


    //////////////////////////////////////
    ///
    ///     LAUNCH ACTIONS
    ///
    ///////////////////////////////////////


        public void Launch_RagdollCreator(){

            DM_RagdollCreator window = (DM_RagdollCreator)EditorWindow.GetWindow<DM_RagdollCreator>(false, "Ragdoll Creator", true);
            window.OpenWizard_Single();

        }//Launch_RagdollCreator


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    //////////////////////////////////////

    //////////////////////////
    ///
    ///     PLAYER
    ///
    //////////////////////////


        private void Player_Catch(){

            var allMonoBehaviours = FindObjectsOfType<PlayerController>();

            if(allMonoBehaviours.Length > 0){

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[8].local);

                character = allMonoBehaviours[0].gameObject;

                #if COMPONENTS_PRESENT

                    playerIsHFPS = true;
                    playerChecked = true;

                #endif

                PlayerItems_Catch();

                EditorUtility.SetDirty(this);

            }//allMonoBehaviours.Length > 0

        }//Player_Catch

        private void PlayerItems_Catch(){

            ItemSwitcher tempItemSwitch = null;

            dualWieldItems = new List<DualWield_Item>();    
            playerItems = new List<GameObject>();

            foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                if(child.GetComponent<ItemSwitcher>() != null){

                    tempItemSwitch = child.GetComponent<ItemSwitcher>();

                }//ItemSwitcher != null

            }//foreach child

            if(tempItemSwitch != null){

                if(tempItemSwitch.ItemList.Count > 0){

                    for(int i = 0; i < tempItemSwitch.ItemList.Count; i++){

                        DualWield_Item newItem = new DualWield_Item();
                        newItem.name = tempItemSwitch.ItemList[i].name;

                        dualWieldItems.Add(newItem);
                        playerItems.Add(tempItemSwitch.ItemList[i]);

                        //Debug.Log("Added " + itemSwitch[0].ItemList[i].name);

                    }//for i ItemList

                }//ItemList.Count > 0

                if(playerItems.Count == tempItemSwitch.ItemList.Count){

                    for(int pi = 0; pi < playerItems.Count; pi++){

                        for(int dwi = 0; dwi < dualWieldItems.Count; dwi++){

                            if(playerItems[pi].name != dualWieldItems[dwi].name){

                                dualWieldItems[dwi].options.Add(playerItems[pi].name);

                            }//name != name

                        }//for dwi dualWieldItems

                    }//for pi playerItems

                }//playerItems.Count = ItemList.Count

            }//tempItemSwitch != null

        }//PlayerItems_Catch

        private bool Player_Check(){

            if(character != null){

                #if COMPONENTS_PRESENT

                    playerChecked = true;

                #endif

                if(character.GetComponent<PlayerController>() != null){

                    //Debug.Log("Player IS HFPS");
                    return true;

                //PlayerController != null
                } else {

                    //Debug.Log("Player is NOT HFPS");
                    return false;

                }//PlayerController != null

            }//character != null

            return false;

        }//Player_Check

        private void Player_Create(){

            if(characterType == Character_Type.Generic){

                GameObject genPlayer = null;
                GameObject newPlayer = null;

                if(createType == Create_Type.AOrB){

                    genPlayer = (GameObject)Resources.Load("_Extensions/Prefabs/Player Creator/Player/Generic/HEROPLAYER (Generic)");

                }//createType = A or B

                if(createType == Create_Type.C){

                    genPlayer = (GameObject)Resources.Load("_Extensions/Prefabs/Player Creator/Player/Generic/HEROPLAYER (Generic) (1.6.3c)");

                }//createType = C

                newPlayer = Instantiate(genPlayer);

                if(createType == Create_Type.AOrB){

                    newPlayer.name = "HEROPLAYER (Generic)(New)";

                }//createType = A or B

                if(createType == Create_Type.C){

                    newPlayer.name = "HEROPLAYER (Generic)(1.6.3c)(New)";

                }//createType = C

                if(gameOptions.updGameMan){

                    var gameMan = FindObjectsOfType<HFPS_GameManager>();

                    if(gameMan.Length > 0){

                        gameMan[0].m_PlayerObj = newPlayer;

                    }//gameMan.Length > 0

                }//update game manager

                if(createOptions.customPosition){

                    newPlayer.transform.parent = createOptions.createPos;
                    newPlayer.transform.localPosition = new Vector3(0, 0, 0);
                    newPlayer.transform.localEulerAngles = new Vector3(0, 0, 0);
                    newPlayer.transform.parent = null;

                }//customPosition

            }//characterType = generic

            if(characterType == Character_Type.ComponentsGeneric){

                GameObject compPlayer = null;
                GameObject newPlayer = null;

                if(createType == Create_Type.AOrB){

                    compPlayer = (GameObject)Resources.Load("_Extensions/Prefabs/Player Creator/Player/Custom/HEROPLAYER (custom)");

                }//createType = A or B

                if(createType == Create_Type.C){

                    compPlayer = (GameObject)Resources.Load("_Extensions/Prefabs/Player Creator/Player/Custom/HEROPLAYER (custom) (1.6.3c)");

                }//createType = C

                newPlayer = Instantiate(compPlayer);

                if(createType == Create_Type.AOrB){

                    newPlayer.name = "HEROPLAYER (custom)(New)";

                }//createType = A or B

                if(createType == Create_Type.C){

                    newPlayer.name = "HEROPLAYER (custom)(1.6.3c)(New)";

                }//createType = C

                if(gameOptions.updGameMan){

                    var gameMan = FindObjectsOfType<HFPS_GameManager>();

                    if(gameMan.Length > 0){

                        gameMan[0].m_PlayerObj = newPlayer;

                    }//gameMan.Length > 0

                }//update game manager

                if(createOptions.customPosition){

                    newPlayer.transform.parent = createOptions.createPos;
                    newPlayer.transform.localPosition = new Vector3(0, 0, 0);
                    newPlayer.transform.localEulerAngles = new Vector3(0, 0, 0);
                    newPlayer.transform.parent = null;

                }//customPosition

            }//characterType = components generic

            if(characterType == Character_Type.Custom){

                if(character != null && playerLayout != null && playerAnimator != null){

                    ragdollOptions.tempBodyAnim = null;

                    GameObject newPlayer = Instantiate(playerLayout);
                    newPlayer.name = "HEROPLAYER (" + character.name + ")";

                    GameObject newCharacter = Instantiate(character);
                    newCharacter.name = character.name;

                    foreach(Transform child in newPlayer.transform.GetComponentsInChildren<Transform>()){

                        if(child.name == "HeroBody"){

                            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local);

                            newCharacter.transform.parent = child;
                            newCharacter.transform.localPosition = new Vector3(0, 0, 0);
                            newCharacter.transform.localEulerAngles = new Vector3(0, 0, 0);

                            if(child.GetComponent<BodyAnimator>() != null){

                                ragdollOptions.tempBodyAnim = child.GetComponent<BodyAnimator>();

                            }//BodyAnimator != null

                        }//name = HeroBody

                    }//foreach child

                    newCharacter.layer = playerCreateOptions.limbVisibleLayer;

                    foreach(Transform childChar in newCharacter.transform.GetComponentsInChildren<Transform>(true)){

                        childChar.gameObject.layer = playerCreateOptions.limbVisibleLayer;

                    }//foreach childChar

                    if(playerCreateOptions.invisibleParts.Count > 0){

                        for(int ivp = 0; ivp < playerCreateOptions.invisibleParts.Count; ivp++){

                            playerCreateOptions.invisibleParts[ivp].layer = playerCreateOptions.invisibleLayer;

                        }//for ivp invisibleParts

                    }//invisibleParts.Count > 0

                    newCharacter.GetComponent<Animator>().runtimeAnimatorController = playerAnimator;
                    newCharacter.GetComponent<Animator>().cullingMode = UnityEngine.AnimatorCullingMode.AlwaysAnimate;

                    HFPS.Systems.AnimationEvent newAnimEvents = newCharacter.AddComponent<HFPS.Systems.AnimationEvent>();

                    HFPS.Systems.AnimationEvent.AnimEvents tempEvent = new HFPS.Systems.AnimationEvent.AnimEvents();
                    tempEvent.EventCallName = "Footstep";
                    tempEvent.CallEvent = new UnityEvent();

                    UnityAction methodDelegate = System.Delegate.CreateDelegate (typeof(UnityAction), newPlayer.GetComponent<FootstepsController>(), "PlayFootstep") as UnityAction;
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent.CallEvent, methodDelegate);

                    newAnimEvents.AnimationEvents = new HFPS.Systems.AnimationEvent.AnimEvents[1];
                    newAnimEvents.AnimationEvents[0] = tempEvent;

                    ragdollOptions.tempBodyAnim.InvisibleLayer = playerCreateOptions.invisibleLayer;
                    ragdollOptions.tempBodyAnim.LimbVisibleLayer = playerCreateOptions.limbVisibleLayer;

                    if(playerCreateOptions.spineBones.Count > 0){

                        ragdollOptions.tempBodyAnim.SpineBones = new BodyAnimator.SpineBone[playerCreateOptions.spineBones.Count];

                        for(int spb = 0; spb < playerCreateOptions.spineBones.Count; spb++){

                            ragdollOptions.tempBodyAnim.SpineBones[spb] = playerCreateOptions.spineBones[spb];

                        }//for spb spineBones

                    //spineBones.Count > 0
                    } else {

                        ragdollOptions.tempBodyAnim.SpineBones = new BodyAnimator.SpineBone[0];

                    }//spineBones.Count > 0

                    if(playerCreateOptions.activateLimbs.Count > 0){

                        ragdollOptions.tempBodyAnim.ActivateLimbs = new GameObject[playerCreateOptions.activateLimbs.Count];

                        for(int al = 0; al < playerCreateOptions.activateLimbs.Count; al++){

                            ragdollOptions.tempBodyAnim.ActivateLimbs[al] = playerCreateOptions.activateLimbs[al];

                        }//for al activateLimbs

                    //activateLimbs.Count > 0
                    } else {

                        ragdollOptions.tempBodyAnim.ActivateLimbs = new GameObject[0];

                    }//activateLimbs.Count > 0

                    if(playerCreateOptions.deactivateLimbs.Count > 0){

                        ragdollOptions.tempBodyAnim.DeactivateLimbs = new GameObject[playerCreateOptions.deactivateLimbs.Count];

                        for(int del = 0; del < playerCreateOptions.deactivateLimbs.Count; del++){

                            ragdollOptions.tempBodyAnim.DeactivateLimbs[del] = playerCreateOptions.deactivateLimbs[del];

                        }//for del deactivateLimbs

                    //deactivateLimbs.Count > 0
                    } else {

                        ragdollOptions.tempBodyAnim.DeactivateLimbs = new GameObject[0];

                    }//deactivateLimbs.Count > 0

                    if(ragdollOptions.addDeathParent){

                        Transform tempHead = newCharacter.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);

                        if(tempHead != null){

                            GameObject newDP = new GameObject();
                            newDP.name = "DeathParent";

                            newDP.transform.parent = tempHead;

                            newDP.transform.localPosition = new Vector3(0, 0, 0);
                            newDP.transform.localEulerAngles = new Vector3(0, 0, 0);
                            newDP.transform.localScale = new Vector3(1f, 1f, 1f);

                            newDP.layer = playerCreateOptions.limbVisibleLayer;

                            ragdollOptions.tempBodyAnim.NewDeathCamParent = newDP.transform;

                        }//tempHead != null

                    //addDeathParent
                    } else {

                        Transform tempTrans = null;

                        foreach(Transform childDeath in newCharacter.transform.GetComponentsInChildren<Transform>(true)){

                            if(childDeath.name == "DeathParent"){

                                tempTrans = childDeath;
                                ragdollOptions.tempBodyAnim.NewDeathCamParent = childDeath;

                            }//name = DeathParent

                        }//foreach childDeath 

                        if(tempTrans == null){

                            ragdollOptions.tempBodyAnim.NewDeathCamParent = null;

                        }//tempTrans = null

                    }//addDeathParent

                    if(ragdollOptions.fixHeadCollider){

                        Transform tempHead = newCharacter.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
                        CapsuleCollider tempHeadCol = new CapsuleCollider();

                        if(tempHead != null){

                            if(tempHead.GetComponent<Collider>()){

                                DestroyImmediate(tempHead.GetComponent<Collider>());

                            }//collider present

                            tempHeadCol = tempHead.gameObject.AddComponent<CapsuleCollider>();

                            tempHeadCol.enabled = false;

                            if(!ragdollOptions.autoHeadFix){

                                tempHeadCol.center = ragdollOptions.headCenter;
                                tempHeadCol.radius = ragdollOptions.headRadius;
                                tempHeadCol.height = ragdollOptions.headHeight;
                                tempHeadCol.direction = (int)ragdollOptions.headDirection;

                            //!autoHeadFix
                            } else {

                                tempHeadCol.center = new Vector3(-8.11303e-07f, 11.35702f, 11.58915f);
                                tempHeadCol.radius = 11.91596f;
                                tempHeadCol.height = 37.54185f;
                                tempHeadCol.direction = 2;

                            }//!autoHeadFix

                        }//tempHead != null

                    }//fixHeadCollider

                    bool stopRootSearch = false;

                    foreach(Transform child in newPlayer.transform.GetComponentsInChildren<Transform>(true)){

                        if(!stopRootSearch){

                            if(child.name == "HeroRoot"){

                                stopRootSearch = true;

                                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local);

                                DestroyImmediate(child.gameObject);

                            }//name = HeroRoot

                        }//!stopRootSearch

                    }//foreach child

                    if(gameOptions.updGameMan){

                        var gameMan = FindObjectsOfType<HFPS_GameManager>();

                        if(gameMan.Length > 0){

                            gameMan[0].m_PlayerObj = newPlayer;

                        }//gameMan.Length > 0

                    }//update game manager

                    if(createOptions.customPosition){

                        newPlayer.transform.parent = createOptions.createPos;
                        newPlayer.transform.localPosition = new Vector3(0, 0, 0);
                        newPlayer.transform.localEulerAngles = new Vector3(0, 0, 0);
                        newPlayer.transform.parent = null;

                    }//customPosition

                    if(createOptions.destroyChar){

                        DestroyImmediate(character);

                        character = null;

                    //destroyChar
                    } else {

                        character.SetActive(false);   

                    }//destroyChar

                }//character, playerLayout & playerAnimator != null

            }//characterType = custom

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local);

        }//Player_Create

        private void Player_CreateDefaults(bool showDebug){

            gameOptions.updGameMan = true;

            playerLayout = (GameObject)Resources.Load("Setup/Game/HEROPLAYER");
            playerAnimator = Animator_Find("PlayerBody Animator");

            playerCreateOptions.limbVisibleLayer = 11;
            playerCreateOptions.invisibleLayer = 12;

            ragdollOptions.addDeathParent = true;
            ragdollOptions.fixHeadCollider = true;

            if(showDebug){

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[7].local);

            }//showDebug

        }//Player_CreateDefaults

        private void Player_Update(){

            #if COMPONENTS_PRESENT

            if(character != null){

                HFPS_References tempReferences = new HFPS_References();
                HFPS_AudioFader tempAudFade = new HFPS_AudioFader();
                HFPS_FOVManager tempFOVMan = new HFPS_FOVManager();
                HFPS_PlayerMan tempPlayMan = new HFPS_PlayerMan();
                HFPS_MaterialCont tempMatCont = new HFPS_MaterialCont();


    //////////////////////////////////
    ///
    ///     ADD NEW
    ///
    //////////////////////////////////

    /////////////////////////
    ///
    ///     MAIN COMPONENTS
    ///
    /////////////////////////

    //////////////////
    ///
    ///     REFERENCES
    ///
    //////////////////


                if(addOrUpdateOptions.addReferences){

                    if(character.GetComponent<HFPS_References>() == null){

                        tempReferences = character.AddComponent<HFPS_References>();

                        tempReferences.characterController = character.GetComponent<CharacterController>();
                        tempReferences.playCont = character.GetComponent<PlayerController>();

                        tempReferences.playerRigid = character.GetComponent<Rigidbody>();
                        tempReferences.healthManager = character.GetComponent<HealthManager>();

                        ScriptManager tempScriptMan = null;

                        foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                            if(child.GetComponent<ScriptManager>() != null){

                                tempScriptMan = child.GetComponent<ScriptManager>();

                            }//ScriptManager != null

                        }//foreach child

                        if(tempScriptMan != null){

                            tempReferences.playerFunct = tempScriptMan.gameObject.GetComponent<PlayerFunctions>();
                            tempReferences.mouseLook = tempScriptMan.gameObject.GetComponent<MouseLook>();
                            tempReferences.jsEffects = tempScriptMan.gameObject.GetComponent<JumpscareEffects>();

                        }//tempScriptMan != null

                        ItemSwitcher tempItemSwitch = null;

                        foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                            if(child.GetComponent<ItemSwitcher>() != null){

                                tempItemSwitch = child.GetComponent<ItemSwitcher>();

                            }//ItemSwitcher != null

                        }//foreach child

                        if(tempItemSwitch != null){

                            tempReferences.itemSwitcher = tempItemSwitch;

                        }//tempItemSwitch != null

                    }//HFPS_References = null

                }//addReferences


    //////////////////
    ///
    ///     AUDIO FADER
    ///
    //////////////////


                if(addOrUpdateOptions.addAudioFader){

                    if(character.GetComponent<HFPS_AudioFader>() == null){

                        tempAudFade = character.AddComponent<HFPS_AudioFader>();

                        tempAudFade.fadeMulti = audioFaderOptions.fadeMulti;

                        if(audioFaderOptions.sources.ambience != null){

                            tempAudFade.ambience = audioFaderOptions.sources.ambience;

                        }//ambience != null

                        if(audioFaderOptions.sources.music != null){

                            tempAudFade.music = audioFaderOptions.sources.music;

                        }//music != null

                    }//HFPS_AudioFader = null

                }//addAudioFader


    //////////////////
    ///
    ///     FOV MANAGER
    ///
    //////////////////


                if(addOrUpdateOptions.addFOVMan){

                    if(character.GetComponent<HFPS_FOVManager>() == null){

                        tempFOVMan = character.AddComponent<HFPS_FOVManager>();

                        tempFOVMan.globalZoomMulti = fovOptions.globalZoomMulti;
                        tempFOVMan.globalUnzoomMulti = fovOptions.globalUnZoomMulti;

                        if(fovOptions.states.Count > 0){

                            tempFOVMan.states = new List<HFPS_FOVManager.Camera_State>();

                            for(int fvs = 0; fvs < fovOptions.states.Count; fvs++){

                                tempFOVMan.states.Add(fovOptions.states[fvs]);

                            }//for fvs states 

                        }//states.Count > 0

                        if(fovOptions.autoFindCameras){

                            if(fovOptions.cameras.Count > 0){

                                tempFOVMan.cameras = new List<Camera>();

                                for(int c = 0; c < fovOptions.cameras.Count; c++){

                                    tempFOVMan.cameras.Add(fovOptions.cameras[c]);

                                }//for c cameras

                            //cameras.Count > 0
                            } else {

                                ScriptManager tempScriptMan = null;

                                foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                                    if(child.GetComponent<ScriptManager>() != null){

                                        tempScriptMan = child.GetComponent<ScriptManager>();

                                    }//ScriptManager != null

                                }//foreach child

                                if(tempScriptMan != null){

                                    tempFOVMan.cameras.Add(tempScriptMan.MainCamera);
                                    tempFOVMan.cameras.Add(tempScriptMan.ArmsCamera);

                                }//tempScriptMan != null

                            }//cameras.Count > 0

                        //autoFindCameras
                        } else {

                            if(fovOptions.cameras.Count > 0){

                                for(int c = 0; c < fovOptions.cameras.Count; c++){

                                    tempFOVMan.cameras.Add(fovOptions.cameras[c]);

                                }//for c cameras

                            }//cameras.Count > 0

                        }//autoFindCameras

                    }//HFPS_FOVManager = null

                }//addFOVMan


    //////////////////
    ///
    ///     PLAYER MANAGER
    ///
    //////////////////


                if(addOrUpdateOptions.addPlayerMan){

                    if(character.GetComponent<HFPS_PlayerMan>() == null){

                        tempPlayMan = character.AddComponent<HFPS_PlayerMan>();

                        tempPlayMan.references = tempReferences;
                        tempReferences.playerMan = tempPlayMan;

                        tempPlayMan.slowDownUse = (HFPS_PlayerMan.SlowDown)playerManOptions.slowDownUse;

                        if(playerManOptions.meleeConts.Count > 0){

                            tempPlayMan.meleeConts = new List<HFPS_PlayerMan.Melee_Cont>();

                            for(int mc = 0; mc < playerManOptions.meleeConts.Count; mc++){

                                tempPlayMan.meleeConts.Add(playerManOptions.meleeConts[mc]);

                            }//for mc melee Conts

                            if(tempPlayMan.meleeConts.Count > 0){

                                if(playerItems.Count > 0){

                                    for(int pli = 0; pli < playerItems.Count; pli++){

                                        for(int tmc = 0; tmc < tempPlayMan.meleeConts.Count; tmc++){

                                            if(tempPlayMan.meleeConts[tmc].meleeCont == null){

                                                if(playerItems[pli].name == tempPlayMan.meleeConts[tmc].name){

                                                    tempPlayMan.meleeConts[tmc].meleeCont = playerItems[pli].GetComponent<MeleeController>();

                                                }//name = name

                                            }//meleeCont = null

                                        }//for tmc meleeConts

                                    }//for pli playerItems

                                }//playerItems.Count > 0

                            }//meleeConts.Count > 0

                        }//meleeConts.Count > 0

                        if(playerManOptions.weaponConts.Count > 0){

                            tempPlayMan.weaponConts = new List<HFPS_PlayerMan.Weapon_Cont>();

                            for(int wc = 0; wc < playerManOptions.weaponConts.Count; wc++){

                                tempPlayMan.weaponConts.Add(playerManOptions.weaponConts[wc]);

                            }//for wc weaponConts

                            if(tempPlayMan.weaponConts.Count > 0){

                                if(playerItems.Count > 0){

                                    for(int pli = 0; pli < playerItems.Count; pli++){

                                        for(int twc = 0; twc < tempPlayMan.weaponConts.Count; twc++){

                                            if(tempPlayMan.weaponConts[twc].weaponCont == null){

                                                if(playerItems[pli].name == tempPlayMan.weaponConts[twc].name){

                                                    tempPlayMan.weaponConts[twc].weaponCont = playerItems[pli].GetComponent<WeaponController>();

                                                }//name = name

                                            }//weaponCont = null

                                        }//for twc weaponConts

                                    }//for pli playerItems

                                }//playerItems.Count > 0

                            }//weaponConts.Count > 0

                        }//weaponConts.Count > 0

                    }//HFPS_PlayerMan = null

                }//addPlayerMan


    //////////////////
    ///
    ///     MATERIAL CONTROLLER
    ///
    //////////////////


                if(addOrUpdateOptions.addMaterialCont){

                    if(character.GetComponent<HFPS_MaterialCont>() == null){

                        tempMatCont = character.AddComponent<HFPS_MaterialCont>();

                        if(matContOptions.bodyParts.Count > 0){

                            for(int bp = 0; bp < matContOptions.bodyParts.Count; bp++){

                                tempMatCont.bodyParts.Add(matContOptions.bodyParts[bp]);

                            }//for bp bodyParts

                            if(tempMatCont.bodyParts.Count > 0){

                                List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();

                                foreach(Transform child in character.transform.GetComponentsInChildren<Transform>(true)){

                                    if(child.GetComponent<SkinnedMeshRenderer>() != null){

                                        skinnedMeshes.Add(child.GetComponent<SkinnedMeshRenderer>());

                                    }//SkinnedMeshRenderer != null

                                }//foreach child

                                if(skinnedMeshes.Count > 0){

                                    foreach(var meshes in skinnedMeshes){

                                        for(int tbp = 0; tbp < tempMatCont.bodyParts.Count; tbp++){

                                            if(tempMatCont.bodyParts[tbp].meshParts.Count > 0){

                                                for(int tmp = 0; tmp < tempMatCont.bodyParts[tbp].meshParts.Count; tmp++){

                                                    if(tempMatCont.bodyParts[tbp].meshParts[tmp].skinnedMesh == null){

                                                        if(tempMatCont.bodyParts[tbp].meshParts[tmp].nameOfMesh != ""){

                                                            if(meshes.name == tempMatCont.bodyParts[tbp].meshParts[tmp].nameOfMesh){

                                                                tempMatCont.bodyParts[tbp].meshParts[tmp].skinnedMesh = meshes;

                                                            }//name = nameOfMesh

                                                        //nameOfMesh != null
                                                        } else {

                                                            if(meshes.name == tempMatCont.bodyParts[tbp].meshParts[tmp].name){

                                                                tempMatCont.bodyParts[tbp].meshParts[tmp].skinnedMesh = meshes;

                                                            }//name = name

                                                        }//nameOfMesh != null

                                                    }//skinnedMesh = null

                                                }//for tmp meshParts

                                            }//meshParts.Count > 0

                                        }//for tbp bodyParts

                                    }//foreach meshes

                                }//skinnedMeshes.Length > 0

                            }//bodyParts.Count > 0

                        }//bodyParts.Count > 0

                    }//HFPS_MaterialCont = null

                }//addMaterialCont


    /////////////////////////
    ///
    ///     SUB COMPONENTS
    ///
    /////////////////////////

    //////////////////
    ///
    ///     DUAL WIELD
    ///
    //////////////////


                if(addOrUpdateOptions.addDualWield){

                    if(playerItems.Count > 0){

                        for(int pli = 0; pli < playerItems.Count; pli++){

                            if(playerItems[pli].GetComponent<HFPS_DualWield>() == null){

                                HFPS_DualWield tempDualWield = playerItems[pli].AddComponent<HFPS_DualWield>();

                                if(tempReferences.itemSwitcher != null){

                                    if(!tempReferences.itemSwitcher.dualWields.Contains(tempDualWield)){

                                        tempReferences.itemSwitcher.dualWields.Add(tempDualWield);

                                    }//!Contains

                                //itemSwitcher != null
                                } else {

                                    ItemSwitcher tempItemSwitch = null;

                                    foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                                        if(child.GetComponent<ItemSwitcher>() != null){

                                            tempItemSwitch = child.GetComponent<ItemSwitcher>();

                                        }//ItemSwitcher != null

                                    }//foreach child

                                    if(tempItemSwitch != null){

                                        if(!tempItemSwitch.dualWields.Contains(tempDualWield)){

                                            tempItemSwitch.dualWields.Add(tempDualWield);

                                        }//!Contains

                                    }//tempItemSwitch != null

                                }//itemSwitcher != null

                                if(dualWieldItems.Count > 0){

                                    tempDualWield.incompIDs = new List<int>();

                                    for(int dwi = 0; dwi < dualWieldItems.Count; dwi++){

                                        if(dualWieldItems[dwi].name == tempDualWield.name){

                                            tempDualWield.dualWield = dualWieldItems[dwi].canDualWield;

                                            if(dualWieldItems[dwi].selectedOptions.Count > 0){

                                                for(int so = 0; so < dualWieldItems[dwi].selectedOptions.Count; so++){

                                                    int tempInt = Options_Check(playerItems, dualWieldItems[dwi].selectedOptions[so]);

                                                    if(tempInt > -1){

                                                        tempDualWield.incompIDs.Add(tempInt);

                                                    }//tempInt > -1

                                                }//for so selectedOptions

                                            }//selectedOptions.Count > 0

                                        }//name = name

                                    }//for dwi dualWieldItems

                                }//dualWieldItems.Count > 0

                            }//HFPS_DualWield = null

                        }//playerItems

                    }//playerItems

                }//addDualWield 


    /////////////////////////
    ///
    ///     SUB OBJECTS
    ///
    /////////////////////////

    //////////////////
    ///
    ///     SCREEN EVENTS
    ///
    //////////////////


                if(addOrUpdateOptions.addScreenEvents){

                    ScriptManager tempScriptMan = null;

                    foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                        if(child.GetComponent<ScriptManager>() != null){

                            tempScriptMan = child.GetComponent<ScriptManager>();

                        }//ScriptManager != null

                    }//foreach child

                    if(tempScriptMan != null){

                        bool present = false;

                        foreach(Transform child in tempScriptMan.MainCamera.transform.GetComponentsInChildren<Transform>(true)){

                            if(child.GetComponent<HFPS_ScreenEvents>()){

                                present = true;

                            }//HFPS_ScreenEvents

                        }//foreach child

                        if(!present){

                            GameObject seObj = null;
                            GameObject newSEObj = null;

                            seObj = (GameObject)Resources.Load("_Extensions/Prefabs/Player Creator/Camera/Screen Events");
                            newSEObj = Instantiate(seObj);
                            newSEObj.name = "Screen Events";

                            newSEObj.transform.parent = tempScriptMan.MainCamera.transform;
                            newSEObj.transform.localPosition = new Vector3(0, 0, 0);
                            newSEObj.transform.localEulerAngles = new Vector3(0, 0, 0);

                            HFPS_ScreenEvents tempScreenEvents = newSEObj.GetComponent<HFPS_ScreenEvents>();

                            if(addOrUpdateOptions.addReferences){

                                tempReferences.screenEvents = tempScreenEvents;

                            }//addReferences

                        }//!present

                    }//tempScriptMan != null

                }//addScreenEvents 


    //////////////////
    ///
    ///     SUB ACTIONS
    ///
    //////////////////


                if(addOrUpdateOptions.addSubActions){

                    ScriptManager tempScriptMan = null;

                    foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                        if(child.GetComponent<ScriptManager>() != null){

                            tempScriptMan = child.GetComponent<ScriptManager>();

                        }//ScriptManager != null

                    }//foreach child

                    if(tempScriptMan != null){

                        bool present = false;

                        foreach(Transform child in tempScriptMan.ArmsCamera.transform.GetComponentsInChildren<Transform>(true)){

                            if(child.GetComponent<HFPS_SubActionsHandler>()){

                                present = true;

                            }//HFPS_SubActionsHandler

                        }//foreach child

                        if(!present){

                            GameObject saObj = null;
                            GameObject newSAObj = null;

                            saObj = (GameObject)Resources.Load("_Extensions/Prefabs/Player Creator/Camera/Sub Actions");
                            newSAObj = Instantiate(saObj);
                            newSAObj.name = "Sub Actions";

                            newSAObj.transform.parent = playerItems[0].transform.parent;
                            newSAObj.transform.localPosition = new Vector3(0, 0, 0);
                            newSAObj.transform.localEulerAngles = new Vector3(0, 0, 0);

                            HFPS_SubActionsHandler tempSubActs = newSAObj.GetComponent<HFPS_SubActionsHandler>();

                            if(addOrUpdateOptions.addReferences){

                                tempSubActs.refs = tempReferences;
                                tempReferences.subActionsHandler = tempSubActs;

                            }//addReferences

                            tempSubActs.useActionDelay = subActionsOptions.useActionDelay;
                            tempSubActs.actionDelay = subActionsOptions.actionDelay;

                            tempSubActs.inputType = (HFPS_SubActionsHandler.Input_Type)subActionsOptions.inputType;
                            tempSubActs.holdTime = subActionsOptions.holdTime;
                            tempSubActs.holdMulti = subActionsOptions.holdMulti;

                            if(subActionsOptions.subActions.Count > 0){

                                if(tempSubActs.subActions.Count > 0){

                                    for(int sa = 0; sa < subActionsOptions.subActions.Count; sa++){

                                        if(!SubAct_Check(tempSubActs.subActions, subActionsOptions.subActions[sa].name)){

                                            tempSubActs.subActions.Add(subActionsOptions.subActions[sa]);

                                        }//!sub action not present

                                    }//for sa subActions

                                //subActions.Count > 0
                                } else {

                                    tempSubActs.subActions = new List<HFPS_SubActionsHandler.Sub_Action>();

                                    for(int sa = 0; sa < subActionsOptions.subActions.Count; sa++){

                                        tempSubActs.subActions.Add(subActionsOptions.subActions[sa]);

                                    }//for sa subActions

                                }//subActions.Count > 0

                            }//subActions.Count > 0

                            if(subActionsOptions.actionInputs.Count > 0){

                                if(tempSubActs.actionInputs.Count > 0){

                                    for(int ai = 0; ai < subActionsOptions.actionInputs.Count; ai++){

                                        if(!SubActInput_Check(tempSubActs.actionInputs, subActionsOptions.actionInputs[ai].name)){

                                            tempSubActs.actionInputs.Add(subActionsOptions.actionInputs[ai]);

                                        }//!sub action not present

                                    }//for ai actionInputs

                                //actionInputs.Count > 0
                                } else {

                                    tempSubActs.actionInputs = new List<HFPS_SubActionsHandler.Action_Input>();

                                    for(int ai = 0; ai < subActionsOptions.actionInputs.Count; ai++){

                                        tempSubActs.actionInputs.Add(subActionsOptions.actionInputs[ai]);

                                    }//for ai actionInputs

                                }//actionInputs.Count > 0

                            }//actionInputs.Count > 0

                        }//!present

                    }//tempScriptMan != null

                }//addSubActions 


    //////////////////////////////////
    ///
    ///     UPDATE
    ///
    //////////////////////////////////

    /////////////////////////
    ///
    ///     MAIN COMPONENTS
    ///
    /////////////////////////

    //////////////////
    ///
    ///     PRONE
    ///
    //////////////////


                if(addOrUpdateOptions.updateProne){

                    PlayerController tempCont = character.GetComponent<PlayerController>();

                    if(tempCont != null){

                        tempCont.controllerFeatures.enableProne = proneOptions.enableProne;

                        tempCont.basicSettings.proneSpeed = proneOptions.proneSpeed;

                        tempCont.controllerAdjustments.proneCenter = proneOptions.proneCenter;

                    }//tempCont != null

                }//updateProne


    //////////////////
    ///
    ///     ZOOM
    ///
    //////////////////


                if(addOrUpdateOptions.updateZoom){

                    if(tempReferences.playerFunct != null){

                        tempReferences.playerFunct.zoomType = (PlayerFunctions.Zoom_Type)(int)zoomType;

                    //playerFunct != null
                    } else {

                        PlayerFunctions tempPlayFunct = null;

                        foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                            if(child.GetComponent<PlayerFunctions>() != null){

                                tempPlayFunct = child.GetComponent<PlayerFunctions>();

                            }//PlayerFunctions != null

                        }//foreach child

                        if(tempPlayFunct != null){

                            tempPlayFunct.zoomType = (PlayerFunctions.Zoom_Type)(int)zoomType;

                        }//tempPlayFunct != null

                    }//playerFunct != null

                }//updateZoom

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[9].local);

            }//character != null

            #else

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local);

            #endif

        }//Player_Update

        public void Player_UpdateDefaults(){

            #if COMPONENTS_PRESENT

                playerIsHFPS = false;
                playerChecked = false;

                updateOptions.featOpts = false;
                updateOptions.playOpts = false;
                updateOptions.proneOpts = false;
                updateOptions.zoomOpts = false;

                updateOptions.audioFadeOpts = false;
                updateOptions.dualWieldOpts = false;
                updateOptions.fovOpts = false;
                updateOptions.matOpts = false;
                updateOptions.playManOpts = false;
                updateOptions.screenEventsOpts = false;
                updateOptions.subActsOpts = false;

                PlayerUpdate_AddState(false);
                FeaturesUpdate_AllState(false);

                playerItems = new List<GameObject>();
                dualWieldItems = new List<DualWield_Item>();
                audioFaderOptions = new AudioFader_Options();

                fovOptions = new FOV_Options();
                fovAuto = new FOV_Auto();

                matContOptions = new MaterialCont_Options();
                playerManOptions = new PlayerMan_Options();
                playerManAuto = new PlayerMan_Auto();

                proneOptions = new Prone_Options();
                proneAuto = new Prone_Auto();

                subActionsOptions = new SubActions_Options();
                subActionsAuto = new SubActions_Auto();

                zoomType = PlayerFunctions.Zoom_Type.NoZoom;

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[12].local);

            #else

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local);

            #endif

        }//Player_UpdateDefaults


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    //////////////////////////////////////

    //////////////////////////
    ///
    ///     TEMPLATE SAVE
    ///
    //////////////////////////


        public void Template_Save(){

            #if COMPONENTS_PRESENT

            if(template != null){


    ///////////////////
    ///
    ///     CREATE
    ///
    ///////////////////


                if(useType == Use_Type.Create){

                    if(template.templateType != DM_PlayerCreator_Template.Template_Type.PlayerNew){

                        template.templateType = DM_PlayerCreator_Template.Template_Type.PlayerNew;

                    }//templateType != player new

                }//useType = create


    ///////////////////
    ///
    ///     UPDATE
    ///
    ///////////////////


                if(useType == Use_Type.Update){

                    if(template.templateType != DM_PlayerCreator_Template.Template_Type.PlayerUpdate){

                        template.templateType = DM_PlayerCreator_Template.Template_Type.PlayerUpdate;

                    }//templateType != player update


    /////////////
    ///
    ///     ADD OR UPDATE VALUES
    ///
    /////////////


                    template.addOrUpdateOptions.addAudioFader = addOrUpdateOptions.addAudioFader;
                    template.addOrUpdateOptions.addDualWield = addOrUpdateOptions.addDualWield;
                    template.addOrUpdateOptions.addFOVMan = addOrUpdateOptions.addFOVMan;
                    template.addOrUpdateOptions.addMaterialCont = addOrUpdateOptions.addMaterialCont;
                    template.addOrUpdateOptions.addPlayerMan = addOrUpdateOptions.addPlayerMan;
                    template.addOrUpdateOptions.addReferences = addOrUpdateOptions.addReferences;
                    template.addOrUpdateOptions.addScreenEvents = addOrUpdateOptions.addScreenEvents;
                    template.addOrUpdateOptions.addSubActions = addOrUpdateOptions.addSubActions;
                    template.addOrUpdateOptions.updateProne = addOrUpdateOptions.updateProne;
                    template.addOrUpdateOptions.updateZoom = addOrUpdateOptions.updateZoom;


    /////////////
    ///
    ///     AUDIO FADER VALUES
    ///
    /////////////


                    template.audioFaderOptions.fadeMulti = audioFaderOptions.fadeMulti;

                    if(audioFaderOptions.sources.ambience != null){

                        audioFaderOptions.sources.ambName = audioFaderOptions.sources.ambience.name;
                        template.audioFaderOptions.sources.ambName = audioFaderOptions.sources.ambience.name;

                    }//ambience != null

                    if(audioFaderOptions.sources.music != null){

                        audioFaderOptions.sources.musicName = audioFaderOptions.sources.music.name;
                        template.audioFaderOptions.sources.musicName = audioFaderOptions.sources.music.name;

                    }//music != null


    /////////////
    ///
    ///     DUAL WIELD VALUES
    ///
    /////////////

                    template.dualWieldItems = new List<DualWield_Item>();

                    if(dualWieldItems.Count > 0){

                        for(int i = 0; i < dualWieldItems.Count; i++){

                            template.dualWieldItems.Add(dualWieldItems[i]);

                        }//for i dualWieldItems

                    }//dualWieldItems.Count > 0


    /////////////
    ///
    ///     FOV VALUES
    ///
    /////////////


                    template.fovOptions.globalZoomMulti = fovOptions.globalZoomMulti;
                    template.fovOptions.globalUnZoomMulti = fovOptions.globalUnZoomMulti;
                    template.fovOptions.autoFindCameras = fovOptions.autoFindCameras;

                    template.fovOptions.cameras = new List<Camera>();
                    template.fovOptions.states = new List<HFPS_FOVManager.Camera_State>();

                    if(fovOptions.states.Count > 0){

                        for(int i = 0; i < fovOptions.states.Count; i++){

                            template.fovOptions.states.Add(fovOptions.states[i]);

                        }//for i states

                    }//states.Count > 0


    /////////////
    ///
    ///     MATERIAL CONT VALUES
    ///
    /////////////


                    template.matContOptions.bodyParts = new List<HFPS_MaterialCont.BodyParts>();

                    if(matContOptions.bodyParts.Count > 0){

                        for(int i = 0; i < matContOptions.bodyParts.Count; i++){

                            template.matContOptions.bodyParts.Add(matContOptions.bodyParts[i]);

                        }//for i bodyParts

                        if(template.matContOptions.bodyParts.Count > 0){

                            for(int b = 0; b < template.matContOptions.bodyParts.Count; b++){

                                for(int bm = 0; bm < template.matContOptions.bodyParts[b].meshParts.Count; bm++){

                                    if(matContOptions.bodyParts[b].meshParts[bm].skinnedMesh != null){

                                        matContOptions.bodyParts[b].meshParts[bm].nameOfMesh = matContOptions.bodyParts[b].meshParts[bm].skinnedMesh.name;
                                        template.matContOptions.bodyParts[b].meshParts[bm].nameOfMesh = matContOptions.bodyParts[b].meshParts[bm].skinnedMesh.name;

                                    }//skinnedMesh != null

                                    template.matContOptions.bodyParts[b].meshParts[bm].skinnedMesh = null;

                                }//for bm meshParts

                            }//for b bodyParts

                        }//bodyParts.Count > 0

                    }//bodyParts.Count > 0


    /////////////
    ///
    ///     PLAYER MANAGER VALUES
    ///
    /////////////


                    template.playerManOptions.slowDownUse = (HFPS_PlayerMan.SlowDown)playerManOptions.slowDownUse;

                    template.playerManOptions.meleeConts = new List<HFPS_PlayerMan.Melee_Cont>();
                    template.playerManOptions.weaponConts = new List<HFPS_PlayerMan.Weapon_Cont>();

                    if(playerManOptions.meleeConts.Count > 0){

                        for(int i = 0; i < playerManOptions.meleeConts.Count; i++){

                            template.playerManOptions.meleeConts.Add(playerManOptions.meleeConts[i]);

                        }//for i meleeConts

                        if(template.playerManOptions.meleeConts.Count > 0){

                            for(int m = 0; m < template.playerManOptions.meleeConts.Count; m++){

                                template.playerManOptions.meleeConts[m].meleeCont = null;

                            }//for m meleeConts

                        }//meleeConts.Count > 0

                    }//meleeConts.Count > 0

                    if(playerManOptions.weaponConts.Count > 0){

                        for(int i = 0; i < playerManOptions.weaponConts.Count; i++){

                            template.playerManOptions.weaponConts.Add(playerManOptions.weaponConts[i]);

                        }//for i weaponConts

                        if(template.playerManOptions.weaponConts.Count > 0){

                            for(int w = 0; w < template.playerManOptions.weaponConts.Count; w++){

                                template.playerManOptions.weaponConts[w].weaponCont = null;

                            }//for w weaponConts

                        }//weaponConts.Count > 0

                    }//weaponConts.Count > 0


    /////////////
    ///
    ///     PRONE VALUES
    ///
    /////////////


                    template.proneOptions.enableProne = proneOptions.enableProne;
                    template.proneOptions.proneSpeed = proneOptions.proneSpeed;
                    template.proneOptions.proneCenter = proneOptions.proneCenter;


    /////////////
    ///
    ///     SUB ACTIONS VALUES
    ///
    /////////////


                    template.subActionsOptions.subActions = new List<HFPS_SubActionsHandler.Sub_Action>();
                    template.subActionsOptions.actionInputs = new List<HFPS_SubActionsHandler.Action_Input>();

                    if(subActionsOptions.subActions.Count > 0){

                        for(int i = 0; i < subActionsOptions.subActions.Count; i++){

                            template.subActionsOptions.subActions.Add(subActionsOptions.subActions[i]);

                        }//for i subActions

                        if(template.subActionsOptions.subActions.Count > 0){

                            for(int s = 0; s < template.subActionsOptions.subActions.Count; s++){

                                if(subActionsOptions.subActions[s].action != null){

                                    subActionsOptions.subActions[s].actName = subActionsOptions.subActions[s].action.name;

                                }//action != null

                                template.subActionsOptions.subActions[s].action = null;

                            }//for s subActions

                        }//subActions.Count > 0

                    }//subActions.Count > 0   

                    if(subActionsOptions.actionInputs.Count > 0){

                        for(int i = 0; i < subActionsOptions.actionInputs.Count; i++){

                            template.subActionsOptions.actionInputs.Add(subActionsOptions.actionInputs[i]);

                        }//for i actionInputs

                    }//actionInputs.Count > 0   

                    template.subActionsOptions.useActionDelay = subActionsOptions.useActionDelay;
                    template.subActionsOptions.actionDelay = subActionsOptions.actionDelay;

                    template.subActionsOptions.inputType = (HFPS_SubActionsHandler.Input_Type)subActionsOptions.inputType;

                    template.subActionsOptions.holdTime = subActionsOptions.holdTime;
                    template.subActionsOptions.holdMulti = subActionsOptions.holdMulti;


    /////////////
    ///
    ///     ZOOM VALUES
    ///
    /////////////


                    template.zoomType = (PlayerFunctions.Zoom_Type)zoomType;


    /////////////
    ///
    ///     DONE
    ///
    /////////////


                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[10].local);


                }//useType = update

            }//template != null

            #else

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local);

            #endif

        }//Template_Save


    //////////////////////////
    ///
    ///     TEMPLATE LOAD
    ///
    //////////////////////////


        public void Template_Load(){

            #if COMPONENTS_PRESENT

            if(template != null){

                if(useType == Use_Type.Update){


    /////////////
    ///
    ///     ADD OR UPDATE VALUES
    ///
    /////////////


                    addOrUpdateOptions.addAudioFader = template.addOrUpdateOptions.addAudioFader;
                    addOrUpdateOptions.addDualWield = template.addOrUpdateOptions.addDualWield;
                    addOrUpdateOptions.addFOVMan = template.addOrUpdateOptions.addFOVMan;
                    addOrUpdateOptions.addMaterialCont = template.addOrUpdateOptions.addMaterialCont;
                    addOrUpdateOptions.addPlayerMan = template.addOrUpdateOptions.addPlayerMan;
                    addOrUpdateOptions.addReferences = template.addOrUpdateOptions.addReferences;
                    addOrUpdateOptions.addScreenEvents = template.addOrUpdateOptions.addScreenEvents;
                    addOrUpdateOptions.addSubActions = template.addOrUpdateOptions.addSubActions;
                    addOrUpdateOptions.updateProne = template.addOrUpdateOptions.updateProne;
                    addOrUpdateOptions.updateZoom = template.addOrUpdateOptions.updateZoom;


    /////////////
    ///
    ///     AUDIO FADER VALUES
    ///
    /////////////


                    audioFaderOptions.fadeMulti = template.audioFaderOptions.fadeMulti;

                    if(template.audioFaderOptions.sources.ambName != ""){

                        GameObject tempAmb = GameObject.Find(template.audioFaderOptions.sources.ambName);

                        if(tempAmb != null){

                            audioFaderOptions.sources.ambience = tempAmb.GetComponent<AudioSource>();

                        }//tempAmb != null

                    }//ambName != null

                    if(template.audioFaderOptions.sources.musicName != ""){

                        GameObject tempMusic = GameObject.Find(template.audioFaderOptions.sources.musicName);

                        if(tempMusic != null){

                            audioFaderOptions.sources.music = tempMusic.GetComponent<AudioSource>();

                        }//tempMusic != null

                    }//music != null


    /////////////
    ///
    ///     DUAL WIELD VALUES
    ///
    /////////////


                    dualWieldItems = new List<DualWield_Item>();

                    if(template.dualWieldItems.Count > 0){

                        for(int i = 0; i < template.dualWieldItems.Count; i++){

                            dualWieldItems.Add(template.dualWieldItems[i]);

                        }//for i dualWieldItems

                    }//dualWieldItems.Count > 0


    /////////////
    ///
    ///     FOV VALUES
    ///
    /////////////


                    fovOptions.globalZoomMulti = template.fovOptions.globalZoomMulti;
                    fovOptions.globalUnZoomMulti = template.fovOptions.globalUnZoomMulti;
                    fovOptions.autoFindCameras = template.fovOptions.autoFindCameras;

                    fovOptions.states = new List<HFPS_FOVManager.Camera_State>();

                    if(template.fovOptions.states.Count > 0){

                        for(int i = 0; i < template.fovOptions.states.Count; i++){

                            fovOptions.states.Add(template.fovOptions.states[i]);

                        }//for i states

                    }//states.Count > 0

                    if(fovOptions.autoFindCameras){

                        fovOptions.cameras = new List<Camera>();

                        ScriptManager tempScriptMan = null;

                        foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                            if(child.GetComponent<ScriptManager>() != null){

                                tempScriptMan = child.GetComponent<ScriptManager>();

                            }//ScriptManager != null

                        }//foreach child

                        if(tempScriptMan != null){

                            fovOptions.cameras.Add(tempScriptMan.MainCamera);
                            fovOptions.cameras.Add(tempScriptMan.ArmsCamera);

                        }//tempScriptMan != null

                    }//autoFindCameras


    /////////////
    ///
    ///     MATERIAL CONT VALUES
    ///
    /////////////


                    matContOptions.bodyParts = new List<HFPS_MaterialCont.BodyParts>();

                    if(template.matContOptions.bodyParts.Count > 0){

                        for(int i = 0; i < template.matContOptions.bodyParts.Count; i++){

                            matContOptions.bodyParts.Add(template.matContOptions.bodyParts[i]);

                        }//for i bodyParts

                        if(matContOptions.bodyParts.Count > 0){

                            List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();

                            foreach(Transform child in character.transform.GetComponentsInChildren<Transform>(true)){

                                if(child.GetComponent<SkinnedMeshRenderer>() != null){

                                    skinnedMeshes.Add(child.GetComponent<SkinnedMeshRenderer>());

                                }//SkinnedMeshRenderer != null

                            }//foreach child

                            if(skinnedMeshes.Count > 0){

                                foreach(var meshes in skinnedMeshes){

                                    for(int tbp = 0; tbp < matContOptions.bodyParts.Count; tbp++){

                                        if(matContOptions.bodyParts[tbp].meshParts.Count > 0){

                                            for(int tmp = 0; tmp < matContOptions.bodyParts[tbp].meshParts.Count; tmp++){

                                                if(matContOptions.bodyParts[tbp].meshParts[tmp].nameOfMesh != ""){

                                                    if(meshes.name == matContOptions.bodyParts[tbp].meshParts[tmp].nameOfMesh){

                                                        matContOptions.bodyParts[tbp].meshParts[tmp].skinnedMesh = meshes;

                                                    }//name = nameOfMesh

                                                }//nameOfMesh != null

                                            }//for tmp meshParts

                                        }//meshParts.Count > 0

                                    }//for tbp bodyParts

                                }//foreach meshes

                            }//skinnedMeshes.Length > 0

                        }//bodyParts.Count > 0

                    }//bodyParts.Count > 0


    /////////////
    ///
    ///     PLAYER MANAGER VALUES
    ///
    /////////////


                    playerManOptions.slowDownUse = (HFPS_PlayerMan.SlowDown)template.playerManOptions.slowDownUse;

                    playerManOptions.meleeConts = new List<HFPS_PlayerMan.Melee_Cont>();
                    playerManOptions.weaponConts = new List<HFPS_PlayerMan.Weapon_Cont>();

                    if(template.playerManOptions.meleeConts.Count > 0){

                        for(int i = 0; i < template.playerManOptions.meleeConts.Count; i++){

                            playerManOptions.meleeConts.Add(template.playerManOptions.meleeConts[i]);

                        }//for i meleeConts

                        if(playerManOptions.meleeConts.Count > 0){

                            if(playerItems.Count > 0){

                                for(int pli = 0; pli < playerItems.Count; pli++){

                                    for(int tmc = 0; tmc < playerManOptions.meleeConts.Count; tmc++){

                                        if(playerItems[pli].name == playerManOptions.meleeConts[tmc].name){

                                            playerManOptions.meleeConts[tmc].meleeCont = playerItems[pli].GetComponent<MeleeController>();

                                        }//name = name

                                    }//for tmc meleeConts

                                }//for pli playerItems

                            }//playerItems.Count > 0

                        }//meleeConts.Count > 0

                    }//meleeConts.Count > 0

                    if(template.playerManOptions.weaponConts.Count > 0){

                        for(int i = 0; i < template.playerManOptions.weaponConts.Count; i++){

                            playerManOptions.weaponConts.Add(template.playerManOptions.weaponConts[i]);

                        }//for i weaponConts

                        if(playerManOptions.weaponConts.Count > 0){

                            if(playerItems.Count > 0){

                                for(int pli = 0; pli < playerItems.Count; pli++){

                                    for(int tmw = 0; tmw < playerManOptions.weaponConts.Count; tmw++){

                                        if(playerItems[pli].name == playerManOptions.weaponConts[tmw].name){

                                            playerManOptions.weaponConts[tmw].weaponCont = playerItems[pli].GetComponent<WeaponController>();

                                        }//name = name

                                    }//for tmw weaponConts

                                }//for pli playerItems

                            }//playerItems.Count > 0

                        }//weaponConts.Count > 0

                    }//weaponConts.Count > 0


    /////////////
    ///
    ///     PRONE VALUES
    ///
    /////////////


                    proneOptions.enableProne = template.proneOptions.enableProne;
                    proneOptions.proneSpeed = template.proneOptions.proneSpeed;
                    proneOptions.proneCenter = template.proneOptions.proneCenter;


    /////////////
    ///
    ///     SUB ACTIONS VALUES
    ///
    /////////////


                    subActionsOptions.subActions = new List<HFPS_SubActionsHandler.Sub_Action>();
                    subActionsOptions.actionInputs = new List<HFPS_SubActionsHandler.Action_Input>();

                    if(template.subActionsOptions.subActions.Count > 0){

                        for(int i = 0; i < template.subActionsOptions.subActions.Count; i++){

                            subActionsOptions.subActions.Add(template.subActionsOptions.subActions[i]);

                        }//for i subActions

                        if(subActionsOptions.subActions.Count > 0){

                            List<HFPS_SubAction> tempSubActs = new List<HFPS_SubAction>();

                            foreach(Transform child in character.transform.GetComponentsInChildren<Transform>(true)){

                                if(child.GetComponent<HFPS_SubAction>() != null){

                                    tempSubActs.Add(child.GetComponent<HFPS_SubAction>());

                                }//HFPS_SubAction != null

                            }//foreach child

                            if(tempSubActs.Count > 0){

                                foreach(var subAct in tempSubActs){

                                    for(int s = 0; s < subActionsOptions.subActions.Count; s++){

                                        if(subActionsOptions.subActions[s].actName != ""){

                                            if(subAct.name == subActionsOptions.subActions[s].actName){

                                                subActionsOptions.subActions[s].action = subAct;

                                            }//name = actName

                                        }//actName != null

                                    }//for s subActions

                                }//foreach subAct

                            }//tempSubActs.Count > 0

                        }//subActions.Count > 0

                    }//subActions.Count > 0   

                    if(template.subActionsOptions.actionInputs.Count > 0){

                        for(int i = 0; i < template.subActionsOptions.actionInputs.Count; i++){

                            subActionsOptions.actionInputs.Add(template.subActionsOptions.actionInputs[i]);

                        }//for i actionInputs

                    }//actionInputs.Count > 0   

                    subActionsOptions.useActionDelay = template.subActionsOptions.useActionDelay;
                    subActionsOptions.actionDelay = template.subActionsOptions.actionDelay;

                    subActionsOptions.inputType = (HFPS_SubActionsHandler.Input_Type)template.subActionsOptions.inputType;

                    subActionsOptions.holdTime = template.subActionsOptions.holdTime;
                    subActionsOptions.holdMulti = template.subActionsOptions.holdMulti;


    /////////////
    ///
    ///     ZOOM VALUES
    ///
    /////////////


                    zoomType = (PlayerFunctions.Zoom_Type)template.zoomType;


    /////////////
    ///
    ///     DONE
    ///
    /////////////


                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[11].local);


                }//useType = update

            }//template != null

            #else

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[13].local);

            #endif

        }//Template_Load


    //////////////////////////
    ///
    ///     RESET
    ///
    //////////////////////////


        public void Ragdoll_Reset(){

            if(ragdollOptions.charAnim != null){

                ragdollOptions.charAnim = null;

            }//ragdollOptions.charAnim != null

            if(ragdollOptions.animChecked){

                ragdollOptions.animChecked = false;

            }//ragdollOptions.animChecked

            if(ragdollOptions.pelvis != null){

                ragdollOptions.pelvis = null;

            }//ragdollOptions.pelvis != null

            if(ragdollOptions.tempCollider != null){

                ragdollOptions.tempCollider = null;

            }//ragdollOptions.tempCollider != null

        }//Ragdoll_Reset


    //////////////////////////
    ///
    ///     STATES
    ///
    //////////////////////////


        private void PlayerUpdate_AddState(bool state){

            addOrUpdateOptions.updateProne = state;
            addOrUpdateOptions.updateZoom = state;

        }//PlayerUpdate_AddState

        private void FeaturesUpdate_AllState(bool state){

            addOrUpdateOptions.addAudioFader = state;
            addOrUpdateOptions.addDualWield = state;
            addOrUpdateOptions.addFOVMan = state;
            addOrUpdateOptions.addMaterialCont = state;
            addOrUpdateOptions.addPlayerMan = state;
            addOrUpdateOptions.addReferences = state;
            addOrUpdateOptions.addScreenEvents = state;
            addOrUpdateOptions.addSubActions = state;

        }//FeaturesUpdate_AllState


    //////////////////////////////////////
    ///
    ///     CHECK ACTIONS
    ///
    //////////////////////////////////////


        public int Options_Check(List<GameObject> items, string option){

            for(int i = 0; i < items.Count; i++) {

                if(items[i].name == option){

                    return i;

                }//name = option

            }//for i items

            return -1;

        }//Options_Check


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

                    if(dmMenusLocData.dictionary[d].asset == "Player Creator"){

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

                                window = GetWindow<HFPS_PlayerCreator>(false, "Player Creator" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Player Creator Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<HFPS_PlayerCreator>(false, "Player Creator " + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Player Creator Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Player Creator Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<HFPS_PlayerCreator>(false, "Player Creator " + verNumb, true);
                    window.maxSize = window.minSize = windowsSize;

                }//results.Length > 0

            }//!versionCheckStatic

        }//Version_FindStatic



    //////////////////////////////////////
    ///
    ///     INTERNAL HELPERS
    ///
    ///////////////////////////////////////


        #if COMPONENTS_PRESENT

            public bool SubAct_Check(List<HFPS_SubActionsHandler.Sub_Action> subActs, string name){

                bool found = false;

                for(int sa = 0; sa < subActs.Count; sa++){

                    if(!found){

                        if(subActs[sa].name == name){

                            found = true;

                        }//name = name

                    }//!found

                }//for sa subActs

                return found;

            }//SubAct_Check

            public bool SubActInput_Check(List<HFPS_SubActionsHandler.Action_Input> subActsInputs, string name){

                bool found = false;

                for(int sai = 0; sai < subActsInputs.Count; sai++){

                    if(!found){

                        if(subActsInputs[sai].name == name){

                            found = true;

                        }//name = name

                    }//!found

                }//for sai subActsInputs

                return found;

            }//SubActInput_Check

        #endif


    //////////////////////////////////////
    ///
    ///     EXTRAS
    ///
    ///////////////////////////////////////


       public AnimatorController Animator_Find(string fileName){

            string[] results;
            AnimatorController tempAnim = new AnimatorController();

            results = AssetDatabase.FindAssets(fileName);

            if(results.Length > 0){

                foreach(string guid in results){

                    if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                        tempAnim = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(guid));

                        if(tempAnim != null){

                            //Debug.Log(tempAnim.name + " Found!");

                            return tempAnim;

                        }//tempAnim != null

                    //Exists
                    } else {

                        //Debug.Log(fileName + " NOT Found!");

                        return null;

                    }//Exists

                }//foreach guid

            //results.Length > 0
            } else {

                //Debug.Log(fileName + " NOT Found!");

                return null;

            }//results.Length > 0

            return null;

        }//Animator_Find

        private void OnDestroy() {

            window = null;
            verNumb = "";

        }//OnDestroy


    }//HFPS_PlayerCreator
    
    
}//namespace

#endif
