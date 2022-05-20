using System;
using UnityEngine;

namespace GoblinStateMachine
{
    public class AirLock : IState
    {
        private KnockBackHandler _stunHandler;
        private readonly EntityStateMachine _stateMachine;

        public AirLock(FiniteStateMachine stateMachine)
        {
            _stateMachine = stateMachine as EntityStateMachine;
            _stunHandler = _stateMachine.StunHandler;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _stunHandler.GetComponent<CalculateKnockBack>().enabled = false;
            _stunHandler.GetComponent<Rigidbody>().useGravity = false;
        }

        public void OnExit()
        {
            _stunHandler.GetComponent<CalculateKnockBack>().enabled = true;
            _stunHandler.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}