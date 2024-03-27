using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using Newtonsoft.Json.Linq;
using HFPS.Player;
using HFPS.Systems;
using ThunderWire.Input;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Player/Player Attention")]
    public class HFPS_PlayerAttention : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


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
        public class Events {

            [Space]

            public UnityEvent onAttentionStart;
            public UnityEvent onAttentionEnd;
            public UnityEvent onAttentionEndLate;

        }//Events

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
        public class Action {

            [Space]

            public string name;
            public string text;

            [Space]

            public UnityEvent actionInit;

        }//Action


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Attention_Type {

            Regular = 0,
            Input = 1,

        }//Attention_Type

        public enum AttentionStop_Type {

            Manual = 0,
            Auto = 1,

        }//AttentionStop_Type

        public enum MoveType {

            Look = 0,
            Move = 1,
            State = 2,

        }//MoveType

        public enum Player_Unlock {

            Manual = 0,
            Auto = 1,

        }//Player_Unlock

        public enum PlayerState {

            Current = 0,
            Stand = 1,
            Crouch = 2,
            Prone = 3,

        }//PlayerState

        public enum LockSettings {

            Unlock = 0,
            KeepLocked = 1,

        }//LockSettings

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

        public enum UseType {

            SingleUse = 0,
            MultiUse = 1,

        }//UseType


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     GENERAL OPTIONS
    ///
    ///////////////////////////


        public UseType useType;
        public Attention_Type attentionType;
        public AttentionStop_Type attentionStopType;
        public float attentionTime;


    ///////////////////////////
    ///
    ///     PLAYER OPTIONS
    ///
    ///////////////////////////


        public bool showMouse;

        public Input_Lock jumpInput;
        public Input_Lock stateInput;

        public Input_Lock leanInput;
        public Input_Lock sprintInput;
        public Input_Lock zoomInput;


    ///////////////////////////
    ///
    ///     ITEMS OPTIONS
    ///
    ///////////////////////////


        public Input_Lock itemInputLock;
        public Input_Lock itemUseLock;
        public Input_Lock itemZoomLock;

        public ItemDisplayState itemDispStart;
        public ItemDisplayState itemDispEnd;


    ///////////////////////////
    ///
    ///     MOVEMENT OPTIONS
    ///
    ///////////////////////////


        public List<Action_Point> actions;

        public Player_Unlock playerUnlock;
        public float unlockWait;


    ///////////////////////////
    ///
    ///     UI OPTIONS
    ///
    ///////////////////////////


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


    ///////////////////////////
    ///
    ///     EXTENSIONS OPTIONS
    ///
    ///////////////////////////


        public bool useSceneAction;
        public HFPS_SceneAction sceneAction;

        public ActionBarSettings actionBarSettings;


    ///////////////////////////
    ///
    ///     EVENTS
    ///
    ///////////////////////////


        public Events events;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public DM_InternEnums.PlayInput_Type inputType;
        public HFPS_References refs;
        public DM_ActionBar actionBar;
        public bool actBarShowing;
        public int currentItem;
        public bool locked;

        public int tabs;

        public bool genOpts;
        public bool playOpts;
        public bool itemOpts;
        public bool uiOpts;
        public bool moveOpts;
        public bool extOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            bool catchAB = false;

            actBarShowing = false;
            currentItem = -1;

            if(attentionType == Attention_Type.Input){

                if(actionBarSettings.useActionBar){

                    if(actionBar == null){

                        if(!actionBarSettings.useCustomActionBar){

                            catchAB = true;

                        //!useCustomActionBar
                        } else {

                            if(actionBarSettings.actionBar == null){

                                catchAB = true;

                            //actionBar == null
                            } else {

                                actionBar = actionBarSettings.actionBar;

                            }//actionBar == null

                        }//!useCustomActionBar

                        if(catchAB){

                            if(DM_ActionBar.instance != null){

                                actionBar = DM_ActionBar.instance;

                            }//instance != null

                        }//catchAB

                    }//actionBar = null

                }//useActionBar

            }//attentionType = input

        }//StartInit


    //////////////////////////////////////
    ///
    ///     ATTENTION ACTIONS
    ///
    ///////////////////////////////////////


        public void AttentionInit_Delayed(float delay){

            StartCoroutine("AttentionDelayed", delay);

        }//AttentionInit_Delayed

        private IEnumerator AttentionDelayed(float delay){

            yield return new WaitForSeconds(delay);

            Attention_Init();

        }//AttentionDelayed

        public void Attention_Init(){

            if(!locked){

                events.onAttentionStart.Invoke();

                StartCoroutine("Attention_Start");

            }//!locked

        }//Attention_Init

        private IEnumerator Attention_Start(){

            if(refs == null){

                refs = HFPS_References.instance;

            }//refs = null

            if(showMouse){

                if(InputHandler.InputIsInitialized) {

                    InputCheck_Type();

                }//InputIsInitialized

            }//showMouse

            if(attentionType == Attention_Type.Input){

                if(actionBarSettings.useActionBar){

                    actionBar.ActionBar_State(false);

                }//useActionBar

            }//attentionType = input

            Items_Update(false);
            Player_LockState(true);
            
            #if COMPONENTS_PRESENT

                if(jumpInput == Input_Lock.Lock){

                    refs.playerMan.LockJump_State(true);

                }//jumpInput = lock

                if(stateInput == Input_Lock.Lock){

                    refs.playerMan.LockStateInput_State(true);

                }//stateInput = lock

                if(sprintInput == Input_Lock.Lock){

                    refs.playerMan.SprintLock_State(true);

                }//sprintInput = lock

                if(leanInput == Input_Lock.Lock){

                    refs.playerMan.LeanLock_State(true);

                }//leanInput = lock

                if(zoomInput == Input_Lock.Lock){

                    refs.playerMan.ZoomLock_State(true);

                }//zoomInput = lock

                if(itemUseLock == Input_Lock.Lock){

                    refs.playerMan.WeaponsUseLock_State(true);

                }//itemUseLock = lock

                if(itemZoomLock == Input_Lock.Lock){

                    refs.playerMan.WeaponsZoomLock_State(true);

                }//itemZoomLock = lock
            
            #endif

            if(gameUIStateStart == Action_State.Nothing){

                if(useDisplaySet){
                
                    if(displaySet != ""){
                    
                        HFPS_UICont.instance.DisplaySet_State(false, displaySet);
                    
                    }//displaySet != null
                
                }//useDisplaySet

            }//gameUIStateStart = nothing
            
            if(!useDisplaySet){

                if(gameUIStateStart == Action_State.Enable){

                    HFPS_GameManager.Instance.gamePanels.MainGamePanel.SetActive(true);

                }//gameUIStateStart = enable

                if(gameUIStateStart == Action_State.Disable){

                    HFPS_GameManager.Instance.gamePanels.MainGamePanel.SetActive(false);

                }//gameUIStateStart = disable
            
            }//!useDisplaySet

            if(pauseStateStart == Action_State.Enable){

                HFPS_UICont.instance.PauseLock_State(false);

            }//pauseStateStart = enable

            if(pauseStateStart == Action_State.Disable){

                HFPS_UICont.instance.PauseLock_State(true);

            }//pauseStateStart = disable

            if(inventoryStateStart == Action_State.Enable){

                HFPS_UICont.instance.InventoryLock_State(false);

            }//inventoryStateStart = enable

            if(inventoryStateStart == Action_State.Disable){

                HFPS_UICont.instance.InventoryLock_State(true);

            }//inventoryStateStart = disable

            if(pauseStateStart != Action_State.Disable){

                if(saveStateStart == Action_State.Enable){

                    HFPS_UICont.instance.Save_State(true);

                }//saveState = enable

                if(saveStateStart == Action_State.Disable){

                    HFPS_UICont.instance.Save_State(false);

                }//saveState = disable

                if(loadStateStart == Action_State.Enable){

                    HFPS_UICont.instance.Load_State(true);

                }//loadStateStart = enable

                if(loadStateStart == Action_State.Disable){

                    HFPS_UICont.instance.Load_State(false);

                }//loadStateStart = disable

            }//pauseStateStart != disable

            if(useSceneAction){

                if(sceneAction != null){

                    sceneAction.Actions_Init();

                }//sceneAction != null

            }//useSceneAction

            if(actions.Count > 0){

                for(int a = 0; a < actions.Count; a++) {

                    if(actions[a].stateChange != PlayerState.Current){

                        Player_State(actions[a].stateChange);

                    }//stateChange != current

                    if(actions[a].moveType == MoveType.Look){

                        if(actions[a].transform != null){

                            Player_Look(new Vector2(actions[a].transform.localEulerAngles.y, actions[a].transform.localEulerAngles.z), actions[a].speed);

                        //transform != null
                        } else {

                            Player_Look(new Vector2(actions[a].rotation.x, actions[a].rotation.y), actions[a].speed);

                        }//transform != null

                    }//moveType = look

                    if(actions[a].moveType == MoveType.Move){

                        if(actions[a].transform != null){

                            Player_Move(actions[a].transform.position, actions[a].speed);

                        //transform != null
                        } else {

                            Player_Move(actions[a].position, actions[a].speed);

                        }//transform != null

                    }//moveType = move

                    actions[a].actionEvent.Invoke();

                    yield return new WaitForSeconds(actions[a].nextWait);

                }//for a actions

            }//actions.Count > 0

            if(showMouse){

                if(inputType == DM_InternEnums.PlayInput_Type.Keyboard){

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;    

                }//inputType = keyboard

            }//showMouse

            if(attentionStopType == AttentionStop_Type.Auto){

                yield return new WaitForSeconds(attentionTime);

                Attention_Stop();

            }//attentionStopType = auto

        }//Attention_Start

        public void Attention_Stop(){

            events.onAttentionEnd.Invoke();

            StartCoroutine("AttentionStop_Buff");

        }//Attention_Stop

        public IEnumerator AttentionStop_Buff(){

            if(showMouse){

                if(inputType == DM_InternEnums.PlayInput_Type.Keyboard){

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;    

                }//inputType = keyboard

            }//showMouse

            if(playerUnlock == Player_Unlock.Auto){

                yield return new WaitForSeconds(unlockWait);

                Items_Update(true);
                Player_LockState(false);

            }//playerUnlock = auto
            
            #if COMPONENTS_PRESENT

                if(jumpInput == Input_Lock.Lock){

                    refs.playerMan.LockJump_State(false);

                }//jumpInput = lock

                if(stateInput == Input_Lock.Lock){

                    refs.playerMan.LockStateInput_State(false);

                }//stateInput = lock

                if(sprintInput == Input_Lock.Lock){

                    refs.playerMan.SprintLock_State(false);

                }//sprintInput = lock

                if(leanInput == Input_Lock.Lock){

                    refs.playerMan.LeanLock_State(false);

                }//leanInput = lock

                if(zoomInput == Input_Lock.Lock){

                    refs.playerMan.ZoomLock_State(false);

                }//zoomInput = lock

                if(itemUseLock == Input_Lock.Lock){

                    refs.playerMan.WeaponsUseLock_State(false);

                }//itemUseLock = lock

                if(itemZoomLock == Input_Lock.Lock){

                    refs.playerMan.WeaponsZoomLock_State(false);

                }//itemZoomLock = lock
            
            #endif

            if(pauseStateStart != Action_State.Disable){

                if(saveStateEnd == Action_State.Enable){

                    HFPS_UICont.instance.Save_State(true);

                }//saveState = enable

                if(saveStateEnd == Action_State.Disable){

                    HFPS_UICont.instance.Save_State(false);

                }//saveState = disable

                if(loadStateEnd == Action_State.Enable){

                    HFPS_UICont.instance.Load_State(true);

                }//loadStateEnd = enable

                if(loadStateEnd == Action_State.Disable){

                    HFPS_UICont.instance.Load_State(false);

                }//loadStateEnd = disable

            }//pauseStateStart != disable

            if(gameUIStateStart == Action_State.Nothing){

                if(useDisplaySet){
                
                    if(displaySet != ""){
                    
                        HFPS_UICont.instance.DisplaySet_State(true, displaySet);
                    
                    }//displaySet != null
                
                }//useDisplaySet

            }//gameUIStateStart = nothing
            
            if(!useDisplaySet){

                if(gameUIStateEnd == Action_State.Enable){

                    HFPS_GameManager.Instance.gamePanels.MainGamePanel.SetActive(true);

                }//gameUIStateEnd = enable

                if(gameUIStateEnd == Action_State.Disable){

                    HFPS_GameManager.Instance.gamePanels.MainGamePanel.SetActive(false);

                }//gameUIStateEnd = disable
            
            }//!useDisplaySet

            if(pauseStateEnd == Action_State.Enable){

                HFPS_UICont.instance.PauseLock_State(false);

            }//pauseStateEnd = enable

            if(pauseStateEnd == Action_State.Disable){

                HFPS_UICont.instance.PauseLock_State(true);

            }//pauseStateEnd = disable

            if(inventoryStateEnd == Action_State.Enable){

                HFPS_UICont.instance.InventoryLock_State(false);

            }//inventoryStateEnd = enable

            if(inventoryStateEnd == Action_State.Disable){

                HFPS_UICont.instance.InventoryLock_State(true);

            }//inventoryStateEnd = disable

            if(useType == UseType.SingleUse){

                Locked_State(true);

            }//useType = single

            events.onAttentionEndLate.Invoke();

            if(attentionType == Attention_Type.Input){

                if(actionBarSettings.useActionBar){

                    actionBar.ActionBar_State(true);

                }//useActionBar   

            }//attentionType = input

        }//Attention_Stop


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

                if(refs.itemSwitcher != null){

                    if(itemDispEnd == ItemDisplayState.Show){

                        refs.itemSwitcher.SelectSwitcherItem(currentItem);

                        currentItem = -1;

                    }//itemDispEnd = show

                }//itemSwitcher != null

                #if COMPONENTS_PRESENT

                    if(itemInputLock == Input_Lock.Lock){

                        Inventory.Instance.CustomLock_State(false);

                    }//itemInputLock = lock

                #endif

            //show
            } else {

                #if COMPONENTS_PRESENT

                    if(itemInputLock == Input_Lock.Lock){

                        Inventory.Instance.CustomLock_State(true);

                    }//itemInputLock = lock

                #endif

                if(refs.itemSwitcher != null){

                    if(itemDispStart == ItemDisplayState.Hide){

                        if(itemDispEnd == ItemDisplayState.Show){

                            currentItem = refs.itemSwitcher.currentItem;

                        }//itemDispEnd = show

                        #if COMPONENTS_PRESENT

                            refs.itemSwitcher.DeselectItems_Forced();

                        #endif

                    }//itemDispStart = hide

                }//itemSwitcher != null

            }//show

        }//Items_Update


    ///////////////////////////////
    ///
    ///     PLAYER ACTIONS
    ///
    ///////////////////////////////


        public void Player_Look(Vector2 newRotation, float newSpeed){

            refs.mouseLook.LerpLook(newRotation, newSpeed, true);

        }//Player_Look

        public void Player_Move(Vector3 newPosition, float newSpeed){

            #if COMPONENTS_PRESENT

                refs.playCont.StartCoroutine(refs.playCont.MovePlayer(newPosition, newSpeed, false, false));

            #endif

        }//Player_Move

        public void Player_State(PlayerState newState){

            if(newState == PlayerState.Stand){

                if(refs.playCont.characterState != PlayerController.CharacterState.Stand){

                    refs.playCont.characterState = PlayerController.CharacterState.Stand;

                }//characterState != stand

            }//newState = stand

            if(newState == PlayerState.Crouch){

                if(refs.playCont.characterState != PlayerController.CharacterState.Crouch){

                    refs.playCont.characterState = PlayerController.CharacterState.Crouch;

                }//characterState != crouch

            }//newState = stand

            if(newState == PlayerState.Prone){

                if(refs.playCont.characterState != PlayerController.CharacterState.Prone){

                    refs.playCont.characterState = PlayerController.CharacterState.Prone;

                }//characterState != prone

            }//newState = stand

        }//Player_State

        public void Player_LockState(bool state){

            #if COMPONENTS_PRESENT

                refs.playCont.LockMove_State(state);
                refs.mouseLook.LockLook(state);

                refs.playCont.inputMovement = Vector2.zero;
                refs.playCont.inputX = 0;
                refs.playCont.inputY = 0;

            #endif

        }//Player_LockState


    ///////////////////////////////
    ///
    ///     ACTION BAR ACTIONS
    ///
    ///////////////////////////////


        public void ActionBar_Update(bool state){
        
            if(actionBar != null){

                if(state){

                    if(actionBarSettings.actions.Count > 0){
                    
                        actionBar.Actions_Add(actionBarSettings.actions);

                        actionBar.ActionBar_Check();
                        actionBar.ActionBar_StateCheck();

                    }//actions.Count > 0

                //state
                } else {

                    if(actionBarSettings.actions.Count > 0){
                    
                        actionBar.Actions_Clear();

                        actionBar.ActionBar_Check();
                        actionBar.ActionBar_StateCheck();

                    }//actions.Count > 0

                }//state
            
            }//actionBar != null

        }//ActionBar_Update


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


    //////////////////////////////////////
    ///
    ///     CATCH ACTIONS
    ///
    ///////////////////////////////////////


        public void Catch_Refs(HFPS_References newRefs){

            refs = newRefs;

        }//Catch_Refs


    //////////////////////////////////////
    ///
    ///     LOCKED ACTIONS
    ///
    ///////////////////////////////////////


        public void Locked_State(bool state){

            locked = state;

        }//Locked_State


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            locked = (bool)token["locked"];

        }//OnLoad


    }//HFPS_PlayerAttention


}//namespace