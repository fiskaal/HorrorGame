using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New DM Languages", menuName = "Dizzy Media/Scriptables/Localization/New DM Languages", order = 1)]
public class DM_Languages : ScriptableObject {

    public enum Language_Type {
    
        Core = 0,
        Diaries = 1,
        
    }//Language_Type

    public Language_Type languageType;
    
    public List<string> languages = new List<string>();
    
}//DM_Languages
