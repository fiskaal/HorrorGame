using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Ignitable/Ignitable Handler")]
    public class HFPS_IgniteHand : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     GENERAL OPTIONS
    ///
    ///////////////////////////


        [TagSelectorAttribute]
        public string detectTag;

        public float igniteRate;
        public float igniteMulti;


    ///////////////////////////
    ///
    ///     AUDIO OPTIONS
    ///
    ///////////////////////////


        public bool useIgniteSound;
        public AudioSource source;
        public AudioClip igniteClip;


    ///////////////////////////
    ///
    ///     AUTO OPTIONS
    ///
    ///////////////////////////


        public HFPS_Ignitable tempIgnite;

        public bool isLighting;
        public float tempIgniteTime;

        public bool locked;

        public int tabs;

        public bool genOpts;
        public bool audioOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            tempIgnite = null;
            isLighting = false;
            tempIgniteTime = 0;

        }//start


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update(){

            if(!locked){

                if(isLighting){

                    if(tempIgniteTime > 0){

                        tempIgniteTime -= igniteRate * igniteMulti * Time.deltaTime;

                    //tempIgniteTime > 0
                    } else {

                        isLighting = false;

                        Ignite_Finish();

                    }//tempIgniteTime > 0

                }//isLighting

            }//!locked

        }//update


    //////////////////////////////////////
    ///
    ///     IGNITE ACTIONS
    ///
    ///////////////////////////////////////


        public void Ignite(){

            if(tempIgnite != null){

                tempIgnite.Ignite();
                tempIgnite = null;

            }//tempIgnite != null

        }//Ignite 

        private void Ignite_Start(){

            tempIgniteTime = 0;
            tempIgniteTime = tempIgnite.igniteTime;
            isLighting = true;

            if(useIgniteSound){

                if(source.clip != igniteClip){

                    source.clip = igniteClip;

                }//clip != clip

                source.loop = true;
                source.Play();

            }//useIgniteSound

        }//Ignite_Start

        private void Ignite_Stop(){

            isLighting = false;
            tempIgniteTime = 0;
            tempIgnite = null;

            if(useIgniteSound){

                source.Stop();

            }//useIgniteSound

        }//Ignite_Stop

        private void Ignite_Finish(){

            tempIgniteTime = 0;

            Ignite();

            if(useIgniteSound){

                source.Stop();

            }//useIgniteSound

        }//Ignite_Finish


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void Locked_State(bool state){

            locked = state;

        }//Locked_State


    //////////////////////////////////////
    ///
    ///     TRIGGER ACTIONS
    ///
    ///////////////////////////////////////


        void OnTriggerEnter(Collider other){

            if(!locked && !isLighting){

                if(other.gameObject.tag == detectTag){

                    if(other.gameObject.GetComponent<HFPS_Ignitable>() != null){

                        tempIgnite = other.gameObject.GetComponent<HFPS_Ignitable>().Ignitable_Catch();

                    }//HFPS_References != null

                    if(tempIgnite != null){

                        if(tempIgnite.igniteType == HFPS_Ignitable.Ignite_Type.Trigger){

                            if(!tempIgnite.isLit){

                                Ignite_Start();

                            }//isLit

                        }//igniteType = trigger

                    }//tempIgnite != null

                }//tag = detectTag

            }//!locked & !isLighting

        }//OnTriggerEnter

        void OnTriggerExit(Collider other){

            if(!locked){

                if(other.gameObject.tag == detectTag){

                    if(tempIgnite != null){

                        if(tempIgnite.igniteType == HFPS_Ignitable.Ignite_Type.Trigger){

                            Ignite_Stop();

                        }//igniteType = trigger

                    }//tempIgnite != null

                }//tag = detectTag

            }//!locked & !isLighting

        }//OnTriggerExit


    }//HFPS_IgniteHand


}//namespace