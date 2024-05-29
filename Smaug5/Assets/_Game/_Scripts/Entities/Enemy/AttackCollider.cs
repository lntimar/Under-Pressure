using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    #region Vari�veis Globais
    // Componentes:
    private EnemyBehaviour _enemyBehaviour;
    private Animator _animator;
    #endregion

    #region Fun��es Unity
    private void Start() => _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();

    private void Update() => VerifyIsAttacking();
    #endregion

    #region Fun��es Pr�prias
    public void DisableAttack() => _enemyBehaviour.disableAttack();

    public void EnableAttack() => _enemyBehaviour.enableAttack();

    private void VerifyIsAttacking()
    {
        var animInfo = _animator.GetCurrentAnimatorClipInfo(0);
        var curAnimName = animInfo[0].clip.name;
        if (curAnimName.Contains("Attack"))
            _animator.applyRootMotion = true;
        else
            _animator.applyRootMotion = false;
    }
    #endregion
}
