using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private EnemyBehaviour enemyBehavior;

    private void Start()
    {
        enemyBehavior = GetComponentInParent<EnemyBehaviour>();
    }

    public void DisableAttack()
    {
        enemyBehavior.disableAttack();
    }

    public void EnableAttack() 
    { 
        enemyBehavior.enableAttack();
    }
}
