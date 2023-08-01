using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private StandingState standingState = new StandingState();
    private DuckingState duckingState = new DuckingState();

    private PlayerState currentState;

    public Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeState(PlayerStates.Standing);
    }

    void Update()
    {
        currentState.Update(this);
    }


    public void ChangeState(PlayerStates state)
    {
        if (currentState != null)
            currentState.Exit(this);

        switch (state)
        {
            case PlayerStates.Standing:
                currentState = standingState;
                break;
            case PlayerStates.Ducking:
                currentState = duckingState;
                break;
        }
        currentState.Enter(this);
    }

}


public abstract class PlayerState
{
    public abstract void Enter(PlayerController player);
    public abstract void Exit(PlayerController player);
    public abstract void Update(PlayerController player);
}


public class StandingState : PlayerState
{
    public override void Enter(PlayerController player)
    {
        
    }

    public override void Exit(PlayerController player)
    {

    }

    public override void Update(PlayerController player)
    {

    }
}


public class DuckingState : PlayerState
{
    public override void Enter(PlayerController player)
    {

    }

    public override void Exit(PlayerController player)
    {

    }

    public override void Update(PlayerController player)
    {

    }
}

public enum PlayerStates
{
    Standing,
    Ducking,
};

