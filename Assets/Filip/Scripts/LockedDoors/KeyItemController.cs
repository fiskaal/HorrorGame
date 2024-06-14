using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KeySystem { 

    public class KeyItemController : MonoBehaviour
    {
        [SerializeField] private bool redDoor = false;
        [SerializeField] private bool redKey = false;

        [SerializeField] private bool orangeDoor = false;
        [SerializeField] private bool orangeKey = false;

        [SerializeField] private bool blueDoor = false;
        [SerializeField] private bool blueKey = false;

        [SerializeField] private bool safeDoor = false;
        [SerializeField] private bool safeKey = false;

        [SerializeField] private bool redDoorLocked = true;
        [SerializeField] private bool orangeDoorLocked = true;
        [SerializeField] private bool blueDoorLocked = true;

        [SerializeField] private UnityEvent pickUpEvent;
        [SerializeField] private UnityEvent unlockedEvent;

        [SerializeField] private Inventory inventory = null;

        private KeyDoorController doorObject;

        private void Start()
        {
            if (redDoor) 
            { 
                doorObject = GetComponent<KeyDoorController>();
            }

            if (orangeDoor)
            {
                doorObject = GetComponent<KeyDoorController>();
            }

            if (blueDoor)
            {
                doorObject = GetComponent<KeyDoorController>();
            }

            if (safeDoor)
            {
                doorObject = GetComponent<KeyDoorController>();
            }
        }

        public void ObjectInteraction()
        {
            if (redDoor)
            {
                if (redDoorLocked && inventory.hasRedKey)
                {
                    unlockedEvent.Invoke();
                    redDoorLocked = false;
                }
                else
                    doorObject.PlayAnimation();

            }else if (redKey)
            {
                inventory.PickedUpRedKey();
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }

            if (orangeDoor)
            {
                if (orangeDoorLocked && inventory.hasOrangeKey)
                {
                    unlockedEvent.Invoke();
                    orangeDoorLocked = false;
                }
                else
                    doorObject.PlayAnimation();
            }
            else if (orangeKey)
            {
                inventory.PickedUpOrangeKey();
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }

            if (blueDoor)
            {
                if (blueDoorLocked && inventory.hasBlueKey)
                {
                    unlockedEvent.Invoke();
                    blueDoorLocked = false;
                }
                else
                    doorObject.PlayAnimation();
            }
            else if (blueKey)
            {
                inventory.PickedUpBlueKey();
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }

            if (safeDoor)
            {
                doorObject.PlaySafeAnimation();
            }
            else if (safeKey)
            {
                inventory.PickedUpSafeKey();
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }
        }

    }
}
