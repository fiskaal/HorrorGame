using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Hit/Health Spot")]
    public class HFPS_HealthSpot : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Health {

            [Space]

            public int minHealth;
            public int maxHealth;

        }//Health

        [Serializable]
        public class Events {

            [Space]

            public UnityEvent onDamage;
            public UnityEvent onHealthReset;
            public UnityEvent onDeath;

        }//Events


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public Health health;
        public Events events;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public int curHealth;
        public bool isDead;

        public int tabs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            Health_Set();

        }//StartInit


    //////////////////////////////////////
    ///
    ///     HEALTH ACTIONS
    ///
    ///////////////////////////////////////


        public void Health_Set(){

            curHealth = health.maxHealth;
            isDead = false;

        }//Health_Set

        public void Health_Reset(){

            curHealth = health.maxHealth;
            isDead = false;

            events.onHealthReset.Invoke();

        }//Health_Reset

        public void Damage(int amount){

            if(!isDead){

                curHealth -= amount;

                events.onDamage.Invoke();

                if(curHealth <= health.minHealth){

                    isDead = true;

                    Death();

                }//curHealth <= minHealth

            }//!isDead

        }//Damage

        public void Death(){

            isDead = true;

            events.onDeath.Invoke();

        }//Death


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"isDead", isDead }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            isDead = (bool)token["isDead"];

            if(isDead){

                curHealth = 0;

            }//locked

        }//OnLoad


    }//HFPS_HealthSpot


}//namespace