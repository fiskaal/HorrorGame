using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Widescreen/Simple Widescreen")]
    public class SimpleWidescreen : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static SimpleWidescreen instance; 


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public GameObject holder;

        public Animator anim;
        public AnimationClip show;
        public AnimationClip hide;

        [Header("Auto")]
        public bool locked;

        public int tabs;

        public bool genOpts;
        public bool animOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {

            instance = this;
            locked = false;

        }//start


    //////////////////////////////////////
    ///
    ///     WIDESCREEN ACTIONS
    ///
    ///////////////////////////////////////

    //////////////////////
    ///
    ///     IN ACTIONS
    ///
    //////////////////////


        public void WidescreenIN_Delayed(float time){

            StartCoroutine("WideScreenIn_Buff", time);

        }//WidescreenIN_Delayed

        private IEnumerator WideScreenIn_Buff(float time){

            yield return new WaitForSeconds(time);

            Widescreen_IN();

        }//WideScreenIn_Buff

        public void Widescreen_IN(){

            holder.SetActive(true);
            anim.Play(show.name);

        }//Widescreen_IN

        public void Widescreen_IN(float time){

            holder.SetActive(true);
            anim.Play(show.name);

            StartCoroutine("WidescreenBuff", time);

        }//Widescreen_IN

        private IEnumerator WidescreenBuff(float time){

            yield return new WaitForSeconds(time);

            Widescreen_OUT();

        }//WidescreenBuff


    //////////////////////
    ///
    ///     OUT ACTIONS
    ///
    //////////////////////


        public void WidescreenOUT_Delayed(float time){

            StartCoroutine("WidescreenOUT_Buff", time);

        }//WidescreenOUT_Delayed

        private IEnumerator WidescreenOUT_Buff(float time){

            yield return new WaitForSeconds(time);

            Widescreen_OUT();

        }//WidescreenOUT_Buff

        public void Widescreen_OUT(){

            anim.Play(hide.name);

            StartCoroutine("HideBuff");

        }//Widescreen_OUT


    //////////////////////////////////////
    ///
    ///     FINISH ACTIONS
    ///
    ///////////////////////////////////////


        private IEnumerator HideBuff(){

            yield return new WaitForSeconds(hide.length + 0.4f);

            holder.SetActive(false);

        }//HideBuff


    }//SimpleWidescreen


}//namespace