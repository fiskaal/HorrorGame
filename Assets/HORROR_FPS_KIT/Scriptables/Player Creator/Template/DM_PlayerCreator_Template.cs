#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine.AI;
using System;

using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Player Template", menuName = "Dizzy Media/Extensions/Player Creator/Scriptables/Template/New Player Template", order = 1)]
    public class DM_PlayerCreator_Template : ScriptableObject {


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Template_Type {

            PlayerNew = 0,
            PlayerUpdate = 1,

        }//Template_Type


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        [Space]

        public Template_Type templateType;

        [Space]

        public HFPS_PlayerCreator.AddOrUpdate_Options addOrUpdateOptions;
        public HFPS_PlayerCreator.AudioFader_Options audioFaderOptions;
        public List<HFPS_PlayerCreator.DualWield_Item> dualWieldItems = new List<HFPS_PlayerCreator.DualWield_Item>();
        public HFPS_PlayerCreator.FOV_Options fovOptions;
        public HFPS_PlayerCreator.MaterialCont_Options matContOptions;
        public HFPS_PlayerCreator.PlayerMan_Options playerManOptions;
        public HFPS_PlayerCreator.Prone_Options proneOptions;
        public HFPS_PlayerCreator.SubActions_Options subActionsOptions;

        #if COMPONENTS_PRESENT

            public PlayerFunctions.Zoom_Type zoomType;

        #endif


    }//DM_PlayerCreator_Template


}//namespace

#endif
