using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _targetDistance = 4f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private Transform _camTransform;

    private Animator _animator;
    private GameObject[] _enemiesInWorld;
    private GameObject _nearestEnemy;
    private GameObject _targetedEnemy;
    private Vector3 _rayRotation;
    private Vector3 _direction;
    private RaycastHit _rayHit;
    private Player _player;
    public GameObject NearestEnemy
    {
        get { return _nearestEnemy; }
    }

    public GameObject TargetedEnemy
    {
        get { return _targetedEnemy; }
    }

    public bool EnemyInRange { get; private set; }

    private float _enemyDistance;
    private bool _targetedWithRay;
    private bool _hit;
    private float _turnSmoothVelocity;
    private Vector3 _turnTowardDirection;
    private Quaternion _lookRotation;
    private DodgeManeuver _dodgeManeuver;

    public GameObject Enemy { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        _player = GetComponent<Player>();
        _dodgeManeuver = GetComponent<DodgeManeuver>();
        _animator = GetComponentInChildren<Animator>();
        _enemiesInWorld = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        if (_targetedEnemy != null)
        {
            EnemyInRange = Vector3.Distance(transform.position, _targetedEnemy.transform.position) <= _targetDistance;
            Debug.DrawLine(transform.position, _targetedEnemy.transform.position, Color.yellow);
        }

        StoreEnemiesInArray();
        FindNearestEnemy();
        CreateEnemyDetectingRay();

        Enemy = _nearestEnemy;
        
        if (_targetedEnemy != null)
            Debug.DrawLine(transform.position, _nearestEnemy.transform.position, Color.red);
    }

    private void FixedUpdate()
    {
        ToggleTargetedEnemy();
        TurnTowardEnemy();
    }

    private void StoreEnemiesInArray()
    {
        _enemiesInWorld = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void CreateEnemyDetectingRay()
    {
        float horizontal = Input.GetAxis($"Horizontal {_player.PlayerNumber}");
        float vertical = Input.GetAxis($"Vertical {_player.PlayerNumber}");
        _direction = new Vector3(horizontal, 0f, vertical);


        float targetAngle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
        _rayRotation = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    private void ToggleTargetedEnemy()
    {
        if (_targetedWithRay == false)
            if (_nearestEnemy != null)
                TargetNearestEnemy();

        if (_direction.magnitude >= .1)
            TargetEnemyWithRayCast();
        else
            TargetNearestEnemy();

        if (_targetedEnemy != null)
            if (Vector3.Distance(transform.position, _targetedEnemy.transform.position) > _targetDistance)
                _targetedEnemy = null;

        if (_targetedEnemy == null)
            _targetedWithRay = false;
    }

    void TargetNearestEnemy()
    {
        var yRange = Mathf.Abs(_nearestEnemy.transform.position.y - transform.position.y);
        var yIsInRange = yRange >= 0 && yRange <= 1;
        if (yIsInRange && Vector3.Distance(transform.position, _nearestEnemy.transform.position) <= _targetDistance)
            _targetedEnemy = _nearestEnemy;
        else if(!yIsInRange)
            _targetedEnemy = null;
    }

    void TargetEnemyWithRayCast()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
            _rayRotation, out _rayHit, 3f, _layerMask))
        {
            var enemy = _rayHit.collider.gameObject;
            var yRange = Mathf.Abs(_nearestEnemy.transform.position.y - transform.position.y);

            var yIsInRange = yRange >= 0 && yRange <= 1;
            if (yIsInRange && Vector3.Distance(transform.position, enemy.transform.position) <= _targetDistance)
            {
                _targetedEnemy = _rayHit.collider.gameObject;
                _targetedWithRay = true;
            }
            else if(!yIsInRange)
            {
                _targetedEnemy = null;
                _targetedWithRay = false;
            }
        }

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
            _rayRotation * 3, Color.blue);
    }

    void TurnTowardEnemy()
    {
        if (_animator.GetBool("Attacking"))
        {
            if (_targetedEnemy != null)
            {
                _turnTowardDirection = (_targetedEnemy.transform.position - transform.position).normalized;
                _lookRotation = Quaternion.LookRotation(new Vector3(_turnTowardDirection.x, 0, _turnTowardDirection.z));
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }


    void FindNearestEnemy()
    {
        var nearestDistance = float.MaxValue;
        foreach (GameObject enemy in _enemiesInWorld)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                _nearestEnemy = enemy;
            }
        }
    }
}