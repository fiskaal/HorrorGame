using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Destroyable/Destroyable")]
    public class HFPS_Destroyable : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Break_Stage {

            [Space]

            public string name;

            [Space]

            public int healthCheck;

            [Space]

            public bool addForce;

            public ForceType forceType;

            public float force;
            public float rotateForce;

            [Space]

            public GameObject hide;
            public Transform parent;
            public Transform explosionSource;

            [Space]

            public List<GameObject> show;
            public List<Rigidbody> rigids;

            [Space]

            public UnityEvent breakEvent;

        }//Break_Stage

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


        public enum HealthType {

            None = 0,
            Health = 1,

        }//HealthType

        public enum DamageType {

            None = 0,
            Break = 1,
            Explode = 2,

        }//DamgeType

        public enum BreakType {

            None = 0,
            Invincible = 1,
            SingleBreak = 2,
            MultiBreak = 3,

        }//BreakType

        public enum ForceType {

            Regular = 0,
            Explosion = 1,

        }//ForceType

        public enum ExplodeAttribute {

            None = 0,
            Radial = 1,

        }//ExplodeAttribute

        public enum DestroyType {

            None = 0,
            AfterTime = 1,

        }//DestroyType


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ////////////////////////////
    ///
    ///     USER OPTIONS
    ///
    ////////////////////////////

    ////////////////////
    ///
    ///     GENERAL
    ///
    ////////////////////


        public DamageType damageType;
        public BreakType breakType;

        public bool affectedByExplosions;

        public GameObject parent;
        public Collider collider;


    ////////////////////
    ///
    ///     HEALTH
    ///
    ////////////////////


        public HealthType healthType;
        public int minHealth;
        public int maxHealth;


    ////////////////////
    ///
    ///     ANIMATION
    ///
    ////////////////////


        public bool useAnimation;
        public Animation anim;
        public AnimationClip damageClip;
        public AnimationClip breakClip;


    ////////////////////
    ///
    ///     EXPLOSION
    ///
    ////////////////////


        public ExplodeAttribute explodeAttribute;
        public float explodeWait;

        public SphereCollider radial;
        public float radialSize;
        public float radialMulti;


    ////////////////////
    ///
    ///     STAGES
    ///
    ////////////////////


        public List<Break_Stage> breakStages;


    ////////////////////
    ///
    ///     DESTROY
    ///
    //////////////////// 


        public DestroyType destroyType;
        public float destroyWait;


    ////////////////////
    ///
    ///     SOUNDS
    ///
    ////////////////////


        public List<Sound_Library> soundLibrary;


    ////////////////////////////
    ///
    ///     AUTO
    ///
    ////////////////////////////


        public int tempSound;
        public int curStage;
        public int curHealth;
        public bool updateRadial;
        public bool locked;

        public RaycastHit tempRayHit;

        public int tabs;

        public bool genOpts;
        public bool healthOpts;
        public bool animOpts;
        public bool explOpts;
        public bool stageOpts;
        public bool destOpts;
        public bool soundOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            tempSound = 0;
            curStage = 1;

            curHealth = maxHealth;

            if(radial != null){

                radial.gameObject.SetActive(false);
                radial.radius = 0;

            }//radial != null

            locked = false;

        }//start


    //////////////////////////////////////
    ///
    ///     UPDATE ACTIONS
    ///
    ///////////////////////////////////////


        void Update(){

            if(damageType == DamageType.Explode){

                if(updateRadial){

                    if(radial.radius < radialSize){

                        radial.radius += radialMulti * Time.deltaTime;

                    }//radius < radialSize

                    if(radial.radius >= radialSize){

                        updateRadial = false;

                        radial.gameObject.SetActive(false);

                    }//radius >= radialSize

                }//updateRadial

            }//damageType = Explode

        }//Update


    //////////////////////////////////////
    ///
    ///     DAMAGE ACTIONS
    ///
    ///////////////////////////////////////


        public void ReceiveRaycast(RaycastHit hit){

            tempRayHit = hit;

        }//ReceiveRaycast

        public void ReceiveDamage(int damage){

            if(!locked){

                Damage_Init(damage);

                //Debug.Log("Hit " + this.gameObject.name + " " + "Damage " + damage);

            }//!locked

        }//ReceiveDamage

        public void Damage_Init(int damage){

            if(healthType == HealthType.None){

                if(damageType == DamageType.None){

                    if(breakType == BreakType.Invincible){

                        if(useAnimation){

                            anim.Play(damageClip.name);

                        }//useAnimation

                    }//breakType = invincible

                }//damageType = none

                if(damageType == DamageType.Break | damageType == DamageType.Explode){

                    if(breakType != BreakType.Invincible){

                        if(breakType == BreakType.SingleBreak){

                            if(breakStages.Count < 2){

                                curStage = 1;

                            //breakStages.Count < 2
                            } else {

                                curStage = breakStages.Count;

                                //Debug.Log(breakStages.Count);

                            }//breakStages.Count < 2

                        }//breakType = Stages

                        if(breakType == BreakType.SingleBreak | breakType == BreakType.MultiBreak){

                            if(damageType == DamageType.Explode){

                                if(curStage > 1){

                                    StopCoroutine("ExplodeBuff");

                                }//curStage > 1

                            }//damageType = Explode

                            Break_Check(false);

                        }//breakType = Stages

                    //breakType != invincible
                    } else {

                        if(useAnimation){

                            anim.Play(damageClip.name);

                        }//useAnimation

                    }//breakType != invincible

                }//damageType = break or explode

            }//healthType = none

            if(healthType == HealthType.Health){

                if(damageType == DamageType.Break){

                    if(breakType != BreakType.Invincible){

                        if(curHealth > minHealth){

                            curHealth -= damage;

                            Break_Check(false);

                        //curHealth > minHealth
                        } else if(curHealth <= minHealth){

                            curHealth = minHealth;

                            Break_Check(true);

                        }//curHealth <= 0                    

                    }//breakType != invincible

                }//damageType = break

            }//healthType = health

        }//Damage_Init


    //////////////////////////////////////
    ///
    ///     BREAK ACTIONS
    ///
    ///////////////////////////////////////


        public void Break_Check(bool immediate){

            bool processStage = false;

            if(breakStages.Count > 0){

                if(immediate){

                    processStage = true;

                    curStage = breakStages.Count;

                }//immediate

                if(healthType == HealthType.Health){

                    if(curHealth <= breakStages[curStage - 1].healthCheck){

                        processStage = true;

                    }//curHealth <= healthCheck

                    if(curHealth <= minHealth){

                        if(collider != null){

                            collider.enabled = false;

                        }//collider != null

                        if(parent != null){

                            parent.SetActive(false);

                        //parent != null
                        } else {

                            if(useAnimation){

                                anim.Play(breakClip.name);

                            }//useAnimation

                        }//parent != null

                    //curHealth <= 0
                    } else {

                        if(useAnimation){

                            anim.Play(damageClip.name);

                        }//useAnimation

                    }//curHealth <= 0

                }//healthType = health

                if(healthType != HealthType.Health){

                    processStage = true;

                    if(curStage == breakStages.Count){

                        if(collider != null){

                            collider.enabled = false;

                        }//collider != null

                        if(parent != null){

                            parent.SetActive(false);

                        }//parent != null

                    }//curStage = breakStages.Count

                }//healthType != health

                if(processStage){

                    if(breakStages[curStage - 1].show.Count > 0){

                        for(int i = 0; i < breakStages[curStage - 1].show.Count; ++i ) {

                            if(breakStages[curStage - 1].parent != null){

                                breakStages[curStage - 1].show[i].transform.parent = breakStages[curStage - 1].parent;

                            //parent != null
                            } else {

                                breakStages[curStage - 1].show[i].transform.parent = this.transform;

                            }//parent != null

                            if(breakStages[curStage - 1].hide != null){

                                breakStages[curStage - 1].hide.SetActive(false);

                            }//hide != null

                            breakStages[curStage - 1].show[i].SetActive(true);

                            if(breakStages[curStage - 1].addForce){

                                if(breakStages[curStage - 1].rigids[i].isKinematic){

                                    breakStages[curStage - 1].rigids[i].isKinematic = false;

                                }//isKinematic

                                if(!breakStages[curStage - 1].rigids[i].useGravity){

                                    breakStages[curStage - 1].rigids[i].useGravity = true;

                                }//!useGravity

                                if(breakStages[curStage - 1].forceType == ForceType.Regular){

                                    breakStages[curStage - 1].rigids[i].AddRelativeForce(Vector3.forward * -breakStages[curStage - 1].force);
                                    breakStages[curStage - 1].rigids[i].AddRelativeTorque(Vector3.forward * -breakStages[curStage - 1].rotateForce * UnityEngine.Random.Range(-5f, 5f));
                                    breakStages[curStage - 1].rigids[i].AddRelativeTorque(Vector3.right * - breakStages[curStage - 1].rotateForce * UnityEngine.Random.Range(-5f, 5f));

                                }//forceType = regular

                                if(breakStages[curStage - 1].forceType == ForceType.Explosion){

                                    breakStages[curStage - 1].rigids[i].AddExplosionForce(breakStages[curStage - 1].force, breakStages[curStage - 1].explosionSource.position, radialSize);

                                }//forceType = explosion

                            }//addForce

                        }//for i show

                    }//show.Count > 0

                    breakStages[curStage - 1].breakEvent.Invoke();

                    if(curStage == breakStages.Count){

                        if(damageType == DamageType.Explode){

                            if(explodeAttribute == ExplodeAttribute.Radial){

                                if(radial != null){

                                    radial.gameObject.SetActive(true);

                                    updateRadial = true;

                                }//radial != null

                            }//explodeAttribute = Radial

                        }//damageType = explode

                        if(destroyType == DestroyType.AfterTime){

                            if(breakStages[curStage - 1].parent != null){

                                StartCoroutine("DestroyBuff", breakStages[curStage - 1].parent.gameObject);

                            //parent != null
                            } else {

                                StartCoroutine("DestroyBuff", this.gameObject);

                            }//parent != null

                        }//destroyType = AfterTime

                        locked = true;

                    //curStage = breakStages.Count
                    } else {

                        curStage += 1;

                        if(damageType == DamageType.Explode){

                            StartCoroutine("ExplodeBuff");

                        }//damageType = explode

                    }//curStage = breakStages.Count

                }//processStage

            }//breakStages.Count > 0

        }//Break_Check


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

            //source != null
            } else {

                if(soundLibrary[slot - 1].volume > 0){

                    AudioSource.PlayClipAtPoint(soundLibrary[slot - 1].clips[tempSound], tempRayHit.transform.position, soundLibrary[slot - 1].volume);

                //volume > 0
                } else {

                    AudioSource.PlayClipAtPoint(soundLibrary[slot - 1].clips[tempSound], tempRayHit.transform.position);

                }//volume > 0

            }//source != null

        }//LibraryPlay_Sound


    //////////////////////////////////////
    ///
    ///     BUFF ACTIONS
    ///
    ///////////////////////////////////////


        private IEnumerator ExplodeBuff(){

            yield return new WaitForSeconds(explodeWait);

            Break_Check(false);

        }//ExplodeBuff

        private IEnumerator DestroyBuff(GameObject newParent){

            yield return new WaitForSeconds(destroyWait);

            Destroy(newParent);

        }//DestroyBuff


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            locked = (bool)token["locked"];

            if(locked){

                this.gameObject.SetActive(false);

            }//locked

        }//OnLoad


    }//HFPS_Destroyable


}//namespace