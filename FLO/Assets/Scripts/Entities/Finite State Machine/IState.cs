using System.Collections;
using UnityEngine;

public interface IState
{
    void Tick();
    void FixedTick();
    void OnEnter();
    void OnExit();
}