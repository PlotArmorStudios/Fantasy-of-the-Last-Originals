using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Skills
{
    public class UISkillSlot : MonoBehaviour, IPointerDownHandler,
        IEndDragHandler,
        IDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler, ISelectableUI
    {
        public event Action<UISkillSlot> OnSlotClicked;

        [SerializeField] private UISkillsPanel _linkedPanel;
        [SerializeField] private Image _image;
        [SerializeField] private Image _selectedImage;
        [SerializeField] private Image _focusedImage;
        [SerializeField] protected int _sortIndex;
        [SerializeField] private TMP_Text _inputText;

        [field: SerializeField] public Image TimerImage { get; private set; }
        [field: SerializeField] public TMP_Text TimeOverlay { get; private set; }
        [field: SerializeField] public SlotType SlotType { get; private set; }

        public bool IsEmpty { get; set; }
        public Skill Skill { get; private set; }

        public Sprite Icon => _image.sprite;
        public bool IconImageEnabled => _image.enabled;
        public int SortIndex => _sortIndex;

        protected SkillInventory Skills => _linkedPanel.Skills;

        private void OnValidate()
        {
            int hotKeyNumber = transform.GetSiblingIndex() + 1;
            _sortIndex = hotKeyNumber;
            gameObject.name = "Skill Slot " + hotKeyNumber;
            _inputText.text = (hotKeyNumber).ToString();
            if (_inputText.text == "10")
                _inputText.text = "0";
        }

        public virtual void SetSkill(Skill skill)
        {
            Skill = skill;

            if (skill != null)
                skill.CurrentSlot = this;

            _image.sprite =
                skill != null
                    ? skill.Icon
                    : null; //if the skill isn't null, set to item.Icon. If it is null, set to null.
            _image.enabled = skill != null;
        }

        public void Clear()
        {
            Skill = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSlotClicked?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
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
}