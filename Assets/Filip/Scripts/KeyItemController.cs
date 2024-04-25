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

        [SerializeField] private UnityEvent pickUpEvent;


        [SerializeField] private KeyInventory _keyInventory = null;

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
        }

        public void ObjectInteraction()
        {
            if (redDoor)
            {
                doorObject.PlayAnimation();
            }else if (redKey)
            {
                _keyInventory.hasRedKey = true;
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }

            if (orangeDoor)
            {
                doorObject.PlayAnimation();
            }
            else if (orangeKey)
            {
                _keyInventory.hasOrangeKey = true;
                pickUpEvent.Invoke();
                gameObject.SetActive(false);
            }
        }

    }
}
