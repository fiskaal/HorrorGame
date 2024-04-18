using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NoteRaycast : MonoBehaviour
{
    [Header("Raycast Features")]
    [SerializeField] private float rayLenght = 5;
    private Camera _camera;

    private NoteController _noteController;

    [Header("Crosshair")]
    [SerializeField] private Image crosshair;
    [SerializeField] private GameObject imageE;

    [Header("Input Key")]
    [SerializeField] private KeyCode interactKey;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        imageE.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if(Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)),transform.forward, out RaycastHit hit, rayLenght))
        {

            var readableItem = hit.collider.GetComponent<NoteController>();

            if(readableItem != null)
            {
                _noteController = readableItem;
                ShowImageE(true);
            }
            else
            {
                ClearNote();
            }

        }
        else
        {
            ClearNote();
        }

        if(_noteController != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _noteController.ShowNote();
            }
        }


    }

    void ClearNote()
    {
        if(_noteController != null)
        {
            ShowImageE(false);
            _noteController = null;
        }


    }

    void ShowImageE(bool on)
    {
        if (on)
        {
            imageE.SetActive(true);
        }
        else
        {
            imageE.SetActive(false);
        }
    }

}
