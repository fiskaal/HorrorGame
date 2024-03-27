using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/World/Forward Detect")]
    public class DM_ForwardDetect : MonoBehaviour {


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////


        public enum Forward_Direction {

            x = 0,
            y = 1,
            z = 2,

        }//Forward_Direction

        public enum Forward_Type {

            Regular = 0,
            Reverse = 1,

        }//Forward_Type


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////


        public bool useGizmos = true;
        public float rayDist;
        public Color gizmoColor;

        public bool detect = true;
        public Forward_Direction forwardDir;
        public Forward_Type forwardType;

        [Range(0, 180)]
        public float forwardAngle = 15;

        public int tabs;
        public int gizmosInt = 1;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start() {}//start


    //////////////////////////
    //
    //      DETECT ACTIONS
    //
    //////////////////////////


        public bool CanDetect(){

            return detect;

        }//CanDetect

        public bool IsInForward(Transform target) {

            Vector3 direction = new Vector3(0, 0, 0);
            var angle = Vector3.Angle(new Vector3(0, 0, 0), new Vector3(0, 0, 0));

            if(detect){

                if(forwardDir == Forward_Direction.x){

                    if(forwardType == Forward_Type.Regular){

                        direction = transform.TransformDirection(transform.rotation.x + 1, 0, 0);

                    }//forwardType = regular

                    if(forwardType == Forward_Type.Reverse){

                        direction = transform.TransformDirection(transform.rotation.x - 1, 0, 0);

                    }//forwardType = reverse

                }//forwardDir = X

                if(forwardDir == Forward_Direction.y){

                    if(forwardType == Forward_Type.Regular){

                        direction = transform.TransformDirection(0, transform.rotation.y + 1, 0);

                    }//forwardType = regular

                    if(forwardType == Forward_Type.Reverse){

                        direction = transform.TransformDirection(0, transform.rotation.y - 1, 0);

                    }//forwardType = reverse

                }//forwardDir = Y

                if(forwardDir == Forward_Direction.z){

                    if(forwardType == Forward_Type.Regular){

                        direction = transform.TransformDirection(0, 0, transform.rotation.z + 1) * rayDist;

                    }//forwardType = regular

                    if(forwardType == Forward_Type.Reverse){

                        direction = transform.TransformDirection(0, 0, transform.rotation.z - 1) * rayDist;

                    }//forwardType = reverse

                }//forwardDir = Z

                angle = Vector3.Angle(direction, target.forward);

            }//detect

            return angle <= forwardAngle;

        }//IsInForward


    //////////////////////////
    //
    //      GIZMOS ACTIONS
    //
    //////////////////////////


        void OnDrawGizmos() {

            if(useGizmos){

                Gizmos.color = gizmoColor;
                Vector3 direction = new Vector3(0, 0, 0);

                if(forwardDir == Forward_Direction.x){

                    if(forwardType == Forward_Type.Regular){

                        direction = transform.TransformDirection(transform.rotation.x + 1, 0, 0) * rayDist;

                    }//forwardType = regular

                    if(forwardType == Forward_Type.Reverse){

                        direction = transform.TransformDirection(transform.rotation.x - 1, 0, 0) * rayDist;

                    }//forwardType = reverse

                }//forwardDir = X

                if(forwardDir == Forward_Direction.y){

                    if(forwardType == Forward_Type.Regular){

                        direction = transform.TransformDirection(0, transform.rotation.y + 1, 0) * rayDist;

                    }//forwardType = regular

                    if(forwardType == Forward_Type.Reverse){

                        direction = transform.TransformDirection(0, transform.rotation.y - 1, 0) * rayDist;

                    }//forwardType = reverse

                }//forwardDir = Y

                if(forwardDir == Forward_Direction.z){

                    if(forwardType == Forward_Type.Regular){

                        direction = transform.TransformDirection(0, 0, transform.rotation.z + 1) * rayDist;

                    }//forwardType = regular

                    if(forwardType == Forward_Type.Reverse){

                        direction = transform.TransformDirection(0, 0, transform.rotation.z - 1) * rayDist;

                    }//forwardType = reverse

                }//forwardDir = Z

                Gizmos.DrawRay(transform.position, direction);

            }//useGizmos

        }//OnDrawGizmos


    }//DM_ForwardDetect


}//namespace