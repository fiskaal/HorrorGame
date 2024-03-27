using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Screen Events/Screen Events")]
    public class HFPS_ScreenEvents : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_ScreenEvents instance;


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class ScreenEvent {

            public List<Category> category;

        }//ScreenEvent

        [Serializable]
        public class Category {

            [Space]

            public string name;

            [Space]

            public List<Event> events;

        }//Category

        [Serializable]
        public class Event {

            [Space]

            public string name;

            [Space]

            public GameObject holder;

            [Space]

            public Animator anim;
            public AnimationClip clip;
            public int slot;

            [Space]

            public bool useDelay;
            public float delayWait;

            [Space]

            public float hideExtraWait;

        }//Event


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        [Space]

        public ScreenEvent screenEvents;

        [Space]

        public int curCategory;
        public int curEvent;

        public int tabs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

        }//Awake

        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            curCategory = 0;
            curEvent = 0;

        }//StartInit


    //////////////////////////////////////
    ///
    ///     SCREEN EVENT ACTIONS
    ///
    ///////////////////////////////////////


        public void ScreenEvent_Init(){

            if(screenEvents.category[curCategory - 1].events[curEvent - 1].useDelay){

                StartCoroutine("ScreenEvent_StartBuff");

            //useDelay
            } else {

                StartCoroutine("ScreenEvent_Start");

            }//useDelay

        }//ScreenEvent_Init

        private IEnumerator ScreenEvent_StartBuff(){

            yield return new WaitForSeconds(screenEvents.category[curCategory - 1].events[curEvent - 1].delayWait);

            StartCoroutine("ScreenEvent_Start");

        }//ScreenEvent_StartBuff

        private IEnumerator ScreenEvent_Start(){

            screenEvents.category[curCategory - 1].events[curEvent - 1].holder.SetActive(true);

            if(screenEvents.category[curCategory - 1].events[curEvent - 1].slot > 0){

                screenEvents.category[curCategory - 1].events[curEvent - 1].anim.SetInteger("Slot", screenEvents.category[curCategory - 1].events[curEvent - 1].slot); 

            }//slot > 0

            yield return new WaitForSeconds(screenEvents.category[curCategory - 1].events[curEvent - 1].clip.length + screenEvents.category[curCategory - 1].events[curEvent - 1].hideExtraWait);

            screenEvents.category[curCategory - 1].events[curEvent - 1].holder.SetActive(false);

        }//ScreenEvent_Start


    //////////////////////////////////////
    ///
    ///     SLOT ACTIONS
    ///
    ///////////////////////////////////////


        public void Category_Set(int newCat){

            curCategory = newCat;

        }//Category_Set

        public void EventSlot_Set(int newEvent){

            curEvent = newEvent;

        }//EventSlot_Set


    }//HFPS_ScreenEvents


}//namespace