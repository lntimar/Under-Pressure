using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Movimentação")]
    public bool isSprinting = false;
    public float sprintSpeed = 20f;
    public float moveSpeed = 15f;
    public float gravity = -9.81f;
    public CharacterController characterController;

    [Header("Pulo")]
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    //public int jumpsRemaining = 1;

    /*[Header("Agachar")]
    public Transform playerBody;
    public bool isCrouching = false;
    Vector3 crouchScale = new Vector3(1.2f, 0.9f, 1.2f);
    Vector3 playerScale = new Vector3(1.2f, 1.8f, 1.2f);
    */
    #endregion

    #region Funções Unity
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0.2f; //Podia ser 0, mas o checksphere ativa antes, ent é mais seguro deixar menor
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * moveSpeed * Time.deltaTime);

        #region Pular
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        #endregion

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        #region Correr
        //CORRER
        /*if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
        }
        else
        {
            isSprinting = false;
        }*/
        #endregion

        #region Agachar
        //AGACHAR (ARRUMAR)
        /*if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && !isCrouching)
        {
            isCrouching = true;
            playerBody.transform.localScale = crouchScale;
            characterController.transform.localScale = crouchScale;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && isGrounded && isCrouching)
        {
            isCrouching = false;
            playerBody.transform.localScale = playerScale;
            characterController.transform.localScale = playerScale;
        }*/
        #endregion
    }
    #endregion

    #region Funções Próprias

    #endregion
}
