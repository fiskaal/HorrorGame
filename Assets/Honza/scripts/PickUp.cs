using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public void HidePickUp()
    {
        Destroy(gameObject);
    }
}
