using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    #region Global Variables
    //Tamanho da visão do inimigo
    public float lookRadius = 10f;
    public float minSpeed = 0f;

    Transform target;
    NavMeshAgent agent;
    Animator animator;
    BoxCollider[] boxColliders;
    private Rigidbody _rb;

    #endregion

    #region Default Methods
    void Start()
    {
        animator = transform.Find("GFX").gameObject.GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        boxColliders = GetComponentsInChildren<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Calcula a distância entre o Player e o Inimigo
        float distance = Vector3.Distance(target.position, transform.position);
        
        //Se player está dentro do raio de visão, calcula automaticamente a rota até ele
        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position);

            //Está perto do player e pronto para atacar
            if (distance <= agent.stoppingDistance)
            {
                animator.SetBool("Chasing", false);
                //Debug.Log("chasing off");
                EnemyAttack();
                FaceTarget();
            }
            else
            {
                animator.SetBool("Chasing", true);
                //Debug.Log("chasing on");
            }
        }
    }

    #endregion

    #region Custom Methods
    //Faz com que o inimigo sempre olhe para o player
    private void FaceTarget ()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //Ataque acontece quando player encosta na hitbox na mão do inimigo
    public void EnemyAttack()
    {
        var animInfo = animator.GetCurrentAnimatorClipInfo(0);
        var curAnimName = animInfo[0].clip.name;
        if (!curAnimName.Contains("Attack"))
        {
            animator.Play("Enemy Attack " + Random.Range(1, 5));
            Debug.Log("Trigger Attack");
            //agent.SetDestination(transform.position);
        }
    }

    public void disableAttack()
    {
        foreach (BoxCollider collider in boxColliders)
        {
            collider.enabled = false;
            Debug.Log("Desativou Colisor");
        }
    }

    public void enableAttack()
    {
        foreach (BoxCollider collider in boxColliders)
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
