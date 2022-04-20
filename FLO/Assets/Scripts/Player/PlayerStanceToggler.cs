using UnityEngine;

public class PlayerStanceToggler : StanceToggler
{
    public PlayerStance Stance { get; set; }

    protected override void Start()
    {
        ChangeStance(PlayerStance.Stance1, 1);
    }

    protected override void Update()
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

    protected void ChangeStance(PlayerStance stance, int stanceIndex)
    {
        Stance = stance;
        CurrentStance = stanceIndex;
        StanceChanged = true;
        TriggerChangeStance(stanceIndex);
    }
}