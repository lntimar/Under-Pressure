using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDuct : MonoBehaviour
{
    private PlayerMove _playerMoveScript;

    private void Awake() => _playerMoveScript = FindObjectOfType<PlayerMove>();

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Duct"))
            _playerMoveScript.SetCanStopCrouch(false);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Duct"))
            _playerMoveScript.SetCanStopCrouch(true);
    } 
}
