using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/Gizmos/Transform Indicator")]
    public class TransInd : MonoBehaviour {

        public enum GizmoType {

            RayLine = 0,
            Box = 1,

        }//GizmoType

        public enum ForwardDirection {

            x = 0,
            y = 1,
            z = 2,

        }//ForwardDirection

        public bool useGizmos;

        public GizmoType gizmoType;
        public ForwardDirection forwardDir;

        public float rayDist;

        public Color gizmoColor;
        public Vector3 gizmoBox_Size;


        void Start(){}

         void OnDrawGizmos() {

             if(useGizmos){

                 if((int)gizmoType == 0){

                     Gizmos.color = gizmoColor;
                     Vector3 direction = new Vector3(0, 0, 0);

                     if((int)forwardDir == 0){

                        direction = transform.TransformDirection(transform.rotation.x + 1, 0, 0) * rayDist;

                     }//forwardDir = X

                     if((int)forwardDir == 1){

                        direction = transform.TransformDirection(0, transform.rotation.y + 1, 0) * rayDist;

                     }//forwardDir = Y

                     if((int)forwardDir == 2){

                        direction = transform.TransformDirection(0, 0, transform.rotation.z + 1) * rayDist;

                     }//forwardDir = Z

                     Gizmos.DrawRay(transform.position, direction);

                 }//showRay

                 if((int)gizmoType == 1){

                     Gizmos.matrix = this.transform.localToWorldMatrix;
                     Gizmos.color = gizmoColor;
                     Gizmos.DrawCube(Vector3.zero, gizmoBox_Size);

                 }//showBox

             }//useGizmos

         }//OnDrawGizmos


    }//TransInd


}//namespace
