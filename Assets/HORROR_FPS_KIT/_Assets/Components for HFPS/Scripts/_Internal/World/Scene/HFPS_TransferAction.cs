using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DizzyMedia.Shared;

namespace DizzyMedia.HFPS_Components {

    public class HFPS_TransferAction : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_TransferAction instance;


    //////////////////////////////////////
    ///
    ///     INTERNAL VALUES
    ///
    ///////////////////////////////////////


        [SerializeField] private HFPS_CharacterAction.ActionType actionType;
        [SerializeField] private string actionName = "";
        [SerializeField] private float oldRadius = -1;
        [SerializeField] private int currentItem = -1;

        [Space]

        [SerializeField] private bool pauseLockState = false;
        [SerializeField] private bool inventoryLockState = false;
        [SerializeField] private bool saveState = true;
        [SerializeField] private bool loadState = true;

        [Space]

        [SerializeField] private bool lockMoveX = false;
        [SerializeField] private bool lockMoveY = false;
        [SerializeField] private bool lockJump = false;
        [SerializeField] private bool lockStateInput = false;
        [SerializeField] private bool lockLean = false;
        [SerializeField] private bool lockZoom = false;
        [SerializeField] private bool lockWeapZoom = false;
        [SerializeField] private bool lockItemUse = false;
        [SerializeField] private bool lockItemSwitch = false;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

            DontDestroyOnLoad(this.gameObject);

        }//Awake


    //////////////////////////////////////
    ///
    ///     ACTION DATA ACTIONS
    ///
    ///////////////////////////////////////


        public void ActionData_Catch(HFPS_CharacterAction.ActionType newType, string name, float radius, int item, bool pauseLockState, bool inventoryLockState, bool saveState, bool loadState, bool lockMoveX, bool lockMoveY, bool lockJump, bool lockStateInput, bool lockLean, bool lockZoom, bool lockWeapZoom, bool lockItemUse, bool lockItemSwitch){

            ActionType_Set(newType);
            ActionName_Set(name);
            OldRadius_Set(radius);
            CurrentItem_Set(item);

            PauseLock_State(pauseLockState);
            InventoryLock_State(inventoryLockState);
            Save_State(saveState);
            Load_State(loadState);

            LockMoveX_State(lockMoveX);
            LockMoveY_State(lockMoveY);
            LockJump_State(lockJump);
            LockStateInput_State(lockStateInput);
            LockLean_State(lockLean);
            LockZoom_State(lockZoom);
            LockWeapZoom_State(lockWeapZoom);
            LockItemUse_State(lockItemUse);
            LockItemSwitch_State(lockItemSwitch);

        }//ActionData_Catch


    //////////////////////////////////////
    ///
    ///     ACTION TYPE ACTIONS
    ///
    ///////////////////////////////////////


        public void ActionType_Set(HFPS_CharacterAction.ActionType newType){

            actionType = newType;

        }//ActionType_Set

        public HFPS_CharacterAction.ActionType ActionType_Get(){

            return actionType;

        }//ActionType_Get


    //////////////////////////////////////
    ///
    ///     NAME ACTIONS
    ///
    ///////////////////////////////////////


        public void ActionName_Set(string name){

            actionName = name;

        }//ActionName_Set

        public string ActionName_Get(){

            return actionName;

        }//ActionName_Get


    //////////////////////////////////////
    ///
    ///     RADIUS ACTIONS
    ///
    ///////////////////////////////////////


        public void OldRadius_Set(float radius){

            oldRadius = radius;

        }//OldRadius_Set

        public float OldRadius_Get(){

            return oldRadius;

        }//OldRadius_Get


    //////////////////////////////////////
    ///
    ///     ITEM ACTIONS
    ///
    ///////////////////////////////////////


        public void CurrentItem_Set(int item){

            currentItem = item;

        }//CurrentItem_Set

        public int CurrentItem_Get(){

            return currentItem;

        }//CurrentItem_Get

        public void LockItemUse_State(bool state){

            lockItemUse = state;

        }//LockItemUse_State

        public bool LockItemUse_Get(){

            return lockItemUse;

        }//LockItemUse_Use

        public void LockItemSwitch_State(bool state){

            lockItemSwitch = state;

        }//LockItemSwitch_State

        public bool LockItemSwitch_Get(){

            return lockItemSwitch;

        }//LockITemSwitch_Get


    //////////////////////////////////////
    ///
    ///     PAUSE ACTIONS
    ///
    ///////////////////////////////////////


        public void PauseLock_State(bool state){

            pauseLockState = state;

        }//PauseLock_State

        public bool PauseLockState_Get(){

            return pauseLockState;

        }//PauseLockState_Get


    //////////////////////////////////////
    ///
    ///     INVENTORY ACTIONS
    ///
    ///////////////////////////////////////


        public void InventoryLock_State(bool state){

            inventoryLockState = state;

        }//InventoryLock_State

        public bool InventoryLockState_Get(){

            return inventoryLockState;

        }//InventoryLockState_Get


    //////////////////////////////////////
    ///
    ///     SAVE ACTIONS
    ///
    ///////////////////////////////////////


        public void Save_State(bool state){

            saveState = state;

        }//Save_State

        public bool SaveState_Get(){

            return saveState;

        }//SaveState_Get


    //////////////////////////////////////
    ///
    ///     LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public void Load_State(bool state){

            loadState = state;

        }//Load_State

        public bool LoadState_Get(){

            return loadState;

        }//LoadState_Get


    //////////////////////////////////////
    ///
    ///     MOVE ACTIONS
    ///
    ///////////////////////////////////////


        public void LockMoveX_State(bool state){

            lockMoveX = state;

        }//LockMoveX_State

        public bool LockMoveX_Get(){

            return lockMoveX;

        }//LockMoveX_Get

        public void LockMoveY_State(bool state){

            lockMoveY = state;

        }//LockMoveY_State

        public bool LockMoveY_Get(){

            return lockMoveY;

        }//LockMoveY_Get


    //////////////////////////////////////
    ///
    ///     PLAYER STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void LockJump_State(bool state){

            lockJump = state;

        }//LockJump_State

        public bool LockJump_Get(){

            return lockJump;

        }//LockJump_Get

        public void LockStateInput_State(bool state){

            lockStateInput = state;

        }//LockStateInput_State

        public bool LockStateInput_Get(){

            return lockStateInput;

        }//LockStateInput_Get

        public void LockLean_State(bool state){

            lockLean = state;

        }//LockLean_State

        public bool LockLean_Get(){

            return lockLean;

        }//LockLean_Get


    //////////////////////////////////////
    ///
    ///     ZOOM ACTIONS
    ///
    ///////////////////////////////////////


        public void LockZoom_State(bool state){

            lockZoom = state;

        }//LockZoom_State

        public bool LockZoom_Get(){

            return lockZoom;

        }//LockZoom_Get

        public void LockWeapZoom_State(bool state){

            lockWeapZoom = state;

        }//LockWeapZoom_State

        public bool LockWeapZoom_Get(){

            return lockWeapZoom;

        }//LockWeapZoom_Get


    //////////////////////////////////////
    ///
    ///     DESTROY ACTIONS
    ///
    ///////////////////////////////////////


        void OnDestroy(){

            instance = null;

        }//OnDestroy


    }//HFPS_TransferAction

    
}//namespace