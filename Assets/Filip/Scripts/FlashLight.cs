using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{

    public GameObject baterka;
    private bool OnOff = false;
    public AudioSource OnOffSound;
    // Start is called before the first frame update
    void Start()
    {
        baterka.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(OnOff == false)
            {
                baterka.gameObject.SetActive(true);
                OnOff = true;
                OnOffSound.Play();
            }
            else
            {
                baterka.gameObject.SetActive(false);
                OnOff = false;
                OnOffSound.Play();
            }

        }
    }

}
