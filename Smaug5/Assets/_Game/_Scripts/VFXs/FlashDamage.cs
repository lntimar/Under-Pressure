using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FlashDamage : MonoBehaviour
{
    [Header("Configurações:")]
    [SerializeField] private float maxIntensity;
    [SerializeField] private PostProcessVolume postVolume;

    // Componentes:
    private Vignette _vignette;

    private float _curIntensity = 0f;

    #region Funções Unity
    private void Start()
    {
        postVolume = GetComponent<PostProcessVolume>();

        postVolume.profile.TryGetSettings<Vignette>(out _vignette);

        if (!_vignette)
            print("error, vignette empty");
        else
            _vignette.enabled.Override(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TakeDamageEffect(0.4f));
        }
    }
    #endregion

    #region Funções Próprias
    public IEnumerator TakeDamageEffect(float time)
    {
        _curIntensity = maxIntensity;
        _vignette.enabled.Override(true);
        _vignette.intensity.Override(_curIntensity);

        yield return new WaitForSeconds(time);

        while (_curIntensity > 0)
        {
            _curIntensity -= 0.01f;

            if (_curIntensity < 0) _curIntensity = 0;

            _vignette.intensity.Override(_curIntensity);

            yield return new WaitForSeconds(0.1f);
        }

        _vignette.enabled.Override(false);
        yield break;
    }
    #endregion
}
