using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

#if COMPONENTS_PRESENT

    using DizzyMedia.HFPS_Components;

#endif

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/Hit/Hit Receiver")]
    public class HFPS_HitReceiver : MonoBehaviour, ISaveable {


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class OnHitEvent : UnityEvent<int> {

            public int damage;

        }//OnHitEvent

        [Serializable]
        public class OnRayEvent : UnityEvent<RaycastHit> {

            public RaycastHit hit;

        }//OnRayEvent

        [Serializable]
        public class Message {

            [Space]

            public string name;

        }//Message


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////


        #if COMPONENTS_PRESENT

            public HFPS_Destroyable destroyable;

        #endif


    //////////////////////////
    //
    //      USER OPTIONS
    //
    //////////////////////////


        public bool useDelayUnlock;
        public float unlockDelay;


    //////////////////////////
    //
    //      MESSAGES OPTIONS
    //
    //////////////////////////


        public List<Message> messages;


    //////////////////////////
    //
    //      EVENTS
    //
    //////////////////////////


        public OnHitEvent onHit;
        public OnRayEvent onRayHit;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public float tempDamage;
        public bool locked;

        public int tabs;

        public bool startOpts;
        public bool genOpts;
        public bool messOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Awake(){

            LockState(true);

        }//Awake

        void Start(){

            StartInit();

        }//Start

        public void StartInit(){

            if(useDelayUnlock){

                StartCoroutine("LockStateDelay", false);

            //useDelayUnlock
            } else {

                LockState(false);

            }//useDelayUnlock

        }//StartInit


    //////////////////////////
    //
    //      BROADCAST ACTIONS
    //
    //////////////////////////


        public void Broadcast_Messages(RaycastHit hit){

            Debug.Log("Messages Start");

            if(messages.Count > 0){

                for(int i = 0; i < messages.Count; ++i ) {

                    if(tempDamage > 0){

                        hit.collider.gameObject.BroadcastMessage(messages[i].name, tempDamage, SendMessageOptions.DontRequireReceiver);

                        Debug.Log("Message Sent = " + messages[i].name + " " + tempDamage);

                    //tempDamage > 0
                    } else {

                        hit.collider.gameObject.BroadcastMessage(messages[i].name, SendMessageOptions.DontRequireReceiver);

                        Debug.Log("Message Sent = " + messages[i].name);

                    }//tempDamage > 0

                }//for i messages

            }//messages.Count > 0

        }//Broadcast_Messages

        public void Broadcast_Messages(){

            //Debug.Log("Messages Start");

            if(messages.Count > 0){

                for(int i = 0; i < messages.Count; ++i ) {

                    if(tempDamage > 0){

                        this.gameObject.BroadcastMessage(messages[i].name, tempDamage, SendMessageOptions.DontRequireReceiver);

                        //Debug.Log("Message Sent = " + messages[i].name + " " + tempDamage);

                    //tempDamage > 0
                    } else {

                        this.gameObject.BroadcastMessage(messages[i].name, SendMessageOptions.DontRequireReceiver);

                        //Debug.Log("Message Sent = " + messages[i].name);

                    }//tempDamage > 0

                }//for i messages

            }//messages.Count > 0

        }//Broadcast_Messages


    //////////////////////////
    //
    //      DAMAGE ACTIONS
    //
    //////////////////////////


        public void ApplyDamageRay(RaycastHit hit){

            if(!locked){

                onRayHit.hit = hit;
                onRayHit.Invoke(onRayHit.hit);

            }//!locked

        }//ApplyDamageRay

        public void ApplyDamage(int damage){

            if(!locked){

                tempDamage = damage;

                onHit.damage = damage;
                onHit.Invoke(onHit.damage);

            }//!locked

        }//ApplyDamage


    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void LockState(bool state){

            locked = state;

        }//LockState

        public IEnumerator LockStateDelay(bool state){

            yield return new WaitForSeconds(unlockDelay);

            locked = state;

        }//LockStateDelay


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            locked = (bool)token["locked"];

        }//OnLoad


    }//HFPS_HitReceiver


}//namespace