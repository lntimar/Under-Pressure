using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnTransition : MonoBehaviour
{
    #region Vari�veis Globais
    // Unity Inspector:
    [Header("Configura��es:")]

    [Header("Transi��o:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float loadTime;
    #endregion

    #region Fun��es Pr�prias
    public void ApplyTransition() => TransitionManager.Instance().Transition(transitionSettings, loadTime);
    #endregion
}
