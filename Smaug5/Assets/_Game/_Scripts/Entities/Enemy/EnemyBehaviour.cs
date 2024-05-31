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

    [Header("Sons:")] 
    [SerializeField] private float screamInterval = 20f;

    // Componentes:
    private NavMeshAgent _agent;
    private Animator _animator;
    private BoxCollider[] _boxColliders;
    private Rigidbody _rb;

    // Referências:
    private Transform _target;

    // Sfx:
    private int _enemySfxIndex = 1;
    private bool _canScream = true;
    #endregion

    #region Default Methods
    private void Start()
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
        //if (distance <= lookRadius && distance > _agent.stoppingDistance)
        if (distance > _agent.stoppingDistance + (0.5f))
        {
            _agent.SetDestination(_target.position);
            FaceTarget();
            _animator.SetBool("Chasing", true);
            PlayScreamSFX();
            Debug.Log("chasing on");
        }
        else
        {
            //Está perto do player e pronto para atacar
            //_agent.SetDestination(transform.position);
            _agent.velocity = Vector3.zero;
            _animator.SetBool("Chasing", false);
            Debug.Log("chasing off");
            EnemyAttack();
            FaceTarget();
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
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("enemy attack " + _enemySfxIndex);

                if (_enemySfxIndex == 1) _enemySfxIndex = 2;
                else _enemySfxIndex = 1;
            }
            _animator.Play("Enemy Attack " + Random.Range(1, 4));
            Debug.Log("Trigger Attack");
            //_agent.SetDestination(transform.position);
        }
    }

    public void disableAttack()
    {
        foreach (BoxCollider collider in _boxColliders)
        {
            collider.enabled = false;
            //Debug.Log("Desativou Colisor");
        }
    }

    public void enableAttack()
    {
        foreach (BoxCollider collider in _boxColliders)
        {
            collider.enabled = true;
            //Debug.Log("Ativou Colisor");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //var player = other.GetComponent<PlayerMovement>();
        /*var player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("Bateu");
            //tirar vida jogador
        }*/
    }

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
