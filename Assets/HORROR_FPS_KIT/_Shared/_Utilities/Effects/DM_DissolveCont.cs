using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.Utility {

    [AddComponentMenu("Dizzy Media/Utilities/Effects/Dissolve Controller")]
    public class DM_DissolveCont : MonoBehaviour {


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [System.Serializable]
        public class DissolveMesh {

            [Space]

            public string name;
            public Dissolve_Type dissolveType;

            [Space]

            public Mesh_Type meshType;
            public MeshRenderer meshRenderer;
            public SkinnedMeshRenderer skinnedMesh;
            public int materialSlot; 

            [Space]

            public bool useCustomSpeed;
            public float dissolveSpeed;

            [Space]

            [Header("Auto")]

            public Material[] mats;
            public float amount = 0f;

        }//DissolveMesh

        [System.Serializable]
        public class Finish_Action {

            [Space]

            public Finish_Type finishType;

            [Space]

            public bool useFinishDelay;
            public float finishWait;

        }//Finish_Action


    //////////////////////////
    //
    //      ENUMS
    //
    //////////////////////////


        public enum Dissolve_Type {

            In = 0,
            Out = 1,

        }//Dissolve_Type

        public enum Start_Type {

            Nothing = 0,
            DissolveInAll = 1,
            DissolveOutAll = 2,
            DissolveInSlot = 3,
            DissolveOutSlot = 4,

        }//StartType

        public enum Finish_Type {

            None = 0,
            Destroy = 1,
            Disable = 2,

        }//FinishType

        public enum Mesh_Type {

            MeshRenderer = 0,
            SkinnedMeshRenderer = 1,

        }//Mesh_Type


    //////////////////////////
    //
    //      VALUES
    //
    //////////////////////////

    ///////////////
    //
    //   REFERENCES
    //
    ///////////////


        [Space]

        [Header("References")]

        [Space]

        public List<DissolveMesh> dissolveMeshes;


    ///////////////
    //
    //   SPEED OPTIONS
    //
    ///////////////


        [Space]

        [Header("User Options")]

        [Space]

        public bool autoStart;
        public bool delayStart;
        public float startWait;

        public Start_Type startType;
        public int startSlot;

        [Space]

        public float speedGlobal = 0.5f;

        [Space]

        public Finish_Action finishAction;


    ///////////////
    //
    //   AUTO
    //
    ///////////////


        [Space]

        [Header("Auto")]

        [Space]

        public Dissolve_Type curDissolveType;

        public int tempSlot = -1;
        public bool dissolving;
        public bool dissolveAll;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Start(){

            StartInit();

        }//Start

        public void StartInit(){

            tempSlot = -1;
            dissolving = false;

            if(dissolveMeshes.Count > 0){

                for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                    dissolveMeshes[dm].amount = 0;

                    if(dissolveMeshes[dm].mats.Length < 1){

                        if(dissolveMeshes[dm].meshType == Mesh_Type.MeshRenderer){

                            dissolveMeshes[dm].mats = dissolveMeshes[dm].meshRenderer.materials;

                        }//meshType = MeshRenderer

                        if(dissolveMeshes[dm].meshType == Mesh_Type.SkinnedMeshRenderer){

                            dissolveMeshes[dm].mats = dissolveMeshes[dm].skinnedMesh.materials;

                        }//meshType = skinned mesh

                    }//mats.Length < 1

                }//for dm dissolveMeshes

            }//dissolveMeshes.Count > 0

            if(autoStart){

                if(delayStart){

                    StartCoroutine("StartDissolve_Wait");

                //delayStart
                } else {

                    StartDissolve();

                }//delayStart

            }//autoStart

        }//StartInit

        private IEnumerator StartDissolve_Wait(){

            yield return new WaitForSeconds(startWait);

            StartDissolve();

        }//StartDissolve_Wait

        private void StartDissolve(){

            if(startType == Start_Type.DissolveInAll){

                Dissolve_In();

            }//startType = DissolveInAll

            if(startType == Start_Type.DissolveOutAll){

                Dissolve_Out();

            }//startType = DissolveOutAll

            if(startType == Start_Type.DissolveInSlot){

                Dissolve_In(startSlot);

            }//startType = DissolveInSlot

            if(startType == Start_Type.DissolveOutSlot){

                Dissolve_Out(startSlot);

            }//startType = DissolveOutSlot

        }//StartDissolve


    //////////////////////////
    //
    //      UPDATE ACTIONS
    //
    //////////////////////////


        void Update(){

            if(dissolving){

                if(curDissolveType == Dissolve_Type.In){

                    if(dissolveAll){

                        if(dissolveMeshes.Count > 0){

                            for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                                if(dissolveMeshes[dm].mats.Length > 0){

                                    if(dissolveMeshes[dm].amount > 0){

                                        dissolveMeshes[dm].amount -= Time.deltaTime;

                                        if(dissolveMeshes[dm].useCustomSpeed){

                                            dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[dm].amount * dissolveMeshes[dm].dissolveSpeed));

                                        //useCustomSpeed
                                        } else {

                                            dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[dm].amount * speedGlobal));

                                        }//useCustomSpeed

                                    //amount > 0
                                    } else {

                                        dissolving = false;
                                        dissolveAll = false;
                                        dissolveMeshes[dm].amount = 0;

                                        dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", 0);

                                        Finish_Check();

                                    }//amount > 0

                                }//mats.Length > 0

                            }//for dm dissolveMeshes

                        }//dissolveMeshes.Count > 0

                    //dissolveAll
                    } else {

                        if(tempSlot > -1){

                            if(dissolveMeshes.Count > 0){

                                if(dissolveMeshes[tempSlot - 1].mats.Length > 0){

                                    if(dissolveMeshes[tempSlot - 1].amount > 0){

                                        dissolveMeshes[tempSlot - 1].amount -= Time.deltaTime;

                                        if(dissolveMeshes[tempSlot - 1].useCustomSpeed){

                                            dissolveMeshes[tempSlot - 1].mats[dissolveMeshes[tempSlot - 1].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[tempSlot - 1].amount * dissolveMeshes[tempSlot - 1].dissolveSpeed));

                                        //useCustomSpeed
                                        } else {

                                            dissolveMeshes[tempSlot - 1].mats[dissolveMeshes[tempSlot - 1].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[tempSlot - 1].amount * speedGlobal));

                                        }//useCustomSpeed

                                    //amount > 0
                                    } else {

                                        dissolving = false;
                                        dissolveMeshes[tempSlot - 1].amount = 0;

                                        dissolveMeshes[tempSlot - 1].mats[dissolveMeshes[tempSlot - 1].materialSlot].SetFloat("_Cutoff", 0);

                                        Finish_Check();

                                    }//amount > 0

                                }//mats.Length > 0

                            }//dissolveMeshes.Count > 0

                        }//tempSlot > -1

                    }//dissolveAll

                }//curDissolveType = In

                if(curDissolveType == Dissolve_Type.Out){

                    if(dissolveAll){

                        if(dissolveMeshes.Count > 0){

                            for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                                if(dissolveMeshes[dm].mats.Length > 0){

                                    if(dissolveMeshes[dm].amount < 2){

                                        dissolveMeshes[dm].amount += Time.deltaTime;

                                        if(dissolveMeshes[dm].useCustomSpeed){

                                            dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[dm].amount * dissolveMeshes[dm].dissolveSpeed));

                                        //useCustomSpeed
                                        } else {

                                            dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[dm].amount * speedGlobal));

                                        }//useCustomSpeed

                                    //amount < 2
                                    } else {

                                        dissolving = false;
                                        dissolveAll = false;
                                        dissolveMeshes[dm].amount = 2;

                                        dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", 1);

                                        Finish_Check();

                                    }//amount < 2

                                }//mats.Length > 0

                            }//for dm dissolveMeshes

                        }//dissolveMeshes.Count > 0

                    //dissolveAll
                    } else {

                        if(tempSlot > -1){

                            if(dissolveMeshes.Count > 0){

                                if(dissolveMeshes[tempSlot - 1].mats.Length > 0){

                                    if(dissolveMeshes[tempSlot - 1].amount < 2){

                                        dissolveMeshes[tempSlot - 1].amount += Time.deltaTime;

                                        if(dissolveMeshes[tempSlot - 1].useCustomSpeed){

                                            dissolveMeshes[tempSlot - 1].mats[dissolveMeshes[tempSlot - 1].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[tempSlot - 1].amount * dissolveMeshes[tempSlot - 1].dissolveSpeed));

                                        //useCustomSpeed
                                        } else {

                                            dissolveMeshes[tempSlot - 1].mats[dissolveMeshes[tempSlot - 1].materialSlot].SetFloat("_Cutoff", Mathf.Sin(dissolveMeshes[tempSlot - 1].amount * speedGlobal));

                                        }//useCustomSpeed

                                    //amount < 2
                                    } else {

                                        dissolving = false;
                                        dissolveMeshes[tempSlot - 1].amount = 2;

                                        dissolveMeshes[tempSlot - 1].mats[dissolveMeshes[tempSlot - 1].materialSlot].SetFloat("_Cutoff", 1);

                                        Finish_Check();

                                    }//amount < 2

                                }//mats.Length > 0

                            }//dissolveMeshes.Count > 0

                        }//tempSlot > -1

                    }//dissolveAll

                }//curDissolveType = Out

            }//dissolving

        }//Update


    //////////////////////////
    //
    //      DISSOLVE ACTIONS
    //
    //////////////////////////

    ///////////////
    //
    //      IN
    //
    ///////////////


        public void Dissolve_In(int slot){

            curDissolveType = Dissolve_Type.In;
            tempSlot = slot;

            dissolveMeshes[slot - 1].amount = 2;

            dissolving = true;

        }//Dissolve_In

        public void Dissolve_In(){

            curDissolveType = Dissolve_Type.In;
            tempSlot = -1;

            for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                dissolveMeshes[dm].amount = 2;

            }//for dm dissolveMeshes

            dissolveAll = true;
            dissolving = true;

        }//Dissolve_In


    ///////////////
    //
    //      OUT
    //
    ///////////////


        public void Dissolve_Out(int slot){

            curDissolveType = Dissolve_Type.Out;
            tempSlot = slot;

            dissolveMeshes[slot - 1].amount = 0;

            dissolving = true;

        }//Dissolve_Out

        public void Dissolve_Out(){

            curDissolveType = Dissolve_Type.Out;
            tempSlot = -1;

            for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                dissolveMeshes[dm].amount = 0;

            }//for dm dissolveMeshes

            dissolveAll = true;
            dissolving = true;

        }//Dissolve_Out


    ///////////////
    //
    //      QUICK IN
    //
    ///////////////


        public void DissolveQuick_In(int slot){

            curDissolveType = Dissolve_Type.In;

            if(dissolveMeshes.Count > 0){

                if(dissolveMeshes[slot - 1].mats.Length > 0){

                    dissolveMeshes[slot - 1].mats[dissolveMeshes[slot - 1].materialSlot].SetFloat("_Cutoff", 0);

                }//mats.Length > 0

            }//dissolveMeshes.Count > 0

        }//DissolveQuick_In

        public void DissolveQuick_In(){

            curDissolveType = Dissolve_Type.In;

            if(dissolveMeshes.Count > 0){

                for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                    if(dissolveMeshes[dm].mats.Length > 0){

                        dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", 0);

                    }//mats.Length > 0

                }//for dm dissolveMeshes

            }//dissolveMeshes.Count > 0

        }//DissolveQuick_In


    ///////////////
    //
    //      QUICK OUT
    //
    ///////////////


        public void DissolveQuick_Out(int slot){

            curDissolveType = Dissolve_Type.Out;

            if(dissolveMeshes.Count > 0){

                if(dissolveMeshes[slot - 1].mats.Length > 0){

                    dissolveMeshes[slot - 1].mats[dissolveMeshes[slot - 1].materialSlot].SetFloat("_Cutoff", 1);

                }//mats.Length > 0

            }//dissolveMeshes.Count > 0

        }//DissolveQuick_Out

        public void DissolveQuick_Out(){

            curDissolveType = Dissolve_Type.Out;

            if(dissolveMeshes.Count > 0){

                for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                    if(dissolveMeshes[dm].mats.Length > 0){

                        dissolveMeshes[dm].mats[dissolveMeshes[dm].materialSlot].SetFloat("_Cutoff", 1);

                    }//mats.Length > 0

                }//for dm dissolveMeshes

            }//dissolveMeshes.Count > 0

        }//DissolveQuick_Out


    //////////////////////////
    //
    //      FINISH ACTIONS
    //
    //////////////////////////


        private void Finish_Check(){

            int tempCount = 0;

            if(finishAction.finishType != Finish_Type.None){

                if(dissolveMeshes.Count > 0){

                    for(int dm = 0; dm < dissolveMeshes.Count; dm++){

                        if(dissolveMeshes[dm].dissolveType == Dissolve_Type.In){

                            if(dissolveMeshes[dm].amount == 0){

                                tempCount += 1;

                            }//amount = 0

                        }//dissolveType = In

                        if(dissolveMeshes[dm].dissolveType == Dissolve_Type.Out){

                            if(dissolveMeshes[dm].amount == 2){

                                tempCount += 1;

                            }//amount = 2

                        }//dissolveType = In

                    }//for dm dissolveMeshes

                    if(tempCount == dissolveMeshes.Count){

                        if(finishAction.useFinishDelay){

                            StartCoroutine("FinishCheckDone_Buff");

                        //useFinishDelay
                        } else {

                            FinishCheck_Done();

                        }//useFinishDelay

                    }//tempCount = dissolveMeshes.Count

                }//dissolveMeshes.Count > 0

            }//finishType != None

        }//Finish_Check

        private IEnumerator FinishCheckDone_Buff(){

            yield return new WaitForSeconds(finishAction.finishWait);

            FinishCheck_Done();

        }//FinishCheckDone_Buff

        private void FinishCheck_Done(){

            if(finishAction.finishType == Finish_Type.Destroy){

                Destroy(this.gameObject);

            }//finishType = destroy

            if(finishAction.finishType == Finish_Type.Disable){

                this.gameObject.SetActive(false);

            }//finishType = disable

        }//FinishCheck_Done


    }//DM_DissolveCont


}//namespace
