using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform orbSpawnPoint;
    
    // Referências
    public static SonarScript SonarScript;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        if (PlayerStats.HasGun)
        {
            SonarScript = FindObjectOfType<SonarScript>();
        }
    }

    private void Start() => CurrentHealth = MaxHealth;
    #endregion

    #region Funções Próprias
    public void ChangeHealthPoints(int points)
    {
        if (CurrentHealth > 0)
            CurrentHealth -= points;

        if (CurrentHealth <= 0)
        {
            if (animator != null) 
            {
                animator.enabled = false;
                GetComponent<EnemyBehaviour>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
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

        Invoke("SelfDestruct", destroyTime);
    }

    private void SelfDestruct() => Destroy(gameObject);
    #endregion
}
