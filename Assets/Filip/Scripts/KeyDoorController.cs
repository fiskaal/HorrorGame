using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace KeySystem { 
    public class KeyDoorController : MonoBehaviour
    {
        private Animator doorAnim;
        private bool doorOpen = false;

        //[Header("Animation Names")]
        //[SerializeField] private string openAnimationName = "Open";
        //[SerializeField] private string closedAnimationName = "Closed";
        //[SerializeField] private string lockedAnimationName = "Locked";

        [SerializeField] private int timeToShowUI = 1;
        [SerializeField] private GameObject showDoorLockedUI = null;

        [SerializeField] private KeyInventory _keyInventory = null;

        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;

        [SerializeField] private UnityEvent openEvent;
        [SerializeField] private UnityEvent closeEvent;
        [SerializeField] private UnityEvent lockedEvent;

        private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }

        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }

        private IEnumerator ShowDoorLocked()
        {
            showDoorLockedUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            showDoorLockedUI.SetActive(false);

        }

        public void PlayAnimation()
        {
            if (_keyInventory.hasRedKey)
            {
                OpenDoor();
            }
            else if (_keyInventory.hasOrangeKey)
            {
                OpenDoor();
            }
            else
            {
                doorAnim.Play("Locked", 0, 0.0f);
                lockedEvent.Invoke();
                StartCoroutine(ShowDoorLocked());
            }
        }

        void OpenDoor()
        {
            if (!doorOpen && !pauseInteraction)
            {
                doorAnim.Play("Open", 0, 0.0f);
                doorOpen = true;
                openEvent.Invoke();
                StartCoroutine(PauseDoorInteraction());
            }
            else if (doorOpen && !pauseInteraction)
            {
                doorAnim.Play("Closed", 0, 0.0f);
                doorOpen = false;
                closeEvent.Invoke();
                StartCoroutine(PauseDoorInteraction());
            }
        }
    }

    
}
