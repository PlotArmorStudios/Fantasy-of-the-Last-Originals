using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInput : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseSkill(5);
        }
    }

    private void UseSkill(int skillSlot)
    {   
        Debug.Log($"Use skill {skillSlot}");
        _animator.CrossFade($"Skill Slot {skillSlot}", .25f, 0);
    }
}
