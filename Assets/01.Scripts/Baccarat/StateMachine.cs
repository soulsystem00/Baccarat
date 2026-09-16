using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    None,
    Start,
    Betting,
    Show,
    Result,
}

public class StateMachine : MonoSingleton<StateMachine>
{
    [SerializeField] private StateType currentState = StateType.None;

    private readonly Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    protected override void Awake()
    {
        base.Awake();

        states.Add(StateType.None, null);
        states.Add(StateType.Start, new StartState());
        states.Add(StateType.Betting, new BettingState());
        states.Add(StateType.Show, new ShowState());
        states.Add(StateType.Result, new ResultState());
    }

    private void Start()
    {
        ChangeState(currentState);
    }

    private void Update()
    {
        states[currentState]?.OnUpdate();
    }

    public void ChangeState(StateType newState)
    {
        if (newState == currentState)
            return;

        states[currentState]?.OnExit();
        currentState = newState;
        states[currentState]?.OnEnter();
    }

    public StateType GetCurrentState()
    {
        return currentState;
    }
}
