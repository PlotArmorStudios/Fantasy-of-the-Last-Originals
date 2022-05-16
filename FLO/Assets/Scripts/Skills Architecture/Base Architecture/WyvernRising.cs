using InventoryScripts;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu]
    public class WyvernRising : Skill
    {
        public override void Use()
        {
            Debug.Log("use wyvern rising");
            SkillController.GetComponent<Animator>().CrossFade("Wyvern Rising", 0f, 0);
            SkillController.GetComponent<Inventory>().ActiveWeapon.UnSheath();
            SkillController.GetComponent<CombatManager>().ReceiveInput();
        }

        public override void BeginCooldown()
        {
            Debug.Log("Begin cooldown");
        }
    }
}