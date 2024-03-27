using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Player/Material Controller")]
    public class HFPS_MaterialCont : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     INSTANCE
    ///
    ///////////////////////////////////////


        public static HFPS_MaterialCont instance;


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class BodyParts {

            public string name;

            [Space]

            public List<MeshParts> meshParts;

        }//BodyParts

        [Serializable]
        public class MeshParts {

            [Space]

            public string name;

            [Space]

            public SkinnedMeshRenderer skinnedMesh;

            [Space]

            public int matSlot;
            public Material originalMat;
            public Material invisMat;

            [Header("Auto")]

            public string nameOfMesh;

        }//MeshParts


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     USER OPTIONS
    ///
    ///////////////////////////


        public List<BodyParts> bodyParts;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public int curSlot;
        public List<int> savedSlots;
        public bool locked;

        public int tabs;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            instance = this;

        }//Awake

        void Start() {

            curSlot = 0;
            savedSlots = new List<int>();

            locked = false;

        }//start


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void Mesh_State(bool state){

            if(curSlot > 0){

                if(bodyParts.Count > 0){

                    if(bodyParts[curSlot - 1].meshParts.Count > 0){

                        for(int i = 0; i < bodyParts[curSlot - 1].meshParts.Count; ++i ) {

                            Material[] tempMats = bodyParts[curSlot - 1].meshParts[i].skinnedMesh.materials;

                            if(state){

                                if(bodyParts[curSlot - 1].meshParts[i].skinnedMesh.materials[bodyParts[curSlot - 1].meshParts[i].matSlot] != bodyParts[curSlot - 1].meshParts[i].originalMat){

                                    tempMats[bodyParts[curSlot - 1].meshParts[i].matSlot] = bodyParts[curSlot - 1].meshParts[i].originalMat;

                                }//mat = original mat

                                if(savedSlots.Contains(curSlot)){

                                    savedSlots.Remove(curSlot);

                                }//Contains

                            //state
                            } else {

                                if(bodyParts[curSlot - 1].meshParts[i].skinnedMesh.materials[bodyParts[curSlot - 1].meshParts[i].matSlot] != bodyParts[curSlot - 1].meshParts[i].invisMat){

                                    tempMats[bodyParts[curSlot - 1].meshParts[i].matSlot] = bodyParts[curSlot - 1].meshParts[i].invisMat;

                                }//mat = invisible mat

                                if(!savedSlots.Contains(curSlot)){

                                    savedSlots.Add(curSlot);

                                }//!Contains

                            }//state

                            bodyParts[curSlot - 1].meshParts[i].skinnedMesh.materials = tempMats;

                        }//for i meshParts

                    }//meshParts.Count > 0

                }//bodyParts.Count > 0

            //curSlot > 0
            } else {

                Debug.Log("No Slot Set");

            }//curSlot > 0

        }//Mesh_State

        public void Mesh_State(int slot, bool state){

            if(bodyParts.Count > 0){

                if(bodyParts[slot - 1].meshParts.Count > 0){

                    for(int i = 0; i < bodyParts[slot - 1].meshParts.Count; ++i ) {

                        Material[] tempMats = bodyParts[slot - 1].meshParts[i].skinnedMesh.materials;

                        if(state){

                            if(bodyParts[slot - 1].meshParts[i].skinnedMesh.materials[bodyParts[slot - 1].meshParts[i].matSlot] != bodyParts[slot - 1].meshParts[i].originalMat){

                                tempMats[bodyParts[slot - 1].meshParts[i].matSlot] = bodyParts[slot - 1].meshParts[i].originalMat;

                            }//mat = original mat

                            if(savedSlots.Contains(slot)){

                                savedSlots.Remove(slot);

                            }//Contains

                        //state
                        } else {

                            if(bodyParts[slot - 1].meshParts[i].skinnedMesh.materials[bodyParts[slot - 1].meshParts[i].matSlot] != bodyParts[slot - 1].meshParts[i].invisMat){

                                tempMats[bodyParts[slot - 1].meshParts[i].matSlot] = bodyParts[slot - 1].meshParts[i].invisMat;

                            }//mat = invisible mat

                            if(!savedSlots.Contains(slot)){

                                savedSlots.Add(slot);

                            }//!Contains

                        }//state

                        bodyParts[slot - 1].meshParts[i].skinnedMesh.materials = tempMats;

                    }//for i meshParts

                }//meshParts.Count > 0

            }//bodyParts.Count > 0

        }//Mesh_State

        public void All_State(bool state){

            if(bodyParts.Count > 0){

                for(int b = 0; b < bodyParts.Count; ++b ) {

                    if(bodyParts[b].meshParts.Count > 0){

                        for(int m = 0; m < bodyParts[b].meshParts.Count; ++m ) {

                            Material[] tempMats = bodyParts[b].meshParts[m].skinnedMesh.materials;

                            if(state){

                                if(bodyParts[b].meshParts[m].skinnedMesh.materials[bodyParts[b].meshParts[m].matSlot] != bodyParts[b].meshParts[m].originalMat){

                                    tempMats[bodyParts[b].meshParts[m].matSlot] = bodyParts[b].meshParts[m].originalMat;

                                }//mat = original mat

                                int tempInt = b + 1;

                                if(savedSlots.Contains(tempInt)){

                                    savedSlots.Remove(tempInt);

                                }//Contains

                            //state
                            } else {

                                if(bodyParts[b].meshParts[m].skinnedMesh.materials[bodyParts[b].meshParts[m].matSlot] != bodyParts[b].meshParts[m].invisMat){

                                    tempMats[bodyParts[b].meshParts[m].matSlot] = bodyParts[b].meshParts[m].invisMat;

                                }//mat = invisible mat

                                int tempInt = b + 1;

                                if(!savedSlots.Contains(tempInt)){

                                    savedSlots.Add(tempInt);

                                }//!Contains

                            }//state

                            bodyParts[b].meshParts[m].skinnedMesh.materials = tempMats;

                        }//for m meshParts

                    }//meshParts.Count > 0

                }//for b bodyParts

            }//bodyParts.Count > 0

        }//All_State


    //////////////////////////////////////
    ///
    ///     CUR SLOT ACTIONS
    ///
    ///////////////////////////////////////


        public void CurSlot_Set(int slot){

            curSlot = slot;

        }//CurSlot_Set

        public void CurSlot_Reset(){

            curSlot = 0;

        }//CurSlot_Reset


    //////////////////////////
    //
    //      LOCK ACTIONS
    //
    //////////////////////////


        public void LockState(bool state){

            locked = state;

        }//LockState


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"savedSlots", savedSlots },
                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            savedSlots = token["savedSlots"].ToObject<List<int>>();
            locked = (bool)token["locked"];

            StartCoroutine("Load_Buff");

        }//OnLoad

        private IEnumerator Load_Buff(){

            yield return new WaitForSeconds(0.1f);

            if(savedSlots.Count > 0){

                for(int i = 0; i < savedSlots.Count; ++i ) {

                    Mesh_State(savedSlots[i], false);

                }//for i savedSlots

            }//savedSlots.Count > 0

        }//Load_Buff


    }//HFPS_MaterialCont


}//namespace