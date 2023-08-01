using Unity.VisualScripting;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // CONTROLLER INPUTS
    public bool[] grab = new bool[2];
    public bool[] trigger = new bool[2];
    public bool[] button_a = new bool[2];
    public bool[] button_b = new bool[2];

    // OBJECTS
    public Transform[] hand = new Transform[2];

    // STATS
    public int[] actionable = new int[2];  // 0 = nothing, 1 = holding item
    public Vector3[] handVel = new Vector3[2];
    public Vector3[] handRotVel = new Vector3[2];

    public bool debug;

    // PRIVATE VARIABLES
    private Vector3[,] lastPos = new Vector3[2, 4];  /* x by y grid where x is LR hand, and y is number of positions stored for average velocity calc. y = 0 = current hand position
                                                      * y = 4 means 3 past frames of smoothing on throwing velocity */
    private Quaternion[] lastRot = new Quaternion[2];

    private void Start()
    {


    }

    private void FixedUpdate()
    {
        //GetVelocity(0);
        GetVelocity(1);

        GetAngularVelocity(1);

        if (debug)
            if (Input.GetKeyDown(KeyCode.E))
                grab[1] = true;
            else if(Input.GetKeyUp(KeyCode.E))
                grab[1] = false;
    }

    /* Creates a rolling average of velocities using lastPos[x,y]
     * where x is the hand whose velocity is being calculated
     * and y is the number of past frames used to extrapolate average.
     * Past frames are given decreasing weights for calculating velocity 
     * Formula: Velocity = (nFrameVelocity / 2^n) + ... + (nLastFrameVelocity / 2^nLast)*2
     * Last is multiplied by 2 to fill the gap since it gets infinitely small */
    private void GetVelocity(int handLR)
    {

        // Cycle positions
        for (int n = lastPos.GetLength(1)-1; n >0; n--)
        {
            lastPos[handLR, n] = lastPos[handLR, n-1];
        }
        lastPos[handLR, 0] = hand[handLR].position;

        Vector3 velocity = Vector3.zero;

        // Add values
        velocity = (lastPos[handLR, lastPos.GetLength(1) - 2] - lastPos[handLR, lastPos.GetLength(1) - 1]) / Mathf.Pow(2, lastPos.GetLength(1)-1);
        for (int n = lastPos.GetLength(1) - 1; n > 0; n--)  
        {
            velocity += ( lastPos[handLR,n-1] - lastPos[handLR,n] ) / Mathf.Pow(2,n);
            //Debug.Log("VELOCITY " + n + " = " + velocity);
        }

        handVel[handLR] = velocity;
    }

    private void GetAngularVelocity(int handLR)
    {

        Quaternion deltaRotation = hand[handLR].rotation * Quaternion.Inverse(lastRot[handLR]);

        lastRot[handLR] = hand[handLR].rotation;

        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

        angle *= Mathf.Deg2Rad;

        handRotVel[handLR] = (1.0f / Time.deltaTime) * angle * axis;

        //Debug.Log(handRotVel[handLR]);











        /*  Quaternion q = lastRot[handLR,0] * Quaternion.Inverse(lastRot[handLR, 1]);
          // no rotation?
          // You may want to increase this closer to 1 if you want to handle very small rotations.
          // Beware, if it is too close to one your answer will be Nan
          if (Mathf.Abs(q.w) > 1022.5f / 1024.0f)
          {
              handRotVel[handLR] = Vector3.zero;
          }
          float gain;
          // handle negatives, we could just flip it but this is faster
          if (q.w < 0.0f)
          {
              float angle = Mathf.Acos(-q.w);
              gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
          }
          else
          {
              float angle = Mathf.Acos(q.w);
              gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
          }

          lastRot[handLR, 1] = lastRot[handLR, 0];
          lastRot[handLR, 0] = hand[handLR].rotation;

          Debug.Log(new Vector3(q.x * gain, q.y * gain, q.z * gain));

          handRotVel[handLR] = new Vector3(q.x * gain, q.y * gain, q.z * gain);*/

        //Debug.Log(hand[handLR].GetComponent<Rigidbody>().angularVelocity);
    }


}
