using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    #region Global Variables
    // Inspector:
    [Header("Configurações:")]
    //Tamanho da visão do inimigo
    [Header("Visão do Inimigo:")]
    [SerializeField] private float lookRadius = 10f;

    [Header("Movimentação:")]
    [SerializeField] private float minSpeed = 0f;

    [Header("Sons:")] 
    [SerializeField] private float screamInterval = 20f;

    [Header("Ataque:")] 
    [SerializeField] private float resetAttackTime = 1.25f;

    // Componentes:
    private NavMeshAgent _agent;
    private BoxCollider[] _boxColliders;

    // Referências:
    private Transform _target;
    private Animator _enemyAnimator;
    private Transform _enemyTransform;

    // Sfx:
    private int _enemySfxIndex = 1;
    private bool _canScream = true;

    // Ataque:
    private bool _canAttack = true;
    #endregion

    #region Default Methods
    private void Start()
    {
        _enemyAnimator = transform.Find("Enemy Body").gameObject.GetComponent<Animator>();
        _enemyTransform = _enemyAnimator.gameObject.transform;
        _target = PlayerManager.instance.player.transform;
        _agent = GetComponent<NavMeshAgent>();
        _boxColliders = GetComponentsInChildren<BoxCollider>();
    }

    private void Update()
    {
        //Calcula a distância entre o Player e o Inimigo
        float distance = Vector3.Distance(transform.position,  _target.position);

        if (distance <= _agent.stoppingDistance)
        {
            _agent.enabled = false;

            if (_canAttack)
            {
                _enemyAnimator.Play("Enemy Attack " + Random.Range(1, 3));
                _canAttack = false;

                Invoke("ResetCanAttack", resetAttackTime);

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("enemy attack " + _enemySfxIndex);

                    if (_enemySfxIndex == 1) _enemySfxIndex = 2;
                    else _enemySfxIndex = 1;
                }
            }
            FaceTarget();
        }
        else
        {
            if (distance <= lookRadius)
            {
                FaceTarget();
                _enemyAnimator.Play("Enemy Running");
                PlayScreamSFX();
                _agent.enabled = true;
                _agent.SetDestination(_target.position);
            }
            else
            {
                _enemyAnimator.Play("Enemy Idle");
                _agent.enabled = false;
            }
        }
    }
    #endregion

    #region Custom Methods
    //Faz com que o inimigo sempre olhe para o player
    private void FaceTarget ()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

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

    private void ResetCanAttack() => _canAttack = true;

    //Apenas visual do raio de visão
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void PlayScreamSFX()
    {
        if (AudioManager.Instance != null && _canScream)
        {
            AudioManager.Instance.PlaySFX("enemy scream " + Random.Range(1, 3));
            StartCoroutine(SetScreamInterval());
        }
    }

    private IEnumerator SetScreamInterval()
    {
        _canScream = false;
        yield return new WaitForSeconds(screamInterval);
        _canScream = true;
    }
    #endregion
}
