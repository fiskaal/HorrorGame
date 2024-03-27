using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using DizzyMedia.Shared;

using ThunderWire.Utility;
using HFPS.Systems;
using HFPS.UI;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/Systems/Ignitable/Ignitable")]
    public class HFPS_Ignitable : MonoBehaviour, IItemSelect, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Item_Save {

            public string name;
            public int slot;
            public int ID;
            public int amount;
            public int switcherID;
            public string shortcut;
            public List<ItemDataPair> customData;

        }//Item_Save 


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum Ignite_Type {

            Trigger = 0,
            Manual = 1,

        }//Ignite_Type

        public enum Interact_Type {

            OpenInventory = 0,
            AutoDetect = 1,

        }//Interact_Type

        public enum Item_Type {

            Regular = 0,
            Switcher = 1,

        }//Item_Type

        public enum ItemUse_Type {

            Keep = 0,
            Remove = 1,

        }//ItemUse_Type


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

    ///////////////////
    ///
    ///     GENERAL OPTIONS
    ///
    ///////////////////


        public Ignite_Type igniteType;
        public float igniteTime;
        public Collider trigger;


    ///////////////////
    ///
    ///     IGNITE OPTIONS
    ///
    ///////////////////


        public List<GameObject> showOnLit;


    ///////////////////
    ///
    ///     ITEM OPTIONS
    ///
    ///////////////////


        public bool requireItem;

        [InventorySelector]
        public int itemID;


    ///////////////////
    ///
    ///     INTERACTION OPTIONS
    ///
    ///////////////////


        public Interact_Type interactType;
        public Item_Type itemType;
        public ItemUse_Type itemUseType;
        public bool detectItemShowing;
        public int switcherSlot = -1;

        public string selectText;
        public string emptyText;
        public string wrongItemText;


    ///////////////////
    ///
    ///     SOUND OPTIONS
    ///
    ///////////////////


        public bool useLitSound;
        public AudioSource audSource;
        public AudioClip audClip;


    ///////////////////////////
    ///
    ///     EVENTS
    ///
    ///////////////////////////


        public UnityEvent onCorrectItem;
        public UnityEvent onIncorrectItem;

        public UnityEvent onLit;
        public UnityEvent onLitLoad;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        public string tempSelectText;
        public string tempEmptyText;
        public string tempWrongText;

        public bool present;
        public bool isLit;
        public bool locked;

        private Item_Save itemSave;
        private Item[] items;
        private InventoryItemData invItemData;

        public int tabs;

        public bool genOpts;
        public bool igniteOpts;
        public bool itemOpts;
        public bool interactOpts;
        public bool soundOpts;

        private HFPS_References compRefs;
        private Inventory inventory;
        private HFPS_GameManager gameManager;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            compRefs = HFPS_References.instance;
            inventory = Inventory.Instance;
            gameManager = HFPS_GameManager.Instance;

        }//Awake

        void Start() {

            StartInit();

        }//start

        public void StartInit(){

            if(igniteType == Ignite_Type.Manual){

                items = new Item[1];
                items[0] = inventory.GetItem(itemID);

                if(items[0] != null){

                    if(interactType == Interact_Type.OpenInventory){

                        if(tempSelectText != selectText + " " + items[0].Title){

                            tempSelectText = selectText + " " + items[0].Title;

                        }//tempSelectText

                    }//interactType = open inventory

                    if(tempEmptyText != emptyText + " " + items[0].Title){

                        tempEmptyText = emptyText + " " + items[0].Title;

                    }//tempEmptyText

                    if(tempWrongText != items[0].Title + " " + wrongItemText){

                        tempWrongText = items[0].Title + " " + wrongItemText;

                    }//tempEmptyText

                }//items[0] != null

            }//igniteType = manual

        }//StartInit


    //////////////////////////////////////
    ///
    ///     USE ACTIONS
    ///
    ///////////////////////////////////////

    ///////////////////////////
    ///
    ///     INIT
    ///
    ///////////////////////////


        public void Interaction_Init(){

            if(!locked) {

                if(igniteType == Ignite_Type.Manual){

                    if(requireItem){

                        ItemData_Catch();

                        if(interactType == Interact_Type.OpenInventory){

                            if(itemType == Item_Type.Regular){

                                inventory.OnInventorySelect(new int[1] { itemID }, new string[0], this, tempSelectText, tempEmptyText);

                            }//itemType = regular

                            if(itemType == Item_Type.Switcher){

                                if(!detectItemShowing){

                                    inventory.OnInventorySelect(new int[1] { itemID }, new string[0], this, tempSelectText, tempEmptyText);

                                //!detectItemShowing
                                } else {

                                    ItemCheck(itemID);

                                }//!detectItemShowing

                            }//itemType = switcher

                        }//interactType = OpenInventory

                        if(interactType == Interact_Type.AutoDetect){

                            ItemCheck(itemID);

                        }//AutoDetect

                    //requireItem
                    } else {

                        Ignite();

                    }//requireItem

                }//igniteType = manual

            }//!locked

        }//Interaction_Init


    ///////////////////////////
    ///
    ///     ITEM ACTIONS
    ///
    ///////////////////////////


        private void ItemData_Catch(){

            itemSave = new Item_Save();

            ItemData itemData = new ItemData();
            invItemData = new InventoryItemData();

            invItemData = inventory.ItemDataOfItem(itemID);

            if(invItemData != null){

                itemData = invItemData.data;

                itemSave.customData = new List<ItemDataPair>();

                if(itemData.data.Count > 0){

                    foreach(var data in itemData.data) {

                        ItemDataPair tempPair = new ItemDataPair(data.Key, data.Value.ToString());
                        itemSave.customData.Add(tempPair);

                    }//foreach data

                }//itemData.Count > 0

                itemSave.name = invItemData.itemTitle;
                itemSave.slot = invItemData.slotID;
                itemSave.ID = invItemData.itemID;
                itemSave.amount = invItemData.itemAmount;
                itemSave.switcherID = invItemData.item.Settings.useSwitcherID;

                if(!string.IsNullOrEmpty(invItemData.shortcut)){

                    itemSave.shortcut = invItemData.shortcut;

                //shortcut != null
                } else {

                    itemSave.shortcut = "";

                }//shortcut != null

            }//invItemData != null

        }//ItemData_Catch

        public void OnItemSelect(int ID, ItemData data) {

            ItemCheck(ID);

        }//OnItemSelect

        private void ItemCheck(int ID){

            bool canIgnite = false;
            present = false;

            if(interactType == Interact_Type.OpenInventory){

                if(itemType == Item_Type.Regular){

                    if(ID == itemID){

                        present = true;

                    }//ID = itemID

                }//itemType = regular

                if(itemType == Item_Type.Switcher){

                    if(!detectItemShowing){

                        if(ID == itemID){

                            present = true;

                        }//ID = itemID

                    //!detectItemShowing
                    } else {

                        if(inventory.CheckItemInventory(ID)) {

                            present = true;

                        }//CheckItemInventory

                    }//!detectItemShowing

                }//itemType = switcher

            }//interactType = open inventory

            if(interactType == Interact_Type.AutoDetect){

                if(inventory.CheckItemInventory(ID)) {

                    present = true;

                }//CheckItemInventory

            }//interactType = auto detect

            if(present){

                canIgnite = true;

                if(itemType == Item_Type.Regular){

                    onCorrectItem.Invoke();

                    if(itemUseType == ItemUse_Type.Keep){

                        if(interactType == Interact_Type.OpenInventory){

                            StartCoroutine("Item_AddDelayed");

                        }//interactType = open inventory

                    }//itemUseType = keep

                    if(itemUseType == ItemUse_Type.Remove){

                        if(interactType == Interact_Type.OpenInventory){

                            inventory.RemoveSelectedItem(true);

                        }//interactType = open inventory

                        if(interactType == Interact_Type.AutoDetect){

                            inventory.RemoveItemAmount(ID, itemSave.amount);

                        }//interactType = auto detect

                    }//itemUseType = remove

                }//itemType = regular

                if(itemType == Item_Type.Switcher){

                    if(!detectItemShowing){

                        onCorrectItem.Invoke();

                        if(itemUseType == ItemUse_Type.Keep){

                            if(interactType == Interact_Type.OpenInventory){

                                StartCoroutine("Item_AddDelayed");

                            }//interactType = open inventory

                        }//itemUseType = keep

                        if(itemUseType == ItemUse_Type.Remove){

                            if(interactType == Interact_Type.OpenInventory){

                                inventory.RemoveSelectedItem(true);

                            }//interactType = open inventory

                            if(interactType == Interact_Type.AutoDetect){

                                inventory.RemoveItemAmount(ID, itemSave.amount);

                            }//interactType = auto detect

                        }//itemUseType = remove

                    //!detectItemShowing
                    } else {

                        if(compRefs.itemSwitcher.currentItem > - 1){

                            if(compRefs.itemSwitcher.currentItem == switcherSlot){

                                canIgnite = true;

                                onCorrectItem.Invoke();

                            //currentItem = switcherSlot
                            } else {

                                canIgnite = false;

                                onIncorrectItem.Invoke();

                                gameManager.ShowQuickMessage(tempWrongText, "NoItems");

                            }//currentItem = switcherSlot

                        //currentItem > -1
                        } else {

                            canIgnite = false;

                            onIncorrectItem.Invoke();

                            gameManager.ShowQuickMessage(tempWrongText, "NoItems");

                        }//currentItem > -1

                    }//!detectItemShowing

                }//itemType = switcher

                if(canIgnite){

                    Ignite();

                }//canIgnite

            //present
            } else {

                onIncorrectItem.Invoke();

                if(tempEmptyText != "") {

                    gameManager.ShowQuickMessage(tempEmptyText, "NoItems");

                }//tempEmptyText != null

            }//present

        }//ItemCheck

        public IEnumerator Item_AddDelayed(){

            #if COMPONENTS_PRESENT

                if(itemSave != null){

                    yield return new WaitForSeconds(0.1f);

                    inventory.AddItemToSlotCustom(itemSave.slot, itemSave.ID, 1, invItemData, false);

                    yield return new WaitForSeconds(0.1f);

                    if(inventory.CheckItemInventory(itemSave.ID)) {

                        if(!string.IsNullOrEmpty(itemSave.shortcut)){

                            yield return new WaitForSeconds(0.2f);

                            inventory.ShortcutBindCustom(itemSave.ID, itemSave.slot, itemSave.shortcut);

                        }//shortcut != null

                    }//has item

                }//itemSave != null

            #else 

                yield return new WaitForSeconds(0.1f);

            #endif

        }//Item_AddDelayed


    //////////////////////////////////////
    ///
    ///     IGNITE ACTIONS
    ///
    ///////////////////////////////////////


        public void Ignite(){

            if(trigger != null){

                trigger.enabled = false;

            }//trigger != null

            isLit = true;
            locked = true;

            if(useLitSound){

                if(audSource != null && audClip != null){

                    audSource.PlayOneShot(audClip, audSource.volume);

                }//audSource & audClip != null

            }//useLitSound

            if(showOnLit.Count > 0){

                for(int a = 0; a < showOnLit.Count; a++) {

                    showOnLit[a].SetActive(true);

                }//for a showOnLit

            }//showOnLit.Count > 0

            onLit.Invoke();

        }//Ignite

        private void Ignite_Load(){

            if(trigger != null){

                trigger.enabled = false;

            }//trigger != null

            isLit = true;
            locked = true;

            if(showOnLit.Count > 0){

                for(int a = 0; a < showOnLit.Count; a++) {

                    showOnLit[a].SetActive(true);

                }//for a showOnLit

            }//showOnLit.Count > 0

            onLitLoad.Invoke();

        }//Ignite_Load


    //////////////////////////////////////
    ///
    ///     CATCH ACTIONS
    ///
    ///////////////////////////////////////


        public HFPS_Ignitable Ignitable_Catch(){

            if(!isLit){

                return this;

            //!isLit
            } else {

                return null;

            }//!isLit

        }//Ignitable_Catch


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"isLit", isLit },
                {"locked", locked }

            };//Dictionary

        }//OnSave

        public void OnLoad(JToken token) {

            isLit = (bool)token["isLit"];
            locked = (bool)token["locked"];

            if(isLit){

                Ignite_Load();

            }//isLit

        }//OnLoad


    }//HFPS_Ignitable


}//namespace