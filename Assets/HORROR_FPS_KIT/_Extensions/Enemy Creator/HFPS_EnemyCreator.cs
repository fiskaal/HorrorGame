#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.IO;
using UnityEngine.AI;

using DizzyMedia.Version;

using HFPS.Systems;

namespace DizzyMedia.Extension {

    public class HFPS_EnemyCreator : EditorWindow {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        private static HFPS_EnemyCreator window;
        private static Vector2 windowsSize = new Vector2(400, 600);

        public GameObject character;

        public DM_EnemyCreator_Template template;

        private static DM_Version dmVersion;
        private static string versionName = "EnemyCreator Version";
        private static string verNumb = "";
        private static bool versionCheckStatic = false;

        public static DM_InternEnums.Language language;
        private static DM_MenusLocData dmMenusLocData;
        private static string menusLocDataName = "DM_M_Data";
        private static int menusLocDataSlot;
        private static bool languageLock = false;

        Vector2 scrollPos;
        Animator charAnim;
        Transform pelvis;
        Collider tempCollider;
        bool animChecked;

        bool animUpdated;
        bool capsuleAdded;
        bool audSourceAdded;
        bool navMeshAdded;
        bool healthAdded;
        bool stepsAdded;
        bool aiAdded;
        bool parentUpdated;
        bool waysAdded;

        bool enemyCreated;

        private int tabs;


    //////////////////////////////////////
    ///
    ///     EDITOR WINDOW
    ///
    ///////////////////////////////////////


        [MenuItem("Tools/Dizzy Media/Extensions/HFPS/HFPS Enemy Creator", false , 13)]
        public static void OpenWizard() {

            if(dmVersion == null){

                versionCheckStatic = false;
                Version_FindStatic();

            //dmVersion == null
            } else {

                verNumb = dmVersion.version;

                window = GetWindow<HFPS_EnemyCreator>(false, "Enemy Creator" + " v" + verNumb, true);
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

            EnemyCreator_Screen();

        }//OnGUI


    //////////////////////////////////////
    ///
    ///     EDITOR DISPLAY
    ///
    ///////////////////////////////////////


        public void EnemyCreator_Screen(){

            GUI.skin.button.alignment = TextAnchor.MiddleCenter;

            Texture t0 = (Texture)Resources.Load("EditorContent/EnemyCreator/EnemyCreator_Header");

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));

            EditorGUI.BeginChangeCheck();

            ScriptableObject target = this;
            SerializedObject soTar = new SerializedObject(target);

            SerializedProperty characterRef = soTar.FindProperty("character");
            SerializedProperty templateRef = soTar.FindProperty("template");

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

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(characterRef, true);
                    EditorGUILayout.PropertyField(templateRef, true);

                    if(character != null){

                        if(!animChecked){

                            if(charAnim == null){

                                if(character.GetComponent<Animator>() != null){

                                    charAnim = character.GetComponent<Animator>();
                                    pelvis = charAnim.GetBoneTransform(HumanBodyBones.Hips);

                                    if(pelvis != null){ 

                                        tempCollider = pelvis.GetComponent<Collider>();

                                    }//pelvis != null

                                    animChecked = true;

                                //animator != null
                                } else {

                                    animChecked = true;

                                }//animator != null

                            }//charAnim == null

                        //!animChecked
                        } else {

                            if(charAnim != null){

                                EditorGUILayout.Space();

                                if(tempCollider != null){

                                    EditorGUILayout.HelpBox(character.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local, MessageType.Info);

                                //tempCollider != null
                                } else {

                                    EditorGUILayout.HelpBox(character.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local, MessageType.Warning);

                                    EditorGUILayout.Space();

                                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                                        Launch_RagdollCreator();

                                    }//button

                                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                                        Ragdoll_Reset();

                                    }//button

                                }//tempCollider != null

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                if(animUpdated){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local, MessageType.Info);

                                }//animUpdated

                                if(capsuleAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local, MessageType.Info);

                                }//capsuleAdded

                                if(audSourceAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[7].local, MessageType.Info);

                                }//audSourceAdded

                                if(navMeshAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[8].local, MessageType.Info);

                                }//navMeshAdded

                                if(healthAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[9].local, MessageType.Info);

                                }//healthAdded

                                if(stepsAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[10].local, MessageType.Info);

                                }//stepsAdded

                                if(aiAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[11].local, MessageType.Info);

                                }//aiAdded 

                                if(parentUpdated){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[12].local, MessageType.Info);

                                }//parentUpdated

                                if(waysAdded){

                                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[13].local, MessageType.Info);

                                }//waysAdded 

                                EditorGUILayout.EndScrollView();

                            //charAnim != null
                            } else {

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox(character.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local, MessageType.Error);

                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                                EditorGUILayout.EndScrollView();

                            }//charAnim != null

                        }//!animChecked

                    //character != null
                    } else {

                        Ragdoll_Reset();

                        animUpdated = false;
                        capsuleAdded = false;
                        audSourceAdded = false;
                        navMeshAdded = false;
                        healthAdded = false;
                        stepsAdded = false;
                        aiAdded = false;
                        parentUpdated = false;
                        waysAdded = false;

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, MessageType.Error);

                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local, MessageType.Warning);

                        EditorGUILayout.EndScrollView();

                    }//character != null

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    if(character != null){

                        GUI.enabled = true;

                    //character != null
                    } else {

                        GUI.enabled = false;

                    }//character != null

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)){

                        Character_Create();

                    }//button

                    if(enemyCreated){

                        GUI.enabled = true;

                    //enemyCreated
                    } else {

                        GUI.enabled = false;

                    }//enemyCreated

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[4].local)){

                        Notifications_Reset();

                    }//button

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

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

        }//EnemyCreator_Screen


    //////////////////////////////////////
    ///
    ///     LAUNCH ACTIONS
    ///
    ///////////////////////////////////////


        public void Launch_RagdollCreator(){

            DM_RagdollCreator window = (DM_RagdollCreator)EditorWindow.GetWindow<DM_RagdollCreator>(false, "Ragdoll Creator", true);
            window.OpenWizard_Single();

        }//Launch_RagdollCreator


    //////////////////////////////////////
    ///
    ///     EDITOR ACTIONS
    ///
    //////////////////////////////////////


        public void Ragdoll_Reset(){

            if(charAnim != null){

                charAnim = null;

            }//charAnim != null

            if(animChecked){

                animChecked = false;

            }//animChecked

            if(pelvis != null){

                pelvis = null;

            }//pelvis != null

            if(tempCollider != null){

                tempCollider = null;

            }//tempCollider != null

        }//Ragdoll_Reset

        public void Notifications_Reset(){

            animUpdated = false;
            capsuleAdded = false;
            audSourceAdded = false;
            navMeshAdded = false;
            healthAdded = false;
            stepsAdded = false;
            aiAdded = false;
            parentUpdated = false;
            waysAdded = false;

            enemyCreated = false;

        }//Notifications_Reset

        public void Character_Create(){

            GameObject footCatch = null;

            if(character != null){


    //////////////////////////
    ///     FOOTSTEPS AUDIO
    //////////////////////////


                if(template.footsteps != null){

                    GameObject tempFoots = Instantiate(template.footsteps.footsteps, character.transform);
                    footCatch = tempFoots;

                    tempFoots.name = "Footsteps";

                }//footsteps != null


    //////////////////////////
    ///     LAYERS
    //////////////////////////


                character.layer = template.general.enemyLayer;

                foreach(Transform child in character.transform.GetComponentsInChildren<Transform>()){

                    if(child != pelvis){

                        child.gameObject.layer = template.general.enemyLayer;

                    }//child != pelvis

                }//foreach child

                foreach(Transform child2 in pelvis.GetComponentsInChildren<Transform>()){

                    child2.gameObject.layer = template.general.bodyLayer;

                }//foreach child2


    //////////////////////////
    ///     ANIMATOR
    //////////////////////////


                charAnim.runtimeAnimatorController = template.general.enemyAnim;
                charAnim.cullingMode = UnityEngine.AnimatorCullingMode.CullUpdateTransforms;

                animUpdated = true;


    //////////////////////////
    ///     CAPSULE
    //////////////////////////


                CapsuleCollider tempBodyCol = character.AddComponent<CapsuleCollider>();

                tempBodyCol.center = template.capsule.center;
                tempBodyCol.radius = template.capsule.radius;
                tempBodyCol.height = template.capsule.height;

                tempBodyCol.direction = (int)template.capsule.direction;

                capsuleAdded = true;


    //////////////////////////
    ///     AUDIO SOURCE
    //////////////////////////


                AudioSource tempBodyAud = character.AddComponent<AudioSource>();

                tempBodyAud.playOnAwake = false;
                tempBodyAud.spatialBlend = 1;
                tempBodyAud.rolloffMode = AudioRolloffMode.Custom;
                tempBodyAud.maxDistance = 30;

                audSourceAdded = true;


    //////////////////////////
    ///     NAV MESH AGENT
    //////////////////////////


                NavMeshAgent tempNav = character.AddComponent<NavMeshAgent>();

                tempNav.baseOffset = template.navMesh.baseOffset;
                tempNav.speed = template.navMesh.speed;
                tempNav.angularSpeed = template.navMesh.angularSpeed;
                tempNav.acceleration = template.navMesh.acceleration;
                tempNav.stoppingDistance = template.navMesh.stoppingDistance;
                tempNav.autoBraking = template.navMesh.autoBraking;
                tempNav.radius = template.navMesh.navRadius;
                tempNav.height = template.navMesh.navHeight;
                tempNav.obstacleAvoidanceType = template.navMesh.quality;
                tempNav.avoidancePriority = template.navMesh.priority;
                tempNav.autoTraverseOffMeshLink = template.navMesh.autoTraverseOffMeshLink;
                tempNav.autoRepath = template.navMesh.autoRepath;
                tempNav.areaMask = (int)template.navMesh.areaMask;

                navMeshAdded = true;


    //////////////////////////
    ///     HEALTH
    //////////////////////////


                NPCHealth tempHealth = character.AddComponent<NPCHealth>();

                tempHealth.Hips = pelvis;
                tempHealth.FleshTag = template.health.fleshTag;
                tempHealth.Health = template.health.health;
                tempHealth.MaxHealth = template.health.maxHealth;
                tempHealth.HtAudio = template.health.hitAudio;
                tempHealth.disableHealth = template.health.disableHealth;
                tempHealth.head = charAnim.GetBoneTransform(HumanBodyBones.Head).GetComponent<Collider>();
                tempHealth.allowHeadshot = template.health.allowHeadshot;
                tempHealth.headshotDamage = template.health.headshotDamage;

                tempHealth.corpseRemoveType = template.health.corpseRemoveType;
                tempHealth.corpseTime = template.health.corpseTime;

                healthAdded = true;


    //////////////////////////
    ///     FOOTSTEPS 
    //////////////////////////


                NPCFootsteps tempStepsComp = character.AddComponent<NPCFootsteps>();

                tempStepsComp.footstepsMask = template.footsteps.footstepsMask;
                tempStepsComp.footstepsAudio = footCatch.GetComponent<AudioSource>();
                tempStepsComp.npcFootsteps = template.footsteps.npcFootsteps;
                tempStepsComp.defaultFootsteps = template.footsteps.defaultFootsteps;
                tempStepsComp.groundRay = template.footsteps.groundRay;
                tempStepsComp.allowDefaultFootsteps = template.footsteps.allowDefaultFootsteps;
                tempStepsComp.eventBasedFootsteps = template.footsteps.eventBasedFootsteps;
                tempStepsComp.walkVelocity = template.footsteps.walkVelocity;
                tempStepsComp.volumeWalk = template.footsteps.volumeWalk;
                tempStepsComp.volumeRun = template.footsteps.volumeRun;
                tempStepsComp.walkNextWait = template.footsteps.walkNextWait;
                tempStepsComp.runNextWait = template.footsteps.runNextWait;

                stepsAdded = true;


    //////////////////////////
    ///     ZOMBIE AI
    //////////////////////////

    //////////////////
    ///     GENERAL
    //////////////////

                ZombieBehaviourAI tempZombAI = character.AddComponent<ZombieBehaviourAI>();

                tempZombAI.sleepBehaviour = template.zombieAI.sleepBehaviour;
                tempZombAI.zombieBehaviour = template.zombieAI.zombieBehaviour;
                tempZombAI.animator = charAnim;
                tempZombAI.sightMask = template.zombieAI.sightMask;
                tempZombAI.threatMask = template.zombieAI.threatMask;


    //////////////////
    ///     BEHAVIOUR SETTINGS
    //////////////////


                tempZombAI.behaviourSettings.enableScream = template.zombieAI.behaviourSettings.enableScream;
                tempZombAI.behaviourSettings.enableAgony = template.zombieAI.behaviourSettings.enableAgony;
                tempZombAI.behaviourSettings.enableHunger = template.zombieAI.behaviourSettings.enableHunger;
                tempZombAI.behaviourSettings.soundReaction = template.zombieAI.behaviourSettings.soundReaction;
                tempZombAI.behaviourSettings.runToThreat = template.zombieAI.behaviourSettings.runToThreat;
                tempZombAI.behaviourSettings.attackOthers = template.zombieAI.behaviourSettings.attackOthers;
                tempZombAI.behaviourSettings.hungerRecoverHealth = template.zombieAI.behaviourSettings.hungerRecoverHealth;
                tempZombAI.behaviourSettings.randomWaypoint = template.zombieAI.behaviourSettings.randomWaypoint;
                tempZombAI.behaviourSettings.waypointsReassign = template.zombieAI.behaviourSettings.waypointsReassign;
                tempZombAI.behaviourSettings.lowHPIgnorePlayer = template.zombieAI.behaviourSettings.lowHPIgnorePlayer;
                tempZombAI.behaviourSettings.playerInvisible = template.zombieAI.behaviourSettings.playerInvisible;
                tempZombAI.behaviourSettings.damageThreat = template.zombieAI.behaviourSettings.damageThreat;
                tempZombAI.behaviourSettings.wakeByThreat = template.zombieAI.behaviourSettings.wakeByThreat;

                tempZombAI.behaviourSettings.patrolTime = template.zombieAI.behaviourSettings.patrolTime;
                tempZombAI.behaviourSettings.screamNext = template.zombieAI.behaviourSettings.screamNext;
                tempZombAI.behaviourSettings.agonyNext = template.zombieAI.behaviourSettings.agonyNext;

                tempZombAI.behaviourSettings.playerLostPatrol = template.zombieAI.behaviourSettings.playerLostPatrol;
                tempZombAI.behaviourSettings.hungerTime = template.zombieAI.behaviourSettings.hungerTime;


    //////////////////
    ///     AI SETTINGS
    //////////////////


                tempZombAI.aiSettings.walkSpeed = template.zombieAI.aiSettings.walkSpeed;
                tempZombAI.aiSettings.runSpeed = template.zombieAI.aiSettings.runSpeed;
                tempZombAI.aiSettings.eatingStoppingDist = template.zombieAI.aiSettings.eatingStoppingDist;
                tempZombAI.aiSettings.agentRotationSpeed = template.zombieAI.aiSettings.agentRotationSpeed;
                tempZombAI.aiSettings.speedChangeSpeed = template.zombieAI.aiSettings.speedChangeSpeed;
                tempZombAI.aiSettings.rotateManually = template.zombieAI.aiSettings.rotateManually;
                tempZombAI.aiSettings.accelerateManually = template.zombieAI.aiSettings.accelerateManually;
                tempZombAI.aiSettings.walkRootMotion = template.zombieAI.aiSettings.walkRootMotion;


    //////////////////
    ///     DAMAGE SETTINGS
    //////////////////


                tempZombAI.damageSettings.damageValue = template.zombieAI.damageSettings.damageValue;
                tempZombAI.damageSettings.damageKickback = template.zombieAI.damageSettings.damageKickback;
                tempZombAI.damageSettings.kickbackTime = template.zombieAI.damageSettings.kickbackTime;


    //////////////////
    ///     SENSOR SETTINGS
    //////////////////


                tempZombAI.sensorSettings.headOffset = template.zombieAI.sensorSettings.headOffset;
                tempZombAI.sensorSettings.reactionAngleTurn = template.zombieAI.sensorSettings.reactionAngleTurn;
                tempZombAI.sensorSettings.soundReactRange = template.zombieAI.sensorSettings.soundReactRange;
                tempZombAI.sensorSettings.sightsFOV = template.zombieAI.sensorSettings.sightsFOV;
                tempZombAI.sensorSettings.attackFOV = template.zombieAI.sensorSettings.attackFOV;

                tempZombAI.sensorSettings.sightsDistance = template.zombieAI.sensorSettings.sightsDistance;
                tempZombAI.sensorSettings.attackDistance = template.zombieAI.sensorSettings.attackDistance;
                tempZombAI.sensorSettings.idleHearRange = template.zombieAI.sensorSettings.idleHearRange;
                tempZombAI.sensorSettings.veryCloseRange = template.zombieAI.sensorSettings.veryCloseRange;
                tempZombAI.sensorSettings.chaseTimeHide = template.zombieAI.sensorSettings.chaseTimeHide;
                tempZombAI.sensorSettings.threatRadius = template.zombieAI.sensorSettings.threatRadius;
                tempZombAI.sensorSettings.lowHP = template.zombieAI.sensorSettings.lowHP;


    //////////////////
    ///     NPC SOUNDS
    //////////////////


                tempZombAI.m_NPCSounds = new ZombieBehaviourAI.NPCSounds();

                tempZombAI.m_NPCSounds.ScreamSound = template.zombieAI.npcSounds.ScreamSound;
                tempZombAI.m_NPCSounds.EatingSound = template.zombieAI.npcSounds.EatingSound;
                tempZombAI.m_NPCSounds.AgonizeSound = template.zombieAI.npcSounds.AgonizeSound;
                tempZombAI.m_NPCSounds.TakeDamageSound = template.zombieAI.npcSounds.TakeDamageSound;
                tempZombAI.m_NPCSounds.DieSound = template.zombieAI.npcSounds.DieSound;

                tempZombAI.m_NPCSounds.IdleSounds = new AudioClip[template.zombieAI.npcSounds.IdleSounds.Length];

                for(int id = 0; id < tempZombAI.m_NPCSounds.IdleSounds.Length; id++){

                    tempZombAI.m_NPCSounds.IdleSounds[id] = template.zombieAI.npcSounds.IdleSounds[id];

                }//for id idle sounds

                tempZombAI.m_NPCSounds.ReactionSounds = new AudioClip[template.zombieAI.npcSounds.ReactionSounds.Length];

                for(int r = 0; r < tempZombAI.m_NPCSounds.ReactionSounds.Length; r++){

                    tempZombAI.m_NPCSounds.ReactionSounds[r] = template.zombieAI.npcSounds.ReactionSounds[r];

                }//for r reaction sounds 

                tempZombAI.m_NPCSounds.AttackSounds = new AudioClip[template.zombieAI.npcSounds.AttackSounds.Length];

                for(int a = 0; a < tempZombAI.m_NPCSounds.AttackSounds.Length; a++){

                    tempZombAI.m_NPCSounds.AttackSounds[a] = template.zombieAI.npcSounds.AttackSounds[a];

                }//for a attack sounds 


    //////////////////
    ///     NPC SOUNDS VOLUME
    //////////////////


                tempZombAI.m_NPCSoundsVolume = new ZombieBehaviourAI.NPCSoundsVolume();

                tempZombAI.m_NPCSoundsVolume.ScreamVolume = template.zombieAI.npcSoundsVolume.ScreamVolume;
                tempZombAI.m_NPCSoundsVolume.EatingVolume = template.zombieAI.npcSoundsVolume.EatingVolume;
                tempZombAI.m_NPCSoundsVolume.AgonizeVolume = template.zombieAI.npcSoundsVolume.AgonizeVolume;
                tempZombAI.m_NPCSoundsVolume.TakeDamageVolume = template.zombieAI.npcSoundsVolume.TakeDamageVolume;
                tempZombAI.m_NPCSoundsVolume.DieVolume = template.zombieAI.npcSoundsVolume.DieVolume;
                tempZombAI.m_NPCSoundsVolume.IdleVolume = template.zombieAI.npcSoundsVolume.IdleVolume;
                tempZombAI.m_NPCSoundsVolume.ReactionVolume = template.zombieAI.npcSoundsVolume.ReactionVolume;
                tempZombAI.m_NPCSoundsVolume.AttackVolume = template.zombieAI.npcSoundsVolume.AttackVolume;


    //////////////////
    ///     EVENT OPTIONS
    //////////////////


                tempZombAI.playAttractedSounds = template.zombieAI.playAttractedSounds;
                tempZombAI.eventPlayAttackSound = template.zombieAI.eventPlayAttackSound;
                tempZombAI.eventPlayScreamSound = template.zombieAI.eventPlayScreamSound;
                tempZombAI.eventPlayAgonySound = template.zombieAI.eventPlayAgonySound;
                tempZombAI.eventPlayEatSound = template.zombieAI.eventPlayEatSound;

                aiAdded = true;


    //////////////////////////
    ///     PARENT
    //////////////////////////


                GameObject newParent = new GameObject("Enemy AI (" + character.name + ")");

                newParent.transform.parent = character.transform;
                newParent.transform.localPosition = new Vector3(0, 0, 0);
                newParent.transform.localEulerAngles = new Vector3(0, 0, 0);
                newParent.transform.parent = null;
                newParent.transform.position = new Vector3(newParent.transform.localPosition.x, newParent.transform.localPosition.y + 1.4f, newParent.transform.localPosition.z);

                character.transform.parent = newParent.transform;

                parentUpdated = true;


    //////////////////////////
    ///     WAYPOINTS
    //////////////////////////


                if(template.general.waypoints != null){

                    GameObject newWays = Instantiate(template.general.waypoints, newParent.transform);
                    newWays.name = template.general.waypoints.name;
                    newWays.transform.localPosition = new Vector3(0, -1.4f, 0);

                    waysAdded = true;

                }//waypoints != null


    //////////////////////////
    ///     FINISH
    //////////////////////////


                enemyCreated = true;


            }//character != null

        }//Character_Create


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

                    if(dmMenusLocData.dictionary[d].asset == "Enemy Creator"){

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

            Debug.Log("Language Saved");

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

                                window = GetWindow<HFPS_EnemyCreator>(false, "Enemy Creator" + " v" + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Enemy Creator Version found");

                            //tempVersion != null
                            } else {

                                if(verNumb == ""){

                                    verNumb = "Unknown";

                                }//verNumb = null

                                window = GetWindow<HFPS_EnemyCreator>(false, "Enemy Creator " + verNumb, true);
                                window.maxSize = window.minSize = windowsSize;

                                //Debug.Log("Enemy Creator Version NOT found");

                            }//tempVersion != null

                        //Exists
                        } else {

                            //Debug.Log("Enemy Creator Version NOT found"); 

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    verNumb = "Unknown";

                    window = GetWindow<HFPS_EnemyCreator>(false, "Enemy Creator " + verNumb, true);
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


    }//HFPS_EnemyCreator
    
    
}//namespace

#endif
