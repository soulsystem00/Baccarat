using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : IState
{
    private const float StartDelay = 1f;
    private float timer = 0f;

    public void OnEnter()
    {
        GameManager.Instance.GameStart();
    }

    public void OnExit()
    {
        timer = 0f;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= StartDelay)
        {
            StateMachine.Instance.ChangeState(StateType.Betting);
        }
    }
}
