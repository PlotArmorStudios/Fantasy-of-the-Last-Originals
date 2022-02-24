using System;
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] Transform _feet;

    private Animator _animator;
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;

    private EntityStateMachine _entity;
    private Character _character;

    private void Start()
    {
        _character = GetComponent<Character>();
        _animator = GetComponent<Animator>();
        _entity = GetComponent<EntityStateMachine>();
    }

    private void Update()
    {
        if (!_isGrounded)
            _character.FallTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();
    }

    public bool UpdateIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_feet.position, 1f, _groundLayerMask);
        return _isGrounded;
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Ground")
        {
            GetComponent<Character>().IsJumping = false;
            _animator.SetBool("Airborne", false);

            if (GetComponent<Character>().FallTime > 0)
                _animator.SetTrigger("Landing");

            _character.FallTime = 0;
            
            if (_entity)
            {
                _entity.Land = true;
                StartCoroutine(ResetLand());
            }
        }
    }

    private IEnumerator ResetLand()
    {
        yield return new WaitForSeconds(.1f);
        _entity.Land = false;
    }
}

public class Character : MonoBehaviour
{
    public bool IsJumping;
    public float FallTime { get; set; }
}