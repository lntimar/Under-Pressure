using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    #region Vari�veis Globais
    // Componentes:
    private EnemyBehaviour _enemyBehaviour;
    #endregion

    #region Fun��es Unity
    private void Start() => _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    #endregion

    #region Fun��es Pr�prias
    public void DisableAttack() => _enemyBehaviour.disableAttack();

    public void EnableAttack() => _enemyBehaviour.enableAttack();
    #endregion
}
