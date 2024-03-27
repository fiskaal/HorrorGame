using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Scene/Sound Library")]
    public class HFPS_SoundLibrary : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Sound_Library {

            public string name;

            [Space]

            public AudioSource source;
            public List<AudioClip> clips;
            public float volume;

        }//Sound Library


    //////////////////////////////////////
    ///
    ///     VAUES
    ///
    ///////////////////////////////////////


        public List<Sound_Library> soundLibrary;

        public int tempSound;
        public RaycastHit tempRayHit;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start(){

            tempSound = 0;

        }//start


    //////////////////////////////////////
    ///
    ///     HIT ACTIONS
    ///
    ///////////////////////////////////////


        public void ReceiveRaycast(RaycastHit hit){

            tempRayHit = hit;

        }//ReceiveRaycast


    //////////////////////////////////////
    ///
    ///     SOUND ACTIONS
    ///
    ///////////////////////////////////////


        public void LibraryPlay_Sound(int slot){

            int tempSound = 0;

            if(soundLibrary[slot - 1].clips.Count > 0){

                tempSound = UnityEngine.Random.Range(0, soundLibrary[slot - 1].clips.Count);

            }//clips.Count > 0

            if(soundLibrary[slot - 1].source != null){

                if(soundLibrary[slot - 1].volume > 0){

                    soundLibrary[slot - 1].source.PlayOneShot(soundLibrary[slot - 1].clips[tempSound], soundLibrary[slot - 1].volume);

                //volume > 0
                } else {

                    soundLibrary[slot - 1].source.PlayOneShot(soundLibrary[slot - 1].clips[tempSound]);

                }//volume > 0

            //source != null
            } else {

                if(soundLibrary[slot - 1].volume > 0){

                    AudioSource.PlayClipAtPoint(soundLibrary[slot - 1].clips[tempSound], tempRayHit.transform.position, soundLibrary[slot - 1].volume);

                //volume > 0
                } else {

                    AudioSource.PlayClipAtPoint(soundLibrary[slot - 1].clips[tempSound], tempRayHit.transform.position);

                }//volume > 0

            }//source != null

        }//LibraryPlay_Sound


    }//HFPS_SoundLibrary


}//namespace