using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if COMPONENTS_PRESENT

    using DizzyMedia.HFPS_Components;

#endif

using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/Player/References")]
    public class HFPS_References : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_References instance;


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     HFPS
    ///
    ///////////////////////////


        public CharacterController characterController;
        public PlayerController playCont;
        public PlayerFunctions playerFunct;
        public HealthManager healthManager;
        public MouseLook mouseLook;
        public JumpscareEffects jsEffects;
        public ItemSwitcher itemSwitcher;
        public Rigidbody playerRigid;


    ///////////////////////////
    ///
    ///     COMPONENTS
    ///
    ///////////////////////////


        #if COMPONENTS_PRESENT

            public HFPS_AudioFader audioFader;
            public HFPS_PlayerMan playerMan;
            public HFPS_ScreenEvents screenEvents;
            public HFPS_SubActionsHandler subActionsHandler;

        #endif
        

    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////

        
        //#if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_VENDOR_PRESENT)

            public HFPS_CharacterAction charAction;

        //#endif

        public int tabs;

        public bool hfpsOpts;
        public bool compOpts;
        public bool helpCompRefs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

        }//awake

        void Start(){}


    //////////////////////////////////////
    ///
    ///     REFERENCES ACTIONS
    ///
    ///////////////////////////////////////


        public void References_Catch(){


    //////////////////////////
    ///
    ///     HFPS
    ///
    //////////////////////////


            characterController = GetComponent<CharacterController>();
            playCont = GetComponent<PlayerController>();

            playerRigid = GetComponent<Rigidbody>();
            healthManager = GetComponent<HealthManager>();

            ScriptManager tempScriptMan = null;

            foreach(Transform child in transform.GetComponentsInChildren<Transform>()){

                if(child.GetComponent<ScriptManager>() != null){

                    tempScriptMan = child.GetComponent<ScriptManager>();
                    break;

                }//ScriptManager != null

            }//foreach child

            if(tempScriptMan != null){

                playerFunct = tempScriptMan.gameObject.GetComponent<PlayerFunctions>();
                mouseLook = tempScriptMan.gameObject.GetComponent<MouseLook>();
                jsEffects = tempScriptMan.gameObject.GetComponent<JumpscareEffects>();

            }//tempScriptMan != null

            foreach(Transform child in transform.GetComponentsInChildren<Transform>()){

                if(child.GetComponent<ItemSwitcher>() != null){

                    itemSwitcher = child.GetComponent<ItemSwitcher>();
                    break;

                }//ItemSwitcher != null

            }//foreach child
            
            Debug.Log("HFPS References caught!");


    //////////////////////////
    ///
    ///     COMPONENTS
    ///
    //////////////////////////

            
            #if COMPONENTS_PRESENT

                audioFader = HFPS_AudioFader.instance;
                playerMan = GetComponent<HFPS_PlayerMan>();

                foreach(Transform child in tempScriptMan.MainCamera.transform.GetComponentsInChildren<Transform>(true)){

                    if(child.GetComponent<HFPS_ScreenEvents>() != null){

                        screenEvents = child.GetComponent<HFPS_ScreenEvents>();
                        break;

                    }//HFPS_ScreenEvents != null

                }//foreach child

                foreach(Transform child in tempScriptMan.ArmsCamera.transform.GetComponentsInChildren<Transform>(true)){

                    if(child.GetComponent<HFPS_SubActionsHandler>()){

                        subActionsHandler = child.GetComponent<HFPS_SubActionsHandler>();
                        break;

                    }//HFPS_SubActionsHandler

                }//foreach child
                
                Debug.Log("Components References caught!");
            
            #endif

        }//References_Catch


    }//HFPS_References


}//namespace