/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMExample : MonoBehaviour
{
    private Grab grabState = new Grab();
    private Drop dropState = new Drop();
    private Interact interactState = new Interact();

    private HandState currentState;

    public Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeState(HandStates.Grab);
    }

    void Update()
    {
        currentState.Update(this);
    }


    public void ChangeState(HandStates state)
    {
        if (currentState != null)
            currentState.Exit(this);

        switch (state)
        {
            case HandStates.Grab:
                currentState = interactState;
                break;
            case HandStates.Drop:
                currentState = dropState;
                break;
            case HandStates.Interact:
                currentState = grabState;
                break;

        }
        currentState.Enter(this);
    }

}


public abstract class HandState
{
    public abstract void Enter(FSMExample player);
    public abstract void Exit(FSMExample player);
    public abstract void Update(FSMExample player);
}


public class Interact : HandState
{
    public override void Enter(FSMExample player)
    {

    }

    public override void Exit(FSMExample player)
    {

    }

    public override void Update(FSMExample player)
    {

    }
}

public class Drop : HandState
{
    public override void Enter(FSMExample player)
    {

    }

    public override void Exit(FSMExample player)
    {

    }

    public override void Update(FSMExample player)
    {

    }
}

public class Grab : HandState
{
    public override void Enter(FSMExample player)
    {

    }

    public override void Exit(FSMExample player)
    {

    }

    public override void Update(FSMExample player)
    {

    }
}


public enum HandStates
{
    Interact,
    Grab,
    Drop,
};*/