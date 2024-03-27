using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    public class DebugLog_Push : MonoBehaviour {


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [System.Serializable]
        public class Log {

            [Space]

            public string name;

            [Space]

            public bool singleFire;

            [TextArea]
            public string text;

            [Header("Auto")]

            public bool fired;

        }//Log


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////


        public List<Log> logs;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start() {}//Start


    //////////////////////////
    //
    //      LOG ACTIONS
    //
    //////////////////////////


        public void Log_Show(int slot){

            if(!logs[slot - 1].fired){

                Debug.Log(logs[slot - 1].text);

                if(logs[slot - 1].singleFire){

                    logs[slot - 1].fired = true;

                }//singleFire

            }//!fired

        }//Log_Show


    }//DebugLog_Push
    
    
}//namespace
