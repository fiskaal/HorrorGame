using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Enemy/Enemies Holder")]
    public class HFPS_EnemiesHolder : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_EnemiesHolder instance;


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        [Header("Auto")]

        [Space]

        public List<ZombieBehaviourAI> zombAI;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

        }//Awake

        void Start(){}


    }//HFPS_EnemiesHolder


}//namespace