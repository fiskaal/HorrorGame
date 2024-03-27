using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    public class TransForward : MonoBehaviour {

        public bool useGizmos;

        public float rayDist;
        public Color gizmoColor;

        void Start(){}

        void OnDrawGizmos() {

            if(useGizmos){

                Gizmos.color = gizmoColor;
                Gizmos.DrawRay(transform.position, transform.forward);

            }//useGizmos

        }//OnDrawGizmos


    }//TransForward
    
    
}//namespace
