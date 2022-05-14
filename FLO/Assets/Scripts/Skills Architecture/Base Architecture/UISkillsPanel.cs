using System;
using System.Linq;
using InventoryScripts;
using UnityEngine;

namespace Skills
{
    internal class UISkillsPanel : MonoBehaviour
    {
        public SkillInventory Skills { get; set; }

        public event Action OnSelectionChanged;

        [ContextMenu("Swap 01")]
        public void Swap01() => _skillInventory.Move(0, 1);

        private SkillInventory _skillInventory;
        public UISkillSlot[] Slots;
        public SkillInventory SkillInventory => _skillInventory;
        public int SlotCount => Slots.Length;

        public UISkillSlot Selected { get; set; }

        private void Awake()
        {
            Slots = FindObjectsOfType<UISkillSlot>()
                .OrderBy(t => t.SortIndex)
                .ThenBy(t => t.name)
                .ToArray();

            RegisterSlotsForClickCallback();
        }

        private void RegisterSlotsForClickCallback()
        {
            foreach (var slot in Slots)
            {
                slot.OnSlotClicked += HandleSlotClicked;
            }
        }

        private void HandleSlotClicked(UISkillSlot slot)
        {
            if (Selected != null)
            {
                if (SlotCanHoldSkill(slot, Selected.Skill))
                {
                    Swap(slot);
                    Selected.BecomeUnSelected();
                    Selected = null;
                }
            }
            else if (slot.IsEmpty == false)
            {
                Selected = slot;
                Selected.BecomeSelected();
            }

            OnSelectionChanged?.Invoke();
        }

        private bool SlotCanHoldSkill(UISkillSlot slot, Skill selectedSkill)
        {
            return slot.SlotType == SlotType.Skill;
        }

        private void Swap(UISkillSlot slot)
        {
            var slotIndex1 = GetSlotIndex(Selected);
            _skillInventory.Move(GetSlotIndex(Selected), GetSlotIndex(slot));
        }

        private int GetSlotIndex(UISkillSlot selected)
        {
            for (int i = 0; i < SlotCount; i++)
            {
                if (Slots[i] == selected)
                    return i;
            }

            return -1;
        }

        public void Bind(SkillInventory skillInventory)
        {
            if (_skillInventory != null)
            {
                _skillInventory.OnSkillObjectAttained -= HandleSkillObjectPickedUp;
                _skillInventory.OnSkillChanged -= HandleSkillChanged;
            }

            _skillInventory = skillInventory;

            if (_skillInventory != null)
            {
                _skillInventory.OnSkillObjectAttained += HandleSkillObjectPickedUp;
                _skillInventory.OnSkillChanged += HandleSkillChanged;
                RefreshSlots();
            }
            else
            {
                ClearSlots();
            }
        }

        private void HandleSkillChanged(int slotNumber)
        {
            Slots[slotNumber].SetSkill(_skillInventory.GetSkillInSlot(slotNumber));
        }

        private void ClearSlots()
        {
            foreach (var slot in Slots) slot.Clear();
        }

        private void RefreshSlots()
        {
            for (var i = 0; i < Slots.Length; i++)
            {
                var slot = Slots[i];

                if (_skillInventory.ActiveSkills.Count > i)
                    slot.SetSkill(_skillInventory.ActiveSkills[i]);
                else 
                    slot.Clear();
            }
        }

        private void HandleSkillObjectPickedUp(ISkillObject iSkill)
        {
            RefreshSlots();
        }
    }
}