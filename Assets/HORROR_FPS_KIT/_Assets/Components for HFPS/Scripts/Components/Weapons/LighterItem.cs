using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ThunderWire.Utility;
using Newtonsoft.Json.Linq;
using HFPS.Systems;
using HFPS.UI;

namespace HFPS.Player {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Weapons/Lighter")]
    public class LighterItem : SwitcherBehaviour, ISaveableArmsItem {

        [Serializable]
        public class Sound_Library {
            
            public string name;
            
            [Space]
            
            public AudioSource source;
            public AudioClip clip;
            public float volume;
            
        }//Sound Library
        
        private Inventory inventory;
        private Animation anim;

        [Header("Inventory")]
        [InventorySelector]
        public int itemID;

        [Header("Objects")]
        
        public GameObject flame;
        public GameObject light;
        public Transform FlamePosition;

        [Header("Animations")]
        public GameObject itemGO;
        public string DrawAnimation;
        public string HideAnimation;
        public string IdleAnimation;
        
        [Header("Lighter Animations")]
        public Animation lighterAnimation;
        public string lighterOpenAnimation;
        public string lighterCloseAnimation;
        
        [Space]

        public float DrawSpeed = 1f;
        public float HideSpeed = 1f;
        
        [Header("Sounds")]
        
        public List<Sound_Library> soundLibrary;

        private bool isSelected;
        private bool IsPressed;
        private bool IsBlocked;

        void Awake() {
            
            inventory = Inventory.Instance;
            anim = itemGO.GetComponent<Animation>();
        
        }//awake

        void Start() {
            
            if(isSelected) {
                
                OnSwitcherSelect();
            
            }//isSelected
            
        }//start
        
        public override void OnSwitcherSelect() {
            
            if(!anim.isPlaying) {
                
                itemGO.SetActive(true);
                anim.Play(DrawAnimation);
                
                StartCoroutine(OnSelect());
            
            }//!isPlaying
        
        }//OnSwitcherSelect
        
        IEnumerator OnSelect() {
            
            yield return new WaitUntil(() => !anim.isPlaying);
            isSelected = true;
        
        }//OnSelect

        public override void OnSwitcherDeselect() {
            
            if(itemGO.activeSelf && !anim.isPlaying && isSelected) {
                
                StopAllCoroutines();
                StartCoroutine(LighterHide());
                IsPressed = true;
            
            }
        
        }//OnSwitcherDeselect
        
        #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT)
        
            public override void OnSwitcherDeselect_Forced() {

                if(itemGO.activeSelf) {

                    StopAllCoroutines();
                    StartCoroutine(LighterHide());
                    IsPressed = true;

                }//itemGO.activeSelf

            }//OnSwitcherDeselect_Forced
        
        #endif
        
        public override void OnSwitcherActivate() {
            
            isSelected = true;
            itemGO.SetActive(true);
            flame.SetActive(true);
            light.SetActive(true);
            anim.Play(IdleAnimation);

        }//OnSwitcherActivate

        public override void OnSwitcherDeactivate() {
            
            StopAllCoroutines();
            isSelected = false;
            flame.SetActive(false);
            light.SetActive(false);
            itemGO.SetActive(false);
            
        }//OnSwitcherDeactivate
        
        public void BlowOut_Event() {
        
            soundLibrary[0].source.PlayOneShot(soundLibrary[0].clip, soundLibrary[0].volume);
        
        }//BlowOut_Event
        
        IEnumerator LighterHide() {
            
            anim.Play(HideAnimation);

            yield return new WaitUntil(() => !anim.isPlaying);

            IsPressed = true;
            isSelected = false;
        
        }//LighterHide

        public void OnItemBlock(bool blocked) {
            
            IsBlocked = blocked;
        
        }//OnItemBlock
        
        public void LighterOpen_Event() {
            
            soundLibrary[1].source.PlayOneShot(soundLibrary[1].clip, soundLibrary[1].volume);
            
            if(lighterAnimation != null){
                
                lighterAnimation.Play(lighterOpenAnimation);
                
            }//lighterAnimation
            
        }//LighterOpen_Event
        
        public void LighterClose_Event() {
            
            if(lighterAnimation != null){
                
                lighterAnimation.Play(lighterCloseAnimation);
                
            }//lighterAnimation
            
            soundLibrary[2].source.PlayOneShot(soundLibrary[2].clip, soundLibrary[2].volume);
            
            flame.SetActive(false);
            light.SetActive(false);
            
        }//LighterClose_Event
        
        public void LighterOn_Event() {
            
            soundLibrary[3].source.PlayOneShot(soundLibrary[3].clip, soundLibrary[3].volume);
            
            flame.SetActive(true);
            light.SetActive(true);
            
        }//LighterOn_Event
        
        public void LighterOff_Event() {
            
            flame.SetActive(false);
            light.SetActive(false);
            
        }//LighterOff_Event

        void Update() {
            
            flame.transform.position = FlamePosition.position;

            if(IsPressed && !(anim.isPlaying)) {
                
                itemGO.SetActive(false);
                IsPressed = false;
            
            }
            
        }//Update
        
        
//////////////////////////////////////
///
///     SAVE/LOAD ACTIONS
///
///////////////////////////////////////
    
    
        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {
            
                

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {
        
            

        }//OnLoad
        
        
    }//LighterItem
    

}//namespace player