using System;
using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações:")]

    [Header("Referências:")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Animator playerWalkAnimator;
    [SerializeField] private Animator playerGunAnimator;
    [SerializeField] private Animator playerCrouchAnimator;
    [SerializeField] private Animator playerClimbAnimator;

    [Header("Passos:")] 
    [SerializeField] private float walkStepsInterval;
    [SerializeField] private float climbStepsInterval;
    [SerializeField] private float sprintStepsInterval;

    [Header("Rotação:")]
    [SerializeField] private float sensitivity = 50f;
    [SerializeField] private float sensMultiplier = 1f;

    [Header("Andar:")]
    [SerializeField] private float moveSpeed = 4500;
    [SerializeField] private float crouchSpeed = 2000;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float counterMovement = 0.175f;
    [SerializeField] private float threshold = 0.01f;
    [SerializeField] private float maxSlopeAngle = 35f;
    [SerializeField] private float crouchScalar = 0.35f;
    [SerializeField] private float sprintScalar = 20f;

    [Header("Pulo:")]
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float jumpForce = 550f;

    [Header("Escalar:")]
    [SerializeField] private float climbSpeed = 5f;

    // Referências:
    private Transform _playerCam;
    private GameObject _weapon;
    private GameObject _scanner;
    private GameMenu _gameMenuScript;

    // Componentes:
    private Rigidbody _rb;

    // Rota��o:
    private float _xRotation;
    private float _desiredX;

    // Pulo:
    private bool _readyToJump = true;
    private bool _cancellingGrounded;

    // Agachar:
    private Vector3 _crouchScale;
    private Vector3 _playerScale;
    private bool _canResetCrouchSpeed = true;
    [HideInInspector] public bool CanStopCrouch = true;

    // Inputs:
    private float _x, _y;
    [HideInInspector] public bool IsJumping { get; private set; }
    private bool _sprinting, _crouching;
    public bool HasTouchStairs = false;

    // SFX:
    private bool _canPlayStepSFX = true;
    private int _walkStepsIndex = 1;
    private int _climbStepsIndex = 1;
    private int _sprintStepsIndex = 1;
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerCam = FindObjectOfType<MoveCamera>().transform;
        _gameMenuScript = FindObjectOfType<GameMenu>();

        _weapon = FindObjectOfType<Weapon>().gameObject;
        _scanner = FindObjectOfType<ScannerHUD>().gameObject;

        if (PlayerStats.HasGun)
        {
            playerWalkAnimator.gameObject.SetActive(false);
            playerGunAnimator.gameObject.SetActive(true);
            _weapon.SetActive(true);
        }
        else
        {
            playerWalkAnimator.gameObject.SetActive(true);
            playerGunAnimator.gameObject.SetActive(false);
            _weapon.SetActive(false);
        }

        if (PlayerStats.HasScanner)
            _scanner.SetActive(true);
        else
            _scanner.SetActive(false);
    }
    
    private void Start()
    {
        _playerScale = transform.localScale;
        _crouchScale = _playerScale / 2;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();

        if (!_gameMenuScript.IsPaused())
            Look();

        Animate();

        if (PlayerStats.HasGun)
        {
            if (_crouching || HasTouchStairs)
                _weapon.SetActive(false);
            else
                _weapon.SetActive(true);
        }

        if (PlayerStats.HasScanner)
        {
            if (_crouching || HasTouchStairs)
                _scanner.SetActive(false);
            else
                _scanner.SetActive(true);
        }
        
        PlayStepsSFX();
    }

    private void FixedUpdate()
    {
        if (!HasTouchStairs)
            Movement();
        else
            StairsMovement();
    }

    private void OnCollisionStay(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.contacts[i].normal;
            if (IsFloor(normal))
            {
                grounded = true;
                _cancellingGrounded = false;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        float delay = 3f;
        if (!_cancellingGrounded)
        {
            _cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }
    #endregion

    #region Funções Próprias
    private void MyInput()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
        IsJumping = Input.GetButton("Jump");

        _sprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (_canResetCrouchSpeed)
            {
                _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
                _canResetCrouchSpeed = false;
            }
            StartCrouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && CanStopCrouch)
        {
            _canResetCrouchSpeed = true;
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        _crouching = true;
        transform.localScale = _crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void StopCrouch()
    {
        _crouching = false;
        transform.localScale = _playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    public void SetCanStopCrouch(bool canStop)
    {
        if (canStop)
        {
            _crouching = false;
            CanStopCrouch = true;
            StartCoroutine(StopCrouchInterval(1.5f));
        }
        else
        {
            CanStopCrouch = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator StopCrouchInterval(float t)
    {
        yield return new WaitForSeconds(t);
        _crouching = false;
        StopCrouch();
    }

    private void Movement()
    {
        _rb.useGravity = true;
        _rb.AddForce(Vector3.down * Time.deltaTime * 10);

        var mag = FindVelRelativeToLook();
        var xMag = mag.x;
        var yMag = mag.y;

        CounterMovement(_x, _y, mag);

        if (_readyToJump && IsJumping) Jump();

        var maxSpeed = this.maxSpeed;
        if (_sprinting) maxSpeed *= 4f;

        if (_x > 0 && xMag > maxSpeed) _x = 0;
        if (_x < 0 && xMag < -maxSpeed) _x = 0;
        if (_y > 0 && yMag > maxSpeed) _y = 0;
        if (_y < 0 && yMag < -maxSpeed) _y = 0;

        var multiplier = 1f;
        var multiplierV = 1f;

        var scalar = 1f;
        if (_crouching)
            scalar = crouchScalar;
        else if (_sprinting && grounded)
            scalar = sprintScalar;

        _rb.AddForce(orientation.transform.forward * _y * moveSpeed * scalar * Time.deltaTime * multiplier * multiplierV);
  
        _rb.AddForce(orientation.transform.right * _x * moveSpeed * scalar * Time.deltaTime * multiplier);
    }

    private void StairsMovement()
    {
        _rb.useGravity = false;

        var verticalInput = Input.GetAxis("Vertical");

        _rb.velocity = Vector3.up * verticalInput * climbSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (grounded && _readyToJump)
        {
            _readyToJump = false;

            _rb.AddForce(Vector2.up * jumpForce * 1.5f);

            if (_y > 0f)
            {
                if (_sprinting)
                    _rb.AddForce(Vector3.forward * jumpForce * 1f);
                else
                    _rb.AddForce(transform.forward * jumpForce * 0.5f);
            }

            Vector3 vel = _rb.velocity;
            if (_rb.velocity.y < 0.5f)
                _rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (_rb.velocity.y > 0)
                _rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump() => _readyToJump = true;
    
    private void Look()
    {
        var mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        var rot = _playerCam.transform.localRotation.eulerAngles;
        _desiredX = rot.y + mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _playerCam.transform.localRotation = Quaternion.Euler(_xRotation, _desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, _desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            _rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            _rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(_rb.velocity.x, 2f) + Mathf.Pow(_rb.velocity.z, 2f))) > maxSpeed)
        {
            var fallspeed = _rb.velocity.y;
            var n = _rb.velocity.normalized * maxSpeed;
            _rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        var lookAngle = orientation.transform.eulerAngles.y;
        var moveAngle = Mathf.Atan2(_rb.velocity.x, _rb.velocity.z) * Mathf.Rad2Deg;

        var u = Mathf.DeltaAngle(lookAngle, moveAngle);
        var v = 90 - u;

        var magnitude = _rb.velocity.magnitude;
        var yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        var xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        var angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private void StopGrounded() => grounded = false;

    private void Animate()
    {
        if (_crouching)  // Agachado
        {
            playerCrouchAnimator.gameObject.SetActive(true);
            playerClimbAnimator.gameObject.SetActive(false);
            playerWalkAnimator.gameObject.SetActive(false);
            playerGunAnimator.gameObject.SetActive(false);

            if (_x != 0 || _y != 0)
                playerCrouchAnimator.SetBool("move", true);
            else
                playerCrouchAnimator.SetBool("move", false);
        }
        else if (HasTouchStairs) // Escalando
        {
            playerCrouchAnimator.gameObject.SetActive(false);
            playerClimbAnimator.gameObject.SetActive(true);
            playerWalkAnimator.gameObject.SetActive(false);
            playerGunAnimator.gameObject.SetActive(false);

            if (_x != 0 || _y != 0)
            {
                playerClimbAnimator.SetBool("move", true);
            }
            else
            {
                playerClimbAnimator.SetBool("move", false);
            }
        }
        else // Andando
        {
            if (!PlayerStats.HasGun) // Sem Arma
            {
                playerCrouchAnimator.gameObject.SetActive(false);
                playerClimbAnimator.gameObject.SetActive(false);
                playerWalkAnimator.gameObject.SetActive(true);

                if (_x != 0 || _y != 0)
                {
                    playerWalkAnimator.SetBool("move", true);

                    if (_sprinting)
                        playerWalkAnimator.speed = 1.5f;
                    else
                        playerWalkAnimator.speed = 1f;
                }
                else
                {
                    playerWalkAnimator.SetBool("move", false);
                }
            }
            else // Com Arma
            {
                playerCrouchAnimator.gameObject.SetActive(false);
                playerClimbAnimator.gameObject.SetActive(false);
                playerWalkAnimator.gameObject.SetActive(false);
                playerGunAnimator.gameObject.SetActive(true);

                if (_x != 0 || _y != 0)
                {
                    playerGunAnimator.SetBool("move", true);

                    if (_sprinting)
                        playerGunAnimator.speed = 1.5f;
                    else
                        playerGunAnimator.speed = 1f;
                }
                else
                {
                    playerGunAnimator.SetBool("move", false);
                }
            }
        }
    }

    private void PlayStepsSFX()
    {
        if (_x != 0 || _y != 0)
        {
            if (_canPlayStepSFX && !_crouching)
            {
                _canPlayStepSFX = false;

                if (_sprinting)
                {
                    AudioManager.Instance.PlaySFX("player run " + _sprintStepsIndex);

                    if (_sprintStepsIndex < 4) _sprintStepsIndex++;
                    else _sprintStepsIndex = 1;

                    StartCoroutine(ResetCanPlayStepSFX(sprintStepsInterval));
                }
                else if (HasTouchStairs)
                {
                    AudioManager.Instance.PlaySFX("player climb " + _climbStepsIndex);

                    if (_climbStepsIndex == 1) _climbStepsIndex = 2;
                    else _climbStepsIndex = 2;

                    StartCoroutine(ResetCanPlayStepSFX(climbStepsInterval));
                }
                else
                {
                    AudioManager.Instance.PlaySFX("player walk " + _walkStepsIndex);

                    if (_walkStepsIndex == 1) _walkStepsIndex = 2;
                    else _walkStepsIndex = 2;

                    StartCoroutine(ResetCanPlayStepSFX(walkStepsInterval));
                }
            }
        }
        else
        {
            _walkStepsIndex = 1;
            _sprintStepsIndex = 1;
            _climbStepsIndex = 1;
        }
    }

    private IEnumerator ResetCanPlayStepSFX(float t)
    {
        yield return new WaitForSeconds(t);
        _canPlayStepSFX = true;
    }
    #endregion
}