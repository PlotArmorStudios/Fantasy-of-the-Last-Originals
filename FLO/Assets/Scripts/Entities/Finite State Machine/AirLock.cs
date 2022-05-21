using System;
using UnityEngine;

namespace GoblinStateMachine
{
    [System.Serializable]
    public class AirLock : IState
    {
        [SerializeField] private float _maxTimeInAirLock = 2;
        
        private KnockBackHandler _stunHandler;
        private readonly EntityStateMachine _stateMachine;
        private float _timeInAirLock;

        public AirLock(FiniteStateMachine stateMachine)
        {
            _stateMachine = stateMachine as EntityStateMachine;
            _stunHandler = _stateMachine.StunHandler;
        }

        public void Tick()
        {
            _timeInAirLock += Time.deltaTime;
            
            if (_timeInAirLock >= _maxTimeInAirLock)
                _stunHandler.AirLocked = false;
        }

        public void FixedTick()
        {
            
        }

        public void OnEnter()
        {
            _stunHandler.GetComponent<Rigidbody>().useGravity = false;
        }

        public void OnExit()
        {
            _stunHandler.GetComponent<Rigidbody>().useGravity = true;
            _timeInAirLock = 0;
        }
    }
}