using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalk : MonoBehaviour
{
    public Transform body;
    public Transform[] legs;
    public Transform[] stepCheck;
    public Vector3[] oldStep;
    public Vector3[] stepOffset;
    public Vector3[] stepAdjust;
    public float speed;

    public float stepThreshold;
    private float[] dist;
    public Vector3[] steps;
    public Vector3[] compensate;

    public Transform[] debug;

    private RaycastHit hit;

    public int turn = 0;

    private bool[] moving;
    void Start()
    {
        steps = new Vector3[legs.Length];
        dist = new float[legs.Length];
        moving = new bool[legs.Length];
        compensate = new Vector3[legs.Length];

        for(int n = 0; n < legs.Length; n++)
        {
            Debug.Log(n);
            //RaycastHit test;
            //Physics.Raycast(legs[n].position + (Vector3.up * 0.5f), -Vector3.up, out test);
            //steps[n] = test.point + stepAdjust[n];
            oldStep[n] = legs[n].position;
            steps[n] = oldStep[n];
        }
    }

    void Update()
    {
        for(int n = 0; n<legs.Length;n++)
        {
            Debug.Log(n);
            MoveLeg(n);
        }



    }

    Vector3 GetPosition(int n)
    {
        if (Physics.Raycast( new Vector3(body.position.x - oldStep[n].x, body.position.y, body.position.z - oldStep[n].z)  + (Vector3.up * 1) + stepOffset[n], -Vector3.up, out hit))
        {
            //print("Found an object - distance: " + hit.distance);
            return hit.point + stepAdjust[n];
        }
        else
            return new Vector3(0, 0, 0);
    }


    void MoveLeg(int n)
    {


        if (moving[n])
        {
            legs[n].position = Vector3.Lerp(legs[n].position, steps[n], speed * Time.deltaTime);

            if (Vector3.Distance(legs[n].position, steps[n]) < 0.1f)
            {
                legs[n].position = steps[n];

                if (n < legs.Length-1)
                {
                    turn = n + 1;
                    oldStep[n + 1] = legs[n + 1].position;
                }
                else
                {
                    turn = 0;
                    oldStep[0] = legs[0].position;
                }

                moving[n] = false;
            }
        }
        else
        {
            legs[n].position = steps[n];
            stepCheck[n].position = GetPosition(n);

            dist[n] = Vector3.Distance(stepCheck[n].position - legs[n].forward*stepThreshold, oldStep[n]);

            if (dist[n] > stepThreshold && turn == n)
            {
                moving[n] = true;
                steps[n] = stepCheck[n].position;
            }
        }
    }
}
