using UnityEngine;
using UnityEngine.AI;

public class Pathfinder : MonoBehaviour
{
    private NavMeshPath path;

    public Vector3[] Pathfind(Vector3 origin, Vector3 target)
    {
        path = new NavMeshPath();
        if (NavMesh.CalculatePath(origin, target, NavMesh.AllAreas, path))
        {
            return path.corners;
        }
        else
        {
            return null;
        }
    }
}