using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStairs : MonoBehaviour
{
    [Header("Refer�ncias:")]
    [SerializeField] private PlayerMove playerMoveScript;

    private void OnTriggerStay(Collider collision) => playerMoveScript.HasTouchStairs = true;
    
    private void OnTriggerExit(Collider collision) => playerMoveScript.HasTouchStairs = false;
}
