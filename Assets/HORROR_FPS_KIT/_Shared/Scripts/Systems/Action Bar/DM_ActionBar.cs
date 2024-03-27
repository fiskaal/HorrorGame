using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#if EASYHIDE_PRESENT

    using DizzyMedia.HFPS_EasyHide;

#endif

using HFPS.Systems;
using ThunderWire.Input;

#if TW_LOCALIZATION_PRESENT

    using ThunderWire.Localization;

#endif

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Systems/Action Bar/Action Bar")]
    public class DM_ActionBar : MonoBehaviour {


    //////////////////////////
    //
    //      INSTANCE
    //
    //////////////////////////


        public static DM_ActionBar instance;


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class Action {

            [Space]

            public string name;
            public GameObject holder;
            
            [Space]
            
            public Text actionText;
            public string defaultText;

            [Space]

            [Header("Input Type")]
            
            [Space]
            
            public List<Action_Input> actionInputs;

            [Space]

            [Header("Events")]

            [Space]

            public UnityEvent onActionInit;

            [Header("Auto")]

            [Space]

            public bool actionPressed;

        }//Action
        
        [Serializable]
        public class Action_Input {
        
            [Space]
        
            public string name;
            
            [Space]
            
            public InputHandler.Device device;
            public GameObject holder;
            
            [Space]
            
            public string input;
            
            [Space]
            
            public Image image;
            public Sprite defaultIcon;
        
        }//Action_Input
        
        [Serializable]
        public class Action_Custom {
        
            [Space]
        
            public string name;
            
            [Space]
            
            public string text;
            public List<ActionInput_Custom> actionInputs;
            
            [Space]
            
            public UnityEvent actionInit;
            
            [Space]
            
            [Header("Auto")]
            
            public string textDisplay;
        
        }//Action_Custom
        
        [Serializable]
        public class ActionInput_Custom {
        
            [Space]
        
            public string name;
            public InputHandler.Device device;
            
            [Space]
            
            public string input;
            public Sprite icon;
        
        }//ActionInput_Custom

        [Serializable]
        public class Auto {

            [Space]

            public List<Action_Custom> tempActions;
            
            #if EASYHIDE_PRESENT
            
                public List<HideHand> hideHands;

            #endif
            
            [Space]

            public bool pauseKeyPressed;
            public bool pauseBackKeyPressed;
            public bool pausedLocked;
            public bool paused;
            public bool actionsActive;
            public bool buffInput;
            public float lockWait;
            public bool locked;

        }//Auto


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////

    /////////////////
    //
    //   DEBUG
    //
    /////////////////


        public bool useDebug;


    /////////////////
    //
    //   REFERENCES
    //
    /////////////////


        public GameObject holder;
        public List<Action> actions;


    /////////////////
    //
    //   START OPTIONS
    //
    /////////////////


        public bool createInstance;


    /////////////////
    //
    //   INPUT OPTIONS
    //
    /////////////////


        public bool detectPause;
        public bool detectPauseBack;
        public string pauseInput;
        public string pauseBackInput;
        public float inputWait;


    /////////////////
    //
    //   AUTO
    //
    /////////////////


        public InputHandler.Device curDevice;
        public Auto auto;

        public int actBarTabs;
        public int debugInt;

        public bool startOpts;
        public bool inputOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        private void OnEnable() {
        
            if (!InputHandler.HasReference)
                throw new NullReferenceException("The InputHandler component was not found on the scene!");

            InputHandler.OnInputsUpdated += OnInputsUpdated;
        
        }//OnEnable

        private void OnDestroy() {
        
            InputHandler.OnInputsUpdated -= OnInputsUpdated;
        
        }//OnDestroy

        void Awake(){

            if(createInstance){

                instance = this;

            }//createInstance

        }//Awake

        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            if(holder.activeSelf){

                ActionBar_State(false);

            }//activeSelf

            Actions_Clear();
            ActionBar_Defaults();

            auto.buffInput = false;
            auto.actionsActive = false;
            auto.locked = false;

        }//StartInit


    //////////////////////////
    //
    //      UPDATE ACTIONS
    //
    //////////////////////////


        void Update(){

            if(!auto.locked){


    ////////////////
    // PAUSE INPUT CHECK
    ////////////////


                if(detectPause && auto.actionsActive){

                    if(InputHandler.InputIsInitialized) {

                        auto.pauseKeyPressed = InputHandler.ReadButton(pauseInput);

                    }//InputIsInitialized

                    if(auto.actionsActive){

                        if(auto.pauseKeyPressed){

                            PauseCheck();

                        }//pauseKeyPressed

                    }//actionsActive

                }//detectPause
                
                if(detectPauseBack && auto.actionsActive && auto.paused){

                    if(InputHandler.InputIsInitialized) {

                        auto.pauseBackKeyPressed = InputHandler.ReadButton(pauseBackInput);

                    }//InputIsInitialized

                    if(auto.actionsActive){

                        if(auto.pauseBackKeyPressed){

                            BackCheck();

                        }//pauseBackKeyPressed

                    }//actionsActive

                }//detectPauseBack, actionsActive & paused


    ////////////////
    // ACTIONS INPUT CHECK
    ////////////////


                if(!auto.paused && !auto.pausedLocked && !auto.buffInput){

                    if(auto.actionsActive){
                    
                        if(curDevice != InputHandler.Device.None){

                            if(auto.tempActions.Count > 0){

                                for(int i = 0; i < auto.tempActions.Count; ++i ) {

                                    if(auto.tempActions[i].actionInputs[(int)(InputHandler.Device)curDevice - 1].input != ""){

                                        if(InputHandler.InputIsInitialized) {

                                            actions[i].actionPressed = InputHandler.ReadButton(auto.tempActions[i].actionInputs[(int)(InputHandler.Device)curDevice - 1].input);

                                        }//InputIsInitialized

                                        if(actions[i].actionPressed && !auto.buffInput) {

                                            auto.buffInput = true;

                                            Action_Init(i);

                                        }//actionPressed & !buffInput

                                    //input != null
                                    } else if(actions[i].actionInputs[(int)(InputHandler.Device)curDevice - 1].input != ""){

                                        if(InputHandler.InputIsInitialized) {

                                            actions[i].actionPressed = InputHandler.ReadButton(actions[i].actionInputs[(int)(InputHandler.Device)curDevice - 1].input);

                                        }//InputIsInitialized

                                        if(actions[i].actionPressed && !auto.buffInput) {

                                            auto.buffInput = true;

                                            Action_Init(i);

                                        }//actionPressed & !buffInput

                                    }//input != null

                                }//for i tempActions

                            }//tempActions.Count > 0
                        
                        }//curDevice != none

                    }//actionsActive

                }//!paused

            }//!locked

        }//Update
        
        
    //////////////////////////
    //
    //      ACTIONS DEFAULT ACTIONS
    //
    //////////////////////////
    
        
        public void ActionBar_Defaults(){
        
            for(int i = 0; i < actions.Count; ++i ) {

                actions[i].holder.SetActive(false);
                actions[i].actionText.text = actions[i].defaultText;
                
                if(actions[i].actionInputs.Count > 0){
                
                    for(int aci = 0; aci < actions[i].actionInputs.Count; ++aci ) {
                    
                        if(actions[i].actionInputs[aci].image != null){
                            
                            if(actions[i].actionInputs[aci].defaultIcon != null){
                                
                                actions[i].actionInputs[aci].image.sprite = actions[i].actionInputs[aci].defaultIcon;
                                
                            }//defaultIcon != null
                        
                        }//image != null
                    
                    }//for aci actionInputs
                
                }//actionInputs.Count > 0

            }//for i actions
            
        }//ActionBar_Defaults


    //////////////////////////
    //
    //      ACTIONS CHECK ACTIONS
    //
    //////////////////////////


        public void ActionBar_Check(){

            if(!auto.locked){

                InputCheck_Icons();

                if(auto.tempActions.Count > 0){

                    for(int i = 0; i < auto.tempActions.Count; ++i ) {

                        if(auto.tempActions[i].textDisplay != ""){
                        
                            actions[i].actionText.text = auto.tempActions[i].textDisplay;
                        
                        }//textDisplay != null
                        
                        if(auto.tempActions[i].actionInputs.Count > 0){
                        
                            for(int aci = 0; aci < auto.tempActions[i].actionInputs.Count; ++aci ) {

                                if(actions[i].actionInputs[aci].image != null){

                                    if(auto.tempActions[i].actionInputs[aci].icon != null){

                                        actions[i].actionInputs[aci].image.sprite = auto.tempActions[i].actionInputs[aci].icon;

                                    //icon != null
                                    } else {
                                        
                                        actions[i].actionInputs[aci].image.sprite = actions[i].actionInputs[aci].defaultIcon;
                                        
                                    }//icon != null

                                }//image != null

                            }//for aci actionInputs
                        
                        }//actionInputs.Count > 0

                        if(actions[i].actionText.text != ""){

                            actions[i].holder.SetActive(true);

                        //text != null
                        } else {

                            actions[i].holder.SetActive(false);

                        }//text != null

                    }//for i tempActions

                }//tempActions.Count > 0

            }//!locked

        }//ActionBar_Check


    //////////////////////////
    //
    //      STATE ACTIONS
    //
    //////////////////////////


        public void ActionBar_State(bool active){

            holder.SetActive(active);

            auto.actionsActive = active;

            if(useDebug){

                Debug.Log("AB State Actions Active = " + auto.actionsActive);

            }//useDebug

        }//ActionBar_State

        public void ActionBar_StateCheck(){

            if(!auto.locked){

                if(useDebug){

                    Debug.Log("AB State Check Action Count = " + auto.tempActions.Count);

                }//useDebug

                if(auto.tempActions.Count > 0){

                    holder.SetActive(true);

                    auto.actionsActive = true;

                //tempActions.Count > 0
                } else {

                    holder.SetActive(false);

                    auto.actionsActive = false;

                }//tempActions.Count > 0

                if(useDebug){

                    Debug.Log("AB State Check Actions Active = " + auto.actionsActive);

                }//useDebug

            }//!locked

        }//ActionBar_State

        public void ActionBar_StateCheckDelayed(float delay){

            StartCoroutine("StateCheckDelayed", delay);

        }//ActionBar_StateCheckDelayed

        public IEnumerator StateCheckDelayed(float delay){

            yield return new WaitForSeconds(delay);

            if(!auto.locked){

                if(auto.tempActions.Count > 0){

                    holder.SetActive(true);

                    auto.actionsActive = true;

                //tempActions.Count > 0
                } else {

                    holder.SetActive(false);

                    auto.actionsActive = false;

                }//tempActions.Count > 0

                if(useDebug){

                    Debug.Log("AB State Check Delay Actions Active = " + auto.actionsActive);

                }//useDebug

            }//!locked

        }//StateCheckDelayed


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
    
        
        private void OnInputsUpdated(InputHandler.Device device, ActionBinding[] bindings){
        
            //Debug.Log("Action Bar - Device Changed");

            curDevice = device;
            
            InputCheck_Icons();
        
        }//OnInputsUpdated
        
        private void InputCheck_Icons(){
        
            if(curDevice != InputHandler.Device.None){

                if(actions.Count > 0){

                    for(int i = 0; i < actions.Count; ++i ) {

                        for(int aci = 0; aci < actions[i].actionInputs.Count; ++aci ) {

                            if(actions[i].actionInputs[aci].holder != null){

                                actions[i].actionInputs[aci].holder.SetActive(false);

                            }//holder != null

                        }//for aci actionInputs

                        if(actions[i].actionInputs[(int)(InputHandler.Device)curDevice - 1].holder != null){

                            actions[i].actionInputs[(int)(InputHandler.Device)curDevice - 1].holder.SetActive(true);

                        }//holder != null

                    }//for i tempActions

                }//actions.Count > 0
            
            }//curDevice != none

        }//InputCheck_Icons


    /////////////////
    //
    //      INPUT ACTIONS
    //
    /////////////////


        public void Action_Init(int slot){

            actions[slot].onActionInit.Invoke();
            auto.tempActions[slot].actionInit.Invoke();

            StartCoroutine("BuffInput", slot);

        }//Action_Init


    /////////////////
    //
    //      INPUT BUFFS
    //
    /////////////////


        public IEnumerator BuffInput(int slot){

            yield return new WaitForSeconds(inputWait);

            auto.buffInput = false;
            actions[slot].actionPressed = false;

        }//BuffInput


    //////////////////////////
    //
    //      RESET ACTIONS
    //
    //////////////////////////


        public void ActionBar_Reset(){

            Actions_Clear();
            ActionBar_Defaults();

            for(int i = 0; i < actions.Count; ++i ) {

                actions[i].holder.SetActive(false);
                actions[i].actionPressed = false;

            }//for i actions

            auto.buffInput = false;
            auto.actionsActive = false;
            auto.locked = false;

        }//ActionBar_Reset
        
        
    //////////////////////////
    //
    //      HIDE HAND ACTIONS
    //
    //////////////////////////
    
    
        #if EASYHIDE_PRESENT

            public void HideHand_Add(HideHand newHideHand){

                if(!auto.hideHands.Contains(newHideHand)){

                    auto.hideHands.Add(newHideHand);

                }//!Contains

            }//HideHand_Add

            public void HideHand_Remove(HideHand newHideHand){

                if(auto.hideHands.Contains(newHideHand)){

                    auto.hideHands.Remove(newHideHand);

                }//Contains

            }//HideHand_Remove

            public void HideHands_Clear(){

                auto.hideHands = new List<HideHand>();

            }//HideHands_Clear
        
        #endif
        
        
    //////////////////////////
    //
    //      ACTION ACTIONS
    //
    //////////////////////////
    
    
        public void Actions_Add(List<Action_Custom> newActs){
        
            List<string> keys = new List<string>();
        
            for(int i = 0; i < newActs.Count; ++i ) {
        
                if(!auto.tempActions.Contains(newActs[i])){

                    auto.tempActions.Add(newActs[i]);

                }//!Contains
            
            }//for i newActs
            
            if(auto.tempActions.Count > 0){
            
                for(int ta = 0; ta < auto.tempActions.Count; ++ta ) {
                
                    auto.tempActions[ta].textDisplay = TextsSource.GetText(auto.tempActions[ta].text, "ActionBar Action");
                    
                    if(!keys.Contains(auto.tempActions[ta].text)){
                    
                        keys.Add(auto.tempActions[ta].text);
                
                    }//!Contains
                    
                }//for ta tempActions
                
                #if TW_LOCALIZATION_PRESENT
                
                    if(HFPS_GameManager.LocalizationEnabled){
                
                        LocalizationSystem.SubscribeAndGet(OnChangeLocalization, keys.ToArray());
            
                    }//LocalizationEnabled
            
                #endif
            
            }//tempActions.Count > 0
        
        }//Actions_Add
        
        public void Actions_Clear(){

            auto.tempActions = new List<Action_Custom>();
            
            #if TW_LOCALIZATION_PRESENT
            
                if(HFPS_GameManager.LocalizationEnabled){
            
                    OnUnsubscribe();
                
                }//LocalizationEnabled

            #endif

        }//Actions_Clear
        

    //////////////////////////
    //
    //      PAUSE ACTIONS
    //
    //////////////////////////


        public void PauseCheck(){

            auto.paused = HFPS_GameManager.Instance.isPaused;

            if(auto.actionsActive){

                if(auto.paused){

                    holder.SetActive(false);
                    
                    auto.pausedLocked = true;

                //paused
                } else {

                    holder.SetActive(true);
                    
                    StartCoroutine("Pause_Buff");

                }//paused

            }//actionsActive

        }//PauseCheck
        
        public void BackCheck(){
        
            if(auto.paused){

                if(auto.actionsActive){
                
                    auto.paused = HFPS_GameManager.Instance.isPaused;

                    if(!auto.paused){

                        auto.pausedLocked = true;
                        auto.buffInput = true;

                        holder.SetActive(true);

                        StartCoroutine("Pause_Buff");

                    }//!paused

                }//actionsActive
            
            }//paused
            
        }//BackCheck
        
        private IEnumerator Pause_Buff(){
        
            yield return new WaitForSeconds(0.1f);
            
            auto.pauseBackKeyPressed = false;
            auto.pausedLocked = false;
            auto.buffInput = false;
            
        }//Pause_Buff
        
        
    //////////////////////////
    //
    //      GET ACTIONS
    //
    //////////////////////////
        
        
        public bool ActionsActive_Get(){
        
            return auto.actionsActive;
            
        }//ActionsActive_Get


    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void Lock_State(bool newLock){

            auto.locked = newLock;

            if(useDebug){

                Debug.Log("Lock State = " + auto.locked);

            }//useDebug

        }//Lock_State

        public void Lock_DelayUpdate(float delay){

            auto.lockWait = delay;

        }//Lock_DelayUpdate

        public void Lock_StateDelay(bool newLock){

            StopCoroutine("LockDelay");
            StartCoroutine("LockDelay", newLock);

        }//Lock_StateDelay

        private IEnumerator LockDelay(bool newLock){

            yield return new WaitForSeconds(auto.lockWait);

            auto.locked = newLock;

            if(useDebug){

                Debug.Log("Lock State Delay = " + auto.locked);

            }//useDebug

        }//LockDelay
        
        
    //////////////////////////
    //
    //      LOCALIZATION ACTIONS
    //
    //////////////////////////
        
        
        #if TW_LOCALIZATION_PRESENT
        
            void OnChangeLocalization(string[] texts) {

                if(auto.tempActions.Count > 0){

                    for(int ta = 0; ta < auto.tempActions.Count; ++ta ) {

                        if(auto.tempActions[ta].text != ""){
                        
                            auto.tempActions[ta].textDisplay = texts[ta];

                        }//text != null
                        
                        if(auto.tempActions[ta].textDisplay != ""){
                        
                            actions[ta].actionText.text = auto.tempActions[ta].textDisplay;
                        
                        }//textDisplay != null
                        
                    }//for ta tempActions

                }//tempActions.Count > 0
            
            }//OnChangeLocalization
            
            void OnUnsubscribe(){
            
                LocalizationSystem.Unsubscribe(this);
            
            }//OnUnsubscribe
        
        #endif
        

    }//DM_ActionBar


}//namespace
