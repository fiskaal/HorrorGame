using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Save System/Components Save Connect")]
    public class HFPS_CompSave_Con : MonoBehaviour {


    //////////////////////////
    //
    //      DATA LOAD & SAVE ACTIONS
    //
    //////////////////////////


        public void Data_Load(){

            HFPS_CompSave.instance.Data_Load();

        }//Data_Load

        public void Data_Save(){

            HFPS_CompSave.instance.Data_Save();

        }//Data_Save


    //////////////////////////
    //
    //      DATA SET ACTIONS
    //
    //////////////////////////

    ///////////////
    //
    //      OBJECTIVES
    //
    ///////////////


        public void Objectives_Catch(){

            HFPS_CompSave.instance.Objectives_Catch();

        }//Objectives_Catch


    //////////////////////////
    //
    //      DATA UPDATE ACTIONS
    //
    //////////////////////////

    ///////////////
    //
    //      OBJECTIVES
    //
    ///////////////


        public void Objectives_Update(){

            HFPS_CompSave.instance.Objectives_Update();

        }//Objectives_Update


    }//HFPS_CompSave_Con


}//namespace