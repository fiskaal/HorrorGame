using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/AI/Possessed")]
    public class HFPS_Possessed : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Events {

            [Space]

            public UnityEvent onMoveStart;
            public UnityEvent onMoveEnd;

        }//Events


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Move_State {

            StartMove = 0,
            Normal = 1,

        }//Move_State

        public enum Move_Type {

            Single = 0,
            Constant = 1,

        }//Move_Type

        public enum Target_Type {

            Custom = 0,
            Player = 1,

        }//Target_Type

        public enum Destroy_Call {

            Manual = 0,
            Auto = 1,

        }//Destroy_Call

        public enum Destroy_Type {

            Destroy = 0,
            Disable = 1,

        }//Destroy_Type


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     START OPTIONS
    ///
    ///////////////////////////


        public bool autoStart;
        public bool useStartDelay;
        public float startDelay;


    ///////////////////////////
    ///
    ///     TARGET OPTIONS
    ///
    ///////////////////////////


        public Target_Type targetType;

        [TagSelectorAttribute]
        public string playerTag;


    ///////////////////////////
    ///
    ///     MOVEMENT OPTIONS
    ///
    ///////////////////////////


        public Move_Type moveType;
        public Transform moveTrans;

        public bool useStartMove;
        public Transform startMove;
        public float startMoveFinishDelay;

        public float moveSpeed;
        public bool useRotation;
        public float rotationSpeed;
        public float moveRange_Max;
        public float moveRange_Stop;


    ///////////////////////////
    ///
    ///     DESTROY OPTIONS
    ///
    ///////////////////////////


        public Destroy_Call destroyCall;
        public Destroy_Type destroyType;
        public float destroyWait;


    ///////////////////////////
    ///
    ///     EVENTS
    ///
    ///////////////////////////


        public Events events;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public Transform target;
        public Move_State moveState;

        public bool canMove;
        public float moveTo_Distance;
        public bool moving;
        public bool stopped;

        private Vector3 velocity = Vector3.zero;
        private float singleStep;
        private Vector3 targetPosition;
        private Vector3 targetDirection;
        private Vector3 newDirection;

        public bool destroyed;
        public bool locked;

        public int tabs;
        public int moveTabs;

        public bool startOpts;
        public bool targetOpts;
        public bool moveOpts;
        public bool destOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            if(autoStart){

                if(targetType == Target_Type.Player){

                    Target_Set(GameObject.FindGameObjectWithTag(playerTag).transform);

                }//targetType = player

                if(useStartDelay){

                    StartCoroutine("AutoStart_Delayed");

                //useStartDelay
                } else {

                    Possession_Init();

                }//useStartDelay

            }//autoStart

        }//StartInit

        private IEnumerator AutoStart_Delayed(){

            yield return new WaitForSeconds(startDelay);

            Possession_Init();

        }//AutoStart_Delayed


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update() {

            if(!locked){

                if(canMove){

                    if(target != null){

                        if(moveState == Move_State.Normal){

                            Movement_Basic();

                        }//moveState = normal

                        if(moveState == Move_State.StartMove){

                            Movement_Start();

                        }//moveState = start move

                    }//target != null

                }//canMove

            }//!locked

        }//update


    //////////////////////////////////////
    ///
    ///     MOVEMENT ACTIONS
    ///
    ///////////////////////////////////////


        public void CanMove_State(bool state){

            canMove = state;

        }//CanMove_State

        private void Movement_Start(){

            if(startMove != null){

                moveTo_Distance = Vector3.Distance(moveTrans.transform.position, startMove.position);

                if(moveTo_Distance < moveRange_Max && moveTo_Distance > moveRange_Stop){

                    targetPosition = startMove.TransformPoint(new Vector3(0, 0, 0));

                    if(useRotation){

                        targetDirection = startMove.position - moveTrans.transform.position;

                        singleStep = rotationSpeed * Time.deltaTime;
                        newDirection = Vector3.RotateTowards(moveTrans.transform.forward, targetDirection, singleStep, 0.0f);

                        moveTrans.transform.rotation = Quaternion.LookRotation(newDirection);

                    }//useRotation

                    moveTrans.transform.position = Vector3.SmoothDamp(moveTrans.transform.position, targetPosition, ref velocity, moveSpeed);

                    moving = true;
                    stopped = false;

                }//moveTo_Distance < moveRange_Max & moveTo_Distance > moveRange_Stop

                if(moveTo_Distance <= moveRange_Stop) {

                    if(useRotation){

                        targetDirection = target.position - moveTrans.transform.position;

                        singleStep = rotationSpeed * Time.deltaTime;
                        newDirection = Vector3.RotateTowards(moveTrans.transform.forward, targetDirection, singleStep, 0.0f);

                        moveTrans.transform.rotation = Quaternion.LookRotation(newDirection);

                    }//useRotation

                    moving = false;
                    stopped = true;
                    CanMove_State(false);

                    StartCoroutine("StartMove_Finish");

                }//moveTo_Distance <= moveRange_Stop

            }//startMove != null

        }//Movement_Start

        private void Movement_Basic(){

            if(target != null){

                moveTo_Distance = Vector3.Distance(moveTrans.transform.position, target.position);

                if(moveTo_Distance < moveRange_Max && moveTo_Distance > moveRange_Stop){

                    targetPosition = target.TransformPoint(new Vector3(0, 0, 0));

                    if(useRotation){

                        targetDirection = target.position - moveTrans.transform.position;

                        singleStep = rotationSpeed * Time.deltaTime;
                        newDirection = Vector3.RotateTowards(moveTrans.transform.forward, targetDirection, singleStep, 0.0f);

                        moveTrans.transform.rotation = Quaternion.LookRotation(newDirection);

                    }//useRotation

                    moveTrans.transform.position = Vector3.SmoothDamp(moveTrans.transform.position, targetPosition, ref velocity, moveSpeed);

                    moving = true;
                    stopped = false;

                }//moveTo_Distance < moveRange_Max & moveTo_Distance > moveRange_Stop

                if(moveType == Move_Type.Single){

                    if(moveTo_Distance <= moveRange_Stop) {

                        if(useRotation){

                            targetDirection = target.position - moveTrans.transform.position;

                            singleStep = rotationSpeed * Time.deltaTime;
                            newDirection = Vector3.RotateTowards(moveTrans.transform.forward, targetDirection, singleStep, 0.0f);

                            moveTrans.transform.rotation = Quaternion.LookRotation(newDirection);

                        }//useRotation

                        moving = false;
                        stopped = true;

                        Possession_Stop();

                    }//moveTo_Distance <= moveRange_Stop

                }//moveType = single

            }//target != null

        }//BasicMovement

        private IEnumerator StartMove_Finish(){

            yield return new WaitForSeconds(startMoveFinishDelay);

            moveState = Move_State.Normal;

            CanMove_State(true);

        }//StartMove_Finish


    //////////////////////////////////////
    ///
    ///     POSSESSION ACTIONS
    ///
    ///////////////////////////////////////


        public void Possession_Init(){

            if(useStartMove){

                moveState = Move_State.StartMove;

            //useStartMove
            } else {

                moveState = Move_State.Normal;

            }//useStartMove

            CanMove_State(true);

            events.onMoveStart.Invoke();

            if(destroyCall == Destroy_Call.Auto){

                Possession_Destroy();

            }//destroyCall = auto

        }//Possession_Init

        public void Possession_Start(){

            CanMove_State(true);

            events.onMoveStart.Invoke();

        }//Possession_Start

        public void Possession_Stop(){

            CanMove_State(false);

            events.onMoveEnd.Invoke();

        }//Possession_Stop

        public void Possession_End(){

            Locked_State(true);
            CanMove_State(false);

            StopCoroutine("AutoStart_Delayed");
            StopCoroutine("StartMove_Finish");

        }//Possession_End

        public void Possession_State(bool state){

            CanMove_State(state);

        }//Possession_State


    //////////////////////////////////////
    ///
    ///     TARGET ACTIONS
    ///
    ///////////////////////////////////////


        public void Target_Set(Transform newTarget){

            target = newTarget;

        }//Target_Set

        public void Target_Clear(){

            target = null;

        }//Target_Clear


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void Destroyed_State(bool state){

            destroyed = state;

        }//Destroyed_State

        public void Locked_State(bool state){

            locked = state;

        }//Locked_State


    //////////////////////////////////////
    ///
    ///     DESTROY ACTIONS
    ///
    ///////////////////////////////////////


        public void Possession_Destroy(){

            Destroyed_State(true);
            Locked_State(true);

            StartCoroutine("DestroyBuff");

        }//Possession_Destroy

        private IEnumerator DestroyBuff(){

            yield return new WaitForSeconds(destroyWait);

            if(destroyType == Destroy_Type.Destroy){

                Destroy(this.gameObject);

            }//destroyType = destroy

            if(destroyType == Destroy_Type.Disable){

                this.gameObject.SetActive(false);

            }//destroyType == disable

        }//DestroyBuff


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"destroyed", destroyed },
                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            destroyed = (bool)token["destroyed"];
            locked = (bool)token["locked"];

            if(destroyed && locked){

                this.gameObject.SetActive(false);

            }//destroyed & locked

        }//OnLoad


    }//HFPS_Possessed


}//namespace