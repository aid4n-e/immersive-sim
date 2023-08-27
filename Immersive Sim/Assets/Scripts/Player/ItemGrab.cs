using System.Collections;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    public InputController input;
    public ReferenceManager mgr;

    public int handLR;
    public Transform hand;

    public AudioSource src;


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
            StartCoroutine(Throw(heldItem));
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
        itemObj.parent = mgr.objectParent;
        item.grabbable = false;
        item.usable = false;
        itemObj.GetComponent<Rigidbody>().isKinematic = true;  // Disable item physics

        Quaternion newRot = hand.rotation * item.rotOffset;
        Vector3 newPos = hand.position + hand.InverseTransformDirection(item.posOffset);

        float lerpSpeed = item.grabSpeed;  // Copy grab speed so it's not overwritten

        mgr.audio.PlayRandomSound(src, 10);

        // Interpolate object toward hand position
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

        item.itemObj.position = hand.position + hand.InverseTransformDirection(item.posOffset);
        item.itemObj.rotation = hand.rotation * item.rotOffset;
        item.itemObj.parent = hand;
        item.handLR = handLR;
        item.usable = true;
    }

    // Throw item from the hand's grasp (trigger when letting go of item)
    private IEnumerator Throw(Item item)
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

    // Force the hand to drop object and become actionable (triggered when an item is taken, like loaded into a gun)
    public IEnumerator Drop()
    {
        Item item = heldItem;
        item.grabbable = false;
        item.handLR = -1;
        item.itemObj.parent = item.refManager.objectParent;
        input.actionable[handLR] = 0;

        float elapsed = 0;

        while (elapsed < item.grabCooldown)  // Wait until cooldown
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
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