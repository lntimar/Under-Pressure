using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CheckStairs : MonoBehaviour
{
    // Refer�ncias:
    private PlayerMove _playerMoveScript;

    private void Awake() => _playerMoveScript = FindObjectOfType<PlayerMove>();
    
    private void OnTriggerStay(Collider collision) => _playerMoveScript.HasTouchStairs = true;
    
    private void OnTriggerExit(Collider collision) => _playerMoveScript.HasTouchStairs = false;
}
