using System.Collections;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    public InputController input;
    public ReferenceManager refManager;

    public int handLR;
    public Transform hand;

    public Item heldItem;
    public Item triggeredItem;
    public Transform triggeredItemObj;

    private void Update()
    {
        if (input.grab[handLR] && input.actionable[handLR] == 0)
        {
            StartGrab();
        }
        if (!input.grab[handLR] && input.actionable[handLR] == 1)
        {
            StartCoroutine(Drop(heldItem));
        }

        if (input.trigger[handLR] && input.actionable[handLR] == 0)
        {
            StartAction();
        }

    }

    private void StartGrab()
    {
        if (triggeredItem != null && triggeredItem.grabbable)  // Ensure an item trigger was recognized
        {
            StartCoroutine(Grab(triggeredItem));
        }
    }


    public IEnumerator Grab(Item item)
    {
        input.actionable[handLR] = 1;
        heldItem = item;
        Transform itemObj = item.itemObj;
        itemObj.parent = refManager.objectParent;
        item.grabbable = false;
        item.usable = false;
        itemObj.GetComponent<Rigidbody>().isKinematic = true;  // Disable item physics

        Quaternion newRot = hand.rotation * item.rotOffset;
        Vector3 newPos = hand.position + hand.InverseTransformDirection(item.posOffset);

        float lerpSpeed = item.grabSpeed;  // Copy grab speed so it's not overwritten

        while (Vector3.Distance(item.itemObj.position, hand.position) > 0.001f)
        {
            if (heldItem == null)
            {
                yield break;
            }

            newRot = hand.rotation * item.rotOffset;
            newPos = hand.position + hand.InverseTransformDirection(item.posOffset);

            item.itemObj.rotation = Quaternion.Lerp(item.itemObj.rotation, newRot, Time.deltaTime * 10);
            item.itemObj.position = Vector3.Lerp(item.itemObj.position, newPos,  Time.deltaTime * 10);

            lerpSpeed += Time.deltaTime * 10 * lerpSpeed;  // Grow speed so it doesn't chase forever
            //lerpSpeed = Mathf.Clamp(lerpSpeed * Time.deltaTime * 10, 0f, 0.999f);  // Grow speed so it doesn't chase forever

            yield return null;
        }

        item.itemObj.position = newPos;
        item.itemObj.parent = hand;
        item.handLR = handLR;
        item.usable = true;
    }

    private IEnumerator Drop(Item item)
    {
        item.grabbable = false;
        item.handLR = -1;
        item.itemObj.GetComponent<Rigidbody>().isKinematic = false;  // Enable item physics

        item.itemObj.parent = item.refManager.objectParent;
        input.actionable[handLR] = 0;

        Transform[] children = item.itemObj.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform child in children)
        {
            // Debug.Log(child.name);
            child.gameObject.layer = item.cooldownLayer;  // Disable player collision
        }

        item.itemObj.GetComponent<Rigidbody>().AddForce(input.handVel[handLR] * 2000);
        item.itemObj.GetComponent<Rigidbody>().angularVelocity = input.handRotVel[handLR] * 500;

        //Item itemMem = triggeredItem;  // Remember which item was dropped

        float elapsed = 0;
        while (elapsed < item.grabCooldown)  // Wait until cooldown
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (Transform child in children)
        {
            // Debug.Log(child.name);
            child.gameObject.layer = item.activeLayer;  // Enable player collision
        }

        item.grabbable = true;
    }


    void StartAction()
    {

    }







    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("ItemTrigger") && triggeredItem == null)
        {
            triggeredItem = other.GetComponentInParent<Item>();
            triggeredItemObj = other.GetComponentInParent<Item>().itemObj;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Item>() == triggeredItem)
        {
            triggeredItem = null;
            triggeredItemObj = null;
        }
    }

}