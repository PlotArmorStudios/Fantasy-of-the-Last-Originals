using UnityEngine;

public class UIEnemyHealthBar : UIHealthBar
{
    [SerializeField] private Entity _enemy;

    private void Start()
    {
        _health = _enemy.GetComponent<EnemyHealth>();
        _health.OnHealthUpdate += HandleUpdateHealth;
        HandleUpdateHealth();
    }
}