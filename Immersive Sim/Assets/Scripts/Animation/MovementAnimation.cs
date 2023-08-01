using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEngine.GraphicsBuffer;

public class MovementAnimation : MonoBehaviour
{

    public Animator animator;

    public void MoveAnimate(float angle, float speed)
    {
        angle = angle * Mathf.Deg2Rad;

        float x = 0;
        float z = 0;
        /*if(angle > 0)
        {
            x = Mathf.Sin(angle) * speed;
            //z = Mathf.Sin(90 - angle) * speed;
        }
        else if(angle < 0)
        {
            x = Mathf.Sin(angle) * speed;
            //z = Mathf.Sin(90 + angle) * speed;
        }
        */

        x = Mathf.Sin(angle) * speed;
        z = (1- Mathf.Abs(x)) * speed;

        animator.SetFloat("Velocity Z", z);
        animator.SetFloat("Velocity X", x);

        Debug.Log("Z = " + z + ", X = " + x);
    }

}