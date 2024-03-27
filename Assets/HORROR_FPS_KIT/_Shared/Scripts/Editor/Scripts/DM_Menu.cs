using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using DizzyMedia.Shared;
using DizzyMedia.Utility;

public class DM_Menu : EditorWindow {
    
    
    [MenuItem("Tools/Dizzy Media/Assets/", false, 0)]
    [MenuItem("Tools/Dizzy Media/Assets/━━▲━━", false, 0)]
    
    [MenuItem("Tools/Dizzy Media/Extensions/", false, 11)]
    [MenuItem("Tools/Dizzy Media/Extensions/━━▲━━", false, 10)]
    
    [MenuItem("Tools/Dizzy Media/Shared/", false, 11)]
    [MenuItem("Tools/Dizzy Media/Shared/━━▲━━", false , 11)]
    [MenuItem("Tools/Dizzy Media/Shared/Components/", false , 22)]
    
    [MenuItem("Tools/Dizzy Media/Utilities/", false, 11)]
    [MenuItem("Tools/Dizzy Media/Utilities/━━▲━━", false , 11)]
    
    
    
//////////////////////////////////////
///
///     MENU BUTTONS
///
///////////////////////////////////////
    
////////////////////////////////
///
///     UTILITIES CREATE
///
////////////////////////////////
    
////////////////////
///
///     EFFECTS
///
////////////////////
    
    
    #if (COMPONENTS_PRESENT || PUZZLER_PRESENT)
    
        [MenuItem("Tools/Dizzy Media/Utilities/Effects/Dissolve Controller", false , 22)]
        public static void Create_DissolveCont() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<DM_DissolveCont>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_DissolveCont 
    
    #endif
        
    #if (COMPONENTS_PRESENT || DM_AD_PRESENT)
    
        [MenuItem("Tools/Dizzy Media/Utilities/Effects/Simple Pulse", false, 22)]
        public static void Create_SimpPulse() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<SimplePulse>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SimpPulse
    
    #endif
    
    
////////////////////
///
///     GIZMOS
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Utilities/Gizmos/Simple Icon", false, 22)]
    public static void Create_SimpIcon() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<SimpleIcon>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_SimpIcon
    
    [MenuItem("Tools/Dizzy Media/Utilities/Gizmos/Transform Forward", false , 22)]
    public static void Create_TransForward() {

        if(Selection.gameObjects.Length > 0){

            Selection.gameObjects[0].AddComponent<TransForward>();

        //Selection > 0
        } else {

            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

        }//Selection > 0

    }//Create_TransForward
    
    [MenuItem("Tools/Dizzy Media/Utilities/Gizmos/Transform Indicator", false, 22)]
    public static void Create_TransInd() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<TransInd>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_TransInd
    
    
////////////////////
///
///     HFPS
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Utilities/HFPS/Mini Audio", false, 22)]
    public static void Create_MiniAudio() {

        if(Selection.gameObjects.Length > 0){

            Selection.gameObjects[0].AddComponent<HFPS_MiniAudio>();

        //Selection > 0
        } else {

            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

        }//Selection > 0

    }//Create_MiniAudio
        
    #if (COMPONENTS_PRESENT || EASYHIDE_PRESENT || PUZZLER_PRESENT || HFPS_VENDOR_PRESENT)
        
        [MenuItem("Tools/Dizzy Media/Utilities/HFPS/Scare Handler", false , 22)]
        public static void Create_ScareHand() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<ScareHand>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ScareHand
    
    #endif
        
        
////////////////////
///
///     WORLD
///
////////////////////
    
        
    #if COMPONENTS_PRESENT
    
        [MenuItem("Tools/Dizzy Media/Utilities/World/Forward Detect", false , 22)]
        public static void Create_ForwardDetect() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<DM_ForwardDetect>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ForwardDetect
    
    #endif
    
    
////////////////////
///
///     UI
///
////////////////////
    
    
    #if (DM_AD_PRESENT || DM_TD_PRESENT)
    
        [MenuItem("Tools/Dizzy Media/Utilities/UI/Input Hold Handler", false, 22)]
        public static void Create_InputHold() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<InputHold_Handler>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_InputHold
        
    #endif
        
    #if DM_AD_PRESENT

        [MenuItem("Tools/Dizzy Media/Utilities/UI/Simple Subtitles", false, 22)]
        public static void Create_SimpleSubs() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<DM_Subtitles>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SimpleSubs

        [MenuItem("Tools/Dizzy Media/Utilities/UI/Simple Subtitles Controller", false, 22)]
        public static void Create_SimpleSubsCont() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<DM_Subtitles_Cont>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SimpleSubsCont
    
    #endif
    
    
////////////////////////////////
///
///     SHARED CREATE
///
////////////////////////////////
    
/////////////////////////
///
///     ACTION BAR
///
/////////////////////////
    
    
    #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || EASYHIDE_PRESENT || PUZZLER_PRESENT || HFPS_VENDOR_PRESENT)
        
        [MenuItem("Tools/Dizzy Media/Shared/Systems/", false , 22)]
    
        [MenuItem("Tools/Dizzy Media/Shared/Systems/Action Bar/Action Bar", false , 22)]
        public static void Create_ActBar() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<DM_ActionBar>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ActBar
    
        [MenuItem("Tools/Dizzy Media/Shared/Systems/Action Bar/Action Bar Controller", false , 22)]
        public static void Create_ActBarCont() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<DM_ActionBarsCont>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ActBarCont

    #endif
        
        
/////////////////////////
///
///     HIT
///
/////////////////////////
        
        
    [MenuItem("Tools/Dizzy Media/Shared/Components/Hit/Hit Receiver", false , 22)]
    public static void Create_HitReceiver() {

        if(Selection.gameObjects.Length > 0){

            Selection.gameObjects[0].AddComponent<HFPS_HitReceiver>();

        //Selection > 0
        } else {

            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

        }//Selection > 0

    }//Create_HitReceiver
        
        
/////////////////////////
///
///     PLAYER
///
/////////////////////////
        
    
    #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)
        
        [MenuItem("Tools/Dizzy Media/Shared/Components/Player/References", false , 22)]
        public static void Create_Refs() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_References>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_Refs
    
    #endif
        
    
/////////////////////////
///
///     UI
///
/////////////////////////
    
/////////////////
///
///     DISPLAY
///
/////////////////
    
        
    #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || EASYHIDE_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)
    
        [MenuItem("Tools/Dizzy Media/Shared/Components/UI/Display/Simple Fade", false , 22)]
        public static void Create_SimpleFade() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<SimpleFade>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SimpleFade
    
    #endif
        
    
/////////////////
///
///     MENU
///
/////////////////

        
    #if (COMPONENTS_PRESENT || HFPS_DURABILITY_PRESENT || PUZZLER_PRESENT || HFPS_SHOOTRANGE_PRESENT || HFPS_VENDOR_PRESENT)

        [MenuItem("Tools/Dizzy Media/Shared/Components/UI/Menu/UI Controller", false , 22)]
        public static void Create_UICont() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_UICont>();

                //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_UICont
    
    
/////////////////////////
///
///     WORLD
///
///////////////////////// 
    
///////////////////
///
///     PLAYER
///
///////////////////
    
    
        [MenuItem("Tools/Dizzy Media/Shared/Components/World/Player/Character Action", false , 22)]
        public static void Create_CharAct() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_CharacterAction>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CharAct
    
    #endif
        
        
///////////////////
///
///     SCENE
///
///////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Shared/Components/World/Scene/Timer", false , 22)]
    public static void Create_DMTimer() {

        if(Selection.gameObjects.Length > 0){

            Selection.gameObjects[0].AddComponent<DM_Timer>();

        //Selection > 0
        } else {

            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

        }//Selection > 0

    }//Create_DMTimer  
    
    
}//DM_Menu
