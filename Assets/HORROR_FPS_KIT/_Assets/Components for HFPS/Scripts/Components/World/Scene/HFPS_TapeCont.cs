using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HFPS.Systems;
using Newtonsoft.Json.Linq;
using ThunderWire.Utility;
using HFPS.UI;

public class HFPS_TapeCont : MonoBehaviour {
    
    [Serializable]
    public class Tape {
        
        public string name;
        
        [Space]
        
        [InventorySelector]
        public int itemID;
        
        public ItemDataPair[] customData;
        
    }//Tape
    
    public VCRPlayer vcrPlayer;
    public List<Tape> tapes;
    
    void Start() {}//start
    
    public void Tape_Play(int slot){
        
        if(vcrPlayer != null){

            if(!vcrPlayer.isOn){
                
                vcrPlayer.PowerOnOff();
            
            }//!isOn
            
            if(tapes.Count > 0){
                
                ItemData itemData = new ItemData();
                
                foreach(var data in tapes[slot - 1].customData) {
                        
                    itemData.data.Add(data.Key, data.Value);
                        
                }//foreach data

                vcrPlayer.OnItemSelect(tapes[slot - 1].itemID, itemData);

            }//tapes.Count > 0
        
        }//vcrPlayer != null
            
    }//Tape_Play

    
}//HFPS_TapeCont
