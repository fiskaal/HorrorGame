using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Sub Actions/Sub Action")]
    public class HFPS_SubAction : MonoBehaviour {


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class Sound_Library {

            public string name;

            [Space]

            public AudioSource source;
            public AudioClip clip;
            public float volume;

        }//Sound Library


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////


        public GameObject holder;
        public AnimationClip actionAnim;
        public float extraTime;

        public List<Sound_Library> soundLibrary;


    //////////////////////////
    //
    //      AUTO
    //
    //////////////////////////


        public int curSoundSlot;
        public bool locked;

        public HFPS_SubActionsHandler subActionHandler;

        public int tabs;

        public bool genOpts;
        public bool animOpts;
        public bool soundOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            curSoundSlot = -1;
            locked = false;

        }//StartInit


    //////////////////////////
    //
    //      SUB ACTION ACTIONS
    //
    //////////////////////////


        public void SubAction_Init(){

            holder.SetActive(true);

            StartCoroutine("SubAction_End");

        }//SubAction_Init

        private IEnumerator SubAction_End(){

            yield return new WaitForSeconds(actionAnim.length + extraTime);

            holder.SetActive(false);

        }//SubAction_End


    //////////////////////////
    //
    //      SUB ACTION EVENTS
    //
    //////////////////////////


        public void SubAction_Sound(){

            soundLibrary[curSoundSlot - 1].source.PlayOneShot(soundLibrary[curSoundSlot - 1].clip, soundLibrary[curSoundSlot - 1].volume);

        }//SubAction_Sound


    //////////////////////////
    //
    //      SOUND SLOT ACTIONS
    //
    //////////////////////////


        public void SoundSlot_Set(int slot){

            curSoundSlot = slot;

        }//SoundSlot_Set

        public void SoundSlot_Reset(){

            curSoundSlot = -1;

        }//SoundSlot_Reset


    }//HFPS_SubAction


}//namespace