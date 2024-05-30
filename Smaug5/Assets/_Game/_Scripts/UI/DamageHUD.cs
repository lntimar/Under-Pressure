using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageHUD : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Referências:")]
    [SerializeField] private PostProcessVolume postVolumeEffect;

    // Referência PlayerStats
    private PlayerStats _playerStatsScript;

    // Referência Vinheta
    private Vignette _vignetteEffect;

    // Valor máximo da intensidade da vinheta
    private float _maxIntensity;

    // Valor atual da intensidade da vinheta
    private float _curIntensityEffect = 0f;
    #endregion

    #region Funções Unity

    private void Awake()
    {
        _playerStatsScript = FindObjectOfType<PlayerStats>();

        // Pegando Referência da Vinheta
        postVolumeEffect.profile.TryGetSettings<Vignette>(out _vignetteEffect);

        // Alocando o valor máximo de intensidade
        _maxIntensity = _vignetteEffect.intensity; 

        _vignetteEffect.intensity.Override(0f);
    } 
    #endregion

    #region Funções Próprias
    private IEnumerator SetDamageEffect()
    {
        var healthRatio = (float)PlayerStats.Health / (float)_playerStatsScript.MaxHealth;

        var newIntensity = (_maxIntensity * (1f - healthRatio)) * 8f;

        // Suma com o efeito
        while (_curIntensityEffect < newIntensity)
        {
            _curIntensityEffect += 0.01f;
            _vignetteEffect.intensity.Override(_curIntensityEffect);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Change() => StartCoroutine(SetDamageEffect());
    #endregion
}
