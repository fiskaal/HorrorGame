using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class HFPS_BossDemo_Type2 : MonoBehaviour {

    
//////////////////////////////////////
///
///     CLASSES
///
///////////////////////////////////////
    
    
    [Serializable]
    public class Animation {
        
        [Space]
        
        public Animator anim;
        
        [Space]
        
        public List<Animation_Types> generalAnims;
        public List<Attack_Types> attackTypes;
        
    }//Animation
    
    [Serializable]
    public class Animation_Types {
        
        [Space]
        
        public string name;
        public AnimationClip clip;
        
        [Space]
        
        public Trigger_Type bodyType;
        public string bodyTrigger;
        public int moveState = -1;
        
        [Space]
        
        public Trigger_Type headType;
        public string headTrigger;

    }//Animation_Types
    
    [Serializable]
    public class Attack_Types {
        
        [Space]
        
        public string name;        
        public Stage_Type stageType;
        public AnimationClip clip;
        
        [Space]
        
        public Trigger_Type bodyType;
        public string bodyTrigger;
        public int moveState = -1;
        
        [Space]
        
        public Trigger_Type headType;
        public string headTrigger;
        
    }//Attack_Types
    
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
    public class Movement {
        
        [Space]
        
        public Transform moveTrans;
        
        [Space]
        
        public float moveSpeed;
        
        [Space]
        
        public bool useRotation;
        public float rotationSpeed;
        
        [Space]
        
        public float moveRange_Max;
        public float moveRange_Stop;
        
    }//Movement
    
    [Serializable]
    public class Stages {
        
        [Space]
        
        public string name;
        
        [Space]
        
        public Stage_Type stageType;
        public float nextStageWait;
        
        [Space]
        
        public Transform target;
        
        [Space]
        
        public List<GameObject> spawnPrefabs;
        public List<Transform> spawnTrans;
        
        [Space]
        
        public float initSpawnWait;
        public int spawnCount;
        public float spawnBuff;
        
    }//Stages
    
    [Serializable]
    public class Events {
        
        [Space]
        
        public UnityEvent onAwake;
        public UnityEvent onDamage;
        public UnityEvent onDeath;
        
    }//Events
    
    [Serializable]
    public class Sound_Library {
            
        public string name;
            
        [Space]
            
        public AudioSource source;
        public List<AudioClip> clips;
        public float volume;
            
    }//Sound Library
    
    [Serializable]
    public class Auto {
        
        public Transform target;
        
        [Space]
        
        public int curHealth;
        public bool invincible;
        public bool isDamaged;
        public bool isDead;

        [Space]
        
        public bool canMove;
        public float moveTo_Distance;
        public bool moving;
        public bool stopped;

        [Space]
        
        public Vector3 velocity = Vector3.zero;
        public float singleStep;
        public Vector3 targetPosition;
        public Vector3 targetDirection;
        public Vector3 newDirection;

        [Space]
        
        public int tempAnimCatch;
        public int curStage;
        public string tempBodyTrig = "";
        public string tempHeadTrig = "";
        public int tempMoveState = -1;
        public bool locked;
        
    }//Auto
    
    
//////////////////////////////////////
///
///     ENUMS
///
///////////////////////////////////////
    
    
    public enum Stage_Type {
        
        Move = 0,
        MoveNSpawn = 1,
        Spawn = 2,
        Lasers = 3,
        
    }//Stage_Type
    
    public enum Trigger_Type {
        
        None = 0,
        Bool = 1,
        Trigger = 2,
        
    }//Trigger_Type
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
    
    public Health health;
    public Animation animations;
    public Movement movement;
    public List<Stages> stages;
    public Events events;
    public List<Sound_Library> soundLibrary;
    
    
////////////////////////
///
///     AUTO
///
////////////////////////
    
    
    [Header("Auto")]

    public Auto auto;
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    
    
    void Start() {
        
        StartInit();
        
    }//start
    
    public void StartInit(){
        
        auto.curHealth = health.maxHealth;
        auto.isDamaged = false;
        auto.isDead = false;
        
        auto.curStage = 0;
        
        auto.tempBodyTrig = "";
        auto.tempHeadTrig = "";
        auto.tempMoveState = -1;
        
        animations.anim.SetBool("Body_Idle", true);
        animations.anim.SetBool("Head_Idle", true);

        animations.anim.SetInteger("Move_State", -1);
        
    }//StartInit
    
    public void WakeUp(){
        
        StartCoroutine("WakeUp_Buff");
        
    }//WakeUp
    
    private IEnumerator WakeUp_Buff(){
        
        events.onAwake.Invoke();
        
        animations.anim.SetBool("Body_Idle", false);
        animations.anim.SetBool("Head_Idle", false);
        
        //animations.anim.SetBool(animations.generalAnims[0].bodyTrigger, true);
        animations.anim.SetBool(animations.generalAnims[0].headTrigger, true);
        
        yield return new WaitForSeconds(0.2f);
        
        animations.anim.SetBool(animations.generalAnims[0].headTrigger, false);
        
        yield return new WaitForSeconds(animations.generalAnims[0].clip.length + 2f);

        Boss_Loop();
        
    }//WakeUp_Buff
    
    
//////////////////////////////////////
///
///     UPDATE ACTIONS
///
//////////////////////////////////////


    void Update() {
    
        if(!auto.locked && !auto.isDead && !auto.isDamaged){
                
            if(auto.canMove){
                
                if(auto.target != null){

                    Movement_Basic();
                    
                }//target != null
                
            }//canMove
            
        }//!locked & !isDead & !isDamaged
    
    }//update
    
    
//////////////////////////////////////
///
///     MOVEMENT ACTIONS
///
///////////////////////////////////////


    public void CanMove_State(bool state){
        
        auto.canMove = state;
        
    }//CanMove_State
    
    private void Movement_Basic(){
        
        if(auto.target != null){
        
            auto.moveTo_Distance = Vector3.Distance(movement.moveTrans.transform.position, auto.target.position);
     
            if(auto.moveTo_Distance < movement.moveRange_Max && auto.moveTo_Distance > movement.moveRange_Stop){
                
                auto.targetPosition = auto.target.TransformPoint(new Vector3(0, 0, 0));
                
                if(movement.useRotation){
                    
                    auto.targetDirection = auto.target.position - movement.moveTrans.transform.position;
                
                    auto.singleStep = movement.rotationSpeed * Time.deltaTime;
                    auto.newDirection = Vector3.RotateTowards(movement.moveTrans.transform.forward, auto.targetDirection, auto.singleStep, 0.0f);

                    movement.moveTrans.transform.rotation = Quaternion.LookRotation(auto.newDirection);
                
                }//useRotation
                    
                movement.moveTrans.transform.position = Vector3.SmoothDamp(movement.moveTrans.transform.position, auto.targetPosition, ref auto.velocity, movement.moveSpeed);
                    
                auto.moving = true;
                auto.stopped = false;
                
            }//moveTo_Distance < moveRange_Max & moveTo_Distance > moveRange_Stop

            if(auto.moveTo_Distance <= movement.moveRange_Stop) {

                if(movement.useRotation){

                    auto.targetDirection = auto.target.position - movement.moveTrans.transform.position;

                    auto.singleStep = movement.rotationSpeed * Time.deltaTime;
                    auto.newDirection = Vector3.RotateTowards(movement.moveTrans.transform.forward, auto.targetDirection, auto.singleStep, 0.0f);

                    movement.moveTrans.transform.rotation = Quaternion.LookRotation(auto.newDirection);

                }//useRotation

                auto.moving = false;
                auto.stopped = true;

            }//moveTo_Distance <= moveRange_Stop
                
        }//target != null
        
    }//BasicMovement
    
    
//////////////////////////////////////
///
///     BOSS ACTIONS
///
///////////////////////////////////////
    
    
    
    public void Boss_Loop(){
        
        if(!auto.isDamaged && !auto.isDead){
            
            StopCoroutine("BossLoop_Buff");
            StartCoroutine("BossLoop_Buff");
        
        }//!isDamaged & !isDead
        
    }//Boss_Loop
    
    private IEnumerator BossLoop_Buff(){

        if(auto.tempMoveState > -1){

            animations.anim.SetInteger("Move_State", -1);

        }//tempMoveState > -1

        if(auto.tempBodyTrig != ""){

            if(animations.attackTypes[auto.tempAnimCatch].bodyType == Trigger_Type.Bool){
                
                animations.anim.SetBool(auto.tempBodyTrig, false);

            }//bodyType = bool
            
        }//tempBodyTrig != null

        if(auto.tempHeadTrig != ""){
            
            if(animations.attackTypes[auto.tempAnimCatch].headType == Trigger_Type.Bool){

                animations.anim.SetBool(auto.tempHeadTrig, false);
                
            }//headType = bool

        }//tempHeadTrig != null
        
        auto.tempBodyTrig = "";
        auto.tempHeadTrig = "";
        auto.tempMoveState = -1;
        auto.tempAnimCatch = -1;
        
        auto.curStage += 1;
            
        if(auto.curStage > stages.Count){
                
            auto.curStage = 1;
                
        }//curStage > stages.Count
        
        CanMove_State(false);
        
        yield return new WaitForSeconds(0.1f);
        
        for(int an = 0; an < animations.attackTypes.Count; an++) {
                
            if(animations.attackTypes[an].stageType == stages[auto.curStage - 1].stageType){
                    
                auto.tempAnimCatch = an;
                
                if(animations.attackTypes[an].bodyTrigger != ""){
                    
                    auto.tempBodyTrig = animations.attackTypes[an].bodyTrigger;
                
                }//bodyTrigger != null
                
                if(animations.attackTypes[an].headTrigger != ""){
                    
                    auto.tempHeadTrig = animations.attackTypes[an].headTrigger;
                
                }//headTrigger != null
                
                if(animations.attackTypes[an].moveState > -1){
                    
                    auto.tempMoveState = animations.attackTypes[an].moveState;
                    
                }//moveState > -1
                
            }//stageType = stageType
                
        }//for an attackTypes
        
        if(stages[auto.curStage - 1].stageType == Stage_Type.Move){
        
            auto.target = stages[auto.curStage - 1].target;
            
            CanMove_State(true);
            
        }//stageType = move
        
        if(stages[auto.curStage - 1].stageType == Stage_Type.MoveNSpawn){
        
            auto.target = stages[auto.curStage - 1].target;
            CanMove_State(true);
            
            Spawn_Enemies();
            
        }//stageType = move and spawn

        if(stages[auto.curStage - 1].stageType == Stage_Type.Spawn){
        
            Spawn_Enemies();
            
        }//stageType = spawn
                
        if(auto.tempMoveState > -1){
                    
            animations.anim.SetInteger("Move_State", auto.tempMoveState);
                    
        }//tempMoveState > -1
        
        if(auto.tempBodyTrig != ""){
                    
            if(animations.attackTypes[auto.tempAnimCatch].bodyType == Trigger_Type.Bool){
                    
                animations.anim.SetBool(auto.tempBodyTrig, true);

            }//bodyType = bool
            
            if(animations.attackTypes[auto.tempAnimCatch].bodyType == Trigger_Type.Trigger){
                    
                animations.anim.ResetTrigger(auto.tempBodyTrig);
                animations.anim.SetTrigger(auto.tempBodyTrig);
                
            }//bodyType = trigger
            
        }//tempBodyTrig != null
                
        if(auto.tempHeadTrig != ""){
            
            if(animations.attackTypes[auto.tempAnimCatch].headType == Trigger_Type.Bool){
                    
                animations.anim.SetBool(auto.tempHeadTrig, true);
                
            }//headType = bool
            
            if(animations.attackTypes[auto.tempAnimCatch].headType == Trigger_Type.Trigger){
                    
                animations.anim.ResetTrigger(auto.tempHeadTrig);
                animations.anim.SetTrigger(auto.tempHeadTrig);
                
            }//headType = trigger
                
        }//tempHeadTrig != null
        
        if(stages[auto.curStage - 1].stageType != Stage_Type.Spawn && stages[auto.curStage - 1].stageType != Stage_Type.MoveNSpawn){

            yield return new WaitForSeconds(animations.attackTypes[auto.tempAnimCatch].clip.length + stages[auto.curStage - 1].nextStageWait);

            if(auto.tempMoveState > -1){

                animations.anim.SetInteger("Move_State", -1);

            }//tempMoveState > -1

            if(auto.tempBodyTrig != ""){
                
                if(animations.attackTypes[auto.tempAnimCatch].bodyType == Trigger_Type.Bool){

                    animations.anim.SetBool(auto.tempBodyTrig, false);

                }//bodyType = bool
                
            }//tempBodyTrig != null

            if(auto.tempHeadTrig != ""){

                if(animations.attackTypes[auto.tempAnimCatch].headType == Trigger_Type.Bool){
                    
                    animations.anim.SetBool(auto.tempHeadTrig, false);

                }//headType = bool
                
            }//tempHeadTrig != null
        
            Boss_Loop();
            
        }//stageType != spawn and move N spawn
        
    }//BossLoop_Buff
    
    public void Attack_Stop(){
        
        auto.isDamaged = true;
        
        CanMove_State(false);

        if(auto.tempMoveState > -1){

            animations.anim.SetInteger("Move_State", -1);

        }//tempMoveState > -1

        if(auto.tempBodyTrig != ""){

            if(animations.attackTypes[auto.tempAnimCatch].bodyType == Trigger_Type.Bool){
                
                animations.anim.SetBool(auto.tempBodyTrig, false);

            }//bodyType = bool
            
        }//tempBodyTrig != null

        if(auto.tempHeadTrig != ""){

            if(animations.attackTypes[auto.tempAnimCatch].headType == Trigger_Type.Bool){
                
                animations.anim.SetBool(auto.tempHeadTrig, false);

            }//headType = bool
            
        }//tempHeadTrig != null
        
        StopCoroutine("BossLoop_Buff");
        StopCoroutine("SpawnEnemies_Buff");
        
        StopCoroutine("AttackStop_Buff");
        StartCoroutine("AttackStop_Buff");
        
    }//Attack_Stop
    
    private IEnumerator AttackStop_Buff(){
        
        //animations.anim.SetBool(animations.generalAnims[1].bodyTrigger, true);
        animations.anim.SetBool(animations.generalAnims[1].headTrigger, true);
        
        yield return new WaitForSeconds(animations.generalAnims[1].clip.length + 2f);
        
        auto.isDamaged = false;
        
        Boss_Loop();
        
    }//AttackStop_Buff
    
    private void Spawn_Enemies(){
        
        StopCoroutine("SpawnEnemies_Buff");
        StartCoroutine("SpawnEnemies_Buff");
        
    }//Spawn_Enemies
    
    private IEnumerator SpawnEnemies_Buff(){
        
        auto.tempAnimCatch = -1;
        
        int randomPrefab = -1;
        int randomTrans = -1;
        int tempSpawnCount = stages[auto.curStage - 1].spawnCount + 1;
        
        for(int an = 0; an < animations.attackTypes.Count; an++) {
                
            if(animations.attackTypes[an].stageType == stages[auto.curStage - 1].stageType){
                    
                auto.tempAnimCatch = an;
                    
            }//stageType = stageType
                
        }//for an attackTypes
   
        yield return new WaitForSeconds(stages[auto.curStage - 1].initSpawnWait);
        
        for(int s = 0; s < tempSpawnCount; s++) {
            
            if(s < tempSpawnCount - 1){
                
                if(stages[auto.curStage - 1].spawnPrefabs.Count > 0){

                    randomPrefab = UnityEngine.Random.Range(0, stages[auto.curStage - 1].spawnPrefabs.Count);

                //spawnPrefabs.Count > 0
                } else {

                    randomPrefab = 0;

                }//spawnPrefabs.Count > 0

                if(stages[auto.curStage - 1].spawnTrans.Count > 0){

                    randomTrans = UnityEngine.Random.Range(0, stages[auto.curStage - 1].spawnTrans.Count);

                //spawnTrans.Count > 0
                } else {

                    randomTrans = 0;

                }//spawnTrans.Count > 0
                
                GameObject newEnemy = Instantiate(stages[auto.curStage - 1].spawnPrefabs[randomPrefab], stages[auto.curStage - 1].spawnTrans[randomTrans]);
                
                newEnemy.transform.parent = null;
                
                yield return new WaitForSeconds(stages[auto.curStage - 1].spawnBuff);
                
            //s <= tempSpawnCount - 1
            } else {

                yield return new WaitForSeconds(animations.attackTypes[auto.tempAnimCatch].clip.length + stages[auto.curStage - 1].nextStageWait);

                Boss_Loop();
                
            }//s <= tempSpawnCount - 1
            
        }//for s tempSpawnCount
        
    }//SpawnEnemies_Buff
    
    public void Death(){
        
        CanMove_State(false);
        
        StopCoroutine("BossLoop_Buff");
        StopCoroutine("SpawnEnemies_Buff");
        StopCoroutine("AttackStop_Buff");
        
        events.onDeath.Invoke();
        
        Invincible_State(false);
        Dead_State(true);
        auto.curHealth = 0;

        if(auto.tempMoveState > -1){

            animations.anim.SetInteger("Move_State", -1);

        }//tempMoveState > -1

        if(auto.tempBodyTrig != ""){
            
            if(animations.attackTypes[auto.tempAnimCatch].bodyType == Trigger_Type.Bool){

                animations.anim.SetBool(auto.tempBodyTrig, false);

            }//bodyType = bool
            
        }//tempBodyTrig != null

        if(auto.tempHeadTrig != ""){
            
            if(animations.attackTypes[auto.tempAnimCatch].headType == Trigger_Type.Bool){

                animations.anim.SetBool(auto.tempHeadTrig, false);

            }//headType = bool
            
        }//tempHeadTrig != null
        
        animations.anim.SetTrigger(animations.generalAnims[2].bodyTrigger);
        
        //StartCoroutine("DestroyBuff");
        
    }//Death
    
    
//////////////////////////////////////
///
///     DAMAGE ACTIONS
///
///////////////////////////////////////
    
    
    public void Damage(int amount){
        
        if(!auto.isDead){
            
            auto.curHealth -= amount;
        
            if(auto.curHealth <= health.minHealth){
            
                auto.isDead = true;
                auto.curHealth = 0;
                
                Death();
            
            //curHealth <= minHealth
            } else {
             
                if(health.healthChecks.Count > 0){
                    
                    for(int h = 0; h < health.healthChecks.Count; h++) {
                        
                        if(!health.healthChecks[h].triggered){
                            
                            if(auto.curHealth <= health.healthChecks[h].health){
                                
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
///     STATE ACTIONS
///
///////////////////////////////////////
    
    
    public void Invincible_State(bool state){
        
        auto.invincible = state;
        
    }//Invincible_State
    
    public void Dead_State(bool state){
        
        auto.isDead = state;
        
    }//Dead_State
    
    
}//HFPS_BossDemo_Type2
