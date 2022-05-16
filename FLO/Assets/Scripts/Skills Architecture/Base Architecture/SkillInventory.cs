using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Skills
{
    public enum SkillState
    {
        Ready,
        Active,
        Cooldown
    }

    public class SkillInventory : MonoBehaviour
    {
        public event Action<int> OnSkillChanged;
        public event Action<ISkillObject> OnSkillObjectAttained;
        public event Action<Skill> OnSkillAttained;

        [SerializeField] private List<Skill> _skillsToEquip;
        
        public const int DEFAULT_SKILLINVENTORY_SIZE = 10;

        private Skill[] _activeSkills = new Skill[DEFAULT_SKILLINVENTORY_SIZE];
        public List<Skill> ActiveSkills => _activeSkills.ToList();
        public UISkillSlot[] Slots { get; set; }

        public int Count => _activeSkills.Count(t => t != null);

        public Controller Controller { get; set; }
        public Player Player { get; set; }

        private void Start()
        {
            Player = GetComponent<Player>();
            for (int i = 0; i < _skillsToEquip.Count; i++)
            {
                ManualSkillEquip(_skillsToEquip[i], i);
            }
        }

        private void Update()
        {
            OnTryUse();
        }

        private void OnTryUse()
        {
            if (_activeSkills[0])
                _activeSkills[0].InputReceiver = Controller.SpecialAttack1;
            if (_activeSkills[1])
                _activeSkills[1].InputReceiver = Controller.SpecialAttack2;
            if (_activeSkills[2])
                _activeSkills[2].InputReceiver = Controller.SpecialAttack3;
            if (_activeSkills[3])
                _activeSkills[3].InputReceiver = Controller.SpecialAttack4;
            if (_activeSkills[4])
                _activeSkills[4].InputReceiver = Controller.SpecialAttack5;
            if (_activeSkills[5])
                _activeSkills[5].InputReceiver = Controller.SpecialAttack6;
            if (_activeSkills[6])
                _activeSkills[6].InputReceiver = Controller.SpecialAttack7;
            if (_activeSkills[7])
                _activeSkills[7].InputReceiver = Controller.SpecialAttack8;
            if (_activeSkills[8])
                _activeSkills[8].InputReceiver = Controller.SpecialAttack9;
            if (_activeSkills[9])
                _activeSkills[9].InputReceiver = Controller.SpecialAttack10;
        }

        public void PickUp(ISkillObject ISkill, int? slot = null)
        {
            var skill = ISkill as SkillObject;
#if DebugSkillInventory
            Debug.Log($"Picked Up {skill.gameObject.name}");
#endif

            if (slot.HasValue == false) slot = FindFirstMatchingSlotType(ISkill);
            if (slot.HasValue == false) return;

            _activeSkills[slot.Value] = ISkill.Skill;
            OnSkillObjectAttained?.Invoke(ISkill);
            skill.WasPickedUp = true;

            StartCoroutine(SetInactive(ISkill));
        }

        public void ManualSkillEquip(Skill skill, int? slot = null)
        {
            _activeSkills[slot.Value] = skill;
#if DebugSkillInventory
            Debug.Log($"Equipped {skill.SkillName}");
            Debug.Log($"Skill slot value is {slot.Value}, " + _activeSkills[slot.Value]);
#endif
            skill.WasEquipped = true;
            skill.SkillController = Player;
            OnSkillAttained?.Invoke(skill);
            OnSkillChanged?.Invoke(slot.Value);
        }

        private bool InventoryIsFull()
        {
            if (_activeSkills.Length == Slots.Length) return true;

            return false;
        }

        private int? FindFirstMatchingSlotType(ISkillObject ISkill)
        {
            for (int i = 0; i < _activeSkills.Length; i++)
            {
                if (_activeSkills[i] == null)
                {
                    return i;
                }
            }

            return null;
        }

        private IEnumerator SetInactive(ISkillObject ISkill)
        {
            var skill = ISkill as SkillObject;
            yield return new WaitForSeconds(.01f);

            skill.gameObject.SetActive(false);
        }

        public void Equip(ISkillObject iSkill)
        {
            var skill = iSkill as SkillObject;

            skill.transform.localPosition = Vector3.zero;
            skill.transform.localRotation = Quaternion.identity;
            skill.WasEquipped = true;
            Debug.Log($"Skill is now in skill tree");
            //ActiveSkills.Add(iSkill.Skill);
        }

        public void Move(int sourceSlot, int destinationSlot)
        {
            var destinationISkill = _activeSkills[destinationSlot];
            _activeSkills[destinationSlot] = _activeSkills[sourceSlot];
            _activeSkills[sourceSlot] = destinationISkill;

            OnSkillChanged?.Invoke(destinationSlot);
            OnSkillChanged?.Invoke(sourceSlot);
        }

        public Skill GetSkillInSlot(int slot)
        {
            return _activeSkills[slot];
        }
    }
}