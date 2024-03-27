using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ThunderWire.Utility;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Scene/Objectives/Objective Push")]
    public class HFPS_ObjectivePush : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public bool useSound;

        private float tempWait;
        private bool usingSound;

        private ObjectiveManager objectiveManager;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            objectiveManager = ObjectiveManager.Instance;

        }//awake

        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            tempWait = 0;
            usingSound = useSound;

        }//StartInit


    //////////////////////////////////////
    ///
    ///     OBJECTIVES ACTIONS
    ///
    ///////////////////////////////////////


        public void Objective_Push(int objectiveID) {

            #if COMPONENTS_PRESENT

                if(!objectiveManager.CheckObjective(objectiveID)) {

                    ObjectiveModel objModel = objectiveManager.activeObjectives.FirstOrDefault(o => o.identifier == objectiveID);

                    if(!objModel.isCompleted) {

                        string text = objModel.objectiveText;

                        if(text.Contains("{") && text.Contains("}")) {

                            text = string.Format(text, objModel.completion, objModel.toComplete);

                        }//Contains

                        objectiveManager.PushObjectiveText(text, objectiveManager.CompleteTime);

                        if(usingSound) { 

                            objectiveManager.PlaySound(objectiveManager.newObjective); 

                        }//usingSound

                    }//!isCompleted

                }//!CheckObjective

            #else 

                Debug.Log("Components is not active!");

            #endif

        }//Objective_Push

        public void Objective_Push(int objectiveID, bool sound) {

            #if COMPONENTS_PRESENT

                if(!objectiveManager.CheckObjective(objectiveID)) {

                    ObjectiveModel objModel = objectiveManager.activeObjectives.FirstOrDefault(o => o.identifier == objectiveID);

                    if(!objModel.isCompleted) {

                        string text = objModel.objectiveText;

                        if(text.Contains("{") && text.Contains("}")) {

                            text = string.Format(text, objModel.completion, objModel.toComplete);

                        }//Contains

                        objectiveManager.PushObjectiveText(text, objectiveManager.CompleteTime);

                        if(sound) { 

                            objectiveManager.PlaySound(objectiveManager.newObjective); 

                        }//sound

                    }//!isCompleted

                }//!CheckObjective

            #else 

                Debug.Log("Components is not active!");

            #endif

        }//Objective_Push

        public void ObjectivePush_Delayed(int objectiveID){

            StartCoroutine("ObjectivePush_Buff", objectiveID);

        }//ObjectivePush_Delayed

        private IEnumerator ObjectivePush_Buff(int objectiveID){

            yield return new WaitForSeconds(tempWait);

            Objective_Push(objectiveID);

        }//ObjectivePush_Buff


    //////////////////////////////////////
    ///
    ///     SET ACTIONS
    ///
    ///////////////////////////////////////


        public void PushDelay_Set(float wait){

            tempWait = wait;

        }//PushDelay_Set


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void Sound_State(bool state){

            usingSound = state;

        }//Sound_State


    //////////////////////////////////////
    ///
    ///     DESTROY ACTIONS
    ///
    ///////////////////////////////////////


        void OnDestroy(){

            objectiveManager = null;

        }//OnDestroy


    }//HFPS_ObjectivePush


}//namespace