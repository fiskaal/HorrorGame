using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using ThunderWire.Helpers;
using ThunderWire.Input;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Sub Actions/Sub Actions Handler")]
    public class HFPS_SubActionsHandler : MonoBehaviour {


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class Sub_Action {

            [Space]

            public string name;    
            public DM_InternEnumsComp.SubAction_Type type;

            [Space]

            public string display;
            public Sprite icon;

            [Space]

            public HFPS_SubAction action;

            [Space]

            [Header("Auto")]

            public int inputSlot = -1;
            public bool isActive;
            public string actName;

        }//Sub_Action

        [Serializable]
        public class Action_Input {

            [Space]

            public string name;

            [Space]

            public string input;

            [Space]

            [Header("Auto")]

            public bool isActive;
            public bool isPressed;
            public bool isHolding;

        }//Action_Input


    //////////////////////////
    //
    //      ENUMS
    //
    //////////////////////////


        public enum Input_Type {

            Press = 0,
            Hold = 1,

        }//Input_Type


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    //////////////////////////
    //
    //      GENERAL
    //
    //////////////////////////


        public HFPS_References refs;


    //////////////////////////
    //
    //      ACTIONS
    //
    //////////////////////////


        public List<Sub_Action> subActions;
        public List<Action_Input> actionInputs;

        public bool useActionDelay;
        public float actionDelay;


    //////////////////////////
    //
    //      INPUT
    //
    //////////////////////////


        public Input_Type inputType;
        public float holdTime;
        public float holdMulti;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public DM_InternEnums.PlayInput_Type playerInput;

        public int curAction;
        public int tempInt;

        public bool isHolding;
        public float curHoldTime;
        public float tempFill;

        public bool locked;
        public float lockDelay;

        public int tabs;

        public bool genOpts;
        public bool actOpts;
        public bool inputOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            curAction = -1;
            tempInt = -1;
            isHolding = false;
            curHoldTime = 0;
            tempFill = 0;

            Locked_State(true);
            LockDelay_Update(0);

            if(subActions.Count > 0){

                for(int sa = 0; sa < subActions.Count; sa++){

                    subActions[sa].inputSlot = -1;
                    subActions[sa].isActive = false;

                }//for sa subActions

            }//subActions.Count > 0

            if(actionInputs.Count > 0){

                for(int ai = 0; ai < actionInputs.Count; ai++){

                    actionInputs[ai].isActive = false;
                    actionInputs[ai].isPressed = false;
                    actionInputs[ai].isHolding = false;

                }//for ai actionInputs

            }//actionInputs.Count > 0

            StartCoroutine("InputCheckBuff");

        }//StartInit


    //////////////////////////
    //
    //      UPDATE ACTIONS
    //
    //////////////////////////


        void Update() {

            if(!locked){

                if(InputHandler.InputIsInitialized) {

                    InputCheck_Type();

                    if(actionInputs.Count > 0){

                        for(int ai = 0; ai < actionInputs.Count; ai++){

                            if(actionInputs[ai].isActive){

                                actionInputs[ai].isPressed = InputHandler.ReadButton(actionInputs[ai].input);

                                if(inputType == Input_Type.Press){

                                    if(actionInputs[ai].isPressed) {

                                        Locked_State(true);

                                        if(!useActionDelay){

                                            SubAction_Activate(ai);

                                        //useActionDelay
                                        } else {

                                            SubAction_ActivateDelayed(ai);

                                        }//useActionDelay

                                    }//isPressed

                                }//inputType = Press

                                if(inputType == Input_Type.Hold){

                                    if(actionInputs[ai].isPressed) {

                                        tempInt = ai;

                                    }//isPressed

                                }//inputType = Hold

                            }//isActive

                        }//for ai actionInputs

                        if(tempInt > -1){

                            if(actionInputs[tempInt].isPressed) {

                                isHolding = true;

                                if(isHolding){

                                    curHoldTime += holdMulti * Time.deltaTime;

                                    if(HFPS_SubActionsUI.instance != null){

                                        tempFill = curHoldTime / holdTime;

                                        HFPS_SubActionsUI.instance.Fill_Update(tempInt, tempFill);

                                    }//instance != null

                                    if(curHoldTime >= holdTime){

                                        Locked_State(true);

                                        if(!useActionDelay){

                                            SubAction_Activate(tempInt);

                                        //useActionDelay
                                        } else {

                                            SubAction_ActivateDelayed(tempInt);

                                        }//useActionDelay

                                    }//curHoldTime >= holdTime

                                }//isHolding

                            } else if(isHolding) {

                                isHolding = false;

                                curHoldTime = 0;
                                tempFill = 0;

                                HFPS_SubActionsUI.instance.Fill_Update(tempInt, tempFill);

                                tempInt = -1;

                            }//isHolding

                        }//tempInt > -1

                    }//actionInputs.Count > 0

                }//InputIsInitialized

            }//!locked

        }//Update


    //////////////////////////
    //
    //      ACTION ACTIONS
    //
    //////////////////////////


        public void SubAction_Activate(int slot){

            curHoldTime = 0;
            tempFill = 0;
            tempInt = -1;

            isHolding = false;

            for(int sa = 0; sa < subActions.Count; sa++){

                if(subActions[sa].inputSlot == slot){

                    curAction = sa;

                    if(subActions[sa].type != DM_InternEnumsComp.SubAction_Type.Exit){

                        subActions[sa].action.SubAction_Init();
                        UI_Fade(subActions[sa].action.actionAnim.length);

                        LockDelay_Update(subActions[sa].action.actionAnim.length + subActions[sa].action.extraTime);
                        LockState_Delayed(false);

                    }//type != Exit

                    if(subActions[sa].type == DM_InternEnumsComp.SubAction_Type.Exit){

                        refs.charAction.Action_Check();

                    }//type != Exit

                }//inputSlot = slot

            }//for sa subActions

        }//SubAction_Activate

        public void SubAction_ActivateDelayed(int slot){

            StartCoroutine("SubAct_ActDelayBuff", slot);

        }//SubAction_ActivateDelayed

        private IEnumerator SubAct_ActDelayBuff(int slot){

            curHoldTime = 0;
            tempFill = 0;
            tempInt = -1;

            isHolding = false;

            for(int sa = 0; sa < subActions.Count; sa++){

                if(subActions[sa].inputSlot == slot){

                    curAction = sa;

                    if(subActions[sa].type != DM_InternEnumsComp.SubAction_Type.Exit){

                        UI_Fade(subActions[sa].action.actionAnim.length);

                        LockDelay_Update(subActions[sa].action.actionAnim.length + subActions[sa].action.extraTime);
                        LockState_Delayed(false);

                        yield return new WaitForSeconds(actionDelay);

                        subActions[sa].action.SubAction_Init();

                    }//type != Exit

                    if(subActions[sa].type == DM_InternEnumsComp.SubAction_Type.Exit){

                        refs.charAction.Action_Check();

                    }//type != Exit

                }//inputSlot = slot

            }//for sa subActions

        }//SubAct_ActDelayBuff

        public void SubActions_Reset(){

            if(subActions.Count > 0){

                for(int sa = 0; sa < subActions.Count; sa++){

                    subActions[sa].inputSlot = -1;
                    subActions[sa].isActive = false;

                }//for sa subActions

            }//subActions.Count > 0

        }//SubActions_Reset

        public void Inputs_Reset(){

            if(actionInputs.Count > 0){

                for(int ai = 0; ai < actionInputs.Count; ai++){

                    actionInputs[ai].isActive = false;
                    actionInputs[ai].isPressed = false;
                    actionInputs[ai].isHolding = false;

                }//for ai actionInputs

            }//actionInputs.Count > 0

        }//Inputs_Reset


    //////////////////////////
    //
    //      UI ACTIONS
    //
    //////////////////////////


        public void UI_Fade(float delay){

            StartCoroutine("UIFade_Buff", delay);

        }//UI_Fade

        private IEnumerator UIFade_Buff(float delay){

            if(HFPS_SubActionsUI.instance != null){

                HFPS_SubActionsUI.instance.Actions_Hide();

                yield return new WaitForSeconds(delay);

                HFPS_SubActionsUI.instance.Actions_Show();

            }//instance != null

        }//UIFade_Buff


    //////////////////////////
    //
    //      INPUT ACTIONS
    //
    //////////////////////////


        public void InputCheck_Type(){

            if(InputHandler.CurrentDevice == InputHandler.Device.MouseKeyboard) {

                playerInput = DM_InternEnums.PlayInput_Type.Keyboard;

            //deviceType = keyboard
            } else if(InputHandler.CurrentDevice.IsGamepadDevice() > 0) {

                playerInput = DM_InternEnums.PlayInput_Type.Gamepad;

            }//deviceType = gamepad

            HFPS_SubActionsUI.instance.playerInput = playerInput;
            InputCheck_Icons();

        }//InputCheck_Type

        public void InputCheck_Icons(){

            HFPS_SubActionsUI.instance.InputCheck_Icons();

        }//InputCheck_Icons

        private IEnumerator InputCheckBuff(){

            yield return new WaitForSeconds(0.2f);

            if(InputHandler.InputIsInitialized) {

                InputCheck_Type();

            }//InputIsInitialized

        }//InputCheckBuff


    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void Locked_State(bool state){

            locked = state;

        }//Locked_State

        public void LockDelay_Update(float delay){

            lockDelay = delay;

        }//LockDelay_Update

        public void LockState_Delayed(bool state){

            StartCoroutine("LockStateDelay_Buff", state);

        }//LockState_Delayed

        private IEnumerator LockStateDelay_Buff(bool state){

            yield return new WaitForSeconds(lockDelay);

            Locked_State(state);

        }//LockStateDelay_Buff


    }//HFPS_SubActionsHandler


}//namespace