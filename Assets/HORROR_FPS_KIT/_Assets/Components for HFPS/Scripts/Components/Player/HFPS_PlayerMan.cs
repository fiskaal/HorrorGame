using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

#if HFPS_WEAPONS_PRESENT

    using DizzyMedia.HFPS_Weapon;

#endif

using Newtonsoft.Json.Linq;
using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Player/Player Manager")]
    public class HFPS_PlayerMan : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_PlayerMan instance;


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Melee_Cont {

            [Space]

            public string name;
            public MeleeController meleeCont;

        }//Melee_Cont

        [Serializable]
        public class Weapon_Cont {

            [Space]

            public string name;
            public WeaponController weaponCont;

        }//Weapon_Cont
        
        [Serializable]
        public class ThrowWeapon_Cont {

            [Space]

            public string name;
            
            #if HFPS_THROWABLES_PRESENT
            
                public HFPS_ThrowWeapon throwWeapCont;

            #endif
            
        }//ThrowWeapon_Cont
        
        [Serializable]
        public class DM_Weap_Cont {
        
            [Space]

            public string name;
            
            #if HFPS_WEAPONS_PRESENT
            
                public DM_Weapon dmWeaponCont;
            
            #endif
        
        }//DM_Weap_Cont


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum SlowDown {

            None = 0,
            SlowOnZoom = 1,

        }//SlowDown


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public SlowDown slowDownUse;

        public HFPS_References references;
        public List<Melee_Cont> meleeConts;
        public List<Weapon_Cont> weaponConts;
        
        #if HFPS_THROWABLES_PRESENT
        
            public List<ThrowWeapon_Cont> throwConts;

        #endif
        
        #if HFPS_WEAPONS_PRESENT
        
            public List<DM_Weap_Cont> dmWeapConts;
        
        #endif
        
        public HFPS_EnemiesHolder enemHold;
        public bool isHiding;

        public int tabs;
        public bool userOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            if(HFPS_EnemiesHolder.instance != null){

                enemHold = HFPS_EnemiesHolder.instance;

            }//instance != null

            #if COMPONENTS_PRESENT

                if(slowDownUse == SlowDown.None){

                    references.playCont.LockZoom_State(true);

                }//slowDownUse = none

                if(slowDownUse == SlowDown.SlowOnZoom){

                    references.playCont.LockZoom_State(false);

                }//slowDownUse = slow on zoom

            #endif

        }//StartInit


    //////////////////////////////////////
    ///
    ///     HIDE ACTIONS
    ///
    ///////////////////////////////////////


        public void Hide_State(bool hide){

            isHiding = hide;

            if(enemHold.zombAI.Count > 0){

                for(int i = 0; i < enemHold.zombAI.Count; ++i ) {

                    enemHold.zombAI[i].behaviourSettings.playerInvisible = isHiding;

                }//for i enemHold

            }//zombAI.Count > 0

        }//Hide_State    


    //////////////////////////////////////
    ///
    ///     MOVE ACTIONS
    ///
    ///////////////////////////////////////


        public void LockMoveX_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LockMoveX_State(state);

            #endif

        }//LockMoveX_State

        public void LimitMoveX_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LimitMoveX_State(state);

            #endif

        }//LimitMoveX_State

        public void LimitMoveX_Set(int direction){

            #if COMPONENTS_PRESENT

                references.playCont.LimitMoveX_Set(direction);

            #endif

        }//LimitMoveX_Set


        public void LockMoveY_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LockMoveY_State(state);

            #endif

        }//LockMoveY_State

        public void LimitMoveY_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LimitMoveY_State(state);

            #endif

        }//LimitMoveY_State

        public void LimitMoveY_Set(int direction){

            #if COMPONENTS_PRESENT

                references.playCont.LimitMoveY_Set(direction);

            #endif

        }//LimitMoveY_Set


        public void LockJump_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LockJump_State(state);

            #endif

        }//LockJump_State


    //////////////////////////////////////
    ///
    ///     SPRINT ACTIONS
    ///
    ///////////////////////////////////////


        public void SprintLock_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LockSprint_State(state);

            #endif

        }//SprintLock_State


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void LockStateInput_State(bool state){

            #if COMPONENTS_PRESENT

                references.playCont.LockStateInput_State(state);

            #endif

        }//LockStateInput_State


    //////////////////////////////////////
    ///
    ///     LEAN ACTIONS
    ///
    ///////////////////////////////////////


        public void LeanLock_State(bool state){

            #if COMPONENTS_PRESENT

                references.playerFunct.LeanLock_State(state);

            #endif

        }//LeanLock_State


    //////////////////////////////////////
    ///
    ///     ZOOM ACTIONS
    ///
    ///////////////////////////////////////


        public void ZoomLock_State(bool state){

            #if COMPONENTS_PRESENT

                references.playerFunct.ZoomLock_State(state);

            #endif

        }//ZoomLock_State


    //////////////////////////////////////
    ///
    ///     WEAPONS ACTIONS
    ///
    ///////////////////////////////////////


        public void WeaponsUseLock_State(bool state){

            #if COMPONENTS_PRESENT

                if(meleeConts.Count > 0){

                    for(int m = 0; m < meleeConts.Count; m++) {

                        meleeConts[m].meleeCont.FireLock_State(state);

                    }//for m meleeConts

                }//meleeConts.Count > 0

                if(weaponConts.Count > 0){

                    for(int w = 0; w < weaponConts.Count; w++) {

                        weaponConts[w].weaponCont.FireLock_State(state);

                    }//for w weaponConts

                }//weaponConts.Count > 0

            #endif
            
            #if HFPS_THROWABLES_PRESENT
            
                if(throwConts.Count > 0){

                    for(int tc = 0; tc < throwConts.Count; tc++) {

                        throwConts[tc].throwWeapCont.FireLock_State(state);

                    }//for tc throwConts

                }//throwConts.Count > 0
            
            #endif
            
            #if HFPS_WEAPONS_PRESENT
            
                if(dmWeapConts.Count > 0){

                    for(int dmwps = 0; dmwps < dmWeapConts.Count; dmwps++) {

                        dmWeapConts[dmwps].dmWeaponCont.FireLock_State(state);

                    }//for dmwps dmWeapConts

                }//dmWeapConts.Count > 0
            
            #endif

        }//WeaponsUseLock_State

        public void WeaponsZoomLock_State(bool state){

            #if COMPONENTS_PRESENT

                /*

                if(meleeConts.Count > 0){

                    for(int m = 0; m < meleeConts.Count; m++) {



                    }//for m meleeConts

                }//meleeConts.Count > 0

                */

                if(weaponConts.Count > 0){

                    for(int w = 0; w < weaponConts.Count; w++) {

                        weaponConts[w].weaponCont.ZoomLock_State(state);

                    }//for w weaponConts

                }//weaponConts.Count > 0

            #endif
            
            /*
            
            #if HFPS_THROWABLES_PRESENT
            
                if(throwConts.Count > 0){

                    for(int tc = 0; tc < throwConts.Count; tc++) {

                        throwConts[tc].throwWeapCont.ZoomLock_State(state);

                    }//for tc throwConts

                }//weaponConts.Count > 0
                
            #endif
            
            */
            
            #if HFPS_WEAPONS_PRESENT
            
                if(dmWeapConts.Count > 0){

                    for(int dmwps = 0; dmwps < dmWeapConts.Count; dmwps++) {

                        dmWeapConts[dmwps].dmWeaponCont.ZoomLock_State(state);

                    }//for dmwps dmWeapConts

                }//dmWeapConts.Count > 0
            
            #endif

        }//WeaponsZoomLock_State
        
        
    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    ///////////////////////////////////////
        
        
        public void Weapons_Catch(){
        
            if(references != null){
            
                if(references.itemSwitcher.ItemList.Count > 0){
                
                    for(int i = 0; i < references.itemSwitcher.ItemList.Count; i++) {
                    
                        
                        //MELEE CHECKS
                    
                        if(references.itemSwitcher.ItemList[i].GetComponent<MeleeController>()){
                        
                            Melee_Cont tempMeleeCont = new Melee_Cont();
                            
                            tempMeleeCont.name = references.itemSwitcher.ItemList[i].gameObject.name;
                            tempMeleeCont.meleeCont = references.itemSwitcher.ItemList[i].GetComponent<MeleeController>();
                            
                            if(!Melee_Check(tempMeleeCont.meleeCont)){
                            
                                meleeConts.Add(tempMeleeCont);
                            
                            //does not have melee item
                            } else {
                            
                                Debug.Log("Melee > " + tempMeleeCont.name + " already caught");
                            
                            }//does not have melee item
                        
                        }//has MeleeController
                        
                        
                        //WEAPON CHECKS
                        
                        if(references.itemSwitcher.ItemList[i].GetComponent<WeaponController>()){
                        
                            Weapon_Cont tempWeapCont = new Weapon_Cont();
                            
                            tempWeapCont.name = references.itemSwitcher.ItemList[i].gameObject.name;
                            tempWeapCont.weaponCont = references.itemSwitcher.ItemList[i].GetComponent<WeaponController>();
                            
                            if(!Weapon_Check(tempWeapCont.weaponCont)){
                            
                                weaponConts.Add(tempWeapCont);
                            
                            //does not have weapon item
                            } else {
                            
                                Debug.Log("Weapon > " + tempWeapCont.name + " already caught");
                            
                            }//does not have weapon item
                        
                        }//has WeaponController
                        
                        
                        //THROWABLE CHECKS
                        
                        #if HFPS_THROWABLES_PRESENT
                        
                            if(references.itemSwitcher.ItemList[i].GetComponent<HFPS_ThrowWeapon>()){

                                ThrowWeapon_Cont tempThrowCont = new ThrowWeapon_Cont();

                                tempThrowCont.name = references.itemSwitcher.ItemList[i].gameObject.name;
                                tempThrowCont.throwWeapCont = references.itemSwitcher.ItemList[i].GetComponent<HFPS_ThrowWeapon>();

                                if(!Throw_Check(tempThrowCont.throwWeapCont)){

                                    weaponConts.Add(tempThrowCont);

                                //does not have throw item
                                } else {

                                    Debug.Log("Throwable > " + tempThrowCont.name + " already caught");

                                }//does not have throw item

                            }//has HFPS_ThrowWeapon
                        
                        #endif
                        
                        
                        //DM WEAPON CHECKS
                        
                        #if HFPS_WEAPONS_PRESENT
                        
                            if(references.itemSwitcher.ItemList[i].GetComponent<DM_Weapon>()){

                                DM_Weap_Cont tempDMWeap = new DM_Weap_Cont();

                                tempDMWeap.name = references.itemSwitcher.ItemList[i].gameObject.name;
                                tempDMWeap.dmWeaponCont = references.itemSwitcher.ItemList[i].GetComponent<DM_Weapon>();

                                if(!DMWeap_Check(tempDMWeap.dmWeaponCont)){

                                    dmWeapConts.Add(tempDMWeap);

                                //does not have DM Weapon item
                                } else {

                                    Debug.Log("DM Weapon > " + tempDMWeap.name + " already caught");

                                }//does not have DM Weapon item

                            }//has DM_Weapon
                        
                        #endif
                        
                    
                    }//for i ItemList
                
                //itemList.Count > 0
                } else {
                
                    Debug.Log("Item Switcher has no items");
                
                }//itemList.Count > 0
            
            //references != null
            } else {
            
                Debug.Log("HFPS References = null");
            
            }//references != null
        
        }//Weapons_Catch
        
        private bool Melee_Check(MeleeController newMeleeCont){
        
            if(meleeConts.Count > 0){

                for(int m = 0; m < meleeConts.Count; m++) {

                    if(meleeConts[m].meleeCont == newMeleeCont){
                    
                        return true;
                    
                    }//meleeCont = newMeleeCont

                }//for m meleeConts

            }//meleeConts.Count > 0
            
            return false;
        
        }//Weapons_Check
        
        private bool Weapon_Check(WeaponController newWeapCont){
        
            if(weaponConts.Count > 0){

                for(int w = 0; w < weaponConts.Count; w++) {
                
                    if(weaponConts[w].weaponCont == newWeapCont){
                    
                        return true;
                    
                    }//weaponConts = newWeapCont

                }//for w weaponConts

            }//weaponConts.Count > 0
            
            return false;
        
        }//Weapon_Check
        
        #if HFPS_THROWABLES_PRESENT
        
            private bool Throw_Check(HFPS_ThrowWeapon newThrowWeap){

                if(throwConts.Count > 0){

                    for(int tc = 0; tc < throwConts.Count; tc++) {

                        if(throwConts[tc].throwWeapCont == newThrowWeap){

                            return true;

                        }//throwWeapCont = newThrowWeap

                    }//for tc throwConts

                }//throwConts.Count > 0

                return false;

            }//Throw_Check
        
        #endif
        
        #if HFPS_WEAPONS_PRESENT
        
            private bool DMWeap_Check(DM_Weapon newDMWeap){

                if(dmWeapConts.Count > 0){

                    for(int dmwp = 0; dmwp < dmWeapConts.Count; dmwp++) {

                        if(dmWeapConts[dmwp].dmWeaponCont == newDMWeap){

                            return true;

                        }//dmWeaponCont = newDMWeap

                    }//for dmwp dmWeapConts

                }//dmWeapConts.Count > 0

                return false;

            }//DMWeap_Check
        
        #endif


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"isHiding", isHiding }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            isHiding = (bool)token["isHiding"];

            StartCoroutine("Load_Buff");

        }//OnLoad

        private IEnumerator Load_Buff(){

            yield return new WaitForSeconds(0.4f);

            if(isHiding){

                Hide_State(true);

            }//isHiding

        }//Load_Buff


    }//HFPS_PlayerMan


}//namespace