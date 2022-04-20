using System;
using UnityEngine;

public abstract class StanceToggler : MonoBehaviour
{
    public event Action<int> OnStanceChanged;
    public event Action OnChangeStance;
    public bool StanceChanged { get; set; }
    public int CurrentStance { get; set; }

    protected abstract void Start();

    protected abstract void Update();

    protected virtual void ChangeStance(int stanceIndex)
    {
        CurrentStance = stanceIndex;
        StanceChanged = true;
        TriggerChangeStance(stanceIndex);
    }

    protected virtual void TriggerChangeStance(int stanceIndex)
    {
        OnChangeStance?.Invoke();
        OnStanceChanged?.Invoke(stanceIndex);
    }
}