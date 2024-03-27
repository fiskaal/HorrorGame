using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/UI/Display/Complex Notifications/Complex Notifications Connect")]
    public class ComplexNotifications_Con : MonoBehaviour {


    //////////////////////////////////////
    ///
    ///     VALUES
    ///
    ///////////////////////////////////////


        public string name;
        public float showWait;
        public List<string> texts;


    ///////////////////////////////////////
    //
    //      START ACTIONS
    //
    ///////////////////////////////////////


        void Start() {}//start


    ///////////////////////////////////////
    //
    //      NOTIFICATION ACTIONS
    //
    ///////////////////////////////////////


        public void Notification_Show(){

            if(ComplexNotifications.instance != null){

                if(texts.Count == 0){

                    ComplexNotifications.instance.Notification_Show(name);

                //texts.Count = 0
                } else {

                    ComplexNotifications.instance.Notification_Show(name, texts);

                }//texts.Count = 0

            }//instance != null

        }//Notification_Show

        public void Notification_Show(string name){

            if(ComplexNotifications.instance != null){

                if(texts.Count == 0){

                    ComplexNotifications.instance.Notification_Show(name);

                //texts.Count = 0
                } else {

                    ComplexNotifications.instance.Notification_Show(name, texts);

                }//texts.Count = 0

            }//instance != null

        }//Notification_Show

        public void Notification_Show(int slot){

            if(ComplexNotifications.instance != null){

                if(texts.Count == 0){

                    ComplexNotifications.instance.Notification_Show(slot);

                //texts.Count = 0
                } else {

                    ComplexNotifications.instance.Notification_Show(slot, texts);

                }//texts.Count = 0

            }//instance != null

        }//Notification_Show

        public void Notification_ShowDelayed(string name){

            if(ComplexNotifications.instance != null){

                ComplexNotifications.instance.tempWait = showWait;
                ComplexNotifications.instance.Notification_ShowDelayed(name);

            }//instance != null

        }//Notification_ShowDelayed

        public void Notification_ShowDelayed(int slot){

            if(ComplexNotifications.instance != null){

                ComplexNotifications.instance.tempWait = showWait;
                ComplexNotifications.instance.Notification_ShowDelayed(slot);

            }//instance != null

        }//Notification_ShowDelayed


    }//ComplexNotifications_Con


}//namespace