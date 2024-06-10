using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class BookInteractTrigger : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Referências:")]
    [SerializeField] private Outline outlineEffect;

    [HideInInspector] public bool CanInteract = false;
    #endregion

    #region Funções Unity
    private void Update()
    {
        if (CanInteract)
        {
            outlineEffect.eraseRenderer = false;
        }
        else
        {
            outlineEffect.eraseRenderer = true;
        }
    }
    #endregion
}
