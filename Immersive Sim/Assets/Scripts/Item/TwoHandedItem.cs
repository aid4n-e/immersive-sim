using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class TwoHandedItem : MonoBehaviour
{
    public Transform dominant, off, shoulder;
    public Transform domGrip, offGrip;
    public Transform item;

    public Vector3 domPosOffset;
    public Vector3 offPosOffset;


    private void Start() {
        item = this.transform;

        domPosOffset = this.transform.position - domGrip.position;
        offPosOffset = this.transform.position - offGrip.position;

    }

    void Update()
    {
        Vector3 domPos = dominant.position + domPosOffset;
        Vector3 offPos = off.position + offPosOffset;

        item.position = domPos;

        //item.localRotation = new Quaternion(item.localRotation.x, item.localRotation.y, dominant.localRotation.z, item.localRotation.w);

        Vector3 fwd = offPos - domPos - shoulder.position;


        item.forward = fwd;
    }
}
