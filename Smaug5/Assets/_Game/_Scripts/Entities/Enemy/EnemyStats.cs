using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Estat�sticas do Inimigo")]
    public int MaxHealth = 100;
    public int CurrentHealth;
    public int Damage = 20;
    public float DestroyTime = 1.25f;

    [Header("Refer�ncias")]
    public GameObject Soul;
    public SonarScript sonarScript;
    public Animator animator;
    #endregion

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    #region Fun��es Pr�prias
    public void ChangeHealthPoints(int points)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= points;
        }

        if (CurrentHealth <= 0)
        {
            animator.Play("Enemy Death " + Random.Range(1, 3));
            Invoke("DestroyEnemy", DestroyTime);
        }
    }

    private void DestroyEnemy()
    {
        Instantiate(Soul, transform.position, Quaternion.identity);
        sonarScript.affectedObjects.Remove(gameObject);
        Destroy(gameObject);
    }
    #endregion
}
