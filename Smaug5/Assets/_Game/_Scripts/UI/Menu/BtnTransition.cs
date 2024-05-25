using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnTransition : MonoBehaviour
{
    #region Variáveis Globais
    // Unity Inspector:
    [Header("Configurações:")]

    [Header("Transição:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float loadTime;
    #endregion

    #region Funções Próprias
    public void ApplyTransition() => TransitionManager.Instance().Transition(transitionSettings, loadTime);
    #endregion
}
