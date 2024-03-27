using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Edit Library", menuName = "Dizzy Media/Extensions/Script Editor/Scriptables/Library/New Edit Library", order = 1)]
    public class DM_ScriptEdit_Library : ScriptableObject {

        [Serializable]
        public class Content_Edit {

            [Space]

            public string name;
            public string version;

            [Space]

            public List<Content_Library> library;

        }//Content_Edit

        [Serializable]
        public class Content_Library {

            [Space]

            public string name;
            public DM_ScriptEdit_Template template;

        }//Content_Library

        public Content_Edit content;


    }//DM_ScriptEdit_Library


}//namespace