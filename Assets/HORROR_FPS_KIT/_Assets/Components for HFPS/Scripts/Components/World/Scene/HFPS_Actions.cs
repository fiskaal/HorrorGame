using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Scene/Actions")]
    public class HFPS_Actions : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_Actions instance;


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Action {

            [Space]

            public string name;
            public HFPS_CharacterAction action;

        }//Action

        [Serializable]
        public class Auto {

            [Space]

            public string tempName = "";
            public float tempRadius = -1;
            public int tempItem = -1;

            [Space]

            public bool tempPauseLock = false;
            public bool tempInvLock = false;
            public bool tempSaveState = true;
            public bool tempLoadState = true;

            [Space]

            public bool tempLockMoveX = false;
            public bool tempLockMoveY = false;
            public bool tempLockJump = false;
            public bool tempLockStateInput = false;
            public bool tempLockLean = false;
            public bool tempLockZoom = false;
            public bool tempLockWeapZoom = false;
            public bool tempLockItemUse = false;
            public bool tempLockItemSwitch = false;

            [Space]

            public bool locked;

        }//Auto


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public bool useStartDelay;
        public float startWait;

        public List<Action> actions = new List<Action>();

        public HFPS_CharacterAction tempAction;
        public HFPS_CharacterAction.ActionType tempType;

        public Auto auto;

        public bool locked;

        public int tabs;

        public bool genOpts;
        public bool actOpts;
        public bool autoFoldShow;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

        }//awake

        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            tempAction = null;

            auto.tempName = "";
            auto.tempRadius = -1;
            auto.tempItem = -1;

            auto.tempPauseLock = false;
            auto.tempInvLock = false;
            auto.tempSaveState = true;
            auto.tempLoadState = true;

            auto.tempLockMoveX = false;
            auto.tempLockMoveY = false;
            auto.tempLockJump = false;
            auto.tempLockStateInput = false;
            auto.tempLockLean = false;
            auto.tempLockZoom = false;
            auto.tempLockWeapZoom = false;
            auto.tempLockItemUse = false;
            auto.tempLockItemSwitch = false;

            if(useStartDelay){

                StartCoroutine("StartBuff");

            //useStartDelay
            } else {

                Actions_Check();

            }//useStartDelay

        }//StartInit

        private IEnumerator StartBuff(){

            yield return new WaitForSeconds(startWait);

            Actions_Check();

        }//StartBuff


    //////////////////////////////////////
    ///
    ///     ACTION ACTIONS
    ///
    ///////////////////////////////////////


        public void Actions_Check(){

            bool present = false;

            if(HFPS_TransferAction.instance != null){

                tempType = HFPS_TransferAction.instance.ActionType_Get();

                auto.tempName = HFPS_TransferAction.instance.ActionName_Get();
                auto.tempRadius = HFPS_TransferAction.instance.OldRadius_Get();
                auto.tempItem = HFPS_TransferAction.instance.CurrentItem_Get();

                auto.tempPauseLock = HFPS_TransferAction.instance.PauseLockState_Get();
                auto.tempInvLock = HFPS_TransferAction.instance.InventoryLockState_Get();
                auto.tempSaveState = HFPS_TransferAction.instance.SaveState_Get();
                auto.tempLoadState = HFPS_TransferAction.instance.LoadState_Get();

                auto.tempLockMoveX = HFPS_TransferAction.instance.LockMoveX_Get();
                auto.tempLockMoveY = HFPS_TransferAction.instance.LockMoveY_Get();
                auto.tempLockJump = HFPS_TransferAction.instance.LockJump_Get();
                auto.tempLockStateInput = HFPS_TransferAction.instance.LockStateInput_Get();
                auto.tempLockLean = HFPS_TransferAction.instance.LockLean_Get();
                auto.tempLockZoom = HFPS_TransferAction.instance.LockZoom_Get();
                auto.tempLockWeapZoom = HFPS_TransferAction.instance.LockWeapZoom_Get();
                auto.tempLockItemUse = HFPS_TransferAction.instance.LockItemUse_Get();
                auto.tempLockItemSwitch = HFPS_TransferAction.instance.LockItemSwitch_Get();

            }//instance != null

            if(auto.tempName != null){

                if(actions.Count > 0){

                    for(int i = 0; i < actions.Count; i++){

                        if(!present){

                            if(actions[i].name == auto.tempName){

                                present = true;
                                tempAction = actions[i].action;

                            }//name = tempName

                        }//!present

                    }//for i actions

                }//actions.Count > 0

                if(tempAction != null){

                    StartCoroutine("ActionTransfer_Init");
                    StartCoroutine("TransferAction_Destroy");

                }//tempAction != null

            }//tempName != null

        }//Actions_Check

        private IEnumerator ActionTransfer_Init(){

            if(tempAction != null){

                tempAction.auto.actionTransferred = true;

                HFPS_UICont.instance.PauseLock_State(auto.tempPauseLock);
                HFPS_UICont.instance.InventoryLock_State(auto.tempInvLock);
                HFPS_UICont.instance.Save_State(auto.tempSaveState);
                HFPS_UICont.instance.Load_State(auto.tempLoadState);
                
                #if COMPONENTS_PRESENT

                    tempAction.auto.refs.playerMan.LockMoveX_State(auto.tempLockMoveX);
                    tempAction.auto.refs.playerMan.LockMoveY_State(auto.tempLockMoveY);
                    tempAction.auto.refs.playerMan.LockJump_State(auto.tempLockJump);
                    tempAction.auto.refs.playerMan.LockStateInput_State(auto.tempLockStateInput);
                    tempAction.auto.refs.playerMan.LeanLock_State(auto.tempLockLean);
                    tempAction.auto.refs.playerMan.ZoomLock_State(auto.tempLockZoom);
                    tempAction.auto.refs.playerMan.WeaponsZoomLock_State(auto.tempLockWeapZoom);
                    tempAction.auto.refs.playerMan.WeaponsUseLock_State(auto.tempLockItemUse);

                    Inventory.Instance.CustomLock_State(auto.tempLockItemSwitch);

                #endif

                tempAction.auto.oldRadius = auto.tempRadius;
                tempAction.auto.currentItem = auto.tempItem;

                tempAction.CharAction_Set();
                tempAction.Interactions_Update(tempType, false, false);
                tempAction.auto.actionActive = true;

                tempAction.auto.refs.transform.position = tempAction.teleportTrans.position;

                tempAction.Player_Look(new Vector2(tempAction.teleportTrans.localEulerAngles.y, tempAction.teleportTrans.localEulerAngles.z), 100);

                yield return new WaitForSeconds(tempAction.teleportWait);

                tempAction.Action_Check();

            }//tempAction != null

        }//ActionTransfer_Init


    //////////////////////////////////////
    ///
    ///     TRANSFER ACTION ACTIONS
    ///
    ///////////////////////////////////////


        private IEnumerator TransferAction_Destroy(){

            yield return new WaitForSeconds(tempAction.teleportWait + 0.3f);

            if(HFPS_TransferAction.instance != null){

                Destroy(HFPS_TransferAction.instance.gameObject);

            }//instance != null

        }//TransferAction_Destroy


    }//HFPS_Actions


}//namespace