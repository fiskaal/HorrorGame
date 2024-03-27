using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New DM Menu Loc", menuName = "Dizzy Media/Scriptables/Localization/New DM Menu Loc", order = 1)]
public class DM_MenuLoc : ScriptableObject {

    [System.Serializable]
    public class Localization {
    
        public List<Languages> languages = new List<Languages>();
        
    }//Localization
    
    [System.Serializable]
    public class Languages {
    
        [Space]
        
        public string name;
        
        [Space]
        
        public List<Window> windows;
    
    }//Languages
    
    [System.Serializable]
    public class Window {
    
        [Space]
        
        public string name;
        
        [Space]
        
        public List<Sections> sections;
    
    }//Window
    
    [System.Serializable]
    public class Sections {
    
        [Space]
    
        public string name;
        public string local;
        
        [Space]
        
        public List<Texts> texts;
        public List<SingleLocal> buttons;
        public List<Toggles> toggles;
        public List<Prompts> prompts;
        public List<SingleLocal> singleValues;
    
    }//Sections
    
    [System.Serializable]
    public class Texts {
    
        [Space]
        
        public string name;
    
        [Space]
    
        [TextArea(1, 20)]
        public string text;
    
    }//Texts
    
    [System.Serializable]
    public class SingleLocal {
    
        [Space]
        
        public string name;
        public string local;
    
    }//SingleLocal
    
    [System.Serializable]
    public class Toggles {
    
        [Space]
        
        public string name;
        
        [Space]
        
        public string header;
        
        [Space]
        
        public string valTrue;
        public string valFalse;
    
    }//Toggles
    
    [System.Serializable]
    public class Prompts {
    
        [Space]
        
        public string name;
    
        [Space]
        
        public string header;
    
        [Space]
    
        [TextArea(1, 20)]
        public string message;
        
        [Space]
        
        public List<SingleLocal> buttons;
    
    }//Prompts
    
    public Localization localization;

}//DM_MenuLoc
