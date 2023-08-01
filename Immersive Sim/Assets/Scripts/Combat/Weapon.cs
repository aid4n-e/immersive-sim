using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public bool debugShoot;

    public Transform barrel;
    public Vector3 aim;

    public LayerMask mask;


    public float damValue = 1;
    public float penValue = 1;


    void Start()
    {
        aim = barrel.forward;
    }


    void Update()
    {
        if(debugShoot)
        {
            FireWeapon();
            debugShoot = false;
        }
    }


    public void FireWeapon()
    {
        RaycastHit hit;

        // Fire raycast from barrel.forward
        bool ray = Physics.Raycast(barrel.position, barrel.forward, out hit, 100, mask);
        Debug.DrawRay(barrel.position, barrel.forward * 1000, Color.yellow,1);
        if (ray)
        {
            Limb limb = hit.transform.gameObject.GetComponent<Limb>();
            NPCStats npcHit = limb.npcStats;
            if (npcHit != null)
            {
                Debug.Log("Hit");
                npcHit.InflictDamage(damValue, penValue);
            }

        }




    }
}



