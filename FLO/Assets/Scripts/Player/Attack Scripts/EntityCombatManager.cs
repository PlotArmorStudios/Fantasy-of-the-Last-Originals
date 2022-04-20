using UnityEngine;

public class EntityCombatManager : CombatManager
{
    public bool TriggerAttack { get; set; }

    protected override void HandleInput()
    {
        if (PauseMenu.Active) return;

        Attack();
        ReceiveInput();

        if (TriggerAttack)
        {
            CanReceiveInput = true;
            InputCount = Random.Range(1, 4);
            TriggerAttack = false;
        }
    }
}