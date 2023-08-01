using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/* Weapons have a copy of this class to request shot raycasts & damage application */

public class Gun : MonoBehaviour
{
    public LayerMask mask;

    public float dam;
    public float pen;

    private NPCStats victim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(Vector3 barrel, Vector3 dir)
    {
        dir = dir.normalized * 1000;

        RaycastHit hit;
        bool ray = Physics.Raycast(barrel, dir, out hit, 1000, mask);  // Fire raycast from barrel toward direction
        Debug.DrawLine(barrel, dir, Color.yellow, 0.25f);

        if(ray)
        {
            FindVictim(hit);  // Find 
            victim.InflictDamage(dam, pen);
        }

    }

    private void FindVictim(RaycastHit hit)
    {
        victim = hit.transform.GetComponent<NPCHitbox>().npcstats;
        victim.hitLimb = hit.transform.GetComponent<NPCHitbox>().limb;
    }


}
