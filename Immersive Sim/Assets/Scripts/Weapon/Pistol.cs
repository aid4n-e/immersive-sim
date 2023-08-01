using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

/* Handles reloading, ammo counting, and firing of the Pistol weapon */


public class Pistol : MonoBehaviour
{

    public Gun gun;

    public Ammunition ammunition;

    public Transform slideHand;

    public Transform barrel;
    public Transform[] slidePos;  // 0 = actual slide position, 1 = memory of neutral slide position 

    public Transform lastMag;  // Tracks mag to be loaded
    public Transform[] loadPoints;  // 0 = base of load area, 1 = inside load area

    public Vector3 startPos;
    private Vector3 lastPos;

    public bool triggerInput = false;
    public bool ejectInput = false;
    public bool slideInput = false;
    public bool loadInput = false;

    public int slideState = 0;  /* 0 if untouched, 1 if grabbed (move slide), 2 if fully pulled (eject round), 3 if neutral after fully pulled (chamber round)
                                 * additionally, -1 if released from partial pull (do nothing), -2 if released from full pull (chamber round) */



    public bool chambered = false;  // Track if bullet is in chamber


    private int fireMode = 0;

    public float fireCooldown;
    public float actionCooldown;


    public float ammo = 0;

    public bool loaded = false;
    public bool loading = false;  // Mag is being loaded
    public bool occupied = false;



    private Vector3 dir;


    // Start is called before the first frame update
    void Start()
    {
        gun = this.GetComponent<Gun>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (triggerInput && !occupied)
        {
            Fire();
        }

        else if (ejectInput && !occupied)
        {
            MagEject();
        }

        else if (loadInput && !loading)
        {
            Load(slideHand);
        }

        Slide(slideHand);

    }


    private void Fire()
    {
        if (chambered)
        {
            gun.Fire(barrel.position, barrel.forward);

            // If semi, force disable trigger
            if (fireMode == 0)
            {
                triggerInput = false;
            }

            Chamber();

        }

        else
        {
            //empty click sfx
        }
    }

    // Update slide position when grabbed
    private void Slide(Transform hand)
    {
        if (slideInput && slideState == 0)
        {
            slideState = 1;
            StartCoroutine(SlideLoop(hand));
        }
        else if(!slideInput && slideState > 0)
        {
            slideState *= -1;
        }
    }


    IEnumerator SlideLoop(Transform hand)
    {
        /* FORGET THIS SHIT - LERP IS THE WAY
         * TWO LOAD POINTS
         * SNAP TO BOTTOM POINT, THEN SET START POSITION AS HAND POSITION
         * float zDis ALREADY WORKS
         * Vector3.Lerp(bottompoint, toppoint, zDis)
         * 
         * FORGET THAT SHIT
         * magazine position = starting loadpoint + clamped zDis, don't reset zDis every update
         * */


        startPos = hand.position;

        float snap = 0f;

        float railDis = Vector3.Distance(slidePos[1].position, slidePos[2].position);

        Vector3 releasePos = Vector3.zero;
        float slideToNeutralDis = 0;

        while (slideState != 0)
        {
            if (slideState > 0)  // If holding, follow hand
            {
                Vector3 startDis = slidePos[0].InverseTransformPoint(startPos);
                Vector3 handDis = slidePos[0].InverseTransformPoint(hand.position);
                Vector3 dis = handDis - startDis;
                float zDis = Mathf.Clamp(dis.z, -railDis, 0f);

                slidePos[0].position = slidePos[1].position;
                //slidePos[0].localPosition -= slidePos[0].TransformDirection(new Vector3(0, 0, zDis));
                slidePos[0].position += transform.forward * zDis;
            }
            else if (slideState < 0)  // If not holding, snap to neutral
            {
                if(releasePos == Vector3.zero)
                {
                    releasePos = slidePos[0].position;
                    slideToNeutralDis = Vector3.Distance(slidePos[0].position, slidePos[1].position);
                }

                snap = Mathf.Clamp(snap + 0.05f, 0, slideToNeutralDis);
                slidePos[0].position = releasePos;
                slidePos[0].position += slidePos[0].forward * snap;

                startPos = Vector3.zero;
            }


            if (slideState != 2 && slideState != -2 && Vector3.Distance(slidePos[0].position, slidePos[2].position) < 0.01f)  // Check if slide is within fully pulled distanced (eject)
            {
                //Debug.Log("- PULLED -");
                if (slideState == 1)
                {
                    slideState = 2;
                    BulletEject();
                }
            }

            else if (slideState != 1 && slideState != -1 && Vector3.Distance(slidePos[0].position, slidePos[1].position) < 0.1f)  // Check if slide is within chambering distance
            {
                //Debug.Log("- HALF PULL -");
                if (slideState == 2)  // If slide was fully pulled, then held to neutral: chamber round
                {
                    Chamber();
                    slideState = 1;  // Keep holding
                }

                else if (slideState == -2)  // If slide was fully pulled, then released to neutral: chamber round
                {
                    Chamber();
                    slideState = -1;
                }
            }

            else if (slideState == -1 && Vector3.Distance(slidePos[0].position, slidePos[1].position) < 0.001f)  // Check if slide is within neutral distance
            {
                //Debug.Log("- NEUTRAL -");
                slideState = 0;  // Let go
            }

            yield return null;

        }
    }



    private void Chamber()
    {
        Debug.Log("- CHAMBERED -");

        if(loaded && ammo > 0)
        {
            chambered = true;
            ammo--;
        }
    }

    private void BulletEject()
    {
        Debug.Log("- BULLET EJECTED -");
        chambered = false;
    }
    
    private void MagEject()
    {
        ejectInput = false;

        ammunition.ammo = ammo;
        ammo = 0;
        lastMag.GetComponent<Rigidbody>().isKinematic = false;
        lastMag.GetComponent<Rigidbody>().AddForce(-loadPoints[0].forward * 10);
        lastMag.GetComponent<Item>().grabbable = true;
        lastMag.parent = ammunition.objectParent;

    }

    private void Load(Transform hand)
    {
        if(lastMag.GetComponent<Item>().usable)
        {
            StartCoroutine(LoadLoop(hand));
        }

    }

    IEnumerator LoadLoop(Transform hand)
    {
        /* FORGET THIS SHIT - LERP IS THE WAY
         * TWO LOAD POINTS
         * SNAP TO BOTTOM POINT, THEN SET START POSITION AS HAND POSITION
         * float zDis ALREADY WORKS
         * Vector3.Lerp(bottompoint, toppoint, zDis)
         * 
         * FORGET THAT SHIT
         * magazine position = starting loadpoint + clamped zDis, don't reset zDis every update
         * */

        loadInput = false;
        loading = true;
        lastMag.GetComponent<Item>().usable = false;
        lastMag.GetComponent<Rigidbody>().isKinematic = true;
        lastMag.parent = this.transform;
        lastMag.position = loadPoints[0].position;

        startPos = hand.position;

        while (!loaded)
        {
            Vector3 startDis = loadPoints[0].InverseTransformPoint(startPos);
            Vector3 handDis = loadPoints[0].InverseTransformPoint(hand.position);
            Vector3 dis = handDis - startDis;
            Vector3 rail = loadPoints[1].position - loadPoints[0].position;
            float railDis = Vector3.Distance(loadPoints[0].position, loadPoints[1].position);
            float zDis = Mathf.Clamp(dis.z, 0f, railDis);

            //float projLength = Mathf.Clamp(Vector3.Dot(hand.position - loadPoints[0].position, loadPoints[0].forward.normalized), 0f, (hand.position - loadPoints[0].position).magnitude);   
            //float handToRail = (loadPoints[0].position + loadPoints[0].right.normalized * projLength).magnitude;

            float handToRailDis = Vector3.ProjectOnPlane(hand.position - loadPoints[0].position, loadPoints[0].forward.normalized).magnitude;  // Vector plane projection from hand to nearest point on rail

            Debug.Log(handToRailDis);
            //Debug.Log(zDis);

            lastMag.position = loadPoints[0].position;
            lastMag.position += lastMag.forward * zDis;

            if ((lastMag.position - loadPoints[1].position).magnitude < 0.005f)
            {
                lastMag.position = loadPoints[1].position;
                break;
            }
            else if (handToRailDis > 0.1f  || ((hand.position - loadPoints[0].position).magnitude > 0.1f && rail.magnitude < (hand.position - loadPoints[1].position).magnitude))  // Break reload, reset item
            {
                StartCoroutine(hand.GetComponent<ItemGrab>().Grab( lastMag.GetComponent<Item>() ));
                float elapsed = 0;
                while (elapsed < 0.3f)
                {
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                loading = false;
                yield break;
            }

            yield return null;
        }

        // Successfully loaded
        occupied = false;
        loading = false;
        loaded = true;
        loadInput = false;

        lastMag.position = loadPoints[1].position;

        ammunition = lastMag.GetComponent<Ammunition>();
        ammo = ammunition.ammo;  // Get ammo count
    }

}