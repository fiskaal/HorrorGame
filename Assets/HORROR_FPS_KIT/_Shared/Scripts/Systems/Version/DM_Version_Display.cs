using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Version;

namespace DizzyMedia.Shared {

    public class DM_Version_Display : MonoBehaviour {


        public Text versionText;
        public DM_Version template;

        void Start() {

            StartInit();

        }//start
        
        public void StartInit(){
            
            versionText.text = "v" + template.version;
            
        }//StartInit


    }//DM_Version_Display
    
    
}//namespace
