using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

[AddComponentMenu("Dizzy Media/Components for HFPS/World/Scene/Activate Handler")]
public class HFPS_ActiveHand : MonoBehaviour, ISaveable {
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////

///////////////////////////
///
///     USER OPTIONS
///
///////////////////////////

    
    public DynamicObject dynamObj;
    
    public int activeCount;
    public bool autoCountCheck;
    
    
///////////////////////////
///
///     EVENTS OPTIONS
///
///////////////////////////


    [Space] 
    
    public UnityEvent onActiveEvent;
    public UnityEvent onActiveLoadEvent;
    
    
///////////////////////////
///
///     AUTO
///
///////////////////////////

    
    public int curCount;
    public bool locked;


//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////


    void Start() {}//start
    
    
//////////////////////////////////////
///
///     COUNT ACTIONS
///
///////////////////////////////////////


    public void Count_Add(int amount){
        
        curCount += amount;
        
        if(autoCountCheck){
        
            Count_Check();
        
        }//autoCountCheck
        
    }//Count_Add

    public void Count_Check(){

        if(!locked){

            if(curCount == activeCount){

                if(dynamObj != null){

                    dynamObj.ParseUseType(0);
                    dynamObj.isLocked = false;

                }//dynamObj != null

                onActiveEvent.Invoke();

                locked = true;

            }//curCount = activeCount
        
        }//!locked
        
    }//Count_Check
    
    
//////////////////////////////////////
///
///     SAVE/LOAD ACTIONS
///
///////////////////////////////////////
    
    
    public Dictionary<string, object> OnSave() {
        
        return new Dictionary<string, object> {
            
            {"curCount", curCount },
            {"locked", locked }
        
        };//Dictionary
        
    }//OnSave

    public void OnLoad(JToken token) {

        curCount = (int)token["curCount"];
        locked = (bool)token["locked"];
        
        if(curCount == activeCount){
        
            if(dynamObj != null){
                
                dynamObj.ParseUseType(0);
                dynamObj.isLocked = false;
            
            }//dynamObj != null
        
            onActiveLoadEvent.Invoke();
        
        }//curCount = activeCount
        
    }//OnLoad
    

}//HFPS_ActiveHand
