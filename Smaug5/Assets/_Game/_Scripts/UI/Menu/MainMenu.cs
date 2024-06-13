using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Variáveis Globais
    // Unity Inspector:
    [Header("Configurações:")]

    [Header("Primeira Cena:")]
    [SerializeField] private string firstSceneName;

    [Header("Transição:")]
    [SerializeField] private TransitionSettings firstTransitionSettings;
    [SerializeField] private float firstLoadTime;
    [SerializeField] private TransitionSettings playTransitionSettings;
    [SerializeField] private float playLoadTime;

    private static bool _firstTime = true;
    #endregion

    #region Funções Próprias

    private void Start()
    {
        if (_firstTime)
        {
            _firstTime = false;
            TransitionManager.Instance().Transition(firstTransitionSettings, firstLoadTime);
        }
    }

    public void StartGame() => TransitionManager.Instance().Transition(firstSceneName, playTransitionSettings, playLoadTime);

    public void GoToOptions() => MainMenuManager.Instance.OpenMenu(Default.Options, MainMenuManager.MainMenu);

    public void GoToControls() => MainMenuManager.Instance.OpenMenu(Default.Controls, MainMenuManager.MainMenu);

    public void GoToCredits() => MainMenuManager.Instance.OpenMenu(Default.Credits, MainMenuManager.MainMenu);

    public void ExitGame() => Application.Quit();
    #endregion
}
