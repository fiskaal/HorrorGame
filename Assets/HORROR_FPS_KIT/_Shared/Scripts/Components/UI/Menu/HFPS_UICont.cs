using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if COMPONENTS_PRESENT

    using DizzyMedia.HFPS_Components;

#endif

using HFPS.Systems;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/UI/Menu/UI Controller")]
    public class HFPS_UICont : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_UICont instance;
        
        
    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////
    
        
        [Serializable]
        public class Display_Set {
        
            [Space]
        
            public string name;
            
            [Space]
            
            public List<DisplaySet_Object> objects;
        
        }//Display_Set
        
        [Serializable]
        public class DisplaySet_Object {
        
            public string name;
            public GameObject holder;
            
            [Header("Auto")]
            public bool wasActive;
        
        }//DisplaySet_Object
        
    
    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public HFPS_GameManager gameManager;
        
        #if COMPONENTS_PRESENT
        
            public SaveGameHandler saveGameHand;

        #endif
        
        [Space]

        public Button saveButton;
        public Button loadButton;
        
        [Space]
        
        public List<Display_Set> displaySets;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){
        
            instance = this;
            
        }//Awake

        void Start() {}//start
        
            
    //////////////////////////////////////
    ///
    ///     PAUSE & INVENTORY ACTIONS
    ///
    //////////////////////////////////////
    
    
        public void DisplaySet_State(bool state, string name){
        
            if(displaySets.Count > 0){
        
                for(int ds = 0; ds < displaySets.Count; ds++) {

                    if(displaySets[ds].name == name){
                    
                        if(displaySets[ds].objects.Count > 0){
                        
                            for(int o = 0; o < displaySets[ds].objects.Count; o++) {
                                
                                if(state){
                    
                                    if(displaySets[ds].objects[o].wasActive){
                                    
                                        displaySets[ds].objects[o].holder.SetActive(state);
                                        displaySets[ds].objects[o].wasActive = false;
                                        
                                    }//wasActive
                                    
                                //state
                                } else {
                                
                                    displaySets[ds].objects[o].wasActive = displaySets[ds].objects[o].holder.activeSelf;
                                    displaySets[ds].objects[o].holder.SetActive(state);
                                
                                }//state
                            
                            }//for uo uiObjects
                        
                        }//uiObjects > 0
                    
                    }//name = name

                }//for ds displaySets
    
            }//displaySets.Count > 0
    
        }//DisplaySet_State


    //////////////////////////////////////
    ///
    ///     PAUSE & INVENTORY ACTIONS
    ///
    //////////////////////////////////////


        public void PauseLock_State(bool state){

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                gameManager.PauseLock_State(state);

            #endif

        }//PauseLock_State

        public void InventoryLock_State(bool state){

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

                gameManager.InventoryLock_State(state);

            #endif

        }//InventoryLock_State


    //////////////////////////////////////
    ///
    ///     SAVE & LOAD ACTIONS
    ///
    //////////////////////////////////////

    /////////////////////////
    ///
    ///     SAVE ACTIONS
    ///
    ////////////////////////


        public void Save_Init(bool preLoad){

            #if COMPONENTS_PRESENT

                saveGameHand.PreLoad_State(preLoad);

                saveGameHand.SaveSerializedData(true);
            
            #endif

        }//Save_Init

        public void Save_State(bool state){

            saveButton.interactable = state;

        }//Save_State


    /////////////////////////
    ///
    ///     LOAD ACTIONS
    ///
    /////////////////////////


        public void Load_State(bool state){

            loadButton.interactable = state;

        }//Load_State


    }//HFPS_UICont


}//namespace
