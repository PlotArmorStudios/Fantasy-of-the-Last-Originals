using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;
using Skills;

public enum PlayerStance
{
    Stance1,
    Stance2,
    Stance3,
    Stance4
}

[DefaultExecutionOrder(100)]
public class Player : Character
{
    [SerializeField] private int _playerNumber;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private bool _jump;
    [SerializeField] private bool isGrounded;

    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _weight = 2f;
    [SerializeField] private float _turnSmoothTime = 2f;
    [SerializeField] private float _turnSmoothVelocity = 2f;

    [SerializeField] private Camera _camera;
    [field: SerializeField] public bool IsFalling { get; set; }

    public Controller Controller { get; private set; }

    public bool IsGrounded => _groundCheck.IsGrounded;
    public Animator Animator => _animator;

    public bool HasController
    {
        get { return Controller != null; }
    }

    public int PlayerNumber => _playerNumber;

    public bool MultiplePlayers { get; set; }

    private CameraLogic _cameraLogic;

    private Rigidbody _rb;
    private Animator _animator;
    private PlayerCombatManager _combatManager;

    private GroundCheck _groundCheck;
    public float FallTimer { get; set; }

    private DodgeManeuver _dodgeManeuver;

    public Transform CamTransform;

    private Vector3 _movement;
    private Vector3 _heightMovement;
    private float _horizontalInput;
    private float _verticalInput;

    private PhotonView _view;

    private State _currentState;
    private bool _negateGravity;
    private bool _applyUpForce;
    private float _upForce;
    private SkillInventory _skillInventory;


    private void Awake()
    {
        _combatManager = GetComponent<PlayerCombatManager>();
        _skillInventory = GetComponent<SkillInventory>();
    }

    void Start()
    {
        _view = GetComponent<PhotonView>();
        _dodgeManeuver = GetComponent<DodgeManeuver>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
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

    public void InitializePlayer(Controller controller)
    {
        Controller = controller;
        _combatManager.Controller = controller;
        _skillInventory.Controller = controller;
        gameObject.name = $"Player {_playerNumber} {controller.gameObject.name}";
        
        if (ControllerManager.MultiplePlayers)
            _camera.gameObject.SetActive(false);
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
        return !IsAttacking() && Input.GetButtonDown($"Jump {_playerNumber}") && _rb && _groundCheck.IsGrounded;
    }

    private void ReadHorizontalAndVerticalInput()
    {
        _horizontalInput = Input.GetAxisRaw($"Horizontal {_playerNumber}");
        _verticalInput = Input.GetAxisRaw($"Vertical {_playerNumber}");
    }

    private void RotateInDirectionOfMovement()
    {
        //have player face the direction the camera is facing only if they are moving
        //Rotate toward movement direction
        _movement = new Vector3(_horizontalInput, 0, _verticalInput);
        
        if (_movement.magnitude >= .3f && !_animator.GetBool("Attacking") &&
            !_dodgeManeuver.Dodging) //only set transform.forward when m_movement vector is diff from vector3.zero
        {
            CalculateMovementDirection();
        }
        else
        {
            //let character jump while stopping sliding
            //character only stops completely when grounded
            //set airborne false whenever grounded
            if (_groundCheck.IsGrounded)
            {
                if (!_dodgeManeuver.Dodging)
                {
                    _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
                }
            }
        }
    }

    private void CalculateMovementDirection()
    {
        //rotate movement direction based on camera rotation
        float targetAngle = 0;

        if (ControllerManager.MultiplePlayers)
            targetAngle = CalculateAngle();
        else
            targetAngle = CalculateAngleWithCam();

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
                //aerial mobility
                _rb.AddForce(.1f * moveDir, ForceMode.VelocityChange);
            }
        }
    }

    private float CalculateAngleWithCam()
    {
        return Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg + CamTransform.eulerAngles.y;
    }

    private float CalculateAngle()
    {
        return Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg;
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
        }
    }

    public void NegateGravity(float delay)
    {
        StartCoroutine(StopGravity(delay));
    }

    

    private IEnumerator StopGravity(float reapplyDelay)
    {
        _negateGravity = true;
        yield return new WaitForSeconds(reapplyDelay);
        _negateGravity = false;
    }

    private void ApplyGravity()
    {
        if (!_groundCheck.IsGrounded)
        {
            if (_negateGravity)
            {
                _rb.velocity = Vector3.zero;
                FallTimer += 0;
            }
            else
            {
                FallTimer += Time.deltaTime;
                var downForce = _weight * FallTimer * FallTimer;

                if (downForce > 3f)
                    downForce = 3f;

                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - downForce, _rb.velocity.z);

                if (_rb.velocity.y < 0)
                    IsFalling = true;
            }

            _animator.SetBool("Airborne", true);
        }
        else
        {
            FallTimer = 0;
            IsFalling = false;
        }
    }
}