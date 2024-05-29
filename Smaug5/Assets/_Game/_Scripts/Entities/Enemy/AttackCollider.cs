using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private EnemyBehaviour _enemyBehavior;
    private Animator _animator;

    private void Start()
    {
        _enemyBehavior = GetComponentInParent<EnemyBehaviour>();
        _animator = GetComponent<Animator>();
    }

    public void DisableAttack()
    {
        _enemyBehavior.disableAttack();
        _animator.applyRootMotion = false;
    }

    public void EnableAttack() 
    { 
        _enemyBehavior.enableAttack();
        _animator.applyRootMotion = true;
    }
}
