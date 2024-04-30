using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Estatísticas do Inimigo")]
    public int MaxHealth = 100;
    public int CurrentHealth;
    public int Damage = 20;

    [Header("Referências")]
    public GameObject _soul;
    public SonarScript sonarScript;
    #endregion

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    #region Funções Próprias
    public void ChangeHealthPoints(int points)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= points;
        }

        if (CurrentHealth <= 0)
        {
            Debug.Log(gameObject.name + " is DEAD, not big surprise");
            Instantiate(_soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        sonarScript.affectedObjects.Remove(gameObject);
    }
    #endregion
}
