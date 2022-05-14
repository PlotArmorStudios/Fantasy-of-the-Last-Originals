using System;
using UnityEngine;

namespace Skills
{
    public class SkillObject : MonoBehaviour, ISkillObject
    {

        public event Action OnPickedUp;
        
        [SerializeField] private Sprite _icon;
        [SerializeField] private SlotType _slotType;
        public Skill Skill { get; set; }
        public Sprite Icon { get; set; }
        
        public SlotType SlotType => _slotType;
        public bool WasEquipped { get; set; }
        public bool WasPickedUp { get; set; }

        private void Start()
        {
            Icon = _icon;
        }
        
        public ParticleSystem ActivateParticle;

        void OnTriggerEnter(Collider other)
        {
            if (WasPickedUp)
                return;

            var skillInventory = other.GetComponent<SkillInventory>();

            if (skillInventory != null)
            {
                skillInventory.PickUp(this);
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
}