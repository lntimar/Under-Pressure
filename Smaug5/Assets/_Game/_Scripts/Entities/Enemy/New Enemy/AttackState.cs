using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    public float stopAttackDistance = 12f; 
    private Transform player;    
    //Índice do ataque aleatório        
    private int attackIndex;
    

    //Método chamado ao entrar no estado de ataque
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
       
        //Escolhe um ataque aleatório ao entrar no estado
        ChooseRandomAttack(animator);
    }

    //Método chamado durante o estado de ataque
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);

        //Se o jogador estiver fora do alcance, sai do estado de ataque
        if (distance > stopAttackDistance)
        {
            animator.SetBool("isAttacking", false);
        }
        else if (stateInfo.normalizedTime >= 1.0f) //Verifica se a animação terminou
        {
            // Escolhe outro ataque após a animação atual terminar
            ChooseRandomAttack(animator);
        }

        FaceTarget(animator.transform);
    }

    //Método chamado ao sair do estado de ataque
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Limpa o índice de ataque
        animator.SetInteger("attackIndex", -1);
    }

    //Método para escolher um ataque aleatório
    private void ChooseRandomAttack(Animator animator)
    {
        attackIndex = Random.Range(0, 3);
        animator.SetInteger("attackIndex", attackIndex);
    }

    private void FaceTarget(Transform enemyTransform)
    {
        //Calcula a direção para o jogador
        Vector3 direction = (player.position - enemyTransform.position).normalized;
        //Calcula a rotação necessária para olhar para o jogador (ignora o eixo Y)
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //Gradualmente ajusta a rotação para olhar para o jogador
        enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
