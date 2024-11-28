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

    [Header("Referências:")]
    [SerializeField] private ScannerHUD scannerHud;

    // Referências:
    private DamageHUD _damageScript;

    // Componentes:
    private PlayerProgress _playerProgress;

    // Arma
    public static bool HasGun = false;

    // Scanner
    public static bool HasScanner = false;
    
    // Orbe
    private static float _curEmissionIntensity = 0f;

    public static int Health = 0;

    public static int MaxHealthValue;
    #endregion

    #region Funções Unity
    private void Start()
    {
        _damageScript = FindObjectOfType<DamageHUD>();

        _playerProgress = GetComponent<PlayerProgress>();

        MaxHealthValue = MaxHealth;
        
        if (Health == 0)
        {
            ChangeHealthPoints(MaxHealth);
            _curEmissionIntensity = 0f;
            ChangeOrbSouls(0);
        }
        else
        {
            if (scannerHud != null)
                scannerHud.SetSonarEnergyBar(Souls);
            
            ChangeHealthPoints();
            ChangeOrbSouls();
        }
    }
    #endregion

    #region Funções Próprias
    public void ChangeHealthPoints(int points=0)
    {
        Health = Mathf.Clamp(Health + points, 0, MaxHealth);

        if (_damageScript != null)
            _damageScript.Change();

        if (scannerHud != null)
            scannerHud.SetLifeText(Health);

        if (Health == 0)
            _playerProgress.Restart();
    }

    public void ChangeOrbSouls(int count=0)
    {
        if (Souls + count < 0)
        {
            Souls = 0;
            _curEmissionIntensity = 0;
            scannerHud.ResetSonarEnergyBar();
        }
        else
        {
            Souls += count;
            _curEmissionIntensity += emissionModifier * count;
            
            if (scannerHud != null)
                scannerHud.SetSonarEnergyBar(Souls);
        }
        
        OrbLightMaterial.SetVector("_EmissionColor", EmissionColor * _curEmissionIntensity);
    }
    #endregion
}
