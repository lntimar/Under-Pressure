using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private EnemyBehavior enemyBehavior;

    private void Start()
    {
        enemyBehavior = GetComponentInParent<EnemyBehavior>();
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
