using System;
using UnityEngine;

public class StanceToggler : MonoBehaviour
{
    public event Action<int> OnStanceChanged;
    private Animator _animator;
    public PlayerStance Stance { get; set; }
    public bool StanceChanged { get; set; }
    public int CurrentStance { get; set; }
    public event Action OnChangeStance;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        ChangeStance(PlayerStance.Stance1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeStance(PlayerStance.Stance1, 1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeStance(PlayerStance.Stance2, 2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeStance(PlayerStance.Stance3, 3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeStance(PlayerStance.Stance4, 4);
    }

    void ChangeStance(PlayerStance stance, int stanceIndex)
    {
        Stance = stance;
        CurrentStance = stanceIndex;
        StanceChanged = true;
        OnChangeStance?.Invoke();
        OnStanceChanged?.Invoke(stanceIndex);
    }

}