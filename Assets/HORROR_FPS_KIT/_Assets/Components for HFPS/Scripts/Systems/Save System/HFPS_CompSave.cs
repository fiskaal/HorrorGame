using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using DizzyMedia.Version;

using HFPS.Systems;
using ThunderWire.Helpers;
using ThunderWire.Utility;
using HFPS.UI;

#if UNITY_EDITOR

    using UnityEditor;

#endif

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Save System/Components Save")]
    public class HFPS_CompSave : MonoBehaviour {


    //////////////////////////
    //
    //      INSTANCE
    //
    //////////////////////////


        public static HFPS_CompSave instance = null;


    //////////////////////////
    //
    //      CLASSES
    //
    //////////////////////////


        [Serializable]
        public class Temp_Identity {

            public int identifier;
            public bool isCompleted;

        }//Temp_Identity

        [Serializable]
        public class Events {

            [Space]

            public UnityEvent onStart;
            public UnityEvent onSave;
            public UnityEvent onLoad;

        }//Events


    //////////////////////////
    //
    //      ENUMS
    //
    //////////////////////////


        public enum Items_SaveType {

            None = 0,
            Global = 1,

        }//Items_SaveType

        public enum Objectives_SaveType {

            None = 0,
            Global = 1,
            PerScene = 2,

        }//Objectives_SaveType


    /////////////////
    //
    //   DEBUG
    //
    /////////////////


        public bool useDebug;


    /////////////////
    //
    //   START OPTIONS
    //
    /////////////////


        public bool dontDestroy;
        public bool createInstance;


    /////////////////
    //
    //   SAVE OPTIONS
    //
    /////////////////


        public float initWait;

        public bool useStartLoad;
        public bool secureSave;
        public bool debugJSON;


    /////////////////
    //
    //   ITEMS OPTIONS
    //
    /////////////////


        public Items_SaveType itemsSaveType;


    /////////////////
    //
    //   OBJECTIVES OPTIONS
    //
    /////////////////


        public Objectives_SaveType objSaveType;


    /////////////////
    //
    //   FILE OPTIONS
    //
    /////////////////


        public string compDataName;

        public bool resetOnPlayStop;
        public bool clearSavesOnPlayStop;


    /////////////////
    //
    //   EVENTS
    //
    /////////////////


        public Events events;


    /////////////////
    //
    //   AUTO
    //
    /////////////////


        public Comp_SaveData saveData = new Comp_SaveData();
        public DM_Version compVersion;
        private string versionName = "Components Version";
        public bool loading;
        public bool locked;

        private Inventory inventory;
        private ObjectiveManager objectiveManager;

        public int tabs;
        public int fileTabs;
        public int debugInt;
        public bool startOpts;
        public bool saveOpts;
        public bool itemsOpts;
        public bool objOpts;
        public bool fileOpts;


    //////////////////////////
    //
    //      START ACTIONS
    //
    //////////////////////////


        void Awake(){

            locked = false;

            if(createInstance){

                if(instance == null){

                    instance = this;

                //instance == null
                } else {

                    locked = true;
                    this.gameObject.SetActive(false);

                }//instance == null

            }//createInstance

        }//Awake

        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            StartCoroutine("InitBuff");

        }//StartInit

        private IEnumerator InitBuff(){

            yield return new WaitForSeconds(initWait);

            events.onStart.Invoke();

            if(dontDestroy){

                if(this.transform.parent != null){

                    this.transform.parent = null;

                }//parent != null

                DontDestroyOnLoad(this.gameObject);

            }//dontDestroy

            if(useStartLoad){

                Loading_State(true);
                Data_Load();

            }//useStartLoad

        }//InitBuff


    //////////////////////////
    //
    //      DATA LOAD & SAVE ACTIONS
    //
    //////////////////////////


        public void Data_Load(){

            events.onLoad.Invoke();


    /////////////////
    //
    //     COMP DATA
    //
    ///////////////// 


            string tempCompName = "/" + compDataName + ".json";
            string tempPath = Application.persistentDataPath + tempCompName;

            if(File.Exists(tempPath)) {

                if(secureSave){

                    byte[] catchByte = File.ReadAllBytes(Application.persistentDataPath + tempCompName);

                    string tempCatch = StringObfuscator.Parse(catchByte);

                    if(debugJSON){

                        File.WriteAllText("Comp_Temp.txt", tempCatch);

                        saveData = JsonUtility.FromJson<Comp_SaveData>(File.ReadAllText("Comp_Temp.txt"));

                    //debugJSON
                    } else {

                        saveData = JsonUtility.FromJson<Comp_SaveData>(tempCatch);

                    }//debugJSON

                //secureSave
                } else {

                    saveData = JsonUtility.FromJson<Comp_SaveData>(File.ReadAllText(Application.persistentDataPath + tempCompName));

                }//secureSave

                if(useDebug){

                    Debug.Log("Components Data Found");

                }//useDebug

            //File.Exists
            } else {

                saveData = new Comp_SaveData();

                saveData.compVersion = compVersion.version;

                saveData.levelData.levels = new Comp_LevelData.Level[0];

                saveData.objectivesData.activeObjectives.Clear();
                saveData.objectivesData.activeObjectives = new List<ObjectiveModel>();

                string tempJSON = JsonUtility.ToJson(saveData, true);

                if(secureSave){

                    byte[] tempSecSave = StringObfuscator.Convert(tempJSON);

                    File.WriteAllBytes(Application.persistentDataPath + tempCompName, tempSecSave);

                //secureSave
                } else {

                    File.WriteAllText(Application.persistentDataPath + tempCompName, tempJSON);

                }//secureSave

                if(useDebug){

                    Debug.Log("Components Data Created");

                }//useDebug

            }//File.Exists

        }//Data_Load

        public void Data_Save(){

            events.onSave.Invoke();


    /////////////////
    //
    //     INVENTORY
    //
    /////////////////


            if(itemsSaveType == Items_SaveType.Global){

                Items_Catch();

            }//itemsSaveType = global


    /////////////////
    //
    //     OBJECTIVES
    //
    /////////////////


            if(objSaveType == Objectives_SaveType.Global){

                Objectives_Catch();

            }//objSaveType = global

            if(objSaveType == Objectives_SaveType.PerScene){

                ObjectivesScene_Catch();

            }//objSaveType = per scene


    /////////////////
    //
    //     COMP DATA
    //
    /////////////////


            string tempCompName = "/" + compDataName + ".json";

            string tempPath = Application.persistentDataPath + tempCompName;

            if(File.Exists(tempPath)) {

                string tempJSON = JsonUtility.ToJson(saveData, true);

                if(secureSave){

                    byte[] tempSecSave = StringObfuscator.Convert(tempJSON);

                    File.WriteAllBytes(Application.persistentDataPath + tempCompName, tempSecSave);

                //secureSave
                } else {

                    File.WriteAllText(Application.persistentDataPath + tempCompName, tempJSON);

                }//secureSave

                if(useDebug){

                    Debug.Log("Components Data Saved");

                }//useDebug

            //File.Exists
            } else {

                if(useDebug){

                    Debug.Log("Components Data Not Found");

                }//useDebug

            }//File.Exists

        }//Data_Save


    //////////////////////////
    //
    //      DATA SET ACTIONS
    //
    //////////////////////////

    ///////////////
    //
    //      INVENTORY
    //
    ///////////////


        public void Items_Catch(){

            #if COMPONENTS_PRESENT

                inventory = Inventory.Instance;

                if(saveData.inventoryData.items.Count > 0){

                    saveData.inventoryData.oldItems = new List<Comp_InventoryData.Item>();

                    for(int i = 0; i < saveData.inventoryData.items.Count; ++i ) {

                        saveData.inventoryData.oldItems.Add(saveData.inventoryData.items[i]);

                    }//for i items

                }//items.Count > 0

                saveData.inventoryData.items = new List<Comp_InventoryData.Item>();

                if(inventory != null){

                    if(inventory.AnyItemInventroy()){

                        foreach(var slot in inventory.Slots) {

                            if(slot.GetComponent<InventorySlot>().itemData != null) {

                                ItemData itemData = new ItemData();
                                InventoryItemData invItemData = slot.GetComponent<InventorySlot>().itemData;
                                itemData = invItemData.data;

                                Comp_InventoryData.Item tempItem = new Comp_InventoryData.Item();
                                tempItem.customData = new List<ItemDataPair>();                        

                                if(itemData.data.Count > 0){

                                    foreach(var data in itemData.data) {

                                        ItemDataPair tempPair = new ItemDataPair(data.Key, data.Value.ToString());
                                        tempItem.customData.Add(tempPair);

                                    }//foreach data

                                }//itemData.Count > 0

                                tempItem.name = invItemData.itemTitle;
                                tempItem.slot = invItemData.slotID;
                                tempItem.ID = invItemData.itemID;
                                tempItem.amount = invItemData.itemAmount;
                                tempItem.switcherID = invItemData.item.Settings.useSwitcherID;

                                if(!string.IsNullOrEmpty(invItemData.shortcut)){

                                    if(useDebug){

                                        Debug.Log("TempItem " + tempItem.name + " " + "Shortcut " + tempItem.shortcut);

                                    }//useDebug

                                    tempItem.shortcut = invItemData.shortcut;

                                //shortcut != null
                                } else {

                                    tempItem.shortcut = "";

                                }//shortcut != null

                                saveData.inventoryData.items.Add(tempItem);

                            }//itemData != null

                        }//foreach slot

                    }//has items

                    if(saveData.inventoryData.items.Count > 0 && saveData.inventoryData.oldItems.Count > 0){

                        for(int it = 0; it < saveData.inventoryData.items.Count; ++it ) {

                            for(int ot = 0; ot < saveData.inventoryData.oldItems.Count; ++ot ) {

                                if(saveData.inventoryData.items[it].name == saveData.inventoryData.oldItems[ot].name){

                                    if(saveData.inventoryData.oldItems[ot].slot != saveData.inventoryData.items[it].slot){

                                        //Debug.Log("OT " + ot + " != " + "IT " + it);

                                        Comp_InventoryData.Item tempItem = saveData.inventoryData.oldItems[ot];

                                        saveData.inventoryData.oldItems.RemoveAt(ot);
                                        saveData.inventoryData.oldItems.Insert(it, tempItem);

                                    }//slot != slot

                                }//name = name

                            }//for ot oldItems

                        }//for it items

                    }//items.Count > 0 & oldItems.Count > 0

                    inventory = null;

                }//inventory != null

            #endif

        }//Items_Catch


    ///////////////
    //
    //      LEVELS
    //
    ///////////////


        public void Level_Add(string scene, string save){

            if(useDebug){

                Debug.Log("Adding Level: " + scene + " Save: " + save);

            }//useDebug

            bool adding = false;
            bool done = false;
            int tempCount = 0;

            List<Comp_LevelData.Level> tempList = new List<Comp_LevelData.Level>();

            if(saveData.levelData.levels.Length > 0){

                for(int i = 0; i < saveData.levelData.levels.Length; ++i ) {

                    if(!done){

                        if(saveData.levelData.levels[i].name == scene){

                            if(saveData.levelData.levels[i].save != save){

                                saveData.levelData.levels[i].save = save;

                            }//save != save

                            done = true;

                        //name != scene
                        } else {

                            if(!adding){

                                tempCount += 1;

                                Comp_LevelData.Level tempLevel = new Comp_LevelData.Level();

                                tempLevel.name = saveData.levelData.levels[i].name;
                                tempLevel.save = saveData.levelData.levels[i].save;
                                tempLevel.firstVisit = saveData.levelData.levels[i].firstVisit;

                                if(objSaveType == Objectives_SaveType.PerScene){

                                    if(saveData.levelData.levels[i].activeObjectives.Count > 0){

                                        tempLevel.activeObjectives = new List<ObjectiveModel>();

                                        for(int ao = 0; ao < saveData.levelData.levels[i].activeObjectives.Count; ++ao ) {

                                            tempLevel.activeObjectives.Add(saveData.levelData.levels[i].activeObjectives[ao]);

                                            //if(useDebug){

                                                //Debug.Log("Temp Objectives Added = " + saveData.levelData.levels[i].activeObjectives[ao].objectiveText);

                                            //}//useDebug

                                        }//for ao activeObjectives

                                    }//activeObjectives.Count > 0

                                }//objSaveType = per scene

                                tempList.Add(tempLevel);

                                if(tempCount == saveData.levelData.levels.Length){

                                    adding = true;

                                    saveData.levelData.levels = new Comp_LevelData.Level[saveData.levelData.levels.Length + 1];

                                    Comp_LevelData.Level tempLevelNew = new Comp_LevelData.Level();

                                    tempLevelNew.name = scene;
                                    tempLevelNew.save = save;
                                    tempLevelNew.firstVisit = true;

                                    tempLevelNew.activeObjectives = new List<ObjectiveModel>();

                                    tempList.Add(tempLevelNew);

                                    for(int l = 0; l < tempList.Count; ++l ) {

                                        saveData.levelData.levels[l].name = tempList[l].name;
                                        saveData.levelData.levels[l].save = tempList[l].save;
                                        saveData.levelData.levels[l].firstVisit = tempList[l].firstVisit;
                                        saveData.levelData.levels[l].activeObjectives = tempList[l].activeObjectives;

                                    }//for l tempList

                                    done = true;

                                }//tempCount = levels.Length

                            }//!adding

                        }//name != scene

                    }//!done

                }//for i levels

            //levels.Length > 0
            } else {

                saveData.levelData.levels = new Comp_LevelData.Level[1];

                saveData.levelData.levels[0].name = scene;
                saveData.levelData.levels[0].save = save;
                saveData.levelData.levels[0].activeObjectives = new List<ObjectiveModel>();

                done = true;

            }//levels.Length > 0

            if(done){

                Data_Save();

            }//done

        }//Level_Add


    ///////////////
    //
    //      OBJECTIVES
    //
    ///////////////


        public void Objectives_Catch(){

            objectiveManager = ObjectiveManager.Instance;

            if(objectiveManager != null){

                if(objectiveManager.activeObjectives.Count > 0){

                    saveData.objectivesData.activeObjectives.Clear();
                    saveData.objectivesData.activeObjectives = new List<ObjectiveModel>();

                    for(int i = 0; i < objectiveManager.activeObjectives.Count; i++) {

                        ObjectiveModel objModel = objectiveManager.objectives.FirstOrDefault(o => o.identifier == objectiveManager.activeObjectives[i].identifier);

                        objModel.completion = objectiveManager.activeObjectives[i].completion;
                        objModel.isCompleted = objectiveManager.activeObjectives[i].isCompleted;

                        objModel.objective = null;
                        objModel.objEvent = null;

                        saveData.objectivesData.activeObjectives.Add(objModel);

                    }//for i objectiveManager.activeObjectives

                }//activeObjectives.Count > 0

                objectiveManager = null;

            }//objectiveManager != null

        }//Objectives_Catch

        public void ObjectivesScene_Catch(){

            int tempSlot = -1;

            objectiveManager = ObjectiveManager.Instance;

            if(objectiveManager != null){

                if(objectiveManager.activeObjectives.Count > 0){

                    tempSlot = Get_CurLevelSlot();

                }//activeObjectives.Count > 0

                if(tempSlot > -1){

                    if(!saveData.levelData.levels[tempSlot].firstVisit){

                        saveData.levelData.levels[tempSlot].activeObjectives = new List<ObjectiveModel>();

                        for(int i = 0; i < objectiveManager.activeObjectives.Count; i++) {

                            ObjectiveModel objModel = objectiveManager.objectives.FirstOrDefault(o => o.identifier == objectiveManager.activeObjectives[i].identifier);

                            objModel.completion = objectiveManager.activeObjectives[i].completion;
                            objModel.isCompleted = objectiveManager.activeObjectives[i].isCompleted;

                            objModel.objective = null;
                            objModel.objEvent = null;

                            saveData.levelData.levels[tempSlot].activeObjectives.Add(objModel);

                        }//for i objectiveManager.activeObjectives

                    //firstVisit
                    } //else {

                        //saveData.levelData.levels[tempSlot].firstVisit = false;

                    //}//firstVisit

                }//tempSlot > -1

            }//objectiveManager != null

        }//ObjectivesScene_Catch


    //////////////////////////
    //
    //      DATA UPDATE ACTIONS
    //
    //////////////////////////

    ///////////////
    //
    //      INVENTORY
    //
    ///////////////


        public void Items_Update(){

            StartCoroutine("ItemsUpdate_Buff");

        }//Items_Update

        private IEnumerator ItemsUpdate_Buff(){

            #if COMPONENTS_PRESENT

                bool slotsChangedInner = false;
                bool slotsChanged = false;

                inventory = Inventory.Instance;

                if(itemsSaveType == Items_SaveType.Global){

                    if(saveData.inventoryData.oldItems.Count > 0){

                        if(inventory.AnyItemInventroy()){

                            for(int i = 0; i < saveData.inventoryData.oldItems.Count; ++i ) {

                                if(!Item_Check(saveData.inventoryData.items, saveData.inventoryData.oldItems[i].ID)){

                                    if(inventory.CheckItemInventory(saveData.inventoryData.oldItems[i].ID)) {

                                        inventory.RemoveItem(saveData.inventoryData.oldItems[i].ID, true, false);

                                    }//item is present

                                }//old item not in new item list

                            }//for i oldItems

                        }//has items

                    }//oldItems.Count > 0

                    if(saveData.inventoryData.items.Count > 0){

                        if(inventory.AnyItemInventroy()){

                            for(int it = 0; it < saveData.inventoryData.items.Count; ++it ) {

                                InventoryItemData invItemData = new InventoryItemData();
                                invItemData.data = new ItemData();

                                if(!inventory.CheckItemInventory(saveData.inventoryData.items[it].ID)) {

                                    if(saveData.inventoryData.items[it].customData.Count > 0){

                                        foreach(var data in saveData.inventoryData.items[it].customData) {

                                            if(!invItemData.data.TryToGet(data.Key, out bool caught)){

                                                invItemData.data.data.Add(data.Key, data.Value);

                                            }//!try to get

                                        }//foreach data

                                    }//customData.Count > 0

                                    inventory.AddItemToSlotCustom(saveData.inventoryData.items[it].slot, saveData.inventoryData.items[it].ID, saveData.inventoryData.items[it].amount, invItemData, false);

                                //item is not present
                                } else {

                                    int tempInt = inventory.GetItemSlotID(saveData.inventoryData.items[it].ID);

                                    if(tempInt != saveData.inventoryData.items[it].slot){

                                        if(!string.IsNullOrEmpty(saveData.inventoryData.items[it].shortcut)){

                                            if(useDebug){

                                                Debug.Log("Slot " + it + " " + "Shortcut = " + saveData.inventoryData.items[it].shortcut);

                                            }//useDebug

                                            invItemData.shortcut = saveData.inventoryData.items[it].shortcut;

                                        }//shortcut != null

                                        if(saveData.inventoryData.items[it].customData.Count > 0){

                                            foreach(var data in saveData.inventoryData.items[it].customData) {

                                                if(!invItemData.data.TryToGet(data.Key, out bool caught)){

                                                    invItemData.data.data.Add(data.Key, data.Value);

                                                }//!try to get

                                            }//foreach data

                                        }//customData.Count > 0

                                        inventory.RemoveSlotItem(tempInt, true);                                

                                        yield return new WaitForSeconds(0.1f);

                                        inventory.AddItemToSlotCustom(saveData.inventoryData.items[it].slot, saveData.inventoryData.items[it].ID, saveData.inventoryData.items[it].amount, invItemData, false);

                                        slotsChangedInner = true;

                                    }//tempInt != slot

                                }//item is not present

                                if(it == saveData.inventoryData.items.Count - 1){

                                    if(slotsChangedInner){

                                        slotsChanged = true;

                                        if(useDebug){

                                            Debug.Log("Slots changed");

                                        }//useDebug

                                    }//slotsChangedInner

                                }//it = items.Count - 1

                            }//for it items

                            yield return new WaitForSeconds(0.1f);

                            if(slotsChanged){

                                if(inventory.Shortcuts.Count > 0) {

                                    inventory.Shortcuts = new List<Inventory.ShortcutModel>();

                                }//Shortcuts.Count > 0

                                 for(int it2 = 0; it2 < saveData.inventoryData.items.Count; ++it2 ) {

                                    if(inventory.CheckItemInventory(saveData.inventoryData.items[it2].ID)) {

                                        if(!string.IsNullOrEmpty(saveData.inventoryData.items[it2].shortcut)){

                                            yield return new WaitForSeconds(0.1f);

                                            if(useDebug){

                                                Debug.Log("Shortcut bind " + " | " + "Slot " + saveData.inventoryData.items[it2].slot + " " + "Shortcut " + saveData.inventoryData.items[it2].shortcut);

                                            }//useDebug

                                            inventory.ShortcutBindCustom(saveData.inventoryData.items[it2].ID, saveData.inventoryData.items[it2].slot, saveData.inventoryData.items[it2].shortcut);

                                        }//shortcut != null

                                    }//has item

                                }//for it2 items

                            }//slotsChanged

                        }//has items

                    }//items.Count > 0

                }//itemsSaveType = global

            #else 

                yield return new WaitForSeconds(0.1f);

            #endif

        }//ItemsUpdate_Buff


    ///////////////
    //
    //      OBJECTIVES
    //
    ///////////////


        public void ObjectivesUpdate_Delayed(float wait){

            StartCoroutine("ObjectivesUpdate_Buff", wait);

        }//ObjectivesUpdate_Delayed

        private IEnumerator ObjectivesUpdate_Buff(float wait){

            yield return new WaitForSeconds(wait);

            Objectives_Update();

        }//ObjectivesUpdate_Buff

        public void Objectives_Update(){

            if(!loading){

                if(useDebug){

                    Debug.Log("Objectives Update Start");

                }//useDebug

                bool clearSceneObjectives = false;
                //int tempCount = 0;

                List<Temp_Identity> tempIdents = new List<Temp_Identity>();

                objectiveManager = ObjectiveManager.Instance;

                if(objectiveManager.activeObjectives.Count > 0){

                    if(objSaveType == Objectives_SaveType.Global){

                        for(int oao = 0; oao < objectiveManager.activeObjectives.Count; oao++) {

                            Temp_Identity newIdent = new Temp_Identity();

                            newIdent.identifier = objectiveManager.activeObjectives[oao].identifier;
                            newIdent.isCompleted = objectiveManager.activeObjectives[oao].isCompleted;

                            tempIdents.Add(newIdent);

                        }//for oao activeObjectives

                        for(int sao = 0; sao < saveData.objectivesData.activeObjectives.Count; sao++) {

                            if(!Identity_Check(tempIdents, saveData.objectivesData.activeObjectives[sao].identifier)){

                                if(saveData.objectivesData.activeObjectives[sao].isCompleted){

                                    objectiveManager.activeObjectives.Add(saveData.objectivesData.activeObjectives[sao]);

                                //isCompleted
                                } else {

                                    objectiveManager.AddObjectiveModel(saveData.objectivesData.activeObjectives[sao]);

                                }//isCompleted

                            //identy not present
                            } else {

                                int tempSlot = Identity_Catch(tempIdents, saveData.objectivesData.activeObjectives[sao].identifier);

                                if(objectiveManager.activeObjectives[tempSlot].completion != saveData.objectivesData.activeObjectives[sao].completion){

                                    objectiveManager.activeObjectives[tempSlot].completion = saveData.objectivesData.activeObjectives[sao].completion;

                                }//completion != completion

                                if(!Complete_Check(saveData.objectivesData.activeObjectives[sao].isCompleted, objectiveManager.activeObjectives[tempSlot].isCompleted)){

                                    objectiveManager.activeObjectives[tempSlot].isCompleted = saveData.objectivesData.activeObjectives[sao].isCompleted;

                                    if(objectiveManager.activeObjectives[tempSlot].isCompleted){

                                        if(objectiveManager.activeObjectives[tempSlot].objective != null){

                                            Destroy(objectiveManager.activeObjectives[tempSlot].objective);

                                            objectiveManager.activeObjectives[tempSlot].objective = null;

                                        }//objective != null

                                        //saveData.objectivesData.activeObjectives.Remove(saveData.objectivesData.activeObjectives[sao]);

                                    }//isCompleted

                                }//complete does not match

                            }//identy not present

                        }//for sao activeObjectives

                    }//objSaveType = global

                    if(objSaveType == Objectives_SaveType.PerScene){

                        clearSceneObjectives = true;

                    }//objSaveType = per scene

                //activeObjectives.Count > 0
                } else {

                    if(objSaveType == Objectives_SaveType.Global){

                        for(int sao = 0; sao < saveData.objectivesData.activeObjectives.Count; sao++) {

                            if(saveData.objectivesData.activeObjectives[sao].isCompleted){

                                objectiveManager.activeObjectives.Add(saveData.objectivesData.activeObjectives[sao]);

                            //isCompleted
                            } else {

                                objectiveManager.AddObjectiveModel(saveData.objectivesData.activeObjectives[sao]);

                            }//isCompleted

                        }//for sao activeObjectives

                    }//objSaveType = global

                    if(objSaveType == Objectives_SaveType.PerScene){

                        clearSceneObjectives = true;

                    }//objSaveType = per scene

                }//activeObjectives.Count > 0

                if(clearSceneObjectives){

                    if(useDebug){

                        Debug.Log("Clearing scene objectives started");

                    }//useDebug

                    int tempSlot = Get_CurLevelSlot();

                    if(tempSlot > -1){

                        if(objectiveManager.activeObjectives.Count > 0){

                            for(int ao = 0; ao < objectiveManager.activeObjectives.Count; ao++) {

                                Destroy(objectiveManager.activeObjectives[ao].objective);

                            }//for ao activeObjectives

                            objectiveManager.activeObjectives.Clear();
                            objectiveManager.activeObjectives = new List<ObjectiveModel>();

                            if(useDebug){

                                Debug.Log("Scene objectives cleared");

                            }//useDebug

                        }//activeObjectives.Count > 0

                        if(saveData.levelData.levels[tempSlot].activeObjectives.Count > 0){

                            for(int slao = 0; slao < saveData.levelData.levels[tempSlot].activeObjectives.Count; slao++) {

                                if(saveData.levelData.levels[tempSlot].activeObjectives[slao].isCompleted){

                                    objectiveManager.activeObjectives.Add(saveData.levelData.levels[tempSlot].activeObjectives[slao]);

                                //isCompleted
                                } else {

                                    objectiveManager.AddObjectiveModel(saveData.levelData.levels[tempSlot].activeObjectives[slao]);

                                }//isCompleted

                            }//for slao

                            if(useDebug){

                                Debug.Log("New Objectives Added");

                            }//useDebug

                        }//activeObjectives.Count > 0

                    }//tempSlot > -1

                }//clearSceneObjectives

            //!loading
            } else {

                Loading_State(false);

                int tempSlot = Get_CurLevelSlot();

                if(objSaveType == Objectives_SaveType.Global){

                    Objectives_Update();

                }//objSaveType = global

                if(objSaveType == Objectives_SaveType.PerScene){

                    if(tempSlot > -1){

                        if(saveData.levelData.levels[tempSlot].firstVisit){

                            if(objectiveManager.activeObjectives.Count > 0){

                                for(int ao = 0; ao < objectiveManager.activeObjectives.Count; ao++) {

                                    Destroy(objectiveManager.activeObjectives[ao].objective);

                                }//for ao activeObjectives

                                objectiveManager.activeObjectives.Clear();
                                objectiveManager.activeObjectives = new List<ObjectiveModel>();

                                if(useDebug){

                                    Debug.Log("Scene objectives cleared");

                                }//useDebug

                            }//activeObjectives.Count > 0

                            saveData.levelData.levels[tempSlot].firstVisit = false;

                        }//firstVisit

                    }//tempSlot > -1

                }//objSaveType = per scene

                if(tempSlot > -1){

                    saveData.levelData.levels[tempSlot].firstVisit = false;

                //tempSlot > -1
                } else {

                    Level_Add(SceneManager.GetActiveScene().name, "");
                    Objectives_Update();

                }//tempSlot > -1

            }//!loading

        }//Objectives_Update    


    //////////////////////////
    //
    //      DATA CHECK ACTIONS
    //
    //////////////////////////

    ///////////////
    //
    //      INVENTORY
    //
    ///////////////


        public bool Item_Check(List<Comp_InventoryData.Item> itemList, int ID){

            for(int i = 0; i < itemList.Count; ++i ) {

                if(itemList[i].ID == ID){

                    return true;

                }//id = id

            }//for i itemList

            return false;

        }//Item_Check


    ///////////////
    //
    //      LEVELS
    //
    ///////////////


        public bool Level_Check(string scene){

            int tempCount = 0;
            bool present = false;

            if(saveData.levelData.levels.Length > 0){

                for(int i = 0; i < saveData.levelData.levels.Length; ++i ) {

                    tempCount += 1;

                    if(saveData.levelData.levels[i].name == scene){

                        present = true;

                    }//name = scene

                    if(tempCount == saveData.levelData.levels.Length){

                        return present;

                    }//tempCount = levels.Length

                }//for i levels

            }//levels.Length > 0

            return false;

        }//Level_Check

        public bool Save_Check(string save){

            bool present = false;

            if(saveData.levelData.levels.Length > 0){

                for(int i = 0; i < saveData.levelData.levels.Length; ++i ) {

                    if(!present){

                        if(saveData.levelData.levels[i].save == save){

                            present = true;

                            return present;

                        }//name = scene

                    }//!present

                }//for i levels

            }//levels.Length > 0

            return false;

        }//Save_Check


    ///////////////
    //
    //      OBJECTIVES
    //
    ///////////////


        public int Identity_Catch(List<Temp_Identity> identities, int curIdentity){

            for(int i = 0; i < identities.Count; i++) {

                if(identities[i].identifier == curIdentity){

                    return i;

                }//identifier != curIdentity

            }//for i identities

            return -1;

        }//Identity_Catch

        public bool Identity_Check(List<Temp_Identity> identities, int curIdentity){

            bool present = false;

            for(int i = 0; i < identities.Count; i++) {

                if(identities[i].identifier == curIdentity){

                    present = true;

                }//identifier != curIdentity

            }//for i identities

            return present;

        }//Identity_Check

        public bool Complete_Check(bool first, bool second){

            if(first != second){

                return false;

            //first != second
            } else {

                return true;

            }//first != second

        }//Complete_Check


    //////////////////////////
    //
    //      DATA GET ACTIONS
    //
    //////////////////////////


        public int Get_CurLevelSlot(){

            string tempScene = SceneManager.GetActiveScene().name;

            if(saveData.levelData.levels.Length > 0){

                for(int i = 0; i < saveData.levelData.levels.Length; ++i ) {

                    if(saveData.levelData.levels[i].name == tempScene){

                        if(useDebug){

                            Debug.Log("Found Level = " + saveData.levelData.levels[i].name);

                        }//useDebug

                        return i;

                    }//name = tempScene

                }//for i levels

            }//levels.Length > 0

            if(useDebug){

                Debug.Log("Did NOT Find Level");

            }//useDebug

            return -1;

        }//Get_CurLevelSlot

        public string Get_SaveName(string scene){

            if(saveData.levelData.levels.Length > 0){

                for(int i = 0; i < saveData.levelData.levels.Length; ++i ) {

                    if(saveData.levelData.levels[i].name == scene){

                        return saveData.levelData.levels[i].save;

                    }//name = scene

                }//for i levels

            }//levels.Length > 0

            return "";

        }//Get_SaveName


    //////////////////////////
    //
    //      RESET ACTIONS
    //
    //////////////////////////


        public void Reset_CompData(bool createNew){

            if(compDataName != ""){

                string tempCompName = "/" + compDataName + ".json";

                string tempPath = Application.persistentDataPath + tempCompName;

                if(File.Exists(tempPath)) {

                    File.Delete(tempPath);

                    if(useDebug){

                        Debug.Log("Components Data File Cleared");

                    }//useDebug

                    if(createNew){

                        #if UNITY_EDITOR

                            if(EditorApplication.isPlaying){

                                Data_Load();

                            }//isPlaying

                        #endif

                    }//createNew

                //File.Exists
                } else {

                    if(useDebug){

                        Debug.Log("No Components Data File");

                    }//useDebug

                }//File.Exists

            //compDataName != null
            } else {

                if(useDebug){

                    Debug.Log("Components Data Name = null");

                }//useDebug

            }//compDataName != null

        }//Reset_CompData

        #if UNITY_EDITOR

            public void Reset_Saves(){

                string folderPath = "";
                string finalFolderPath = "";

                AssetDatabase.Refresh();

                if(SerializationHelper.HasReference){

                    folderPath = SerializationHelper.Settings.GetSerializationPath();
                    finalFolderPath = Path.Combine(folderPath, "SavedGame");

                //HasReference
                } else {

                    SerializationHelper tempSerHelp = SerializationHelper.Instance;

                    folderPath = tempSerHelp.serializationSettings.GetSerializationPath();
                    finalFolderPath = Path.Combine(folderPath, "SavedGame");

                }//HasReference

                if(finalFolderPath != ""){

                    if(Directory.Exists(finalFolderPath)) {

                        DirectoryInfo dinfo = new DirectoryInfo(finalFolderPath);
                        FileInfo[] finfo = dinfo.GetFiles("*.sav");

                        if(finfo.Length > 0) {

                            foreach(var file in finfo) {

                                File.Delete(file.FullName);

                            }//foreach file

                            Debug.Log("HFPS Save files cleared");

                            AssetDatabase.Refresh();

                        //finfo.Length > 0
                        } else {

                            Debug.Log("No HFPS Save files");

                        }//finfo.Length > 0

                    //directory exists
                    } else {

                        Debug.Log("No HFPS Save directory");

                    }//directory exists

                }//finalFolderPath != null

            }//Reset_Saves

        #endif


    //////////////////////////
    //
    //      STATE ACTIONS
    //
    //////////////////////////


        public void Loading_State(bool state){

            loading = state;

        }//Loading_State


    //////////////////////////
    //
    //      DESTROY ACTIONS
    //
    //////////////////////////


        #if UNITY_EDITOR

            void OnDestroy(){

                if(!locked){

                    if(resetOnPlayStop){

                        Reset_CompData(false);

                    }//resetOnPlayStop

                    if(clearSavesOnPlayStop){

                        Reset_Saves();

                    }//clearSavesOnPlayStop

                }//!locked

            }//OnDestroy

        #endif


    //////////////////////////
    //
    //      EXTRA ACTIONS
    //
    //////////////////////////


        #if UNITY_EDITOR

            public void Version_Find(){

                string[] results;
                DM_Version tempVersion = ScriptableObject.CreateInstance<DM_Version>();

                results = AssetDatabase.FindAssets(versionName);

                if(results.Length > 0){

                    foreach(string guid in results){

                        if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                            tempVersion = AssetDatabase.LoadAssetAtPath<DM_Version>(AssetDatabase.GUIDToAssetPath(guid));

                            if(tempVersion != null){

                                compVersion = tempVersion;

                                //Debug.Log("Components Version found");

                            //tempVersion != null
                            } else {

                                //Debug.Log("Components Version NOT found");

                            }//tempVersion != null

                            //Exists
                        } else {

                            //Debug.Log("Components Version NOT found");

                        }//Exists

                    }//foreach guid

                //results.Length > 0
                } else {

                    Debug.Log("Components Version NOT found");

                }//results.Length > 0

            }//Version_Find

        #endif


    }//HFPS_CompSave


}//namespace