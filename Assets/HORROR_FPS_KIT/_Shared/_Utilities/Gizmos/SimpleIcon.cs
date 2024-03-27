using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/Gizmos/Simple Icon")]
    public class SimpleIcon : MonoBehaviour {

        public bool showGizmos;

        public Texture icon;

        public Vector3 offset;

        void Start(){}

        void OnDrawGizmos() {

            if(showGizmos && icon != null){

                Gizmos.DrawIcon(transform.position + offset, icon.name, true);

            }//showGizmos

        }//OnDrawGizmos


    }//SimpleIcon


}//namespace
