using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class WeaponCollectTrigger : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Referências:")]
    [SerializeField] private Outline outlineEffect;

    [HideInInspector] public bool CanInteract = false;
    #endregion

    #region Funções Unity
    private void Awake() => VerifyAlreadyCaught();

    private void Update()
    {
        if (CanInteract)
        {
            outlineEffect.eraseRenderer = false;
            if (Input.GetKeyDown(KeyCode.E))
                Get();
        }
        else
        {
            outlineEffect.eraseRenderer = true;
        }
    }
    #endregion

    #region Funções Próprias
    private void Get()
    {
        PlayerStats.HasGun = true;
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon catch");
        Destroy(transform.parent.gameObject);
        EnemyStats.SonarScript = FindObjectOfType<SonarScript>();
    }

    private void VerifyAlreadyCaught()
    {
        if (PlayerStats.HasGun)
            Destroy(transform.parent.gameObject);
    }
    #endregion
}
