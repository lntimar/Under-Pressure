using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageHUD : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Refer�ncias:")]
    [SerializeField] private PostProcessVolume postVolumeEffect;

    // Refer�ncia PlayerStats
    private PlayerStats _playerStatsScript;

    // Refer�ncia Vinheta
    private Vignette _vignetteEffect;

    // Valor m�ximo da intensidade da vinheta
    private float _maxIntensity;

    // Valor atual da intensidade da vinheta
    private float _curIntensityEffect = 0f;
    #endregion

    #region Fun��es Unity

    private void Awake()
    {
        _playerStatsScript = FindObjectOfType<PlayerStats>();

        // Pegando Refer�ncia da Vinheta
        postVolumeEffect.profile.TryGetSettings<Vignette>(out _vignetteEffect);

        // Alocando o valor m�ximo de intensidade
        _maxIntensity = _vignetteEffect.intensity; 

        _vignetteEffect.intensity.Override(0f);
    }
    #endregion

    #region Fun��es Pr�prias
    private IEnumerator SetDamageEffect()
    {
        // Calcule a propor��o atual de sa�de
        var healthRatio = (float)PlayerStats.Health / (float)_playerStatsScript.MaxHealth;

        // Determina a nova intensidade com base na propor��o de sa�de
        var newIntensity = (_maxIntensity * (1f - healthRatio)) * 8f;

        // Se a nova intensidade for menor que a atual, reduza a intensidade
        if (newIntensity < _curIntensityEffect)
        {
            // Reduz a intensidade
            while (_curIntensityEffect > newIntensity)
            {
                _curIntensityEffect -= 0.01f;
                _vignetteEffect.intensity.Override(_curIntensityEffect);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            // Aumenta a intensidade
            while (_curIntensityEffect < newIntensity)
            {
                _curIntensityEffect += 0.01f;
                _vignetteEffect.intensity.Override(_curIntensityEffect);
                yield return new WaitForSeconds(0.01f);
            }
        }

        // Ajusta a intensidade final para n�o ultrapassar o valor calculado
        _curIntensityEffect = Mathf.Clamp(_curIntensityEffect, 0, newIntensity);
        _vignetteEffect.intensity.Override(_curIntensityEffect);
    }

    public void Change()
    {
        if (_playerStatsScript != null)
            StartCoroutine(SetDamageEffect());
    }

    #endregion
}
