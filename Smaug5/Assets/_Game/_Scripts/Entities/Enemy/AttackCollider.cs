using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    #region Variáveis Globais
    // Componentes:
    private EnemyBehaviour _enemyBehaviour;
    #endregion

    #region Funções Unity
    private void Start() => _enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    #endregion

    #region Funções Próprias
    public void DisableAttack() => _enemyBehaviour.disableAttack();

    public void EnableAttack() => _enemyBehaviour.enableAttack();
    #endregion
}
