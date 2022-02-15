using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public enum PlayerID
{
    _P1,
    _P2
}

public enum PlayerStance
{
    Stance1,
    Stance2,
    Stance3,
    Stance4
}

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerID _playerID;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private bool _jump;
    [SerializeField] private bool isGrounded;
    
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _downPull = 2f;
    [SerializeField] private float _turnSmoothTime = 2f;
    [SerializeField] private float _turnSmoothVelocity = 2f;

    [SerializeField] private Camera _camera;
    
    public bool IsGrounded => _groundCheck.IsGrounded;
    public Animator Animator => _animator;
    
    private CameraLogic _cameraLogic;

    private Rigidbody _rb;
    private Animator _animator;
    private CombatManagerScript _combatManager;

    private GroundCheck _groundCheck;
    private float _fallTimer;
    
    private PlayerDash _playerDash;

    public Transform CamTransform;
    public PlayerStance Stance;

    public bool IsJumping;

    private Vector3 _movement;
    private Vector3 _heightMovement;
    private float _horizontalInput;
    private float _verticalInput;

    private PhotonView _view;


    private void OnEnable()
    {
        _combatManager = GetComponent<CombatManagerScript>();
        if (_combatManager.enabled == false)
            _combatManager.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        _playerDash = GetComponent<PlayerDash>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _groundCheck = GetComponent<GroundCheck>();

        if (_camera) _cameraLogic = _camera.GetComponent<CameraLogic>();

        CamTransform = _camera.transform;

        //Multiplayer PhotonView Processes
        if (!_view.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<CinemachineStateDrivenCamera>().gameObject);
        }
    }

    void Update()
    {
        if (!_view.IsMine) return;
        if (PauseMenu.Active) return;

        ReadHorizontalAndVerticalInput();
        ApplyMovementInputToAnimator();
        
        if (PlayerJumpedFromGround()) _jump = true;
    }

    private void FixedUpdate()
    {
        if (!_view.IsMine) return;
        if (PauseMenu.Active) return;

        ApplyGravity();
        HandleJumpAnimation();
        RotateInDirectionOfMovement();
    }
    
    private void ApplyMovementInputToAnimator()
    {
        if (_animator)
        {
            _animator.SetFloat("MovementInput",
                Mathf.Max(Mathf.Abs(_horizontalInput),
                    Mathf.Abs(_verticalInput))); //Mathf.Max returns whichever value is greater of the two paraments in (float a, float b)
        }
    }

    private bool IsAttacking()
    {
        return _animator.GetBool("Attacking");
    }

    private bool PlayerJumpedFromGround()
    {
        return !IsAttacking() && Input.GetButtonDown("Jump" + _playerID) && _rb && _groundCheck.IsGrounded;
    }

    private void ReadHorizontalAndVerticalInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal" + _playerID); //inputs WITH smoothing (GetAxis, not GetAxisRaw)
        _verticalInput = Input.GetAxisRaw("Vertical" + _playerID); //m_playerID for determining if player 1 or 2
    }

    private void RotateInDirectionOfMovement()
    {
        //have player face the direction the camera is facing only if they are moving
        //Rotate toward movement direction
        if (_movement.magnitude >= .3f && !_animator.GetBool("Attacking") &&
            !_playerDash.Dashing) //only set transform.forward when m_movement vector is diff from vector3.zero
        {
            //rotate movement direction based on camera rotation
            float targetAngle = Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg + CamTransform.eulerAngles.y;
            if (Mathf.Abs(Input.GetAxisRaw("Mouse X")) > 0)
            {
                _turnSmoothTime = .3f;
            }
            else
            {
                _turnSmoothTime = .075f;
            }

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                _turnSmoothTime);
            
            Vector3 moveDir;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            //the angle that the character is moving * the actual movement itself
            moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            var moving = moveDir.normalized * _movementSpeed;

            if (!_animator.GetBool("Attacking"))
            {
                //how to have character face direction you are moving


                if (!IsJumping)
                {
                    _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, 5f);
                    //m_rb.AddForce(m_movementSpeed * moveDir, ForceMode.VelocityChange);
                    _rb.velocity = new Vector3(moving.x, _rb.velocity.y, moving.z);
                }
                else if (IsJumping)
                {
                    _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, 10f);
                    //aerial mobility
                    _rb.AddForce(.1f * moveDir, ForceMode.VelocityChange);
                }
            }
        }
        else
        {
            //let character jump while stopping sliding
            //character only stops completely when grounded
            //set airborne false whenever grounded
            if (_groundCheck.IsGrounded)
            {
                if (!_playerDash.Dashing)
                {
                    _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
                }
            }
        }
    }

    private void HandleJumpAnimation()
    {
        //Now I can have animations turn "Attacking" boolean on and off during a specific number of frames, turning movement ability on and off
        if (!_animator.GetBool("Attacking"))
        {
            if (_jump)
            {
                _jump = false;
                _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                       | RigidbodyConstraints.FreezeRotationZ;
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpHeight, _rb.velocity.z);
                IsJumping = true; //for landing
                _animator.SetBool("Landing", false);
                _animator.CrossFadeInFixedTime("Jumping", .2f, 0);
            }

            _movement = new Vector3(_horizontalInput, 0, _verticalInput);
        }
    }

    private void ApplyGravity()
    {
        if (!_groundCheck.IsGrounded)
        {
            _fallTimer += Time.deltaTime;
            var downForce = _downPull * _fallTimer * _fallTimer;

            if (downForce > 10f)
            {
                downForce = 10f;
            }

            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - downForce, _rb.velocity.z);

            _animator.SetBool("Airborne", true);
        }
        else
        {
            _fallTimer = 0;
        }
    }

    //Landing Functionality
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Ground")
        {
            IsJumping = false;
            _animator.SetBool("Airborne", false);
            _animator.SetTrigger("Landing");
        }
    }

    private void OnDisable()
    {
        _combatManager.enabled = false;
    }
}