using UnityEngine;
using UnityEngine.UI;

public abstract class UIHealthBar : MonoBehaviour
{
    [SerializeField] protected Image _healthBarImage;
    protected HealthLogic _health;

    protected void HandleUpdateHealth()
    {
        _healthBarImage.fillAmount = _health.CurrentHealthValue / _health.MAXHealthValue;
    }
}