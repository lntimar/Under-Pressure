using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{
    private float timer;
    public float chaseRange = 20;
    public float patrolSpeed = 2;
    private int randomTime;
    Transform player;
    NavMeshAgent agent;

    //Lista com os pontos de patrulha do inimigo
    List<Transform> wayPoints = new List<Transform>();
    private NewEnemyBehaviour neBehaviour;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        neBehaviour = animator.GetComponent<NewEnemyBehaviour>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        timer = 0;
        //Tempo aleatório pra ficar idle
        randomTime = neBehaviour.RandomInteger(5,10);
        
        //Encontra o objeto pai com os Waypoints
        if (neBehaviour != null)
        {
            wayPoints = neBehaviour.GetWaypoints();
        }

        // Inicia a patrulha para um waypoint aleatório
        if (wayPoints.Count > 0)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
        /*GameObject go = GameObject.FindGameObjectWithTag("WayPoints");
        foreach(Transform t in go.transform)
            wayPoints.Add(t);

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);*/
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        timer += Time.deltaTime;
        //if (timer >= 7)
        if (timer >= randomTime)
            animator.SetBool("isPatrolling", false);


        //Persegue jogador com base na distância
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
        {
            //Debug.Log("ativar caçada");
            animator.SetBool("isChasing", true);
        }
        
        //Debug.Log(distance);
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
