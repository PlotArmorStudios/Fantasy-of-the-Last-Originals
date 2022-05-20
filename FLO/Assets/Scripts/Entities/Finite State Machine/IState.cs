using System.Collections;
using UnityEngine;

public interface IState
{
    void Tick();
    void FixedTick();
    public void OnEnter();
    public void OnExit();
}