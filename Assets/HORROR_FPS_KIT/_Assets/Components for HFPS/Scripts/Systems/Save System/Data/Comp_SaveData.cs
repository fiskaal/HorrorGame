using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Comp_SaveData {

    public string compVersion = "";

    public Comp_InventoryData inventoryData = new Comp_InventoryData();
    public Comp_LevelData levelData = new Comp_LevelData();
    public Comp_ObjectivesData objectivesData = new Comp_ObjectivesData();

}//Comp_SaveData
