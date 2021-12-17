using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWeaponSlot : UIInventorySlot
{
    [SerializeField] private Transform _weaponScabbard;

    private void Start()
    {
        OnSlotClicked += HandleWeaponEquip;
    }

    public override void SetItem(Item item)
    {
        base.SetItem(item);
    }
    private void HandleWeaponEquip(UIInventorySlot obj)
    {
        Debug.Log($"Set weapon with {Item}.");
        Item.gameObject.SetActive(true);
        Item.WasEquipped = true;
        Item.ActivateParticle.Play();
        Inventory.ActiveWeapon = (Weapon)Item;
        Inventory.SheathWeapon(Item);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        var droppedOnSlot = eventData.pointerCurrentRaycast.gameObject?.GetComponentInParent<UIInventorySlot>();

        if (droppedOnSlot != null)
        {
            droppedOnSlot.OnPointerDown(eventData);
        }
        else
        {
            OnPointerDown(eventData);
        }
    }

    private void OnValidate()
    {
        int hotKeyNumber = transform.GetSiblingIndex() + 1;
        _sortIndex = hotKeyNumber;
        gameObject.name = "Weapon Inventory Slot " + hotKeyNumber;
    }
}