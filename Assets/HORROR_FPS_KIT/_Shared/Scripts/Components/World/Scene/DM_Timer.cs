using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/World/Scene/Timer")]
    public class DM_Timer : MonoBehaviour, ISaveable {

        
        //////////////////////////////////////
        ///
        ///     CLASSES
        ///
        //////////////////////////////////////
        
        
        [Serializable]
        public class Events {
            
            [Space]
            
            public UnityEvent onTimerStart;
            public UnityEvent onTimerStop;
            
            [Space]
            
            public UnityEvent onTimerPause;
            public UnityEvent onTimerUnPause;
            
            [Space]
            
            public UnityEvent onExpired;
            public UnityEvent onExpiredLoad;
            
        }//Events
        
        
        //////////////////////////////////////
        ///
        ///     ENUMS
        ///
        //////////////////////////////////////
        
        
        public enum Timer_Type {
            
            Auto = 0,
            Manual = 1,
            
        }//Timer_Type
        
        
        //////////////////////////////////////
        ///
        ///     VALUES
        ///
        //////////////////////////////////////
        
        
        [Space]
        
        public Timer_Type timerType;
        
        [Space]
        
        public float totalTime;
        public float reduceAmount;
        
        [Space]
        
        public Events events;
        
        [Space]
        
        [Header("Auto")]
        
        public float curTime;
        public bool reduceTime;
        public bool timeExpired;
        public bool locked;
        
        
        //////////////////////////////////////
        ///
        ///     START ACTIONS
        ///
        //////////////////////////////////////
        
        
        void Start() {

            StartInit();

        }//start
        
        public void StartInit(){
            
            curTime = 0;
            timeExpired = false;
            locked = false;
            
            if(timerType == Timer_Type.Auto){
                
                curTime = totalTime;
                reduceTime = true;
                
                events.onTimerStart.Invoke();
                
            }//timerType = auto
            
        }//StartInit

        
        //////////////////////////////////////
        ///
        ///     UPDATE ACTIONS
        ///
        //////////////////////////////////////
        

        void Update() {

            if(!locked){
                
                if(reduceTime){
                    
                    curTime -= reduceAmount * Time.deltaTime;
                    
                    if(curTime <= 0){
                        
                        reduceTime = false;
                        
                        Timer_Expired();
                        
                    }//curTime <= 0
                    
                }//reduceTime
                
            }//!locked
            
        }//update
        
        
        //////////////////////////////////////
        ///
        ///     TIMER ACTIONS
        ///
        //////////////////////////////////////
        
        
        public void Timer_Start(){
            
            curTime = totalTime;
            reduceTime = true;
            
        }//Timer_Start
        
        public void Timer_Pause(bool state){
            
            if(state){
                
                events.onTimerPause.Invoke();
                
                reduceTime = false;
                
            //state
            } else {
                
                if(curTime > 0){
                    
                    events.onTimerUnPause.Invoke();
                
                    reduceTime = true;
                
                //curTime > 0
                } else {
                 
                    reduceTime = false;
                    
                }//curTime > 0
                    
            }//state
            
        }//Timer_Pause
        
        public void Timer_Stop(){
            
            reduceTime = false;
            curTime = 0;
            
            events.onTimerStop.Invoke();
            
        }//Timer_Stop
        
        public void Timer_Expired(){
            
            events.onExpired.Invoke();
            
            timeExpired = true;
            curTime = 0;
            
        }//Timer_Expired
        
        
    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"curTime", curTime },
                {"reduceTime", reduceTime },
                {"timeExpired", timeExpired },
                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            curTime = (float)token["curTime"];
            reduceTime = (bool)token["reduceTime"];
            timeExpired = (bool)token["timeExpired"];
            locked = (bool)token["locked"];
            
            if(timeExpired){
                
                events.onExpiredLoad.Invoke();
                
            }//timeExpired

        }//OnLoad


    }//DM_Timer
    
    
}//namespace
