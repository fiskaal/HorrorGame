#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine.AI;
using System;

using HFPS.Systems;

namespace DizzyMedia.Extension {

    [CreateAssetMenu(fileName = "New Enemy Template", menuName = "Dizzy Media/Extensions/Enemy Creator/Scriptables/Template/New Enemy Template", order = 1)]
    public class DM_EnemyCreator_Template : ScriptableObject {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [System.Serializable]
        public class General {

            [Space]

            [Layer] public int enemyLayer = 8;
            [Layer] public int bodyLayer = 10;

            [Space]

            public AnimatorController enemyAnim;
            public GameObject waypoints;

        }//General

        [System.Serializable]
        public class Capsule {

            [Space]

            public Vector3 center = new Vector3(0, 1.05f, 0);
            public float radius = 0.3f;
            public float height = 2;
            public Direction direction = Direction.YAxis;

        }//Capsule

        [System.Serializable]
        public class NavMesh {

            [Space]

            public float baseOffset = 0.01f;
            public float speed;
            public float angularSpeed = 270;
            public float acceleration = 50;
            public float stoppingDistance = 1.5f;
            public bool autoBraking = true  ;

            [Space]

            public float navRadius = 0.5f;
            public float navHeight = 2;

            [Space]

            public ObstacleAvoidanceType quality = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            public int priority = 50;

            [Space]

            public bool autoTraverseOffMeshLink = true;
            public bool autoRepath = true;
            public NavMeshAreas areaMask;

        }//NavMesh

        [System.Serializable]
        public class Health {

            [Space]

            public string fleshTag = "Flesh";
            public int health = 100;
            public int maxHealth = 200;
            public AudioClip hitAudio;
            public bool disableHealth;

            [Space]

            public bool allowHeadshot;
            public int headshotDamage = 35;

            [Space]

            public NPCHealth.CorpseRemoveType corpseRemoveType;
            public float corpseTime = 10;

        }//Health 

        [System.Serializable]
        public class Footsteps {

            [Space]

            public GameObject footsteps;

            public LayerMask footstepsMask;
            public NPCFootsteps.Footstep[] npcFootsteps;
            public AudioClip[] defaultFootsteps;

            [Space]

            public float groundRay = 0.4f;
            public bool allowDefaultFootsteps = true;
            public bool eventBasedFootsteps = true;
            public float walkVelocity = 1.2f;
            public float volumeWalk = 1;
            public float volumeRun = 1.5f;
            public float walkNextWait = 2;
            public float runNextWait = 0.3f;

        }//Footsteps

        [System.Serializable]
        public class ZombieAI {

            [Space]

            public ZombieBehaviourAI.StartingSleep sleepBehaviour;
            public ZombieBehaviourAI.GeneralBehaviour zombieBehaviour;

            [Space]

            public LayerMask sightMask;
            public LayerMask threatMask;

            public ZombieBehaviourAI.BehaviourSettings behaviourSettings;
            public ZombieBehaviourAI.AISettings aiSettings;
            public ZombieBehaviourAI.DamageSettings damageSettings;
            public ZombieBehaviourAI.SensorSettings sensorSettings;

            [Space]

            public ZombieBehaviourAI.NPCSounds npcSounds;
            public ZombieBehaviourAI.NPCSoundsVolume npcSoundsVolume;

            [Space]

            public bool playAttractedSounds = true;
            public bool eventPlayAttackSound = true;
            public bool eventPlayScreamSound;
            public bool eventPlayAgonySound;
            public bool eventPlayEatSound;

        }//ZombieAI


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Direction {

            XAxis = 0,
            YAxis = 1,
            ZAxis = 2,

        }//Direction

        [Flags]
        public enum NavMeshAreas {

            None = 0,              
            Walkable = 1, 
            NotWalkable = 2, 
            Jump = 4,
            All = ~0,

        }//NavMeshAreas


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public General general;
        public Capsule capsule;
        public NavMesh navMesh;
        public Health health;
        public Footsteps footsteps;
        public ZombieAI zombieAI;


    }//DM_EnemyCreator_Template
    

}//namespace

#endif
