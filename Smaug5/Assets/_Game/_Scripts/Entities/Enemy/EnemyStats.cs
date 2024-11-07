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
    public Animator animator;
    
    // Referências
    private SonarScript _sonarScript;
    #endregion

    #region Funções Unity
    private void Awake() => _sonarScript = FindObjectOfType<SonarScript>();

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
        Instantiate(soulPrefab, transform.position + Vector3.up * 2.25f, Quaternion.identity);
        _sonarScript.affectedObjects.Remove(gameObject);
        Invoke("SelfDestruct", 0.05f);
    }

    private void SelfDestruct() => Destroy(gameObject);
    #endregion
}
