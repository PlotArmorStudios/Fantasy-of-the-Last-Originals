using UnityEngine;

public class HitBoxLifeTime : MonoBehaviour
{
    [SerializeField] private GameObject _hitBox;
    [SerializeField] private float _startLifeTime = .5f;
    
    private float _currentLifeTime;

    private void Start()
    {
        _currentLifeTime = _startLifeTime;
    }

    private void Update()
    {
        _currentLifeTime -= Time.time;
        
        if(_currentLifeTime == 0)
            _hitBox.SetActive(false);
    }
}