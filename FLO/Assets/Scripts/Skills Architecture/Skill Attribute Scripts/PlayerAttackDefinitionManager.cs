using System;
using System.Linq;
using InventoryScripts;
using Unity.Mathematics;

public class PlayerAttackDefinitionManager : AttackDefinitionManager
{
    private Inventory _inventory;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
    }

    public void SetActiveWeaponHitBox()
    {
        if (_inventory.ActiveWeapon)
            _inventory.ActiveWeapon.SwitchHitBoxActiveState();
    }

    public void PlayInteractiveVFX()
    {
        if (ActiveObject)
            ActiveObject.GetComponent<IEffect>().PlayEffect();
    }
}