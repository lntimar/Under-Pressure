using System;
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

    [Header("Rotação")]
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


    // Componentes:
    private Rigidbody _rb;

    // Rotação:
    private float _xRotation;
    private float _desiredX;

    // Pulo:
    private bool _readyToJump = true;
    private bool _cancellingGrounded;

    // Agachar:
    private Vector3 _crouchScale;
    private Vector3 _playerScale;

    // Inputs:
    private float _x, _y;
    private bool _jumping, _sprinting, _crouching;
    
    public bool HasTouchStairs = false;
    #endregion

    #region Funções Unity

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerCam = FindObjectOfType<MoveCamera>().transform;

        _weapon = FindObjectOfType<Weapon>().gameObject;

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
        Look();
        Animate();

        if (PlayerStats.HasGun)
        {
            if (_crouching || HasTouchStairs)
                _weapon.SetActive(false);
            else
                _weapon.SetActive(true);
        }
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
        //Make sure we are only checking for walkable layers
        int layer = collision.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                _cancellingGrounded = false;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
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
        _jumping = Input.GetButton("Jump");
        _crouching = Input.GetKey(KeyCode.LeftControl);
        _sprinting = Input.GetKey(KeyCode.LeftShift);

        //Crouching
        if (Input.GetKey(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch()
    {
        transform.localScale = _crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void StopCrouch()
    {
        transform.localScale = _playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        _rb.useGravity = true;
        _rb.AddForce(Vector3.down * Time.deltaTime * 10);
        //Find actual velocity relative to where player is looking
        var mag = FindVelRelativeToLook();
        var xMag = mag.x;
        var yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(_x, _y, mag);

        //If holding jump && ready to jump, then jump
        if (_readyToJump && _jumping) Jump();

        //Set max speed
        var maxSpeed = this.maxSpeed;
        if (_sprinting) maxSpeed *= 4f;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (_x > 0 && xMag > maxSpeed) _x = 0;
        if (_x < 0 && xMag < -maxSpeed) _x = 0;
        if (_y > 0 && yMag > maxSpeed) _y = 0;
        if (_y < 0 && yMag < -maxSpeed) _y = 0;

        //Some multipliers
        var multiplier = 1f;
        var multiplierV = 1f;

        // Movement in air
        /*
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        */

        //Apply forces to move player
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
        var horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0)
            _rb.velocity = new Vector3(horizontalInput, verticalInput, 0f).normalized * climbSpeed * Time.deltaTime;
        else
            _rb.velocity = Vector3.right * horizontalInput * climbSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (grounded && _readyToJump)
        {
            _readyToJump = false;

            //Add jump forces
            _rb.AddForce(Vector2.up * jumpForce * 1.5f);

            //If jumping while falling, reset _y velocity.
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

        //Find current look rotation
        var rot = _playerCam.transform.localRotation.eulerAngles;
        _desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        //Perform the rotations
        _playerCam.transform.localRotation = Quaternion.Euler(_xRotation, _desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, _desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || _jumping) return;

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            _rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            _rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(_rb.velocity.x, 2) + Mathf.Pow(_rb.velocity.z, 2))) > maxSpeed)
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
                playerClimbAnimator.SetBool("move", true);
            else
                playerClimbAnimator.SetBool("move", false);
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
    #endregion
}