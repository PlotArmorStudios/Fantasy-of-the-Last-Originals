using System;
using System.Collections;
using InventoryScripts;
using UnityEngine;

public class Item : MonoBehaviour
{
    public event Action OnPickedUp;

    [SerializeField] private Sprite _icon;
    [SerializeField] private SlotType _slotType;

    public Sprite Icon => _icon;
    public SlotType SlotType => _slotType;
    public bool WasPickedUp { get; set; }
    public bool WasEquipped { get; set; }
    public ParticleSystem ActivateParticle;

    void OnTriggerEnter(Collider other)
    {
        if (WasPickedUp)
            return;

        var inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.PickUp(this);
            OnPickedUp?.Invoke();
        }
    }


    void OnValidate()
    {
        var collider = GetComponent<Collider>();
        if (collider != null)
            if (collider.isTrigger == false)
                collider.isTrigger = true;
    }
}