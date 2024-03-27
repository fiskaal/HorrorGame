#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using System.IO;

using DizzyMedia.Version;

using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.Extension {

    public class HFPS_WeaponCreator : EditorWindow {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////

    /////////////////////////////
    ///
    ///     SETTINGS
    ///
    /////////////////////////////


        [Serializable]
        public class Candle_Settings {

            public AudioClip BlowOut;

            [Space]

            public AnimationClip IdleAnimation;
            public AnimationClip BlowOutAnimation;

            [Space]

            public AnimationClip DrawAnimation;
            [Range(0, 5)] public float DrawSpeed = 1f;

            [Space]

            public AnimationClip HideAnimation;
            [Range(0, 5)] public float HideSpeed = 1f;

            [Space]

            public bool candleReduction;
            public float reductionRate;
            public float maxScale;
            public float minScale;

            [Space]

            [InventorySelector]
            public int itemID;

            [Space]

            public bool blowOutKeepCandle;
            public float scaleKeepCandle;

            [Space]

            public GameObject Candle;
            public GameObject CandleLight;

            public GameObject CandleFlame;
            public Transform FlamePosition;

            [Space]

            public Model_Type candleType;
            public Transform candleParent;
            public GameObject candlePrefab;

            [Space]

            public Model_Type flameType;
            public Transform candleFlameParent;
            public GameObject flamePrefab;

            [Space]

            public Model_Type lightType;
            public Transform candleLightParent;
            public GameObject lightPrefab;

        }//Candle_Settings

        [Serializable]
        public class Flashlight_Settings {

            public AudioClip clickSound;

            [Space]

            public AnimationClip IdleAnim;

            [Space]

            public AnimationClip DrawAnim;
            [Range(0, 5)] public float DrawSpeed = 1f;

            public AnimationClip HideAnim;
            [Range(0, 5)] public float HideSpeed = 1f;

            public AnimationClip ReloadAnim;
            [Range(0, 5)] public float ReloadSpeed = 1f;

            [Space]

            public bool enableExtra;

            public AnimationClip ScareAnim;
            [Range(0, 5)] public float ScareAnimSpeed = 1f;

            public AnimationClip NoPowerAnim;
            [Range(0, 5)] public float NoPowerAnimSpeed = 1f;

            [Space]

            public bool infiniteBattery;
            public float batteryLifeInSec;
            public float canReloadPercent;

            [Space]

            [InventorySelector]
            public int itemID;

            [Space]

            public Light LightObject;
            public float flashlightIntensity = 1f;

            [Space]

            public Sprite FlashlightIcon;

            [Space]

            public AudioSource audioSource;

            [Space]

            public Model_Type flashLightType;
            public Transform flashlightParent;
            public GameObject flashlightPrefab;

            [Space]

            public Model_Type lightType;
            public Transform lightParent;
            public GameObject lightPrefab;

        }//Flashlight_Settings

        [Serializable]
        public class Lantern_Settings {

            public AudioClip ShowSound;
            [Range(0, 1)] public float ShowVolume = 1;

            public AudioClip HideSound;
            [Range(0, 1)] public float HideVolume = 1;

            public AudioClip ReloadOilSound;
            [Range(0, 1)] public float ReloadVolume = 1;

            [Space]

            public AnimationClip DrawAnim;
            [Range(0, 5)] public float DrawSpeed = 1f;

            public AnimationClip HideAnim;
            [Range(0, 5)] public float HideSpeed = 1f;

            public AnimationClip ReloadAnim;
            [Range(0, 5)] public float ReloadSpeed = 1f;

            public AnimationClip IdleAnim;

            [Space]

            [InventorySelector]
            public int itemID;

            [Space]

            public bool useHingeJoint = false;
            public HingeJoint hingeLantern;
            [Tooltip("Difference after second lantern draw")]
            public float secondDrawDiff;

            [Space]

            public float oilLifeInSec = 300f;
            public float oilPercentage = 100;
            public float lightReductionRate = 5f;
            public float canReloadPercent;
            public float hideIntensitySpeed;
            public float oilReloadSpeed;
            public float timeWaitToReload;

            [Space]

            public Light LanternLight;
            public MeshRenderer flameMesh;
            public string ColorString = "_Color";

            [Space]

            public Sprite LanternIcon;

            [Space]

            public Model_Type lanternType;
            public Transform lanternParent;
            public GameObject lanternPrefab;

            [Space]

            public Model_Type flameType;
            public Transform flameParent;
            public GameObject flamePrefab;

            [Space]

            public Model_Type lightType;
            public Transform lightParent;
            public GameObject lightPrefab;

            [Space]

            public Model_Type hingeType;
            public GameObject hingeParent;

            [Space]

            public Model_Type hingeConType;
            public Transform hingeConParent;
            public GameObject hingeConPrefab;
            public Rigidbody hingeConnect;

        }//Lantern_Settings

        [Serializable]
        public class Lighter_Settings {

            [Space]

            #if COMPONENTS_PRESENT

                public List<LighterItem.Sound_Library> soundLibrary;

                [Space]

            #endif

            public AnimationClip IdleAnimation;

            public AnimationClip DrawAnimation;
            [Range(0, 5)] public float DrawSpeed = 1f;

            public AnimationClip HideAnimation;
            [Range(0, 5)] public float HideSpeed = 1f;

            [Space]

            public Animation lighterAnimation;
            public AnimationClip lighterOpenAnimation;
            public AnimationClip lighterCloseAnimation;

            [Space]

            [InventorySelector]
            public int itemID;

            [Space]

            public GameObject flame;
            public GameObject light;
            public Transform FlamePosition;

            [Space]

            public Model_Type lighterType;
            public Transform lighterParent;
            public GameObject lighterPrefab;

            [Space]

            public Model_Type flameType;
            public Transform flameParent;
            public GameObject flamePrefab;

            [Space]

            public Model_Type lightType;
            public Transform lightParent;
            public GameObject lightPrefab;

        }//Lighter_Settings

        [Serializable]
        public class Melee_Settings {

            [Space]

            public AudioClip DrawSound;
            [Range(0, 1)] public float DrawVolume = 1f;

            public AudioClip HideSound;
            [Range(0, 1)] public float HideVolume = 1f;

            public AudioClip SwaySound;
            [Range(0, 1)] public float SwayVolume = 1f;

            [Space]

            public AnimationClip DrawAnim;
            [Range(0, 5)] public float DrawSpeed = 1f;

            public AnimationClip HideAnim;
            [Range(0, 5)] public float HideSpeed = 1f;

            public AnimationClip AttackAnim;
            [Range(0, 5)] public float AttackSpeed = 1f;

            [Space]

            [InventorySelector]
            public int itemID;

            [Space]

            public SurfaceID surfaceID = SurfaceID.Texture;
            public SurfaceDetailsScriptable surfaceDetails;
            public int defaultSurfaceID;

            [Space]

            public LayerMask HitLayer;
            public float HitDistance;
            public float HitForce;
            public float HitWaitDelay;
            public Vector2Int AttackDamage;

            [Space]

            public Vector3 SwayKickback;
            public float SwaySpeed = 0.1f;

            [Space]

            public AudioSource audioSource;

            [Space]

            public Model_Type weaponType;
            public Transform weaponParent;
            public GameObject weaponPrefab;

        }//Melee_Settings

        [Serializable]
        public class Gun_Settings {

            public WeaponController.WeaponType weaponType = WeaponController.WeaponType.Semi;
            public WeaponController.BulletType bulletType = WeaponController.BulletType.None;
            public LayerMask raycastMask;
            public LayerMask soundReactionMask;

            [Space]

            public WeaponController.AudioSettings audioSettings = new WeaponController.AudioSettings();
            public WeaponController.AimingSettings aimingSettings = new WeaponController.AimingSettings();
            public WeaponController.AnimationSettings animationSettings = new WeaponController.AnimationSettings();
            public WeaponController.InventorySettings inventorySettings = new WeaponController.InventorySettings();

            [Space]

            public WeaponController.BulletSettings bulletSettings = new WeaponController.BulletSettings();
            public WeaponController.BulletModelSettings bulletModelSettings = new WeaponController.BulletModelSettings();
            public SurfaceDetailsScriptable surfaceDetails;

            [Space]

            public WeaponController.KickbackSettings kickbackSettings = new WeaponController.KickbackSettings();
            public StabilizeKickback kickback;

            [Space]

            public WeaponController.MuzzleFlashSettings muzzleFlashSettings = new WeaponController.MuzzleFlashSettings();
            public WeaponController.WeaponSettings weaponSettings = new WeaponController.WeaponSettings();

            #if (HFPS_163a || HFPS_163b)

                public WeaponController.ShotgunSettings shotgunSettings = new WeaponController.ShotgunSettings();

            #endif

            #if HFPS_163c

                public ShellEject_Settings shellEjectSettings = new ShellEject_Settings();

            #endif

            public WeaponController.NPCReactionSettings npcReactionSettings = new WeaponController.NPCReactionSettings();

            [Space]

            public AudioSource audioSource;

            [Space]

            public Model_Type weapModelType;
            public Transform weaponParent;
            public GameObject weaponPrefab;

            [Space]

            public Model_Type muzzleFlashType;
            public Transform muzzleFlashParent;
            public GameObject muzzleFlashPrefab;

            [Space]

            public Model_Type muzzleLightType;
            public Transform muzzleLightParent;
            public GameObject muzzleLightPrefab;

        }//Gun_Settings

        [Serializable]
        public class ShellEject_Settings {

            public bool ejectShells;

            public Transform ejectPosition;
            public GameObject shellPrefab;
            public Vector3 shellRotation;
            public float ejectSpeed = 10f;

            [Tooltip("Eject shells automatically on fire or with an animation event?")]
            public bool ejectAutomatiacally = false;

        }//ShellEject_Settings


    /////////////////////////////
    ///
    ///     AUTO
    ///
    /////////////////////////////


        [Serializable]
        public class Candle_Auto {

            public bool audOpts;
            public bool animOpts;
            public bool candleOpts;
            public bool invOpts;

            public int candleTabs;
            public int candleModelTabs;

        }//Candle_Auto

        [Serializable]
        public class Flashlight_Auto {

            public bool audOpts;
            public bool animOpts;
            public bool flashOpts;
            public bool invOpts;
            public bool uiOpts;

            public int animTabs;
            public int flashTabs;
            public int flashModelTabs;

        }//Flashlight_Auto

        [Serializable]
        public class Lantern_Auto {

            public bool audOpts;
            public bool animOpts;
            public bool invOpts;
            public bool lantOpts;
            public bool uiOpts;

            public int lanternTabs;
            public int lanternModelTabs;
            public int lanternHingeTabs;

            public bool showFlameCreate;

        }//Lantern_Auto

        [Serializable]
        public class Lighter_Auto {

            public bool audOpts;
            public bool animOpts;
            public bool invOpts;
            public bool lighterOpts;

            public int animTabs;
            public int lighterTabs;

        }//Lighter_Auto

        [Serializable]
        public class Melee_Auto {

            public bool audOpts;
            public bool animOpts;
            public bool invOpts;
            public bool meleeOpts;
            public bool weapOpts;

            public int meleeTabs;

        }//Melee_Auto

        [Serializable]
        public class Gun_Auto {

            public bool audOpts;
            public bool aimOpts;
            public bool animOpts;
            public bool invOpts;
            public bool shootOpts;
            public bool shellOpts;
            public bool weapOpts;

            public int shootTabs;
            public int shootMuzzleTabs;
            public int bulletTabs;
            public int weapTabs;

        }//Gun_Auto

        [Serializable]
        public class ScrollPositions {

            public Vector2 arms_scrollPos;

            public Vector2 candle_scrollPos;

            public Vector2 candleAudOpts_scrollPos;
            public Vector2 candleAnimOpts_scrollPos;
            public Vector2 candleCanOpts_scrollPos;
            public Vector2 candleInvOpts_scrollPos;

            public Vector2 flashlight_scrollPos;

            public Vector2 flashAudOpts_scrollPos;
            public Vector2 flashAnimOpts_scrollPos;
            public Vector2 flashFlashOpts_scrollPos;
            public Vector2 flashInvOpts_scrollPos;
            public Vector2 flashUIOpts_scrollPos;

            public Vector2 lantern_scrollPos;

            public Vector2 lantAudOpts_scrollPos;
            public Vector2 lantAnimOpts_scrollPos;
            public Vector2 lantInvOpts_scrollPos;
            public Vector2 lantLantOpts_scrollPos;
            public Vector2 lantUIOpts_scrollPos;

            public Vector2 lighter_scrollPos;

            public Vector2 lighterAudOpts_scrollPos;
            public Vector2 lighterAnimOpts_scrollPos;
            public Vector2 lighterInvOpts_scrollPos;
            public Vector2 lighterLighterOpts_scrollPos;

            public Vector2 melee_scrollPos;

            public Vector2 meleeAudOpts_scrollPos;
            public Vector2 meleeAnimOpts_scrollPos;
            public Vector2 meleeInvOpts_scrollPos;
            public Vector2 meleeMeleeOptsHit_scrollPos;
            public Vector2 meleeMeleeOptsKick_scrollPos;
            public Vector2 meleeMeleeOptsSurf_scrollPos;

            public Vector2 meleeWeapOpts_scrollPos;

            public Vector2 gun_scrollPos;

            public Vector2 gunAudOpts_scrollPos;
            public Vector2 gunAimOpts_scrollPos;
            public Vector2 gunAnimOpts_scrollPos;
            public Vector2 gunInvOpts_scrollPos;

            public Vector2 gunShootBulletOpts_scrollPos;
            public Vector2 gunShootBulletModelOpts_scrollPos;
            public Vector2 gunShootKickOpts_scrollPos;
            public Vector2 gunShootMuzzOpts_scrollPos;
            public Vector2 gunShootShootOpts_scrollPos;
            public Vector2 gunShellOpts_scrollPos;
            public Vector2 gunShootReactOpts_scrollPos;

            public Vector2 gunWeapOpts_scrollPos;

        }//ScrollPositions


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Weapon_Type {

            Candle = 0,
            Flashlight = 1,
            Lantern = 2,
            Lighter = 3,
            Melee = 4,
            Gun = 5,

        }//Weapon_Type

        public enum Model_Type {

            Create = 0,
            Local = 1,

        }//Model_Type


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static HFPS_WeaponCreator window;
        private static Vector2 windowsSize = new Vector2(400, 650);

        private static DM_Version dmVersion;
        private static string versionName = "WeaponCreator Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        public Weapon_Type weaponType;
        public DM_WeaponCreator_Template template;

        public string weaponName = "";
        public GameObject armsPrefab;
        public GameObject armsParent;
        public ItemSwitcher itemSwitcher;

        public Candle_Settings candleSettings;
        public Flashlight_Settings flashlightSettings;
        public Lantern_Settings lanternSettings;
        public Lighter_Settings lighterSettings;
        public Melee_Settings meleeSettings;
        public Gun_Settings gunSettings;

        public Candle_Auto candleAuto;
        public Flashlight_Auto flashlightAuto;
        public Lantern_Auto lanternAuto;
        public Lighter_Auto lighterAuto;
        public Melee_Auto meleeAuto;
        public Gun_Auto gunAuto;

        public ScrollPositions scrollPositions;

        private bool displayShow = false;
        private bool bottomShow = true;

        private GameObject tempWeapon;
        private GameObject tempArms;


    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/HFPS/HFPS Weapon Creator", false , 13)]
        public static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<HFPS_WeaponCreator>(false, "Weapon Creator" + " v" + verNumb, true);
                window.maxSize = window.minSize = windowsSize;

            }//dmVersion == null

            if(dmMenusLocData == null){

                languageLock = false;
                DM_LocDataFind();

            //dmMenusLocData = null
            } else {

                language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

            }//dmMenusLocData = null

        }//OpenWizard

        private void OnGUI() {

            WeaponCreator_Screen();

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        public void WeaponCreator_Screen(){

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t0 = (Texture)Resources.Load("EditorContent/WeaponCreator/WeaponCreator_Header");

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));

            EditorGUI.BeginChangeCheck();

            ScriptableObject target = this;
            SerializedObject soTar = new SerializedObject(target);

            SerializedProperty weaponTypeRef = soTar.FindProperty("weaponType");
            SerializedProperty templateRef = soTar.FindProperty("template");

            SerializedProperty weaponNameRef = soTar.FindProperty("weaponName");
            SerializedProperty armsPrefabRef = soTar.FindProperty("armsPrefab");
            SerializedProperty armsParentRef = soTar.FindProperty("armsParent");
            SerializedProperty itemSwitcherRef = soTar.FindProperty("itemSwitcher");

            //CANDLE

            SerializedProperty BlowOut = soTar.FindProperty("candleSettings.BlowOut");

            SerializedProperty candleIdleAnimation = soTar.FindProperty("candleSettings.IdleAnimation");
            SerializedProperty candleBlowOutAnimation = soTar.FindProperty("candleSettings.BlowOutAnimation");

            SerializedProperty candleDrawAnimation = soTar.FindProperty("candleSettings.DrawAnimation");
            SerializedProperty candleDrawSpeed = soTar.FindProperty("candleSettings.DrawSpeed");

            SerializedProperty candleHideAnimation = soTar.FindProperty("candleSettings.HideAnimation");
            SerializedProperty candleHideSpeed = soTar.FindProperty("candleSettings.HideSpeed");

            SerializedProperty candleItemID = soTar.FindProperty("candleSettings.itemID");

            SerializedProperty candleTypeRef = soTar.FindProperty("candleSettings.candleType");

            SerializedProperty candleParentRef = soTar.FindProperty("candleSettings.candleParent");
            SerializedProperty Candle = soTar.FindProperty("candleSettings.Candle");
            SerializedProperty candlePrefab = soTar.FindProperty("candleSettings.candlePrefab");

            SerializedProperty candleFlameParentRef = soTar.FindProperty("candleSettings.candleFlameParent");

            SerializedProperty candleFlameTypeRef = soTar.FindProperty("candleSettings.flameType");
            SerializedProperty CandleFlame = soTar.FindProperty("candleSettings.CandleFlame");
            SerializedProperty FlamePosition = soTar.FindProperty("candleSettings.FlamePosition");
            SerializedProperty flamePrefab = soTar.FindProperty("candleSettings.flamePrefab");

            SerializedProperty candleLightParentRef = soTar.FindProperty("candleSettings.candleLightParent");

            SerializedProperty candleLightTypeRef = soTar.FindProperty("candleSettings.lightType");
            SerializedProperty CandleLight = soTar.FindProperty("candleSettings.CandleLight");
            SerializedProperty candleLightPrefabRef = soTar.FindProperty("candleSettings.lightPrefab");

            //FLASHLIGHT

            SerializedProperty clickSound = soTar.FindProperty("flashlightSettings.clickSound");

            SerializedProperty flashIdleAnim = soTar.FindProperty("flashlightSettings.IdleAnim");

            SerializedProperty flashDrawAnim = soTar.FindProperty("flashlightSettings.DrawAnim");
            SerializedProperty flashDrawSpeed = soTar.FindProperty("flashlightSettings.DrawSpeed");

            SerializedProperty flashHideAnim = soTar.FindProperty("flashlightSettings.HideAnim");
            SerializedProperty flashHideSpeed = soTar.FindProperty("flashlightSettings.HideSpeed");

            SerializedProperty flashReloadAnim = soTar.FindProperty("flashlightSettings.ReloadAnim");
            SerializedProperty flashReloadSpeed = soTar.FindProperty("flashlightSettings.ReloadSpeed");

            SerializedProperty flashScareAnim = soTar.FindProperty("flashlightSettings.ScareAnim");
            SerializedProperty flashScareAnimSpeed = soTar.FindProperty("flashlightSettings.ScareAnimSpeed");

            SerializedProperty flashNoPowerAnim = soTar.FindProperty("flashlightSettings.NoPowerAnim");
            SerializedProperty flashNoPowerAnimSpeed = soTar.FindProperty("flashlightSettings.NoPowerAnimSpeed");

            SerializedProperty flashItemID = soTar.FindProperty("flashlightSettings.itemID");

            SerializedProperty flashLightTypeRef = soTar.FindProperty("flashlightSettings.flashLightType");
            SerializedProperty lightTypeRef = soTar.FindProperty("flashlightSettings.lightType");

            SerializedProperty flashlightParentRef = soTar.FindProperty("flashlightSettings.flashlightParent");
            SerializedProperty flashlightPrefabRef = soTar.FindProperty("flashlightSettings.flashlightPrefab");

            SerializedProperty flashlightLightParentRef = soTar.FindProperty("flashlightSettings.lightParent");
            SerializedProperty flashLightPrefab = soTar.FindProperty("flashlightSettings.lightPrefab");
            SerializedProperty flashLightObject = soTar.FindProperty("flashlightSettings.LightObject");

            SerializedProperty FlashlightIcon = soTar.FindProperty("flashlightSettings.FlashlightIcon");

            //LANTERN

            SerializedProperty lanternShowSound = soTar.FindProperty("lanternSettings.ShowSound");
            SerializedProperty lanternShowVolume = soTar.FindProperty("lanternSettings.ShowVolume");

            SerializedProperty lanternHideSound = soTar.FindProperty("lanternSettings.HideSound");
            SerializedProperty lanternHideVolume = soTar.FindProperty("lanternSettings.HideVolume");

            SerializedProperty lanternReloadOilSound = soTar.FindProperty("lanternSettings.ReloadOilSound");
            SerializedProperty lanternReloadVolume = soTar.FindProperty("lanternSettings.ReloadVolume");

            SerializedProperty lanternIdleAnim = soTar.FindProperty("lanternSettings.IdleAnim");

            SerializedProperty lanternDrawAnim = soTar.FindProperty("lanternSettings.DrawAnim");
            SerializedProperty lanternDrawSpeed = soTar.FindProperty("lanternSettings.DrawSpeed");

            SerializedProperty lanternHideAnim = soTar.FindProperty("lanternSettings.HideAnim");
            SerializedProperty lanternHideSpeed = soTar.FindProperty("lanternSettings.HideSpeed");

            SerializedProperty lanternReloadAnim = soTar.FindProperty("lanternSettings.ReloadAnim");
            SerializedProperty lanternReloadSpeed = soTar.FindProperty("lanternSettings.ReloadSpeed");

            SerializedProperty lanternItemID = soTar.FindProperty("lanternSettings.itemID");

            SerializedProperty LanternLight = soTar.FindProperty("lanternSettings.LanternLight");
            SerializedProperty lanternFlameMesh = soTar.FindProperty("lanternSettings.flameMesh");
            SerializedProperty lanternColorString = soTar.FindProperty("lanternSettings.ColorString");

            SerializedProperty hingeLantern = soTar.FindProperty("lanternSettings.hingeLantern");

            SerializedProperty LanternIcon = soTar.FindProperty("lanternSettings.LanternIcon");

            SerializedProperty lanternTypeRef = soTar.FindProperty("lanternSettings.lanternType");
            SerializedProperty lanternParentRef = soTar.FindProperty("lanternSettings.lanternParent");
            SerializedProperty lanternPrefabRef = soTar.FindProperty("lanternSettings.lanternPrefab");

            SerializedProperty lanternFlameTypeRef = soTar.FindProperty("lanternSettings.flameType");
            SerializedProperty lanternFlameParentRef = soTar.FindProperty("lanternSettings.flameParent");
            SerializedProperty lanternFlamePrefabRef = soTar.FindProperty("lanternSettings.flamePrefab");

            SerializedProperty lanternLightTypeRef = soTar.FindProperty("lanternSettings.lightType");
            SerializedProperty lanternLightParentRef = soTar.FindProperty("lanternSettings.lightParent");
            SerializedProperty lanternLightPrefabRef = soTar.FindProperty("lanternSettings.lightPrefab");

            SerializedProperty lanternHingeTypeRef = soTar.FindProperty("lanternSettings.hingeType");
            SerializedProperty lanternHingeParent = soTar.FindProperty("lanternSettings.hingeParent");

            SerializedProperty lanternHingeConTypeRef = soTar.FindProperty("lanternSettings.hingeConType");
            SerializedProperty lanternHingeConParentRef = soTar.FindProperty("lanternSettings.hingeConParent");
            SerializedProperty lanternHingeConPrefabRef = soTar.FindProperty("lanternSettings.hingeConPrefab");

            SerializedProperty lanternHingeConnect = soTar.FindProperty("lanternSettings.hingeConnect");

            //LIGHTER

            SerializedProperty lighterSoundLibrary = soTar.FindProperty("lighterSettings.soundLibrary");

            SerializedProperty lighterIdleAnimation = soTar.FindProperty("lighterSettings.IdleAnimation");

            SerializedProperty lighterDrawAnimation = soTar.FindProperty("lighterSettings.DrawAnimation");
            SerializedProperty lighterDrawSpeed = soTar.FindProperty("lighterSettings.DrawSpeed");

            SerializedProperty lighterHideAnimation = soTar.FindProperty("lighterSettings.HideAnimation");
            SerializedProperty lighterHideSpeed = soTar.FindProperty("lighterSettings.HideSpeed");

            SerializedProperty lighterAnimation = soTar.FindProperty("lighterSettings.lighterAnimation");

            SerializedProperty lighterOpenAnimation = soTar.FindProperty("lighterSettings.lighterOpenAnimation");
            SerializedProperty lighterCloseAnimation = soTar.FindProperty("lighterSettings.lighterCloseAnimation");

            SerializedProperty lighterItemID = soTar.FindProperty("lighterSettings.itemID");

            SerializedProperty lighterFlame = soTar.FindProperty("lighterSettings.flame");
            SerializedProperty lighterFlamePosition = soTar.FindProperty("lighterSettings.FlamePosition");

            SerializedProperty lighterLight = soTar.FindProperty("lighterSettings.light");

            SerializedProperty lighterLighterTypeRef = soTar.FindProperty("lighterSettings.lighterType");
            SerializedProperty lighterLighterParentRef = soTar.FindProperty("lighterSettings.lighterParent");
            SerializedProperty lighterLighterPrefabRef = soTar.FindProperty("lighterSettings.lighterPrefab");

            SerializedProperty lighterFlameTypeRef = soTar.FindProperty("lighterSettings.flameType");
            SerializedProperty lighterFlameParentRef = soTar.FindProperty("lighterSettings.flameParent");
            SerializedProperty lighterFlamePrefabRef = soTar.FindProperty("lighterSettings.flamePrefab");

            SerializedProperty lighterLightTypeRef = soTar.FindProperty("lighterSettings.lightType");
            SerializedProperty lighterLightParentRef = soTar.FindProperty("lighterSettings.lightParent");
            SerializedProperty lighterLightPrefabRef = soTar.FindProperty("lighterSettings.lightPrefab");

            //MELEE

            SerializedProperty meleeAudSource = soTar.FindProperty("meleeSettings.audioSource");

            SerializedProperty meleeDrawSound = soTar.FindProperty("meleeSettings.DrawSound");
            SerializedProperty meleeDrawVolume = soTar.FindProperty("meleeSettings.DrawVolume");

            SerializedProperty meleeHideSound = soTar.FindProperty("meleeSettings.HideSound");
            SerializedProperty meleeHideVolume = soTar.FindProperty("meleeSettings.HideVolume");

            SerializedProperty meleeSwaySound = soTar.FindProperty("meleeSettings.SwaySound");
            SerializedProperty meleeSwayVolume = soTar.FindProperty("meleeSettings.SwayVolume");

            SerializedProperty meleeDrawAnim = soTar.FindProperty("meleeSettings.DrawAnim");
            SerializedProperty meleeDrawSpeed = soTar.FindProperty("meleeSettings.DrawSpeed");

            SerializedProperty meleeHideAnim = soTar.FindProperty("meleeSettings.HideAnim");
            SerializedProperty meleeHideSpeed = soTar.FindProperty("meleeSettings.HideSpeed");

            SerializedProperty meleeAttackAnim = soTar.FindProperty("meleeSettings.AttackAnim");
            SerializedProperty meleeAttackSpeed = soTar.FindProperty("meleeSettings.AttackSpeed");

            SerializedProperty meleeItemID = soTar.FindProperty("meleeSettings.itemID");

            SerializedProperty meleeSurfaceID = soTar.FindProperty("meleeSettings.surfaceID");
            SerializedProperty meleeSurfaceDetails = soTar.FindProperty("meleeSettings.surfaceDetails");

            SerializedProperty meleeHitLayer = soTar.FindProperty("meleeSettings.HitLayer");    
            SerializedProperty meleeAttackDamage = soTar.FindProperty("meleeSettings.AttackDamage"); 
            SerializedProperty meleeSwayKickback = soTar.FindProperty("meleeSettings.SwayKickback");

            SerializedProperty meleeWeaponTypeRef = soTar.FindProperty("meleeSettings.weaponType");
            SerializedProperty meleeWeaponParentRef = soTar.FindProperty("meleeSettings.weaponParent"); 
            SerializedProperty meleeWeaponPrefabRef = soTar.FindProperty("meleeSettings.weaponPrefab");

            //GUN

            SerializedProperty gunReloadSound = soTar.FindProperty("gunSettings.audioSettings.reloadSound");

            SerializedProperty gunSoundDraw = soTar.FindProperty("gunSettings.audioSettings.soundDraw");
            SerializedProperty gunVolumeDraw = soTar.FindProperty("gunSettings.audioSettings.volumeDraw");

            SerializedProperty gunSoundFire = soTar.FindProperty("gunSettings.audioSettings.soundFire");
            SerializedProperty gunVolumeFire = soTar.FindProperty("gunSettings.audioSettings.volumeFire");

            SerializedProperty gunSoundEmpty = soTar.FindProperty("gunSettings.audioSettings.soundEmpty");
            SerializedProperty gunVolumeEmpty = soTar.FindProperty("gunSettings.audioSettings.volumeEmpty");

            SerializedProperty gunSoundReload = soTar.FindProperty("gunSettings.audioSettings.soundReload");
            SerializedProperty gunVolumeReload = soTar.FindProperty("gunSettings.audioSettings.volumeReload");

            SerializedProperty gunVolumeImpact = soTar.FindProperty("gunSettings.audioSettings.impactVolume");

            SerializedProperty gunAimPosition = soTar.FindProperty("gunSettings.aimingSettings.aimPosition");

            SerializedProperty gunHideAnim = soTar.FindProperty("gunSettings.animationSettings.hideAnim");
            SerializedProperty gunFireAnim = soTar.FindProperty("gunSettings.animationSettings.fireAnim");
            SerializedProperty gunReloadAnim = soTar.FindProperty("gunSettings.animationSettings.reloadAnim");

            SerializedProperty gunBeforeReloadAnim = soTar.FindProperty("gunSettings.animationSettings.beforeReloadAnim");
            SerializedProperty gunAfterReloadAnim = soTar.FindProperty("gunSettings.animationSettings.afterReloadAnim");
            SerializedProperty gunAfterReloadEmptyAnim = soTar.FindProperty("gunSettings.animationSettings.afterReloadEmptyAnim");

            SerializedProperty gunWeaponID= soTar.FindProperty("gunSettings.inventorySettings.weaponID");
            SerializedProperty gunBulletsID = soTar.FindProperty("gunSettings.inventorySettings.bulletsID");

            SerializedProperty gunBarrelEndPosition = soTar.FindProperty("gunSettings.bulletModelSettings.barrelEndPosition");
            SerializedProperty gunBulletPrefab = soTar.FindProperty("gunSettings.bulletModelSettings.bulletPrefab");
            SerializedProperty gunBulletRotation = soTar.FindProperty("gunSettings.bulletModelSettings.bulletRotation");

            SerializedProperty gunSurfaceID = soTar.FindProperty("gunSettings.bulletSettings.surfaceID");
            SerializedProperty gunSurfaceDetails = soTar.FindProperty("gunSettings.surfaceDetails");
            SerializedProperty gunFleshTag = soTar.FindProperty("gunSettings.bulletSettings.FleshTag");

            SerializedProperty gunMuzzleRotation = soTar.FindProperty("gunSettings.muzzleFlashSettings.muzzleRotation");
            SerializedProperty gunMuzzleFlashRef = soTar.FindProperty("gunSettings.muzzleFlashSettings.muzzleFlash");
            SerializedProperty gunMuzzleLightRef = soTar.FindProperty("gunSettings.muzzleFlashSettings.muzzleLight");

            #if (HFPS_163a || HFPS_163b)

                SerializedProperty gunEjectPosition = soTar.FindProperty("gunSettings.shotgunSettings.ejectPosition");
                SerializedProperty gunShellPrefab = soTar.FindProperty("gunSettings.shotgunSettings.shellPrefab");
                SerializedProperty gunShellRotation = soTar.FindProperty("gunSettings.shotgunSettings.shellRotation");

            #endif

            #if HFPS_163c

                SerializedProperty gunEjectPosition = soTar.FindProperty("gunSettings.shellEjectSettings.ejectPosition");
                SerializedProperty gunShellPrefab = soTar.FindProperty("gunSettings.shellEjectSettings.shellPrefab");
                SerializedProperty gunShellRotation = soTar.FindProperty("gunSettings.shellEjectSettings.shellRotation");

            #endif

            SerializedProperty gunWeaponType = soTar.FindProperty("gunSettings.weaponType");
            SerializedProperty gunBulletType = soTar.FindProperty("gunSettings.bulletType");
            SerializedProperty gunRaycastMask = soTar.FindProperty("gunSettings.raycastMask");
            SerializedProperty gunSoundReactionMask = soTar.FindProperty("gunSettings.soundReactionMask");

            SerializedProperty gunKickbackRef = soTar.FindProperty("gunSettings.kickback");

            SerializedProperty gunAudioSource = soTar.FindProperty("gunSettings.audioSource");

            SerializedProperty gunWeapModelTypeRef = soTar.FindProperty("gunSettings.weapModelType");
            SerializedProperty gunWeaponParentRef = soTar.FindProperty("gunSettings.weaponParent");
            SerializedProperty gunWeaponPrefabRef = soTar.FindProperty("gunSettings.weaponPrefab");

            SerializedProperty gunMuzzleFlashTypeRef = soTar.FindProperty("gunSettings.muzzleFlashType");
            SerializedProperty gunMuzzleFlashParentRef = soTar.FindProperty("gunSettings.muzzleFlashParent");
            SerializedProperty gunMuzzleFlashPrefabRef = soTar.FindProperty("gunSettings.muzzleFlashPrefab");

            SerializedProperty gunMuzzleLightTypeRef = soTar.FindProperty("gunSettings.muzzleLightType");
            SerializedProperty gunMuzzleLightParentRef = soTar.FindProperty("gunSettings.muzzleLightParent");
            SerializedProperty gunMuzzleLightPrefabRef = soTar.FindProperty("gunSettings.muzzleLightPrefab");

            //END

            if(tempWeapon != null && tempArms != null){

                displayShow = true;
                bottomShow = true;

            //tempWeapon & tempArms != null
            } else {

                displayShow = false;
                bottomShow = false;

            }//tempWeapon & tempArms != null

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            language = (DM_InternEnums.Language)EditorGUILayout.EnumPopup("Language", language); 

            if(dmMenusLocData != null){

                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[0].local)) {

                    Language_Save();

                }//Button

            }//dmMenusLocData != null

            EditorGUILayout.EndHorizontal();

            if(dmMenusLocData != null){

                if(verNumb == "Unknown"){

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[0].texts[0].text, MessageType.Error);

                //verNumb == "Unknown"
                } else {

                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Info);

                    if(displayShow){

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(weaponTypeRef, true);
                        EditorGUILayout.PropertyField(templateRef, true);

                    //displayShow
                    } else {

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(templateRef, true);

                    }//displayShow

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(weaponNameRef, true);

                    if(displayShow){

                        EditorGUILayout.PropertyField(armsParentRef, true);

                    //displayShow
                    } else {

                        EditorGUILayout.PropertyField(armsPrefabRef, true);

                    }//displayShow

                    EditorGUILayout.PropertyField(itemSwitcherRef, true);

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    if(displayShow){

                        if(weaponType == Weapon_Type.Candle){

                            UI_Hide(Weapon_Type.Flashlight);
                            UI_Hide(Weapon_Type.Lantern);
                            UI_Hide(Weapon_Type.Lighter);
                            UI_Hide(Weapon_Type.Melee);
                            UI_Hide(Weapon_Type.Gun);

                            if(!candleAuto.animOpts && !candleAuto.audOpts && !candleAuto.candleOpts && !candleAuto.invOpts){

                                scrollPositions.candle_scrollPos = GUILayout.BeginScrollView(scrollPositions.candle_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!animOpts, !audOpts, !candleOpts & !invOpts

                            if(!candleAuto.animOpts && !candleAuto.candleOpts && !candleAuto.invOpts){

                                candleAuto.audOpts = GUILayout.Toggle(candleAuto.audOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[0].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !candleOpts & !invOpts

                            if(!candleAuto.audOpts && !candleAuto.candleOpts && !candleAuto.invOpts){

                                candleAuto.animOpts = GUILayout.Toggle(candleAuto.animOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[1].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !candleOpts & !invOpts

                            if(!candleAuto.audOpts && !candleAuto.animOpts && !candleAuto.invOpts){

                                candleAuto.candleOpts = GUILayout.Toggle(candleAuto.candleOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[2].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !audOpts & !invOpts

                            if(!candleAuto.audOpts && !candleAuto.animOpts && !candleAuto.candleOpts){

                                candleAuto.invOpts = GUILayout.Toggle(candleAuto.invOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[3].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !audOpts & !candleOpts

                            if(candleAuto.audOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[0].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.candleAudOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.candleAudOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(BlowOut, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//audOpts

                            if(candleAuto.animOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[1].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.candleAnimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.candleAnimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.PropertyField(candleIdleAnimation, true);
                                EditorGUILayout.PropertyField(candleBlowOutAnimation, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(candleDrawAnimation, true);
                                EditorGUILayout.PropertyField(candleDrawSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(candleHideAnimation, true);
                                EditorGUILayout.PropertyField(candleHideSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//animOpts

                            if(candleAuto.candleOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[2].text, MessageType.Info);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                candleAuto.candleTabs = GUILayout.SelectionGrid(candleAuto.candleTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[4].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[5].local }, 2);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.candleCanOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.candleCanOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                if(candleAuto.candleTabs == 0){

                                    EditorGUILayout.Space();

                                    candleAuto.candleModelTabs = GUILayout.SelectionGrid(candleAuto.candleModelTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[6].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[7].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[8].local }, 3);

                                    GUILayout.Space(5);

                                    if(candleAuto.candleModelTabs == 0){

                                        EditorGUILayout.PropertyField(candleTypeRef, true);

                                        if(candleSettings.candleType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            if(candleSettings.Candle != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[0].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //Candle != null
                                            } else {

                                                EditorGUILayout.PropertyField(candleParentRef, true);

                                                if(candleSettings.candleParent != null){

                                                    GUI.enabled = true;

                                                //candleParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//candleParent != null

                                                EditorGUILayout.PropertyField(candlePrefab, true);

                                                EditorGUILayout.Space();

                                                if(candleSettings.candlePrefab != null){

                                                    GUI.enabled = true;

                                                //candlePrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//candlePrefab != null

                                            }//Candle != null

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[9].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].buttons[1].local)){

                                                    Candle_Create();

                                                }//DisplayDialog  

                                            }//Buttton

                                        }//candleType = create

                                        if(candleSettings.candleType == Model_Type.Local){

                                            EditorGUILayout.PropertyField(Candle, true);

                                        }//candleType = local

                                    }//candleModelTabs = candle

                                    if(candleAuto.candleModelTabs == 1){

                                        EditorGUILayout.PropertyField(candleFlameTypeRef, true);

                                        if(candleSettings.flameType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            if(candleSettings.CandleFlame != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[1].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //CandleFlame != null
                                            } else {

                                                EditorGUILayout.PropertyField(candleFlameParentRef, new GUIContent("Flame Parent"), true);

                                                if(candleSettings.candleFlameParent != null){

                                                    GUI.enabled = true;

                                                //candleFlameParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//candleFlameParent != null

                                                EditorGUILayout.PropertyField(flamePrefab, true);

                                                EditorGUILayout.Space();

                                                if(candleSettings.flamePrefab != null){

                                                    GUI.enabled = true;

                                                //flamePrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//flamePrefab != null

                                            }//CandleFlame != null

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[10].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].buttons[1].local)){

                                                    CandleFlame_Create();

                                                }//DisplayDialog  

                                            }//Buttton

                                        }//flameType = create

                                        if(candleSettings.flameType == Model_Type.Local){

                                            EditorGUILayout.Space();

                                            EditorGUILayout.PropertyField(CandleFlame, true);
                                            EditorGUILayout.PropertyField(FlamePosition, true);

                                        }//flameType = local

                                    }//candleModelTabs = flame

                                    if(candleAuto.candleModelTabs == 2){

                                        EditorGUILayout.PropertyField(candleLightTypeRef, true);

                                        if(candleSettings.lightType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            if(candleSettings.CandleLight != null){

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[2].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //CandleLight != null
                                            } else {

                                                EditorGUILayout.PropertyField(candleLightParentRef, new GUIContent("Light Parent"), true);

                                                if(candleSettings.candleLightParent != null){

                                                    GUI.enabled = true;

                                                //candleLightParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//candleLightParent != null

                                                EditorGUILayout.PropertyField(candleLightPrefabRef, true);

                                                EditorGUILayout.Space();

                                                if(candleSettings.lightPrefab != null){

                                                    GUI.enabled = true;

                                                //lightPrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightPrefab != null

                                            }//CandleLight != null

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[11].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[2].buttons[1].local)){

                                                    CandleLight_Create();

                                                }//DisplayDialog  

                                            }//Buttton

                                        }//lightType = create

                                        if(candleSettings.lightType == Model_Type.Local){

                                            EditorGUILayout.Space();

                                            EditorGUILayout.PropertyField(CandleLight, true);

                                        }//lightType = local

                                    }//candleModelTabs = lighy

                                }//candleTabs = model

                                if(candleAuto.candleTabs == 1){

                                    EditorGUILayout.Space();

                                    candleSettings.candleReduction = EditorGUILayout.Toggle("Candle Reduction?", candleSettings.candleReduction);

                                    if(candleSettings.candleReduction){

                                        EditorGUILayout.Space();

                                        candleSettings.reductionRate = EditorGUILayout.FloatField("Reduction Rate", candleSettings.reductionRate);
                                        candleSettings.maxScale = EditorGUILayout.FloatField("Max Scale", candleSettings.maxScale);
                                        candleSettings.minScale = EditorGUILayout.FloatField("Min Scale", candleSettings.minScale);

                                    }//candleReduction

                                }//candleTabs = reduction

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//candleOpts

                            if(candleAuto.invOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[3].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.candleInvOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.candleInvOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                GUILayout.Space(15);

                                EditorGUILayout.PropertyField(candleItemID, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.Space();

                                candleSettings.blowOutKeepCandle = EditorGUILayout.Toggle("Blow Out Keep Candle", candleSettings.blowOutKeepCandle);

                                if(candleSettings.blowOutKeepCandle){

                                    candleSettings.scaleKeepCandle = EditorGUILayout.FloatField("Scale Keep Candle", candleSettings.scaleKeepCandle);

                                }//blowOutKeepCandle

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//invOpts

                            if(!candleAuto.animOpts && !candleAuto.audOpts && !candleAuto.candleOpts && !candleAuto.invOpts){

                                EditorGUILayout.EndScrollView();

                                bottomShow = true;

                            }//!animOpts, !audOpts, !candleOpts & !invOpts

                        }//weaponType = Candle

                        if(weaponType == Weapon_Type.Flashlight){

                            UI_Hide(Weapon_Type.Candle);
                            UI_Hide(Weapon_Type.Lantern);
                            UI_Hide(Weapon_Type.Lighter);
                            UI_Hide(Weapon_Type.Melee);
                            UI_Hide(Weapon_Type.Gun);

                            if(!flashlightAuto.audOpts && !flashlightAuto.animOpts && !flashlightAuto.flashOpts && !flashlightAuto.invOpts && !flashlightAuto.uiOpts){

                                scrollPositions.flashlight_scrollPos = GUILayout.BeginScrollView(scrollPositions.flashlight_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!audOpts, !animOpts, !flashOpts, !invOpts & !uiOpts

                            if(!flashlightAuto.animOpts && !flashlightAuto.flashOpts && !flashlightAuto.invOpts && !flashlightAuto.uiOpts){

                                flashlightAuto.audOpts = GUILayout.Toggle(flashlightAuto.audOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[0].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !flashOpts, !invOpts & !uiOpts

                            if(!flashlightAuto.audOpts && !flashlightAuto.flashOpts && !flashlightAuto.invOpts && !flashlightAuto.uiOpts){

                                flashlightAuto.animOpts = GUILayout.Toggle(flashlightAuto.animOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[1].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !flashOpts, !invOpts & !uiOpts

                            if(!flashlightAuto.audOpts && !flashlightAuto.animOpts && !flashlightAuto.invOpts && !flashlightAuto.uiOpts){

                                flashlightAuto.flashOpts = GUILayout.Toggle(flashlightAuto.flashOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[2].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !invOpts & !uiOpts

                            if(!flashlightAuto.audOpts && !flashlightAuto.animOpts && !flashlightAuto.flashOpts && !flashlightAuto.uiOpts){

                                flashlightAuto.invOpts = GUILayout.Toggle(flashlightAuto.invOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[3].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !flashOpts & !uiOpts

                            if(!flashlightAuto.audOpts && !flashlightAuto.animOpts && !flashlightAuto.flashOpts && !flashlightAuto.invOpts){

                                flashlightAuto.uiOpts = GUILayout.Toggle(flashlightAuto.uiOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[4].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !flashOpts & !invOpts

                            if(flashlightAuto.audOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[0].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.flashAudOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.flashAudOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(clickSound, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//audOpts

                            if(flashlightAuto.animOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[1].text, MessageType.Info);

                                EditorGUILayout.Space();

                                flashlightAuto.animTabs = GUILayout.SelectionGrid(flashlightAuto.animTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[5].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[6].local }, 2);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.flashAnimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.flashAnimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                if(flashlightAuto.animTabs == 0){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(flashIdleAnim, true);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(flashDrawAnim, new GUIContent("Draw Animation"), true);
                                    EditorGUILayout.PropertyField(flashDrawSpeed, true);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(flashHideAnim, new GUIContent("Hide Animation"), true);
                                    EditorGUILayout.PropertyField(flashHideSpeed, true);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(flashReloadAnim, new GUIContent("Reload Animation"), true);
                                    EditorGUILayout.PropertyField(flashReloadSpeed, true);

                                }//animTabs = general

                                if(flashlightAuto.animTabs == 1){

                                    EditorGUILayout.Space();

                                    flashlightSettings.enableExtra = EditorGUILayout.Toggle("Enable Extra?", flashlightSettings.enableExtra);

                                    if(flashlightSettings.enableExtra){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(flashScareAnim, new GUIContent("Scare Animation"), true);
                                        EditorGUILayout.PropertyField(flashScareAnimSpeed, new GUIContent("Scare Speed"), true);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(flashNoPowerAnim, new GUIContent("No Power Animation"), true);
                                        EditorGUILayout.PropertyField(flashNoPowerAnimSpeed, new GUIContent("No Power Speed"),true);

                                    }//enableExtra

                                }//animTabs = Extra

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//animOpts

                            if(flashlightAuto.flashOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[2].text, MessageType.Info);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                flashlightAuto.flashTabs = GUILayout.SelectionGrid(flashlightAuto.flashTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[7].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[8].local }, 2);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.flashFlashOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.flashFlashOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                if(flashlightAuto.flashTabs == 0){

                                    EditorGUILayout.Space();

                                    flashlightAuto.flashModelTabs = GUILayout.SelectionGrid(flashlightAuto.flashModelTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[9].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[10].local }, 2);

                                    if(flashlightAuto.flashModelTabs == 0){

                                        GUILayout.Space(5);

                                        EditorGUILayout.PropertyField(flashLightTypeRef, true);

                                        if(flashlightSettings.flashLightType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            EditorGUILayout.PropertyField(flashlightParentRef, true);

                                            if(flashlightSettings.flashlightParent != null){

                                                GUI.enabled = true;

                                            //flashlightParent != null
                                            } else {

                                                GUI.enabled = false;

                                            }//flashlightParent != null

                                            EditorGUILayout.PropertyField(flashlightPrefabRef, true);

                                            EditorGUILayout.Space();

                                            if(flashlightSettings.flashlightParent != null){

                                                if(flashlightSettings.flashlightPrefab != null){

                                                    GUI.enabled = true;

                                                //flashlightPrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//flashlightPrefab != null

                                            //flashlightParent != null
                                            } else {

                                                GUI.enabled = false;

                                            }//flashlightParent != null

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[11].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[0].buttons[1].local)){

                                                    Flashlight_Create();

                                                }//DisplayDialog  

                                            }//Buttton

                                        }//flashLightType = create

                                    }//flashModelTabs = flashlight

                                    if(flashlightAuto.flashModelTabs == 1){

                                        GUILayout.Space(5);

                                        EditorGUILayout.PropertyField(lightTypeRef, true);

                                        if(flashlightSettings.lightType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            if(flashlightSettings.LightObject != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.Space();

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].singleValues[0].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //LightObject != null
                                            } else {

                                                EditorGUILayout.PropertyField(flashlightLightParentRef, true);

                                                if(flashlightSettings.lightParent != null){

                                                    GUI.enabled = true;

                                                //lightParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightParent != null

                                                EditorGUILayout.PropertyField(flashLightPrefab, new GUIContent("Light Object"), true);

                                                EditorGUILayout.Space();

                                                if(flashlightSettings.lightParent != null){

                                                    if(flashlightSettings.lightPrefab != null){

                                                        GUI.enabled = true;

                                                    //lightPrefab != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//lightPrefab != null

                                                //lightParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightParent != null

                                            }//LightObject != null

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].buttons[12].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].prompts[1].buttons[1].local)){

                                                    FlashlightLight_Create();

                                                }//DisplayDialog 

                                            }//Buttton

                                        }//lightType = create

                                        if(flashlightSettings.lightType == Model_Type.Local){

                                            EditorGUILayout.PropertyField(flashLightObject, true);
                                            flashlightSettings.flashlightIntensity = EditorGUILayout.FloatField("Light Intensity", flashlightSettings.flashlightIntensity);

                                        }//lightType = local

                                    }//flashModelTabs = light

                                }//flashTabs = models

                                if(flashlightAuto.flashTabs == 1){

                                    EditorGUILayout.Space();

                                    flashlightSettings.infiniteBattery = EditorGUILayout.Toggle("Infinite Battery?", flashlightSettings.infiniteBattery);

                                    if(!flashlightSettings.infiniteBattery){

                                        EditorGUILayout.Space();

                                        flashlightSettings.batteryLifeInSec = EditorGUILayout.FloatField("Battery Life In Seconds", flashlightSettings.batteryLifeInSec);
                                        flashlightSettings.canReloadPercent = EditorGUILayout.FloatField("Can Reload Percent", flashlightSettings.canReloadPercent);

                                    }//infiniteBattery

                                }//flashTabs = battery

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//flashOpts

                            if(flashlightAuto.invOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[3].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.flashInvOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.flashInvOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(flashItemID, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//invOpts

                            if(flashlightAuto.uiOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[3].texts[4].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.flashUIOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.flashUIOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.PropertyField(FlashlightIcon, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//uiOpts

                            if(!flashlightAuto.audOpts && !flashlightAuto.animOpts && !flashlightAuto.flashOpts && !flashlightAuto.invOpts && !flashlightAuto.uiOpts){

                                EditorGUILayout.EndScrollView();

                                bottomShow = true;

                            }//!audOpts, !animOpts, !flashOpts, !invOpts & !uiOpts

                        }//weaponType = Flashlight

                        if(weaponType == Weapon_Type.Lantern){

                            UI_Hide(Weapon_Type.Candle);
                            UI_Hide(Weapon_Type.Flashlight);
                            UI_Hide(Weapon_Type.Lighter);
                            UI_Hide(Weapon_Type.Melee);
                            UI_Hide(Weapon_Type.Gun);

                            if(!lanternAuto.audOpts && !lanternAuto.animOpts && !lanternAuto.invOpts && !lanternAuto.lantOpts && !lanternAuto.uiOpts){

                                scrollPositions.lantern_scrollPos = GUILayout.BeginScrollView(scrollPositions.lantern_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!audOpts, !animOpts, !invOpts, !lantOpts & !uiOpts

                            if(!lanternAuto.animOpts && !lanternAuto.invOpts && !lanternAuto.lantOpts && !lanternAuto.uiOpts){

                                lanternAuto.audOpts = GUILayout.Toggle(lanternAuto.audOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[0].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !invOpts, !lantOpts & !uiOpts

                            if(!lanternAuto.audOpts && !lanternAuto.invOpts && !lanternAuto.lantOpts && !lanternAuto.uiOpts){

                                lanternAuto.animOpts = GUILayout.Toggle(lanternAuto.animOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[1].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !invOpts, !lantOpts & !uiOpts

                            if(!lanternAuto.audOpts && !lanternAuto.animOpts && !lanternAuto.lantOpts && !lanternAuto.uiOpts){

                                lanternAuto.invOpts = GUILayout.Toggle(lanternAuto.invOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[2].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !lantOpts & !uiOpts

                            if(!lanternAuto.audOpts && !lanternAuto.animOpts && !lanternAuto.invOpts && !lanternAuto.uiOpts){

                                lanternAuto.lantOpts = GUILayout.Toggle(lanternAuto.lantOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[3].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !invOpts & !uiOpts

                            if(!lanternAuto.audOpts && !lanternAuto.animOpts && !lanternAuto.invOpts && !lanternAuto.lantOpts){

                                lanternAuto.uiOpts = GUILayout.Toggle(lanternAuto.uiOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[4].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !invOpts & !lantOpts

                            if(lanternAuto.audOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].texts[0].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.lantAudOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lantAudOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternShowSound, true);
                                EditorGUILayout.PropertyField(lanternShowVolume, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternHideSound, true);
                                EditorGUILayout.PropertyField(lanternHideVolume, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternReloadOilSound, true);
                                EditorGUILayout.PropertyField(lanternReloadVolume, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//audOpts

                            if(lanternAuto.animOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].texts[1].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.lantAnimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lantAnimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternIdleAnim, new GUIContent("Idle Animation"), true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternDrawAnim, new GUIContent("Draw Animation"),true);
                                EditorGUILayout.PropertyField(lanternDrawSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternHideAnim, new GUIContent("Hide Animation"),true);
                                EditorGUILayout.PropertyField(lanternHideSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternReloadAnim, new GUIContent("Reload Animation"),true);
                                EditorGUILayout.PropertyField(lanternReloadSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//animOpts

                            if(lanternAuto.invOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].texts[2].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.lantInvOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lantInvOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(lanternItemID, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//invOpts

                            if(lanternAuto.lantOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].texts[3].text, MessageType.Info);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                lanternAuto.lanternTabs = GUILayout.SelectionGrid(lanternAuto.lanternTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[5].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[6].local }, 2);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.lantLantOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lantLantOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                if(lanternAuto.lanternTabs == 0){

                                    lanternAuto.lanternModelTabs = GUILayout.SelectionGrid(lanternAuto.lanternModelTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[8].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[9].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[10].local }, 3);

                                    if(lanternAuto.lanternModelTabs == 0){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(lanternTypeRef, true);

                                        EditorGUILayout.Space();

                                        if(lanternSettings.lanternType == Model_Type.Create){

                                            EditorGUILayout.PropertyField(lanternParentRef, true);

                                            if(lanternSettings.lanternParent != null){

                                                GUI.enabled = true;

                                            //lanternParent != null
                                            } else {

                                                GUI.enabled = false;

                                            }//lanternParent != null

                                            EditorGUILayout.PropertyField(lanternPrefabRef, true);

                                            if(lanternSettings.lanternPrefab != null){

                                                GUI.enabled = true;

                                            //lanternPrefab != null
                                            } else {

                                                GUI.enabled = false;

                                            }//lanternPrefab != null

                                            EditorGUILayout.Space();

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[11].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[0].buttons[1].local)){

                                                    Lantern_Create();

                                                }//DisplayDialog 

                                            }//Buttton

                                        }//lanternType = create

                                    }//lanternModelTabs = lantern

                                    if(lanternAuto.lanternModelTabs == 1){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(lanternFlameTypeRef, true);

                                        EditorGUILayout.Space();

                                        if(lanternSettings.flameType == Model_Type.Create){

                                            if(lanternSettings.flameMesh != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.Space();

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].singleValues[1].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //flameMesh != null
                                            } else {

                                                #if COMPONENTS_PRESENT

                                                    lanternAuto.showFlameCreate = true;

                                                #else

                                                    if(lanternSettings.LanternLight != null){

                                                        lanternAuto.showFlameCreate = true;

                                                    //LanternLight != null
                                                    } else {

                                                        lanternAuto.showFlameCreate = false;

                                                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].singleValues[0].local, MessageType.Warning);

                                                        GUI.enabled = false;

                                                    }//LanternLight != null

                                                #endif

                                                if(lanternAuto.showFlameCreate){

                                                    EditorGUILayout.PropertyField(lanternFlameParentRef, true);

                                                    if(lanternSettings.flameParent != null){

                                                        GUI.enabled = true;

                                                    //flameParent != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//flameParent != null

                                                    EditorGUILayout.PropertyField(lanternFlamePrefabRef, true);

                                                    if(lanternSettings.flamePrefab != null){

                                                        GUI.enabled = true;

                                                    //flamePrefab != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//flamePrefab != null

                                                }//showFlameCreate

                                            }//flameMesh != null

                                            EditorGUILayout.Space();

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[12].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[1].buttons[1].local)){

                                                    LanternFlame_Create();

                                                }//DisplayDialog

                                            }//Buttton

                                        }//flameType = create

                                        if(lanternSettings.flameType == Model_Type.Local){

                                            EditorGUILayout.PropertyField(lanternFlameMesh, true);

                                        }//flameType = local

                                    }//lanternModelTabs = flame

                                    if(lanternAuto.lanternModelTabs == 2){

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(lanternLightTypeRef, true);

                                        EditorGUILayout.Space();

                                        if(lanternSettings.lightType == Model_Type.Create){

                                            if(lanternSettings.LanternLight != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.Space();

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].singleValues[2].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //LanternLight != null
                                            } else {

                                                EditorGUILayout.PropertyField(lanternLightParentRef, true);

                                                if(lanternSettings.lightParent != null){

                                                    GUI.enabled = true;

                                                //lightParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightParent != null

                                                EditorGUILayout.PropertyField(lanternLightPrefabRef, true);

                                                if(lanternSettings.lightPrefab != null){

                                                    GUI.enabled = true;

                                                //lightPrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightPrefab != null

                                            }//LanternLight != null

                                            EditorGUILayout.Space();

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[13].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[2].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[2].buttons[1].local)){

                                                    LanternLight_Create();

                                                }//DisplayDialog

                                            }//Buttton

                                        }//lightType = create

                                        if(lanternSettings.lightType == Model_Type.Local){

                                            EditorGUILayout.PropertyField(LanternLight, true);
                                            EditorGUILayout.PropertyField(lanternColorString, true);

                                        }//lightType = local

                                    }//lanternModelTabs = light

                                }//lanternTabs = model

                                if(lanternAuto.lanternTabs == 1){

                                    EditorGUILayout.Space();

                                    lanternSettings.useHingeJoint = EditorGUILayout.Toggle("Use Hinge Joint?", lanternSettings.useHingeJoint);

                                    if(lanternSettings.useHingeJoint){

                                        EditorGUILayout.Space();

                                        lanternAuto.lanternHingeTabs = GUILayout.SelectionGrid(lanternAuto.lanternHingeTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[6].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[7].local }, 2);

                                        if(lanternAuto.lanternHingeTabs == 0){

                                            EditorGUILayout.PropertyField(lanternHingeTypeRef, true);

                                            EditorGUILayout.Space();

                                            if(lanternSettings.hingeType == Model_Type.Create){

                                                if(lanternSettings.hingeLantern != null){

                                                    GUI.enabled = true;

                                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].singleValues[4].local, MessageType.Warning);

                                                    EditorGUILayout.Space();

                                                    GUI.enabled = false;

                                                //hingeLantern != null
                                                } else {

                                                    EditorGUILayout.PropertyField(lanternHingeParent, true);

                                                    if(lanternSettings.hingeParent != null){

                                                        GUI.enabled = true;

                                                    //hingeParent != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//hingeParent != null

                                                }//hingeLantern != null

                                                EditorGUILayout.Space();

                                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[14].local)){

                                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[3].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[3].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[3].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[3].buttons[1].local)){

                                                        LanternHinge_Create();

                                                    }//DisplayDialog

                                                }//Buttton

                                            }//hingeType = create

                                            if(lanternSettings.hingeType == Model_Type.Local){

                                                EditorGUILayout.PropertyField(hingeLantern, true);
                                                lanternSettings.secondDrawDiff = EditorGUILayout.FloatField("Second Draw Difference", lanternSettings.secondDrawDiff);

                                            }//hingeType = local

                                        }//lanternHingeTabs = hinge

                                        if(lanternAuto.lanternHingeTabs == 1){

                                            EditorGUILayout.PropertyField(lanternHingeConTypeRef, new GUIContent("Hinge Connect Type"), true);

                                            EditorGUILayout.Space();

                                            if(lanternSettings.hingeConType == Model_Type.Create){

                                                if(lanternSettings.hingeConnect != null){

                                                    GUI.enabled = true;

                                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].singleValues[5].local, MessageType.Warning);

                                                    EditorGUILayout.Space();

                                                    GUI.enabled = false;

                                                //hingeConnect != null
                                                } else {

                                                    if(lanternSettings.hingeLantern != null){

                                                        EditorGUILayout.PropertyField(lanternHingeConParentRef, true);

                                                        if(lanternSettings.hingeConParent != null){

                                                            GUI.enabled = true;

                                                        //hingeConParent != null
                                                        } else {

                                                            GUI.enabled = false;

                                                        }//hingeConParent != null

                                                        EditorGUILayout.PropertyField(lanternHingeConPrefabRef, new GUIContent("Hinge Connect Prefab"), true);

                                                    //hingeLantern != null
                                                    } else {

                                                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].singleValues[3].local, MessageType.Warning);

                                                        GUI.enabled = false;

                                                    }//hingeLantern != null

                                                }//hingeConnect != null

                                                EditorGUILayout.Space();

                                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].buttons[15].local)){

                                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[4].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[4].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[4].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].prompts[4].buttons[1].local)){

                                                        LanternHingeConnect_Create();

                                                    }//DisplayDialog

                                                }//Buttton

                                            }//hingeConType = create

                                            if(lanternSettings.hingeConType == Model_Type.Local){

                                                EditorGUILayout.PropertyField(lanternHingeConnect, true);

                                            }//hingeConType = local

                                        }//lanternHingeTabs = hinge connect

                                    }//useHingeJoint

                                }//lanternTabs = light

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//lantOpts

                            if(lanternAuto.uiOpts){

                                bottomShow = false;

                                GUILayout.Space(5);

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[4].texts[4].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.lantUIOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lantUIOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(LanternIcon, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//uiOpts

                            if(!lanternAuto.audOpts && !lanternAuto.animOpts && !lanternAuto.invOpts && !lanternAuto.lantOpts && !lanternAuto.uiOpts){

                                EditorGUILayout.EndScrollView();

                                bottomShow = true;

                            }//!audOpts, !animOpts, !invOpts, !lantOpts & !uiOpts

                        }//weaponType = Lantern

                        if(weaponType == Weapon_Type.Lighter){

                            UI_Hide(Weapon_Type.Candle);
                            UI_Hide(Weapon_Type.Flashlight);
                            UI_Hide(Weapon_Type.Lantern);
                            UI_Hide(Weapon_Type.Melee);
                            UI_Hide(Weapon_Type.Gun);

                            #if COMPONENTS_PRESENT

                                if(!lighterAuto.audOpts && !lighterAuto.animOpts && !lighterAuto.invOpts && !lighterAuto.lighterOpts){

                                    scrollPositions.lighter_scrollPos = GUILayout.BeginScrollView(scrollPositions.lighter_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                }//!audOpts, !animOpts, !invOpts & !lighterOpts

                                if(!lighterAuto.animOpts && !lighterAuto.invOpts && !lighterAuto.lighterOpts){

                                    lighterAuto.audOpts = GUILayout.Toggle(lighterAuto.audOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[0].local, GUI.skin.button);

                                    GUILayout.Space(5);

                                }//!animOpts, !invOpts & !lighterOpts

                                if(!lighterAuto.audOpts && !lighterAuto.invOpts && !lighterAuto.lighterOpts){

                                    lighterAuto.animOpts = GUILayout.Toggle(lighterAuto.animOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[1].local, GUI.skin.button);

                                    GUILayout.Space(5);

                                }//!animOpts, !invOpts & !lighterOpts

                                if(!lighterAuto.audOpts && !lighterAuto.animOpts && !lighterAuto.lighterOpts){

                                    lighterAuto.invOpts = GUILayout.Toggle(lighterAuto.invOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[2].local, GUI.skin.button);

                                    GUILayout.Space(5);

                                }//!audOpts, !animOpts & !lighterOpts

                                if(!lighterAuto.audOpts && !lighterAuto.animOpts && !lighterAuto.invOpts){

                                    lighterAuto.lighterOpts = GUILayout.Toggle(lighterAuto.lighterOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[3].local, GUI.skin.button);

                                    GUILayout.Space(5);

                                }//!audOpts, !animOpts & !invOpts

                                if(lighterAuto.audOpts){

                                    bottomShow = false;

                                    GUILayout.Space(5);

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].texts[0].text, MessageType.Info);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.lighterAudOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lighterAudOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(lighterSoundLibrary, true);

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//audOpts

                                if(lighterAuto.animOpts){

                                    bottomShow = false;

                                    GUILayout.Space(5);

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].texts[1].text, MessageType.Info);

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    lighterAuto.animTabs = GUILayout.SelectionGrid(lighterAuto.animTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[4].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[5].local }, 2);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.lighterAnimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lighterAnimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    if(lighterAuto.animTabs == 0){

                                        EditorGUILayout.PropertyField(lighterIdleAnimation, true);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(lighterDrawAnimation, true);
                                        EditorGUILayout.PropertyField(lighterDrawSpeed, true);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(lighterHideAnimation, true);
                                        EditorGUILayout.PropertyField(lighterHideSpeed, true);

                                    }//animTabs = arms

                                    if(lighterAuto.animTabs == 1){

                                        EditorGUILayout.PropertyField(lighterAnimation, true);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(lighterOpenAnimation, true);
                                        EditorGUILayout.PropertyField(lighterCloseAnimation, true);

                                    }//animTabs = lighter

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//animOpts

                                if(lighterAuto.invOpts){

                                    bottomShow = false;

                                    GUILayout.Space(5);

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].texts[2].text, MessageType.Info);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.lighterInvOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lighterInvOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(lighterItemID, true);

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//invOpts

                                if(lighterAuto.lighterOpts){

                                    bottomShow = false;

                                    GUILayout.Space(5);

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].texts[3].text, MessageType.Info);

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    lighterAuto.lighterTabs = GUILayout.SelectionGrid(lighterAuto.lighterTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[6].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[7].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[8].local }, 3);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.lighterLighterOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.lighterLighterOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    if(lighterAuto.lighterTabs == 0){

                                        EditorGUILayout.PropertyField(lighterLighterTypeRef, true);

                                        EditorGUILayout.Space();

                                        if(lighterSettings.lighterType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            EditorGUILayout.PropertyField(lighterLighterParentRef, true);

                                            if(lighterSettings.lighterParent != null){

                                                GUI.enabled = true;

                                            //lighterParent != null
                                            } else {

                                                GUI.enabled = false;

                                            }//lighterParent != null

                                            EditorGUILayout.PropertyField(lighterLighterPrefabRef, true);

                                            if(lighterSettings.lighterPrefab != null){

                                                GUI.enabled = true;

                                            //lighterPrefab != null
                                            } else {

                                                GUI.enabled = false;

                                            }//lighterPrefab != null

                                            EditorGUILayout.Space();

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[9].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[0].buttons[1].local)){

                                                    LighterModel_Create();

                                                }//DisplayDialog

                                            }//Buttton

                                        }//lighterType = create

                                    }//lighterTabs = model

                                    if(lighterAuto.lighterTabs == 1){

                                        EditorGUILayout.PropertyField(lighterFlameTypeRef, true);

                                        EditorGUILayout.Space();

                                        if(lighterSettings.flameType == Model_Type.Create){

                                            EditorGUILayout.Space();

                                            if(lighterSettings.flame != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].singleValues[0].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //flame != null
                                            } else {

                                                EditorGUILayout.PropertyField(lighterFlameParentRef, new GUIContent("Flame Parent"), true);

                                                if(lighterSettings.flameParent != null){

                                                    GUI.enabled = true;

                                                //flameParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//flameParent != null

                                                EditorGUILayout.PropertyField(lighterFlamePrefabRef, true);

                                                EditorGUILayout.Space();

                                                if(lighterSettings.flamePrefab != null){

                                                    GUI.enabled = true;

                                                //flamePrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//flamePrefab != null

                                            }//flame != null

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[10].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[1].buttons[1].local)){

                                                    LighterFlame_Create();

                                                }//DisplayDialog

                                            }//Buttton

                                        }//flameType = create

                                        if(lighterSettings.flameType == Model_Type.Local){

                                            EditorGUILayout.Space();

                                            EditorGUILayout.PropertyField(lighterFlame, true);
                                            EditorGUILayout.PropertyField(lighterFlamePosition, true);

                                        }//flameType = local

                                    }//lighterTabs = flame

                                    if(lighterAuto.lighterTabs == 2){

                                        EditorGUILayout.PropertyField(lighterLightTypeRef, true);

                                        EditorGUILayout.Space();

                                        if(lighterSettings.lightType == Model_Type.Create){

                                            if(lighterSettings.light != null){

                                                GUI.enabled = true;

                                                EditorGUILayout.Space();

                                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].singleValues[1].local, MessageType.Warning);

                                                EditorGUILayout.Space();

                                                GUI.enabled = false;

                                            //light != null
                                            } else {

                                                EditorGUILayout.PropertyField(lighterLightParentRef, true);

                                                if(lighterSettings.lightParent != null){

                                                    GUI.enabled = true;

                                                //lightParent != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightParent != null

                                                EditorGUILayout.PropertyField(lighterLightPrefabRef, true);

                                                if(lighterSettings.lightPrefab != null){

                                                    GUI.enabled = true;

                                                //lightPrefab != null
                                                } else {

                                                    GUI.enabled = false;

                                                }//lightPrefab != null

                                            }//light != null

                                            EditorGUILayout.Space();

                                            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].buttons[11].local)){

                                                if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[2].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[5].prompts[2].buttons[1].local)){

                                                    LighterLight_Create();

                                                }//DisplayDialog

                                            }//Buttton

                                        }//lightType = create

                                        if(lighterSettings.lightType == Model_Type.Local){

                                            EditorGUILayout.Space();

                                            EditorGUILayout.PropertyField(lighterLight, true);

                                        }//lightType = local

                                    }//lighterTabs = light

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//lighterOpts

                                if(!lighterAuto.audOpts && !lighterAuto.animOpts && !lighterAuto.invOpts && !lighterAuto.lighterOpts){

                                    EditorGUILayout.EndScrollView();

                                    bottomShow = true;

                                }//!audOpts, !animOpts, !invOpts & !lighterOpts

                            #else

                                bottomShow = false;

                                EditorGUILayout.HelpBox("COMPONENTS NOT ACTIVATED!", MessageType.Error);

                            #endif

                        }//weaponType = Lighter

                        if(weaponType == Weapon_Type.Melee){

                            UI_Hide(Weapon_Type.Candle);
                            UI_Hide(Weapon_Type.Flashlight);
                            UI_Hide(Weapon_Type.Lantern);
                            UI_Hide(Weapon_Type.Lighter);
                            UI_Hide(Weapon_Type.Gun);

                            if(!meleeAuto.audOpts && !meleeAuto.animOpts && !meleeAuto.invOpts && !meleeAuto.meleeOpts && !meleeAuto.weapOpts){

                                scrollPositions.melee_scrollPos = GUILayout.BeginScrollView(scrollPositions.melee_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!audOpts, !animOpts, !invOpts, !meleeOpts & !weapOpts

                            if(!meleeAuto.animOpts && !meleeAuto.invOpts && !meleeAuto.meleeOpts && !meleeAuto.weapOpts){

                                meleeAuto.audOpts = GUILayout.Toggle(meleeAuto.audOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[0].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !invOpts, !meleeOpts & !weapOpts

                            if(!meleeAuto.audOpts && !meleeAuto.invOpts && !meleeAuto.meleeOpts && !meleeAuto.weapOpts){

                                meleeAuto.animOpts = GUILayout.Toggle(meleeAuto.animOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[1].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!animOpts, !invOpts, !meleeOpts & !weapOpts

                            if(!meleeAuto.audOpts && !meleeAuto.animOpts && !meleeAuto.meleeOpts && !meleeAuto.weapOpts){

                                meleeAuto.invOpts = GUILayout.Toggle(meleeAuto.invOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[2].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !meleeOpts & !weapOpts

                            if(!meleeAuto.audOpts && !meleeAuto.animOpts && !meleeAuto.invOpts && !meleeAuto.weapOpts){

                                meleeAuto.meleeOpts = GUILayout.Toggle(meleeAuto.meleeOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[3].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !invOpts & !weapOpts

                            if(!meleeAuto.audOpts && !meleeAuto.animOpts && !meleeAuto.invOpts && !meleeAuto.meleeOpts){

                                meleeAuto.weapOpts = GUILayout.Toggle(meleeAuto.weapOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[4].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !invOpts & !meleeOpts

                            if(meleeAuto.audOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].texts[0].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.meleeAudOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeAudOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeDrawSound, true);
                                EditorGUILayout.PropertyField(meleeDrawVolume, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeHideSound, true);
                                EditorGUILayout.PropertyField(meleeHideVolume, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeSwaySound, true);
                                EditorGUILayout.PropertyField(meleeSwayVolume, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//audOpts

                            if(meleeAuto.animOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].texts[1].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.meleeAnimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeAnimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeDrawAnim, new GUIContent("Draw Animation"), true);
                                EditorGUILayout.PropertyField(meleeDrawSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeHideAnim, new GUIContent("Hide Animation"),true);
                                EditorGUILayout.PropertyField(meleeHideSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeAttackAnim, new GUIContent("Attack Animation"),true);
                                EditorGUILayout.PropertyField(meleeAttackSpeed, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//animOpts

                            if(meleeAuto.invOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].texts[2].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.meleeInvOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeInvOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(meleeItemID, true);

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//invOpts

                            if(meleeAuto.meleeOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].texts[3].text, MessageType.Info);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                meleeAuto.meleeTabs = GUILayout.SelectionGrid(meleeAuto.meleeTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[5].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[6].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[7].local }, 3);

                                EditorGUILayout.Space();

                                if(meleeAuto.meleeTabs == 0){

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.meleeMeleeOptsHit_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeMeleeOptsHit_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(meleeHitLayer, true);

                                    EditorGUILayout.Space();

                                    meleeSettings.HitDistance = EditorGUILayout.FloatField("Hit Distance", meleeSettings.HitDistance);
                                    meleeSettings.HitForce = EditorGUILayout.FloatField("Hit Force", meleeSettings.HitForce);
                                    meleeSettings.HitWaitDelay = EditorGUILayout.FloatField("Hit Wait Delay", meleeSettings.HitWaitDelay);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(meleeAttackDamage, true);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//meleeTabs = hit

                                if(meleeAuto.meleeTabs == 1){

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.meleeMeleeOptsKick_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeMeleeOptsKick_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(meleeSwayKickback, true);

                                    EditorGUILayout.Space();

                                    meleeSettings.SwaySpeed = EditorGUILayout.FloatField("Sway Speed", meleeSettings.SwaySpeed);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//meleeTabs = kickback

                                if(meleeAuto.meleeTabs == 2){

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.meleeMeleeOptsSurf_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeMeleeOptsSurf_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(meleeSurfaceID, true);
                                    EditorGUILayout.PropertyField(meleeSurfaceDetails, true);
                                    meleeSettings.defaultSurfaceID = EditorGUILayout.IntField("Default Surface ID", meleeSettings.defaultSurfaceID);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//meleeTabs = surface

                            }//meleeOpts

                            if(meleeAuto.weapOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].texts[4].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.meleeWeapOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.meleeWeapOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.PropertyField(meleeWeaponTypeRef, true);

                                EditorGUILayout.Space();

                                if(meleeSettings.weaponType == Model_Type.Create){

                                    EditorGUILayout.PropertyField(meleeWeaponParentRef, true);

                                    if(meleeSettings.weaponParent != null){

                                        GUI.enabled = true;

                                    //weaponParent != null
                                    } else {

                                        GUI.enabled = false;

                                    }//weaponParent != null

                                    EditorGUILayout.PropertyField(meleeWeaponPrefabRef, true);

                                    if(meleeSettings.weaponPrefab != null){

                                        GUI.enabled = true;

                                    //weaponPrefab != null
                                    } else {

                                        GUI.enabled = false;

                                    }//weaponPrefab != null

                                    EditorGUILayout.Space();

                                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].buttons[8].local)){

                                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[6].prompts[0].buttons[1].local)){

                                            MeleeWeapon_Create();

                                        }//DisplayDialog

                                    }//Buttton

                                }//weaponType = create

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//weapOpts

                            if(!meleeAuto.audOpts && !meleeAuto.animOpts && !meleeAuto.invOpts && !meleeAuto.meleeOpts && !meleeAuto.weapOpts){

                                EditorGUILayout.EndScrollView();

                                bottomShow = true;

                            }//!audOpts, !animOpts, !invOpts, !meleeOpts & !weapOpts

                        }//weaponType = Melee

                        if(weaponType == Weapon_Type.Gun){

                            UI_Hide(Weapon_Type.Candle);
                            UI_Hide(Weapon_Type.Flashlight);
                            UI_Hide(Weapon_Type.Lantern);
                            UI_Hide(Weapon_Type.Melee);

                            if(!gunAuto.audOpts && !gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                scrollPositions.gun_scrollPos = GUILayout.BeginScrollView(scrollPositions.gun_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                            }//!audOpts, !aimOpts, !animOpts, !invOpts, !shootOpts, !shellOpts & !weapOpts

                            if(!gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                gunAuto.audOpts = GUILayout.Toggle(gunAuto.audOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[0].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!aimOpts, !animOpts, !invOpts, !shootOpts, !shellOpts & !weapOpts

                            if(!gunAuto.audOpts && !gunAuto.animOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                gunAuto.aimOpts = GUILayout.Toggle(gunAuto.aimOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[1].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!audOpts, !animOpts, !invOpts, !shootOpts, !shellOpts & !weapOpts

                            if(!gunAuto.aimOpts && !gunAuto.audOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                gunAuto.animOpts = GUILayout.Toggle(gunAuto.animOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[2].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!aimOpts, !audOpts, !invOpts, !shootOpts, !shellOpts & !weapOpts

                            if(!gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.audOpts && !gunAuto.shootOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                gunAuto.invOpts = GUILayout.Toggle(gunAuto.invOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[3].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!aimOpts, !animOpts, !audOpts, !shootOpts, !shellOpts & !weapOpts

                            if(!gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.audOpts && !gunAuto.invOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                gunAuto.shootOpts = GUILayout.Toggle(gunAuto.shootOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[4].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!aimOpts, !animOpts, !audOpts, !invOpts, !shellOpts & !weapOpts

                            #if (HFPS_163a || HFPS_163b)

                                if(gunSettings.weaponType == WeaponController.WeaponType.Shotgun){

                                    if(!gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.audOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.weapOpts){

                                        gunAuto.shellOpts = GUILayout.Toggle(gunAuto.shellOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[5].local, GUI.skin.button);

                                        GUILayout.Space(5);

                                    }//!aimOpts, !animOpts, !audOpts, !invOpts, !shootOpts & !weapOpts

                                }//weaponType = shotgun

                            #endif

                            #if HFPS_163c

                                if(!gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.audOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.weapOpts){

                                    gunAuto.shellOpts = GUILayout.Toggle(gunAuto.shellOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[6].local, GUI.skin.button);

                                    GUILayout.Space(5);

                                }//!aimOpts, !animOpts, !audOpts, !invOpts, !shootOpts & !weapOpts

                            #endif

                            if(!gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.audOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.shellOpts){

                                gunAuto.weapOpts = GUILayout.Toggle(gunAuto.weapOpts, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[7].local, GUI.skin.button);

                                GUILayout.Space(5);

                            }//!aimOpts, !animOpts, !audOpts & !invOpts, !shootOpts & !shellOpts

                            if(gunAuto.audOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[0].text, MessageType.Info);

                                EditorGUILayout.Space();

                                //EditorGUILayout.PropertyField(gunAudioSource, true);
                                EditorGUILayout.PropertyField(gunReloadSound, new GUIContent("Reload Sound Init"), true);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.gunAudOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunAudOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunSoundDraw, new GUIContent("Draw Sound"), true);
                                EditorGUILayout.PropertyField(gunVolumeDraw, new GUIContent("Draw Volume"), true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunSoundFire, new GUIContent("Fire Sound"), true);
                                EditorGUILayout.PropertyField(gunVolumeFire, new GUIContent("Fire Volume"), true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunSoundEmpty, new GUIContent("Empty Sound"), true);
                                EditorGUILayout.PropertyField(gunVolumeEmpty, new GUIContent("Empty Volume"), true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunSoundReload, new GUIContent("Reload Sound"), true);
                                EditorGUILayout.PropertyField(gunVolumeReload, new GUIContent("Reload Volume"), true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunVolumeImpact, new GUIContent("Impact Volume"), true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//audOpts

                            if(gunAuto.aimOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[1].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.gunAimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunAimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                gunSettings.aimingSettings.enableAiming = EditorGUILayout.Toggle("Enable Aiming?", gunSettings.aimingSettings.enableAiming);

                                if(gunSettings.aimingSettings.enableAiming){

                                    gunSettings.aimingSettings.steadyAim = EditorGUILayout.Toggle("Steady Aim?", gunSettings.aimingSettings.steadyAim);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(gunAimPosition, true);

                                    EditorGUILayout.Space();

                                    gunSettings.aimingSettings.aimSpeed = EditorGUILayout.FloatField("Aim Speed", gunSettings.aimingSettings.aimSpeed);

                                    EditorGUILayout.Space();

                                    gunSettings.aimingSettings.zoomFOVSmooth = EditorGUILayout.FloatField("Zoom FOV Smooth", gunSettings.aimingSettings.zoomFOVSmooth);
                                    gunSettings.aimingSettings.unzoomFOVSmooth = EditorGUILayout.FloatField("Unzoom FOV Smooth", gunSettings.aimingSettings.unzoomFOVSmooth);
                                    gunSettings.aimingSettings.zoomFOV = EditorGUILayout.IntField("Zoom FOV", gunSettings.aimingSettings.zoomFOV);

                                }//enableAiming

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//aimOpts

                            if(gunAuto.animOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[2].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.gunAnimOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunAnimOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunHideAnim, true);
                                EditorGUILayout.PropertyField(gunFireAnim, true);
                                EditorGUILayout.PropertyField(gunReloadAnim, true);

                                EditorGUILayout.PropertyField(gunBeforeReloadAnim, true);
                                EditorGUILayout.PropertyField(gunAfterReloadAnim, true);
                                EditorGUILayout.PropertyField(gunAfterReloadEmptyAnim, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//animOpts

                            if(gunAuto.invOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[3].text, MessageType.Info);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.gunInvOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunInvOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunWeaponID, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(gunBulletsID, true);

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//invOpts

                            if(gunAuto.shootOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[4].text, MessageType.Info);

                                EditorGUILayout.Space();

                                gunAuto.shootTabs = GUILayout.SelectionGrid(gunAuto.shootTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[8].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[9].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[10].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[11].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[12].local }, 3);

                                if(gunAuto.shootTabs == 0){

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    gunAuto.bulletTabs = GUILayout.SelectionGrid(gunAuto.bulletTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[13].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[14].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[15].local }, 3);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.gunShootBulletOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunShootBulletOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    if(gunAuto.bulletTabs == 0){

                                        GUILayout.Space(15);

                                        EditorGUILayout.PropertyField(gunBulletPrefab, true);
                                        EditorGUILayout.PropertyField(gunBarrelEndPosition, true);

                                        EditorGUILayout.Space();

                                        gunSettings.bulletModelSettings.bulletForce = EditorGUILayout.FloatField("Bullet Force", gunSettings.bulletModelSettings.bulletForce);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(gunBulletRotation, true);

                                    }//bulletTabs = model

                                    if(gunAuto.bulletTabs == 1){

                                        GUILayout.Space(15);

                                        gunSettings.bulletSettings.keepReloadMagBullets = EditorGUILayout.Toggle("Keep Reload Mag Bullets?", gunSettings.bulletSettings.keepReloadMagBullets);

                                        EditorGUILayout.Space();

                                        gunSettings.bulletSettings.bulletsInMag = EditorGUILayout.IntField("Bullets In Mag", gunSettings.bulletSettings.bulletsInMag);
                                        gunSettings.bulletSettings.bulletsPerMag = EditorGUILayout.IntField("Bullets Per Mag", gunSettings.bulletSettings.bulletsPerMag);
                                        gunSettings.bulletSettings.bulletsPerShot = EditorGUILayout.IntField("Bullets Per Shot", gunSettings.bulletSettings.bulletsPerShot);

                                    }//bulletTabs = magazine

                                    if(gunAuto.bulletTabs == 2){

                                        GUILayout.Space(15);

                                        EditorGUILayout.PropertyField(gunSurfaceID, true);
                                        EditorGUILayout.PropertyField(gunSurfaceDetails, true);

                                        gunSettings.bulletSettings.defaultSurfaceID = EditorGUILayout.IntField("Default Surface ID", gunSettings.bulletSettings.defaultSurfaceID);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(gunFleshTag, true);
                                        gunSettings.bulletSettings.soundOnImpact = EditorGUILayout.Toggle("Sound On Impact?", gunSettings.bulletSettings.soundOnImpact);

                                    }//bulletTabs = surface

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//shootTabs = bullet

                                if(gunAuto.shootTabs == 1){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(gunKickbackRef, true);

                                    EditorGUILayout.Space();

                                    if(gunSettings.kickback != null){

                                        GUI.enabled = false;

                                    //kickback != null
                                    } else {

                                        GUI.enabled = true;

                                    }//kickback != null

                                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[16].local)){

                                        Catch_Kickback();

                                    }//Buttton

                                    GUI.enabled = true;

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.gunShootKickOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunShootKickOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    gunSettings.kickbackSettings.kickUp = EditorGUILayout.FloatField("Kick Up", gunSettings.kickbackSettings.kickUp);
                                    gunSettings.kickbackSettings.kickSideways = EditorGUILayout.FloatField("Kick Sideways", gunSettings.kickbackSettings.kickSideways);
                                    gunSettings.kickbackSettings.kickTime = EditorGUILayout.FloatField("Kick Time", gunSettings.kickbackSettings.kickTime);
                                    gunSettings.kickbackSettings.kickReturnSpeed = EditorGUILayout.FloatField("Kick Return Speed", gunSettings.kickbackSettings.kickReturnSpeed);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//shootTabs = kickback

                                if(gunAuto.shootTabs == 2){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.gunShootMuzzOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunShootMuzzOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    gunSettings.muzzleFlashSettings.enableMuzzleFlash = EditorGUILayout.Toggle("Enable Muzzle Flash?", gunSettings.muzzleFlashSettings.enableMuzzleFlash);

                                    if(gunSettings.muzzleFlashSettings.enableMuzzleFlash){

                                        EditorGUILayout.Space();

                                        gunAuto.shootMuzzleTabs = GUILayout.SelectionGrid(gunAuto.shootMuzzleTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[17].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[18].local }, 2);

                                        EditorGUILayout.Space();

                                        if(gunAuto.shootMuzzleTabs == 0){

                                            EditorGUILayout.PropertyField(gunMuzzleFlashTypeRef, true);

                                            EditorGUILayout.Space();

                                            if(gunSettings.muzzleFlashType == Model_Type.Create){

                                                if(gunSettings.muzzleFlashSettings.muzzleFlash != null){

                                                    GUI.enabled = true;

                                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].singleValues[0].local, MessageType.Warning);

                                                    EditorGUILayout.Space();

                                                    GUI.enabled = false;

                                                //muzzleFlash != null
                                                } else {

                                                    EditorGUILayout.PropertyField(gunMuzzleFlashParentRef, true);

                                                    if(gunSettings.muzzleFlashParent != null){

                                                        GUI.enabled = true;

                                                    //muzzleFlashParent != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//muzzleFlashParent != null

                                                    EditorGUILayout.PropertyField(gunMuzzleFlashPrefabRef, true);

                                                    EditorGUILayout.Space();

                                                    if(gunSettings.muzzleFlashPrefab != null){

                                                        GUI.enabled = true;

                                                    //muzzleFlashPrefab != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//muzzleFlashPrefab != null

                                                }//muzzleFlash != null

                                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[19].local)){

                                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[0].buttons[1].local)){

                                                        GunMuzzleFlash_Create();

                                                    }//DisplayDialog

                                                }//Buttton

                                            }//muzzleFlashType = create

                                            if(gunSettings.muzzleFlashType == Model_Type.Local){

                                                EditorGUILayout.PropertyField(gunMuzzleFlashRef, true);
                                                EditorGUILayout.PropertyField(gunMuzzleRotation, true);

                                            }//muzzleFlashType = local

                                        }//shootMuzzleTabs = flash

                                        if(gunAuto.shootMuzzleTabs == 1){

                                            EditorGUILayout.PropertyField(gunMuzzleLightTypeRef, true);

                                            EditorGUILayout.Space();

                                            if(gunSettings.muzzleLightType == Model_Type.Create){

                                                if(gunSettings.muzzleFlashSettings.muzzleLight != null){

                                                    GUI.enabled = true;

                                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].singleValues[1].local, MessageType.Warning);

                                                    EditorGUILayout.Space();

                                                    GUI.enabled = false;

                                                //muzzleLight != null
                                                } else {

                                                    EditorGUILayout.PropertyField(gunMuzzleLightParentRef, true);

                                                    if(gunSettings.muzzleLightParent != null){

                                                        GUI.enabled = true;

                                                    //muzzleLightParent != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//muzzleLightParent != null

                                                    EditorGUILayout.PropertyField(gunMuzzleLightPrefabRef, true);

                                                    EditorGUILayout.Space();

                                                    if(gunSettings.muzzleLightPrefab != null){

                                                        GUI.enabled = true;

                                                    //muzzleLightPrefab != null
                                                    } else {

                                                        GUI.enabled = false;

                                                    }//muzzleLightPrefab != null

                                                }//muzzleLight != null

                                                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[20].local)){

                                                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[1].buttons[1].local)){

                                                        GunMuzzleLight_Create();

                                                    }//DisplayDialog

                                                }//Buttton

                                            }//muzzleLightType = create

                                            if(gunSettings.muzzleLightType == Model_Type.Local){

                                                EditorGUILayout.PropertyField(gunMuzzleLightRef, true);

                                            }//muzzleLightType = local

                                        }//shootMuzzleTabs = light

                                    }//enableMuzzleFlash

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//shootTabs = muzzle flash

                                if(gunAuto.shootTabs == 3){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.gunShootShootOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunShootShootOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    gunSettings.weaponSettings.weaponDamage = EditorGUILayout.IntField("Weapon Damage", gunSettings.weaponSettings.weaponDamage);

                                    EditorGUILayout.Space();

                                    gunSettings.weaponSettings.shootRange = EditorGUILayout.FloatField("Shoot Range", gunSettings.weaponSettings.shootRange);
                                    gunSettings.weaponSettings.hitforce = EditorGUILayout.FloatField("Hit Force", gunSettings.weaponSettings.hitforce);
                                    gunSettings.weaponSettings.fireRate = EditorGUILayout.FloatField("Fire Rate", gunSettings.weaponSettings.fireRate);
                                    gunSettings.weaponSettings.recoil = EditorGUILayout.FloatField("Recoil", gunSettings.weaponSettings.recoil);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//shootTabs = shoot

                                if(gunAuto.shootTabs == 4){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                    scrollPositions.gunShootReactOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunShootReactOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                    EditorGUILayout.Space();

                                    gunSettings.npcReactionSettings.enableSoundReaction = EditorGUILayout.Toggle("Enable Sound Reaction?", gunSettings.npcReactionSettings.enableSoundReaction);

                                    if(gunSettings.npcReactionSettings.enableSoundReaction){

                                        gunSettings.npcReactionSettings.soundReactionRadius = EditorGUILayout.FloatField("Sound Reaction Radius", gunSettings.npcReactionSettings.soundReactionRadius);

                                    }//enableSoundReaction

                                    EditorGUILayout.Space();

                                    EditorGUILayout.EndScrollView();

                                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    EditorGUILayout.EndVertical();

                                }//shootTabs = reaction

                            }//shootOpts

                            if(gunAuto.shellOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                #if (HFPS_163a || HFPS_163b)

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[5].text, MessageType.Info);

                                #endif

                                #if HFPS_163c

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[6].text, MessageType.Info);

                                #endif

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.gunShellOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunShellOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                #if (HFPS_163a || HFPS_163b)

                                    EditorGUILayout.PropertyField(gunEjectPosition, true);
                                    gunSettings.shotgunSettings.ejectSpeed = EditorGUILayout.FloatField("Eject Speed", gunSettings.shotgunSettings.ejectSpeed);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(gunShellPrefab, true);
                                    EditorGUILayout.PropertyField(gunShellRotation, true);

                                #endif

                                #if HFPS_163c

                                    gunSettings.bulletSettings.ejectShells = EditorGUILayout.Toggle("Eject Shells?", gunSettings.bulletSettings.ejectShells);

                                    if(gunSettings.bulletSettings.ejectShells){

                                        gunSettings.shellEjectSettings.ejectAutomatiacally = EditorGUILayout.Toggle("Eject Automatically?", gunSettings.shellEjectSettings.ejectAutomatiacally);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(gunEjectPosition, true);
                                        gunSettings.shellEjectSettings.ejectSpeed = EditorGUILayout.FloatField("Eject Speed", gunSettings.shellEjectSettings.ejectSpeed);

                                        EditorGUILayout.Space();

                                        EditorGUILayout.PropertyField(gunShellPrefab, true);
                                        EditorGUILayout.PropertyField(gunShellRotation, true);

                                    }//ejectShells

                                #endif

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//shellOpts

                            if(gunAuto.weapOpts){

                                bottomShow = false;

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].texts[7].text, MessageType.Info);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                gunAuto.weapTabs = GUILayout.SelectionGrid(gunAuto.weapTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[13].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[21].local }, 2);

                                EditorGUILayout.Space();

                                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                                scrollPositions.gunWeapOpts_scrollPos = GUILayout.BeginScrollView(scrollPositions.gunWeapOpts_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.Space();

                                if(gunAuto.weapTabs == 0){

                                    EditorGUILayout.PropertyField(gunWeapModelTypeRef, new GUIContent("Weapon Model Type"), true);

                                    EditorGUILayout.Space();

                                    if(gunSettings.weapModelType == Model_Type.Create){

                                        EditorGUILayout.PropertyField(gunWeaponParentRef, true);

                                        if(gunSettings.weaponParent != null){

                                            GUI.enabled = true;

                                        //weaponParent != null
                                        } else {

                                            GUI.enabled = false;

                                        }//weaponParent != null

                                        EditorGUILayout.PropertyField(gunWeaponPrefabRef, true);

                                        if(gunSettings.weaponPrefab != null){

                                            GUI.enabled = true;

                                        //weaponPrefab != null
                                        } else {

                                            GUI.enabled = false;

                                        }//weaponPrefab != null

                                        EditorGUILayout.Space();

                                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].buttons[22].local)){

                                            if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[2].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[7].prompts[2].buttons[1].local)){

                                                GunWeapon_Create();

                                            }//DisplayDialog

                                        }//Buttton

                                    }//weapModelType = create

                                }//weapTabs = model

                                if(gunAuto.weapTabs == 1){

                                    EditorGUILayout.PropertyField(gunWeaponType, true);
                                    EditorGUILayout.PropertyField(gunBulletType, true);

                                    EditorGUILayout.Space();

                                    EditorGUILayout.PropertyField(gunRaycastMask, true);
                                    EditorGUILayout.PropertyField(gunSoundReactionMask, true);

                                }//weapTabs = type

                                EditorGUILayout.Space();

                                EditorGUILayout.EndScrollView();

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                EditorGUILayout.EndVertical();

                            }//weapOpts

                            if(!gunAuto.audOpts && !gunAuto.aimOpts && !gunAuto.animOpts && !gunAuto.invOpts && !gunAuto.shootOpts && !gunAuto.shellOpts && !gunAuto.weapOpts){

                                EditorGUILayout.EndScrollView();

                                bottomShow = true;

                            }//!audOpts, !aimOpts, !animOpts, !invOpts, !shootOpts, !shellOpts & !weapOpts

                        }//weaponType = Gun

                    }//displayShow

                    if(bottomShow){

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        if(template != null){

                            GUI.enabled = true;

                        //template != null
                        } else {

                            GUI.enabled = false;

                        }//template != null

                        EditorGUILayout.BeginHorizontal();

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                            if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[1].local)){

                                Template_Save();

                            }//DisplayDialog

                        }//Buttton

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)){

                            if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[1].local)){

                                Template_Load();

                            }//DisplayDialog

                        }//Buttton

                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5);

                        GUI.enabled = true;

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[5].local)){

                            if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[3].buttons[1].local)){

                                Weapon_Create();

                            }//DisplayDialog

                        }//Buttton

                        EditorGUILayout.Space();

                    }//bottomShow

                    if(!displayShow){

                        scrollPositions.arms_scrollPos = GUILayout.BeginScrollView(scrollPositions.arms_scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        if(weaponName != ""){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local + weaponName, MessageType.Info);

                        //weaponName != null
                        } else {

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local + weaponName, MessageType.Warning);

                        }//weaponName != null

                        EditorGUILayout.Space();

                        if(armsPrefab != null){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local + armsPrefab.name, MessageType.Info);

                        //armsPrefab != null
                        } else {

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local , MessageType.Warning);

                        }//armsPrefab != null

                        EditorGUILayout.Space();

                        if(itemSwitcher != null){

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local, MessageType.Info);

                        //itemSwitcher != null
                        } else {

                            EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local, MessageType.Warning);

                        }//itemSwitcher != null

                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        if(itemSwitcher != null){

                            GUI.enabled = false;

                        //itemSwitcher != null
                        } else {

                            GUI.enabled = true;

                        }//itemSwitcher != null

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                            ItemSwitcher_Catch();

                        }//Buttton

                        GUILayout.Space(5);

                        EditorGUILayout.BeginHorizontal();

                        GUI.enabled = false;

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                            //NOTHING

                        }//Buttton

                        if(template != null){

                            GUI.enabled = true;

                        //template != null
                        } else {

                            GUI.enabled = false;

                        }//template != null

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)){

                            if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[1].buttons[1].local)){

                                Template_Load();

                            }//DisplayDialog

                        }//Buttton

                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5);

                        if(itemSwitcher == null){

                            GUI.enabled = false;

                        //itemSwitcher == null
                        } else {

                            if(armsPrefab != null){

                                if(weaponName != ""){

                                    GUI.enabled = true;

                                //weaponName != null
                                } else {

                                    GUI.enabled = false;    

                                }//weaponName != null

                            //armsPrefab != null
                            } else {

                                GUI.enabled = false;

                            }//armsPrefab != null

                        }//itemSwitcher == null

                        if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[4].local)){

                            if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[2].buttons[1].local)){

                                Arms_Create();

                            }//DisplayDialog

                        }//Buttton

                        EditorGUILayout.Space();

                    }//!displayShow

                }//verNumb == "Unknown"

            //dmMenusLocData != null 
            } else {

                if(!languageLock){

                    DM_LocDataFind();

                }//!languageLock 

            }//dmMenusLocData != null    

            if(EditorGUI.EndChangeCheck()){

                soTar.ApplyModifiedProperties();

            }//EndChangeCheck

        }//WeaponCreator_Screen


    //////////////////////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    //////////////////////////////////////////////////////

    //////////////////////////////////
    ///
    ///     UI
    ///
    //////////////////////////////////


        private void UI_Hide(Weapon_Type type){

            if(type == Weapon_Type.Candle){

                candleAuto.animOpts = false;
                candleAuto.audOpts = false;
                candleAuto.candleOpts = false;
                candleAuto.invOpts = false;

                candleAuto.candleTabs = 0;
                candleAuto.candleModelTabs = 0;

            }//type = candle

            if(type == Weapon_Type.Flashlight){

                flashlightAuto.audOpts = false;
                flashlightAuto.animOpts = false;
                flashlightAuto.flashOpts = false;
                flashlightAuto.invOpts = false;
                flashlightAuto.uiOpts = false;

                flashlightAuto.animTabs = 0;
                flashlightAuto.flashTabs = 0;
                flashlightAuto.flashModelTabs = 0;

            }//type = flashlight

            if(type == Weapon_Type.Lantern){

                lanternAuto.audOpts = false;
                lanternAuto.animOpts = false;
                lanternAuto.invOpts = false;
                lanternAuto.lantOpts = false;
                lanternAuto.uiOpts = false;

                lanternAuto.lanternTabs = 0;
                lanternAuto.lanternModelTabs = 0;
                lanternAuto.lanternHingeTabs = 0;

            }//type = lantern

            if(type == Weapon_Type.Lighter){

                lighterAuto.audOpts = false;
                lighterAuto.animOpts = false;
                lighterAuto.invOpts = false;
                lighterAuto.lighterOpts = false;

                lighterAuto.animTabs = 0;
                lighterAuto.lighterTabs = 0;

            }//type = lighter

            if(type == Weapon_Type.Melee){

                meleeAuto.audOpts = false;
                meleeAuto.animOpts = false;
                meleeAuto.invOpts = false;
                meleeAuto.meleeOpts = false;
                meleeAuto.weapOpts = false;

                meleeAuto.meleeTabs = 0;

            }//type = melee

            if(type == Weapon_Type.Gun){

                gunAuto.audOpts = false;
                gunAuto.aimOpts = false;
                gunAuto.animOpts = false;
                gunAuto.invOpts = false;
                gunAuto.shootOpts = false;
                gunAuto.shellOpts = false;
                gunAuto.weapOpts = false;

                gunAuto.shootTabs = 0;
                gunAuto.bulletTabs = 0;

            }//type = gun

        }//UI_Hide


    //////////////////////////////////
    ///
    ///     TEMPLATE ACTIONS
    ///
    //////////////////////////////////

    /////////////////////////
    ///
    ///     SAVE
    ///
    /////////////////////////


        private void Template_Save(){

            if(template != null){

                template.weaponName = weaponName;
                template.weaponType = weaponType;
                template.armsPrefab = armsPrefab;

                if(weaponType == Weapon_Type.Candle){

                    template.candleSettings.BlowOut = candleSettings.BlowOut;

                    template.candleSettings.IdleAnimation = candleSettings.IdleAnimation;
                    template.candleSettings.BlowOutAnimation = candleSettings.BlowOutAnimation;

                    template.candleSettings.DrawAnimation = candleSettings.DrawAnimation;
                    template.candleSettings.DrawSpeed = candleSettings.DrawSpeed;

                    template.candleSettings.HideAnimation = candleSettings.HideAnimation;
                    template.candleSettings.HideSpeed = candleSettings.HideSpeed;

                    template.candleSettings.candleReduction = candleSettings.candleReduction;
                    template.candleSettings.reductionRate = candleSettings.reductionRate;
                    template.candleSettings.maxScale = candleSettings.maxScale;
                    template.candleSettings.minScale = candleSettings.minScale;

                    template.candleSettings.itemID = candleSettings.itemID;
                    template.candleSettings.blowOutKeepCandle = candleSettings.blowOutKeepCandle;
                    template.candleSettings.scaleKeepCandle = candleSettings.scaleKeepCandle;

                    template.candleSettings.candleType = candleSettings.candleType;

                    if(candleSettings.candlePrefab != null){

                        template.candleSettings.candlePrefab = candleSettings.candlePrefab;

                    }//candlePrefab != null

                    template.candleSettings.flameType = candleSettings.flameType;

                    if(candleSettings.flamePrefab != null){

                        template.candleSettings.flamePrefab = candleSettings.flamePrefab;

                    }//flamePrefab != null

                    template.candleSettings.lightType = candleSettings.lightType;

                    if(candleSettings.lightPrefab != null){

                        template.candleSettings.lightPrefab = candleSettings.lightPrefab;

                    }//lightPrefab != null

                }//weaponType = candle

                if(weaponType == Weapon_Type.Flashlight){

                    template.flashlightSettings.clickSound = flashlightSettings.clickSound;

                    template.flashlightSettings.IdleAnim = flashlightSettings.IdleAnim;

                    template.flashlightSettings.DrawAnim = flashlightSettings.DrawAnim;
                    template.flashlightSettings.DrawSpeed = flashlightSettings.DrawSpeed;

                    template.flashlightSettings.HideAnim = flashlightSettings.HideAnim;
                    template.flashlightSettings.HideSpeed = flashlightSettings.HideSpeed;

                    template.flashlightSettings.ReloadAnim = flashlightSettings.ReloadAnim;
                    template.flashlightSettings.ReloadSpeed = flashlightSettings.ReloadSpeed;

                    template.flashlightSettings.enableExtra = flashlightSettings.enableExtra;

                    template.flashlightSettings.ScareAnim = flashlightSettings.ScareAnim;
                    template.flashlightSettings.ScareAnimSpeed = flashlightSettings.ScareAnimSpeed;

                    template.flashlightSettings.NoPowerAnim = flashlightSettings.NoPowerAnim;
                    template.flashlightSettings.NoPowerAnimSpeed = flashlightSettings.NoPowerAnimSpeed;

                    template.flashlightSettings.infiniteBattery = flashlightSettings.infiniteBattery;
                    template.flashlightSettings.batteryLifeInSec = flashlightSettings.batteryLifeInSec;
                    template.flashlightSettings.canReloadPercent = flashlightSettings.canReloadPercent;

                    template.flashlightSettings.itemID = flashlightSettings.itemID;
                    template.flashlightSettings.flashlightIntensity = flashlightSettings.flashlightIntensity;
                    template.flashlightSettings.FlashlightIcon = flashlightSettings.FlashlightIcon;

                    template.flashlightSettings.flashLightType = flashlightSettings.flashLightType;

                    if(flashlightSettings.flashlightPrefab != null){

                        template.flashlightSettings.flashlightPrefab = flashlightSettings.flashlightPrefab;

                    }//flashlightPrefab != null

                    template.flashlightSettings.lightType = flashlightSettings.lightType;

                    if(flashlightSettings.lightPrefab != null){

                        template.flashlightSettings.lightPrefab = flashlightSettings.lightPrefab;

                    }//lightPrefab != null

                }//weaponType = Flashlight

                if(weaponType == Weapon_Type.Lantern){

                    template.lanternSettings.ShowSound = lanternSettings.ShowSound;
                    template.lanternSettings.ShowVolume = lanternSettings.ShowVolume;

                    template.lanternSettings.HideSound = lanternSettings.HideSound;
                    template.lanternSettings.HideVolume = lanternSettings.HideVolume;

                    template.lanternSettings.ReloadOilSound = lanternSettings.ReloadOilSound;
                    template.lanternSettings.ReloadVolume = lanternSettings.ReloadVolume;

                    template.lanternSettings.DrawAnim = lanternSettings.DrawAnim;
                    template.lanternSettings.DrawSpeed = lanternSettings.DrawSpeed;

                    template.lanternSettings.HideAnim = lanternSettings.HideAnim;
                    template.lanternSettings.HideSpeed = lanternSettings.HideSpeed;

                    template.lanternSettings.ReloadAnim = lanternSettings.ReloadAnim;
                    template.lanternSettings.ReloadSpeed = lanternSettings.ReloadSpeed;

                    template.lanternSettings.IdleAnim = lanternSettings.IdleAnim;

                    template.lanternSettings.itemID = lanternSettings.itemID;

                    template.lanternSettings.useHingeJoint = lanternSettings.useHingeJoint;
                    template.lanternSettings.secondDrawDiff = lanternSettings.secondDrawDiff;

                    template.lanternSettings.oilLifeInSec = lanternSettings.oilLifeInSec;
                    template.lanternSettings.oilPercentage = lanternSettings.oilPercentage;
                    template.lanternSettings.lightReductionRate = lanternSettings.lightReductionRate;
                    template.lanternSettings.canReloadPercent = lanternSettings.canReloadPercent;
                    template.lanternSettings.hideIntensitySpeed = lanternSettings.hideIntensitySpeed;
                    template.lanternSettings.oilReloadSpeed = lanternSettings.oilReloadSpeed;
                    template.lanternSettings.timeWaitToReload = lanternSettings.timeWaitToReload;

                    template.lanternSettings.ColorString = lanternSettings.ColorString;
                    template.lanternSettings.LanternIcon = lanternSettings.LanternIcon;

                    template.lanternSettings.lanternType = lanternSettings.lanternType;
                    template.lanternSettings.lanternPrefab = lanternSettings.lanternPrefab;

                    template.lanternSettings.flameType = lanternSettings.flameType;
                    template.lanternSettings.flamePrefab = lanternSettings.flamePrefab;

                    template.lanternSettings.lightType = lanternSettings.lightType;
                    template.lanternSettings.lightPrefab = lanternSettings.lightPrefab;

                    template.lanternSettings.hingeType = lanternSettings.hingeType;
                    template.lanternSettings.hingeConType = lanternSettings.hingeConType;
                    template.lanternSettings.hingeConPrefab = lanternSettings.hingeConPrefab;

                }//weaponType = Lantern

                if(weaponType == Weapon_Type.Lighter){

                    #if COMPONENTS_PRESENT

                        template.lighterSettings.soundLibrary = lighterSettings.soundLibrary;

                        template.lighterSettings.IdleAnimation = lighterSettings.IdleAnimation;

                        template.lighterSettings.DrawAnimation = lighterSettings.DrawAnimation;
                        template.lighterSettings.DrawSpeed = lighterSettings.DrawSpeed;

                        template.lighterSettings.HideAnimation = lighterSettings.HideAnimation;
                        template.lighterSettings.HideSpeed = lighterSettings.HideSpeed;

                        template.lighterSettings.lighterAnimation = lighterSettings.lighterAnimation;
                        template.lighterSettings.lighterOpenAnimation = lighterSettings.lighterOpenAnimation;
                        template.lighterSettings.lighterCloseAnimation = lighterSettings.lighterCloseAnimation;

                        template.lighterSettings.itemID = lighterSettings.itemID;

                        template.lighterSettings.lighterType = lighterSettings.lighterType;
                        template.lighterSettings.lighterPrefab = lighterSettings.lighterPrefab;

                        template.lighterSettings.flameType = lighterSettings.flameType;
                        template.lighterSettings.flamePrefab = lighterSettings.flamePrefab;

                        template.lighterSettings.lightType = lighterSettings.lightType;
                        template.lighterSettings.lightPrefab = lighterSettings.lightPrefab;

                    #endif

                }//weaponType = lighter

                if(weaponType == Weapon_Type.Melee){

                    template.meleeSettings.DrawSound = meleeSettings.DrawSound;
                    template.meleeSettings.DrawVolume = meleeSettings.DrawVolume;

                    template.meleeSettings.HideSound = meleeSettings.HideSound;
                    template.meleeSettings.HideVolume = meleeSettings.HideVolume;

                    template.meleeSettings.SwaySound = meleeSettings.SwaySound;
                    template.meleeSettings.SwayVolume = meleeSettings.SwayVolume;

                    template.meleeSettings.DrawAnim = meleeSettings.DrawAnim;
                    template.meleeSettings.DrawSpeed = meleeSettings.DrawSpeed;

                    template.meleeSettings.HideAnim = meleeSettings.HideAnim;
                    template.meleeSettings.HideSpeed = meleeSettings.HideSpeed;

                    template.meleeSettings.AttackAnim = meleeSettings.AttackAnim;
                    template.meleeSettings.AttackSpeed = meleeSettings.AttackSpeed;

                    template.meleeSettings.itemID = meleeSettings.itemID;

                    template.meleeSettings.surfaceID = meleeSettings.surfaceID;
                    template.meleeSettings.surfaceDetails = meleeSettings.surfaceDetails;
                    template.meleeSettings.defaultSurfaceID = meleeSettings.defaultSurfaceID;

                    template.meleeSettings.HitLayer = meleeSettings.HitLayer;
                    template.meleeSettings.HitDistance = meleeSettings.HitDistance;
                    template.meleeSettings.HitForce = meleeSettings.HitForce;
                    template.meleeSettings.HitWaitDelay = meleeSettings.HitWaitDelay;
                    template.meleeSettings.AttackDamage = meleeSettings.AttackDamage;

                    template.meleeSettings.SwayKickback = meleeSettings.SwayKickback;
                    template.meleeSettings.SwaySpeed = meleeSettings.SwaySpeed;

                    template.meleeSettings.weaponType = meleeSettings.weaponType;
                    template.meleeSettings.weaponPrefab = meleeSettings.weaponPrefab;

                }//weaponType = Melee

                if(weaponType == Weapon_Type.Gun){

                    template.gunSettings.audioSettings.reloadSound = gunSettings.audioSettings.reloadSound;

                    template.gunSettings.audioSettings.soundDraw = gunSettings.audioSettings.soundDraw;
                    template.gunSettings.audioSettings.volumeDraw = gunSettings.audioSettings.volumeDraw;

                    template.gunSettings.audioSettings.soundFire = gunSettings.audioSettings.soundFire;
                    template.gunSettings.audioSettings.volumeFire = gunSettings.audioSettings.volumeFire;

                    template.gunSettings.audioSettings.soundEmpty = gunSettings.audioSettings.soundEmpty;
                    template.gunSettings.audioSettings.volumeEmpty = gunSettings.audioSettings.volumeEmpty;

                    template.gunSettings.audioSettings.soundReload = gunSettings.audioSettings.soundReload;
                    template.gunSettings.audioSettings.volumeReload = gunSettings.audioSettings.volumeReload;

                    template.gunSettings.audioSettings.impactVolume = gunSettings.audioSettings.impactVolume;

                    template.gunSettings.aimingSettings.enableAiming = gunSettings.aimingSettings.enableAiming;
                    template.gunSettings.aimingSettings.steadyAim = gunSettings.aimingSettings.steadyAim;
                    template.gunSettings.aimingSettings.aimPosition = gunSettings.aimingSettings.aimPosition;

                    template.gunSettings.aimingSettings.aimSpeed = gunSettings.aimingSettings.aimSpeed;

                    template.gunSettings.aimingSettings.zoomFOVSmooth = gunSettings.aimingSettings.zoomFOVSmooth;
                    template.gunSettings.aimingSettings.unzoomFOVSmooth = gunSettings.aimingSettings.unzoomFOVSmooth;
                    template.gunSettings.aimingSettings.zoomFOV = gunSettings.aimingSettings.zoomFOV;

                    template.gunSettings.animationSettings.hideAnim = gunSettings.animationSettings.hideAnim;
                    template.gunSettings.animationSettings.fireAnim = gunSettings.animationSettings.fireAnim;
                    template.gunSettings.animationSettings.reloadAnim = gunSettings.animationSettings.reloadAnim;

                    template.gunSettings.animationSettings.beforeReloadAnim = gunSettings.animationSettings.beforeReloadAnim;
                    template.gunSettings.animationSettings.afterReloadAnim = gunSettings.animationSettings.afterReloadAnim;
                    template.gunSettings.animationSettings.afterReloadEmptyAnim = gunSettings.animationSettings.afterReloadEmptyAnim;

                    template.gunSettings.inventorySettings.weaponID = gunSettings.inventorySettings.weaponID;
                    template.gunSettings.inventorySettings.bulletsID = gunSettings.inventorySettings.bulletsID;

                    template.gunSettings.bulletSettings.FleshTag = gunSettings.bulletSettings.FleshTag;

                    template.gunSettings.bulletModelSettings.bulletPrefab = gunSettings.bulletModelSettings.bulletPrefab;
                    template.gunSettings.bulletModelSettings.bulletRotation = gunSettings.bulletModelSettings.bulletRotation;
                    template.gunSettings.bulletModelSettings.bulletForce = gunSettings.bulletModelSettings.bulletForce;

                    template.gunSettings.bulletSettings.bulletsInMag = gunSettings.bulletSettings.bulletsInMag;
                    template.gunSettings.bulletSettings.bulletsPerMag = gunSettings.bulletSettings.bulletsPerMag;
                    template.gunSettings.bulletSettings.bulletsPerShot = gunSettings.bulletSettings.bulletsPerShot;

                    template.gunSettings.bulletSettings.keepReloadMagBullets = gunSettings.bulletSettings.keepReloadMagBullets;
                    template.gunSettings.bulletSettings.soundOnImpact = gunSettings.bulletSettings.soundOnImpact;

                    template.gunSettings.bulletSettings.surfaceID = gunSettings.bulletSettings.surfaceID;
                    template.gunSettings.surfaceDetails = gunSettings.surfaceDetails;
                    template.gunSettings.bulletSettings.defaultSurfaceID = gunSettings.bulletSettings.defaultSurfaceID;

                    template.gunSettings.kickbackSettings.kickUp = gunSettings.kickbackSettings.kickUp;
                    template.gunSettings.kickbackSettings.kickSideways = gunSettings.kickbackSettings.kickSideways;
                    template.gunSettings.kickbackSettings.kickTime = gunSettings.kickbackSettings.kickTime;
                    template.gunSettings.kickbackSettings.kickReturnSpeed = gunSettings.kickbackSettings.kickReturnSpeed;

                    template.gunSettings.muzzleFlashSettings.enableMuzzleFlash = gunSettings.muzzleFlashSettings.enableMuzzleFlash;
                    template.gunSettings.muzzleFlashSettings.muzzleRotation = gunSettings.muzzleFlashSettings.muzzleRotation;

                    template.gunSettings.weaponSettings.weaponDamage = gunSettings.weaponSettings.weaponDamage;
                    template.gunSettings.weaponSettings.shootRange = gunSettings.weaponSettings.shootRange;
                    template.gunSettings.weaponSettings.hitforce = gunSettings.weaponSettings.hitforce;
                    template.gunSettings.weaponSettings.fireRate = gunSettings.weaponSettings.fireRate;
                    template.gunSettings.weaponSettings.recoil = gunSettings.weaponSettings.recoil;

                    template.gunSettings.npcReactionSettings.enableSoundReaction = gunSettings.npcReactionSettings.enableSoundReaction;
                    template.gunSettings.npcReactionSettings.soundReactionRadius = gunSettings.npcReactionSettings.soundReactionRadius;

                    template.gunSettings.weaponType = gunSettings.weaponType;
                    template.gunSettings.bulletType = gunSettings.bulletType;
                    template.gunSettings.raycastMask = gunSettings.raycastMask;
                    template.gunSettings.soundReactionMask = gunSettings.soundReactionMask;

                    template.gunSettings.weapModelType = gunSettings.weapModelType;
                    template.gunSettings.weaponPrefab = gunSettings.weaponPrefab;

                    template.gunSettings.muzzleFlashType = gunSettings.muzzleFlashType;
                    template.gunSettings.muzzleFlashPrefab = gunSettings.muzzleFlashPrefab;

                    template.gunSettings.muzzleLightType = gunSettings.muzzleLightType;
                    template.gunSettings.muzzleLightPrefab = gunSettings.muzzleLightPrefab;

                    #if (HFPS_163a || HFPS_163b)

                        if(gunSettings.weaponType == WeaponController.WeaponType.Shotgun){

                            template.gunSettings.shotgunSettings.ejectPosition = gunSettings.shotgunSettings.ejectPosition;
                            template.gunSettings.shotgunSettings.shellPrefab = gunSettings.shotgunSettings.shellPrefab;
                            template.gunSettings.shotgunSettings.shellRotation = gunSettings.shotgunSettings.shellRotation;
                            template.gunSettings.shotgunSettings.ejectSpeed = gunSettings.shotgunSettings.ejectSpeed;

                        }//weaponType = shotgun

                    #endif

                    #if HFPS_163c

                        template.gunSettings.bulletSettings.ejectShells = gunSettings.bulletSettings.ejectShells;

                        template.gunSettings.shellEjectSettings.ejectPosition = gunSettings.shellEjectSettings.ejectPosition;
                        template.gunSettings.shellEjectSettings.shellPrefab = gunSettings.shellEjectSettings.shellPrefab;
                        template.gunSettings.shellEjectSettings.shellRotation = gunSettings.shellEjectSettings.shellRotation;
                        template.gunSettings.shellEjectSettings.ejectSpeed = gunSettings.shellEjectSettings.ejectSpeed;
                        template.gunSettings.shellEjectSettings.ejectAutomatiacally = gunSettings.shellEjectSettings.ejectAutomatiacally;

                    #endif

                }//weaponType = Gun

                Debug.Log(template.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local);

            }//template != null

        }//Template_Save


    /////////////////////////
    ///
    ///     LOAD
    ///
    /////////////////////////


        private void Template_Load(){

            if(template != null){

                if(template.weaponName != ""){

                    weaponName = template.weaponName;

                }//template.weaponName

                weaponType = template.weaponType;

                if(template.armsPrefab != null){

                    armsPrefab = template.armsPrefab;

                }//armsPrefab != null

                if(weaponType == Weapon_Type.Candle){

                    candleSettings.BlowOut = template.candleSettings.BlowOut;

                    candleSettings.IdleAnimation = template.candleSettings.IdleAnimation;
                    candleSettings.BlowOutAnimation = template.candleSettings.BlowOutAnimation;

                    candleSettings.DrawAnimation = template.candleSettings.DrawAnimation;
                    candleSettings.DrawSpeed = template.candleSettings.DrawSpeed;

                    candleSettings.HideAnimation = template.candleSettings.HideAnimation;
                    candleSettings.HideSpeed = template.candleSettings.HideSpeed;

                    candleSettings.candleReduction = template.candleSettings.candleReduction;
                    candleSettings.reductionRate = template.candleSettings.reductionRate;
                    candleSettings.maxScale = template.candleSettings.maxScale;
                    candleSettings.minScale = template.candleSettings.minScale;

                    candleSettings.itemID = template.candleSettings.itemID;
                    candleSettings.blowOutKeepCandle = template.candleSettings.blowOutKeepCandle;
                    candleSettings.scaleKeepCandle = template.candleSettings.scaleKeepCandle;

                    candleSettings.candleType = template.candleSettings.candleType;

                    if(template.candleSettings.candlePrefab != null){

                        candleSettings.candlePrefab = template.candleSettings.candlePrefab;

                    }//candlePrefab != null

                    candleSettings.flameType = template.candleSettings.flameType;

                    if(template.candleSettings.flamePrefab != null){

                        candleSettings.flamePrefab = template.candleSettings.flamePrefab;

                    }//flamePrefab != null

                    candleSettings.lightType = template.candleSettings.lightType;

                    if(template.candleSettings.lightPrefab != null){

                        candleSettings.lightPrefab = template.candleSettings.lightPrefab;

                    }//lightPrefab != null

                }//weaponType = candle

                if(weaponType == Weapon_Type.Flashlight){

                    flashlightSettings.clickSound = template.flashlightSettings.clickSound;

                    flashlightSettings.IdleAnim = template.flashlightSettings.IdleAnim;

                    flashlightSettings.DrawAnim = template.flashlightSettings.DrawAnim;
                    flashlightSettings.DrawSpeed = template.flashlightSettings.DrawSpeed;

                    flashlightSettings.HideAnim = template.flashlightSettings.HideAnim;
                    flashlightSettings.HideSpeed = template.flashlightSettings.HideSpeed;

                    flashlightSettings.ReloadAnim = template.flashlightSettings.ReloadAnim;
                    flashlightSettings.ReloadSpeed = template.flashlightSettings.ReloadSpeed;

                    flashlightSettings.enableExtra = template.flashlightSettings.enableExtra;

                    flashlightSettings.ScareAnim = template.flashlightSettings.ScareAnim;
                    flashlightSettings.ScareAnimSpeed = template.flashlightSettings.ScareAnimSpeed;

                    flashlightSettings.NoPowerAnim = template.flashlightSettings.NoPowerAnim;
                    flashlightSettings.NoPowerAnimSpeed = template.flashlightSettings.NoPowerAnimSpeed;

                    flashlightSettings.infiniteBattery = template.flashlightSettings.infiniteBattery;
                    flashlightSettings.batteryLifeInSec = template.flashlightSettings.batteryLifeInSec;
                    flashlightSettings.canReloadPercent = template.flashlightSettings.canReloadPercent;

                    flashlightSettings.itemID = template.flashlightSettings.itemID;
                    flashlightSettings.flashlightIntensity = template.flashlightSettings.flashlightIntensity;
                    flashlightSettings.FlashlightIcon = template.flashlightSettings.FlashlightIcon;

                    flashlightSettings.flashLightType = template.flashlightSettings.flashLightType;

                    if(template.flashlightSettings.flashlightPrefab != null){

                        flashlightSettings.flashlightPrefab = template.flashlightSettings.flashlightPrefab;

                    }//flashlightPrefab != null

                    flashlightSettings.lightType = template.flashlightSettings.lightType;

                    if(template.flashlightSettings.lightPrefab != null){

                        flashlightSettings.lightPrefab = template.flashlightSettings.lightPrefab;

                    }//lightPrefab != null

                }//weaponType = Flashlight

                if(weaponType == Weapon_Type.Lantern){

                    lanternSettings.ShowSound = template.lanternSettings.ShowSound;
                    lanternSettings.ShowVolume = template.lanternSettings.ShowVolume;

                    lanternSettings.HideSound = template.lanternSettings.HideSound;
                    lanternSettings.HideVolume = template.lanternSettings.HideVolume;

                    lanternSettings.ReloadOilSound = template.lanternSettings.ReloadOilSound;
                    lanternSettings.ReloadVolume = template.lanternSettings.ReloadVolume;

                    lanternSettings.IdleAnim = template.lanternSettings.IdleAnim;

                    lanternSettings.DrawAnim = template.lanternSettings.DrawAnim;
                    lanternSettings.DrawSpeed = template.lanternSettings.DrawSpeed;

                    lanternSettings.HideAnim = template.lanternSettings.HideAnim;
                    lanternSettings.HideSpeed = template.lanternSettings.HideSpeed;

                    lanternSettings.ReloadAnim = template.lanternSettings.ReloadAnim;
                    lanternSettings.ReloadSpeed = template.lanternSettings.ReloadSpeed;

                    lanternSettings.itemID = template.lanternSettings.itemID;

                    lanternSettings.useHingeJoint = template.lanternSettings.useHingeJoint;
                    lanternSettings.secondDrawDiff = template.lanternSettings.secondDrawDiff;

                    lanternSettings.oilLifeInSec = template.lanternSettings.oilLifeInSec;
                    lanternSettings.oilPercentage = template.lanternSettings.oilPercentage;
                    lanternSettings.lightReductionRate = template.lanternSettings.lightReductionRate;
                    lanternSettings.canReloadPercent = template.lanternSettings.canReloadPercent;
                    lanternSettings.hideIntensitySpeed = template.lanternSettings.hideIntensitySpeed;
                    lanternSettings.oilReloadSpeed = template.lanternSettings.oilReloadSpeed;
                    lanternSettings.timeWaitToReload = template.lanternSettings.timeWaitToReload;

                    lanternSettings.ColorString = template.lanternSettings.ColorString;
                    lanternSettings.LanternIcon = template.lanternSettings.LanternIcon;

                    lanternSettings.lanternType = template.lanternSettings.lanternType;
                    lanternSettings.lanternPrefab = template.lanternSettings.lanternPrefab;

                    lanternSettings.flameType = template.lanternSettings.flameType;
                    lanternSettings.flamePrefab = template.lanternSettings.flamePrefab;

                    lanternSettings.lightType = template.lanternSettings.lightType;
                    lanternSettings.lightPrefab = template.lanternSettings.lightPrefab;

                    lanternSettings.hingeType = template.lanternSettings.hingeType;
                    lanternSettings.hingeConType = template.lanternSettings.hingeConType;
                    lanternSettings.hingeConPrefab = template.lanternSettings.hingeConPrefab;

                }//weaponType = Lantern

                if(weaponType == Weapon_Type.Lighter){

                    #if COMPONENTS_PRESENT

                        lighterSettings.soundLibrary = template.lighterSettings.soundLibrary;

                        lighterSettings.IdleAnimation = template.lighterSettings.IdleAnimation;

                        lighterSettings.DrawAnimation = template.lighterSettings.DrawAnimation;
                        lighterSettings.DrawSpeed = template.lighterSettings.DrawSpeed;

                        lighterSettings.HideAnimation = template.lighterSettings.HideAnimation;
                        lighterSettings.HideSpeed = template.lighterSettings.HideSpeed;

                        lighterSettings.lighterAnimation = template.lighterSettings.lighterAnimation;
                        lighterSettings.lighterOpenAnimation = template.lighterSettings.lighterOpenAnimation;
                        lighterSettings.lighterCloseAnimation = template.lighterSettings.lighterCloseAnimation;

                        lighterSettings.itemID = template.lighterSettings.itemID;

                        lighterSettings.lighterType = template.lighterSettings.lighterType;
                        lighterSettings.lighterPrefab = template.lighterSettings.lighterPrefab;

                        lighterSettings.flameType = template.lighterSettings.flameType;
                        lighterSettings.flamePrefab = template.lighterSettings.flamePrefab;

                        lighterSettings.lightType = template.lighterSettings.lightType;
                        lighterSettings.lightPrefab = template.lighterSettings.lightPrefab;

                    #endif

                }//weaponType = lighter

                if(weaponType == Weapon_Type.Melee){

                    meleeSettings.DrawSound = template.meleeSettings.DrawSound;
                    meleeSettings.DrawVolume = template.meleeSettings.DrawVolume;

                    meleeSettings.HideSound = template.meleeSettings.HideSound;
                    meleeSettings.HideVolume = template.meleeSettings.HideVolume;

                    meleeSettings.SwaySound = template.meleeSettings.SwaySound;
                    meleeSettings.SwayVolume = template.meleeSettings.SwayVolume;

                    meleeSettings.DrawAnim = template.meleeSettings.DrawAnim;
                    meleeSettings.DrawSpeed = template.meleeSettings.DrawSpeed;

                    meleeSettings.HideAnim = template.meleeSettings.HideAnim;
                    meleeSettings.HideSpeed = template.meleeSettings.HideSpeed;

                    meleeSettings.AttackAnim  = template.meleeSettings.AttackAnim;
                    meleeSettings.AttackSpeed = template.meleeSettings.AttackSpeed;

                    meleeSettings.itemID = template.meleeSettings.itemID;

                    meleeSettings.surfaceID = template.meleeSettings.surfaceID;
                    meleeSettings.surfaceDetails = template.meleeSettings.surfaceDetails;
                    meleeSettings.defaultSurfaceID = template.meleeSettings.defaultSurfaceID;

                    meleeSettings.HitLayer  = template.meleeSettings.HitLayer;
                    meleeSettings.HitDistance  = template.meleeSettings.HitDistance;
                    meleeSettings.HitForce  = template.meleeSettings.HitForce;
                    meleeSettings.HitWaitDelay = template.meleeSettings.HitWaitDelay;
                    meleeSettings.AttackDamage = template.meleeSettings.AttackDamage;

                    meleeSettings.SwayKickback = template.meleeSettings.SwayKickback;
                    meleeSettings.SwaySpeed = template.meleeSettings.SwaySpeed;

                    meleeSettings.weaponType = template.meleeSettings.weaponType;
                    meleeSettings.weaponPrefab = template.meleeSettings.weaponPrefab;

                }//weaponType = Melee

                if(weaponType == Weapon_Type.Gun){

                    gunSettings.audioSettings.reloadSound = template.gunSettings.audioSettings.reloadSound;

                    gunSettings.audioSettings.soundDraw = template.gunSettings.audioSettings.soundDraw;
                    gunSettings.audioSettings.volumeDraw = template.gunSettings.audioSettings.volumeDraw;

                    gunSettings.audioSettings.soundFire = template.gunSettings.audioSettings.soundFire;
                    gunSettings.audioSettings.volumeFire = template.gunSettings.audioSettings.volumeFire;

                    gunSettings.audioSettings.soundEmpty = template.gunSettings.audioSettings.soundEmpty;
                    gunSettings.audioSettings.volumeEmpty = template.gunSettings.audioSettings.volumeEmpty;

                    gunSettings.audioSettings.soundReload = template.gunSettings.audioSettings.soundReload;
                    gunSettings.audioSettings.volumeReload = template.gunSettings.audioSettings.volumeReload;

                    gunSettings.audioSettings.impactVolume = template.gunSettings.audioSettings.impactVolume;

                    gunSettings.aimingSettings.enableAiming = template.gunSettings.aimingSettings.enableAiming;
                    gunSettings.aimingSettings.steadyAim = template.gunSettings.aimingSettings.steadyAim;
                    gunSettings.aimingSettings.aimPosition = template.gunSettings.aimingSettings.aimPosition;

                    gunSettings.aimingSettings.aimSpeed = template.gunSettings.aimingSettings.aimSpeed;

                    gunSettings.aimingSettings.zoomFOVSmooth = template.gunSettings.aimingSettings.zoomFOVSmooth;
                    gunSettings.aimingSettings.unzoomFOVSmooth = template.gunSettings.aimingSettings.unzoomFOVSmooth;
                    gunSettings.aimingSettings.zoomFOV = template.gunSettings.aimingSettings.zoomFOV;

                    gunSettings.animationSettings.hideAnim = template.gunSettings.animationSettings.hideAnim;
                    gunSettings.animationSettings.fireAnim = template.gunSettings.animationSettings.fireAnim;
                    gunSettings.animationSettings.reloadAnim = template.gunSettings.animationSettings.reloadAnim;

                    gunSettings.animationSettings.beforeReloadAnim = template.gunSettings.animationSettings.beforeReloadAnim;
                    gunSettings.animationSettings.afterReloadAnim = template.gunSettings.animationSettings.afterReloadAnim;
                    gunSettings.animationSettings.afterReloadEmptyAnim = template.gunSettings.animationSettings.afterReloadEmptyAnim;

                    gunSettings.inventorySettings.weaponID = template.gunSettings.inventorySettings.weaponID;
                    gunSettings.inventorySettings.bulletsID = template.gunSettings.inventorySettings.bulletsID;

                    gunSettings.bulletSettings.FleshTag = template.gunSettings.bulletSettings.FleshTag;

                    gunSettings.bulletModelSettings.bulletPrefab = template.gunSettings.bulletModelSettings.bulletPrefab;
                    gunSettings.bulletModelSettings.bulletRotation = template.gunSettings.bulletModelSettings.bulletRotation;
                    gunSettings.bulletModelSettings.bulletForce = template.gunSettings.bulletModelSettings.bulletForce;

                    gunSettings.bulletSettings.bulletsInMag = template.gunSettings.bulletSettings.bulletsInMag;
                    gunSettings.bulletSettings.bulletsPerMag = template.gunSettings.bulletSettings.bulletsPerMag;
                    gunSettings.bulletSettings.bulletsPerShot = template.gunSettings.bulletSettings.bulletsPerShot;

                    gunSettings.bulletSettings.keepReloadMagBullets = template.gunSettings.bulletSettings.keepReloadMagBullets;
                    gunSettings.bulletSettings.soundOnImpact = template.gunSettings.bulletSettings.soundOnImpact;

                    gunSettings.bulletSettings.surfaceID = template.gunSettings.bulletSettings.surfaceID;
                    gunSettings.surfaceDetails = template.gunSettings.surfaceDetails;
                    gunSettings.bulletSettings.defaultSurfaceID = template.gunSettings.bulletSettings.defaultSurfaceID;

                    gunSettings.kickbackSettings.kickUp = template.gunSettings.kickbackSettings.kickUp;
                    gunSettings.kickbackSettings.kickSideways = template.gunSettings.kickbackSettings.kickSideways;
                    gunSettings.kickbackSettings.kickTime = template.gunSettings.kickbackSettings.kickTime;
                    gunSettings.kickbackSettings.kickReturnSpeed = template.gunSettings.kickbackSettings.kickReturnSpeed;

                    gunSettings.muzzleFlashSettings.enableMuzzleFlash = template.gunSettings.muzzleFlashSettings.enableMuzzleFlash;
                    gunSettings.muzzleFlashSettings.muzzleRotation = template.gunSettings.muzzleFlashSettings.muzzleRotation;

                    gunSettings.weaponSettings.weaponDamage = template.gunSettings.weaponSettings.weaponDamage;
                    gunSettings.weaponSettings.shootRange = template.gunSettings.weaponSettings.shootRange;
                    gunSettings.weaponSettings.hitforce = template.gunSettings.weaponSettings.hitforce;
                    gunSettings.weaponSettings.fireRate = template.gunSettings.weaponSettings.fireRate;
                    gunSettings.weaponSettings.recoil = template.gunSettings.weaponSettings.recoil;

                    gunSettings.npcReactionSettings.enableSoundReaction = template.gunSettings.npcReactionSettings.enableSoundReaction;
                    gunSettings.npcReactionSettings.soundReactionRadius = template.gunSettings.npcReactionSettings.soundReactionRadius;

                    gunSettings.weaponType = template.gunSettings.weaponType;
                    gunSettings.bulletType = template.gunSettings.bulletType;
                    gunSettings.raycastMask = template.gunSettings.raycastMask;
                    gunSettings.soundReactionMask = template.gunSettings.soundReactionMask;

                    gunSettings.weapModelType = template.gunSettings.weapModelType;
                    gunSettings.weaponPrefab = template.gunSettings.weaponPrefab;

                    gunSettings.muzzleFlashType = template.gunSettings.muzzleFlashType;
                    gunSettings.muzzleFlashPrefab = template.gunSettings.muzzleFlashPrefab;

                    gunSettings.muzzleLightType = template.gunSettings.muzzleLightType;
                    gunSettings.muzzleLightPrefab = template.gunSettings.muzzleLightPrefab;

                    #if (HFPS_163a || HFPS_163b)

                        if(gunSettings.weaponType == WeaponController.WeaponType.Shotgun){

                            gunSettings.shotgunSettings.ejectPosition = template.gunSettings.shotgunSettings.ejectPosition;
                            gunSettings.shotgunSettings.shellPrefab = template.gunSettings.shotgunSettings.shellPrefab;
                            gunSettings.shotgunSettings.shellRotation = template.gunSettings.shotgunSettings.shellRotation;
                            gunSettings.shotgunSettings.ejectSpeed = template.gunSettings.shotgunSettings.ejectSpeed;

                        }//weaponType = shotgun

                    #endif

                    #if HFPS_163c

                        gunSettings.bulletSettings.ejectShells = template.gunSettings.bulletSettings.ejectShells;

                        gunSettings.shellEjectSettings.ejectPosition = template.gunSettings.shellEjectSettings.ejectPosition;
                        gunSettings.shellEjectSettings.shellPrefab = template.gunSettings.shellEjectSettings.shellPrefab;
                        gunSettings.shellEjectSettings.shellRotation = template.gunSettings.shellEjectSettings.shellRotation;
                        gunSettings.shellEjectSettings.ejectSpeed = template.gunSettings.shellEjectSettings.ejectSpeed;
                        gunSettings.shellEjectSettings.ejectAutomatiacally = template.gunSettings.shellEjectSettings.ejectAutomatiacally;

                    #endif

                }//weaponType = Gun

                Debug.Log(template.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[7].local);

            }//template != null

        }//Template_Load


    //////////////////////////////////
    ///
    ///     CATCH ACTIONS
    ///
    //////////////////////////////////


        private void ItemSwitcher_Catch(){

            var tempItemSwitchs = FindObjectsOfType<ItemSwitcher>();

            if(tempItemSwitchs.Length > 0){

                itemSwitcher = tempItemSwitchs[0];

                EditorUtility.SetDirty(this);

            }//tempItemSwitchs.Length > 0

            if(itemSwitcher != null){

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[9].local);

            }//itemSwitcher != null

            EditorUtility.SetDirty(this);

        }//ItemSwitcher_Catch

        private void Catch_Kickback(){

            var tempKickbacks = FindObjectsOfType<StabilizeKickback>();

            if(tempKickbacks.Length > 0){

                for(int k = 0; k < tempKickbacks.Length; k++){

                    if(tempKickbacks[k].gameObject.name == "ArmsKickback"){

                        gunSettings.kickback = tempKickbacks[k];

                    }//name = ArmsKickback

                }//for k tempKickbacks

            }//tempKickbacks.Length > 0

            if(gunSettings.kickback != null){

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[10].local);

            }//kickback != null

            EditorUtility.SetDirty(this);

        }//Catch_Kickback


    //////////////////////////////////
    ///
    ///     OBJECT ACTIONS
    ///
    //////////////////////////////////


        private void Object_Create(GameObject newObj, Transform parent){

            GameObject tempObj = Instantiate(newObj, parent);
            tempObj.name = newObj.name;

            Debug.Log(newObj.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[8].local);

        }//Object_Create


    //////////////////////////////////
    ///
    ///     ARMS ACTIONS
    ///
    //////////////////////////////////


        private void Arms_Create(){

            if(itemSwitcher != null){

                if(armsPrefab != null){

                    if(weaponName != ""){

                        GameObject tempObj = new GameObject();
                        GameObject newWeap = Instantiate(tempObj, itemSwitcher.WallHitTransform);
                        newWeap.name = weaponName;
                        newWeap.layer = 16;

                        GameObject tempHands = Instantiate(armsPrefab, newWeap.transform);
                        tempHands.name = armsPrefab.name;

                        tempHands.transform.localPosition = new Vector3(0, -1.5f, 0);
                        tempHands.transform.localEulerAngles = new Vector3(0, 0, 0);

                        foreach(Transform childs in tempHands.transform.GetComponentsInChildren<Transform>(true)){

                            childs.gameObject.layer = 16;

                        }//foreach

                        tempWeapon = newWeap;
                        tempArms = tempHands;

                        DestroyImmediate(tempObj);

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[11].local);

                        armsParent = tempHands;

                    }//weaponName != null

                }//armsPrefab != null

            }//itemSwitcher != null

        }//Arms_Create


    //////////////////////////////////
    ///
    ///     CANDLE ACTIONS
    ///
    //////////////////////////////////


        private void Candle_Create(){

            GameObject tempObj = Instantiate(candleSettings.candlePrefab, candleSettings.candleParent);

            tempObj.name = candleSettings.candlePrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            candleSettings.Candle = tempObj;
            candleSettings.candleType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[12].local);
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[13].local);

        }//Candle_Create

        private void CandleFlame_Create(){

            GameObject tempObj = Instantiate(candleSettings.flamePrefab, candleSettings.candleFlameParent);

            tempObj.name = candleSettings.flamePrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);
            tempObj.transform.parent = candleSettings.candleFlameParent.parent;

            candleSettings.CandleFlame = tempObj;
            candleSettings.FlamePosition = candleSettings.candleFlameParent;
            candleSettings.flameType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[14].local);
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[15].local);
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[16].local);

        }//CandleFlame_Create

        private void CandleLight_Create(){

            GameObject tempObj = Instantiate(candleSettings.lightPrefab, candleSettings.candleLightParent);

            tempObj.name = candleSettings.lightPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            candleSettings.CandleLight = tempObj;
            candleSettings.lightType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[17].local);
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[18].local);

        }//CandleLight_Create


    //////////////////////////////////
    ///
    ///     FLASHLIGHT ACTIONS
    ///
    //////////////////////////////////


        private void Flashlight_Create(){

            GameObject tempObj = Instantiate(flashlightSettings.flashlightPrefab, flashlightSettings.flashlightParent);

            tempObj.name = flashlightSettings.flashlightPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            flashlightSettings.flashLightType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[19].local);

        }//Flashlight_Create

        private void FlashlightLight_Create(){

            GameObject tempObj = Instantiate(flashlightSettings.lightPrefab, flashlightSettings.lightParent);

            tempObj.name = flashlightSettings.lightPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            flashlightSettings.LightObject = tempObj.GetComponent<Light>();
            flashlightSettings.lightType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[20].local);
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[21].local);

        }//FlashlightLight_Create


    //////////////////////////////////
    ///
    ///     LANTERN ACTIONS
    ///
    //////////////////////////////////


        private void Lantern_Create(){

            GameObject tempObj = Instantiate(lanternSettings.lanternPrefab, lanternSettings.lanternParent);

            tempObj.name = lanternSettings.lanternPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lanternSettings.lanternType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[22].local);

        }//Lantern_Create

        private void LanternFlame_Create(){

            GameObject tempObj = Instantiate(lanternSettings.flamePrefab, lanternSettings.flameParent);

            tempObj.name = lanternSettings.flamePrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lanternSettings.flameType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

                if(childs.GetComponent<MeshRenderer>() != null){

                    lanternSettings.flameMesh = childs.GetComponent<MeshRenderer>();

                }//MeshRenderer != null

            }//foreach

            #if COMPONENTS_PRESENT

                //NOTHING / LEAVE AS IS

            #else 

                if(lanternSettings.flameMesh != null){

                    if(lanternSettings.LanternLight != null){

                        lanternSettings.flameMesh.transform.parent = lanternSettings.LanternLight.transform;

                        DestroyImmediate(tempObj);

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[23].local);
                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[24].local);

                    }//LanternLight != null

                }//flameMesh != null

            #endif

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[25].local);

        }//LanternFlame_Create

        private void LanternLight_Create(){

            GameObject tempObj = Instantiate(lanternSettings.lightPrefab, lanternSettings.lightParent);

            tempObj.name = lanternSettings.lightPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lanternSettings.LanternLight = tempObj.GetComponent<Light>();
            lanternSettings.lightType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            EditorUtility.SetDirty(this);

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[26].local);
            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[27].local);

        }//LanternLight_Create

        private void LanternHinge_Create(){

            Rigidbody tempRigid = lanternSettings.hingeParent.AddComponent<Rigidbody>();

            tempRigid.mass = 1;
            tempRigid.drag = 3;
            tempRigid.angularDrag = 1;

            HingeJoint tempHinge = lanternSettings.hingeParent.AddComponent<HingeJoint>();

            //tempHinge.connectedBody = lanternSettings.hingeConnect;

            tempHinge.useLimits = true;
            tempHinge.enablePreprocessing = false;

            JointLimits limits = tempHinge.limits;
            limits.min = -110;
            limits.max = 33;
            limits.bounciness = 0.5f;
            limits.bounceMinVelocity = 0.2f;
            tempHinge.limits = limits;

            lanternSettings.hingeLantern = tempHinge;
            lanternSettings.hingeType = Model_Type.Local;

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[28].local);

        }//LanternHinge_Create

        private void LanternHingeConnect_Create(){

            GameObject tempObj = Instantiate(lanternSettings.hingeConPrefab, lanternSettings.hingeConParent);

            tempObj.name = lanternSettings.hingeConPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lanternSettings.hingeConnect = tempObj.GetComponent<Rigidbody>();
            lanternSettings.hingeConType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[29].local);

            if(lanternSettings.hingeLantern != null){

                lanternSettings.hingeLantern.connectedBody = lanternSettings.hingeConnect;

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[30].local);

            }//hingeLantern != null

        }//LanternHingeConnect_Create



    //////////////////////////////////
    ///
    ///     LIGHTER ACTIONS
    ///
    //////////////////////////////////


        private void LighterModel_Create(){

            GameObject tempObj = Instantiate(lighterSettings.lighterPrefab, lighterSettings.lighterParent);

            tempObj.name = lighterSettings.lighterPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lighterSettings.lighterType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[31].local);

        }//LighterModel_Create

        private void LighterFlame_Create(){

            GameObject tempObj = Instantiate(lighterSettings.flamePrefab, lighterSettings.flameParent);

            tempObj.name = lighterSettings.flamePrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lighterSettings.flame = tempObj;
            lighterSettings.flameType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[32].local);

        }//LighterFlame_Create

        private void LighterLight_Create(){

            GameObject tempObj = Instantiate(lighterSettings.lightPrefab, lighterSettings.lightParent);

            tempObj.name = lighterSettings.lightPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            lighterSettings.light = tempObj;
            lighterSettings.lightType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[33].local);

        }//LighterLight_Create


    //////////////////////////////////
    ///
    ///     MELEE ACTIONS
    ///
    //////////////////////////////////


        private void MeleeWeapon_Create(){

            GameObject tempObj = Instantiate(meleeSettings.weaponPrefab, meleeSettings.weaponParent);

            tempObj.name = meleeSettings.weaponPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            meleeSettings.weaponType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[34].local);

        }//MeleeWeapon_Create


    //////////////////////////////////
    ///
    ///     GUN ACTIONS
    ///
    //////////////////////////////////


        private void GunWeapon_Create(){

            GameObject tempObj = Instantiate(gunSettings.weaponPrefab, gunSettings.weaponParent);

            tempObj.name = gunSettings.weaponPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            gunSettings.weapModelType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[35].local);

        }//GunWeapon_Create

        private void GunMuzzleFlash_Create(){

            GameObject tempObj = Instantiate(gunSettings.muzzleFlashPrefab, gunSettings.muzzleFlashParent);

            tempObj.name = gunSettings.muzzleFlashPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            gunSettings.muzzleFlashType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

                if(childs.GetComponent<MeshRenderer>() != null){

                    gunSettings.muzzleFlashSettings.muzzleFlash = childs.GetComponent<MeshRenderer>();

                }//MeshRenderer != null

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[36].local);

        }//GunMuzzleFlash_Create

        private void GunMuzzleLight_Create(){

            GameObject tempObj = Instantiate(gunSettings.muzzleLightPrefab, gunSettings.muzzleLightParent);

            tempObj.name = gunSettings.muzzleLightPrefab.name;
            tempObj.layer = 16;

            tempObj.transform.localPosition = new Vector3(0, 0, 0);
            tempObj.transform.localEulerAngles = new Vector3(0, 0, 0);

            gunSettings.muzzleFlashSettings.muzzleLight = tempObj.GetComponent<Light>();
            gunSettings.muzzleLightType = Model_Type.Local;

            foreach(Transform childs in tempObj.transform.GetComponentsInChildren<Transform>(true)){

                childs.gameObject.layer = 16;

            }//foreach

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[37].local);

        }//GunMuzzleLight_Create


    //////////////////////////////////
    ///
    ///     WEAPON ACTIONS
    ///
    //////////////////////////////////


        private void Weapon_Create(){


    /////////////////////////
    ///
    ///     CANDLE
    ///
    /////////////////////////


            if(weaponType == Weapon_Type.Candle){

                CandleItem tempCandleItem = tempWeapon.AddComponent<CandleItem>();

                tempCandleItem.CandleGO = armsParent;
                tempCandleItem.BlowOut = candleSettings.BlowOut;

                tempCandleItem.IdleAnimation = candleSettings.IdleAnimation.name;

                if(candleSettings.BlowOutAnimation != null){

                    tempCandleItem.BlowOutAnimation = candleSettings.BlowOutAnimation.name;

                //tempCandleItem.BlowOutAnimation != null
                } else {

                    if(candleSettings.HideAnimation != null){

                        tempCandleItem.BlowOutAnimation = candleSettings.HideAnimation.name;

                    }//HideAnimation != null

                }//tempCandleItem.BlowOutAnimation != null

                if(candleSettings.DrawAnimation != null){

                    tempCandleItem.DrawAnimation = candleSettings.DrawAnimation.name;

                }//DrawAnimation != null

                tempCandleItem.DrawSpeed = candleSettings.DrawSpeed;

                if(candleSettings.HideAnimation != null){

                    tempCandleItem.HideAnimation = candleSettings.HideAnimation.name;

                }//HideAnimation != null

                tempCandleItem.HideSpeed = candleSettings.HideSpeed;

                tempCandleItem.Candle = candleSettings.Candle;
                tempCandleItem.CandleLight = candleSettings.CandleLight;

                tempCandleItem.CandleFlame = candleSettings.CandleFlame;
                tempCandleItem.FlamePosition = candleSettings.FlamePosition;

                tempCandleItem.candleReduction = candleSettings.candleReduction;
                tempCandleItem.reductionRate = candleSettings.reductionRate;
                tempCandleItem.maxScale = candleSettings.maxScale;
                tempCandleItem.minScale = candleSettings.minScale;

                tempCandleItem.CandleID = candleSettings.itemID;
                tempCandleItem.InventoryID = candleSettings.itemID;

                tempCandleItem.blowOutKeepCandle = candleSettings.blowOutKeepCandle;
                tempCandleItem.scaleKeepCandle = candleSettings.scaleKeepCandle;

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[38].local);

                if(armsParent.GetComponent<Animation>() == null){

                    Animation tempAnim = armsParent.AddComponent<Animation>();

                    tempAnim.playAutomatically = false;

                    if(candleSettings.IdleAnimation != null){

                        SerializedObject idleSerialize = new SerializedObject(candleSettings.IdleAnimation);

                        if(!idleSerialize.FindProperty("m_Legacy").boolValue){

                            idleSerialize.FindProperty("m_Legacy").boolValue = true;
                            idleSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(candleSettings.IdleAnimation, candleSettings.IdleAnimation.name);

                    }//IdleAnimation != null

                    if(candleSettings.BlowOutAnimation != null){

                        SerializedObject blowOutSerialize = new SerializedObject(candleSettings.BlowOutAnimation);

                        if(!blowOutSerialize.FindProperty("m_Legacy").boolValue){

                            blowOutSerialize.FindProperty("m_Legacy").boolValue = true;
                            blowOutSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(candleSettings.BlowOutAnimation, candleSettings.BlowOutAnimation.name);

                    }//tempCandleItem.BlowOutAnimation != null

                    if(candleSettings.DrawAnimation != null){

                        SerializedObject drawSerialize = new SerializedObject(candleSettings.DrawAnimation);

                        if(!drawSerialize.FindProperty("m_Legacy").boolValue){

                            drawSerialize.FindProperty("m_Legacy").boolValue = true;
                            drawSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(candleSettings.DrawAnimation, candleSettings.DrawAnimation.name);

                    }//DrawAnimation != null

                    if(candleSettings.HideAnimation != null){

                        SerializedObject hideSerialize = new SerializedObject(candleSettings.HideAnimation);

                        if(!hideSerialize.FindProperty("m_Legacy").boolValue){

                            hideSerialize.FindProperty("m_Legacy").boolValue = true;
                            hideSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(candleSettings.HideAnimation, candleSettings.HideAnimation.name);

                    }//HideAnimation != null

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[44].local);

                }//Animation = null

                HFPS.Systems.AnimationEvent tempAnimEvent = null;

                if(armsParent.GetComponent<HFPS.Systems.AnimationEvent>() == null){

                    tempAnimEvent = armsParent.AddComponent<HFPS.Systems.AnimationEvent>();

                //AnimationEvent = null
                } else {

                    tempAnimEvent = armsParent.GetComponent<HFPS.Systems.AnimationEvent>();

                }//AnimationEvent = null

                if(tempAnimEvent != null){

                    HFPS.Systems.AnimationEvent.AnimEvents tempEvent = new HFPS.Systems.AnimationEvent.AnimEvents();
                    tempEvent.EventCallName = "BlowOut";
                    tempEvent.CallEvent = new UnityEvent();

                    UnityAction methodDelegate = System.Delegate.CreateDelegate (typeof(UnityAction), tempCandleItem, "BlowOut_Event") as UnityAction;
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent.CallEvent, methodDelegate);

                    tempAnimEvent.AnimationEvents = new HFPS.Systems.AnimationEvent.AnimEvents[1];
                    tempAnimEvent.AnimationEvents[0] = tempEvent;

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[48].local);

                }//tempAnimEvent != null

            }//weaponType = candle


    /////////////////////////
    ///
    ///     FLASHLIGHT
    ///
    /////////////////////////


            if(weaponType == Weapon_Type.Flashlight){

                FlashlightItem tempFlashItem = tempWeapon.AddComponent<FlashlightItem>();

                tempFlashItem.FlashlightGO = armsParent;
                tempFlashItem.ClickSound = flashlightSettings.clickSound;

                if(flashlightSettings.IdleAnim != null){

                    tempFlashItem.IdleAnim = flashlightSettings.IdleAnim.name;

                }//IdleAnim != null

                if(flashlightSettings.DrawAnim != null){

                    tempFlashItem.DrawAnim = flashlightSettings.DrawAnim.name;

                }//DrawAnim != null

                tempFlashItem.DrawSpeed = flashlightSettings.DrawSpeed;

                if(flashlightSettings.HideAnim != null){

                    tempFlashItem.HideAnim = flashlightSettings.HideAnim.name;

                }//HideAnim != null

                tempFlashItem.HideSpeed = flashlightSettings.HideSpeed;

                if(flashlightSettings.ReloadAnim != null){

                    tempFlashItem.ReloadAnim = flashlightSettings.ReloadAnim.name;

                //ReloadAnim != null
                } else {

                    if(flashlightSettings.HideAnim != null){

                        tempFlashItem.ReloadAnim = flashlightSettings.HideAnim.name;

                    }//HideAnim != null

                }//ReloadAnim != null

                tempFlashItem.ReloadSpeed = flashlightSettings.ReloadSpeed;

                tempFlashItem.enableExtra = flashlightSettings.enableExtra;

                if(flashlightSettings.ScareAnim != null){

                    tempFlashItem.ScareAnim = flashlightSettings.ScareAnim.name;

                }//ScareAnim != null

                tempFlashItem.ScareAnimSpeed = flashlightSettings.ScareAnimSpeed;

                if(flashlightSettings.NoPowerAnim != null){

                    tempFlashItem.NoPowerAnim = flashlightSettings.NoPowerAnim.name;

                }//NoPowerAnim != null

                tempFlashItem.NoPowerAnimSpeed = flashlightSettings.NoPowerAnimSpeed;

                tempFlashItem.InfiniteBattery = flashlightSettings.infiniteBattery;

                tempFlashItem.batteryLifeInSec = flashlightSettings.batteryLifeInSec;
                tempFlashItem.canReloadPercent = flashlightSettings.canReloadPercent;

                tempFlashItem.FlashlightID = flashlightSettings.itemID;

                if(flashlightSettings.LightObject != null){

                    tempFlashItem.LightObject = flashlightSettings.LightObject;

                }//LightObject != null

                tempFlashItem.flashlightIntensity = flashlightSettings.flashlightIntensity;

                tempFlashItem.FlashlightIcon = flashlightSettings.FlashlightIcon;

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[39].local);

                if(armsParent.GetComponent<Animation>() == null){

                    Animation tempAnim = armsParent.AddComponent<Animation>();

                    tempAnim.playAutomatically = false;

                    if(flashlightSettings.IdleAnim != null){

                        SerializedObject idleSerialize = new SerializedObject(flashlightSettings.IdleAnim);

                        if(!idleSerialize.FindProperty("m_Legacy").boolValue){

                            idleSerialize.FindProperty("m_Legacy").boolValue = true;
                            idleSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(flashlightSettings.IdleAnim, flashlightSettings.IdleAnim.name);

                    }//IdleAnimation != null

                    if(flashlightSettings.DrawAnim != null){

                        SerializedObject drawSerialize = new SerializedObject(flashlightSettings.DrawAnim);

                        if(!drawSerialize.FindProperty("m_Legacy").boolValue){

                            drawSerialize.FindProperty("m_Legacy").boolValue = true;
                            drawSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(flashlightSettings.DrawAnim, flashlightSettings.DrawAnim.name);

                    }//DrawAnim != null

                    if(flashlightSettings.HideAnim != null){

                        SerializedObject hideSerialize = new SerializedObject(flashlightSettings.HideAnim);

                        if(!hideSerialize.FindProperty("m_Legacy").boolValue){

                            hideSerialize.FindProperty("m_Legacy").boolValue = true;
                            hideSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(flashlightSettings.HideAnim, flashlightSettings.HideAnim.name);

                    }//HideAnim != null

                    if(flashlightSettings.ReloadAnim != null){

                        SerializedObject reloadSerialize = new SerializedObject(flashlightSettings.ReloadAnim);

                        if(!reloadSerialize.FindProperty("m_Legacy").boolValue){

                            reloadSerialize.FindProperty("m_Legacy").boolValue = true;
                            reloadSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(flashlightSettings.ReloadAnim, flashlightSettings.ReloadAnim.name);

                    }//ReloadAnim != null

                    if(flashlightSettings.ScareAnim != null){

                        SerializedObject scareSerialize = new SerializedObject(flashlightSettings.ScareAnim);

                        if(!scareSerialize.FindProperty("m_Legacy").boolValue){

                            scareSerialize.FindProperty("m_Legacy").boolValue = true;
                            scareSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(flashlightSettings.ScareAnim, flashlightSettings.ScareAnim.name);

                    }//ScareAnim != null

                    if(flashlightSettings.NoPowerAnim != null){

                        SerializedObject noPowerSerialize = new SerializedObject(flashlightSettings.NoPowerAnim);

                        if(!noPowerSerialize.FindProperty("m_Legacy").boolValue){

                            noPowerSerialize.FindProperty("m_Legacy").boolValue = true;
                            noPowerSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(flashlightSettings.NoPowerAnim, flashlightSettings.NoPowerAnim.name);

                    }//NoPowerAnim != null

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[44].local);

                }//Animation = null

                HFPS.Systems.AnimationEvent tempAnimEvent = null;

                if(armsParent.GetComponent<HFPS.Systems.AnimationEvent>() == null){

                    tempAnimEvent = armsParent.AddComponent<HFPS.Systems.AnimationEvent>();

                //AnimationEvent = null
                } else {

                    tempAnimEvent = armsParent.GetComponent<HFPS.Systems.AnimationEvent>();

                }//AnimationEvent = null

                if(tempAnimEvent != null){

                    HFPS.Systems.AnimationEvent.AnimEvents tempEvent = new HFPS.Systems.AnimationEvent.AnimEvents();
                    tempEvent.EventCallName = "FlashlightClick";
                    tempEvent.CallEvent = new UnityEvent();

                    UnityAction methodDelegate = System.Delegate.CreateDelegate (typeof(UnityAction), tempFlashItem, "Event_FlashlightOn") as UnityAction;
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent.CallEvent, methodDelegate);

                    tempAnimEvent.AnimationEvents = new HFPS.Systems.AnimationEvent.AnimEvents[1];
                    tempAnimEvent.AnimationEvents[0] = tempEvent;

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[49].local);

                }//tempAnimEvent != null

                HFPS.Systems.AnimationSoundEvents tempAnimSoundEvent = null;

                if(armsParent.GetComponent<HFPS.Systems.AnimationSoundEvents>() == null){

                    tempAnimSoundEvent = armsParent.AddComponent<HFPS.Systems.AnimationSoundEvents>();

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[45].local);

                //AnimationEvent = null
                } else {

                    tempAnimSoundEvent = armsParent.GetComponent<HFPS.Systems.AnimationSoundEvents>();

                }//AnimationEvent = null

                if(tempAnimSoundEvent != null){

                    tempAnimSoundEvent.soundVolume = 1f;

                    if(template != null){

                        if(template.animationSoundEvents.Count > 0){

                            for(int ase = 0; ase < template.animationSoundEvents.Count; ase++){

                                tempAnimSoundEvent.soundEvents.Add(template.animationSoundEvents[ase]);

                            }//for ase animationSoundEvents

                        }//animationSoundEvents.Count > 0

                    }//template != null

                }//tempAnimSoundEvent != null

                AudioSource tempAudSource = null;

                if(armsParent.GetComponent<AudioSource>() == null){

                    tempAudSource = armsParent.AddComponent<AudioSource>();

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[46].local);

                //AudioSource = null
                } else {

                    tempAudSource = armsParent.GetComponent<AudioSource>();

                }//AudioSource = null

                if(tempAudSource != null){

                    tempAudSource.playOnAwake = false;

                    tempFlashItem.audioSource = tempAudSource;

                }//tempAudSource != null

            }//weaponType = flashlight


    /////////////////////////
    ///
    ///     LANTERN
    ///
    /////////////////////////


            if(weaponType == Weapon_Type.Lantern){

                LanternItem tempLanternItem = tempWeapon.AddComponent<LanternItem>();

                tempLanternItem.LanternGO = armsParent;

                tempLanternItem.ShowSound = lanternSettings.ShowSound;
                tempLanternItem.ShowVolume = lanternSettings.ShowVolume;

                tempLanternItem.HideSound = lanternSettings.HideSound;
                tempLanternItem.HideVolume = lanternSettings.HideVolume;

                tempLanternItem.ReloadOilSound = lanternSettings.ReloadOilSound;
                tempLanternItem.ReloadVolume = lanternSettings.ReloadVolume;

                if(lanternSettings.IdleAnim != null){

                    tempLanternItem.IdleAnim = lanternSettings.IdleAnim.name;

                }//IdleAnim != null

                if(lanternSettings.DrawAnim != null){

                    tempLanternItem.DrawAnim = lanternSettings.DrawAnim.name;

                }//DrawAnim != null

                tempLanternItem.DrawSpeed = lanternSettings.DrawSpeed;

                if(lanternSettings.HideAnim != null){

                    tempLanternItem.HideAnim = lanternSettings.HideAnim.name;

                }//HideAnim != null

                tempLanternItem.HideSpeed = lanternSettings.HideSpeed;

                if(lanternSettings.ReloadAnim != null){

                    tempLanternItem.ReloadAnim = lanternSettings.ReloadAnim.name;

                }//ReloadAnim != null

                tempLanternItem.ReloadSpeed = lanternSettings.ReloadSpeed;

                tempLanternItem.lanternID = lanternSettings.itemID;

                tempLanternItem.useHingeJoint = lanternSettings.useHingeJoint;
                tempLanternItem.hingeLantern = lanternSettings.hingeLantern;
                tempLanternItem.secondDrawDiff = lanternSettings.secondDrawDiff;

                if(lanternSettings.hingeLantern != null){

                    if(lanternSettings.hingeConnect != null){

                        if(lanternSettings.hingeLantern.connectedBody != lanternSettings.hingeConnect){

                            lanternSettings.hingeLantern.connectedBody = lanternSettings.hingeConnect;

                        }//connectedBody != hingeConnect

                    }//hingeConnect != null

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[54].local);

                }//hingeLantern != null

                tempLanternItem.oilLifeInSec = lanternSettings.oilLifeInSec;
                tempLanternItem.oilPercentage = lanternSettings.oilPercentage;
                tempLanternItem.lightReductionRate = lanternSettings.lightReductionRate;
                tempLanternItem.canReloadPercent = lanternSettings.canReloadPercent;
                tempLanternItem.hideIntensitySpeed = lanternSettings.hideIntensitySpeed;
                tempLanternItem.oilReloadSpeed = lanternSettings.oilReloadSpeed;
                tempLanternItem.timeWaitToReload = lanternSettings.timeWaitToReload;

                tempLanternItem.LanternLight = lanternSettings.LanternLight;
                tempLanternItem.ColorString = lanternSettings.ColorString;
                tempLanternItem.LanternIcon = lanternSettings.LanternIcon;

                #if COMPONENTS_PRESENT

                    tempLanternItem.flameMesh = lanternSettings.flameMesh;

                #endif

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[40].local);

                if(armsParent.GetComponent<Animation>() == null){

                    Animation tempAnim = armsParent.AddComponent<Animation>();

                    tempAnim.playAutomatically = false;

                    if(lanternSettings.IdleAnim != null){

                        SerializedObject idleSerialize = new SerializedObject(lanternSettings.IdleAnim);

                        if(!idleSerialize.FindProperty("m_Legacy").boolValue){

                            idleSerialize.FindProperty("m_Legacy").boolValue = true;
                            idleSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(lanternSettings.IdleAnim, lanternSettings.IdleAnim.name);

                    }//IdleAnimation != null

                    if(lanternSettings.DrawAnim != null){

                        SerializedObject drawSerialize = new SerializedObject(lanternSettings.DrawAnim);

                        if(!drawSerialize.FindProperty("m_Legacy").boolValue){

                            drawSerialize.FindProperty("m_Legacy").boolValue = true;
                            drawSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(lanternSettings.DrawAnim, lanternSettings.DrawAnim.name);

                    }//DrawAnim != null

                    if(lanternSettings.HideAnim != null){

                        SerializedObject hideSerialize = new SerializedObject(lanternSettings.HideAnim);

                        if(!hideSerialize.FindProperty("m_Legacy").boolValue){

                            hideSerialize.FindProperty("m_Legacy").boolValue = true;
                            hideSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(lanternSettings.HideAnim, lanternSettings.HideAnim.name);

                    }//HideAnim != null

                    if(lanternSettings.ReloadAnim != null){

                        SerializedObject reloadSerialize = new SerializedObject(lanternSettings.ReloadAnim);

                        if(!reloadSerialize.FindProperty("m_Legacy").boolValue){

                            reloadSerialize.FindProperty("m_Legacy").boolValue = true;
                            reloadSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(lanternSettings.ReloadAnim, lanternSettings.ReloadAnim.name);

                    }//ReloadAnim != null

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[44].local);

                }//Animation = null

                AudioSource tempAudSource = null;

                if(armsParent.GetComponent<AudioSource>() == null){

                    tempAudSource = armsParent.AddComponent<AudioSource>();

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[46].local);

                //AudioSource = null
                } else {

                    tempAudSource = armsParent.GetComponent<AudioSource>();

                }//AudioSource = null

                if(tempAudSource != null){

                    tempAudSource.playOnAwake = false;

                }//tempAudSource != null

            }//weaponType = lantern


    /////////////////////////
    ///
    ///     LIGHTER
    ///
    /////////////////////////


            if(weaponType == Weapon_Type.Lighter){

                #if COMPONENTS_PRESENT

                    LighterItem tempLighterItem = tempWeapon.AddComponent<LighterItem>();

                    tempLighterItem.itemGO = armsParent;

                    tempLighterItem.soundLibrary = new List<LighterItem.Sound_Library>();

                    for(int s = 0; s < lighterSettings.soundLibrary.Count; s++){

                        tempLighterItem.soundLibrary.Add(lighterSettings.soundLibrary[s]);

                    }//for s soundLibrary

                    if(lighterSettings.IdleAnimation != null){

                        tempLighterItem.IdleAnimation = lighterSettings.IdleAnimation.name;

                    }//IdleAnimation != null

                    if(lighterSettings.DrawAnimation != null){

                        tempLighterItem.DrawAnimation = lighterSettings.DrawAnimation.name;

                    }//DrawAnimation != null

                    tempLighterItem.DrawSpeed = lighterSettings.DrawSpeed;

                    if(lighterSettings.HideAnimation != null){

                        tempLighterItem.HideAnimation = lighterSettings.HideAnimation.name;

                    }//HideAnimation != null

                    tempLighterItem.HideSpeed = lighterSettings.HideSpeed;

                    tempLighterItem.lighterAnimation = lighterSettings.lighterAnimation;

                    if(lighterSettings.lighterOpenAnimation != null){

                        tempLighterItem.lighterOpenAnimation = lighterSettings.lighterOpenAnimation.name;

                    }//lighterOpenAnimation != null

                    if(lighterSettings.lighterCloseAnimation != null){

                        tempLighterItem.lighterCloseAnimation = lighterSettings.lighterCloseAnimation.name;

                    }//lighterCloseAnimation != null

                    tempLighterItem.itemID = lighterSettings.itemID;

                    tempLighterItem.flame = lighterSettings.flame;
                    tempLighterItem.light = lighterSettings.light;
                    tempLighterItem.FlamePosition = lighterSettings.FlamePosition;

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[41].local);

                    if(armsParent.GetComponent<Animation>() == null){

                        Animation tempAnim = armsParent.AddComponent<Animation>();

                        tempAnim.playAutomatically = false;

                        if(lighterSettings.DrawAnimation != null){

                            SerializedObject drawSerialize = new SerializedObject(lighterSettings.DrawAnimation);

                            if(!drawSerialize.FindProperty("m_Legacy").boolValue){

                                drawSerialize.FindProperty("m_Legacy").boolValue = true;
                                drawSerialize.ApplyModifiedProperties();

                            }//!boolValue

                            tempAnim.AddClip(lighterSettings.DrawAnimation, lighterSettings.DrawAnimation.name);

                        }//DrawAnimation != null

                        if(lighterSettings.HideAnimation != null){

                            SerializedObject hideSerialize = new SerializedObject(lighterSettings.HideAnimation);

                            if(!hideSerialize.FindProperty("m_Legacy").boolValue){

                                hideSerialize.FindProperty("m_Legacy").boolValue = true;
                                hideSerialize.ApplyModifiedProperties();

                            }//!boolValue

                            tempAnim.AddClip(lighterSettings.HideAnimation, lighterSettings.HideAnimation.name);

                        }//HideAnimation != null

                        if(lighterSettings.IdleAnimation != null){

                            SerializedObject idleSerialize = new SerializedObject(lighterSettings.IdleAnimation);

                            if(!idleSerialize.FindProperty("m_Legacy").boolValue){

                                idleSerialize.FindProperty("m_Legacy").boolValue = true;
                                idleSerialize.ApplyModifiedProperties();

                            }//!boolValue

                            tempAnim.AddClip(lighterSettings.IdleAnimation, lighterSettings.IdleAnimation.name);

                        }//IdleAnimation != null

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[44].local);

                    }//Animation = null

                    HFPS.Systems.AnimationEvent tempAnimEvent = null;

                    if(armsParent.GetComponent<HFPS.Systems.AnimationEvent>() == null){

                        tempAnimEvent = armsParent.AddComponent<HFPS.Systems.AnimationEvent>();

                    //AnimationEvent = null
                    } else {

                        tempAnimEvent = armsParent.GetComponent<HFPS.Systems.AnimationEvent>();

                    }//AnimationEvent = null

                    if(tempAnimEvent != null){


    /////////////
    ///
    ///     OPEN
    ///
    /////////////


                        HFPS.Systems.AnimationEvent.AnimEvents tempEvent_LighterOpen = new HFPS.Systems.AnimationEvent.AnimEvents();
                        tempEvent_LighterOpen.EventCallName = "LighterOpen";
                        tempEvent_LighterOpen.CallEvent = new UnityEvent();

                        UnityAction methodDelegate_LighterOpen = System.Delegate.CreateDelegate (typeof(UnityAction), tempLighterItem, "LighterOpen_Event") as UnityAction;
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent_LighterOpen.CallEvent, methodDelegate_LighterOpen);


    /////////////
    ///
    ///     CLOSE
    ///
    /////////////


                        HFPS.Systems.AnimationEvent.AnimEvents tempEvent_LighterClose = new HFPS.Systems.AnimationEvent.AnimEvents();
                        tempEvent_LighterClose.EventCallName = "LighterClose";
                        tempEvent_LighterClose.CallEvent = new UnityEvent();

                        UnityAction methodDelegate_LighterClose = System.Delegate.CreateDelegate (typeof(UnityAction), tempLighterItem, "LighterClose_Event") as UnityAction;
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent_LighterClose.CallEvent, methodDelegate_LighterClose);


    /////////////
    ///
    ///     ON
    ///
    /////////////


                        HFPS.Systems.AnimationEvent.AnimEvents tempEvent_LighterOn = new HFPS.Systems.AnimationEvent.AnimEvents();
                        tempEvent_LighterOn.EventCallName = "LighterOn";
                        tempEvent_LighterOn.CallEvent = new UnityEvent();

                        UnityAction methodDelegate_LighterOn = System.Delegate.CreateDelegate (typeof(UnityAction), tempLighterItem, "LighterOn_Event") as UnityAction;
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent_LighterOn.CallEvent, methodDelegate_LighterOn);


    /////////////
    ///
    ///     OFF
    ///
    /////////////


                        HFPS.Systems.AnimationEvent.AnimEvents tempEvent_LighterOff = new HFPS.Systems.AnimationEvent.AnimEvents();
                        tempEvent_LighterOff.EventCallName = "LighterOff";
                        tempEvent_LighterOff.CallEvent = new UnityEvent();

                        UnityAction methodDelegate_LighterOff = System.Delegate.CreateDelegate (typeof(UnityAction), tempLighterItem, "LighterOff_Event") as UnityAction;
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent_LighterOff.CallEvent, methodDelegate_LighterOff);


    /////////////
    ///
    ///     BLOW OUT
    ///
    /////////////


                        HFPS.Systems.AnimationEvent.AnimEvents tempEvent_BlowOut = new HFPS.Systems.AnimationEvent.AnimEvents();
                        tempEvent_BlowOut.EventCallName = "BlowOut";
                        tempEvent_BlowOut.CallEvent = new UnityEvent();

                        UnityAction methodDelegate_BlowOut = System.Delegate.CreateDelegate (typeof(UnityAction), tempLighterItem, "BlowOut_Event") as UnityAction;
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent_BlowOut.CallEvent, methodDelegate_BlowOut);


    /////////////
    ///
    ///     ASSIGN
    ///
    /////////////


                        tempAnimEvent.AnimationEvents = new HFPS.Systems.AnimationEvent.AnimEvents[5];

                        tempAnimEvent.AnimationEvents[0] = tempEvent_LighterOpen;
                        tempAnimEvent.AnimationEvents[1] = tempEvent_LighterClose;
                        tempAnimEvent.AnimationEvents[2] = tempEvent_LighterOn;
                        tempAnimEvent.AnimationEvents[3] = tempEvent_LighterOff;
                        tempAnimEvent.AnimationEvents[4] = tempEvent_BlowOut;

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[50].local);
                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[51].local);
                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[52].local);
                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[53].local);
                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[48].local);

                    }//tempAnimEvent != null


                #else

                    //components not active
                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[2].text);

                #endif

            }//weaponType = lighter


    /////////////////////////
    ///
    ///     MELEE
    ///
    /////////////////////////


            if(weaponType == Weapon_Type.Melee){

                MeleeController tempMeleeCont = tempWeapon.AddComponent<MeleeController>();

                tempMeleeCont.MeleeGO = armsParent;

                tempMeleeCont.DrawSound = meleeSettings.DrawSound;
                tempMeleeCont.DrawVolume = meleeSettings.DrawVolume;

                tempMeleeCont.HideSound = meleeSettings.HideSound;
                tempMeleeCont.HideVolume = meleeSettings.HideVolume;

                tempMeleeCont.SwaySound = meleeSettings.SwaySound;
                tempMeleeCont.SwayVolume = meleeSettings.SwayVolume;

                if(meleeSettings.DrawAnim != null){

                    tempMeleeCont.DrawAnim = meleeSettings.DrawAnim.name;

                }//DrawAnim != null

                tempMeleeCont.DrawSpeed = meleeSettings.DrawSpeed;

                if(meleeSettings.HideAnim != null){

                    tempMeleeCont.HideAnim = meleeSettings.HideAnim.name;

                }//HideAnim != null

                tempMeleeCont.HideSpeed = meleeSettings.HideSpeed;

                if(meleeSettings.AttackAnim != null){

                    tempMeleeCont.AttackAnim = meleeSettings.AttackAnim.name;

                }//AttackAnim != null

                tempMeleeCont.AttackSpeed = meleeSettings.AttackSpeed;

                tempMeleeCont.MeleeItemID = meleeSettings.itemID;

                tempMeleeCont.surfaceID = meleeSettings.surfaceID;
                tempMeleeCont.surfaceDetails = meleeSettings.surfaceDetails;
                tempMeleeCont.defaultSurfaceID = meleeSettings.defaultSurfaceID;

                tempMeleeCont.HitLayer = meleeSettings.HitLayer;
                tempMeleeCont.HitDistance = meleeSettings.HitDistance;
                tempMeleeCont.HitForce = meleeSettings.HitForce;
                tempMeleeCont.HitWaitDelay = meleeSettings.HitWaitDelay;
                tempMeleeCont.AttackDamage = meleeSettings.AttackDamage;

                tempMeleeCont.SwayKickback = meleeSettings.SwayKickback;
                tempMeleeCont.SwaySpeed = meleeSettings.SwaySpeed;

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[42].local);

                if(armsParent.GetComponent<Animation>() == null){

                    Animation tempAnim = armsParent.AddComponent<Animation>();

                    tempAnim.playAutomatically = false;

                    if(meleeSettings.DrawAnim != null){

                        SerializedObject drawSerialize = new SerializedObject(meleeSettings.DrawAnim);

                        if(!drawSerialize.FindProperty("m_Legacy").boolValue){

                            drawSerialize.FindProperty("m_Legacy").boolValue = true;
                            drawSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(meleeSettings.DrawAnim, meleeSettings.DrawAnim.name);

                    }//DrawAnim != null

                    if(meleeSettings.HideAnim != null){

                        SerializedObject hideSerialize = new SerializedObject(meleeSettings.HideAnim);

                        if(!hideSerialize.FindProperty("m_Legacy").boolValue){

                            hideSerialize.FindProperty("m_Legacy").boolValue = true;
                            hideSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(meleeSettings.HideAnim, meleeSettings.HideAnim.name);

                    }//HideAnim != null

                    if(meleeSettings.AttackAnim != null){

                        SerializedObject attackSerialize = new SerializedObject(meleeSettings.AttackAnim);

                        if(!attackSerialize.FindProperty("m_Legacy").boolValue){

                            attackSerialize.FindProperty("m_Legacy").boolValue = true;
                            attackSerialize.ApplyModifiedProperties();

                        }//!boolValue

                        tempAnim.AddClip(meleeSettings.AttackAnim, meleeSettings.AttackAnim.name);

                    }//AttackAnim != null

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[44].local);

                }//Animation = null

                AudioSource tempAudSource = null;

                if(armsParent.GetComponent<AudioSource>() == null){

                    tempAudSource = armsParent.AddComponent<AudioSource>();

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[46].local);

                //AudioSource = null
                } else {

                    tempAudSource = armsParent.GetComponent<AudioSource>();

                }//AudioSource = null

                if(tempAudSource != null){

                    tempMeleeCont.audioSource = tempAudSource;

                    tempAudSource.playOnAwake = false;

                    if(tempMeleeCont.audioSource != null){

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[47].local);

                    }//audioSource != null

                }//tempAudSource != null

            }//weaponType = melee


    /////////////////////////
    ///
    ///     GUN
    ///
    /////////////////////////


            if(weaponType == Weapon_Type.Gun){

                WeaponController tempWeapCont = tempWeapon.AddComponent<WeaponController>();

                tempWeapCont.weaponType = gunSettings.weaponType;
                tempWeapCont.bulletType = gunSettings.bulletType;
                tempWeapCont.raycastMask = gunSettings.raycastMask;
                tempWeapCont.soundReactionMask = gunSettings.soundReactionMask;

                tempWeapCont.surfaceDetails = gunSettings.surfaceDetails;
                tempWeapCont.kickback = gunSettings.kickback;

                tempWeapCont.audioSettings.reloadSound = gunSettings.audioSettings.reloadSound;

                tempWeapCont.audioSettings.soundDraw = gunSettings.audioSettings.soundDraw;
                tempWeapCont.audioSettings.volumeDraw = gunSettings.audioSettings.volumeDraw;

                tempWeapCont.audioSettings.soundFire = gunSettings.audioSettings.soundFire;
                tempWeapCont.audioSettings.volumeFire = gunSettings.audioSettings.volumeFire;

                tempWeapCont.audioSettings.soundEmpty = gunSettings.audioSettings.soundEmpty;
                tempWeapCont.audioSettings.volumeEmpty = gunSettings.audioSettings.volumeEmpty;

                tempWeapCont.audioSettings.soundReload = gunSettings.audioSettings.soundReload;
                tempWeapCont.audioSettings.volumeReload = gunSettings.audioSettings.volumeReload;

                tempWeapCont.audioSettings.impactVolume = gunSettings.audioSettings.impactVolume;

                tempWeapCont.aimingSettings.enableAiming = gunSettings.aimingSettings.enableAiming;
                tempWeapCont.aimingSettings.steadyAim = gunSettings.aimingSettings.steadyAim;
                tempWeapCont.aimingSettings.aimPosition = gunSettings.aimingSettings.aimPosition;

                tempWeapCont.aimingSettings.aimSpeed = gunSettings.aimingSettings.aimSpeed;

                tempWeapCont.aimingSettings.zoomFOVSmooth = gunSettings.aimingSettings.zoomFOVSmooth;
                tempWeapCont.aimingSettings.unzoomFOVSmooth = gunSettings.aimingSettings.unzoomFOVSmooth;
                tempWeapCont.aimingSettings.zoomFOV = gunSettings.aimingSettings.zoomFOV;

                tempWeapCont.animationSettings.hideAnim = gunSettings.animationSettings.hideAnim;
                tempWeapCont.animationSettings.fireAnim = gunSettings.animationSettings.fireAnim;
                tempWeapCont.animationSettings.reloadAnim = gunSettings.animationSettings.reloadAnim;

                tempWeapCont.animationSettings.beforeReloadAnim = gunSettings.animationSettings.beforeReloadAnim;
                tempWeapCont.animationSettings.afterReloadAnim = gunSettings.animationSettings.afterReloadAnim;
                tempWeapCont.animationSettings.afterReloadEmptyAnim = gunSettings.animationSettings.afterReloadEmptyAnim;

                tempWeapCont.inventorySettings.weaponID = gunSettings.inventorySettings.weaponID;
                tempWeapCont.inventorySettings.bulletsID = gunSettings.inventorySettings.bulletsID;

                tempWeapCont.bulletModelSettings.barrelEndPosition = gunSettings.bulletModelSettings.barrelEndPosition;
                tempWeapCont.bulletModelSettings.bulletPrefab = gunSettings.bulletModelSettings.bulletPrefab;
                tempWeapCont.bulletModelSettings.bulletRotation = gunSettings.bulletModelSettings.bulletRotation;
                tempWeapCont.bulletModelSettings.bulletForce = gunSettings.bulletModelSettings.bulletForce;

                tempWeapCont.bulletSettings.FleshTag = gunSettings.bulletSettings.FleshTag;

                tempWeapCont.bulletSettings.bulletsInMag = gunSettings.bulletSettings.bulletsInMag;
                tempWeapCont.bulletSettings.bulletsPerMag = gunSettings.bulletSettings.bulletsPerMag;
                tempWeapCont.bulletSettings.bulletsPerShot = gunSettings.bulletSettings.bulletsPerShot;

                tempWeapCont.bulletSettings.keepReloadMagBullets = gunSettings.bulletSettings.keepReloadMagBullets;
                tempWeapCont.bulletSettings.soundOnImpact = gunSettings.bulletSettings.soundOnImpact;

                tempWeapCont.bulletSettings.surfaceID = gunSettings.bulletSettings.surfaceID;
                tempWeapCont.bulletSettings.defaultSurfaceID = gunSettings.bulletSettings.defaultSurfaceID;

                tempWeapCont.kickbackSettings.kickUp = gunSettings.kickbackSettings.kickUp;
                tempWeapCont.kickbackSettings.kickSideways = gunSettings.kickbackSettings.kickSideways;
                tempWeapCont.kickbackSettings.kickTime = gunSettings.kickbackSettings.kickTime;
                tempWeapCont.kickbackSettings.kickReturnSpeed = gunSettings.kickbackSettings.kickReturnSpeed;

                tempWeapCont.muzzleFlashSettings.enableMuzzleFlash = gunSettings.muzzleFlashSettings.enableMuzzleFlash;
                tempWeapCont.muzzleFlashSettings.muzzleRotation = gunSettings.muzzleFlashSettings.muzzleRotation;

                tempWeapCont.muzzleFlashSettings.muzzleFlash = gunSettings.muzzleFlashSettings.muzzleFlash;
                tempWeapCont.muzzleFlashSettings.muzzleLight = gunSettings.muzzleFlashSettings.muzzleLight;

                tempWeapCont.weaponSettings.weaponDamage = gunSettings.weaponSettings.weaponDamage;
                tempWeapCont.weaponSettings.shootRange = gunSettings.weaponSettings.shootRange;
                tempWeapCont.weaponSettings.hitforce = gunSettings.weaponSettings.hitforce;
                tempWeapCont.weaponSettings.fireRate = gunSettings.weaponSettings.fireRate;
                tempWeapCont.weaponSettings.recoil = gunSettings.weaponSettings.recoil;

                tempWeapCont.npcReactionSettings.enableSoundReaction = gunSettings.npcReactionSettings.enableSoundReaction;
                tempWeapCont.npcReactionSettings.soundReactionRadius = gunSettings.npcReactionSettings.soundReactionRadius;

                #if (HFPS_163a || HFPS_163b)

                    if(gunSettings.weaponType == WeaponController.WeaponType.Shotgun){

                        tempWeapCont.shotgunSettings.ejectPosition = gunSettings.shotgunSettings.ejectPosition;
                        tempWeapCont.shotgunSettings.shellPrefab = gunSettings.shotgunSettings.shellPrefab;
                        tempWeapCont.shotgunSettings.shellRotation = gunSettings.shotgunSettings.shellRotation;
                        tempWeapCont.shotgunSettings.ejectSpeed = gunSettings.shotgunSettings.ejectSpeed;

                    }//weaponType = shotgun

                #endif

                #if HFPS_163c

                    tempWeapCont.bulletSettings.ejectShells = gunSettings.bulletSettings.ejectShells;

                    tempWeapCont.shellEjectSettings.ejectPosition = gunSettings.shellEjectSettings.ejectPosition;
                    tempWeapCont.shellEjectSettings.shellPrefab = gunSettings.shellEjectSettings.shellPrefab;
                    tempWeapCont.shellEjectSettings.shellRotation = gunSettings.shellEjectSettings.shellRotation;
                    tempWeapCont.shellEjectSettings.ejectSpeed = gunSettings.shellEjectSettings.ejectSpeed;
                    tempWeapCont.shellEjectSettings.ejectAutomatiacally = gunSettings.shellEjectSettings.ejectAutomatiacally;

                #endif

                Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[43].local);

                if(gunSettings.weaponType == WeaponController.WeaponType.Shotgun){

                    HFPS.Systems.AnimationEvent tempAnimEvent = null;

                    if(armsParent.GetComponent<HFPS.Systems.AnimationEvent>() == null){

                        tempAnimEvent = armsParent.AddComponent<HFPS.Systems.AnimationEvent>();

                    //AnimationEvent = null
                    } else {

                        tempAnimEvent = armsParent.GetComponent<HFPS.Systems.AnimationEvent>();

                    }//AnimationEvent = null

                    if(tempAnimEvent != null){

                        HFPS.Systems.AnimationEvent.AnimEvents tempEvent = new HFPS.Systems.AnimationEvent.AnimEvents();
                        tempEvent.EventCallName = "eject";
                        tempEvent.CallEvent = new UnityEvent();

                        UnityAction methodDelegate = System.Delegate.CreateDelegate (typeof(UnityAction), tempWeapCont, "EjectShell") as UnityAction;
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(tempEvent.CallEvent, methodDelegate);

                        tempAnimEvent.AnimationEvents = new HFPS.Systems.AnimationEvent.AnimEvents[1];
                        tempAnimEvent.AnimationEvents[0] = tempEvent;

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[49].local);

                    }//tempAnimEvent != null

                }//weaponType = shotgun

                HFPS.Systems.AnimationSoundEvents tempAnimSoundEvent = null;

                if(armsParent.GetComponent<HFPS.Systems.AnimationSoundEvents>() == null){

                    tempAnimSoundEvent = armsParent.AddComponent<HFPS.Systems.AnimationSoundEvents>();

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[45].local);

                //AnimationEvent = null
                } else {

                    tempAnimSoundEvent = armsParent.GetComponent<HFPS.Systems.AnimationSoundEvents>();

                }//AnimationEvent = null

                if(tempAnimSoundEvent != null){

                    tempAnimSoundEvent.soundVolume = 1f;

                    if(template != null){

                        if(template.animationSoundEvents.Count > 0){

                            for(int ase = 0; ase < template.animationSoundEvents.Count; ase++){

                                tempAnimSoundEvent.soundEvents.Add(template.animationSoundEvents[ase]);

                            }//for ase animationSoundEvents

                        }//animationSoundEvents.Count > 0

                    }//template != null

                }//tempAnimSoundEvent != null

                AudioSource tempAudSource = null;

                if(armsParent.GetComponent<AudioSource>() == null){

                    tempAudSource = armsParent.AddComponent<AudioSource>();

                    Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[46].local);

                //AudioSource = null
                } else {

                    tempAudSource = armsParent.GetComponent<AudioSource>();

                }//AudioSource = null

                if(tempAudSource != null){

                    tempWeapCont.audioSource = tempAudSource;

                    tempAudSource.playOnAwake = false;

                    if(tempWeapCont.audioSource != null){

                        Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[47].local);

                    }//audioSource != null

                }//tempAudSource != null

            }//weaponType = gun


        }//Weapon_Create


    //////////////////////////////////////
    ///
    ///     LANGUAGE ACTIONS
    ///
    //////////////////////////////////////


        public static void DM_LocDataFind(){

            if(dmMenusLocData == null){

                //Debug.Log("Find Start");

                //AssetDatabase.Refresh();

                string[] results;
                DM_MenusLocData tempMenusLocData = ScriptableObject.CreateInstance<DM_MenusLocData>();

                results = AssetDatabase.FindAssets(menusLocDataName);

                if(results.Length > 0){

                    foreach(string guid in results){

                        if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                            tempMenusLocData = AssetDatabase.LoadAssetAtPath<DM_MenusLocData>(AssetDatabase.GUIDToAssetPath(guid));

                            if(tempMenusLocData != null){

                                dmMenusLocData = tempMenusLocData;

                                if(dmMenusLocData != null){

                                    if(!languageLock){

                                        languageLock = true;

                                        Language_Check();

                                    }//!languageLock

                                }//dmMenusLocData != null

                            }//tempMenusLocData != null

                            //Debug.Log("Menus Loc Data Found");

                        }//file.exists

                    }//foreach guid

                }//results.Length > 0

            //dmMenusLocData = null
            } else {

                if(!languageLock){

                    languageLock = true;

                    language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

                }//!languageLock

            }//dmMenusLocData = null

        }//DM_LocDataFind

        public static void Language_Check(){

            if(dmMenusLocData != null){

                for(int d = 0; d < dmMenusLocData.dictionary.Count; d++){

                    if(dmMenusLocData.dictionary[d].asset == "Weapon Creator"){

                        menusLocDataSlot = d;

                        //Debug.Log("Loc Data Slot = " + menusLocDataSlot);

                    }//asset = IWC

                }//for d dictionary

                language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;

            }//dmMenusLocData != null

        }//Language_Check

        public void Language_Save(){

            if(dmMenusLocData != null){

                if((int)dmMenusLocData.currentLanguage != (int)language){

                    dmMenusLocData.currentLanguage = (DM_InternEnums.Language)(int)language;

                }//currentLanguage != language

            }//dmMenusLocData != null

            Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[1].text);

        }//Language_Save


    //////////////////////////////////////
    ///
    ///     VERSION ACTIONS
    ///
    //////////////////////////////////////


        public static void Version_FindStatic(){

            if(!versionCheckStatic){

                versionCheckStatic = true;

                AssetDatabase.Refresh();

                string[] results;
                DM_Version tempVersion = ScriptableObject.CreateInstance<DM_Version>();

                results = AssetDatabase.FindAssets(versionName);

                if(results.Length > 0){

                    foreach(string guid in results){

                        if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                            tempVersion = AssetDatabase.LoadAssetAtPath<DM_Version>(AssetDatabase.GUIDToAssetPath(guid));

                            if(tempVersion != null){

                                dmVersion = tempVersion;
                                verNumb = dmVersion.version;

                                window = GetWindow<HFPS_WeaponCreator>(false, "Weapon Creator" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Weapon Creator Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<HFPS_WeaponCreator>(false, "Weapon Creator " + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Weapon Creator Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Weapon Creator Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<HFPS_WeaponCreator>(false, "Weapon Creator " + verNumb, true);
                    window.maxSize = window.minSize = windowsSize;

                }//results.Length > 0

            }//!versionCheckStatic

        }//Version_FindStatic


    //////////////////////////////////////
    ///
    ///     EXTRAS
    ///
    ///////////////////////////////////////


        private void OnDestroy() {

            window = null;
            verNumb = "";

        }//OnDestroy


    }//HFPS_WeaponCreator
    
    
}//namespace

#endif
