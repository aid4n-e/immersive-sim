using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public ReferenceManager refManager;
    public Transform itemObj;
    public Vector3 posOffset;
    public Quaternion rotOffset;
     
    public bool grabbable = true;
    public int handLR = -1;  // -1 = not held, 0 = left, 1 = right
    public bool usable = true;
    public float grabSpeed = 10;

    public float grabCooldown = 0.25f;
    public Transform centerOfMass;

    public float maxAngVel = 30;
    public int activeLayer = 9;
    public int cooldownLayer = 10;

    void Start()
    {
        if (itemObj == null)
            itemObj = this.transform;

        if(centerOfMass)
            itemObj.GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
        itemObj.GetComponent<Rigidbody>().maxAngularVelocity = maxAngVel;

        Transform[] children = itemObj.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform child in children)
        {
            // Debug.Log(child.name);
            child.gameObject.layer = activeLayer;  // Disable player collision
        }
    }

    void Update()
    {
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(centerOfMass)
            Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass.localPosition, 0.01f);
    }*/

}
