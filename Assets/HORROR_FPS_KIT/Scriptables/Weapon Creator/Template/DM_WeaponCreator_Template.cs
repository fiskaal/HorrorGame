#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

using HFPS.Systems;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Weapon Template", menuName = "Dizzy Media/Extensions/Weapon Creator/Scriptables/Template/New Weapon Template", order = 1)]
    public class DM_WeaponCreator_Template : ScriptableObject {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    //////////////////////////////////////


        [Space]

        public string weaponName;
        public HFPS_WeaponCreator.Weapon_Type weaponType;

        [Space]

        public GameObject armsPrefab;
        public HFPS_WeaponCreator.Candle_Settings candleSettings = new HFPS_WeaponCreator.Candle_Settings();
        public HFPS_WeaponCreator.Flashlight_Settings flashlightSettings = new HFPS_WeaponCreator.Flashlight_Settings();
        public HFPS_WeaponCreator.Lantern_Settings lanternSettings = new HFPS_WeaponCreator.Lantern_Settings();
        public HFPS_WeaponCreator.Lighter_Settings lighterSettings = new HFPS_WeaponCreator.Lighter_Settings();
        public HFPS_WeaponCreator.Melee_Settings meleeSettings = new HFPS_WeaponCreator.Melee_Settings();
        public HFPS_WeaponCreator.Gun_Settings gunSettings = new HFPS_WeaponCreator.Gun_Settings();

        [Space]

        //public List<HFPS.Systems.AnimationEvent.AnimEvents> animationEvents;
        public List<AnimationSoundEvents.SoundEvents> animationSoundEvents;


    }//DM_WeaponCreator_Template
    

}//namespace

#endif
