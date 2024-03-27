using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Scene/Scene Action")]
    public class HFPS_SceneAction : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Action {

            [Space]

            public string name;

            [Space]

            public UnityEvent actionEvent;

            [Space]

            public float nextActionWait;

        }//Action


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     USER OPTIONS
    ///
    ///////////////////////////


        public List<Action> actions;


    ///////////////////////////
    ///
    ///     EVENTS
    ///
    ///////////////////////////


        public UnityEvent actionsFinished;


    ///////////////////////////
    ///
    ///     AUTO OPTIONS
    ///
    ///////////////////////////


        public int tempAction;
        public bool done;
        public bool locked;

        public int tabs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            tempAction = 0;
            locked = false;

        }//start


    //////////////////////////////////////
    ///
    ///     SCENE ACTIONS ACTIONS
    ///
    ///////////////////////////////////////


        public void Actions_Init(){

            StartCoroutine("Actions_Start");

        }//Actions_Init

        public void Actions_InitDelayed(float delay){

            StartCoroutine("Actions_StartDelayed", delay);

        }//Actions_InitDelayed

        private IEnumerator Actions_StartDelayed(float delay){

            yield return new WaitForSeconds(delay);

            StartCoroutine("Actions_Start");

        }//Actions_StartDelayed

        private IEnumerator Actions_Start(){

            if(!locked){

                if(actions.Count > 0){

                    tempAction = 0;

                    for(int a = 0; a < actions.Count; a++) {

                        if(!done){

                            tempAction += 1;

                            actions[a].actionEvent.Invoke();

                            yield return new WaitForSeconds(actions[a].nextActionWait);

                            if(tempAction == actions.Count){

                                done = true;

                                actionsFinished.Invoke();

                            }//done

                        }//!done

                    }//for a actions

                }//Actions.Count > 0

            }//!locked

        }//Actions_Start

        public void Actions_Stop(){

            StopCoroutine("Actions_Start");

        }//Actions_Stop


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


    }//HFPS_SceneAction


}//namespace