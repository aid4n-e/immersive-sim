using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    public Transform objectParent;
    public float ammo;

    private bool occupied;

    private Pistol pistol;

    public string type;

    private Item item;

    private void Start()
    {
        item = this.GetComponent<Item>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Magazine Detected");

        if(other.GetComponentInParent<Pistol>())
        {
            pistol = other.GetComponentInParent<Pistol>();

            if (other.tag.Equals(type + "Loader") && item.usable == true && pistol.loaded == false && pistol.loading == false && this.item.handLR != -1)
            {
                Debug.Log("LOADING");
                pistol.lastMag = this.transform;
                pistol.loadInput = true;
                item.grabbable = false;
            }
        }





    }

}
