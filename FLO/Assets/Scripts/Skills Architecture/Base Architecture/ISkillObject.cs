using UnityEngine;

namespace Skills
{
    public interface ISkillObject
    {
        Skill Skill { get; set; }
        Sprite Icon { get; set; }
    }
}