using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSlopeStair : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Configura��es:")]

    [Header("Refer�ncias:")]
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private PlayerMove _playerMove;

    // Inputs:
    private float _x, _y;
    #endregion

    #region Fun��es Unity
    private void Update() => VerifyInputs();
    
    private void OnTriggerStay(Collider collision) => StopSlide();

    private void OnTriggerExit(Collider collision) => DropPlayerDown();
    #endregion

    #region Fun��es Pr�prias
    private void VerifyInputs()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
    }

    private void StopSlide()
    {
        if (_x == 0 && _y == 0)
        {
            _playerRb.velocity = Vector3.zero;
            _playerRb.isKinematic = true;
        }
        else
        {
            _playerRb.isKinematic = false;
        }
    }

    private void DropPlayerDown()
    {
        print("Foi!");
        if (!_playerMove.IsJumping)
            _playerRb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
    }
    #endregion
}
