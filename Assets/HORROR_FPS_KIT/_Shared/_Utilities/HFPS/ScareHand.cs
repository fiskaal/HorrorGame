using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ThunderWire.Utility;
using HFPS.Player;
using HFPS.Systems;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/HFPS/Scare Handler")]
    public class ScareHand : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Use_Type {

            SingleUse = 0,
            MultiUse = 1,

        }//Use_Type

        public enum JSEffects {

            Off = 0,
            On = 1,

        }//JSEffects

        public enum Camera_Shake {

            Off = 0,
            On = 1,

        }//Camera_Shake

        public enum Shake_Preset {

            Scare = 0,
            Bump = 1,
            Explosion = 2,
            Earthquake = 3,
            BadTrip = 4,
            HandheldCamera = 5,
            Vibration = 6,
            RoughDriving = 7,

        }//Shake_Preset


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    /////////////////////////////
    ///
    ///     GENERAL OPTIONS
    ///
    /////////////////////////////


        public Use_Type useType;
        public bool disableArmsLock;

        public bool delayFxNshake;
        public float delayFxWait;


    /////////////////////////////
    ///
    ///     EFFECTS OPTIONS
    ///
    /////////////////////////////


        public JSEffects jumpscareEffects;

        public float chromaticAberrationAmount = 0.8f;
        public float vignetteAmount = 0.3f;
        public float effectsTime = 5f;
        [Tooltip("Value sets how long will be player scared.")]
        public float scaredBreathTime = 33f;


    /////////////////////////////
    ///
    ///     SHAKE OPTIONS
    ///
    /////////////////////////////


        public Camera_Shake cameraShake;

        public bool shakeByPreset;
        public Shake_Preset shakePreset;

        public float magnitude = 3f;
        public float roughness = 3f;
        public float startTime = 0.1f;
        public float durationTime = 3f;

        [Header("Scare Position Influence")]
        public Vector3 PositionInfluence = new Vector3(0.15f, 0.15f, 0f);
        public Vector3 RotationInfluence = Vector3.one;


    /////////////////////////////
    ///
    ///     AUDIO OPTIONS
    ///
    /////////////////////////////


        public bool useJumpscareSound;
        public AudioClip JumpscareSound;
        public AudioClip ScaredBreath;

        [Range(0, 5)] 
        public float scareVolume = 0.5f;


    /////////////////////////////
    ///
    ///     AUTO
    ///
    /////////////////////////////


        [Header("Auto")]
        public bool isPlayed;

        private JumpscareEffects jsEffects;

        public int tabs;
        public bool genOpts;
        public bool effOpts;
        public bool shakeOpts;
        public bool audioOpts;


    /////////////////////////////
    ///
    ///     START ACTIONS
    ///
    /////////////////////////////


        void Start() {

            if(jsEffects == null){

                jsEffects = ScriptManager.Instance.gameObject.GetComponent<JumpscareEffects>();

            }//jsEffects == null

        }//Start


    /////////////////////////////
    ///
    ///     SCARE ACTIONS
    ///
    /////////////////////////////


        public void Scare_InitDelayed(float delay){

            StartCoroutine("Scare_InitBuff", delay);

        }//Scare_InitDelayed

        private IEnumerator Scare_InitBuff(float delay){

            yield return new WaitForSeconds(delay);

            Scare_Init();

        }//Scare_InitBuff

        public void Scare_Init(){

            if(useType == Use_Type.SingleUse){

                if(!isPlayed) {

                    Scare_Start();

                }//!isPlayed

            //useType = single use
            } else {

                Scare_Start();

            }//useType = single use

        }//Scare_Init

        public void Scare_Start(){

            CameraShakeInstance shakeInstance = new CameraShakeInstance(0, 0, 0, 0);


    //////////////////
    ///     SOUNDS
    //////////////////


            if(useJumpscareSound){

                if(JumpscareSound != null) {

                    Utilities.PlayOneShot2D(transform.position, JumpscareSound, scareVolume);

                }//JumpscareSound != null

            }//useJumpscareSound


    //////////////////
    ///     SHAKE
    //////////////////


            if(cameraShake == Camera_Shake.On){

                if(shakeByPreset) {

                    if(shakePreset == Shake_Preset.Scare){

                        shakeInstance = CameraShakePresets.Scare;

                    }//shakePreset = scare

                    if(shakePreset == Shake_Preset.Bump){

                        shakeInstance = CameraShakePresets.Bump;

                    }//shakePreset = Bump

                    if(shakePreset == Shake_Preset.Explosion){

                        shakeInstance = CameraShakePresets.Explosion;

                    }//shakePreset = Explosion

                    if(shakePreset == Shake_Preset.Earthquake){

                        shakeInstance = CameraShakePresets.Earthquake;

                    }//shakePreset = Earthquake

                    if(shakePreset == Shake_Preset.BadTrip){

                        shakeInstance = CameraShakePresets.BadTrip;

                    }//shakePreset = BadTrip

                    if(shakePreset == Shake_Preset.HandheldCamera){

                        shakeInstance = CameraShakePresets.HandheldCamera;

                    }//shakePreset = HandheldCamera

                    if(shakePreset == Shake_Preset.Vibration){

                        shakeInstance = CameraShakePresets.Vibration;

                    }//shakePreset = Vibration

                    if(shakePreset == Shake_Preset.RoughDriving){

                        shakeInstance = CameraShakePresets.RoughDriving;

                    }//shakePreset = RoughDriving

                //shakeByPreset
                } else {

                    shakeInstance = new CameraShakeInstance(magnitude, roughness, startTime, durationTime);

                    shakeInstance.PositionInfluence = PositionInfluence;
                    shakeInstance.RotationInfluence = RotationInfluence;

                }//shakeByPreset

            }//cameraShake = on

            if(cameraShake == Camera_Shake.Off){

                shakeInstance = new CameraShakeInstance(0, 0, 0, 0);

            }//cameraShake = off


    //////////////////
    ///     EFFECTS
    //////////////////


            if(delayFxNshake){

                StartCoroutine("FXnShake_Buff", shakeInstance);

            //delayFxNshake
            } else {

                if(jumpscareEffects == JSEffects.On){

                    jsEffects.Scare(shakeInstance, chromaticAberrationAmount, vignetteAmount, scaredBreathTime, effectsTime, ScaredBreath);

                }//jumpscareEffects = on

                if(jumpscareEffects == JSEffects.Off){

                    jsEffects.Scare(shakeInstance, 0, 0, 0.1f, 0.1f, null);

                }//jumpscareEffects = off

            }//delayFxNshake

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_VENDOR_PRESENT)

                if(disableArmsLock){

                    CameraShaker.Instance.DisableArmsLock_State(true);

                }//disableArmsLock

            #endif


    //////////////////
    ///     USE TYPE
    //////////////////


            if(useType == Use_Type.SingleUse){

                isPlayed = true;

            }//useType = single use

        }//Scare_Start

        private IEnumerator FXnShake_Buff(CameraShakeInstance newShake){

            yield return new WaitForSeconds(delayFxWait);

            if(jumpscareEffects == JSEffects.On){

                jsEffects.Scare(newShake, chromaticAberrationAmount, vignetteAmount, scaredBreathTime, effectsTime, ScaredBreath);

            }//jumpscareEffects = on

            if(jumpscareEffects == JSEffects.Off){

                jsEffects.Scare(newShake, 0, 0, 0.1f, 0.1f, null);

            }//jumpscareEffects = off

            #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_VENDOR_PRESENT)

                if(disableArmsLock){

                    CameraShaker.Instance.DisableArmsLock_State(true);

                }//disableArmsLock

            #endif

        }//FXnShake_Buff


    /////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    /////////////////////////////


        public void Played_State(bool state){

            isPlayed = state;

        }//Played_State


    }//ScareHand


}//namespace
