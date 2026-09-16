using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingState : IState
{
    private const float BETTING_TIME = 30f;
    private float timer = 0f;

    public void OnEnter()
    {
        GameManager.Instance.BettingStart();
    }

    public void OnExit()
    {
        timer = 0f;
        GameManager.Instance.BettingEnd();
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        GameManager.Instance.UpdateBettingTimer(Mathf.Max(0f, BETTING_TIME - timer));
        if (timer > BETTING_TIME)
        {
            StateMachine.Instance.ChangeState(StateType.Show);
        }

    }
}
