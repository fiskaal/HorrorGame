using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    public class SimpleInstantiate : MonoBehaviour {


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////


        public GameObject prefab;
        public Transform createPosition;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start() {}//start


    //////////////////////////
    //
    //      CREATE ACTIONS
    //
    //////////////////////////


        public void Create(){

            if(prefab != null && createPosition != null){

                GameObject newGameObject = Instantiate(prefab, createPosition.position, createPosition.rotation) as GameObject;
                newGameObject.transform.parent = createPosition;

            }//prefab != null & createPosition != null

        }//Create

        public void Create_Position(Transform newPos){

            if(prefab != null){

                GameObject newGameObject = Instantiate(prefab, newPos.position, newPos.rotation) as GameObject;
                newGameObject.transform.parent = newPos;

            }//prefab != null

        }//Create_Position

        public void Create_Prefab(GameObject newPrefab){

            if(createPosition != null){

                GameObject newGameObject = Instantiate(newPrefab, createPosition.position, createPosition.rotation) as GameObject;
                newGameObject.transform.parent = createPosition;

            }//createPosition != null

        }//Create_Prefab


    }//SimpleInstantiate

    
}//namespace