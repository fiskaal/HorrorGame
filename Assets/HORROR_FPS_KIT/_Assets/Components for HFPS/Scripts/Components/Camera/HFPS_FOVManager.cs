using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Camera/FOV/FOV Manager")]
    public class HFPS_FOVManager : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_FOVManager instance;


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Camera_State {

            [Space]

            public string name;
            public Cam_State state;

            [Space]

            [Range(0, 179)]
            public int FOV;
            public float zoomMulti = 10f;
            public float unzoomMulti = 5f;

        }//Camera_State

        public enum Cam_State {

            Original = 0,
            Custom = 1,

        }//Cam_State


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public List<Camera_State> states;
        public List<Camera> cameras;

        public float globalZoomMulti = 10f;
        public float globalUnzoomMulti = 5f;

        public float tempWait;
        public float tempFOV;
        public float oldFOV;
        public float tempZoomMulti;

        public bool zoomIN;
        public bool zoomOUT;
        public bool zooming;

        public bool locked;
        private float zoomVelCam;

        public int tabs;

        public bool genOpts;
        public bool statesOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            instance = this;

            StartInit();

        }//start

        public void StartInit(){

            tempWait = 0;
            tempFOV = 0;
            oldFOV = 0;
            tempZoomMulti = 0;

            zoomIN = false;
            zoomOUT = false;
            zooming = false;

            locked = false;

        }//StartInit


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update() {

            if(!locked){

                if(zooming){

                    for(int i = 0; i < cameras.Count; i++){

                        if(zoomIN){

                            cameras[i].fieldOfView = Mathf.SmoothDamp(cameras[i].fieldOfView, tempFOV, ref zoomVelCam, tempZoomMulti * Time.deltaTime);

                        }//zoomIN

                        if(zoomOUT){

                            cameras[i].fieldOfView = Mathf.SmoothDamp(cameras[i].fieldOfView, oldFOV, ref zoomVelCam, tempZoomMulti * Time.deltaTime);

                        }//zoomOUT

                        if(cameras[i].fieldOfView == tempFOV){

                            if(zoomOUT){

                                CamZoomLock_State(false);

                            }//zoomOUT

                            zoomIN = false;
                            zoomOUT = false;
                            zooming = false;

                        }//fieldOfView == tempFOV

                    }//for i cameras

                }//zooming

            }//!locked

        }//update


    //////////////////////////////////////
    ///
    ///     FOV ACTIONS
    ///
    ///////////////////////////////////////


        public void FOV_Update(int newFOV, bool zoomInState){

            if(!locked){

                if(cameras.Count > 0){

                    CamZoomLock_State(true);

                    if(zoomInState){

                        tempFOV = newFOV;
                        oldFOV = cameras[0].fieldOfView;
                        tempZoomMulti = globalZoomMulti;

                        zoomIN = true;

                    //zoomInState
                    } else {

                        tempZoomMulti = globalUnzoomMulti;

                        zoomOUT = true;

                    }//zoomInState

                    zooming = true;

                }//cameras.Count > 0

            }//!locked

        }//FOV_Update

        public void FOV_State(string newState){

            Debug.Log("FOV State call = " + newState);

            if(!locked){

                if(states.Count > 0){

                    CamZoomLock_State(true);

                    for(int i = 0; i < states.Count; i++){

                        if(states[i].name == newState){

                            Debug.Log("State found = " + newState);

                            tempFOV = states[i].FOV;

                            if(states[i].state == Cam_State.Original){

                                tempZoomMulti = states[i].unzoomMulti;

                                zoomOUT = true;

                            }//state = original

                            if(states[i].state == Cam_State.Custom){

                                oldFOV = cameras[0].fieldOfView;
                                tempZoomMulti = states[i].zoomMulti;

                                zoomIN = true;

                            }//state = original

                            zooming = true;

                        }//name = newState

                    }//for i states

                }//states.Count > 0

            }//!locked

        }//FOV_State


    //////////////////////////////////////
    ///
    ///     ZOOM ACTIONS
    ///
    ///////////////////////////////////////


        public void Zoom_In(int newFOV){

            if(!locked){

                if(cameras.Count > 0){

                    CamZoomLock_State(true);

                    tempFOV = newFOV;
                    oldFOV = cameras[0].fieldOfView;
                    tempZoomMulti = globalZoomMulti;

                    zoomIN = true;
                    zooming = true;

                }//cameras.Count > 0

            }//!locked

        }//Zoom_In

        public void Zoom_Out(){

            if(!locked){

                if(cameras.Count > 0){

                    CamZoomLock_State(true);

                    tempZoomMulti = globalUnzoomMulti;

                    zoomOUT = true;
                    zooming = true;

                }//cameras.Count > 0

            }//!locked

        }//Zoom_Out


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void CamZoomLock_State(bool state){

            #if COMPONENTS_PRESENT

                HFPS_References.instance.playerFunct.CamZoomLock_State(state);

            #endif

        }//CamZoomLock_State

        public void Locked_State(bool state){

            locked = state;

        }//Locked_State


    }//HFPS_FOVManager


}//namespace