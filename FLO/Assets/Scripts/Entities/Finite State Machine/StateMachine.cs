using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine //Stores a list of states that can be added or removed from state machine.
{
    private Dictionary<IState, List<StateTransition>> _stateTransitions = new Dictionary<IState, List<StateTransition>>();
    private List<StateTransition> _anyStateTransitions = new List<StateTransition>();
    private IState _currentState;

    //Add states to the state machine
 
    //Add state transitions to the state machine
    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (_stateTransitions.ContainsKey(from) == false)
            _stateTransitions[from] = new List<StateTransition>();

        var stateTransition = new StateTransition(from, to, condition);
        _stateTransitions[from].Add(stateTransition);
    }

    public void AddAnyTransition(IState to, Func<bool> condition)
    {
        var stateTransition = new StateTransition(null, to, condition);
        _anyStateTransitions.Add(stateTransition);
    }
    public void SetState(IState state)
    {
        if (_currentState == state)
            return;
        
        _currentState?.OnExit();
        
        _currentState = state;
        
        Debug.Log($"Changed to state {state}");
        
        _currentState.OnEnter();
    }

    public void Tick()
    {
        StateTransition transition = CheckForTransition();
        if(transition != null) SetState(transition.To);
        _currentState.Tick();
    }

    private StateTransition CheckForTransition()
    {
        foreach (var transition in _anyStateTransitions)
        {
            if (transition.Condition())return transition;
        }
        
        if (_stateTransitions.ContainsKey(_currentState))
        {
            foreach (var transition in _stateTransitions[_currentState])
            {
                if (transition.From == _currentState && transition.Condition()) return transition;
            }
        }

        return null;
    }
}