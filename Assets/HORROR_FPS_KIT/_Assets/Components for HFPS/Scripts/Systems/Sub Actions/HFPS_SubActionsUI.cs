using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using ThunderWire.Input;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Sub Actions/Sub Actions UI")]
    public class HFPS_SubActionsUI : MonoBehaviour {


    //////////////////////////
    //
    //      INSTANCE
    //
    //////////////////////////


        public static HFPS_SubActionsUI instance;


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class ActionHolder {

            [Space]

            public string name;

            [Space]

            public GameObject holder;
            public Image icon;
            public Image highlight;
            public Text text;

            [Space]

            public bool showInput;
            public List<Input_Holder> inputHolders;

            [Space]

            [Header("Auto")]

            public bool isActive;

        }//ActionHolder

        [Serializable]
        public class Input_Holder {

            [Space]

            public string name;

            [Space]

            public GameObject holder;
            public Image inputImage;

            public Sprite noInputSprite;
            public Sprite inputSprite;

        }//Input_Holder


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


        public GameObject holder;


    /////////////////////////
    ///
    ///     INPUT OPTIONS
    ///
    /////////////////////////


        public bool detectPause;
        public string pauseInput;


    //////////////////////////
    //
    //      ANIMATION
    //
    //////////////////////////


        public Animator anim;
        public AnimationClip showAnim;
        public AnimationClip hideAnim;


    //////////////////////////
    //
    //      ACTIONS
    //
    //////////////////////////


        public List<ActionHolder> actionHolders;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public DM_InternEnums.PlayInput_Type playerInput;
        public bool menuOpen;
        public bool menuHidden;
        public bool pauseKeyPressed;
        public bool pausedLocked;
        public bool locked;

        public int tabs;

        public bool genOpts;
        public bool inputOpts;
        public bool animOpts;
        public bool actOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Awake(){

            instance = this;

        }//Awake

        void Start() {

            menuOpen = false;
            menuHidden = false;
            pausedLocked = false;
            locked = false;

        }//Start


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update() {

            if(!locked){

                if(detectPause){

                    if(InputHandler.InputIsInitialized) {

                        pauseKeyPressed = InputHandler.ReadButton(pauseInput);

                    }//InputIsInitialized

                    if(pauseKeyPressed){

                        if(!pausedLocked){

                            pausedLocked = true;

                            if(menuOpen){

                                Actions_HideSolo();

                            //menuOpen
                            } else if(menuHidden){

                                Actions_ShowSolo();

                            }//menuHidden

                        }//!pausedLocked

                    }//pauseKeyPressed & menuOpen

                    if(detectPause){

                        if(!pauseKeyPressed){

                            pausedLocked = false;

                        }//!pauseKeyPressed

                    }//detectPause

                }//detectPause

            }//!locked

        }//Update


    //////////////////////////
    //
    //      ACTION HOLDER ACTIONS
    //
    //////////////////////////


        public void ActionHolders_Reset(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                actionHolders[ah].holder.SetActive(false);

                if(actionHolders[ah].icon != null){

                    actionHolders[ah].icon.sprite = null;

                }//icon != null

                if(actionHolders[ah].highlight != null){

                    actionHolders[ah].highlight.fillAmount = 0;

                }//highlight != null

                if(actionHolders[ah].text != null){

                    actionHolders[ah].text.text = "";

                }//text != null

                actionHolders[ah].isActive = false;

            }//for ah actionHolders

        }//ActionHolders_Reset

        public void ActionHolder_Update(int slot, string display, Sprite icon){

            actionHolders[slot].holder.SetActive(true);

            if(actionHolders[slot].icon != null){

                actionHolders[slot].icon.sprite = icon;

            }//images != null

            if(actionHolders[slot].text != null){

                actionHolders[slot].text.text = display;

            }//text != null

            if(actionHolders[slot].highlight != null){

                actionHolders[slot].highlight.fillAmount = 0;

            }//highlight != null

            actionHolders[slot].isActive = true;

        }//ActionHolder_Update


    //////////////////////////
    //
    //      FILL ACTIONS
    //
    //////////////////////////


        public void Fill_Update(int slot, float amount){

            if(actionHolders[slot].highlight != null){

                actionHolders[slot].highlight.fillAmount = amount;

            }//highlight != null

            if(actionHolders[slot].showInput){

                InputHolder_Update(slot);

            }//showInput

        }//Fill_Update

        public void Fill_Reset(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                if(actionHolders[ah].highlight != null){

                    actionHolders[ah].highlight.fillAmount = 0;

                }//highlight != null

                if(actionHolders[ah].showInput){

                    if(actionHolders[ah].highlight.fillAmount > 0){

                        if(actionHolders[ah].inputHolders[(int)playerInput].inputImage.sprite != actionHolders[ah].inputHolders[(int)playerInput].inputSprite){

                            actionHolders[ah].inputHolders[(int)playerInput].inputImage.sprite = actionHolders[ah].inputHolders[(int)playerInput].inputSprite;

                        }//sprite != inputSprite

                    //fillAmount
                    } else {

                        if(actionHolders[ah].inputHolders[(int)playerInput].inputImage.sprite != actionHolders[ah].inputHolders[(int)playerInput].noInputSprite){

                            actionHolders[ah].inputHolders[(int)playerInput].inputImage.sprite = actionHolders[ah].inputHolders[(int)playerInput].noInputSprite;

                        }//sprite != noInputSprite

                    }//fillAmount

                }//showInput

            }//for ah actionHolders

        }//Fill_Reset


    //////////////////////////
    //
    //      ANIMATION ACTIONS
    //
    //////////////////////////


        public void Actions_Show(){

            holder.SetActive(true);
            anim.Play(showAnim.name);

            menuOpen = true;

        }//Actions_Show

        public void Actions_ShowSolo(){

            if(!menuOpen && menuHidden){

                holder.SetActive(true);

                menuOpen = true;
                menuHidden = false;

                #if COMPONENTS_PRESENT

                    HFPS_References.instance.subActionsHandler.Locked_State(false);

                #endif

            }//!menuOpen and menuHidden

        }//Actions_ShowSolo

        public void Actions_Hide(){

            if(holder.activeSelf){

                anim.Play(hideAnim.name);

                StartCoroutine("HideBuff");

            }//activeSelf

        }//Actions_Hide

        private IEnumerator HideBuff(){

            yield return new WaitForSeconds(hideAnim.length + 0.2f);

            holder.SetActive(false);

            InputHolders_Reset();
            Fill_Reset();

            menuOpen = false;

        }//HideBuff

        public void Actions_HideSolo(){

            holder.SetActive(false);

            menuOpen = false;
            menuHidden = true;

            #if COMPONENTS_PRESENT

                HFPS_References.instance.subActionsHandler.Locked_State(true);
                
            #endif
            
        }//Actions_HideSolo


    //////////////////////////
    //
    //      INPUT ACTIONS
    //
    //////////////////////////


        public void InputHolder_Update(int slot){

            if(actionHolders[slot].highlight.fillAmount > 0){

                if(actionHolders[slot].inputHolders[(int)playerInput].inputImage.sprite != actionHolders[slot].inputHolders[(int)playerInput].inputSprite){

                    actionHolders[slot].inputHolders[(int)playerInput].inputImage.sprite = actionHolders[slot].inputHolders[(int)playerInput].inputSprite;

                }//sprite != inputSprite

            //fillAmount
            } else {

                if(actionHolders[slot].inputHolders[(int)playerInput].inputImage.sprite != actionHolders[slot].inputHolders[(int)playerInput].noInputSprite){

                    actionHolders[slot].inputHolders[(int)playerInput].inputImage.sprite = actionHolders[slot].inputHolders[(int)playerInput].noInputSprite;

                }//sprite != noInputSprite

            }//fillAmount

        }//InputHolder_Update

        public void InputHolders_Reset(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                if(actionHolders[ah].inputHolders[(int)playerInput].inputImage.sprite != actionHolders[ah].inputHolders[(int)playerInput].noInputSprite){

                    actionHolders[ah].inputHolders[(int)playerInput].inputImage.sprite = actionHolders[ah].inputHolders[(int)playerInput].noInputSprite;

                }//sprite != noInputSprite

            }//for ah actionHolders

        }//InputHolders_Reset

        public void InputCheck_Icons(){

            for(int ah = 0; ah < actionHolders.Count; ah++) {

                if(actionHolders[ah].isActive){

                    for(int ih = 0; ih < actionHolders[ah].inputHolders.Count; ih++) {

                        if(actionHolders[ah].inputHolders[ih].holder.activeSelf){

                            actionHolders[ah].inputHolders[ih].holder.SetActive(false);

                        }//activeSelf

                    }//for ih inputHolders

                    if(!actionHolders[ah].inputHolders[(int)playerInput].holder.activeSelf){

                        actionHolders[ah].inputHolders[(int)playerInput].holder.SetActive(true);

                    }//!activeSelf

                }//isActive

            }//for ah actionHolders

        }//InputCheck_Icons


    }//HFPS_SubActionsUI


}//namespace