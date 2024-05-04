using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    #region Global Variables
    [Header("Configurações:")]
    //Tamanho da visão do inimigo
    [Header("Visão do Inimigo:")]
    [SerializeField] private float lookRadius = 10f;

    [Header("Movimentação:")]
    [SerializeField] private float minSpeed = 0f;

    // Componentes:
    private NavMeshAgent _agent;
    private Animator _animator;
    private BoxCollider[] _boxColliders;
    private Rigidbody _rb;

    // Referências:
    private Transform _target;
    #endregion

    #region Default Methods
    void Start()
    {
        _animator = transform.Find("Enemy Body").gameObject.GetComponent<Animator>();
        _target = PlayerManager.instance.player.transform;
        _agent = GetComponent<NavMeshAgent>();
        _boxColliders = GetComponentsInChildren<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Calcula a distância entre o Player e o Inimigo
        float distance = Vector3.Distance(_target.position, transform.position);
        
        //Se player está dentro do raio de visão, calcula automaticamente a rota até ele
        if (distance <= lookRadius) 
        {
            _agent.SetDestination(_target.position);

            //Está perto do player e pronto para atacar
            if (distance <= _agent.stoppingDistance)
            {
                _animator.SetBool("Chasing", false);
                //Debug.Log("chasing off");
                EnemyAttack();
                FaceTarget();
            }
            else
            {
                _animator.SetBool("Chasing", true);
                //Debug.Log("chasing on");
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

    //Ataque acontece quando player encosta na hitbox na mão do inimigo
    public void EnemyAttack()
    {
        var animInfo = _animator.GetCurrentAnimatorClipInfo(0);
        var curAnimName = animInfo[0].clip.name;
        if (!curAnimName.Contains("Attack"))
        {
            _animator.Play("Enemy Attack " + Random.Range(1, 5));
            Debug.Log("Trigger Attack");
            //_agent.SetDestination(transform.position);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            Debug.Log("Bateu");
            //tirar vida jogador
        }
    }

    //Apenas visual do raio de visão
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    #endregion
}
