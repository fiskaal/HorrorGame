using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowing : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject RockPrefab;

    [SerializeField] private Inventory inventory;
    [SerializeField] private int throwCooldown;

    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;

    bool readyToThrow = true;

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && inventory.totalRocks > 0)
            Throw();
    }
    private void Throw()
    {
        readyToThrow = false;
        GameObject rockPorjectile = Instantiate(RockPrefab, throwPoint.position, cam.rotation);
        Rigidbody rockProjectileRB = rockPorjectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
        rockProjectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        inventory.SubstractRock();

        Invoke(nameof(ResetThrow), throwCooldown);
    }
    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
