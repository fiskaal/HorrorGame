using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
    
    public Transform thisTrans;
    public Transform lookTrans;
    
    [Header("Auto")]
    public bool locked;
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    
    
    void Start() {
        
        locked = false;
    
    }//start

    
//////////////////////////////////////
///
///     UPDATE ACTIONS
///
///////////////////////////////////////
    

    void Update() {
        
        if(!locked){

            thisTrans.LookAt(lookTrans);
            
        }//!locked
        
    }//update
    
}//LookAt
