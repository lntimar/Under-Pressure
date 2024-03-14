using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //Tamanho da visão do inimigo
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
        //Calcula a distância entre o Player e o Inimigo
        float distance = Vector3.Distance(target.position, transform.position);
        
        //Se player está dentro do raio de visão, calcula automaticamente a rota até ele
        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position);
        }
    }

    //Apenas visual do raio de visão
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
