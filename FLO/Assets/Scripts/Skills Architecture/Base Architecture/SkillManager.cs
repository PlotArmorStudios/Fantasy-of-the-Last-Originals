using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class SkillManager : MonoBehaviour
    {
        private SkillInventory _skillInventory;

        private void Start()
        {
            _skillInventory = GetComponent<SkillInventory>();
        }

        private void Update()
        {
            foreach (var skill in _skillInventory.ActiveSkills)
            {
                if (skill != null)
                    skill.Tick();
            }
        }
    }
}