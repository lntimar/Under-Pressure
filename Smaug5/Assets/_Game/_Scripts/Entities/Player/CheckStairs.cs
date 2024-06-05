using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStairs : MonoBehaviour
{
    [Header("Referências:")]
    [SerializeField] private PlayerMove playerMoveScript;

    private void OnTriggerStay(Collider collision) => playerMoveScript.HasTouchStairs = true;
    
    private void OnTriggerExit(Collider collision) => playerMoveScript.HasTouchStairs = false;
}
