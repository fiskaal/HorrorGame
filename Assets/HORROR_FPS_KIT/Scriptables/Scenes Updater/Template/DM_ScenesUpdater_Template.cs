#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Scenes Updater Template", menuName = "Dizzy Media/Extensions/Scenes Updater/Scriptables/Template/New Scenes Updater Template", order = 1)]
    public class DM_ScenesUpdater_Template : ScriptableObject {

        public List<SceneAsset> scenes = new List<SceneAsset>();


    }//DM_ScenesUpdater_Template


}//namespace

#endif
