using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : Item, IHaveAHitBox
{
    [SerializeField] private HitBox _hitbox;
    
    private Inventory _playerInventory;

    public HitBox HitBox => _hitbox;

    private void Start()
    {
        _playerInventory = _player.GetComponent<Inventory>();
    }

    void Update()
    {
        if (_player.Stance == PlayerStance.Stance4 && WasEquipped)
        {
            if (_playerInventory == null)
                return;
            
            if (_player.Animator.GetBool("Attacking"))
            {
                _playerInventory.UnSheathWeapon(this);
            }
            else
            {
                _playerInventory.SheathWeapon(this);
            }
        }
    }

    public void SwitchHitBoxActiveState()
    {
        _hitbox.gameObject.SetActive(!_hitbox.gameObject.activeSelf);
    }
}