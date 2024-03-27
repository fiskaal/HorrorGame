using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using DizzyMedia.Shared;

using HFPS.Player;
using HFPS.Editors;
using HFPS.Systems;

namespace DizzyMedia.HFPS_Components {

    public class Components_Menu : EditorWindow {

        
        [MenuItem("Tools/Dizzy Media/Assets/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Extensions/", false, 11)]
        [MenuItem("Tools/Dizzy Media/Assets/Shared/", false, 11)]
        [MenuItem("Tools/Dizzy Media/Assets/Utilities/", false, 11)]
        
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/", false, 11)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/━━▲━━", false, 11)]
        
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/", false, 22)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/AI/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Camera/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Destroyable/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Enemy/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Hit/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Input/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Player/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/UI/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Weapons/", false, 0)]
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/", false, 0)]
        
        #if COMPONENTS_PRESENT
        
            [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Helpers/", false, 22)]
            [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Helpers/Scene/", false , 0)]
            
        #endif
            
        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Helpers/Systems/", false, 0)]
        

    //////////////////////////////////////
    ///
    ///     MENU BUTTONS
    ///
    ///////////////////////////////////////

    ////////////////////////////////
    ///
    ///     COMPONENTS CREATE
    ///
    ////////////////////////////////

    /////////////////////////
    ///
    ///     AI
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/AI/Possessed", false , 31)]
        public static void Create_Possessed() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_Possessed>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_Possessed


    /////////////////////////
    ///
    ///     AI
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Camera/FOV/FOV Manager", false , 31)]
        public static void Create_FOVMan() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_FOVManager>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_FOVMan

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Camera/FOV/FOV Manager Connect", false , 31)]
        public static void Create_FOVManCon() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_FOVManagerCon>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_FOVManCon


    /////////////////////////
    ///
    ///     DESTROYABLE
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Destroyable/Destroyable", false , 31)]
        public static void Create_Destroyable() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_Destroyable>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_Destroyable

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Destroyable/Radial Detect", false , 31)]
        public static void Create_Radial() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_Destroyable>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_Radial


    /////////////////////////
    ///
    ///     ENEMY
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Enemy/Enemies Add To", false , 31)]
        public static void Create_EnemyAddTo() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_EnemiesAddTo>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_EnemyAddTo

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Enemy/Enemies Holder", false , 31)]
        public static void Create_EnemiesHold() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_EnemiesHolder>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_EnemiesHold


    /////////////////////////
    ///
    ///     HIT
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Hit/Health Spot", false , 31)]
        public static void Create_HealthSpot() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_HealthSpot>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_HealthSpot


    /////////////////////////
    ///
    ///     INPUT
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Input/Input Action", false , 31)]
        public static void Create_InputAction() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_InputAction>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_InputAction


    /////////////////////////
    ///
    ///     PLAYER
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Player/Audio Fader", false , 31)]
        public static void Create_AudFade() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_AudioFader>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_AudFade

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Player/Audio Fader Connect", false , 31)]
        public static void Create_AudFadeCon() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_AudioFaderCon>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_AudFadeCon

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Player/Material Controller", false , 31)]
        public static void Create_MatCont() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_MaterialCont>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_MatCont

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Player/Material Controller Connect", false , 31)]
        public static void Create_MatContCon() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_MaterialCont_Con>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_MatContCon

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Player/Player Manager", false , 31)]
        public static void Create_PlayerMan() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_PlayerMan>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_PlayerMan


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


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/UI/Display/Complex Notifications/Complex Notifications", false , 31)]
        public static void Create_CompNotifs() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<ComplexNotifications>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CompNotifs

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/UI/Display/Complex Notifications/Complex Notifications Connect", false , 31)]
        public static void Create_CompNotifsCon() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<ComplexNotifications_Con>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CompNotifsCon




    /////////////////////////
    ///
    ///     SYSTEMS
    ///
    /////////////////////////

    ////////////////////
    ///
    ///     IGNITABLE
    ///
    ////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Ignitable/Ignitable", false , 31)]
        public static void Create_Ignitable() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_Ignitable>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_Ignitable

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Ignitable/Ignite Handler", false , 31)]
        public static void Create_IgniteHand() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_IgniteHand>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_IgniteHand


    ////////////////////
    ///
    ///     SAVE SYSTEM
    ///
    ////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Save System/Components Save", false , 31)]
        public static void Create_CompSaveSys() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_IgniteHand>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CompSaveSys

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Save System/Components Save Connect", false , 31)]
        public static void Create_CompSaveCon() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_CompSave_Con>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CompSaveCon

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Save System/Components Save Loader", false , 31)]
        public static void Create_CompSaveLoader() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_CompSaveLoader>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CompSaveLoader


    ////////////////////
    ///
    ///     SCREEN
    ///
    ////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Screen Events/Screen Events", false , 31)]
        public static void Create_ScreenEvents() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_ScreenEvents>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ScreenEvents


    ////////////////////
    ///
    ///     WIDESCREEN
    ///
    ////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Widescreen/Simple Widescreen", false , 31)]
        public static void Create_SimpWide() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<SimpleWidescreen>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SimpWide

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Widescreen/Simple Widescreen Connect", false , 31)]
        public static void Create_SimpWideCon() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<SimpleWidescreen_Con  >();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SimpWideCon


    ////////////////////
    ///
    ///     SUB ACTIONS
    ///
    ////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Sub Actions/Sub Action", false , 31)]
        public static void Create_SubAct() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_SubAction>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SubAct

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Sub Actions/Sub Actions Handler", false , 31)]
        public static void Create_SubActsHand() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_SubActionsHandler>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SubActsHand

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Systems/Sub Actions/Sub Actions UI", false , 31)]
        public static void Create_SubActsUI() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_SubActionsUI>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SubActsUI


    /////////////////////////
    ///
    ///     WEAPONS
    ///
    /////////////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Weapons/Dual Wield", false , 31)]
        public static void Create_DualWield() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_DualWield>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_DualWield

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/Weapons/Lighter", false , 31)]
        public static void Create_Lighter() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<LighterItem>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_Lighter


    /////////////////////////
    ///
    ///     WORLD
    ///
    /////////////////////////

    ///////////////////
    ///
    ///     ITEMS
    ///
    ///////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Items/Start Items", false , 31)]
        public static void Create_StartItems() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_StartItems>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_StartItems


    ///////////////////
    ///
    ///     PLAYER
    ///
    ///////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Player/Player Attention", false , 31)]
        public static void Create_PlayAttent() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_PlayerAttention>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_PlayAttent


    ///////////////////
    ///
    ///     SCENE
    ///
    ///////////////////


        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Scene/HFPS Actions", false , 31)]
        public static void Create_HFPSActs() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_Actions>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_HFPSActs

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Scene/Active Handler", false , 31)]
        public static void Create_ActiveHand() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_ActiveHand>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ActiveHand

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Scene/Component Trigger", false , 31)]
        public static void Create_CompTrig() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_CompTrig>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_CompTrig

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Scene/Objective Push", false , 31)]
        public static void Create_ObjPush() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_ObjectivePush>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_ObjPush

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Scene/Scene Action", false , 31)]
        public static void Create_SceneAction() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_SceneAction>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SceneAction

        [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Components/World/Scene/Sound Library", false , 31)]
        public static void Create_SoundLib() {

            if(Selection.gameObjects.Length > 0){

                Selection.gameObjects[0].AddComponent<HFPS_SoundLibrary>();

            //Selection > 0
            } else {

                if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}

            }//Selection > 0

        }//Create_SoundLib



    ////////////////////////////////
    ///
    ///     HELPERS
    ///
    ////////////////////////////////

    /////////////////////////
    ///
    ///     SCENE
    ///
    /////////////////////////


        #if COMPONENTS_PRESENT

            [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Helpers/Scene/Scene Update (1.6.3a - 1.6.3b)", false , 31)]
            public static void SceneUpdate_163aNb() {

                bool gameUIselected = false;

                if(Selection.gameObjects.Length > 0){

                    for(int i = 0; i < Selection.gameObjects.Length; i++) {

                        if(Selection.gameObjects[i].GetComponent<SaveGameHandler>() != null){

                            gameUIselected = true;
                            break;

                        }//SaveGameHandler != null

                    }//for i selections

                }//Selection.gameObjects.Length > 0

                if(gameUIselected){

                    GameObject oldPlayer = null;
                    GameObject compPlayer = null;
                    GameObject newPlayer = null;

                    var playerConts = FindObjectsOfType<PlayerController>(true);
                    var gameMan = FindObjectsOfType<HFPS_GameManager>(true);
                    var saveGameHand = FindObjectsOfType<SaveGameHandler>(true);
                    SaveGameHandlerEditor[] tempSaveEditors = (SaveGameHandlerEditor[])Resources.FindObjectsOfTypeAll(typeof(SaveGameHandlerEditor));

                    if(saveGameHand.Length > 0){

                        saveGameHand[0].objectReferences = (ObjectReferences)Resources.Load("GameData/Components_ObjectReferences");

                        Debug.Log("Object References Updated for HFPS 1.6.3a - 1.6.3b");

                    }//saveGameHand.Length > 0

                    if(playerConts.Length > 0){

                        //Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[8].local);

                        oldPlayer = playerConts[0].gameObject;

                    }//playerConts.Length > 0

                    compPlayer = (GameObject)Resources.Load("Prefabs/Player/HEROPLAYER (custom)");

                    newPlayer = Instantiate(compPlayer);
                    newPlayer.name = "HEROPLAYER (custom)(New)";

                    newPlayer.transform.parent = oldPlayer.transform;
                    newPlayer.transform.localPosition = new Vector3(0, 0, 0);
                    newPlayer.transform.localEulerAngles = new Vector3(0, 0, 0);
                    newPlayer.transform.parent = null;

                    DestroyImmediate(oldPlayer);

                    Debug.Log("Player Updated for HFPS 1.6.3a - 1.6.3b");

                    if(gameMan.Length > 0){

                        gameMan[0].m_PlayerObj = newPlayer;

                        Debug.Log("Game Manager Updated for HFPS 1.6.3a - 1.6.3b");

                    }//gameMan.Length > 0

                    if(tempSaveEditors.Length > 0){

                        Debug.Log("Saveables Save Start");

                        tempSaveEditors[0].FindSaveables_Start();

                    }//tempSaveEditors.Length > 0

                //gameUIselected
                } else {

                    Debug.Log("Game UI object NOT SELECTED | Save Game Handler NOT SELECTED");
                    Debug.Log("SELECT _GAMEUI | SELECT SAVE GAME HANDLER SCRIPT OBJECT");

                }//gameUIselected

            }//SceneUpdate_163aNb

            [MenuItem("Tools/Dizzy Media/Assets/Components for HFPS/Helpers/Scene/Scene Update (1.6.3c)", false , 31)]
            public static void SceneUpdate_163c() {

                bool gameUIselected = false;

                if(Selection.gameObjects.Length > 0){

                    for(int i = 0; i < Selection.gameObjects.Length; i++) {

                        if(Selection.gameObjects[i].GetComponent<SaveGameHandler>() != null){

                            gameUIselected = true;
                            break;

                        }//SaveGameHandler != null

                    }//for i selections

                }//Selection.gameObjects.Length > 0

                if(gameUIselected){

                    GameObject oldPlayer = null;
                    GameObject compPlayer = null;
                    GameObject newPlayer = null;

                    var playerConts = FindObjectsOfType<PlayerController>(true);
                    var gameMan = FindObjectsOfType<HFPS_GameManager>(true);
                    var saveGameHand = FindObjectsOfType<SaveGameHandler>(true);
                    SaveGameHandlerEditor[] tempSaveEditors = (SaveGameHandlerEditor[])Resources.FindObjectsOfTypeAll(typeof(SaveGameHandlerEditor));

                    if(saveGameHand.Length > 0){

                        saveGameHand[0].objectReferences = (ObjectReferences)Resources.Load("GameData/Components_ObjectReferences 1.6.3c");

                        Debug.Log("Object References Updated for HFPS 1.6.3c");

                    }//saveGameHand.Length > 0

                    if(playerConts.Length > 0){

                        //Debug.Log(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].singleValues[8].local);

                        oldPlayer = playerConts[0].gameObject;

                    }//playerConts.Length > 0

                    compPlayer = (GameObject)Resources.Load("Prefabs/Player/HEROPLAYER (custom) (1.6.3c)");

                    newPlayer = Instantiate(compPlayer);
                    newPlayer.name = "HEROPLAYER (custom)(1.6.3c)(New)";

                    newPlayer.transform.parent = oldPlayer.transform;
                    newPlayer.transform.localPosition = new Vector3(0, 0, 0);
                    newPlayer.transform.localEulerAngles = new Vector3(0, 0, 0);
                    newPlayer.transform.parent = null;

                    DestroyImmediate(oldPlayer);

                    Debug.Log("Player Updated for HFPS 1.6.3c");

                    if(gameMan.Length > 0){

                        gameMan[0].m_PlayerObj = newPlayer;

                        Debug.Log("Game Manager Updated for HFPS 1.6.3c");

                    }//gameMan.Length > 0

                    if(tempSaveEditors.Length > 0){

                        Debug.Log("Saveables Save Start");

                        tempSaveEditors[0].FindSaveables_Start();

                    }//tempSaveEditors.Length > 0

                //gameUIselected
                } else {

                    Debug.Log("Game UI object NOT SELECTED | Save Game Handler NOT SELECTED");
                    Debug.Log("SELECT _GAMEUI | SELECT SAVE GAME HANDLER SCRIPT OBJECT");

                }//gameUIselected

            }//SceneUpdate_163c

        #endif


    }//Components_Menu
    
    
}//namespace
