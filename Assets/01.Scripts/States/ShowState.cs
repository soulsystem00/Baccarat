using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowState : IState
{
    public void OnEnter()
    {
        GameManager.Instance.ShowCard();
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
    }
}
