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

        [SerializeField] private UnityEvent pickUpEvent;


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
        }

        public void ObjectInteraction()
        {
            if (redDoor)
            {
                doorObject.PlayAnimation();
            }else if (redKey)
            {
                inventory.PickedUpRedKey();
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }

            if (orangeDoor)
            {
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
                doorObject.PlayAnimation();
            }
            else if (blueKey)
            {
                inventory.PickedUpBlueKey();
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }
        }

    }
}
