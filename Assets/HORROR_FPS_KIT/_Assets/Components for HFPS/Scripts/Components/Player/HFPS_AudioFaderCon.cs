using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Player/Audio Fader Connect")]
    public class HFPS_AudioFaderCon : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public HFPS_AudioFader.FadeType type;

        [Space]

        public AudioClip audClip;
        public float volume; 

        [Space]

        public bool keepAmbience;
        public bool immediate;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start(){}


    //////////////////////////////////////
    ///
    ///     FADE ACTIONS
    ///
    ///////////////////////////////////////


        public void Fade_Start(){

            if(HFPS_AudioFader.instance != null){

                HFPS_AudioFader.instance.Fade_Start(type, audClip, volume, keepAmbience, immediate);

            }//instance != null

        }//HFPS_AudioFader


    }//HFPS_AudioFaderCon


}//namespace