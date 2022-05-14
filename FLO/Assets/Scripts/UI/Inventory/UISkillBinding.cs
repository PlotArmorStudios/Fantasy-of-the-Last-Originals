using Skills;
using UnityEngine;

public class UISkillBinding : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        GetComponent<UISkillsPanel>().Bind(_player.GetComponent<SkillInventory>());
    }
}