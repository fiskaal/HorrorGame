using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Version Template", menuName = "Dizzy Media/Extensions/Version Detect/Scriptables/Template/New Edit Template", order = 1)]
    public class DM_VersDetect_Template : ScriptableObject {

        [Serializable]
        public class Content {

            [Space]

            public string name;

            [Space]

            [TextArea(1, 30)]
            public string text;

        }//Content

        public List<Content> content;
        

    }//DM_VersDetect_Template
    

}//namespace
