using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    #region Global Variables
    //Tamanho da vis�o do inimigo
    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;

    #endregion

    #region Default Methods
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Calcula a dist�ncia entre o Player e o Inimigo
        float distance = Vector3.Distance(target.position, transform.position);
        
        //Se player est� dentro do raio de vis�o, calcula automaticamente a rota at� ele
        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position);

            //Est� perto do player e pronto para atacar
            if (distance <= agent.stoppingDistance)
            {
                //Atacar jogador
                FaceTarget();
            }
        }
    }

    #endregion

    #region Custom Methods
    //Faz com que o inimigo sempre olhe para o player
    void FaceTarget ()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //Apenas visual do raio de vis�o
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    #endregion
}
