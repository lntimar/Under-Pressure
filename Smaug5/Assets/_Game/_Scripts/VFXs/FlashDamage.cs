using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FlashDamage : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Configurações:")]
    [SerializeField,Range(0f, 1f)] private float effectTime;

    [Header("Referências:")]
    [SerializeField] private PostProcessVolume postVolume;

    // Referência Vinheta
    private Vignette _vignette;

    // Valor máximo da intensidade da vinheta
    private float _maxIntensity;

    // Valor atual da intensidade da vinheta
    private float _curIntensity = 0f;
    #endregion

    #region Funções Unity

    private void Start()
    {
        postVolume.profile.TryGetSettings<Vignette>(out _vignette); // Pegando a referência da config da vinheta
        _maxIntensity = _vignette.intensity; // Alocando o valor máximo de intensidade
    } 
    #endregion

    #region Funções Próprias
    private IEnumerator SetDamageEffect()
    {
        _curIntensity = _maxIntensity;
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(_curIntensity);

        yield return new WaitForSeconds(effectTime); // Espere para sumir com o efeito

        // Suma com o efeito
        while (_curIntensity > 0)
        {
            _curIntensity -= 0.01f;

            if (_curIntensity < 0) _curIntensity = 0;

            _vignette.intensity.Override(_curIntensity);

            yield return new WaitForSeconds(0.1f);
        }

        // Desative a vinheta
        _vignette.enabled.Override(false);
    }

    public void Apply() => StartCoroutine(SetDamageEffect());
    #endregion
}
