using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Player/Material Controller Connect")]
    public class HFPS_MaterialCont_Con : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Start() {}//start


    //////////////////////////////////////
    ///
    ///     STATE ACTIONS
    ///
    ///////////////////////////////////////


        public void Mesh_State(bool state){

            HFPS_MaterialCont.instance.Mesh_State(state);

        }//Mesh_State

        public void All_State(bool state){

            HFPS_MaterialCont.instance.All_State(state);

        }//All_State


    //////////////////////////////////////
    ///
    ///     CUR SLOT ACTIONS
    ///
    ///////////////////////////////////////


        public void CurSlot_Set(int slot){

            HFPS_MaterialCont.instance.CurSlot_Set(slot);

        }//CurSlot_Set

        public void CurSlot_Reset(){

            HFPS_MaterialCont.instance.CurSlot_Reset();

        }//CurSlot_Reset


    }//HFPS_MaterialCont_Con


}//namespace