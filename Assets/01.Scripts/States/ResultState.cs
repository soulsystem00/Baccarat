using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultState : IState
{
    public void OnEnter()
    {
        GameManager.Instance.ResultStart();
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}
