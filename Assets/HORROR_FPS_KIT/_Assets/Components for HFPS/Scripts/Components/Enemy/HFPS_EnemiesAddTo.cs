using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Enemy/Enemies Add To")]
    public class HFPS_EnemiesAddTo : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        [Header("References")]

        public ZombieBehaviourAI zombAI;

        [Space]

        [Header("User Options")]

        [Space]

        public bool autoAdd;

        [Space]

        [Header("Auto")]

        [Space]

        public HFPS_EnemiesHolder enemHold;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            enemHold = HFPS_EnemiesHolder.instance;

            if(autoAdd){

                if(enemHold != null){

                    if(zombAI == null){

                        zombAI = GetComponent<ZombieBehaviourAI>();

                    }//zombAI == null

                    if(zombAI != null){

                        enemHold.zombAI.Add(zombAI);

                    }//zombAI != null

                //enemHold != null
                } else {

                    Debug.Log("Enemies Holder not found!");

                }//enemHold != null

            }//autoAdd

        }//StartInit


    //////////////////////////////////////
    ///
    ///     LIST ACTIONS
    ///
    ///////////////////////////////////////


        public void RemoveFromList(){

            enemHold.zombAI.Remove(zombAI);

        }//RemoveFromList


    }//HFPS_EnemiesAddTo


}//namespace