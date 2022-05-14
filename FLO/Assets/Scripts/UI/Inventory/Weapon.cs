using System;
using InventoryScripts;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : Item, IHaveAHitBox
{
    [SerializeField] private HitBox _hitbox;

    private Inventory _playerInventory;
    private PlayerStanceToggler _stanceToggler;

    public HitBox HitBox => _hitbox;

    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _playerInventory = _player.GetComponent<Inventory>();
        _stanceToggler = _player.GetComponent<PlayerStanceToggler>();
    }

    void Update()
    {
        if (_stanceToggler.Stance == PlayerStance.Stance4 && WasEquipped)
        {
            if (_playerInventory == null)
                return;

            if (_player.Animator.GetBool("Attacking"))
            {
                UnSheath();
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

    public void UnSheath()
    {
        _playerInventory.UnSheathWeapon(this);
    }
}