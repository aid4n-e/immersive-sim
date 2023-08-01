/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    private EnemyAlertState alertState = new EnemyAlertState();
    private EnemySearchState searchState = new EnemySearchState();
    private EnemyPatrolState patrolState = new EnemyPatrolState();

    private EnemyAwareState currentState;

    public Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeState(EnemyAwareStates.Patrolling);
    }

    void Update()
    {
        currentState.Update(this);
    }


    public void ChangeState(EnemyAwareStates state)
    {
        if (currentState != null)
            currentState.Exit(this);

        switch (state)
        {
            case EnemyAwareStates.Patrolling:
                currentState = patrolState;
                break;
            case EnemyAwareStates.Searching:
                currentState = searchState;
                break;
            case EnemyAwareStates.Alerted:
                currentState = alertState;
                break;

        }
        currentState.Enter(this);
    }

}


public abstract class EnemyAwareState
{
    public abstract void Enter(EnemyAwareness player);
    public abstract void Exit(EnemyAwareness player);
    public abstract void Update(EnemyAwareness player);
}


public class EnemyPatrolState : EnemyAwareState
{
    public override void Enter(EnemyAwareness player)
    {

    }

    public override void Exit(EnemyAwareness player)
    {

    }

    public override void Update(EnemyAwareness player)
    {

    }
}

public class EnemySearchState : EnemyAwareState
{
    public override void Enter(EnemyAwareness player)
    {

    }

    public override void Exit(EnemyAwareness player)
    {

    }

    public override void Update(EnemyAwareness player)
    {

    }
}

public class EnemyAlertState : EnemyAwareState
{
    public override void Enter(EnemyAwareness player)
    {

    }

    public override void Exit(EnemyAwareness player)
    {

    }

    public override void Update(EnemyAwareness player)
    {

    }
}


public enum EnemyAwareStates
{
    Patrolling,
    Alerted,
    Searching,
};*/