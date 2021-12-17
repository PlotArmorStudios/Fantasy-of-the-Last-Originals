using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerDownHandler,
    IEndDragHandler,
    IDragHandler, 
    IPointerEnterHandler,
    IPointerExitHandler
{
    public event Action<UIInventorySlot> OnSlotClicked;

    [SerializeField] private UIInventoryPanel _linkedPanel;
    [SerializeField] private Image _image;
    [SerializeField] private Image _selectedImage;
    [SerializeField] private Image _focusedImage;
    [SerializeField] protected int _sortIndex;
    
    public SlotType SlotType;

    public Item Item { get; private set; }
    public bool IsEmpty => Item == null;
    public Sprite Icon => _image.sprite;
    public bool IconImageEnabled => _image.enabled;
    public int SortIndex => _sortIndex;

    protected Inventory Inventory => _linkedPanel.Inventory;


    private void OnValidate()
    {
        int hotKeyNumber = transform.GetSiblingIndex() + 1;
        _sortIndex = hotKeyNumber;
        gameObject.name = "Inventory Slot " + hotKeyNumber;
    }
    public virtual void SetItem(Item item)
    {
        Item = item;
        _image.sprite =
            item != null ? item.Icon : null; //if the item isn't null, set to item.Icon. If it is null, set to null.
        _image.enabled = item != null;
    }

    public void Clear()
    {
        Item = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(this);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
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

    public void OnDrag(PointerEventData eventData)
    {
    }
    
    public void BecomeSelected()
    {
        if (_selectedImage != null)
            _selectedImage.enabled = true;
    }

    public void BecomeUnSelected()
    {
        if (_selectedImage != null)
            _selectedImage.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_focusedImage)
            _focusedImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_focusedImage)
            _focusedImage.enabled = false;
    }
    private void OnDisable()
    {
        _selectedImage.enabled = false;
        _focusedImage.enabled = false;
    }
}
