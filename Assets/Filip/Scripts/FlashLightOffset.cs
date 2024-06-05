using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightOffset : MonoBehaviour
{
    [SerializeField] private Vector3 vectOffset;
    [SerializeField] private GameObject goFollow;
    [SerializeField] private float speed = 3.0f;

    
    private void Start()
    {
        vectOffset = transform.position - goFollow.transform.position;
    }

    private void Update()
    {
        transform.position = goFollow.transform.position + vectOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }
}
