using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Camera/FOV/FOV Manager Connect")]
    public class HFPS_FOVManagerCon : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {}


    //////////////////////////////////////
    ///
    ///     FOV ACTIONS
    ///
    ///////////////////////////////////////


        public void FOV_State(string newState){

            HFPS_FOVManager.instance.FOV_State(newState);

        }//FOV_State


    //////////////////////////////////////
    ///
    ///     ZOOM ACTIONS
    ///
    ///////////////////////////////////////


        public void Zoom_In(int newFOV){

            HFPS_FOVManager.instance.Zoom_In(newFOV);

        }//Zoom_In

        public void Zoom_Out(){

            HFPS_FOVManager.instance.Zoom_Out();

        }//Zoom_Out


    //////////////////////////////////////
    ///
    ///     LOCK ACTIONS
    ///
    ///////////////////////////////////////


        public void Locked_State(bool state){

            HFPS_FOVManager.instance.Locked_State(state);

        }//Locked_State


    }//HFPS_FOVManagerCon


}//namespace