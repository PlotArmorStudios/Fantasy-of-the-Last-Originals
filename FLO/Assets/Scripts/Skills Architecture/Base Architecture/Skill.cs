using System;
using UnityEngine;

namespace Skills
{
    [System.Serializable]
    public class Skill : ScriptableObject
    {
        public string SkillName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float CoolDownTime;
        [SerializeField] private float ActiveTime;
        
        public int TempIndex;
        public Sprite Icon => _icon;
        
        public bool WasPickedUp { get; set; }
        public bool WasEquipped { get; set; }

        public bool SkillUsed { get; private set; }

        private SkillState _skillState;

        //initiate these
        public Player SkillController { get; set; }
        public UISkillSlot CurrentSlot { get; set; }
        private float _currentActiveTime;
        private float _currentCooldownTime;

        public bool InputReceiver { get; set; }
        
        public virtual void Use()
        {
            SkillUsed = true;
        }

        public virtual void BeginCooldown()
        {
        }
        
        public virtual void Tick()
        {
#if DebugSkill
            Debug.Log($"Ticking skill {TempIndex}");
#endif
            ReceiveInput();
        }

        private void ReceiveInput()
        {
            switch (_skillState)
            {
                case SkillState.Ready:
                    CurrentSlot.TimeOverlay.gameObject.SetActive(false);
                    if (InputReceiver)
                    {
                        Use();
                        _skillState = SkillState.Active;
                        _currentActiveTime = ActiveTime;
                    }

                    break;
                case SkillState.Active:
                    if (_currentActiveTime > 0)
                    {
                        CurrentSlot.TimerImage.fillAmount = 1;
                        _currentActiveTime -= Time.deltaTime;
                    }
                    else
                    {
                        BeginCooldown();
                        _skillState = SkillState.Cooldown;
                        _currentCooldownTime = CoolDownTime;
                        CurrentSlot.TimeOverlay.gameObject.SetActive(true);
                        CurrentSlot.TimeOverlay.text = _currentCooldownTime.ToString("0");
                    }

                    break;
                case SkillState.Cooldown:
                    if (_currentCooldownTime > 0)
                    {
                        _currentCooldownTime -= Time.deltaTime;
                        CurrentSlot.TimerImage.fillAmount = _currentCooldownTime / CoolDownTime;
                        CurrentSlot.TimeOverlay.text = _currentCooldownTime.ToString("0");
                    }
                    else
                    {
                        _skillState = SkillState.Ready;
                    }

                    break;
            }
        }
    }
}