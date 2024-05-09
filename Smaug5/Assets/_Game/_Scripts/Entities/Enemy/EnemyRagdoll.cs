using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Configura��es:")]

    [Header("Tempo:")]
    [SerializeField] private float ragdollTime = 1.5f;

    [Header("Refer�ncias:")]
    [SerializeField] private CapsuleCollider capsuleCollder;

    // Componentes:
    private Animator _animator;
    #endregion

    #region Fun��es Unity
    private void Start() => _animator = GetComponent<Animator>();
    #endregion

    #region Fun��es Pr�prias
    public void StartRagdoll()
    {
        _animator.enabled = false;
        capsuleCollder.enabled = false;

        Invoke("StopRagdoll", ragdollTime);
    }

    private void StogRadgoll()
    {
        _animator.enabled = true;
        capsuleCollder.enabled = true;

        _animator.Play("Enemy Stand Up " + Random.Range(1, 3));
    }
    #endregion
}
