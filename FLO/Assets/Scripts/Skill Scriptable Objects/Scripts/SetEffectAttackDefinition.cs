using UnityEngine;

public class SetEffectAttackDefinition : MonoBehaviour
{
    [SerializeField] private AttackDefinition _attackDefinition;
    
    private AttackDefinitionManager _attackDefinitionManager;
    
    private void Awake()
    {
        _attackDefinitionManager = GetComponentInParent<AttackDefinitionManager>();
        _attackDefinitionManager.SetAttackDefinition(_attackDefinition);
    }
}