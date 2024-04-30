using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Movimenta��o")]
    public bool isSprinting = false;
    public float sprintSpeed = 30f;
    public float normalSpeed = 15f;
    public float moveSpeed;
    public float gravity = -9.81f;
    public CharacterController characterController;

    [Header("Pulo")]
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    [Header("Agachar")]
    public bool isCrouching = false;
    public float crouchSpeed = 7.5f;
    public Transform playerBody;
    Vector3 crouchScale = new Vector3(1.2f, 0.9f, 1.2f);
    Vector3 playerScale = new Vector3(1.2f, 1.8f, 1.2f);

    [Header("Escalar")]
    public float climbingDistance = 0.4f;
    public Transform stairsCheck;
    public LayerMask stairsMask;
    public bool isClimbing = false;

    [Header("Anima��o")] 
    public RuntimeAnimatorController defaultController;
    public RuntimeAnimatorController withGunController;
    public RuntimeAnimatorController crouchController;

    // Componentes:
    private Animator animator;

    #endregion

    #region Fun��es Unity
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Checa se o jogador est� no ch�o
        isClimbing = Physics.CheckSphere(stairsCheck.position, climbingDistance, stairsMask); //*Checa se o jogador est� escalando

        if (!isGrounded)
        {
            isCrouching = false;
        }


        if (isGrounded && velocity.y < -20)
        {
            velocity.y = -2f; //Podia ser 0, mas o checksphere ativa antes, ent � mais seguro deixar menor
        }

        #region Andar
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        #endregion

        #region Animando

        if (isCrouching) // Agachado
        {
            if (animator.runtimeAnimatorController != crouchController)
            {
                animator.runtimeAnimatorController = crouchController;
            }

            // Checa Anima��o de Andar Agachado
            if (move != Vector3.zero && !isSprinting)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else // Default & Com Arma
        {
            if (PlayerStats.HasGun)
            {
                if (animator.runtimeAnimatorController != withGunController)
                {
                    animator.runtimeAnimatorController = withGunController;
                }
            }
            else
            {
                if (animator.runtimeAnimatorController != defaultController)
                {
                    animator.runtimeAnimatorController = defaultController;
                }
            }
            
            // Checa Anima��o de Andar
            if (move != Vector3.zero && !isSprinting)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            // Checa Anima��o de Pulo
            animator.SetBool("isGrounded", isGrounded);

            // Checa Anima��o de Correr
            animator.SetBool("IsRunning", isSprinting);
        }
        #endregion

        if (!isClimbing) //Movimenta��o normal, enquanto n�o est� escalando
        {
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }

        //ARRUMAR
        #region Escalar
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 climbMovement = transform.up * verticalInput * moveSpeed * Time.deltaTime;
            characterController.Move(climbMovement);
        }
        #endregion

        #region Pular
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching) //Se est� no ch�o e n�o est� agachado, pode pular
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        #endregion

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        #region Correr
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching) //Se est� no ch�o, n�o est� agachado, e pressionar shift, pode correr
        {
            isSprinting = true; //Muda a velocidade
            moveSpeed = sprintSpeed;
        }
        else if (!isCrouching)
        {
            isSprinting = false; //Muda a velocidade
            moveSpeed = normalSpeed;
        }
        #endregion

        //MUDAR L�GICA DE AGACHAR, NO MOMENTO ELE S� FICA MENOR
        #region Agachar
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded && !isCrouching)
        {
            isCrouching = true;
            playerBody.transform.localScale = crouchScale;
            characterController.transform.localScale = crouchScale;
            moveSpeed = crouchSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && isGrounded && isCrouching)
        {
            isCrouching = false;
            playerBody.transform.localScale = playerScale;
            characterController.transform.localScale = playerScale;
            moveSpeed = normalSpeed;
        }
        #endregion
    }
    #endregion
}
