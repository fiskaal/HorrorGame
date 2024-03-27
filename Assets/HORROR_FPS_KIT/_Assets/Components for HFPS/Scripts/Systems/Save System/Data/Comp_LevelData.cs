using System;
using UnityEngine;
using System.Collections.Generic;

using HFPS.Systems;
using ThunderWire.Utility;
using HFPS.UI;

[Serializable]
public class Comp_LevelData {

    [Serializable]
    public struct Level {
    
        [Space]
    
        public string name;
        public string save;
        public bool firstVisit;
        
        [Space]
        
        public List<ObjectiveModel> activeObjectives;
    
    }//Level
    
    public Level[] levels = new Level[0];

}//Comp_LevelData
