using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetShotState : StateMachineBehaviour
{
    public float chasingSpeed;
    private float stopChaseDistance;
    public float attackRange;
    private float distance;

    private bool canPlayScream = true;

    private float screamCurInterval = 0;
    private float screamMaxInterval = 0;
    
    PatrolState patrolState;
    NavMeshAgent agent;
    Transform player;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        patrolState = animator.GetBehaviour<PatrolState>();
        if (AudioManager.Instance != null && canPlayScream)
        {
            canPlayScream = false;
            AudioManager.Instance.PlaySFX("enemy scream " + Random.Range(1, 3));
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        agent.speed = chasingSpeed;
        distance = Vector3.Distance(player.position, animator.transform.position);
        stopChaseDistance = distance + 20;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        //Para de perseguir o jogador se a distância for maior que 30
        distance = Vector3.Distance(player.position, animator.transform.position);

        //if (distance > stopChaseDistance || distance < attackRange)
        if (distance > stopChaseDistance)
        {
            Debug.Log("Fora de alcance");
            animator.SetTrigger("outOfRange");
        }

        if (distance <= patrolState.chaseRange)
        {
            Debug.Log("ChaseState");
            animator.SetBool("isChasing", true);
        }
        
        // Intervalo Grito
        if (!canPlayScream)
        {
           screamCurInterval += Time.deltaTime;
           if (screamCurInterval >= screamMaxInterval)
           {
               screamCurInterval = 0;
               canPlayScream = true;
           }
        }
        
        //Debug.Log(distance);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
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
