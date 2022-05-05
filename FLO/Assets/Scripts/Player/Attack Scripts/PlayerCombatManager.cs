using UnityEngine;

public class PlayerCombatManager : CombatManager
{
    public Player Player { get; private set; }

    protected override void Start()
    {
        Player = GetComponent<Player>();
    }

    protected override void HandleInput()
    {
        if (PauseMenu.Active) return;

        Attack();
        ReceiveInput();

        if (Input.GetButtonDown("Light Attack"))
        {
            CanReceiveInput = true;
            InputCount++;
        }
    }

    //When true, player can interrupt current attack animation. Accessed through animation events.
    public void AttackCancelPoint()
    {
        _animator.SetBool("Attacking", false);
    }

    public void AttackLockPoint()
    {
        _animator.SetBool("Attacking", true);
    }
}