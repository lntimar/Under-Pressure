using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class WeaponCollectTrigger : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Refer�ncias:")]
    [SerializeField] private Outline outlineEffect;

    [HideInInspector] public bool CanInteract = false;
    #endregion

    #region Fun��es Unity
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

    #region Fun��es Pr�prias
    private void Get()
    {
        PlayerStats.HasGun = true;
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon catch");
        Destroy(transform.parent.gameObject);
    }

    private void VerifyAlreadyCaught()
    {
        if (PlayerStats.HasGun)
            Destroy(transform.parent.gameObject);
    }
    #endregion
}
