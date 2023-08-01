using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHitbox : MonoBehaviour
{
    public NPCStats npcstats;

    public int limb;

    // Start is called before the first frame update
    void Start()
    {
        npcstats = gameObject.GetComponentInParent<NPCStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
