using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

#if COMPONENTS_PRESENT

    using DizzyMedia.HFPS_Components;

#endif

#if HFPS_DURABILITY_PRESENT

    using DizzyMedia.HFPS_Durability;

#endif

#if PUZZLER_PRESENT

    using DizzyMedia.HFPS_Puzzler;

#endif

#if HFPS_SHOOTRANGE_PRESENT

    using DizzyMedia.HFPS_ShootingRange;

#endif

#if HFPS_VENDOR_PRESENT

    using DizzyMedia.HFPS_Vendor;

#endif

using Newtonsoft.Json.Linq;
using HFPS.Player;
using HFPS.Systems;
using ThunderWire.Input;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/World/Player/Character Action")]
    public class HFPS_CharacterAction : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    //////////////////////////////////////


        [Serializable]
        public class UserOptions {

            public ActionType actionType;
            public ActionAttribute actionAttribute;
            public UseType useType;
            public EndType endType;

            public LockSettings lookLock;
            public LockSettings moveLock;
            public ItemDisplay itemDisplay;

            public bool adjustCharCont;
            public float radius;

            public bool adjustMoveSpeed;
            public float moveSpeed;

            public float interactObjWait;
            
            public bool delayActionsEnd;
            public float actionEndWait;

            #if PUZZLER_PRESENT

                public bool linkComplete;
                public Puzzler_Handler handler;

            #endif

        }//UserOptions

        [Serializable]
        public class StartOptions {

            public StartType startType;
            public StartEndType startEndType;
            public UseType startUse;

            public bool useStartDelay;
            public float startDelay;
            public float startEndWait;

            public PlayerState startState;
            public Transform startPosition;

        }//StartOptions

        [Serializable]
        public class LookOptions {

            [Space]

            public bool updateLookCaps;

            [Space]

            public bool updateLookX;

            public float minX;
            public float maxX;

            [Space]

            public bool updateLookY;

            public float minY;
            public float maxY;

        }//LookOptions

        [Serializable]
        public class PlayerOptions {

            public bool showMouse;

            public Input_Lock xInput;
            public Input_Limit xLimit;
            public Input_LimitHoriz xLimitDir;

            public Input_Lock yInput;
            public Input_Limit yLimit;
            public Input_LimitVert yLimitDir;

            public Input_Lock jumpInput;
            public Input_Lock stateInput;

            public Input_Lock leanInput;
            public LockSettings leanLock;

            public Input_Lock sprintInput;
            public LockSettings sprintLock;

            public Input_Lock zoomInput;
            public LockSettings zoomLock;

        }//PlayerOptions

        [Serializable]
        public class ItemDisplay {

            public Input_Lock itemSwitchLock;
            public Input_Lock itemUseLock;
            public Input_Lock itemZoomLock;

            public ItemDisplayState itemDispEnter;
            public ItemDisplayState itemDispExit;

        }//ItemDisplay

        [Serializable]
        public class HFPS_UI {
        
            public bool useDisplaySet;
            public string displaySet;

            public Action_State gameUIStateStart;
            public Action_State gameUIStateEnd;

            public Action_State pauseStateStart;
            public Action_State pauseStateEnd;

            public Action_State inventoryStateStart;
            public Action_State inventoryStateEnd;

            public Action_State saveStateStart;
            public Action_State saveStateEnd;

            public Action_State loadStateStart;
            public Action_State loadStateEnd;

        }//HFPS_UI

        [Serializable]
        public class AudioOptions {

            public AudioType audioType;
            public bool keepAmbience;

            public AudioClip clip;

            public bool customVolume;
            public float audioVolume;

            public bool immediate;

        }//AudioOptions

        [Serializable]
        public class Extensions {

            [Space]

            public ActionBarSettings actionBarSettings;
            
            #if HFPS_DURABILITY_PRESENT
            
                [Space]

                public BenchSettings benchSettings;

            #endif

            #if PUZZLER_PRESENT
            
                [Space]

                public CameraContSettings cameraContSettings;

                [Space]

                public ItemViewer_Settings itemViewerSettings;
                
                [Space]
                
                public Puzzle_Settings puzzleSettings;

            #endif
            
            #if HFPS_SHOOTRANGE_PRESENT
            
                [Space]

                public ShootRange_Settings shootRangeSettings;

            #endif
            
            #if COMPONENTS_PRESENT
            
                [Space]

                public SubActionsSettings subActionSettings;
            
            #endif
            
            #if HFPS_VENDOR_PRESENT

                [Space]

                public VendorSettings vendorSettings;

            #endif

        }//Extensions

        [Serializable]
        public class ActionBarSettings {

            [Space]

            public bool useActionBar;
            
            [Space]
            
            public bool useCustomActionBar;
            public DM_ActionBar actionBar;

            [Space]

            public List<DM_ActionBar.Action_Custom> actions;

        }//ActionBarSettings
        
        [Serializable]
        public class BenchSettings {

            [Space]

            public bool useBench;
            
            #if HFPS_DURABILITY_PRESENT
            
                public HFPS_Bench bench;

            #endif
            
        }//BenchSettings

        [Serializable]
        public class CameraContSettings {

            [Space]

            public bool useCameraCont;

            [Space]

            public bool useDefault;
            public string customMoveType;

        }//CameraContSettings

        [Serializable]
        public class ItemViewer_Settings {

            [Space]

            public bool useItemViewer;
            public bool useHideDelayOnLook;

            [Space]

            [InventorySelector]
            public int itemID;

        }//ItemViewer_Settings
        
        [Serializable]
        public class Puzzle_Settings {

            [Space]

            public bool usePuzzle;

            #if PUZZLER_PRESENT
            
                [Space]
            
                public ActionType_Puzzle actionType;
                public Puzzler_Handler puzzle;
            
            #endif

        }//Puzzle_Settings
        
        [Serializable]
        public class ShootRange_Settings {

            [Space]

            public bool useRange;
            
            #if HFPS_SHOOTRANGE_PRESENT
            
                public DM_ShootRange shootRange;

            #endif
            
        }//ShootRange_Settings

        [Serializable]
        public class SubActionsSettings {

            [Space]

            public bool useSubActions;

            [Space]

            public List<SubAction_Types> actionTypes;

        }//SubActionsSettings

        [Serializable]
        public class SubAction_Types {

            [Space]

            public string name;
            
            #if COMPONENTS_PRESENT
            
                public DM_InternEnumsComp.SubAction_Type type;
            
            #endif
            
            public int inputSlot;

        }//SubAction_Types
        
        [Serializable]
        public class VendorSettings {

            [Space]
            
            public bool currencySave;
            
            [Space]

            public bool useVendor;
            
            #if HFPS_VENDOR_PRESENT
            
                public HFPS_VendorHandler vendor;

            #endif
            
        }//VendorSettings

        [Serializable]
        public class References {

            public GameObject interactObj;
            public Collider interactCol;
            public DynamicObject dynamObj;
            public Transform actionParent;
            public GameObject endTrigger;

            public GameObject interactFront;
            public GameObject interactBack;

            public GameObject endTrigger_Front;
            public GameObject endTrigger_Back;

            public GameObject exitBlock_Front;
            public GameObject exitBlock_Back;

        }//References

        [Serializable]
        public class Action_Point {

            [Space]

            public string name;
            public MoveType moveType;
            public PlayerState stateChange;

            [Space]

            public float speed;

            [Space]

            public Transform transform;
            public Vector3 position;
            public Vector2 rotation;

            [Space]

            public float nextWait;

            [Space]

            public UnityEvent actionEvent;

        }//Action_Point

        [Serializable]
        public class StartActionEvents {

            [Space]

            public UnityEvent startActionStart;
            public UnityEvent startActionFinish;

            [Space]

            public bool actionFinishDelay;
            public float actionFinishWait;
            public UnityEvent startActionFinishDelayed;

        }//StartActionEvents

        [Serializable]
        public class ActionEvents {

            [Space]

            public UnityEvent actionStart;
            public UnityEvent actionFinish;

        }//ActionEvents

        [Serializable]
        public class ActionStopEvents {

            [Space]

            public UnityEvent actionStopStart;
            public UnityEvent actionStopFinish;

        }//ActionStopEvents

        [Serializable]
        public class Auto {

            public HFPS_GameManager gameManager;
            public HFPS_References refs;
            public HFPS_UICont uiCont;
            
            #if COMPONENTS_PRESENT
        
                public HFPS_SubActionsUI subActsUI;
            
            #endif
            
            public EnterDirection enterDirection;

            public float oldRadius;
            public int currentItem;

            public float minXOld;
            public float maxXOld;
            public float minYOld;
            public float maxYOld;

            public bool startActionLocked;
            public bool actionActive;
            public bool actionTransferred;
            public bool buffLock;
            public float tempWait;
            public bool locked;

        }//Auto

        [Serializable]
        public class AutoExtensions {

            public DM_ActionBar actionBar;

            #if PUZZLER_PRESENT

                public Puzzler_CameraCont camCont;
                public Puzzler_ItemViewer itemViewer;

            #endif

        }//AutoExtensions


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum StartType {

            Regular = 0,
            AutoStart = 1,

        }//StartType

        public enum EndType {

            Input = 0,
            Trigger = 1,

        }//EndType

        public enum StartEndType {

            Auto = 0,
            Input = 1,
            Manual = 2,

        }//StartEndType

        public enum ActionType {

            General = 0,
            TwoSided = 1,
            EnterObject = 2,
            PushObject = 3,

        }//ActionType
        
        public enum ActionType_Puzzle {
        
            None = 0,
            Activate = 1,
            StateCheck = 2,
            
        }//ActionType_Puzzle

        public enum InteractType {
        
            Disable = 0,
            ReEnable = 1,
        
        }//InteractType

        public enum MoveType {

            Look = 0,
            Move = 1,
            State = 2,

        }//MoveType

        public enum UseType {

            SingleUse = 0,
            MultiUse = 1,

        }//UseType

        public enum ActionAttribute {

            None = 0,
            Heal = 1,
            Hide = 2,
            Teleport = 3,

        }//ActionAttribute

        public enum Teleport_Type {

            Local = 0,
            Level = 1,

        }//Teleport_Type

        public enum Transfer_Type {

            None = 0,
            ActionToLevel = 1,

        }//Transfer_Type

        public enum LockSettings {

            Unlock = 0,
            KeepLocked = 1,

        }//LockSettings

        public enum PlayerState {

            Current = 0,
            Stand = 1,
            Crouch = 2,
            Prone = 3,

        }//PlayerState

        public enum Input_Lock {

            None = 0,
            Lock = 1,

        }//Input_Lock

        public enum Input_Limit {

            None = 0,
            Limit = 1,

        }//Input_Limit

        public enum Input_LimitVert {

            Up = 0,
            Down = 1,

        }//Input_LimitVert

        public enum Input_LimitHoriz {

            Left = 0,
            Right = 1,

        }//Input_LimitHoriz

        public enum ItemDisplayState {

            Current = 0,
            Hide = 1,
            Show = 2,

        }//ItemDisplayState

        public enum Action_State {

            Nothing = 0,
            Enable = 1, 
            Disable = 2,

        }//Action_State

        public enum Save_State {

            None = 0,
            Active = 1,

        }//Save_State

        public enum Load_State {

            Normal = 0,
            LoadSavedScene = 1,
            LoadPlayerPersistenceData = 2,

        }//Load_State

        public enum AudioType {

            None = 0,
            Ambience = 1,
            Music = 2,

        }//AudioType

        public enum EnterDirection {

            Front = 0,
            Back = 1,

        }//EnterDirection


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     USER OPTIONS
    ///
    ///////////////////////////


        public StartOptions startOptions;
        public UserOptions userOptions;
        public LookOptions lookOptions;
        public PlayerOptions playerOptions;
        public HFPS_UI hfpsUI;
        public AudioOptions audioOptions;
        public Extensions extensions;
        public References references;

        public List<Action_Point> actionsStart;
        public List<Action_Point> actionsEnd;

        public List<Action_Point> actionsFrontStart;
        public List<Action_Point> actionsFrontEnd;

        public List<Action_Point> actionsBackStart;
        public List<Action_Point> actionsBackEnd;

        public InteractType interactType;
        public Teleport_Type teleportType;
        public Transfer_Type transferType;
        public Save_State saveState;
        public Load_State loadState;
        
        #if COMPONENTS_PRESENT
        
            public HFPS_CharacterAction transferAction;
        
        #endif
        
        public Transform teleportTrans;
        public string transferActionName;
        public string levelName;
        public float teleportWait;


    ///////////////////////////
    ///
    ///     EVENTS
    ///
    ///////////////////////////


        public StartActionEvents startActionEvents;
        public ActionEvents actionEvents;
        public ActionStopEvents actionStopEvents;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public DM_InternEnums.PlayInput_Type inputType;
        public Auto auto;
        public AutoExtensions autoExtensions;

        public int tabs;

        public bool startOpts;
        public bool genOpts;
        public bool interOpts;
        public bool lookOpts;
        public bool moveOpts;
        public bool playOpts;
        public bool itemOpts;
        public bool uiOpts;
        public bool audioOpts;
        public bool extOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            if(userOptions.actionType == ActionType.TwoSided){

                references.endTrigger_Front.SetActive(false);
                references.endTrigger_Back.SetActive(false);

                if(references.exitBlock_Front != null){

                    references.exitBlock_Front.SetActive(false);

                }//exitBlock_Front != null

                if(references.exitBlock_Back != null){

                    references.exitBlock_Back.SetActive(false);

                }//exitBlock_Back != null

            }//actionType = two sided

            EnterDirection_Set(1);

            auto.gameManager = HFPS_GameManager.Instance;
            auto.refs = HFPS_References.instance;
            auto.uiCont = HFPS_UICont.instance;
            
            #if COMPONENTS_PRESENT
            
                auto.subActsUI = HFPS_SubActionsUI.instance;
            
            #endif

            auto.oldRadius = -1;

            auto.minXOld = 0;
            auto.maxXOld = 0;
            auto.minYOld = 0;
            auto.maxYOld = 0;

            auto.currentItem = -1;
            auto.actionActive = false;
            auto.actionTransferred = false;
            auto.buffLock = false;
            auto.tempWait = 0;
            Locked_State(false);

            if(extensions.actionBarSettings.useActionBar){
            
                if(!extensions.actionBarSettings.useCustomActionBar){

                    if(DM_ActionBar.instance != null){

                        autoExtensions.actionBar = DM_ActionBar.instance;

                    }//instance != null
                
                //!useCustomActionBar
                } else {
                
                    autoExtensions.actionBar = extensions.actionBarSettings.actionBar;
                
                }//!useCustomActionBar

            }//useActionBar & !useCustomActionBar

            #if PUZZLER_PRESENT

                if(extensions.cameraContSettings.useCameraCont){

                    if(Puzzler_CameraCont.instance != null){

                        autoExtensions.camCont = Puzzler_CameraCont.instance;

                    }//instance != null

                }//useCameraCont

                if(extensions.itemViewerSettings.useItemViewer){

                    if(Puzzler_ItemViewer.instance != null){

                        autoExtensions.itemViewer = Puzzler_ItemViewer.instance;

                    }//instance != null

                }//useItemViewer

            #endif

            if(startOptions.startType == StartType.AutoStart){

                if(!auto.startActionLocked){

                    if(startOptions.useStartDelay){

                        StartCoroutine("StartAction_Delay");

                    //useStartDelay
                    } else {

                        StartCoroutine("StartAction_Init");

                    }//useStartDelay

                }//!startActionLocked

            }//startType = auto start

        }//StartInit

        private IEnumerator StartAction_Delay(){

            yield return new WaitForSeconds(startOptions.startDelay);

            if(!auto.startActionLocked){

                StartCoroutine("StartAction_Init");

            }//!startActionLocked

        }//StartAction_Delay

        private IEnumerator StartAction_Init(){

            startActionEvents.startActionStart.Invoke();

            auto.actionActive = true;
            auto.buffLock = true;

            CharAction_Set();
            Items_Update(false);
            Interactions_Update(userOptions.actionType, false, false);

            Player_LockState(true);
            Player_State(startOptions.startState);

            Player_Look(new Vector2(startOptions.startPosition.localEulerAngles.y, startOptions.startPosition.localEulerAngles.z), 100);
            Player_Move(startOptions.startPosition.position, 100);
            
            if(hfpsUI.gameUIStateStart == Action_State.Nothing){

                if(hfpsUI.useDisplaySet){
                
                    if(hfpsUI.displaySet != ""){
                    
                        auto.uiCont.DisplaySet_State(false, hfpsUI.displaySet);
                    
                    }//displaySet != null
                
                }//useDisplaySet

            }//gameUIStateStart = nothing
            
            if(!hfpsUI.useDisplaySet){

                if(hfpsUI.gameUIStateStart == Action_State.Enable){

                    auto.gameManager.gamePanels.MainGamePanel.SetActive(true);

                }//gameUIStateStart = enable

                if(hfpsUI.gameUIStateStart == Action_State.Disable){

                    auto.gameManager.gamePanels.MainGamePanel.SetActive(false);

                }//gameUIStateStart = disable
            
            }//!useDisplaySet

            if(hfpsUI.pauseStateStart == Action_State.Enable){

                auto.uiCont.PauseLock_State(false);

            }//pauseStateStart = enable

            if(hfpsUI.pauseStateStart == Action_State.Disable){

                auto.uiCont.PauseLock_State(true);

            }//pauseStateStart = disable

            if(hfpsUI.inventoryStateStart == Action_State.Enable){

                auto.uiCont.InventoryLock_State(false);

            }//inventoryStateStart = enable

            if(hfpsUI.inventoryStateStart == Action_State.Disable){

                auto.uiCont.InventoryLock_State(true);

            }//inventoryStateStart = disable

            if(userOptions.adjustCharCont){

                auto.oldRadius = auto.refs.characterController.radius;
                auto.refs.characterController.radius = userOptions.radius;

            }//adjustCharCont
            
            #if COMPONENTS_PRESENT
            
                if(auto.refs.playerMan != null){

                    if(playerOptions.sprintInput == Input_Lock.Lock){

                        auto.refs.playerMan.SprintLock_State(true);

                    }//sprintInput = lock

                    if(playerOptions.leanInput == Input_Lock.Lock){

                        auto.refs.playerMan.LeanLock_State(true);

                    }//leanInput = lock

                    if(playerOptions.zoomInput == Input_Lock.Lock){

                        auto.refs.playerMan.ZoomLock_State(true);

                    }//zoomInput = lock

                    if(userOptions.itemDisplay.itemUseLock == Input_Lock.Lock){

                        auto.refs.playerMan.WeaponsUseLock_State(true);

                    }//itemUseLock = lock

                    if(userOptions.itemDisplay.itemZoomLock == Input_Lock.Lock){

                        auto.refs.playerMan.WeaponsZoomLock_State(true);

                    }//itemZoomLock = lock
                
                }//playerMan != null
            
            #endif

            if(startOptions.startEndType == StartEndType.Auto){

                MouseLook_Update(false);

                auto.refs.mouseLook.LockLook(true);

                yield return new WaitForSeconds(startOptions.startEndWait);

                auto.buffLock = false;
                Action_Check();

            }//startEndType = auto

            if(startOptions.startEndType == StartEndType.Input){

                if(extensions.actionBarSettings.useActionBar){

                    ActionBar_Update(true);

                }//useActionBar
                
                #if COMPONENTS_PRESENT

                    if(extensions.subActionSettings.useSubActions){

                        SubActions_Update(true);

                    }//useSubActions
                
                #endif

                if(userOptions.lookLock == LockSettings.Unlock){

                    MouseLook_Update(false);

                    auto.refs.mouseLook.LockLook(false);

                }//lookLock = unlock

                yield return new WaitForSeconds(startOptions.startEndWait);

                auto.buffLock = false;

            }//startEndType = Input

            if(startOptions.startEndType == StartEndType.Manual){

                yield return new WaitForSeconds(startOptions.startEndWait);

                auto.buffLock = false;

            }//startEndType = manual

            if(startOptions.startUse == UseType.SingleUse){

                auto.startActionLocked = true;

            }//startUse = single

            startActionEvents.startActionFinish.Invoke();

            if(startActionEvents.actionFinishDelay){

                yield return new WaitForSeconds(startActionEvents.actionFinishWait);

                startActionEvents.startActionFinishDelayed.Invoke();

            }//actionFinishDelay

        }//StartAction_Init


    //////////////////////////////////////
    ///
    ///     ACTION ACTIONS
    ///
    ///////////////////////////////////////

    ////////////////////////////
    ///
    ///     CHECK
    ///
    ////////////////////////////


        public void Action_Check(){

            if(!auto.locked){

                if(!auto.buffLock){

                    if(!auto.actionActive){

                        auto.buffLock = true;

                        StartCoroutine("Action_Start");

                    //!actionActive
                    } else {

                        auto.buffLock = true;

                        StartCoroutine("Action_End");

                    }//!actionActive

                }//!buffLock

            }//!locked

        }//Action_Check


    ////////////////////////////
    ///
    ///     START
    ///
    ////////////////////////////


        public IEnumerator Action_Start(){

            #pragma warning disable
            
            bool pauseLockTemp = false;
            bool inventoryLockTemp = false;
            bool saveStateTemp = true;
            bool loadStateTemp = true;

            bool lockMoveX = false;
            bool lockMoveY = false;
            bool lockJump = false;
            bool lockStateInput = false;
            bool lockLean = false;
            bool lockZoom = false;
            bool lockWeapZoom = false;
            bool lockItemUse = false;
            bool lockItemSwitch = false;
            
            #pragma warning restore

            actionEvents.actionStart.Invoke();

            if(playerOptions.showMouse){

                if(InputHandler.InputIsInitialized) {

                    InputCheck_Type();

                }//InputIsInitialized

            }//showMouse

            CharAction_Set();
            Audio_Update(true);
            Items_Update(false);
            Interactions_Update(userOptions.actionType, false, false);
            
            Player_LockState(true);

            #if COMPONENTS_PRESENT
            
                if(auto.refs.playerMan != null){

                    if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){

                        if(userOptions.actionAttribute == ActionAttribute.Hide){

                            auto.refs.playerMan.Hide_State(true);

                        }//actionAttribute = hide

                    }//actionType = general or enter object
                
               }//playerMan != null

            #endif
            
            if(hfpsUI.gameUIStateStart == Action_State.Nothing){

                if(hfpsUI.useDisplaySet){
                
                    if(hfpsUI.displaySet != ""){
                    
                        auto.uiCont.DisplaySet_State(false, hfpsUI.displaySet);
                    
                    }//displaySet != null
                
                }//useDisplaySet

            }//gameUIStateStart = nothing
            
            if(!hfpsUI.useDisplaySet){

                if(hfpsUI.gameUIStateStart == Action_State.Enable){

                    auto.gameManager.gamePanels.MainGamePanel.SetActive(true);

                }//gameUIStateStart = enable

                if(hfpsUI.gameUIStateStart == Action_State.Disable){

                    auto.gameManager.gamePanels.MainGamePanel.SetActive(false);

                }//gameUIStateStart = disable
            
            }//!useDisplaySet

            if(hfpsUI.pauseStateStart == Action_State.Enable){

                auto.uiCont.PauseLock_State(false);
                pauseLockTemp = false;

            }//pauseStateStart = enable

            if(hfpsUI.pauseStateStart == Action_State.Disable){

                auto.uiCont.PauseLock_State(true);
                pauseLockTemp = true;

            }//pauseStateStart = disable

            if(hfpsUI.inventoryStateStart == Action_State.Enable){

                auto.uiCont.InventoryLock_State(false);
                inventoryLockTemp = false;

            }//inventoryStateStart = enable

            if(hfpsUI.inventoryStateStart == Action_State.Disable){

                auto.uiCont.InventoryLock_State(true);
                inventoryLockTemp = true;

            }//inventoryStateStart = disable

            if(hfpsUI.pauseStateStart != Action_State.Disable){

                if(hfpsUI.saveStateStart == Action_State.Enable){

                    auto.uiCont.Save_State(true);
                    saveStateTemp = true;

                }//saveStateStart = enable

                if(hfpsUI.saveStateStart == Action_State.Disable){

                    auto.uiCont.Save_State(false);
                    saveStateTemp = false;

                }//saveStateStart = disable

                if(hfpsUI.loadStateStart == Action_State.Enable){

                    auto.uiCont.Load_State(true);
                    loadStateTemp = true;

                }//loadStateStart = enable

                if(hfpsUI.loadStateStart == Action_State.Disable){

                    auto.uiCont.Load_State(false);
                    loadStateTemp = false;

                }//loadStateStart = disable

            }//pauseStateStart != disable

            if(userOptions.adjustCharCont){

                auto.oldRadius = auto.refs.characterController.radius;
                auto.refs.characterController.radius = userOptions.radius;

            }//adjustCharCont
            
            #if COMPONENTS_PRESENT
            
                if(auto.refs.playerMan != null){

                    if(userOptions.moveLock == LockSettings.Unlock){

                        if(playerOptions.xInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockMoveX_State(true);
                            lockMoveX = true;

                        //xInput = lock
                        } else {

                            if(playerOptions.xLimit == Input_Limit.Limit){

                                auto.refs.playerMan.LimitMoveX_Set((int)playerOptions.xLimitDir);
                                auto.refs.playerMan.LimitMoveX_State(true);

                            }//xLimit = limit

                        }//xInput = lock

                        if(playerOptions.yInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockMoveY_State(true);
                            lockMoveY = true;

                        //yInput = lock
                        } else {

                            if(playerOptions.yLimit == Input_Limit.Limit){

                                auto.refs.playerMan.LimitMoveY_Set((int)playerOptions.yLimitDir);
                                auto.refs.playerMan.LimitMoveY_State(true);

                            }//yLimit = limit

                        }//yInput = lock

                        if(playerOptions.jumpInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockJump_State(true);
                            lockJump = true;

                        }//jumpInput = lock

                        if(playerOptions.stateInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockStateInput_State(true);
                            lockStateInput = true;

                        }//stateInput = lock

                        if(playerOptions.sprintInput == Input_Lock.Lock){

                            auto.refs.playerMan.SprintLock_State(true);
                            //lockLean = true;

                        }//sprintInput = lock

                    }//moveLock = unlock

                    if(playerOptions.leanInput == Input_Lock.Lock){

                        auto.refs.playerMan.LeanLock_State(true);
                        lockLean = true;

                    }//leanInput = lock

                    if(playerOptions.zoomInput == Input_Lock.Lock){

                        auto.refs.playerMan.ZoomLock_State(true);
                        lockZoom = true;

                    }//zoomInput = lock

                    if(userOptions.itemDisplay.itemUseLock == Input_Lock.Lock){

                        auto.refs.playerMan.WeaponsUseLock_State(true);
                        lockItemUse = true;

                    }//itemUseLock = lock

                    if(userOptions.itemDisplay.itemZoomLock == Input_Lock.Lock){

                        auto.refs.playerMan.WeaponsZoomLock_State(true);
                        lockWeapZoom = true;

                    }//itemZoomLock = lock
                
                }//playerMan != null
            
            #endif

            if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject  | userOptions.actionType == ActionType.PushObject){

                if(actionsStart.Count > 0){

                    for(int a = 0; a < actionsStart.Count; a++) {

                        if(actionsStart[a].stateChange != PlayerState.Current){

                            Player_State(actionsStart[a].stateChange);

                        }//stateChange != current

                        if(actionsStart[a].moveType == MoveType.Look){

                            if(actionsStart[a].transform != null){

                                Player_Look(new Vector2(actionsStart[a].transform.localEulerAngles.y, actionsStart[a].transform.localEulerAngles.z), actionsStart[a].speed);

                            //transform != null
                            } else {

                                Player_Look(new Vector2(actionsStart[a].rotation.x, actionsStart[a].rotation.y), actionsStart[a].speed);

                            }//transform != null

                        }//moveType = look

                        if(actionsStart[a].moveType == MoveType.Move){

                            if(actionsStart[a].transform != null){

                                Player_Move(actionsStart[a].transform.position, actionsStart[a].speed);

                            //transform != null
                            } else {

                                Player_Move(actionsStart[a].position, actionsStart[a].speed);

                            }//transform != null

                        }//moveType = move

                        actionsStart[a].actionEvent.Invoke();

                        yield return new WaitForSeconds(actionsStart[a].nextWait);

                    }//for a actionsStart

                }//actionsStart.Count > 0

            }//actionType = general, enter object or push object

            if(userOptions.actionType == ActionType.TwoSided){

                if(auto.enterDirection == EnterDirection.Front){

                    if(actionsFrontStart.Count > 0){

                        for(int a = 0; a < actionsFrontStart.Count; a++) {

                            if(actionsFrontStart[a].stateChange != PlayerState.Current){

                                Player_State(actionsFrontStart[a].stateChange);

                            }//stateChange != current

                            if(actionsFrontStart[a].moveType == MoveType.Look){

                                if(actionsFrontStart[a].transform != null){

                                    Player_Look(new Vector2(actionsFrontStart[a].transform.localEulerAngles.y, actionsFrontStart[a].transform.localEulerAngles.z), actionsFrontStart[a].speed);

                                //transform != null
                                } else {

                                    Player_Look(new Vector2(actionsFrontStart[a].rotation.x, actionsFrontStart[a].rotation.y), actionsFrontStart[a].speed);

                                }//transform != null

                            }//moveType = look

                            if(actionsFrontStart[a].moveType == MoveType.Move){

                                if(actionsFrontStart[a].transform != null){

                                    Player_Move(actionsFrontStart[a].transform.position, actionsFrontStart[a].speed);

                                //transform != null
                                } else {

                                    Player_Move(actionsFrontStart[a].position, actionsFrontStart[a].speed);

                                }//transform != null

                            }//moveType = move

                            actionsFrontStart[a].actionEvent.Invoke();

                            yield return new WaitForSeconds(actionsFrontStart[a].nextWait);

                        }//for a actionsFrontStart

                    }//actionsFrontStart.Count > 0                

                }//enterDirection = Front

                if(auto.enterDirection == EnterDirection.Back){ 

                    if(actionsBackStart.Count > 0){

                        for(int a = 0; a < actionsBackStart.Count; a++) {

                            if(actionsBackStart[a].stateChange != PlayerState.Current){

                                Player_State(actionsBackStart[a].stateChange);

                            }//stateChange != current

                            if(actionsBackStart[a].moveType == MoveType.Look){

                                if(actionsBackStart[a].transform != null){

                                    Player_Look(new Vector2(actionsBackStart[a].transform.localEulerAngles.y, actionsBackStart[a].transform.localEulerAngles.z), actionsBackStart[a].speed);

                                //transform != null
                                } else {

                                    Player_Look(new Vector2(actionsBackStart[a].rotation.x, actionsBackStart[a].rotation.y), actionsBackStart[a].speed);

                                }//transform != null

                            }//moveType = look

                            if(actionsBackStart[a].moveType == MoveType.Move){

                                if(actionsBackStart[a].transform != null){

                                    Player_Move(actionsBackStart[a].transform.position, actionsBackStart[a].speed);

                                //transform != null
                                } else {

                                    Player_Move(actionsBackStart[a].position, actionsBackStart[a].speed);

                                }//transform != null

                            }//moveType = move

                            actionsBackStart[a].actionEvent.Invoke();

                            yield return new WaitForSeconds(actionsBackStart[a].nextWait);

                        }//for a actionsBackStart

                    }//actionsBackStart.Count > 0  

                }//enterDirection = Back

            }//actionType = two sided

            #if PUZZLER_PRESENT

                if(extensions.cameraContSettings.useCameraCont){

                    if(extensions.cameraContSettings.useDefault){

                        autoExtensions.camCont.MoveType_SetDefault(true);

                    //useDefault
                    } else {

                        autoExtensions.camCont.MoveType_Set(extensions.cameraContSettings.customMoveType, true);

                    }//useDefault

                }//useCameraCont

            #endif

            if(userOptions.actionType == ActionType.EnterObject){

                if(references.dynamObj != null){

                    if((int)references.dynamObj.dynamicType == 0){

                        references.dynamObj.UseObject();

                    }//dynamicType == door

                }//dynamObj != null

            }//actionType = enter object

            if(userOptions.actionType == ActionType.PushObject){

                yield return new WaitForSeconds(0.1f);

                references.endTrigger.transform.parent = references.actionParent;
                this.transform.parent = auto.refs.transform;

            }//actionType = push object

            if(userOptions.lookLock == LockSettings.Unlock){

                MouseLook_Update(false);

                auto.refs.mouseLook.LockLook(false);

            }//lookLock = unlock

            if(userOptions.moveLock == LockSettings.Unlock){
            
                #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                    if(userOptions.adjustMoveSpeed){

                        auto.refs.playCont.CustomSpeed(userOptions.moveSpeed);
                        auto.refs.playCont.CustomSpeed_State(true);

                    }//adjustMoveSpeed

                    auto.refs.playCont.LockMove_State(false);
                
                #endif
                    
                #if COMPONENTS_PRESENT
                
                    if(auto.refs.playerMan != null){

                        if(playerOptions.sprintLock == LockSettings.Unlock){

                            auto.refs.playerMan.SprintLock_State(false);

                        }//sprintLock = unlock
                    
                    }//playerMan != null
                    
                #endif

            }//moveLock = unlock
                
            #if COMPONENTS_PRESENT
            
                if(auto.refs.playerMan != null){

                    if(playerOptions.leanLock == LockSettings.Unlock){

                        auto.refs.playerMan.LeanLock_State(false);

                    }//leanLock = unlock

                    if(playerOptions.zoomLock == LockSettings.Unlock){

                        auto.refs.playerMan.ZoomLock_State(false);

                    }//zoomLock = lock
                
                }//playerMan != null
            
            #endif
            
            #if HFPS_DURABILITY_PRESENT
            
                if(extensions.benchSettings.useBench){

                    if(extensions.benchSettings.bench != null){

                        extensions.benchSettings.bench.Interaction_Init();

                    }//bench != null

                }//useBench

            #endif

            #if PUZZLER_PRESENT

                if(extensions.itemViewerSettings.useItemViewer){

                    autoExtensions.itemViewer.Item_Catch(extensions.itemViewerSettings.itemID);
                    autoExtensions.itemViewer.Item_Show();

                }//useItemViewer
                
                if(extensions.puzzleSettings.usePuzzle){
                
                    if(extensions.puzzleSettings.actionType == ActionType_Puzzle.Activate){
                        
                        extensions.puzzleSettings.puzzle.Interaction_Init();
                
                    }//actionType = activate
                
                    if(extensions.puzzleSettings.actionType == ActionType_Puzzle.StateCheck){
                
                        extensions.puzzleSettings.puzzle.Holders_ActiveStateCheck(true);
                
                    }//actionType = state check
                
                }//usePuzzle

            #endif
            
            #if HFPS_SHOOTRANGE_PRESENT
            
                if(extensions.shootRangeSettings.useRange){
                
                    if(extensions.shootRangeSettings.shootRange != null){

                        extensions.shootRangeSettings.shootRange.Range_Init();

                    }//shootRange != null
                
                }//useRange
            
            #endif
            
            #if HFPS_VENDOR_PRESENT
            
                if(extensions.vendorSettings.currencySave){
                
                    Currency_Save.instance.Transfer_Init();
                
                }//currencySave
            
                if(extensions.vendorSettings.useVendor){

                    if(extensions.vendorSettings.vendor != null){

                        extensions.vendorSettings.vendor.Vendor_Init();
                        extensions.vendorSettings.vendor.CharacterAction_Catch(this);

                    }//vendor != null

                }//useVendor

            #endif

            if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){
            
                #if COMPONENTS_PRESENT

                    if(!extensions.actionBarSettings.useActionBar && !extensions.subActionSettings.useSubActions && userOptions.actionAttribute != ActionAttribute.Teleport){

                        if(interactType == InteractType.ReEnable){

                            yield return new WaitForSeconds(userOptions.interactObjWait);

                            Interactions_Update(userOptions.actionType, true, false);

                        }//interactType = re enable

                    }//!useActionBar & !useSubActions & actionAttribute != teleport

                #else 
                
                    if(!extensions.actionBarSettings.useActionBar){
                    
                        if(interactType == InteractType.ReEnable){

                            yield return new WaitForSeconds(userOptions.interactObjWait);

                            Interactions_Update(userOptions.actionType, true, false);

                        }//interactType = re enable

                    }//!useActionBar
                
                #endif
                
                #if COMPONENTS_PRESENT
    
                    if(userOptions.actionAttribute == ActionAttribute.Heal){

                        Player_Heal();

                    }//actionAttribute = heal

                    if(userOptions.actionAttribute == ActionAttribute.Teleport){

                        yield return new WaitForSeconds(teleportWait);

                        if(teleportType == Teleport_Type.Local){

                            transferAction.auto.oldRadius = auto.oldRadius;
                            transferAction.auto.currentItem = auto.currentItem;

                            transferAction.CharAction_Set();
                            transferAction.Interactions_Update(userOptions.actionType, false, false);
                            transferAction.auto.actionActive = true;

                            auto.refs.transform.position = transferAction.teleportTrans.position;

                            transferAction.Player_Look(new Vector2(transferAction.teleportTrans.localEulerAngles.y, transferAction.teleportTrans.localEulerAngles.z), 100);

                            yield return new WaitForSeconds(teleportWait);

                            transferAction.Action_Check();

                        }//teleportType = local

                        if(teleportType == Teleport_Type.Level){

                            if(transferType == Transfer_Type.ActionToLevel){

                                GameObject transActObj = new GameObject();
                                transActObj.name = "HFPS Transfer_Action";

                                lockMoveX = true;
                                lockMoveY = true;
                                lockJump = true;
                                lockStateInput = true;
                                lockItemSwitch = true;

                                HFPS_TransferAction tempTransAct = transActObj.AddComponent<HFPS_TransferAction>();

                                tempTransAct.ActionData_Catch(userOptions.actionType, transferActionName, auto.oldRadius, auto.currentItem, pauseLockTemp, inventoryLockTemp, saveStateTemp, loadStateTemp, lockMoveX, lockMoveY, lockJump, lockStateInput, lockLean, lockZoom, lockWeapZoom, lockItemUse, lockItemSwitch);

                            }//transferType = action to level

                            if(saveState == Save_State.Active){

                                auto.uiCont.Save_Init(true);

                            }//saveState = active

                            yield return new WaitForSeconds(0.4f);

                            auto.gameManager.ChangeScene_Loader(levelName, (int)loadState);

                        }//teleportType = level

                    }//actionAttribute = teleport
                
                #endif

            }//actionType = general or enter object

            if(userOptions.actionType == ActionType.TwoSided){

                Interactions_Update(userOptions.actionType, true, true);

            }//actionType = two sided
            
            Mouse_Update(playerOptions.showMouse);

            actionEvents.actionFinish.Invoke();

            auto.buffLock = false;
            auto.actionActive = true;
            
            if(userOptions.delayActionsEnd){
            
                yield return new WaitForSeconds(userOptions.actionEndWait);
            
            }//delayActionsEnd
            
            if(extensions.actionBarSettings.useActionBar){

                ActionBar_Update(true);

            }//useActionBar
            
            #if COMPONENTS_PRESENT

                if(extensions.subActionSettings.useSubActions){

                    SubActions_Update(true);

                }//useSubActions
            
            #endif

        }//Action_Start


    ////////////////////////////
    ///
    ///     END
    ///
    ////////////////////////////


        public IEnumerator Action_End(){

            StopCoroutine("Action_Start");

            Audio_Update(false);
            Mouse_Update(false);
            
            #if COMPONENTS_PRESENT

                if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){

                    if(userOptions.actionAttribute == ActionAttribute.Heal){

                        Player_HealStop();

                    }//actionAttribute = heal

                }//actionType = general or enter object
            
            #endif

            actionStopEvents.actionStopStart.Invoke();

            if(extensions.actionBarSettings.useActionBar){

                ActionBar_Update(false);

            }//useActionBar

            #if PUZZLER_PRESENT

                if(extensions.itemViewerSettings.useItemViewer){

                    if(!autoExtensions.itemViewer.isLooking){

                        autoExtensions.itemViewer.Item_Hide();

                    //!isLooking
                    } else {

                        if(extensions.itemViewerSettings.useHideDelayOnLook){

                            autoExtensions.itemViewer.Item_LookAt();

                            yield return new WaitForSeconds(autoExtensions.itemViewer.items[autoExtensions.itemViewer.tempSlot].animation.lookAtReturn.length + 0.15f);

                            autoExtensions.itemViewer.Item_Hide();

                        //useHideDelayOnLook
                        } else {

                            autoExtensions.itemViewer.Item_Hide();

                        }//useHideDelayOnLook

                    }//!isLooking

                }//useItemViewer

                if(extensions.cameraContSettings.useCameraCont){

                    autoExtensions.camCont.Move_Check();

                }//useCameraCont
                
                if(extensions.puzzleSettings.usePuzzle){
                
                    if(extensions.puzzleSettings.actionType == ActionType_Puzzle.StateCheck){
                
                        extensions.puzzleSettings.puzzle.Holders_ActiveStateCheck(false);
                
                    }//actionType = state check
                
                }//usePuzzle

            #endif
            
            #if COMPONENTS_PRESENT

                if(extensions.subActionSettings.useSubActions){

                    SubActions_Update(false);

                }//useSubActions
            
            #endif

            if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){

                Interactions_Update(userOptions.actionType, false, false);

            }//actionType = general or enter object

            if(userOptions.actionType == ActionType.TwoSided){

                Interactions_Update(userOptions.actionType, false, false);

            }//actionType = two sided

            if(userOptions.actionType == ActionType.PushObject){

                references.endTrigger.transform.parent = this.transform;
                this.transform.parent = references.actionParent;

            }//actionType = push object

            Player_LockState(true);
            
            #if COMPONENTS_PRESENT
            
                if(auto.refs.playerMan != null){

                    if(playerOptions.sprintInput == Input_Lock.Lock){

                        auto.refs.playerMan.SprintLock_State(true);

                    }//sprintInput = lock

                    if(playerOptions.leanInput == Input_Lock.Lock){

                        auto.refs.playerMan.LeanLock_State(true);

                    }//leanInput = lock

                    if(playerOptions.zoomInput == Input_Lock.Lock){

                        auto.refs.playerMan.ZoomLock_State(true);

                    }//zoomInput = lock
                
                }//playerMan != null
            
            #endif

            if(userOptions.lookLock == LockSettings.Unlock){

                MouseLook_Update(true);

            }//lookLock = unlock

            if(userOptions.actionType == ActionType.EnterObject){

                if(references.dynamObj != null){

                    if((int)references.dynamObj.dynamicType == 0){

                        references.dynamObj.UseObject();

                    }//dynamicType == door

                }//dynamObj != null

            }//actionType = enter object

            if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject | userOptions.actionType == ActionType.PushObject){

                if(actionsEnd.Count > 0){

                    for(int a = 0; a < actionsEnd.Count; a++) {

                        if(actionsEnd[a].stateChange != PlayerState.Current){

                            Player_State(actionsEnd[a].stateChange);

                        }//stateChange != current

                        if(actionsEnd[a].moveType == MoveType.Look){

                            if(actionsEnd[a].transform != null){

                                Player_Look(new Vector2(actionsEnd[a].transform.localEulerAngles.y, actionsEnd[a].transform.localEulerAngles.z), actionsEnd[a].speed);

                            //transform != null
                            } else {

                                Player_Look(new Vector2(actionsEnd[a].rotation.x, actionsEnd[a].rotation.y), actionsEnd[a].speed);

                            }//transform != null

                        }//moveType = look

                        if(actionsEnd[a].moveType == MoveType.Move){

                            if(actionsEnd[a].transform != null){

                                Player_Move(actionsEnd[a].transform.position, actionsEnd[a].speed);

                            //transform != null
                            } else {

                                Player_Move(actionsEnd[a].position, actionsEnd[a].speed);

                            }//transform != null

                        }//moveType = move

                        actionsEnd[a].actionEvent.Invoke();

                        yield return new WaitForSeconds(actionsEnd[a].nextWait);

                    }//for a actionsEnd

                }//actionsEnd.Count > 0

            }//actionType = general, enter object or push object

            if(userOptions.actionType == ActionType.TwoSided){

                if(auto.enterDirection == EnterDirection.Front){

                    if(actionsFrontEnd.Count > 0){

                        for(int a = 0; a < actionsFrontEnd.Count; a++) {

                            if(actionsFrontEnd[a].stateChange != PlayerState.Current){

                                Player_State(actionsFrontEnd[a].stateChange);

                            }//stateChange != current

                            if(actionsFrontEnd[a].moveType == MoveType.Look){

                                if(actionsFrontEnd[a].transform != null){

                                    Player_Look(new Vector2(actionsFrontEnd[a].transform.localEulerAngles.y, actionsFrontEnd[a].transform.localEulerAngles.z), actionsFrontEnd[a].speed);

                                //transform != null
                                } else {

                                    Player_Look(new Vector2(actionsFrontEnd[a].rotation.x, actionsFrontEnd[a].rotation.y), actionsFrontEnd[a].speed);

                                }//transform != null

                            }//moveType = look

                            if(actionsFrontEnd[a].moveType == MoveType.Move){

                                if(actionsFrontEnd[a].transform != null){

                                    Player_Move(actionsFrontEnd[a].transform.position, actionsFrontEnd[a].speed);

                                //transform != null
                                } else {

                                    Player_Move(actionsFrontEnd[a].position, actionsFrontEnd[a].speed);

                                }//transform != null

                            }//moveType = move

                            actionsFrontEnd[a].actionEvent.Invoke();

                            yield return new WaitForSeconds(actionsFrontEnd[a].nextWait);

                        }//for a actionsFrontEnd

                    }//actionsFrontEnd.Count > 0 

                }//enterDirection = Front

                if(auto.enterDirection == EnterDirection.Back){

                    if(actionsBackEnd.Count > 0){

                        for(int a = 0; a < actionsBackEnd.Count; a++) {

                            if(actionsBackEnd[a].stateChange != PlayerState.Current){

                                Player_State(actionsBackEnd[a].stateChange);

                            }//stateChange != current

                            if(actionsBackEnd[a].moveType == MoveType.Look){

                                if(actionsBackEnd[a].transform != null){

                                    Player_Look(new Vector2(actionsBackEnd[a].transform.localEulerAngles.y, actionsBackEnd[a].transform.localEulerAngles.z), actionsBackEnd[a].speed);

                                //transform != null
                                } else {

                                    Player_Look(new Vector2(actionsBackEnd[a].rotation.x, actionsBackEnd[a].rotation.y), actionsBackEnd[a].speed);

                                }//transform != null

                            }//moveType = look

                            if(actionsBackEnd[a].moveType == MoveType.Move){

                                if(actionsBackEnd[a].transform != null){

                                    Player_Move(actionsBackEnd[a].transform.position, actionsBackEnd[a].speed);

                                //transform != null
                                } else {

                                    Player_Move(actionsBackEnd[a].position, actionsBackEnd[a].speed);

                                }//transform != null

                            }//moveType = move

                            actionsBackEnd[a].actionEvent.Invoke();

                            yield return new WaitForSeconds(actionsBackEnd[a].nextWait);

                        }//for a actionsBackEnd

                    }//actionsBackEnd.Count > 0 

                }//enterDirection = Back

            }//actionType = two sided

            if(userOptions.actionType == ActionType.EnterObject){

                if(references.dynamObj != null){

                    if((int)references.dynamObj.dynamicType == 0){

                        references.dynamObj.UseObject();

                    }//dynamicType == door

                }//dynamObj != null

            }//actionType = enter object

            if(userOptions.adjustCharCont){

                auto.refs.characterController.radius = auto.oldRadius;

            }//adjustCharCont

            #if COMPONENTS_PRESENT

                if(userOptions.adjustMoveSpeed){

                    auto.refs.playCont.CustomSpeed_State(false);
                    auto.refs.playCont.CustomSpeed(0);

                }//adjustMoveSpeed

            #endif

            actionStopEvents.actionStopFinish.Invoke();

            if(userOptions.delayActionsEnd){
            
                yield return new WaitForSeconds(userOptions.actionEndWait);
            
            }//delayActionsEnd
            
            Player_LockState(false);
            
            Items_Update(true);
            
            #if COMPONENTS_PRESENT
            
                if(auto.refs.playerMan != null){

                    if(!auto.actionTransferred){

                        if(playerOptions.xInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockMoveX_State(false);

                        //xInput = lock
                        } else {

                            if(playerOptions.xLimit == Input_Limit.Limit){

                                auto.refs.playerMan.LimitMoveX_State(false);

                            }//xLimit = limit

                        }//xInput = lock

                        if(playerOptions.yInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockMoveY_State(false);

                        //yInput = lock
                        } else {

                            if(playerOptions.yLimit == Input_Limit.Limit){

                                auto.refs.playerMan.LimitMoveY_State(false);

                            }//yLimit = limit

                        }//yInput = lock

                        if(playerOptions.jumpInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockJump_State(false);

                        }//jumpInput = lock

                        if(playerOptions.stateInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockStateInput_State(false);

                        }//stateInput = lock

                        if(playerOptions.sprintInput == Input_Lock.Lock){

                            auto.refs.playerMan.SprintLock_State(false);

                        }//sprintInput = lock

                        if(playerOptions.leanInput == Input_Lock.Lock){

                            auto.refs.playerMan.LeanLock_State(false);

                        }//leanInput = lock

                        if(playerOptions.zoomInput == Input_Lock.Lock){

                            auto.refs.playerMan.ZoomLock_State(false);

                        }//zoomInput = lock

                        if(userOptions.itemDisplay.itemUseLock == Input_Lock.Lock){

                            auto.refs.playerMan.WeaponsUseLock_State(false);

                        }//itemUseLock = lock

                        if(userOptions.itemDisplay.itemZoomLock == Input_Lock.Lock){

                            auto.refs.playerMan.WeaponsZoomLock_State(false);

                        }//itemZoomLock = lock

                    //!actionTransferred
                    } else {

                        auto.refs.playerMan.LockMoveX_State(false);
                        auto.refs.playerMan.LockMoveY_State(false);
                        auto.refs.playerMan.LockJump_State(false);
                        auto.refs.playerMan.LockStateInput_State(false);
                        auto.refs.playerMan.LeanLock_State(false);
                        auto.refs.playerMan.ZoomLock_State(false);
                        auto.refs.playerMan.WeaponsUseLock_State(false);
                        auto.refs.playerMan.WeaponsZoomLock_State(false);

                        auto.actionTransferred = false;

                    }//!actionTransferred

                    if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){

                        if(userOptions.actionAttribute == ActionAttribute.Hide){

                            auto.refs.playerMan.Hide_State(false);

                        }//actionAttribute = hide

                    }//actionType = general or enter object
                
                }//playerMan != null
            
            #endif

            auto.buffLock = false;
            auto.actionActive = false;

            CharAction_Clear();

            if(hfpsUI.pauseStateStart != Action_State.Disable){

                if(hfpsUI.saveStateEnd == Action_State.Enable){

                    auto.uiCont.Save_State(true);

                }//saveStateEnd = enable

                if(hfpsUI.saveStateEnd == Action_State.Disable){

                    auto.uiCont.Save_State(false);

                }//saveStateEnd = disable

                if(hfpsUI.loadStateEnd == Action_State.Enable){

                    auto.uiCont.Load_State(true);

                }//loadStateEnd = enable

                if(hfpsUI.loadStateEnd == Action_State.Disable){

                    auto.uiCont.Load_State(false);

                }//loadStateEnd = disable

            }//pauseStateStart != disable
            
            if(hfpsUI.gameUIStateStart == Action_State.Nothing){

                if(hfpsUI.useDisplaySet){
                
                    if(hfpsUI.displaySet != ""){
                    
                        auto.uiCont.DisplaySet_State(true, hfpsUI.displaySet);
                    
                    }//displaySet != null
                
                }//useDisplaySet

            }//gameUIStateStart = nothing
            
            if(!hfpsUI.useDisplaySet){

                if(hfpsUI.gameUIStateEnd == Action_State.Enable){

                    auto.gameManager.gamePanels.MainGamePanel.SetActive(true);

                }//gameUIStateEnd = enable

                if(hfpsUI.gameUIStateEnd == Action_State.Disable){

                    auto.gameManager.gamePanels.MainGamePanel.SetActive(false);

                }//gameUIStateEnd = disable
            
            }//!useDisplaySet

            if(hfpsUI.pauseStateEnd == Action_State.Enable){

                auto.uiCont.PauseLock_State(false);

            }//pauseStateEnd = enable

            if(hfpsUI.pauseStateEnd == Action_State.Disable){

                auto.uiCont.PauseLock_State(true);

            }//pauseStateEnd = disable

            if(hfpsUI.inventoryStateEnd == Action_State.Enable){

                auto.uiCont.InventoryLock_State(false);

            }//inventoryStateEnd = enable

            if(hfpsUI.inventoryStateEnd == Action_State.Disable){

                auto.uiCont.InventoryLock_State(true);

            }//inventoryStateEnd = disable

            #if COMPONENTS_PRESENT

                if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){

                    if(userOptions.actionAttribute == ActionAttribute.Teleport){

                        if(teleportType == Teleport_Type.Level){

                            if(saveState == Save_State.Active){

                                StartCoroutine("Save_Buff");

                            }//saveState = active

                        }//teleportType = level

                    }//actionAttribute = teleport

                }//actionType = general or enter object
            
            #endif

            if(userOptions.useType == UseType.SingleUse){

                Locked_State(true);

            //useType = single
            } else {

                #if PUZZLER_PRESENT

                    if(userOptions.linkComplete){

                        if(!userOptions.handler.Complete_Get()){

                            Interactions_Update(userOptions.actionType, true, false);

                        //!complete
                        } else {

                            Locked_State(true);

                        }//!complete

                    //linkComplete
                    } else {

                        Interactions_Update(userOptions.actionType, true, false);

                    }//linkComplete

                #else 

                    Interactions_Update(userOptions.actionType, true, false);

                #endif

            }//useType = single

        }//Action_End


    ////////////////////////////
    ///
    ///     UPDATE
    ///
    ////////////////////////////


        private void Action_Update(){

            CharAction_Set();
            Audio_Update(true);
            Interactions_Update(userOptions.actionType, false, false);

            if(hfpsUI.gameUIStateStart == Action_State.Enable){

                auto.gameManager.gamePanels.MainGamePanel.SetActive(true);

            }//gameUIStateStart = enable

            if(hfpsUI.gameUIStateStart == Action_State.Disable){

                auto.gameManager.gamePanels.MainGamePanel.SetActive(false);

            }//gameUIStateStart = disable

            if(hfpsUI.pauseStateStart == Action_State.Enable){

                auto.uiCont.PauseLock_State(false);

            }//pauseStateStart = enable

            if(hfpsUI.pauseStateStart == Action_State.Disable){

                auto.uiCont.PauseLock_State(true);

            }//pauseStateStart = disable

            if(hfpsUI.inventoryStateStart == Action_State.Enable){

                auto.uiCont.InventoryLock_State(false);

            }//inventoryStateStart = enable

            if(hfpsUI.inventoryStateStart == Action_State.Disable){

                auto.uiCont.InventoryLock_State(true);

            }//inventoryStateStart = disable

            if(hfpsUI.pauseStateStart != Action_State.Disable){

                if(hfpsUI.saveStateStart == Action_State.Enable){

                    auto.uiCont.Save_State(true);

                }//saveStateStart = enable

                if(hfpsUI.saveStateStart == Action_State.Disable){

                    auto.uiCont.Save_State(false);

                }//saveStateStart = disable

                if(hfpsUI.loadStateStart == Action_State.Enable){

                    auto.uiCont.Load_State(true);

                }//loadStateStart = enable

                if(hfpsUI.loadStateStart == Action_State.Disable){

                    auto.uiCont.Load_State(false);

                }//loadStateStart = disable

            }//pauseStateStart != disable

            if(userOptions.adjustCharCont){

                auto.oldRadius = auto.refs.characterController.radius;
                auto.refs.characterController.radius = userOptions.radius;

            }//adjustCharCont
            
            #if COMPONENTS_PRESENT

                if(userOptions.moveLock == LockSettings.Unlock){
                
                    if(auto.refs.playerMan != null){

                        if(playerOptions.xInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockMoveX_State(true);

                        }//xInput = lock

                        if(playerOptions.yInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockMoveY_State(true);

                        }//yInput = lock

                        if(playerOptions.jumpInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockJump_State(true);

                        }//jumpInput = lock

                        if(playerOptions.stateInput == Input_Lock.Lock){

                            auto.refs.playerMan.LockStateInput_State(true);

                        }//stateInput = lock
                    
                    }//playerMan != null

                    if(userOptions.adjustMoveSpeed){

                        auto.refs.playCont.CustomSpeed(userOptions.moveSpeed);
                        auto.refs.playCont.CustomSpeed_State(true);

                    }//adjustMoveSpeed

                }//moveLock = unlock
                
                if(auto.refs.playerMan != null){

                    if(playerOptions.sprintInput == Input_Lock.Lock){

                        if(playerOptions.sprintLock != LockSettings.Unlock){

                            auto.refs.playerMan.SprintLock_State(true);

                        }//sprintLock != unlock

                    }//sprintInput = lock

                    if(playerOptions.leanInput == Input_Lock.Lock){

                        if(playerOptions.leanLock != LockSettings.Unlock){

                            auto.refs.playerMan.LeanLock_State(true);

                        }//leanLock != unlock

                    }//leanInput = lock

                    if(playerOptions.zoomInput == Input_Lock.Lock){

                        if(playerOptions.zoomLock != LockSettings.Unlock){

                            auto.refs.playerMan.ZoomLock_State(true);

                        }//zoomLock != unlock

                    }//zoomInput = lock

                    if(userOptions.itemDisplay.itemUseLock == Input_Lock.Lock){

                        auto.refs.playerMan.WeaponsUseLock_State(true);

                    }//itemUseLock = lock

                    if(userOptions.itemDisplay.itemZoomLock == Input_Lock.Lock){

                        auto.refs.playerMan.WeaponsZoomLock_State(true);

                    }//itemZoomLock = lock
                
                }//playerMan != null
            
            #endif

            if(extensions.actionBarSettings.useActionBar){

                ActionBar_Update(true);

            }//useActionBar

            if(userOptions.actionType == ActionType.General | userOptions.actionType == ActionType.EnterObject){
            
                #if COMPONENTS_PRESENT

                    if(!extensions.actionBarSettings.useActionBar && !extensions.subActionSettings.useSubActions && userOptions.actionAttribute != ActionAttribute.Teleport){

                        Interactions_Update(userOptions.actionType, true, false);

                    }//!useActionBar & !useSubActions & actionAttribute != teleport

                #else 
                
                    if(!extensions.actionBarSettings.useActionBar){

                        Interactions_Update(userOptions.actionType, true, false);

                    }//!useActionBar
                
                #endif
                
                #if COMPONENTS_PRESENT

                    if(userOptions.actionAttribute == ActionAttribute.Heal){

                        Player_Heal();

                    }//actionAttribute = heal

                #endif

            }//actionType = general or enter object

            if(userOptions.actionType == ActionType.TwoSided){

                Interactions_Update(userOptions.actionType, true, true);

            }//actionType = two sided
            
            #if COMPONENTS_PRESENT

                if(extensions.subActionSettings.useSubActions){

                    SubActions_Update(true);

                }//useSubActions
            
            #endif

        }//Action_Update


    //////////////////////////////////////
    ///
    ///     ACTION ACTIONS
    ///
    ///////////////////////////////////////

    ///////////////////////////////
    ///
    ///     ITEMS ACTIONS
    ///
    ///////////////////////////////


        public void Items_Update(bool show){

            if(show){

                if(auto.refs.itemSwitcher != null){

                    if(userOptions.itemDisplay.itemDispExit == ItemDisplayState.Show){

                        auto.refs.itemSwitcher.SelectSwitcherItem(auto.currentItem);

                        auto.currentItem = -1;

                    }//itemDisplay = show

                }//itemSwitcher != null

                #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                    if(userOptions.itemDisplay.itemSwitchLock == Input_Lock.Lock){

                        Inventory.Instance.CustomLock_State(false);

                    }//itemSwitchLock = lock

                #endif

            //show
            } else {

                if(auto.refs.itemSwitcher != null){

                    #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                        if(userOptions.itemDisplay.itemSwitchLock == Input_Lock.Lock){

                            Inventory.Instance.CustomLock_State(true);

                        }//itemSwitchLock = lock
                        
                    #endif
                    
                    #if COMPONENTS_PRESENT

                        if(userOptions.itemDisplay.itemDispEnter == ItemDisplayState.Current && userOptions.itemDisplay.itemDispExit != ItemDisplayState.Hide){

                            if(userOptions.actionAttribute == ActionAttribute.Teleport){

                                auto.currentItem = auto.refs.itemSwitcher.currentItem;

                            }//actionAttribute = teleport

                        }//itemDispEnter = current
                    
                    #endif

                    if(userOptions.itemDisplay.itemDispEnter == ItemDisplayState.Hide){

                        if(userOptions.itemDisplay.itemDispExit == ItemDisplayState.Show){

                            auto.currentItem = auto.refs.itemSwitcher.currentItem;

                        }//itemDisplay = show

                        #if COMPONENTS_PRESENT

                            auto.refs.itemSwitcher.DeselectItems_Forced();

                        #else
                        
                             auto.refs.itemSwitcher.DeselectItems();
                        
                        #endif

                    }//itemDisplay = hide

                }//itemSwitcher != null

            }//show

        }//Items_Update


    ///////////////////////////////
    ///
    ///     INTERACTIONS ACTIONS
    ///
    ///////////////////////////////


        public void Interactions_Update(ActionType newType, bool state, bool inside){

            if(userOptions.actionType != ActionType.TwoSided){

                if(references.interactObj != null){

                    references.interactObj.SetActive(state);

                }//interactObj != null

                if(references.interactCol != null){

                    references.interactCol.enabled = state;

                }//interactCol != null

            }//actionType != two sided

            if(userOptions.actionType == ActionType.TwoSided){

                if(!inside){

                    references.interactFront.SetActive(state);
                    references.interactBack.SetActive(state);

                //!inside
                } else {

                    if(auto.enterDirection == EnterDirection.Front){

                        references.endTrigger_Back.SetActive(state);

                        if(references.exitBlock_Front != null){

                            references.exitBlock_Front.SetActive(state);

                        }//exitBlock_Front != null

                    }//enterDirection = front

                    if(auto.enterDirection == EnterDirection.Back){

                        references.endTrigger_Front.SetActive(state);

                        if(references.exitBlock_Back != null){

                            references.exitBlock_Back.SetActive(state);

                        }//exitBlock_Back != null

                    }//enterDirection = back

                }//!inside

            }//actionType = two sided

        }//Interactions_Update


    ///////////////////////////////
    ///
    ///     PLAYER ACTIONS
    ///
    ///////////////////////////////


        public void Player_Look(Vector2 newRotation, float newSpeed){

            auto.refs.mouseLook.LerpLook(newRotation, newSpeed, true);

        }//Player_Look

        public void Player_Move(Vector3 newPosition, float newSpeed){

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                auto.refs.playCont.StartCoroutine(auto.refs.playCont.MovePlayer(newPosition, newSpeed, false, false));
            
            #endif

        }//Player_Move

        public void Player_State(PlayerState newState){

            if(newState == PlayerState.Stand){

                if(auto.refs.playCont.characterState != PlayerController.CharacterState.Stand){

                    auto.refs.playCont.characterState = PlayerController.CharacterState.Stand;

                }//characterState != stand

            }//newState = stand

            if(newState == PlayerState.Crouch){

                if(auto.refs.playCont.characterState != PlayerController.CharacterState.Crouch){

                    auto.refs.playCont.characterState = PlayerController.CharacterState.Crouch;

                }//characterState != crouch

            }//newState = stand

            if(newState == PlayerState.Prone){

                if(auto.refs.playCont.characterState != PlayerController.CharacterState.Prone){

                    auto.refs.playCont.characterState = PlayerController.CharacterState.Prone;

                }//characterState != prone

            }//newState = stand

        }//Player_State

        public void Player_LockState(bool state){

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                auto.refs.playCont.LockMove_State(state);
                auto.refs.mouseLook.LockLook(state);
                auto.refs.mouseLook.deltaInputX = 0;
                auto.refs.mouseLook.deltaInputY = 0;

                auto.refs.playCont.inputMovement = Vector2.zero;
                auto.refs.playCont.inputX = 0;
                auto.refs.playCont.inputY = 0;

            #endif

        }//Player_LockState


    ///////////////////////////////
    ///
    ///     MOUSE ACTIONS
    ///
    ///////////////////////////////


        public void Mouse_Update(bool showMouse){
        
            if(inputType == DM_InternEnums.PlayInput_Type.Keyboard){
        
                if(showMouse){

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                //showMouse
                } else {

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                }//showMouse
            
            }//inputType = keyboard
        
        }//Mouse_Update

        public void MouseLook_Update(bool reset){

            if(lookOptions.updateLookCaps){

                if(!reset){

                    auto.minXOld = auto.refs.mouseLook.minimumX;
                    auto.maxXOld = auto.refs.mouseLook.maximumX;

                    auto.minYOld = auto.refs.mouseLook.minimumY;
                    auto.maxYOld = auto.refs.mouseLook.maximumY;

                    if(lookOptions.updateLookX){

                        auto.refs.mouseLook.minimumX = lookOptions.minX;
                        auto.refs.mouseLook.maximumX = lookOptions.maxX;

                    }//updateLookX

                    if(lookOptions.updateLookY){

                        auto.refs.mouseLook.minimumY = lookOptions.minY;
                        auto.refs.mouseLook.maximumY = lookOptions.maxY;

                    }//updateLookY

                //reset
                } else {

                    if(lookOptions.updateLookX){

                        auto.refs.mouseLook.minimumX = auto.minXOld;
                        auto.refs.mouseLook.maximumX = auto.maxXOld;

                    }//updateLookX

                    if(lookOptions.updateLookY){

                        auto.refs.mouseLook.minimumY = auto.minYOld;
                        auto.refs.mouseLook.maximumY = auto.maxYOld;

                    }//updateLookY

                }//reset

            }//updateLookCaps

        }//MouseLook_Update


    ///////////////////////////////
    ///
    ///     SUB ACTIONS ACTIONS
    ///
    ///////////////////////////////


        public void SubActions_Update(bool state){
        
            #if COMPONENTS_PRESENT
            
                if(auto.refs.subActionsHandler != null){

                    if(state){

                        if(extensions.subActionSettings.actionTypes.Count > 0){

                            for(int at = 0; at < extensions.subActionSettings.actionTypes.Count; at++) {

                                for(int sa = 0; sa < auto.refs.subActionsHandler.subActions.Count; sa++) {

                                    if(extensions.subActionSettings.actionTypes[at].type == auto.refs.subActionsHandler.subActions[sa].type){

                                        auto.refs.subActionsHandler.subActions[sa].isActive = true;
                                        auto.refs.subActionsHandler.subActions[sa].inputSlot = extensions.subActionSettings.actionTypes[at].inputSlot;

                                    }//type = type

                                }//for sa subActions

                            }//for at actionTypes

                        }//actionTypes.Count > 0

                        if(auto.refs.subActionsHandler.actionInputs.Count > 0){

                            for(int sa = 0; sa < auto.refs.subActionsHandler.subActions.Count; sa++) {

                                for(int ai = 0; ai < auto.refs.subActionsHandler.actionInputs.Count; ai++) {

                                    if(auto.refs.subActionsHandler.subActions[sa].inputSlot == ai){

                                        auto.refs.subActionsHandler.actionInputs[ai].isActive = true;

                                    }//inputSlot = ai

                                }//for ai actionInputs

                            }//for sa subActions

                        }//actionInputs.Count > 0

                        if(auto.subActsUI != null){

                            auto.subActsUI.ActionHolders_Reset();

                            for(int sa = 0; sa < auto.refs.subActionsHandler.subActions.Count; sa++) {

                                if(auto.refs.subActionsHandler.subActions[sa].isActive){

                                    auto.subActsUI.ActionHolder_Update(auto.refs.subActionsHandler.subActions[sa].inputSlot, auto.refs.subActionsHandler.subActions[sa].display, auto.refs.subActionsHandler.subActions[sa].icon);

                                }//isActive

                            }//for sa subActions

                            auto.subActsUI.Actions_Show();

                        }//subActsUI != null

                        auto.refs.subActionsHandler.LockDelay_Update(0.2f);
                        auto.refs.subActionsHandler.LockState_Delayed(false);

                    //state
                    } else {

                        auto.refs.subActionsHandler.Locked_State(true);
                        auto.refs.subActionsHandler.SubActions_Reset();
                        auto.refs.subActionsHandler.Inputs_Reset();

                        if(auto.subActsUI != null){

                            auto.subActsUI.Actions_Hide();

                        }//subActsUI != null

                    }//state
                
                }//subActionsHandler != null
            
            #endif

        }//SubActions_Update


    ///////////////////////////////
    ///
    ///     ACTION BAR ACTIONS
    ///
    ///////////////////////////////


        public void ActionBar_Update(bool state){
        
            if(autoExtensions.actionBar != null){

                if(state){

                    if(extensions.actionBarSettings.actions.Count > 0){
                    
                        autoExtensions.actionBar.Actions_Add(extensions.actionBarSettings.actions);

                        autoExtensions.actionBar.ActionBar_Check();
                        autoExtensions.actionBar.ActionBar_StateCheck();

                    }//actions.Count > 0

                //state
                } else {

                    if(extensions.actionBarSettings.actions.Count > 0){
                    
                        autoExtensions.actionBar.Actions_Clear();

                        autoExtensions.actionBar.ActionBar_Check();
                        autoExtensions.actionBar.ActionBar_StateCheck();

                    }//actions.Count > 0

                }//state
            
            }//actionBar != null

        }//ActionBar_Update


    ///////////////////////////////
    ///
    ///     HEAL ACTIONS
    ///
    ///////////////////////////////


        private void Player_Heal(){

            if(!auto.locked){

                #if COMPONENTS_PRESENT

                    if(auto.refs != null){

                        if(auto.refs.healthManager.Health != auto.refs.healthManager.maxRegenerateHealth){

                            auto.refs.healthManager.RegenCoroutine = StartCoroutine(auto.refs.healthManager.Regenerate());

                        }//Health != maxRegenerateHealth

                    //auto.refs != null
                    } else {

                        if(auto.refs.healthManager.Health != auto.refs.healthManager.maxRegenerateHealth){

                            auto.refs.healthManager.RegenCoroutine = StartCoroutine(auto.refs.healthManager.Regenerate());

                        }//Health != maxRegenerateHealth

                    }//auto.refs != null

                #endif

            }//!locked

        }//Player_Heal

        private void Player_HealStop(){

            #if COMPONENTS_PRESENT

                if(auto.refs != null){

                    if(auto.refs.healthManager.RegenCoroutine != null){

                        StopCoroutine(auto.refs.healthManager.RegenCoroutine);
                        auto.refs.healthManager.Health = Mathf.RoundToInt(auto.refs.healthManager.Health);

                    }//RegenCoroutine != null

                //auto.refs != null
                } else {

                    if(auto.refs.healthManager.RegenCoroutine != null){

                        StopCoroutine(auto.refs.healthManager.RegenCoroutine);
                        auto.refs.healthManager.Health = Mathf.RoundToInt(auto.refs.healthManager.Health);

                    }//RegenCoroutine != null    

                }//auto.refs != null

            #endif

        }//Player_HealStop


    ///////////////////////////////
    ///
    ///     AUDIO ACTIONS
    ///
    ///////////////////////////////


        public void Audio_Update(bool play){

            if(audioOptions.audioType != AudioType.None){

                Audio_State(play);

            }//audioType != none

        }//Audio_Update

        public void Audio_State(bool play){
        
            #if COMPONENTS_PRESENT
            
                if(auto.refs.audioFader != null){

                    if(play){

                        if(audioOptions.audioType == AudioType.Music){

                            if(audioOptions.customVolume){

                                auto.refs.audioFader.Fade_Start(HFPS_AudioFader.FadeType.Music, audioOptions.clip, audioOptions.audioVolume, audioOptions.keepAmbience, audioOptions.immediate);

                            //customVolume
                            } else {

                                auto.refs.audioFader.Fade_Start(HFPS_AudioFader.FadeType.Music, audioOptions.clip, 0, audioOptions.keepAmbience, audioOptions.immediate);

                            }//customVolume

                        }//audioType = Music

                        if(audioOptions.audioType == AudioType.Ambience){

                            if(audioOptions.customVolume){

                                auto.refs.audioFader.Fade_Start(HFPS_AudioFader.FadeType.Ambience, audioOptions.clip, audioOptions.audioVolume, audioOptions.keepAmbience, audioOptions.immediate);

                            //customVolume
                            } else {

                                auto.refs.audioFader.Fade_Start(HFPS_AudioFader.FadeType.Ambience, audioOptions.clip, 0, audioOptions.keepAmbience, audioOptions.immediate);

                            }//customVolume

                        }//audioType = Ambience

                    //play
                    } else {

                        auto.refs.audioFader.Fade_Revert();

                    }//play
                
                }//audioFader != null
            
            #endif

        }//Audio_State


    ///////////////////////////////
    ///
    ///     TWO SIDED ACTIONS
    ///
    ///////////////////////////////


        public void EnterDirection_Set(int dir){

            if(dir == 1){

                auto.enterDirection = EnterDirection.Front;

            }//dir = front

            if(dir == 2){

                auto.enterDirection = EnterDirection.Back;

            }//dir = back

        }//EnterDirection_Set


    ///////////////////////////////
    ///
    ///     CHARACTER ACTION ACTIONS
    ///
    ///////////////////////////////


        public void CharAction_Set(){

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)
            
                auto.refs.charAction = this;

            #endif
            
        }//CharAction_Set

        public void CharAction_Clear(){

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                auto.refs.charAction = null;

            #endif

        }//CharAction_Clear


    //////////////////////////
    //
    //      INPUT ACTIONS
    //
    //////////////////////////

    /////////////////
    //
    //      INPUT CHECKS
    //
    /////////////////


        public void InputCheck_Type(){

            if(InputHandler.CurrentDevice == InputHandler.Device.MouseKeyboard) {

                inputType = DM_InternEnums.PlayInput_Type.Keyboard;

            //deviceType = keyboard
            } else if(InputHandler.CurrentDevice.IsGamepadDevice() > 0) {

                inputType = DM_InternEnums.PlayInput_Type.Gamepad;

            }//deviceType = gamepad

        }//InputCheck_Type


    ///////////////////////////////
    ///
    ///     PARSE ACTIONS
    ///
    ///////////////////////////////


        public void Parse_UseType(int type){

            userOptions.useType = (UseType)type;

        }//Parse_UseType


    ///////////////////////////////
    ///
    ///     LOCKED ACTIONS
    ///
    ///////////////////////////////


        public void Locked_State(bool state){

            auto.locked = state;

        }//Locked_State

        public void LockedState_DelaySet(float wait){

            auto.tempWait = wait;

        }//LockedState_DelaySet

        public void LockedState_Delayed(bool state){

            StartCoroutine("LockedStateDelayed_Buff", state);

        }//LockedState_Delayed

        private IEnumerator LockedStateDelayed_Buff(bool state){

            yield return new WaitForSeconds(auto.tempWait);

            Locked_State(state);

        }//LockedStateDelayed_Buff


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        private IEnumerator Save_Buff(){

            yield return new WaitForSeconds(0.5f);

            auto.uiCont.Save_Init(false);

        }//Save_Buff

        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"startActionLocked", auto.startActionLocked },
                {"actionActive", auto.actionActive },
                {"locked", auto.locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            auto.startActionLocked = (bool)token["startActionLocked"];
            auto.actionActive = (bool)token["actionActive"];
            auto.locked = (bool)token["locked"];

            if(auto.actionActive){

                Action_Update();

            }//actionActive

        }//OnLoad


    }//HFPS_CharacterAction


}//namespace