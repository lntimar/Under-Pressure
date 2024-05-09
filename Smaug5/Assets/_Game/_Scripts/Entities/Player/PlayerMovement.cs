using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Modelo:")] 
    public GameObject playerBody1;
    public GameObject playerBody2;

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
    public RuntimeAnimatorController climbController;

    [Header("Refer�ncias:")] 
    public CameraHeadBob cameraHeadBobScript;

    private Animator playerAnimator;

    private enum PlayerModel
    {
        DEFAULT,
        WITH_GUN
    }
    #endregion

    #region Fun��es Unity
    void Awake() => ChangeModel(PlayerModel.DEFAULT);

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Checa se o jogador est� no ch�o
        isClimbing = Physics.CheckSphere(stairsCheck.position, climbingDistance, stairsMask); //Checa se o jogador est� escalando

        if (!isGrounded)
        {
            if (!isClimbing)
            {
                cameraHeadBobScript.Stop();
            }

            isCrouching = false;
        }
        else
        {
            if (!cameraHeadBobScript.IsActive())
            {
                cameraHeadBobScript.Enable();
            }
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
            if (playerAnimator.runtimeAnimatorController != crouchController)
            {
                ChangeModel(PlayerModel.DEFAULT);

                playerAnimator.runtimeAnimatorController = crouchController;
                playerAnimator.speed = 1f;
            }

            // Checa Anima��o de Andar Agachado
            if (move != Vector3.zero && !isSprinting)
            {
                playerAnimator.SetBool("isWalking", true);
            }
            else
            {
                playerAnimator.SetBool("isWalking", false);
            }
        }
        else if (isClimbing) // Escalando
        {
            if (playerAnimator.runtimeAnimatorController != climbController)
            {
                ChangeModel(PlayerModel.DEFAULT);

                playerAnimator.runtimeAnimatorController = climbController;
            }
        }
        else // Default & Com Arma
        {
            if (PlayerStats.HasGun)
            {
                if (playerAnimator.runtimeAnimatorController != withGunController)
                {
                    ChangeModel(PlayerModel.WITH_GUN);

                    playerAnimator.runtimeAnimatorController = withGunController;
                    playerAnimator.speed = 1f;
                }
            }
            else
            {
                if (playerAnimator.runtimeAnimatorController != defaultController)
                {
                    ChangeModel(PlayerModel.DEFAULT);

                    playerAnimator.runtimeAnimatorController = defaultController;
                    playerAnimator.speed = 1f;
                }
            }
            
            // Checa Anima��o de Andar
            if (move != Vector3.zero && isGrounded)
            {
                if (isSprinting) // Caso estiver Correndo
                {
                    playerAnimator.SetBool("isWalking", false);
                    playerAnimator.SetBool("isSprinting", true);
                }
                else // Caso s� estiver Caminhando
                {
                    playerAnimator.SetBool("isWalking", true);
                    playerAnimator.SetBool("isSprinting", false);
                }
            }
            else
            {
                playerAnimator.SetBool("isWalking", false);
                playerAnimator.SetBool("isSprinting", false);
            }

            // Desativa Anima��o de Pulo
            playerAnimator.SetFloat("speedY", velocity.y);
        }
        #endregion

        if (!isClimbing) //Movimenta��o normal, enquanto n�o est� escalando
        {
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }

        #region Escalar
        if (isClimbing)
        {
            gravity = 0f;
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 climbDirection = Vector3.up * verticalInput * moveSpeed;

            if (verticalInput == 0)
            {
                velocity.y = 0f;
            }

            characterController.Move(climbDirection * Time.deltaTime);
        }
        else
        {
            gravity = -9.81f;
        }
        #endregion

        #region Pular
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching) //Se est� no ch�o e n�o est� agachado, pode pular
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerAnimator.SetTrigger("Jump");
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
            //playerBody.transform.localScale = crouchScale;
            //characterController.transform.localScale = crouchScale;
            moveSpeed = crouchSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && isGrounded && isCrouching)
        {
            isCrouching = false;
            //playerBody.transform.localScale = playerScale;
            //characterController.transform.localScale = playerScale;
            moveSpeed = normalSpeed;
        }
        #endregion
    }

    // Ativa o modelo do Player desejado para as anima��es
    void ChangeModel(PlayerModel type)
    {
        if (type == PlayerModel.DEFAULT)
        {
            playerBody1.SetActive(true);
            playerBody2.SetActive(false);

            playerAnimator = playerBody1.GetComponent<Animator>();
        }
        else // WITH_GUN
        {
            playerBody1.SetActive(false);
            playerBody2.SetActive(true);

            playerAnimator = playerBody2.GetComponent<Animator>();
        }
    }
    #endregion
}
