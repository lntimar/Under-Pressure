using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class BookInteractTrigger : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Refer�ncias:")]
    [SerializeField] private Outline outlineEffect;

    [HideInInspector] public bool CanInteract = false;
    #endregion

    #region Fun��es Unity
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
