using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/UI/Display/Simple Fade")]
    public class SimpleFade : MonoBehaviour {


    //////////////////////////
    //
    //      INSTANCE
    //
    //////////////////////////


        public static SimpleFade instance;


    //////////////////////////
    //
    //      ENUMS
    //
    //////////////////////////


        public enum InitState {

            None = 0,
            FadeIn = 1,
            FadeOut = 2,

        }//InitState

        public enum AnimType {

            A_String = 0,
            Bool = 1,

        }//AnimType


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////

    /////////////////
    //
    //   DEBUG
    //
    /////////////////


        public bool useDebug;


    /////////////////
    //
    //   REFERENCES
    //
    /////////////////


        public Animator fadeAnim;
        public AudioSource audSource;


    /////////////////
    //
    //   START OPTIONS
    //
    /////////////////


        public InitState initState;

        public bool initWait;
        public float waitTime;


    /////////////////
    //
    //   ANIMATION OPTIONS
    //
    /////////////////


        public AnimType animType;
        public string fadeInString;
        public string fadeOutString;


    /////////////////
    //
    //   SOUND OPTIONS
    //
    /////////////////


        public bool useFadeSound;
        public AudioClip audClip;
        public float audVol;


    /////////////////
    //
    //   AUTO
    //
    /////////////////


        public bool init;
        public bool locked;

        public int debugInt;
        public int simpFadeTabs;
        public bool startOpts;
        public bool animOpts;
        public bool soundOpts;

        public bool audioRefs;
        public bool dispRefs;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Awake() {

            instance = this;

        }//Awake

        void Start() {

            init = true;
            StartInit();

        }//Start

        public void StartInit(){

            locked = false;

            if(initWait){

                StartCoroutine("InitBuff");

            //initWait
            } else {

                Fade_Init();

            }//initWait

        }//StartInit

        private IEnumerator InitBuff(){

            yield return new WaitForSeconds(waitTime);

            Fade_Init();

        }//InitBuff

        public void Fade_Init(){

            if(useFadeSound){

                audSource.PlayOneShot(audClip, audVol);

            }//useFadeSound

            if((int)initState == 1){

                if((int)animType == 0){

                    fadeAnim.Play(fadeInString);

                }//animType = string

                if((int)animType == 1){

                    fadeAnim.SetBool(fadeOutString, false);
                    fadeAnim.SetBool(fadeInString, true);

                }//animType = bool

            }//initState = fade in

            if((int)initState == 2){

                if((int)animType == 0){

                    fadeAnim.Play(fadeOutString);

                }//animType = string

                if((int)animType == 1){

                    fadeAnim.SetBool(fadeInString, false);
                    fadeAnim.SetBool(fadeOutString, true);

                }//animType = bool

            }//initState = fade out

            StartCoroutine("InitUnlock");

        }//Fade_Init

        private IEnumerator InitUnlock(){

            yield return new WaitForSeconds(0.2f);

            init = false;

        }//InitUnlock


    //////////////////////////
    //
    //      FADE ACTIONS
    //
    //////////////////////////


        public void Fade_In(){

            if(!init){

                if(useFadeSound){

                    audSource.PlayOneShot(audClip, audVol);

                }//useFadeSound

                if((int)animType == 0){

                    fadeAnim.Play(fadeInString);

                }//animType = string

                if((int)animType == 1){

                    fadeAnim.SetBool(fadeOutString, false);
                    fadeAnim.SetBool(fadeInString, true);

                }//animType = bool

                if(useDebug){

                    Debug.Log("FadeIn");

                }//useDebug

            }//!init

        }//Fade_In

        public void FadeIn_Delay(float delay){

            StartCoroutine("FadeInBuff", delay);

        }//FadeOut_Delay

        public IEnumerator FadeInBuff(float delay){

            yield return new WaitForSeconds(delay);

            Fade_In();

        }//FadeOutBuff

        public void Fade_Out(){

            if(!init){

                if(useFadeSound){

                    audSource.PlayOneShot(audClip, audVol);

                }//useFadeSound

                if((int)animType == 0){

                    fadeAnim.Play(fadeOutString);

                }//animType = string

                if((int)animType == 1){

                    fadeAnim.SetBool(fadeInString, false);
                    fadeAnim.SetBool(fadeOutString, true);

                }//animType = bool

                if(useDebug){

                    Debug.Log("FadeOut");

                }//useDebug

            }//!init

        }//Fade_Out

        public void FadeOut_Delay(float delay){

            StartCoroutine("FadeOutBuff", delay);

        }//FadeOut_Delay

        public IEnumerator FadeOutBuff(float delay){

            yield return new WaitForSeconds(delay);

            Fade_Out();

        }//FadeOutBuff


    }//SimpleFade


}//namespace