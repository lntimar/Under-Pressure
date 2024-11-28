using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Configurações:")]

    [Header("Estatísticas do Inimigo")]
    public int MaxHealth = 100;
    public int CurrentHealth;
    public static int Damage = 25;
    public float destroyTime = 1.25f;

    [Header("Referências")]
    public GameObject soulPrefab;
    public GameObject lifePrefab;
    public Animator animator;
    private PatrolState patrolState;
    [SerializeField] private Transform orbSpawnPoint;
    
    // Referências
    Transform player;
    public static SonarScript SonarScript;

    private float distance;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        if (PlayerStats.HasGun)
        {
            SonarScript = FindObjectOfType<SonarScript>();
        }
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolState = animator.GetBehaviour<PatrolState>();

    } 

    private void Update()
    {
        distance = Vector3.Distance(player.position, animator.transform.position);
    }

    #endregion

    #region Funções Próprias
    public void ChangeHealthPoints(int points)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= points;
            if (gameObject.CompareTag("Enemy"))
            {
                if (distance > patrolState.chaseRange)
                {
                    animator.SetTrigger("getShot");
                }
            }
            //Debug.Log("Dano");
        }

        if (CurrentHealth <= 0)
        {
            if (animator != null) 
            {
                animator.enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                if (gameObject.CompareTag("Enemy"))
                {
                    GetComponent<NewEnemyBehaviour>().enabled = false;
                    GetComponent<NavMeshAgent>().enabled = false;
                }
            }

            Invoke("DestroyEnemy", destroyTime);
        }
    }

    public void DestroyEnemy()
    {
        // Caso a Vida for maximizada, dropar energia do sonar mais frequentemente
        if (PlayerStats.Health == PlayerStats.MaxHealthValue)
        {
            if (Random.Range(0, 100) < 25)
                Instantiate(lifePrefab, orbSpawnPoint.position + Vector3.up * 2.25f, Quaternion.identity);
            else
                Instantiate(soulPrefab, orbSpawnPoint.position + Vector3.up * 2.25f, Quaternion.identity);
        } // Senão, dropar vida mais frequentemente
        else
        {
            if (Random.Range(0, 100) < 25)
                Instantiate(soulPrefab, orbSpawnPoint.position + Vector3.up * 2.25f, Quaternion.identity);
            else
                Instantiate(lifePrefab, orbSpawnPoint.position + Vector3.up * 2.25f, Quaternion.identity);
        }

        /*if (SonarScript.affectedObjects.Contains(gameObject))
        {
            SonarScript.affectedObjects.Remove(gameObject);
        }*/

        Invoke("SelfDestruct", 0.2f);
    }

    private void SelfDestruct() => Destroy(gameObject);
    #endregion
}
