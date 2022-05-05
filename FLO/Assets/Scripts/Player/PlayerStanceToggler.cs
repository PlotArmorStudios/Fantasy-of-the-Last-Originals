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
        if (Input.GetKeyDown(KeyCode.E))
            ChangeStance(PlayerStance.Stance1, 1);
        if (Input.GetKeyDown(KeyCode.R))
            ChangeStance(PlayerStance.Stance2, 2);
        if (Input.GetKeyDown(KeyCode.F))
            ChangeStance(PlayerStance.Stance3, 3);
        if (Input.GetKeyDown(KeyCode.V))
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