using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Version Library", menuName = "Dizzy Media/Extensions/Version Detect/Scriptables/Library/New Version Library", order = 1)]
    public class DM_VersDetect_Library : ScriptableObject {

        [Serializable]
        public class Content {

            [Space]

            public string asset;
            public string version;

            [Space]

            public List<Content_Versions> versions;

        }//Content

        [Serializable]
        public class Content_Versions {

            [Space]

            public string name;
            //public Version_Type version;

            //[Space]

            public List<Content_Library> library;

        }//Content_Versions

        [Serializable]
        public class Content_Library {

            [Space]

            public string name;
            public DM_VersDetect_Template template;

        }//Content_Library

        /*

        public enum Version_Type {

            a = 0,
            b = 1,
            c = 2,

        }//Version_Type

        */

        public Content content;
        

    }//DM_VersDetect_Library
    

}//namespace
