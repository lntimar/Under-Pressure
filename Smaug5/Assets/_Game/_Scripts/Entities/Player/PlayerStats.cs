using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Estatísticas do Jogador")]
    public int Health = 100;
    public int MaxHealth = 100;
    public int Souls = 0;
    //public float speed = 12f;
    //public int shield = 100;
    //public float armor = 5f;

    // Referências:
    private DamageHUD _damageScript;

    // Componentes:
    private PlayerProgress _playerProgress;

    public static bool HasGun = true;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        _playerProgress = GetComponent<PlayerProgress>();
        _damageScript = FindObjectOfType<DamageHUD>();
    }
    #endregion

    #region Funções Próprias
    public void ChangeHealthPoints(int points)
    {
        Health = Mathf.Clamp(Health + points, 0, MaxHealth);

        _damageScript.Change();

        if (Health == 0)
            _playerProgress.Restart();
    }
    #endregion
}
