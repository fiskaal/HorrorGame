using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using HFPS.Systems;

#if TW_LOCALIZATION_PRESENT

    using ThunderWire.Localization;

#endif

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/UI/Display/Complex Notifications/Complex Notifications")]
    public class ComplexNotifications : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static ComplexNotifications instance;


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Notifications {

            [Space]

            public string name;
            public GameObject holder;

            [Space]

            public Animations animation;

            [Space]

            public List<Texts> texts;

            [Space]

            public Sound sound;

            [Space]

            public UnityEvent finishEvent;

        }//Notifications

        [Serializable]
        public class Animations {

            [Space]

            public bool useAnim;
            public Animator anim;
            public AnimationClip show;
            public AnimationClip hide;
            public float animBuff;

        }//Animations

        [Serializable]
        public class Texts {

            [Space]

            public string name;

            [Space]

            public Text text;
            public TMP_Text textMeshPro;

            [Space]

            public bool customText;
            public string notification;

            [Space]

            public bool customColor;
            public Color textColor;

        }//Texts

        [Serializable]
        public class Sound {

            [Space]

            public bool useSound;
            public AudioClip sound;
            public float volume;

            [Space]

            public bool delaySound;
            public float soundWait;

        }//Sound
        
        [Serializable]
        public class Texts_Temp {
        
            [Space]
            
            public string text;
            public string key;
        
        }//Texts_Temp


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    //////////////////////////
    //
    //      REFERENCES
    //
    //////////////////////////


        public AudioSource audSource;


    //////////////////////////
    //
    //      NOTIFICATIONS
    //
    //////////////////////////


        public List<Notifications> notifications;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public int curNotif;
        public List<string> tempTexts;

        public float tempWait;
        public float tempHideWait;
        public float tempVolume;
        public bool locked;

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

            curNotif = 0;
            tempTexts = new List<string>();

            tempWait = 0;
            tempHideWait = 0;
            tempVolume = 0;

            locked = false;

        }//StartInit
        

    //////////////////////////////////////
    ///
    ///     AUDIO ACTIONS
    ///
    ///////////////////////////////////////


        public void Audio_Play(AudioClip clip){

            if(audSource != null){

                audSource.PlayOneShot(clip, audSource.volume);

            }//audSource != null

        }//Audio_Play

        public void Audio_Play(AudioClip clip, float volume){

            if(audSource != null){

                audSource.PlayOneShot(clip, volume);

            }//audSource != null

        }//Audio_Play

        private IEnumerator AudioPlay_Delayed(AudioClip clip){

            yield return new WaitForSeconds(tempWait);

            if(tempVolume > 0){

                Audio_Play(clip, tempVolume);

            //tempVolume > 0
            } else {

                Audio_Play(clip);

            }//tempVolume > 0

        }//AudioPlay_Delayed


    //////////////////////////////////////
    ///
    ///     NOTIFICATION ACTIONS
    ///
    ///////////////////////////////////////

    //////////////////////////
    //
    //      SHOW DELAYED
    //
    //////////////////////////


        public void Notification_ShowDelayed(string name){

            StartCoroutine("NotifShowDelayBuff", name);

        }//Notification_ShowDelayed

        private IEnumerator NotifShowDelayBuff(string name){

            yield return new WaitForSeconds(tempWait);

            Notification_Show(name);

        }//NotifShowDelayBuff


        public void Notification_ShowDelayed(int slot){

            StartCoroutine("NotifShowDelayBuff", slot);

        }//Notification_ShowDelayed

        private IEnumerator NotifShowDelayBuff(int slot){

            yield return new WaitForSeconds(tempWait);

            Notification_Show(slot);

        }//NotifShowDelayBuff


    //////////////////////////
    //
    //      SHOW STRING - REGULAR
    //
    //////////////////////////


        public void Notification_Show(string name){

            for(int i = 0; i < notifications.Count; i++) {

                if(notifications[i].name == name){

                    curNotif = i + 1;

                    Notification_Show(curNotif);
                    
                }//name = name

            }//for i notifications

        }//Notification_Show STRING


    //////////////////////////
    //
    //      SHOW STRING - W LIST
    //
    //////////////////////////


        public void Notification_Show(string name, List<string> texts){

            for(int i = 0; i < notifications.Count; i++) {

                if(notifications[i].name == name){
                
                    curNotif = i + 1;

                    Notification_Show(curNotif, texts);

                }//name = name

            }//for i notifications

        }//Notification_Show STRING and string list


    //////////////////////////
    //
    //      SHOW INT - REGULAR
    //
    //////////////////////////


        public void Notification_Show(int slot){
        
            curNotif = slot;
            
            List<string> keys = new List<string>();
            tempTexts = new List<string>();
            
            string tempString = "";

            for(int t = 0; t < notifications[slot - 1].texts.Count; t++) {
            
                if(notifications[slot - 1].texts[t].customText){
                
                    if(notifications[slot - 1].texts[t].notification != ""){
                    
                        tempString = TextsSource.GetText(notifications[slot - 1].texts[t].notification, "Notification");
                        
                        if(tempString == "Notification"){
                        
                            tempString = notifications[slot - 1].texts[t].notification;

                        //tempString = default / empty
                        } else {
                        
                            if(!keys.Contains(notifications[slot - 1].texts[t].notification)){
                            
                                keys.Add(notifications[slot - 1].texts[t].notification);
                        
                            }//!contains
                            
                        }//tempString = default / empty
                        
                        tempTexts.Add(tempString);
                        
                        if(tempString != ""){
                            
                            if(notifications[slot - 1].texts[t].text != null){

                                notifications[slot - 1].texts[t].text.text = tempString;

                            }//notification != null

                            if(notifications[slot - 1].texts[t].textMeshPro != null){

                                notifications[slot - 1].texts[t].textMeshPro.text = tempString;

                            }//textMeshPro != null
                        
                        }//tempString != null
                    
                    }//notification != null
                
                }//customText
                    
            }//for t texts

            if(notifications[slot - 1].holder != null){

                notifications[slot - 1].holder.SetActive(true);

            }//holder != null

            if(notifications[slot - 1].animation.useAnim){

                if(notifications[slot - 1].animation.anim != null){

                    if(notifications[slot - 1].animation.show != null){

                        notifications[slot - 1].animation.anim.Play(notifications[slot - 1].animation.show.name);

                    }//show != null

                    if(notifications[slot - 1].animation.hide != null){

                        tempHideWait = notifications[slot - 1].animation.show.length + notifications[slot - 1].animation.animBuff;

                        StartCoroutine("Notification_BuffDelayed", slot);

                    //hide != null
                    } else {

                        tempHideWait = notifications[slot - 1].animation.show.length + notifications[slot - 1].animation.animBuff;

                        StartCoroutine("Notification_Buff", slot);

                    }//hide != null

                }//anim != null

            }//useAnim

            if(notifications[slot - 1].sound.useSound){

                if(audSource != null){

                    if(notifications[slot - 1].sound.sound != null){

                        if(!notifications[slot - 1].sound.delaySound){

                            if(notifications[slot - 1].sound.volume > 0){

                                Audio_Play(notifications[slot - 1].sound.sound, notifications[slot - 1].sound.volume);

                            //volume > 0
                            } else {

                                Audio_Play(notifications[slot - 1].sound.sound);

                            }//volume > 0

                        //!delaySound
                        } else {

                            if(notifications[slot - 1].sound.soundWait > 0){

                                tempWait = notifications[slot - 1].sound.soundWait;

                                if(notifications[slot - 1].sound.volume > 0){

                                    tempVolume = notifications[slot - 1].sound.volume;

                                //volume > 0
                                } else {

                                    tempVolume = 0;

                                }//volume > 0

                                StartCoroutine("AudioPlay_Delayed", notifications[slot - 1].sound.sound);

                            }//soundWait > 0

                        }//!delaySound

                    }//sound != null

                }//audSource != null

            }//useSound
            
            #if TW_LOCALIZATION_PRESENT
                
                if(HFPS_GameManager.LocalizationEnabled){
                
                    if(keys.Count > 0){
                
                        LocalizationSystem.SubscribeAndGet(OnChangeLocalization, keys.ToArray());
            
                    }//keys.Count > 0
            
                }//LocalizationEnabled
            
            #endif

        }//Notification_Show INT


    //////////////////////////
    //
    //      SHOW INT - WITH LIST
    //
    //////////////////////////


        public void Notification_Show(int slot, List<string> texts){
        
            curNotif = slot;
            
            List<string> keys = new List<string>();
            tempTexts = new List<string>();
            
            string tempString = "";

            if(texts.Count > 0){

                for(int t = 0; t < notifications[slot - 1].texts.Count; t++) {
                
                    if(!notifications[slot - 1].texts[t].customText){
                    
                        tempString = TextsSource.GetText(texts[t], "Notification");
                        
                        if(tempString == "Notification"){
                        
                            tempString = texts[t];

                        //tempString = default / empty
                        } else {
                        
                            if(!keys.Contains(texts[t])){
                            
                                keys.Add(texts[t]);
                        
                            }//!contains
                            
                        }//tempString = default / empty
                        
                        tempTexts.Add(tempString);
                        
                    //!customText
                    } else {
                    
                        if(notifications[slot - 1].texts[t].notification != ""){

                            tempString = TextsSource.GetText(notifications[slot - 1].texts[t].notification, "Notification");
                            
                            if(tempString == "Notification"){

                                tempString = notifications[slot - 1].texts[t].notification;

                            //tempString = default / empty
                            } else {

                                if(!keys.Contains(texts[t])){

                                    keys.Add(texts[t]);

                                }//!contains

                            }//tempString = default / empty
                            
                            tempTexts.Add(tempString);

                        }//notification != null
                    
                    }//!customText
                    
                    if(tempString != ""){
                    
                        if(notifications[slot - 1].texts[t].text != null){
                        
                            notifications[slot - 1].texts[t].text.text = tempString;
                        
                        }//text != null
                        
                        if(notifications[slot - 1].texts[t].textMeshPro != null){
                        
                            notifications[slot - 1].texts[t].textMeshPro.text = tempString;
                        
                        }//textMeshPro != null
                        
                    }//tempString != null

                }//for t texts

            }//texts.Count > 0

            if(notifications[slot - 1].holder != null){

                notifications[slot - 1].holder.SetActive(true);

            }//holder != null

            if(notifications[slot - 1].animation.useAnim){

                if(notifications[slot - 1].animation.anim != null){

                    if(notifications[slot - 1].animation.show != null){

                        notifications[slot - 1].animation.anim.Play(notifications[slot - 1].animation.show.name);

                    }//show != null

                    if(notifications[slot - 1].animation.hide != null){

                        tempHideWait = notifications[slot - 1].animation.show.length + notifications[slot - 1].animation.animBuff;

                        StartCoroutine("Notification_BuffDelayed", slot);

                    //hide != null
                    } else {

                        tempHideWait = notifications[slot - 1].animation.show.length + notifications[slot - 1].animation.animBuff;

                        StartCoroutine("Notification_Buff", slot);

                    }//hide != null

                }//anim != null

            }//useAnim

            if(notifications[slot - 1].sound.useSound){

                if(audSource != null){

                    if(notifications[slot - 1].sound.sound != null){

                        if(!notifications[slot - 1].sound.delaySound){

                            if(notifications[slot - 1].sound.volume > 0){

                                Audio_Play(notifications[slot - 1].sound.sound, notifications[slot - 1].sound.volume);

                            //volume > 0
                            } else {

                                Audio_Play(notifications[slot - 1].sound.sound);

                            }//volume > 0

                        //!delaySound
                        } else {

                            if(notifications[slot - 1].sound.soundWait > 0){

                                tempWait = notifications[slot - 1].sound.soundWait;

                                if(notifications[slot - 1].sound.volume > 0){

                                    tempVolume = notifications[slot - 1].sound.volume;

                                //volume > 0
                                } else {

                                    tempVolume = 0;

                                }//volume > 0

                                StartCoroutine("AudioPlay_Delayed", notifications[slot - 1].sound.sound);

                            }//soundWait > 0

                        }//!delaySound

                    }//sound != null

                }//audSource != null

            }//useSound
            
            #if TW_LOCALIZATION_PRESENT
                
                if(HFPS_GameManager.LocalizationEnabled){
                
                    if(keys.Count > 0){
            
                        LocalizationSystem.SubscribeAndGet(OnChangeLocalization, keys.ToArray());

                    }//keys.Count > 0

                }//LocalizationEnabled
            
            #endif

        }//Notification_Show INT and string list


    //////////////////////////
    //
    //      BUFFS
    //
    //////////////////////////


        private IEnumerator Notification_BuffDelayed(int slot){

            yield return new WaitForSeconds(tempHideWait);

            notifications[slot - 1].animation.anim.Play(notifications[slot - 1].animation.hide.name);

            tempHideWait = notifications[slot - 1].animation.hide.length + 0.3f;

            StartCoroutine("Notification_Buff", slot);

        }//Notification_BuffDelayed

        private IEnumerator Notification_Buff(int slot){

            yield return new WaitForSeconds(tempHideWait);

            if(notifications[slot - 1].holder != null){

                notifications[slot - 1].holder.SetActive(false);

            }//holder != null

            notifications[slot - 1].finishEvent.Invoke();
            
            curNotif = 0;
            tempTexts = new List<string>();
            
            #if TW_LOCALIZATION_PRESENT
            
                if(HFPS_GameManager.LocalizationEnabled){
            
                    OnUnsubscribe();
                
                }//LocalizationEnabled

            #endif

        }//Notification_Buff
        
        
    //////////////////////////
    //
    //      LOCALIZATION ACTIONS
    //
    //////////////////////////
        
        
        #if TW_LOCALIZATION_PRESENT
        
            void OnChangeLocalization(string[] texts) {

                if(texts.Length > 0){

                    for(int t = 0; t < texts.Length; ++t ) {

                        if(texts[t] != ""){
                        
                            tempTexts[t] = texts[t];

                            if(notifications[curNotif - 1].texts[t].text != null){

                                notifications[curNotif - 1].texts[t].text.text = tempTexts[t];

                            }//text != null

                            if(notifications[curNotif - 1].texts[t].textMeshPro != null){

                                notifications[curNotif - 1].texts[t].textMeshPro.text = tempTexts[t];

                            }//textMeshPro != null

                        }//tempString != null
                        
                    }//for t texts

                }//texts.Count > 0

            }//OnChangeLocalization
            
            void OnUnsubscribe(){
            
                LocalizationSystem.Unsubscribe(this);
            
            }//OnUnsubscribe
        
        #endif


    }//ComplexNotifications


}//namespace