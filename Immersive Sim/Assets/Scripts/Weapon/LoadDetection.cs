using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDetection : MonoBehaviour
{
    public Pistol pistol;

    public string type;

    private bool occupied = false;

    private void Start()
    {
        pistol = GetComponentInParent<Pistol>();
    }

    private void Update()
    {
        
    }

}
