using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //Tamanho da vis�o do inimigo
    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;

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
        }
    }

    //Apenas visual do raio de vis�o
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
