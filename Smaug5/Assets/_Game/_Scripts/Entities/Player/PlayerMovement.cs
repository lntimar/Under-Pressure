using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Movimentação")]
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

    [Header("Animação")] 
    public RuntimeAnimatorController defaultController;
    public RuntimeAnimatorController withGunController;
    public RuntimeAnimatorController crouchController;
    public RuntimeAnimatorController climbController;

    // Componentes:
    private Animator animator;

    #endregion

    #region Funções Unity
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Checa se o jogador está no chão
        isClimbing = Physics.CheckSphere(stairsCheck.position, climbingDistance, stairsMask); //*Checa se o jogador está escalando

        if (!isGrounded)
        {
            isCrouching = false;
        }


        if (isGrounded && velocity.y < -20)
        {
            velocity.y = -2f; //Podia ser 0, mas o checksphere ativa antes, ent é mais seguro deixar menor
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
                animator.speed = 1f;
            }

            // Checa Animação de Andar Agachado
            if (move != Vector3.zero && !isSprinting)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else if (isClimbing) // Escalando
        {
            if (animator.runtimeAnimatorController != climbController)
            {
                animator.runtimeAnimatorController = climbController;
            }

            if (move.y != 0)
            {
                animator.speed = 1f;
            }
            else
            {
                animator.speed = 0f;
            }
        }
        else // Default & Com Arma
        {
            if (PlayerStats.HasGun)
            {
                if (animator.runtimeAnimatorController != withGunController)
                {
                    animator.runtimeAnimatorController = withGunController;
                    animator.speed = 1f;
                }
            }
            else
            {
                if (animator.runtimeAnimatorController != defaultController)
                {
                    animator.runtimeAnimatorController = defaultController;
                    animator.speed = 1f;
                }
            }
            
            // Checa Animação de Andar
            if (move != Vector3.zero && !isSprinting)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            // Checa Animação de Pulo
            animator.SetBool("isGrounded", isGrounded);

            // Checa Animação de Correr
            animator.SetBool("IsRunning", isSprinting);
        }
        #endregion

        if (!isClimbing) //Movimentação normal, enquanto não está escalando
        {
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }

        //QUASE LÁ
        #region Escalar
        if (isClimbing)
        {
            gravity = 0f;
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 climbDirection = Vector3.up * verticalInput * moveSpeed;
            characterController.Move(climbDirection * Time.deltaTime);
        }
        else
        {
            gravity = -9.81f;
        }
        #endregion

        #region Pular
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching) //Se está no chão e não está agachado, pode pular
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        #endregion

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        #region Correr
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !isCrouching) //Se está no chão, não está agachado, e pressionar shift, pode correr
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

        //MUDAR LÓGICA DE AGACHAR, NO MOMENTO ELE SÓ FICA MENOR
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
