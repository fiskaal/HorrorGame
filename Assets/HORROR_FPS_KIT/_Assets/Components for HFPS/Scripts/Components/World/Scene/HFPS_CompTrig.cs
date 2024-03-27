using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Scene/Component Trigger")]
    public class HFPS_CompTrig : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class RefsUnityEvent : UnityEvent<HFPS_References> {

            public HFPS_References refsCatch;

        }//RefsUnityEvent

        [Serializable]
        public class TimedEvents {

            [Space]

            public UnityEvent onTimeStart;
            public UnityEvent onTimeFinish;
            public UnityEvent onReset;
            public UnityEvent onCancel;

        }//TimedEvents


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Trigger_Type {

            Destroyable = 0,
            FallControl = 1,
            PlayerAttention = 2,
            PlayerDamage = 3,
            PlayerHeal = 4,
            PlayerHide = 5,
            Possessed = 6,
            Save = 7,
            ScreenEvent = 8,
            OnlyEvents = 9,

        }//Trigger_Type

        public enum Use_Type {

            Manual = 0,
            Single = 1,
            Multi = 2,

        }//Use_Type

        public enum Heal_Use {

            OnEnter = 0,
            OnExit = 1,
            OnStay = 2,

        }//Heal_Use

        public enum Action_State {

            Nothing = 0,
            Enable = 1, 
            Disable = 2,

        }//Action_State

        public enum HideType {

            Normal = 0,
            TimeLimited = 1,

        }//HideType

        public enum TimeCheckType {

            OnEnter = 0,
            OnStay = 1,

        }//TimeCheckType

        public enum Unlock_Type {

            None = 0,
            Delayed = 1,

        }//Unlock_Type


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     GENERAL
    ///
    ///////////////////////////


        public Trigger_Type triggerType;
        public Heal_Use healUse;
        public Use_Type useType;

        public bool useTriggerDelay;
        public float triggerDelay;

        [TagSelectorAttribute]
        public string detectTag;

        public Unlock_Type unlockType;
        public float unlockDelay;

        public int category;
        public int eventSlot;

        public Collider trigger;

        public HFPS_PlayerAttention hfpsPlayerAttention;
        public HFPS_Possessed hfpsPossessed;
        public HFPS_Destroyable hfpsDestroyable;
        public int damageAmount;

        public HideType hideType;
        public TimeCheckType timeCheckType;

        public float activeTime;
        public float countDownMulti;

        public bool autoReset;
        public float resetWait;

        public bool preLoadScene;


    ///////////////////////////
    ///
    ///     UI
    ///
    ///////////////////////////


        public Action_State pauseStateEnter;
        public Action_State pauseStateExit;

        public Action_State inventoryStateEnter;
        public Action_State inventoryStateExit;

        public Action_State saveStateEnter;
        public Action_State saveStateExit;

        public Action_State loadStateEnter;
        public Action_State loadStateExit;


    ///////////////////////////
    ///
    ///     EVENTS
    ///
    ///////////////////////////


        public UnityEvent onManualTrigger;
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;
        public UnityEvent onHeal;
        public UnityEvent onReset;
        public RefsUnityEvent refsCatch;

        public TimedEvents timedEvents;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public HFPS_References refs;
        public bool healing;
        public bool countDownActive;
        public float currentTime;
        public bool isInside;
        public bool locked;

        public int tabs;

        public bool genOpts;
        public bool uiOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            healing = false;

            countDownActive = false;
            currentTime = 0;

        }//StartInit


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update(){

            if(!locked){

                if(countDownActive){

                    if(currentTime > 0){

                        currentTime -= countDownMulti * Time.deltaTime;

                    //currentTime > 0
                    } else {

                        countDownActive = false;
                        currentTime = 0;

                        CountDown_Finish();

                    }//currentTime > 0

                }//countDownActive

            }//!locked

        }//Update


    //////////////////////////////////////
    ///
    ///     TRIGGER ACTIONS
    ///
    ///////////////////////////////////////    


        public void Manual_Trigger(){

            if(!locked){

                if(useTriggerDelay){

                    StartCoroutine("ManualTrigger_Buff");

                //useTriggerDelay
                } else {

                    onManualTrigger.Invoke();

                }//useTriggerDelay

            }//!locked

        }//Manual_Trigger

        private IEnumerator ManualTrigger_Buff(){

            yield return new WaitForSeconds(triggerDelay);

            onManualTrigger.Invoke();

        }//ManualTrigger_Buff


    //////////////////////////////////////
    ///
    ///     TRIGGER ACTIONS
    ///
    ///////////////////////////////////////


        public void TriggerState_Set(bool state){

            trigger.enabled = state;

        }//TriggerState_Set


    ///////////////////////////
    ///
    ///     ENTER
    ///
    ///////////////////////////


        void OnTriggerEnter(Collider other){

            if(!locked){

                if(useType != Use_Type.Manual){

                    if(other.gameObject.tag == detectTag){

                        if(!isInside){

                            isInside = true;

                            if(refs == null){

                                if(other.gameObject.GetComponent<HFPS_References>() != null){

                                    refs = other.gameObject.GetComponent<HFPS_References>();

                                }//HFPS_References != null

                            }//refs = null

                            onTriggerEnter.Invoke();

                            if(refs != null){

                                refsCatch.refsCatch = refs;
                                refsCatch.Invoke(refsCatch.refsCatch);

                                if(triggerType == Trigger_Type.Destroyable){

                                    Destroyable_Damage();

                                }//triggerType = destroyable

                                #if COMPONENTS_PRESENT

                                    if(triggerType == Trigger_Type.FallControl){

                                        Fall_Control(true);

                                    }//triggerType = fall control
                                    
                                    if(triggerType == Trigger_Type.PlayerAttention){

                                        hfpsPlayerAttention.refs = refs;

                                        if(hfpsPlayerAttention.attentionType == HFPS_PlayerAttention.Attention_Type.Regular){

                                            Attention_Init();

                                        }//attentionType = regular

                                    }//triggerType = player attention

                                #endif

                                if(triggerType == Trigger_Type.PlayerDamage){

                                    Player_Damage();

                                }//triggerType = player heal

                                if(triggerType == Trigger_Type.PlayerHeal){

                                    if(healUse == Heal_Use.OnEnter){

                                        Player_Heal();

                                    }//healUse = onEnter

                                }//triggerType = player heal
                                
                                #if COMPONENTS_PRESENT

                                    if(triggerType == Trigger_Type.PlayerHide){

                                        if(hideType == HideType.Normal){

                                            refs.playerMan.Hide_State(true);

                                        }//hideType = normal

                                        if(hideType == HideType.TimeLimited){

                                            if(timeCheckType == TimeCheckType.OnEnter){

                                                refs.playerMan.Hide_State(true);

                                                if(!countDownActive){

                                                    CountDown_State(true);

                                                }//!countDownActive

                                            }//timeCheckType = on enter

                                        }//hideType = time limited

                                    }//triggerType = player hide
                                
                                #endif

                                if(triggerType == Trigger_Type.Possessed){

                                    Possession_Init();

                                }//triggerType = possessed

                                if(triggerType == Trigger_Type.Save){

                                    HFPS_UICont.instance.Save_Init(preLoadScene);

                                }//triggerType = save

                                if(triggerType == Trigger_Type.ScreenEvent){

                                    ScreenEvent_Init();

                                }//triggerType = screen event

                                if(pauseStateEnter == Action_State.Enable){

                                    HFPS_UICont.instance.PauseLock_State(false);

                                }//pauseStateEnter = enable

                                if(pauseStateEnter == Action_State.Disable){

                                    HFPS_UICont.instance.PauseLock_State(true);

                                }//pauseStateEnter = disable

                                if(inventoryStateEnter == Action_State.Enable){

                                    HFPS_UICont.instance.InventoryLock_State(false);

                                }//inventoryStateEnter = enable

                                if(inventoryStateEnter == Action_State.Disable){

                                    HFPS_UICont.instance.InventoryLock_State(true);

                                }//inventoryStateEnter = disable

                                if(saveStateEnter == Action_State.Enable){

                                    HFPS_UICont.instance.Save_State(true);

                                }//saveStateEnter = enable

                                if(saveStateEnter == Action_State.Disable){

                                    HFPS_UICont.instance.Save_State(false);

                                }//saveStateEnter = disable

                                if(loadStateEnter == Action_State.Enable){

                                    HFPS_UICont.instance.Load_State(true);

                                }//loadStateEnter = enable

                                if(loadStateEnter == Action_State.Disable){

                                    HFPS_UICont.instance.Load_State(false);

                                }//loadStateEnter = disable

                            }//refs != null

                        }//!isInside

                    }//tag = detectTag

                }//useType != Manual

            }//!locked

        }//OnTriggerEnter


    ///////////////////////////
    ///
    ///     STAY
    ///
    ///////////////////////////


        void OnTriggerStay(Collider other){

            if(!locked){

                if(useType != Use_Type.Manual){

                    if(other.gameObject.tag == detectTag){

                        if(isInside){

                            if(refs != null){

                                if(triggerType == Trigger_Type.PlayerAttention){

                                    if(hfpsPlayerAttention.attentionType == HFPS_PlayerAttention.Attention_Type.Input){

                                        if(!hfpsPlayerAttention.actBarShowing){

                                            hfpsPlayerAttention.actBarShowing = true;
                                            hfpsPlayerAttention.ActionBar_Update(true);

                                        }//!actBarShowing

                                    }//attentionType = input

                                }//triggerType = player attention

                                if(triggerType == Trigger_Type.PlayerHeal){

                                    if(healUse == Heal_Use.OnStay){

                                        if(!healing){

                                            Healing_State(true);

                                            Player_Heal();

                                        }//!healing

                                    }//healUse = onStay 

                                }//triggerType = player heal
                                
                                #if COMPONENTS_PRESENT

                                    if(triggerType == Trigger_Type.PlayerHide){

                                        if(hideType == HideType.TimeLimited){

                                            if(timeCheckType == TimeCheckType.OnStay){

                                                refs.playerMan.Hide_State(true);

                                                if(!countDownActive){

                                                    CountDown_State(true);

                                                }//!countDownActive

                                            }//timeCheckType = on stay

                                        }//hideType = time limited

                                    }//triggerType = player hide
                                
                                #endif

                            }//refs != null

                        }//isInside

                    }//tag = detectTag

                }//useType != Manual

            }//!locked

        }//OnTriggerStay


    ///////////////////////////
    ///
    ///     EXIT
    ///
    ///////////////////////////


        void OnTriggerExit(Collider other){

            if(!locked){

                if(useType != Use_Type.Manual){

                    if(other.gameObject.tag == detectTag){

                        if(isInside){

                            isInside = false;

                            onTriggerExit.Invoke();

                            if(refs != null){

                                #if COMPONENTS_PRESENT

                                    if(triggerType == Trigger_Type.FallControl){

                                        Fall_Control(false);

                                    }//triggerType = fall control

                                #endif

                                if(triggerType == Trigger_Type.PlayerAttention){

                                    if(hfpsPlayerAttention.attentionType == HFPS_PlayerAttention.Attention_Type.Input){

                                        if(hfpsPlayerAttention.actBarShowing){

                                            hfpsPlayerAttention.actBarShowing = false;
                                            hfpsPlayerAttention.ActionBar_Update(false);

                                        }//actBarShowing

                                    }//attentionType = input

                                }//triggerType = player attention

                                if(triggerType == Trigger_Type.PlayerHeal){

                                    if(healUse == Heal_Use.OnStay){

                                        if(healing){

                                            Healing_State(false);
                                            Player_HealStop();

                                        }//healing

                                    }//healUse = onStay

                                    if(healUse == Heal_Use.OnExit){

                                        Player_Heal();

                                    }//healUse = onExit

                                }//triggerType = player heal
                                
                                #if COMPONENTS_PRESENT

                                    if(triggerType == Trigger_Type.PlayerHide){

                                        if(hideType == HideType.Normal){

                                            refs.playerMan.Hide_State(false);

                                        }//hideType = normal

                                        if(hideType == HideType.TimeLimited){

                                            refs.playerMan.Hide_State(false);
                                            CountDown_State(false);

                                        }//hideType = time limited

                                    }//triggerType = player hide
                                
                                #endif

                                if(useType == Use_Type.Single){

                                    Locked_State(true);
                                    TriggerState_Set(false);

                                }//useType = single

                                if(useType == Use_Type.Multi){

                                    if(triggerType != Trigger_Type.PlayerHide){

                                        if(unlockType == Unlock_Type.Delayed){

                                            Locked_State(true);

                                            StartCoroutine("Trigger_Buff");

                                        }//unlockType = delayed

                                    }//triggerType != player hide

                                }//useType = multi

                                if(pauseStateExit == Action_State.Enable){

                                    HFPS_UICont.instance.PauseLock_State(false);

                                }//pauseStateExit = enable

                                if(pauseStateExit == Action_State.Disable){

                                    HFPS_UICont.instance.PauseLock_State(true);

                                }//pauseStateExit = disable

                                if(inventoryStateExit == Action_State.Enable){

                                    HFPS_UICont.instance.InventoryLock_State(false);

                                }//inventoryStateExit = enable

                                if(inventoryStateExit == Action_State.Disable){

                                    HFPS_UICont.instance.InventoryLock_State(true);

                                }//inventoryStateExit = disable

                                if(saveStateExit == Action_State.Enable){

                                    HFPS_UICont.instance.Save_State(true);

                                }//saveStateExit = enable

                                if(saveStateExit == Action_State.Disable){

                                    HFPS_UICont.instance.Save_State(false);

                                }//saveStateExit = disable

                                if(loadStateExit == Action_State.Enable){

                                    HFPS_UICont.instance.Load_State(true);

                                }//loadStateExit = enable

                                if(loadStateExit == Action_State.Disable){

                                    HFPS_UICont.instance.Load_State(false);

                                }//loadStateExit = disable

                                refs = null;

                            }//refs != null

                        }//isInside

                    }//tag = detectTag

                }//useType != Manual

            }//!locked

        }//OnTriggerExit


    ///////////////////////////
    ///
    ///     BUFFS
    ///
    ///////////////////////////


        private IEnumerator Trigger_Buff(){

            yield return new WaitForSeconds(unlockDelay);

            Locked_State(false);
            onReset.Invoke();

        }//Trigger_Buff


    //////////////////////////////////////
    ///
    ///     COUNTDOWN ACTIONS
    ///
    ///////////////////////////////////////


        public void CountDown_State(bool state){

            if(state){

                CountDown_Init();

            //state
            } else {

                CountDown_Cancel();

            }//state

        }//CountDown_State

        public void CountDown_StateSolo(bool state){

            countDownActive = state;

        }//CountDown_StateSolo

        public void CountDown_Init(){

            currentTime = activeTime;
            countDownActive = true;

            timedEvents.onTimeStart.Invoke();

        }//CountDown_Init

        public void CountDown_Cancel(){

            countDownActive = false;
            currentTime = 0;

            timedEvents.onCancel.Invoke();

        }//CountDown_Cancel

        public void CountDown_Finish(){

            TriggerState_Set(false);
            
            #if COMPONENTS_PRESENT
            
                refs.playerMan.Hide_State(false);
            
            #endif
            
            isInside = false;

            timedEvents.onTimeFinish.Invoke();

            if(useType == Use_Type.Multi){

                if(autoReset){

                    StartCoroutine("CountDown_Reset");

                }//autoReset

            }//useType = multi

        }//TriggerState_Set

        private IEnumerator CountDown_Reset(){

            yield return new WaitForSeconds(resetWait);

            TriggerState_Set(true);

            timedEvents.onReset.Invoke();

        }//CountDown_Reset


    //////////////////////////////////////
    ///
    ///     DESTROYABLE ACTIONS
    ///
    ///////////////////////////////////////


        public void Destroyable_Damage(){

            hfpsDestroyable.ReceiveDamage(damageAmount);

        }//Destroyable_Damage


    //////////////////////////////////////
    ///
    ///     FALL CONTROL ACTIONS
    ///
    ///////////////////////////////////////


        public void Fall_Control(bool state){

            #if COMPONENTS_PRESENT

                if(refs != null){

                    refs.playCont.NoFallDamage_State(state);

                //refs != null
                } else {

                    HFPS_References.instance.playCont.NoFallDamage_State(state);

                }//refs != null

            #endif

        }//Fall_Control


    //////////////////////////////////////
    ///
    ///     PLAYER ATTENTION ACTIONS
    ///
    ///////////////////////////////////////


        public void Attention_Init(){

            hfpsPlayerAttention.Attention_Init();

        }//Attention_Init


    //////////////////////////////////////
    ///
    ///     PLAYER DAMAGE ACTIONS
    ///
    ///////////////////////////////////////


        public void Player_Damage(){

            if(refs != null){

                refs.healthManager.ApplyDamage(damageAmount);

            //refs != null
            } else {

                HFPS_References.instance.healthManager.ApplyDamage(damageAmount);

            }//refs != null

        }//Player_Damage


    //////////////////////////////////////
    ///
    ///     HEAL ACTIONS
    ///
    ///////////////////////////////////////


        private void Player_Heal(){

            if(!locked){

                #if COMPONENTS_PRESENT

                    if(refs != null){

                        if(refs.healthManager.Health != refs.healthManager.maxRegenerateHealth){

                            refs.healthManager.RegenCoroutine = StartCoroutine(refs.healthManager.Regenerate());

                        }//Health != maxRegenerateHealth

                    //refs != null
                    } else {

                        if(HFPS_References.instance.healthManager.Health != HFPS_References.instance.healthManager.maxRegenerateHealth){

                            HFPS_References.instance.healthManager.RegenCoroutine = StartCoroutine(HFPS_References.instance.healthManager.Regenerate());

                        }//Health != maxRegenerateHealth

                    }//refs != null

                #endif

                onHeal.Invoke();

            }//!locked

        }//Player_Heal

        private void Player_HealStop(){

            #if COMPONENTS_PRESENT

                if(refs != null){

                    if(refs.healthManager.RegenCoroutine != null){

                        StopCoroutine(refs.healthManager.RegenCoroutine);
                        refs.healthManager.Health = Mathf.RoundToInt(refs.healthManager.Health);

                    }//RegenCoroutine != null

                //refs != null
                } else {

                    if(HFPS_References.instance.healthManager.RegenCoroutine != null){

                        StopCoroutine(HFPS_References.instance.healthManager.RegenCoroutine);
                        HFPS_References.instance.healthManager.Health = Mathf.RoundToInt(HFPS_References.instance.healthManager.Health);

                    }//RegenCoroutine != null    

                }//refs != null

            #endif

        }//Player_HealStop

        public void Healing_State(bool state){

            healing = state;

        }//Healing_State


    //////////////////////////////////////
    ///
    ///     POSSESSION ACTIONS
    ///
    ///////////////////////////////////////


        public void Possession_Init(){

            hfpsPossessed.Target_Set(refs.transform);
            hfpsPossessed.Possession_Init();

        }//Possession_Init


    //////////////////////////////////////
    ///
    ///     SCREEN EVENT ACTIONS
    ///
    ///////////////////////////////////////


        public void ScreenEvent_Init(){
        
            #if COMPONENTS_PRESENT

                if(!locked){

                    if(refs != null){

                        refs.screenEvents.Category_Set(category);
                        refs.screenEvents.EventSlot_Set(eventSlot);
                        refs.screenEvents.ScreenEvent_Init();

                    //refs != null
                    } else {

                        HFPS_References.instance.screenEvents.Category_Set(category);
                        HFPS_References.instance.screenEvents.EventSlot_Set(eventSlot);

                        HFPS_ScreenEvents.instance.ScreenEvent_Init();

                    }//refs != null

                }//!locked
            
            #endif

        }//ScreenEvent_Init


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

                {"isInside", isInside },
                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            isInside = (bool)token["isInside"];
            locked = (bool)token["locked"];

            if(locked){

                this.gameObject.SetActive(false);

            }//locked

        }//OnLoad


    }//HFPS_CompTrig


}//namespace