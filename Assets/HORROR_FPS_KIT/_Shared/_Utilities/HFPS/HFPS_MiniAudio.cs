using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HFPS.Systems;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/HFPS/Mini Audio")]
    public class HFPS_MiniAudio : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public bool useCustomVolume;
        public float soundVolume;

        private float tempVolume;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start(){

            tempVolume = soundVolume;

        }//start


    //////////////////////////////////////
    ///
    ///     SOUND ACTIONS
    ///
    ///////////////////////////////////////


        public void Play_Sound(AudioClip clip){

            if(ScriptManager.HasReference) {

                if(useCustomVolume){

                    ScriptManager.Instance.SoundEffects.PlayOneShot(clip, tempVolume);

                //useCustomVolume
                } else {

                    ScriptManager.Instance.SoundEffects.PlayOneShot(clip);

                }//useCustomVolume

            }//HasReference

        }//Play_Sound


    //////////////////////////////////////
    ///
    ///     VOLUME ACTIONS
    ///
    ///////////////////////////////////////


        public void Volume_Set(float volume){

            tempVolume = volume;

        }//Volume_Set


    }//HFPS_MiniAudio
    
    
}//namespace
