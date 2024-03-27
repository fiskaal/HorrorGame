using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DizzyMedia.Shared;

using Newtonsoft.Json.Linq;
using ThunderWire.Utility;
using HFPS.Systems;
using HFPS.UI;

namespace DizzyMedia.HFPS_Components {

    [AddComponentMenu("Dizzy Media/Components for HFPS/World/Items/Start Items")]
    public class HFPS_StartItems : MonoBehaviour, ISaveable {


    //////////////////////////////////////
    ///
    ///     CLASSES
    ///
    ///////////////////////////////////////


        [Serializable]
        public class Weapons {

            public string name;

            [Space]

            [InventorySelector]
            public int itemID;

            public ItemDataPair[] customData;

            [Space]

            public bool addItem;
            public bool addShortcut;

            [Space]

            public bool startWithItem;
            public ItemStart itemStart;

            public int itemSlot;

        }//Weapons

        [Serializable]
        public class Items {

            public string name;

            [Space]

            [InventorySelector]
            public int itemID;

            public ItemDataPair[] customData;

            [Space]

            public bool addItem;
            public bool addShortcut;

            [Space]

            public bool randomAmount;

            [MinMax(0, 100)]
            public Vector2Int randomRange;

        }//Items

        [Serializable]
        public class Items_Remove {

            public string name;

            [Space]

            [InventorySelector]
            public int itemID;

        }//Items_Remove


    //////////////////////////////////////
    ///
    ///     ENUMS
    ///
    ///////////////////////////////////////


        public enum ItemStart {

            NoAnimation = 0,
            Animation = 1,

        }//ItemStart


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

    ////////////////////
    ///
    ///     START
    ///
    ////////////////////


        public bool addDelay;
        public float delayWait;


    ////////////////////
    ///
    ///     ITEMS
    ///
    ////////////////////


        public bool clearInventory;
        public List<Weapons> weapons;
        public List<Items> items;
        public List<Items_Remove> itemsRemove;


    ///////////////////////////
    ///
    ///     AUTO
    ///
    ///////////////////////////


        private Inventory inventory;
        public bool present;
        public bool locked;

        public int tabs;

        public bool startOpts;
        public bool itemOpts;
        public bool addItemsOpts;
        public bool removeItemsOpts;


    //////////////////////////////////////
    ///
    ///     START ACTIONS
    ///
    ///////////////////////////////////////


        void Awake(){

            inventory = Inventory.Instance;

        }//Awake

        void Start() {

            StartInit();

        }//Start

        public void StartInit(){

            StartCoroutine("StartBuff");

        }//StartInit

        private IEnumerator StartBuff(){

            yield return new WaitForSeconds(0.2f);

            present = false;

            if(!locked){

                if(addDelay){

                    StartCoroutine("ItemsInitBuff");

                //addDelay
                } else {

                    Items_Init();

                }//addDelay

            }//!locked

        }//StartBuff


    //////////////////////////////////////
    ///
    ///     ITEM ACTIONS
    ///
    ///////////////////////////////////////


        public IEnumerator ItemsInitBuff(){

            yield return new WaitForSeconds(delayWait);

            if(clearInventory){

                Items_Clear();

            }//clearInventory

            Items_Init();

        }//ItemsInitBuff    

        public void Items_Init(){

            int tempAmount = 0;

            if(!clearInventory){

                if(itemsRemove.Count > 0){

                    for(int ir = 0; ir < itemsRemove.Count; ++ir ) {

                        present = false;

                        if(inventory.CheckItemInventory(itemsRemove[ir].itemID)) {

                            present = true;

                        }//CheckItemInventory

                        if(present){

                            inventory.RemoveItem(itemsRemove[ir].itemID, true);

                        }//present

                    }//for ir itemsRemove

                }//itemsRemove.Count > 0    

            }//!clearInventory

            if(weapons.Count > 0){

                for(int w = 0; w < weapons.Count; ++w ) {

                    present = false;

                    if(weapons[w].addItem){

                        if(inventory.CheckItemInventory(weapons[w].itemID)) {

                            present = true;

                        }//CheckItemInventory

                        if(!present){

                            ItemData itemData = new ItemData();

                            if(weapons[w].customData.Length > 0){

                                foreach(var data in weapons[w].customData) {

                                    itemData.data.Add(data.Key, data.Value);

                                }//foreach data

                            }//customData.length > 0

                            inventory.AddItem(weapons[w].itemID, 1, itemData, weapons[w].addShortcut);

                        }//!present

                    }//addItem

                    if(weapons[w].startWithItem){
                    
                        #if COMPONENTS_PRESENT

                            if(weapons[w].itemStart == ItemStart.NoAnimation){

                                HFPS_References.instance.itemSwitcher.ActivateItem(weapons[w].itemSlot);

                            }//itemStart = no animation

                            if(weapons[w].itemStart == ItemStart.Animation){

                                HFPS_References.instance.itemSwitcher.SelectSwitcherItem(weapons[w].itemSlot);

                            }//itemStart = no animation
                        
                        #endif

                    }//startWithItem

                }//for w weapons

            }//weapons.Count > 0

            if(items.Count > 0){

                for(int i = 0; i < items.Count; ++i ) {

                    present = false;

                    if(items[i].addItem){

                        if(inventory.CheckItemInventory(items[i].itemID)) {

                            present = true;

                        }//CheckItemInventory

                        if(!present){

                            ItemData itemData = new ItemData();

                            if(items[i].customData.Length > 0){

                                foreach(var data in items[i].customData) {

                                    itemData.data.Add(data.Key, data.Value);

                                }//foreach data

                            }//customData.length > 0

                            if(items[i].randomAmount){

                                tempAmount = UnityEngine.Random.Range(items[i].randomRange.x, items[i].randomRange.y);

                            //randomAmount
                            } else {

                                tempAmount = items[i].randomRange.y;

                            }//randomAmount

                            if(tempAmount > 0){

                                inventory.AddItem(items[i].itemID, tempAmount, itemData, items[i].addShortcut);

                            }//tempAmount > 0

                        }//!present

                    }//addItem

                }//for i items

            }//items.Count > 0

            locked = true;

        }//Items_Init

        public void Items_Clear() {

            inventory.RemoveAllItems();

        }//Items_Clear


    //////////////////////////////////////
    ///
    ///     SAVE/LOAD ACTIONS
    ///
    ///////////////////////////////////////

    ////////////////////////////
    ///
    ///     SAVE ACTIONS
    ///
    ////////////////////////////


        public Dictionary<string, object> OnSave() {

            return new Dictionary<string, object> {

                {"locked", locked }

            };//Dictionary

        }//OnSave


    ////////////////////////////
    ///
    ///     LOAD ACTIONS
    ///
    ////////////////////////////


        public void OnLoad(JToken token) {

            locked = (bool)token["locked"];

        }//OnLoad


    }//HFPS_StartItems


}//namespace