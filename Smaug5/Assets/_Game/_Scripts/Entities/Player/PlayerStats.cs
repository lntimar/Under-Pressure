using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Estatísticas do Jogador:")]
    public int MaxHealth = 100;
    public static int Souls = 0;

    [Header("Orbe:")]
    public Material OrbLightMaterial;
    public Color EmissionColor;
    [Range(0f, 10f)] public float emissionModifier = 0.7f;

    // Referências:
    private DamageHUD _damageScript;

    // Componentes:
    private PlayerProgress _playerProgress;

    // Arma
    public static bool HasGun = true;

    // Orbe
    private static float _curEmissionIntensity = 0f;

    public static int Health = 0;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        _damageScript = FindObjectOfType<DamageHUD>();

        _playerProgress = GetComponent<PlayerProgress>();

        if (Health == 0)
        {
            ChangeHealthPoints(MaxHealth);
            _curEmissionIntensity = 0f;
            ChangeOrbSouls();
        }
        else
        {
            ChangeHealthPoints();
            ChangeOrbSouls();
        }
    }
    #endregion

    #region Funções Próprias
    public void ChangeHealthPoints(int points=0)
    {
        Health = Mathf.Clamp(Health + points, 0, MaxHealth);

        _damageScript.Change();

        if (Health == 0)
            _playerProgress.Restart();
    }

    public void ChangeOrbSouls(int count=0)
    {
        if (Souls + count < 0)
        {
            Souls = 0;
            _curEmissionIntensity = 0;
        }
        else
        {
            Souls += count;
            _curEmissionIntensity += emissionModifier * count;
        }
        
        OrbLightMaterial.SetVector("_EmissionColor", EmissionColor * _curEmissionIntensity);
    }
    #endregion
}
