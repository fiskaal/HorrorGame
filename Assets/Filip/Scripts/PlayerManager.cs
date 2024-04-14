using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public FlashLight fl;
    public GameObject baterkaObject;
    public bool inReach;
    public GameObject pickupText;
    // Start is called before the first frame update
    void Start()
    {
        inReach = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (inReach && Input.GetButtonDown("Interact"))
        {
            fl.gameObject.SetActive(true);
            baterkaObject.gameObject.SetActive(false);
        }

        

    }

    
}
