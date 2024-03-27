using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Components/UI/Display/Simple Fade Connect")]
    public class SimpleFade_Con : MonoBehaviour {

        void Start() {}//start


    //////////////////////////
    //
    //      FADE ACTIONS
    //
    //////////////////////////


        public void Fade_In(){

            SimpleFade.instance.Fade_In();

        }//Fade_In

        public void FadeIn_Delay(float delay){

            SimpleFade.instance.FadeIn_Delay(delay);

        }//FadeOut_Delay

        public void Fade_Out(){

            SimpleFade.instance.Fade_Out();

        }//Fade_Out

        public void FadeOut_Delay(float delay){

            SimpleFade.instance.FadeOut_Delay(delay);

        }//FadeOut_Delay


    }//SimpleFade_Con
    
    
}//namespace
