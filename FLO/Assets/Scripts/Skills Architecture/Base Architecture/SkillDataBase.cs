using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Skills
{
    public class SkillDataBase : MonoBehaviour
    {
        [SerializeField] private List<Skill> _skills;

        private WyvernRising _wyvernRising;
        
        private void Start()
        {
            InitializeSkills();
        }

        private void InitializeSkills()
        {
            _wyvernRising = new WyvernRising();
        }

        private void AddToList()
        {
            _skills.Add(_wyvernRising);
        }
    }

    public static class SkillFactory
    {
        private static Dictionary<string, Type> _skillsByName;
        private static bool IsInitialized => _skillsByName != null;

        private static void InitializeFactory()
        {
            if (IsInitialized)
                return;
            var skillTypes = Assembly.GetAssembly(typeof(Skill)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Skill)));
            
            // Dictionary for finding these by name later (could be an enum/id instead of string)
            _skillsByName = new Dictionary<string, Type>();
            
            foreach (var skillType in skillTypes)
            {
                var tempSkill = Activator.CreateInstance(skillType) as Skill;
                _skillsByName.Add(tempSkill.SkillName, skillType);
            }
        }

        public static Skill GetSkill(string skillType)
        {
            InitializeFactory();
            if (_skillsByName.ContainsKey(skillType))
            {
                Type type = _skillsByName[skillType];
                var skill = Activator.CreateInstance(type) as Skill;
                return skill;
            }

            return null;
        }

        internal static IEnumerable<string> GetSkillNames()
        {
            UnityEngine.Debug.Log("Test");
            InitializeFactory();
            return _skillsByName.Keys;
        }
    }
}