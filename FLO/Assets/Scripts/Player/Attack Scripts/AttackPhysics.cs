using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPhysics : MonoBehaviour
{
    [SerializeField] float _stoppingDistance = 4f;
    [SerializeField] float _attackSlideDistance = 5f;
    [SerializeField] private StopMovementOnCollision _stopMovement;

    GameObject m_player;

    CharacterController m_controller;
    Rigidbody m_rb;
    private AutoTargetEnemy _enemyTargeter;

    public bool StopMovement;
    public GameObject Enemy => _enemyTargeter.TargetedEnemy;

    public bool EnemyIsTooClose => Vector3.Distance(transform.position, Enemy.transform.position) <
                                   _stoppingDistance;

    bool m_sliding = false;

    private WaitForSeconds _slideDuration;
    private float _newDuration;

    private void Start()
    {
        _slideDuration = new WaitForSeconds(_newDuration);
        m_rb = GetComponent<Rigidbody>();
        m_controller = GetComponent<CharacterController>();
        _enemyTargeter = GetComponent<AutoTargetEnemy>();
    }

    private void Update()
    {
        if (_stopMovement.isActiveAndEnabled)
            StopMovement = _stopMovement.ShouldStop;
        else
            StopMovement = false;
        
        if (m_sliding && !StopMovement) //applies forward movement to character controller when m_sliding is true
        {
            AttackSlide(AttackSlideDistance(_attackSlideDistance));
        }
    }

    void AttackSlide(float distance)
    {
        transform.position += transform.forward * Time.deltaTime * distance;
    }

    public float AttackSlideDistance(float distance)
    {
        return distance;
    }

    void ReadAttackDistanceValue()
    {
        _attackSlideDistance = AttackSlideDistance(_attackSlideDistance);
    }

    IEnumerator SlideDuration(float slideDuration)
    {
        if (StopMovement)
            yield break;

        m_sliding = true;
        _newDuration = slideDuration;
        yield return new WaitForSeconds(slideDuration);
        m_sliding = false;
    }
}