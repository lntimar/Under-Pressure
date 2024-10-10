using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSlopeStair : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Configurações:")]

    [Header("Pulo Slope:")]
    [SerializeField] private float slopeJumpForce;
    [SerializeField] private float slopeJumpTime;

    [Header("Referências:")]
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private PlayerMove _playerMove;

    // Inputs:
    private float _x, _y;

    private bool _isOnSlope = false;
    private bool _isJumpingSlope = false;
    #endregion

    #region Funções Unity
    private void Update()
    {
        VerifyInputs();
        SlopeJump();
    }

    private void OnTriggerStay(Collider collision)
    {
        _isOnSlope = true;
        StopSlide();
    }

    private void OnTriggerExit(Collider collision)
    {
        _isOnSlope = false;
        _playerRb.isKinematic = false;
        DropPlayerDown();
    }
    #endregion

    #region Funções Próprias
    private void VerifyInputs()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");

        if (_isOnSlope && !_isJumpingSlope && Input.GetKeyDown(KeyCode.Space)) 
        {
            _playerRb.isKinematic = false;
            _isJumpingSlope = true;
        }
    }

    private void SlopeJump()
    {
        if (_isJumpingSlope) 
            _playerMove.gameObject.transform.position += Vector3.up * slopeJumpForce * Time.deltaTime;
    }

    private void StopJumpingSlope() => _isJumpingSlope = false;

    private void StopSlide()
    {
        if (_x == 0 && _y == 0)
            _playerRb.isKinematic = true;
        else
            _playerRb.isKinematic = false;
    }

    private void DropPlayerDown()
    {
        if (!_isJumpingSlope)
            _playerRb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
    }
    #endregion
}
