using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Shared {

    [AddComponentMenu("Dizzy Media/Shared/Systems/Action Bar/Action Bars Controller")]
    public class DM_ActionBarsCont : MonoBehaviour {

        
    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////
        
        
        public List<DM_ActionBar> actionBars;
        
        
    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////
    
        
        void Start(){}
        
        
    //////////////////////////
    //
    //      ACTION BAR ACTIONS
    //
    //////////////////////////
        
        
        public void Pause_Check(){
            
            if(actionBars.Count > 0){
                
                for(int i = 0; i < actionBars.Count; ++i ) {
                 
                    if(actionBars[i].ActionsActive_Get()){
                        
                        actionBars[i].PauseCheck();
                        
                    }//actions active
                    
                }//for i actionBars
                
            }//actionBars.Count > 0
            
        }//Pause_Check
        

    }//DM_ActionBarsCont
    
    
}//namespace
