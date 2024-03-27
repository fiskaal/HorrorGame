using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Edit Template", menuName = "Dizzy Media/Extensions/Script Editor/Scriptables/Template/New Edit Template", order = 1)]
    public class DM_ScriptEdit_Template : ScriptableObject {

        [Serializable]
        public class Content_Edit {

            [Space]

            public string name;

            [Space]

            [TextArea(1, 30)]
            public string original;

            [Space]

            [TextArea(1, 30)]
            public string edit;

        }//Content_Edit

        public enum Removable {

            Yes = 0,
            No = 1,

        }//Removable

        [Space]

        public Removable removable;

        [Space]

        public List<Content_Edit> content; 


    }//DM_ScriptEdit_Template


}//namespace
