using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

using Newtonsoft.Json.Linq;
using ThunderWire.Utility;
using HFPS.Systems;
using HFPS.UI;

#if TW_LOCALIZATION_PRESENT

    using ThunderWire.Localization;

#endif

public class HFPS_SimplePuzzle : MonoBehaviour, IItemSelect, ISaveable {
    
    
//////////////////////////////////////
///
///     CLASSES
///
///////////////////////////////////////
    
    
    [Serializable]
    public class Item {
        
        [Space]
        
        public string name;
        
        [Space]
        
        [InventorySelector]
        public int itemID;
        
        public GameObject prefab;
        public Transform spawnPosition;
        public float spawnWait;
        
        [Space]
        
        public List<GameObject> holders;
        public List<Item_Event> itemEvents;
        
        [Space]
        
        public UnityEvent onActiveLoad;
        
        [Header("Auto")]
        
        public bool active;
        
    }//Item
    
    [Serializable]
    public class Item_Event {
        
        [Space]
        
        public string name;
        
        [Space]
        
        public bool delayEvent;
        public float eventWait;
        public UnityEvent newEvent;
        
        [Space]
        
        public float nextWait;
        
    }//Item_Event
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
    
    public GameObject interaction;
    public List<Item> items;
    
    [Space]
    
    public string emptyText;
    public string selectText;
    
    [Space]
    
    public bool useCompleteDelay;
    public float completeDelay;
    
    [Space]
    
    
///////////////////////////////
///
///     EVENTS
///
///////////////////////////////
    
    
    public UnityEvent onCorrectItem;
    public UnityEvent onPuzzleComplete;
    public UnityEvent onCompleteLoad;
    
    public bool useDelayedEvent;
    public float delayEventWait;
    public UnityEvent onPuzzleCompleteDelayed;
    
    
///////////////////////////////
///
///     AUTO
///
///////////////////////////////
    
    
    [Header("Auto")]
    
    public string tempEmptyText;
    public string tempSelectText;
    
    public int activeCount;
    public List<bool> activeList = new List<bool>();
    
    public int tempSlot;
    public bool present;
    public bool complete;
    
    public bool locked;
    
    private Inventory inventory;
    private HFPS_GameManager gameManager;
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////


    #if TW_LOCALIZATION_PRESENT

        void OnEnable() {

            if(HFPS_GameManager.LocalizationEnabled){

                LocalizationSystem.SubscribeAndGet(OnChangeLocalization, emptyText, selectText);

            }//LocalizationEnabled

        }//OnEnable
    
    #endif

    private void OnInitTexts() {
                
        tempEmptyText = TextsSource.GetText(emptyText, "Empty Text");
        tempSelectText = TextsSource.GetText(selectText, "Select Text");
            
    }//OnInitTexts
    
    void Awake(){
        
        inventory = Inventory.Instance;
        gameManager = HFPS_GameManager.Instance;
        
    }//Awake
    
    void Start() {
        
        StartInit();
        
    }//start

    public void StartInit(){
        
        for(int i = 0; i < items.Count; ++i ) {
         
            bool tempBool = false;
            activeList.Add(tempBool);
            
            for(int h = 0; h < items[i].holders.Count; ++h ) {
            
                items[i].holders[h].SetActive(false);
                
            }//for h holders
            
        }//for i items
        
        tempSlot = -1;
        activeCount = 0;
        present = false;
        
        Complete_State(false);
        Locked_State(false);

        if(TextsSource.HasReference) 
            TextsSource.Subscribe(OnInitTexts);
        else OnInitTexts();
        
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
            
            int[] tempIDs = new int[items.Count];

            for(int i = 0; i < items.Count; ++i ) {

                tempIDs[i] = items[i].itemID;

            }//for i items

            inventory.OnInventorySelect(tempIDs, new string[0], this, tempSelectText, tempEmptyText);
            
        }//!locked
        
    }//Interaction_Init
    
    
///////////////////////////
///
///     ITEM ACTIONS
///
///////////////////////////
    
    
    public void OnItemSelect(int ID, ItemData data) {
    
        ItemCheck(ID);
            
    }//OnItemSelect
    
    private void ItemCheck(int ID){
        
        present = false;
        
        for(int i = 0; i < items.Count; i++) {

            if(items[i].itemID == ID){

                present = true;

            }//itemID = ID

        }//for i items
        
        if(present){
            
            activeCount = 0;
            
            onCorrectItem.Invoke();
            
            Puzzle_Check(ID);
            CompleteCheck();

        //present
        } else {

            if(tempEmptyText != "") {

                gameManager.ShowQuickMessage(tempEmptyText, "No Items");

            }//tempEmptyText != null
            
        }//present
        
    }//ItemCheck
    
    
///////////////////////////
///
///     CHECK ACTIONS
///
///////////////////////////
    
    
    public void Puzzle_Check(int ID){
        
        for(int i = 0; i < items.Count; ++i ) {
         
            if(!items[i].active){
            
                if(items[i].itemID == ID){
                    
                    items[i].active = true;
                    activeList[i] = true;
                    
                    if(items[i].itemEvents.Count > 0){
                       
                        StartCoroutine("ItemEvents_Trigger", i);
                        
                    }//itemEvents.Count > 0
                
                    if(items[i].spawnWait > 0){
                        
                        StartCoroutine("Item_Spawn", i);

                    //spawnWait > 0
                    } else {
                     
                        GameObject newItem = Instantiate(items[i].prefab, items[i].spawnPosition);
                        
                    }//spawnWait > 0

                }//itemID = ID
                
            }//!active
            
        }//for i items
        
        for(int i2 = 0; i2 < items.Count; ++i2 ) {

            if(items[i2].active){

                activeCount += 1;

            }//!active

        }//for i2 items
        
    }//Puzzle_Check
    
    
//////////////////////////////////////
///
///     ITEM ACTIONS
///
///////////////////////////////////////
    
    
    public IEnumerator Item_Spawn(int slot){
        
        yield return new WaitForSeconds(items[slot].spawnWait);
        
        GameObject newItem = Instantiate(items[slot].prefab, items[slot].spawnPosition);
        
    }//Item_Spawn
    
    
//////////////////////////////////////
///
///     ITEM EVENTS ACTIONS
///
///////////////////////////////////////
    
    
    private IEnumerator ItemEvents_Trigger(int slot){
        
        if(items[slot].itemEvents.Count > 0){
            
            for(int ie = 0; ie < items[slot].itemEvents.Count; ++ie ) {
             
                if(items[slot].itemEvents[ie].delayEvent){
                    
                    tempSlot = slot;
                    StartCoroutine("ItemEvent_Buff", ie);
                    
                //delayEvent
                } else {
                    
                    items[slot].itemEvents[ie].newEvent.Invoke();
                    
                }//delayEvent
                
                yield return new WaitForSeconds(items[slot].itemEvents[ie].nextWait);
                
            }//for ie itemEvents
            
        }//itemEvents.Count > 0
        
    }//ItemEvents_Trigger
    
    private IEnumerator ItemEvent_Buff(int slot){
        
        yield return new WaitForSeconds(items[tempSlot].itemEvents[slot].eventWait);
        
        items[tempSlot].itemEvents[slot].newEvent.Invoke();
        
    }//ItemEvent_Buff
    
    
//////////////////////////////////////
///
///     INTERACTION ACTIONS
///
///////////////////////////////////////
    
    
    public void Interaction_State(bool state){
        
        if(state){
            
            if(!complete){

                interaction.SetActive(state);

            }//!complete
        
        //state
        } else {
            
            interaction.SetActive(state);
            
        }//state
        
    }//Interaction_State
    
    
//////////////////////////////////////
///
///     COMPLETE ACTIONS
///
///////////////////////////////////////
    
    
    public void CompleteCheck(){
        
        if(activeCount == items.Count){

            Complete_State(true);

        }//activeCount = items.Count
        
        if(complete){
            
            Locked_State(true);
            
            if(useCompleteDelay){
            
                CompleteCheck_Delayed();
                
            //useCompleteDelay
            } else {
            
                onPuzzleComplete.Invoke();
            
            }//useCompleteDelay
            
            if(useDelayedEvent){
            
                Complete_Delayed();
            
            }//useDelayedEvent
           
        //complete
        } else {

            //not complete
            
        }//complete
        
    }//CompleteCheck
    
    public void CompleteCheck_Delayed(){
    
        StartCoroutine("CompleteCheck_Delay");
    
    }//CompleteCheck_Delayed
    
    private IEnumerator CompleteCheck_Delay(){
    
        yield return new WaitForSeconds(completeDelay);
        
        onPuzzleComplete.Invoke();
        
    }//CompleteCheck_Delay
    
    public void Complete_Delayed(){
    
        StartCoroutine("CompleteDelayed_Buff");
    
    }//Complete_Delayed
    
    private IEnumerator CompleteDelayed_Buff(){
    
        yield return new WaitForSeconds(delayEventWait);
        
        onPuzzleCompleteDelayed.Invoke();
        
    }//CompleteDelayed_Buff
    
    public void Complete_State(bool state){
    
        complete = state;
    
    }//Complete_State
    
    
//////////////////////////////////////
///
///     LOCK ACTIONS
///
///////////////////////////////////////
    
    
    public void Locked_State(bool state){
    
        locked = state;
        
    }//Locked_State
    
    
    //////////////////////////
    //
    //      LOCALIZATION ACTIONS
    //
    //////////////////////////
        
        
    #if TW_LOCALIZATION_PRESENT
        
        void OnChangeLocalization(string[] texts) {

            tempEmptyText = texts[0];
            tempSelectText = texts[1];
            
        }//OnChangeLocalization
            
        void OnUnsubscribe(){
            
            LocalizationSystem.Unsubscribe(this);
            
        }//OnUnsubscribe
        
    #endif
    
    
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
            
            {"activeList", activeList},
            {"complete", complete},
            {"locked", locked }
        
        };//Dictionary
        
    }//OnSave
    
    
////////////////////////////
///
///     LOAD ACTIONS
///
////////////////////////////
    

    public void OnLoad(JToken token) {
        
        activeList = token["activeList"].ToObject<List<bool>>();
        complete = (bool)token["complete"];
        locked = (bool)token["locked"];
        
        if(complete) {
            
            interaction.SetActive(false);
            
            for(int i = 0; i < items.Count; ++i ) {

                items[i].active = true;
                
                for(int h = 0; h < items[i].holders.Count; ++h ) {
                
                    items[i].holders[h].SetActive(true);
                    items[i].onActiveLoad.Invoke();
                        
                }//for h holders

            }//for i items
            
        //complete
        } else {
            
            if(activeList.Count > 0){
         
                for(int i = 0; i < items.Count; ++i ) {

                    items[i].active = activeList[i];

                    if(items[i].active){

                        for(int h = 0; h < items[i].holders.Count; ++h ) {

                            items[i].holders[h].SetActive(true);
                            items[i].onActiveLoad.Invoke();

                        }//for h holders

                    }//active

                }//for i items
                
            }//activeList.Count > 0
            
        }//complete
        
    }//OnLoad
    

}//HFPS_SimplePuzzle
