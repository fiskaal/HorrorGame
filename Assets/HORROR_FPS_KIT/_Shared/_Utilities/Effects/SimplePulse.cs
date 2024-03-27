using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/Effects/Simple Pulse")]
    public class SimplePulse : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        [Space]

        [Header("References")]

        [Space]

        public Renderer meshRenderer;

        [Space]

        [Header("User Options")]

        [Space]

        public Color baseColor;
        public Color offColor;
        public int matIndex;

        [Space]

        public int timeMulti;
        public float floor = 0.3f;
        public float ceiling = 1.0f;

        [Space]

        [Header("Auto")]

        [Space]

        public Material[] tempMats;
        public float curEmission;
        public bool locked;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            tempMats = meshRenderer.materials;

        }//Start


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update() {

            if(!locked){

                 curEmission = Mathf.PingPong (Time.time * timeMulti, ceiling - floor);
                 Color finalColor = baseColor * Mathf.LinearToGammaSpace(curEmission);
                 tempMats[matIndex].SetColor ("_EmissionColor", finalColor);

            }//!locked

        }//Update


    //////////////////////////////////////
    ///
    ///     COLOR ACTIONS
    ///
    ///////////////////////////////////////


        public void PulseSet_Off(){

            locked = true;
            curEmission = 0;
            ColorSet_Off();

        }//PulseSet_Off

        public void PulseSet_On(){

            curEmission = floor;
            locked = false;

        }//PulseSet_On


    //////////////////////////////////////
    ///
    ///     COLOR ACTIONS
    ///
    ///////////////////////////////////////


        public void Color_Set(Color newColor){

            newColor = baseColor * Mathf.LinearToGammaSpace(curEmission);
            tempMats[matIndex].SetColor ("_EmissionColor", newColor);

        }//Color_Set

        public void ColorSet_Off(){

            offColor = baseColor * Mathf.LinearToGammaSpace(curEmission);
            tempMats[matIndex].SetColor ("_EmissionColor", offColor);

        }//ColorSet_Off


    //////////////////////////////////////
    ///
    ///     EMISSION ACTIONS
    ///
    ///////////////////////////////////////


        public void EmissionSet_Floor(){

            curEmission = floor;

        }//EmissionSet_Floor

        public void EmissionSet_Ceiling(){

            curEmission = ceiling;

        }//EmissionSet_Ceiling

        public void EmissionSet_Custom(float newEmission){

            curEmission = newEmission;

        }//EmissionSet_Custom


    //////////////////////////////////////
    ///
    ///     LOCK ACTIONS
    ///
    ///////////////////////////////////////


        public void Lock_State(bool state){

            locked = state;

        }//Lock_State


    }//SimplePulse


}//namespace
