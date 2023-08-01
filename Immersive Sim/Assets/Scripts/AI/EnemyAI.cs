using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    public Pathfinder pathfinder;
    public MovementAnimation anim;

    public Transform player;
    public float speed = 0.8f;
    public float turnSpeed = 1.2f;
    public float animSpeed = 0.0f;
    public int alerted = 0;
    public bool moving;
    public Vector3[] path;

    private Vector3 target;
    private Vector3 playerDir;
    private Vector3 lastPos;

    private float angle;
    private float elapsed = 0.0f;
    private float currentSpeed = 0.0f;


    void Start()
    {
        path[0] = transform.position;
        //elapsed = 0.0f;
    }

    void Update()
    {
        if (alerted == 2)
        {
                GetPath();
                if ( path.Length < 2 || (path.Length == 2 && (path[1] - path[0]).magnitude < 0.075f) )  // If remaining nodes == 2, and their distance < x, stop moving. Note that node 1, or path[0], is always this transform's position
                {
                    moving = false;
                }
                else
                {
                    moving = true;

                }
        }

        if(moving)
        {
            angle = GetAngle(path[1], this.transform);  // Get angle between forward direction and next node
            Debug.Log("ANGLE = " + angle);
            MoveTo();  // Interpolate towards next node
            //currentSpeed = GetSpeed();  // Calculate animation speed
        }
        else
        {
            anim.MoveAnimate(angle, 0);  // Apply animation
        }
    }

    private void GetPath()
    {
        playerDir = player.position - this.transform.position; 
        target = (player.position - playerDir.normalized) * 1;  // End path within x units of target

        path = pathfinder.Pathfind(this.transform.position, target);

        /*Render path*/
        if (path != null)
        {
            for (int i = 0; i < path.Length - 1; i++)
                Debug.DrawLine(path[i], path[i + 1], Color.red, 0.25f);
        }

    }

    private void MoveTo()
    {
        Vector3 targetDir = (path[1] - path[0]).normalized;

        if(path.Length != 2 || (path.Length == 2 && (path[1] - path[0]).magnitude > 0.075f))
        {
            //Debug.Log("MoveTo proceeding");
            
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnSpeed * Time.deltaTime, 0.0f);  // Calculate rotation of this enemy toward the next point on path
            this.transform.rotation = Quaternion.LookRotation(newDir);  // Apply rotation

            /*if (angle < 5 && angle > -5)
            {
            }*/

            if (angle < 90 && angle > -90)
            {
                transform.position += targetDir * speed * Time.deltaTime;  // Move toward point
                anim.MoveAnimate(angle, 1);  // Apply animation

            }

        }

        //Debug.Log((path[1] - path[0]).magnitude);

    }

    private float GetAngle(Vector3 target, Transform origin)
    {
        Vector3 targetDir = target - origin.position;
        float newAngle = Vector3.SignedAngle(origin.forward, targetDir, Vector3.up);
        return newAngle;
    }

    private float GetSpeed()
    {
        float finalSpeed = (this.transform.position - lastPos).magnitude * animSpeed;
        lastPos = this.transform.position;

        return (finalSpeed);

    }

}