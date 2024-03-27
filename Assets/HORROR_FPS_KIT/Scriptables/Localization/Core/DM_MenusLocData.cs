using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New DM Menus LocData", menuName = "Dizzy Media/Scriptables/Localization/New DM Menus LocData", order = 1)]
public class DM_MenusLocData : ScriptableObject {

    [System.Serializable]
    public class Dictionary {
    
        [Space]
        
        public string asset;
        public DM_MenuLoc menuLoc;
    
    }//Dictionary
    
    public DM_InternEnums.Language currentLanguage;
    public List<Dictionary> dictionary = new List<Dictionary>();

}//DM_MenusLocData
