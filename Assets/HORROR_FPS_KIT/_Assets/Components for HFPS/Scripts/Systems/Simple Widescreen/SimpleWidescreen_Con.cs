using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Widescreen/Simple Widescreen Connect")]
    public class SimpleWidescreen_Con : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start(){}//Start


    //////////////////////////////////////
    ///
    ///     WIDESCREEN ACTIONS
    ///
    ///////////////////////////////////////

    //////////////////////
    ///
    ///     IN ACTIONS
    ///
    //////////////////////


        public void WidescreenIN_Delayed(float time){

            SimpleWidescreen.instance.WidescreenIN_Delayed(time);

        }//WidescreenIN_Delayed

        public void Widescreen_IN(){

            SimpleWidescreen.instance.Widescreen_IN();

        }//Widescreen_IN

        public void Widescreen_IN(float time){

            SimpleWidescreen.instance.Widescreen_IN(time);

        }//Widescreen_IN


    //////////////////////
    ///
    ///     OUT ACTIONS
    ///
    //////////////////////


        public void WidescreenOUT_Delayed(float time){

            SimpleWidescreen.instance.WidescreenOUT_Delayed(time);

        }//WidescreenOUT_Delayed

        public void Widescreen_OUT(){

            SimpleWidescreen.instance.Widescreen_OUT();

        }//Widescreen_OUT


    }//SimpleWidescreen_Con


}//namespace