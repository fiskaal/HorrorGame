using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class NoteController : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] private KeyCode closekey;

    [Space(10)]
    [SerializeField] public ExamplePlayerController player;

    [Header("UI text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;

    [Space(10)]
    [SerializeField] [TextArea] private string noteText;

    [Space(10)]
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] private UnityEvent hideEvent;
    private bool isOpen = false;

    private string noteName;
    public void ShowNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        //paperSound.PlayOneShot(paper);
        DisablePlayer(true);
        isOpen = true;
        noteName = gameObject.name;
        NoteOpened();
    }

    void HideNote()
    {
        noteCanvas.SetActive(false);
        hideEvent.Invoke();
        DisablePlayer(false);
        isOpen = false;

    }

    void DisablePlayer(bool disable)
    {
        player.enabled = !disable;

    }

    private void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideNote();
            }


        }
    }
    private void NoteOpened()
    {
        switch (noteName)
        {
            case "Note1":
                player.GetComponent<Map>().note1Opened = true;
                player.GetComponent<Map>().LootedZoneCheck();
                break;
            case "Note2":
                player.GetComponent<Map>().note2Opened = true;
                player.GetComponent<Map>().LootedZoneCheck();
                break;
            case "Note3":
                player.GetComponent<Map>().note3Opened = true;
                player.GetComponent<Map>().LootedZoneCheck();
                break;

        }
    }

}
