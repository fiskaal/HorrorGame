using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DizzyMedia.Shared;

using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Destroyable/Radial Detect")]
    public class HFPS_RadialDetect : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum RadialAttribute {

            None = 0,
            OnlyHitRecievers = 1,
            OnlyPlayer = 2,
            OnlyNPC = 3,
            Everything = 4,

        }//RadialAttribute


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     USER OPTIONS
    ///
    ///////////////////////////


        public RadialAttribute radialAttribute;

        [TagSelectorAttribute]
        public string playerTag;

        [TagSelectorAttribute]
        public string npcTag;

        public int damageAmount;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        private HFPS_Destroyable tempDest;

        public int tabs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start(){}


    //////////////////////////////////////
    ///
    ///     DETECT ACTIONS
    ///
    ///////////////////////////////////////

    //////////////////////////
    ///
    ///     RECEIVERS
    ///
    //////////////////////////


        private void Detect_Receivers(Collider other){

            if(other.gameObject.GetComponent<HFPS_HitReceiver>() != null){
            
                #if COMPONENTS_PRESENT

                    if(other.gameObject.GetComponent<HFPS_HitReceiver>().destroyable != null){

                        tempDest = other.gameObject.GetComponent<HFPS_HitReceiver>().destroyable;

                    }//destroyable != null
                
                #endif

                if(tempDest != null){

                    if(!tempDest.locked && tempDest.affectedByExplosions){

                        tempDest.Break_Check(true);

                    }//!locked & affectedByExplosions

                //tempDest != null
                } else {

                    other.gameObject.GetComponent<HFPS_HitReceiver>().ApplyDamage(damageAmount);

                }//tempDest != null

            }//HFPS_HitReceiver != null

        }//Detect_Receivers


    //////////////////////////
    ///
    ///     PLAYER
    ///
    //////////////////////////


        private void Detect_Player(Collider other){

            if(other.gameObject.tag == playerTag){

                other.gameObject.GetComponent<HFPS_References>().healthManager.ApplyDamage(damageAmount);

            }//tag = playerTag

        }//Detect_Player


    //////////////////////////
    ///
    ///     NPC
    ///
    //////////////////////////


        private void Detect_NPC(Collider other){

            if(other.gameObject.tag == npcTag){

                other.gameObject.GetComponent<NPCHealth>().Damage(damageAmount);

            }//tag = npcTag

        }//Detect_NPC


    //////////////////////////////////////
    ///
    ///     TRIGGER ACTIONS
    ///
    ///////////////////////////////////////


        void OnTriggerEnter(Collider other){

            if(radialAttribute == RadialAttribute.OnlyHitRecievers){

                Detect_Receivers(other);

            }//radialAttribute = OnlyHitRecievers

            if(radialAttribute == RadialAttribute.OnlyPlayer){

                Detect_Player(other);

            }//radialAttribute = only player

            if(radialAttribute == RadialAttribute.OnlyNPC){

                Detect_NPC(other);

            }//radialAttribute = only npc

            if(radialAttribute == RadialAttribute.Everything){

                Detect_Receivers(other);
                Detect_Player(other);

            }//radialAttribute = Everything

        }//OnTriggerEnter


    }//HFPS_RadialDetect


}//namespace