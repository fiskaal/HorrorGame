using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using HFPS.Systems;
using ThunderWire.Input;

[AddComponentMenu("Dizzy Media/Components for HFPS/Input/Input Action")]
public class HFPS_InputAction : MonoBehaviour {
    
    
//////////////////////////
//
//      CLASSES
//
//////////////////////////
    
    
    [Serializable]
    public class Input {
        
        [Space]
        
        public string name;
        public string actionInput;
        public InputReceive_Type receiveType;
        public Input_Direction direction;
        public InputTrigger_Type triggerType;
        
        [Space]
        
        public bool requireItem;
        
        [Space]
        
        [InventorySelector]
        public int itemID;
        
        [Header("Events")]
        
        [Space]
        
        public UnityEvent onInputPressed;
        public UnityEvent onInputRelease;
        
        [Header("Auto")]
        
        public bool active;
        public bool isPressed;
        public bool itemChecked;
        public bool itemPresent;
        
    }//Input
    
    [Serializable]
    public class Auto {
        
        [Space]
        
        public int tempSlot;
        public bool canDetect;
        public bool inputsActive;
        public bool pauseKeyPressed;
        public bool paused;
        public bool pausedLocked;
        public bool buffInput;
        public float lockWait;
        public bool locked;
        
    }//Auto
    
    
//////////////////////////
//
//      ENUMS
//
//////////////////////////
    
    
    public enum InputReceive_Type {
        
        Button = 0,
        Vector = 1,
        
    }//InputReceive_Type
    
    public enum InputTrigger_Type {
        
        Press = 0,
        Hold = 1,
        
    }//InputTrigger_Type
    
    public enum Input_Direction {
    
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
    
    }//Input_Direction
    
    
//////////////////////////
//
//      VALUES
//
//////////////////////////
    
/////////////////
//
//   INPUT OPTIONS
//
/////////////////

    
    public bool detectPause;
    public string pauseInput;
    public float inputWait;
    
    public List<Input> inputs;
    public Auto auto;
    
    private Inventory inventory;
    
    
//////////////////////////
//
//      START ACTIONS
//
//////////////////////////

    
    void Awake(){
        
        inventory = Inventory.Instance;
        
    }//Awake

    void Start() {
        
        StartInit();
        
    }//Start
    
    public void StartInit(){
    
        auto.tempSlot = -1;
        auto.canDetect = false;
        auto.buffInput = false;
        auto.inputsActive = false;
        
        Locked_State(false);        
        InputsAll_ActiveState(false);

    }//StartInit


//////////////////////////
//
//      UPDATE ACTIONS
//
//////////////////////////
    

    void Update() {
    
        if(!auto.locked){
            
            
////////////////
// PAUSE INPUT CHECK
////////////////

            
            if(detectPause){
        
                if(InputHandler.InputIsInitialized) {

                    auto.pauseKeyPressed = InputHandler.ReadButton(pauseInput);

                }//InputIsInitialized
                
                if(auto.inputsActive){
                
                    if(auto.pauseKeyPressed){

                        if(!auto.pausedLocked){

                            PauseCheck();

                        }//!pausedLocked

                    }//pauseKeyPressed & menuOpen

                    if(detectPause){

                        if(!auto.pauseKeyPressed){

                            auto.pausedLocked = false;

                        }//!pauseKeyPressed

                    }//detectPause
                
                }//inputsActive
            
            }//detectPause
            
            
////////////////
// ACTIONS INPUT CHECK
////////////////

            
            if(!auto.paused){
                
                if(auto.inputsActive){

                    if(inputs.Count > 0){

                        for(int i = 0; i < inputs.Count; ++i ) {

                            if(inputs[i].actionInput != ""){
                                
                                if(inputs[i].active){
                                
                                    if(inputs[i].requireItem){

                                        if(!inputs[i].itemChecked){

                                            if(inventory.CheckItemInventory(inputs[i].itemID)) {

                                                inputs[i].itemChecked = true;
                                                inputs[i].itemPresent = true;

                                                auto.canDetect = true;

                                            //item is present
                                            } else {

                                                inputs[i].itemPresent = false;
                                                inputs[i].itemChecked = true;

                                                auto.canDetect = false;

                                            }//item is present

                                        }//!itemChecked

                                    //requireItem
                                    } else {

                                        auto.canDetect = true;

                                    }//requireItem
                                    
                                }//active
                                
                                
                                
                                
////////////////
// VECTOR CHECK
////////////////
                                
                                if(inputs[i].active && auto.canDetect){
                                
                                    if(inputs[i].receiveType == InputReceive_Type.Vector){

                                        if(InputHandler.InputIsInitialized) {

                                            if(inputs[i].active){

                                                Vector2 inputVect;

                                                if((inputVect = InputHandler.ReadInput<Vector2>(inputs[i].actionInput)) != null){

                                                    if(inputs[i].direction != Input_Direction.None){

                                                        if(InputHandler.CurrentDevice != InputHandler.Device.MouseKeyboard) {

                                                            if(inputs[i].direction == Input_Direction.Up){

                                                                if(inputVect.y > 0.000001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.y > 0.000001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.y == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//y = 0

                                                                }//isPressed

                                                            }//direction = Up

                                                            if(inputs[i].direction == Input_Direction.Down){

                                                                if(inputVect.y < -0.0000001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.y < -0.0000001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.y == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//y = 0

                                                                }//isPressed

                                                            }//direction = Down

                                                            if(inputs[i].direction == Input_Direction.Left){

                                                                if(inputVect.x < -0.000001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.x < -0.000001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.x == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//x = 0

                                                                }//isPressed

                                                            }//direction = Left

                                                            if(inputs[i].direction == Input_Direction.Right){

                                                                if(inputVect.x > 0.000001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.x > 0.000001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.x == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//x = 0

                                                                }//isPressed

                                                            }//direction = Right

                                                        //currentDevice != mouseKeyboard
                                                        } else {

                                                            if(inputs[i].direction == Input_Direction.Up){

                                                                if(inputVect.y > 0.001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.y > 0.001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.y == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//y = 0

                                                                }//isPressed

                                                            }//direction = Up

                                                            if(inputs[i].direction == Input_Direction.Down){

                                                                if(inputVect.y < -0.001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.y < -0.001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.y == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//y = 0

                                                                }//isPressed

                                                            }//direction = Down

                                                            if(inputs[i].direction == Input_Direction.Left){

                                                                if(inputVect.x < -0.001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.x < -0.001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.x == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//x = 0

                                                                }//isPressed

                                                            }//direction = Left

                                                            if(inputs[i].direction == Input_Direction.Right){

                                                                if(inputVect.x > 0.001 && !auto.buffInput){

                                                                    auto.buffInput = true;
                                                                    inputs[i].isPressed = true;

                                                                    Input_Start(i);

                                                                }//inputVect.x > 0.001

                                                                if(inputs[i].isPressed){

                                                                    if(inputVect.x == 0){

                                                                        inputs[i].isPressed = false;

                                                                        Input_End(i);

                                                                    }//x = 0

                                                                }//isPressed

                                                            }//direction = Right

                                                        }//currentDevice != mouseKeyboard

                                                    }//direction != none

                                                }//inputVect

                                            }//active

                                        }//InputIsInitialized

                                    }//receiveType = vector
                                
                                
////////////////
// BUTTON CHECK
////////////////
                                
                                
                                    if(inputs[i].receiveType == InputReceive_Type.Button){

                                        if(InputHandler.InputIsInitialized) {

                                            if(inputs[i].active){

                                                inputs[i].isPressed = InputHandler.ReadButton(inputs[i].actionInput);

                                            }//active

                                        }//InputIsInitialized

                                        if(inputs[i].isPressed && !auto.buffInput) {

                                            auto.buffInput = true;

                                            Input_Start(i);

                                        }//actionPressed & !buffInput

                                        if(inputs[i].triggerType == InputTrigger_Type.Hold){

                                            if(!inputs[i].isPressed && auto.buffInput) {

                                                auto.buffInput = false;

                                                Input_End(i);

                                            }//buffInput & !isPressed

                                        }//triggerType = hold

                                    }//receiveType = button
                                
                                }//canDetect

                            }//actionInput != null

                        }//for i inputs

                    }//inputs.Count > 0
                    
                }//inputsActive
                
            }//!paused
            
        }//!locked
        
    }//update
    
    
/////////////////
//
//      INPUT ACTIONS
//
/////////////////
    

    public void InputsActive_State(bool state){
        
        auto.inputsActive = state;
        
    }//InputsActive_State
    
    public void Input_Slot(int slot){
        
        auto.tempSlot = slot;
        
    }//Input_Slot
    
    public void Input_Activate(bool state){
        
        inputs[auto.tempSlot - 1].active = state;
        
        if(!state){
                
            inputs[auto.tempSlot - 1].isPressed = false;
            inputs[auto.tempSlot - 1].itemChecked = false;
            inputs[auto.tempSlot - 1].itemPresent = false;
            
            auto.canDetect = false;
            
        }//!state
        
    }//Input_Active
    
    public void InputsAll_ActiveState(bool state){
       
        for(int i = 0; i < inputs.Count; ++i ) {
         
            if(!state){
                
                if(inputs[i].active && inputs[i].isPressed){
                    
                    inputs[i].onInputRelease.Invoke();
                    
                }//active & isPressed
                
            }//!state
            
            inputs[i].active = state;
            
            if(!state){
                
                inputs[i].isPressed = false;
                inputs[i].itemChecked = false;
                inputs[i].itemPresent = false;
                
                auto.canDetect = false;
            
            }//!state
            
        }//for i inputs
        
    }//InputsAll_ActiveState
    
    private void Input_Start(int slot){
        
        inputs[slot].onInputPressed.Invoke();
    
        if(inputs[slot].triggerType == InputTrigger_Type.Press){
            
            StartCoroutine("BuffInput", slot);
    
        }//triggerType = press
        
    }//Input_Start
    
    private void Input_End(int slot){
        
        inputs[slot].onInputRelease.Invoke();
        
        inputs[slot].isPressed = false;
        auto.buffInput = false;
        
    }//Input_End
    
    
/////////////////
//
//      INPUT BUFFS
//
/////////////////

    
    private IEnumerator BuffInput(int slot){
        
        yield return new WaitForSeconds(inputWait);
        
        auto.buffInput = false;
        inputs[slot].isPressed = false;
        
    }//BuffInput
    
    
//////////////////////////
//
//      PAUSE ACTIONS
//
//////////////////////////

    
    private void PauseCheck(){
    
        auto.paused = HFPS_GameManager.Instance.isPaused;
    
    }//PauseCheck
    
    
//////////////////////////
//
//      LOCK ACTIONS
//
//////////////////////////
    
    
    public void Locked_State(bool state){
        
        auto.locked = state;
        
    }//Locked_State
    
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
        
    }//LockDelay
    
    
}//HFPS_InputAction
