using UnityEngine;

public abstract class HandleFallingAnimation : MonoBehaviour
{
    [SerializeField] float _sphereYOffSet;
    [SerializeField] float _landingRadius;

    protected Animator _animator;
    protected Rigidbody _rb;
    protected LayerMask _layerMask;
    protected KnockBackHandler RigidBodyStunHandler;
    protected GroundCheck _groundCheck;

    public Animator Animator => _animator;

    // Start is called before the first frame update
    void Start()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _layerMask = LayerMask.GetMask("Ground");
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        RigidBodyStunHandler = GetComponent<KnockBackHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Grounded", _groundCheck.IsGrounded);
        if (_groundCheck.IsGrounded)
        {
            _animator.SetFloat("Falling", 0);
            _animator.SetFloat("Raising", 0);
        }
        else
        {
            _animator.SetFloat("Falling", _rb.velocity.y);
            _animator.SetFloat("Raising", _rb.velocity.y);
        }

        if (!_groundCheck.IsGrounded && _rb.velocity.y < .1)
        {
            var hit = Physics.CheckSphere(
                new Vector3(transform.position.x, transform.position.y + _sphereYOffSet, transform.position.z),
                _landingRadius, _layerMask);
            if (hit)
                PlayFallAnimation();
        }
        else
            StopFallAnimation();
    }

    protected abstract void PlayFallAnimation();
    protected abstract void StopFallAnimation();
}