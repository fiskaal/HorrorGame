using System;
using UnityEngine;
using System.Collections.Generic;

using HFPS.Systems;
using HFPS.UI;

[Serializable]
public class Comp_InventoryData {

    [Serializable]
    public struct Item {
        
        public string name;
        public int slot;
        public int ID;
        public int amount;
        public int switcherID;
        public string shortcut;
        public List<ItemDataPair> customData;
        
    }//Item 

    public List<Item> items = new List<Item>();
    public List<Item> oldItems = new List<Item>();

}//Comp_InventoryData
