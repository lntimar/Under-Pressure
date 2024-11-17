using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyBehaviour : MonoBehaviour
{
    #region Global Variables
    // Inspector:
    [Header("Configurações:")]
    //Tamanho da visão do inimigo
    [Header("Visão do Inimigo:")]
    [SerializeField] private float lookRadius = 20f;

    [Header("Sons:")] 
    [SerializeField] private float screamInterval = 20f;

    //[Header("Ataque:")] 
    //[SerializeField] private float resetAttackTime = 1.25f;

    // Componentes:
    private NavMeshAgent _agent;
    private BoxCollider[] _boxColliders;

    // Referências:
    private Transform _player;
    private Animator _enemyAnimator;
    private Transform _enemyTransform;

    // Sfx:
    private int _enemySfxIndex = 1;
    private bool _canScream = true;
    #endregion

    #region Default Methods
    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyTransform = _enemyAnimator.gameObject.transform;
        _player = PlayerManager.instance.player.transform;
        _agent = GetComponent<NavMeshAgent>();
        _boxColliders = GetComponentsInChildren<BoxCollider>();
    }

    private void Update()
    {
    }


    #endregion

    #region Custom Methods
    public void disableAttack()
    {
        foreach (BoxCollider collider in _boxColliders)
        {
            collider.enabled = false;
            Debug.Log("Desativou Colisor");
        }
    }

    public void enableAttack()
    {
        foreach (BoxCollider collider in _boxColliders)
        {
            collider.enabled = true;
            Debug.Log("Ativou Colisor");
        }
    }

    //Apenas visual do raio de vis�o
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    #endregion

}
