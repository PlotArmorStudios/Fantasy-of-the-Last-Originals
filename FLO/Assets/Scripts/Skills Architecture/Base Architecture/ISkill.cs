using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public interface ISkill
    {
        public Sprite Icon {get;}
        public void Use(GameObject parent);
        public void Tick();
    }
}