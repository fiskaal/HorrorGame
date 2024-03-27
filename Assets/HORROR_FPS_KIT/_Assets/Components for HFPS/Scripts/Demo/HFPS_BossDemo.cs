using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class HFPS_BossDemo : MonoBehaviour {
    
    
//////////////////////////////////////
///
///     CLASSES
///
///////////////////////////////////////
    
    
    [Serializable]
    public class Health {
        
        [Space]
        
        public int minHealth;
        public int maxHealth;
        
        [Space]
        
        public List<Health_Check> healthChecks;
        
    }//Health
    
    [Serializable]
    public class Health_Check {
        
        [Space]
        
        public float health;
        public UnityEvent healthEvent;
        
        [Header("Auto")]
        public bool triggered;
        
    }//Health_Check
    
    [Serializable]
    public class Stages {
        
        [Space]
        
        public bool randomize;
        
        [Space]
        
        public List<Stages_Settings> stagesSettings;
        
    }//Stages
    
    [Serializable]
    public class Stages_Settings {
        
        [Space]
        
        public string name;
        
        [Space]
        
        public Stage_Type stageType;
        public float nextStageWait;
        
        [Space]
        
        public List<GameObject> spawnPrefabs;
        public List<Transform> spawnTrans;
        
        [Space]
        
        public float initSpawnWait;
        public int spawnCount;
        public float spawnBuff;
        
    }//Stages_Settings
    
    [Serializable]
    public class Events {
        
        [Space]
        
        public UnityEvent onStart;
        public UnityEvent onDamage;
        public UnityEvent onDeath;
        
    }//Events
    
    [Serializable]
    public class Animation {
        
        [Space]
        
        public Animator anim;
        
        [Space]
        
        public List<Attack_Types> attackTypes;
        
        [Space]
        
        public AnimationClip wakeUp;
        public AnimationClip yell;
        public AnimationClip damage;
        public AnimationClip death;
        
    }//Animation
    
    [Serializable]
    public class Attack_Types {
        
        [Space]
        
        public string name;
        
        [Space]
        
        public Stage_Type stageType;
        public AnimationClip attackClip;
        
    }//Attack_Types
    
    [Serializable]
    public class Sound_Library {
            
        public string name;
            
        [Space]
            
        public AudioSource source;
        public List<AudioClip> clips;
        public float volume;
            
    }//Sound Library
    
    
//////////////////////////////////////
///
///     ENUMS
///
///////////////////////////////////////
    
    
    public enum Stage_Type {
        
        Spawn = 0,
        AttackLeft = 1,
        AttackRight = 2,
        AttackCenter = 3,
        AttackLasers = 4,
        
    }//Stage_Type
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
    
    public bool autoStart;
    public bool useStartDelay;
    public float startDelay;
    
    public Health health;
    public Animation animations;
    public Stages stages;
    public Events events;
    public List<Sound_Library> soundLibrary;
    
    public float destroyWait;
    
    [Header("Auto")]
    public int curHealth;
    public bool invincible;
    public bool isDamaged;
    public bool isDead;
    
    public int curStage;

    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    
    
    void Start() {
        
        StartInit();
        
    }//start

    public void StartInit(){
        
        curHealth = health.maxHealth;
        isDamaged = false;
        isDead = false;
        
        curStage = 0;
        
        StartCoroutine("Start_Buff");
        
    }//StartInit
    
    private IEnumerator Start_Buff(){
        
        yield return new WaitForSeconds(animations.wakeUp.length + 2f);
        
        animations.anim.Play(animations.yell.name);
        
        yield return new WaitForSeconds(animations.yell.length + 2f);
        
        if(autoStart){
            
            if(useStartDelay){
        
                StartCoroutine("AutoStart_Delayed");
                
            //useStartDelay
            } else {
                
                Boss_Loop();
                
            }//useStartDelay
            
        }//autoStart
        
    }//Start_Buff
    
    private IEnumerator AutoStart_Delayed(){
       
        yield return new WaitForSeconds(startDelay);
        
        Boss_Loop();
        
    }//AutoStart_Delayed
    

//////////////////////////////////////
///
///     BOSS ACTIONS
///
///////////////////////////////////////
    
    
    public void Boss_Loop(){
        
        if(!isDamaged && !isDead){
            
            StopCoroutine("BossLoop_Buff");
            StartCoroutine("BossLoop_Buff");
        
        }//!isDamaged & !isDead
        
    }//Boss_Loop
    
    private IEnumerator BossLoop_Buff(){
        
        int tempAnimCatch = -1;
        
        if(stages.randomize){
            
            curStage = UnityEngine.Random.Range(0, stages.stagesSettings.Count) + 1;
            
        //randomize
        } else {
            
            curStage += 1;
            
            if(curStage > stages.stagesSettings.Count){
                
                curStage = 1;
                
            }//curStage > stagesSettings.Count
            
        }//randomize
        
        yield return new WaitForSeconds(0.1f);
        
        for(int an = 0; an < animations.attackTypes.Count; an++) {
                
            if(animations.attackTypes[an].stageType == stages.stagesSettings[curStage - 1].stageType){
                    
                tempAnimCatch = an;
                    
            }//stageType = stageType
                
        }//for an attackTypes
        
        if(stages.stagesSettings[curStage - 1].stageType == Stage_Type.Spawn){
        
            Spawn_Possessed();
            
        }//stageType = spawn

        if(stages.stagesSettings[curStage - 1].stageType != Stage_Type.Spawn){
            
            if(tempAnimCatch > -1){

                animations.anim.Play(animations.attackTypes[tempAnimCatch].attackClip.name);

                yield return new WaitForSeconds(animations.attackTypes[tempAnimCatch].attackClip.length + stages.stagesSettings[curStage - 1].nextStageWait);

            }//tempAnimCatch > -1
        
            Boss_Loop();
            
        }//stageType != spawn
        
    }//BossLoop_Buff
    
    public void Attack_Stop(){
        
        isDamaged = true;
        
        StopCoroutine("BossLoop_Buff");
        StopCoroutine("SpawnPossessed_Buff");
        
        StopCoroutine("AttackStop_Buff");
        StartCoroutine("AttackStop_Buff");
        
    }//Attack_Stop
    
    private IEnumerator AttackStop_Buff(){
        
        animations.anim.Play(animations.damage.name);
        
        yield return new WaitForSeconds(animations.damage.length + 2f);
        
        isDamaged = false;
        
        Boss_Loop();
        
    }//AttackStop_Buff
    
    private void Spawn_Possessed(){
        
        StopCoroutine("SpawnPossessed_Buff");
        StartCoroutine("SpawnPossessed_Buff");
        
    }//Spawn_Possessed

    private IEnumerator SpawnPossessed_Buff(){
        
        int tempAnimCatch = -1;
        
        int randomPrefab = -1;
        int randomTrans = -1;
        int tempSpawnCount = stages.stagesSettings[curStage - 1].spawnCount + 1;
        
        for(int an = 0; an < animations.attackTypes.Count; an++) {
                
            if(animations.attackTypes[an].stageType == stages.stagesSettings[curStage - 1].stageType){
                    
                tempAnimCatch = an;
                    
            }//stageType = stageType
                
        }//for an attackTypes
        
        if(tempAnimCatch > -1){
            
            animations.anim.Play(animations.attackTypes[tempAnimCatch].attackClip.name);
            
            yield return new WaitForSeconds(animations.attackTypes[tempAnimCatch].attackClip.length + stages.stagesSettings[curStage - 1].initSpawnWait);
        
        }//tempAnimCatch > -1
        
        for(int s = 0; s < tempSpawnCount; s++) {
            
            if(s < tempSpawnCount - 1){
                
                if(stages.stagesSettings[curStage - 1].spawnPrefabs.Count > 0){

                    randomPrefab = UnityEngine.Random.Range(0, stages.stagesSettings[curStage - 1].spawnPrefabs.Count);

                //spawnPrefabs.Count > 0
                } else {

                    randomPrefab = 0;

                }//spawnPrefabs.Count > 0

                if(stages.stagesSettings[curStage - 1].spawnTrans.Count > 0){

                    randomTrans = UnityEngine.Random.Range(0, stages.stagesSettings[curStage - 1].spawnTrans.Count);

                //spawnTrans.Count > 0
                } else {

                    randomTrans = 0;

                }//spawnTrans.Count > 0
                
                GameObject newPossessed = Instantiate(stages.stagesSettings[curStage - 1].spawnPrefabs[randomPrefab], stages.stagesSettings[curStage - 1].spawnTrans[randomTrans]);
                
                yield return new WaitForSeconds(stages.stagesSettings[curStage - 1].spawnBuff);
                
            //s <= tempSpawnCount - 1
            } else {
             
                yield return new WaitForSeconds(stages.stagesSettings[curStage - 1].nextStageWait);
                
                Boss_Loop();
                
            }//s <= tempSpawnCount - 1
            
        }//for s tempSpawnCount
        
    }//SpawnPossessed_Buff
    
    public void Death(){
        
        StopCoroutine("BossLoop_Buff");
        StopCoroutine("SpawnPossessed_Buff");
        StopCoroutine("AttackStop_Buff");
        
        events.onDeath.Invoke();
        
        invincible = false;
        isDead = true;
        curHealth = 0;
        
        animations.anim.Play(animations.death.name);
        
        StartCoroutine("DestroyBuff");
        
    }//Death
    
    
//////////////////////////////////////
///
///     DAMAGE ACTIONS
///
///////////////////////////////////////
    
    
    public void Damage(int amount){
        
        if(!isDead){
            
            curHealth -= amount;
        
            if(curHealth <= health.minHealth){
            
                isDead = true;
                curHealth = 0;
                
                Death();
            
            //curHealth <= minHealth
            } else {
             
                if(health.healthChecks.Count > 0){
                    
                    for(int h = 0; h < health.healthChecks.Count; h++) {
                        
                        if(!health.healthChecks[h].triggered){
                            
                            if(curHealth <= health.healthChecks[h].health){
                                
                                health.healthChecks[h].triggered = true;
                                health.healthChecks[h].healthEvent.Invoke();
                                
                            }//curHealth <= health
                            
                        }//!triggered
                        
                    }//for h healthChecks
                    
                }//healthChecks.Count > 0
                
            }//curHealth <= minHealth
        
        }//!isDead
            
    }//Damage
    
    
//////////////////////////////////////
///
///     SOUND ACTIONS
///
///////////////////////////////////////
    
    
    public void LibraryPlay_Sound(int slot){
        
        int tempSound = 0;
            
        if(soundLibrary[slot - 1].clips.Count > 0){
             
            tempSound = UnityEngine.Random.Range(0, soundLibrary[slot - 1].clips.Count);
                
        }//clips.Count > 0
        
        if(soundLibrary[slot - 1].source != null){
            
            if(soundLibrary[slot - 1].volume > 0){
                
                soundLibrary[slot - 1].source.PlayOneShot(soundLibrary[slot - 1].clips[tempSound], soundLibrary[slot - 1].volume);
            
            //volume > 0
            } else {
                
                soundLibrary[slot - 1].source.PlayOneShot(soundLibrary[slot - 1].clips[tempSound]);
                
            }//volume > 0
            
        }//source != null
        
    }//LibraryPlay_Sound
    
    
//////////////////////////////////////
///
///     BUFF ACTIONS
///
///////////////////////////////////////
    
    
    private IEnumerator DestroyBuff(){
        
        yield return new WaitForSeconds(destroyWait);
        
        this.gameObject.SetActive(false);
        
    }//DestroyBuff
    
    
//////////////////////////////////////
///
///     STATE ACTIONS
///
///////////////////////////////////////
    
    
    public void Invincible_State(bool state){
        
        invincible = state;
        
    }//Invincible_State
    
    public void Dead_State(bool state){
        
        isDead = state;
        
    }//Dead_State

    
}//HFPS_BossDemo
