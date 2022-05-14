using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class UISkillTree : MonoBehaviour
    {
        [SerializeField] private List<UISkillSlot> _skills;
        public event Action OnSelectionChanged;
    }
}