using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowing : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject rockObject;
    [SerializeField] private Rigidbody rockObjectRB;

    [SerializeField] private Inventory inventory;

    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;

    bool readyToThrow = false;

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && inventory.totalRocks > 0)
            Throw();
    }
    private void Throw()
    {
        readyToThrow = false;
        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForce;
        rockObject.layer = default;
        rockObjectRB.useGravity = true;
        rockObject.transform.SetParent(null);
        rockObjectRB.AddForce(forceToAdd, ForceMode.Impulse);
        inventory.SubstractRock();
        inventory.ActiveItemUsed();
    }
    public void ReadyThrow(GameObject rock)
    {
        readyToThrow = true;
        rockObject = rock;
        rockObjectRB = rock.GetComponent<Rigidbody>();
    }
}
