using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Vari�veis Globais
    // Unity Inspector:
    [Header("Configura��es:")]

    [Header("Transi��o:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float loadTime;
    #endregion

    #region Fun��es Pr�prias
    public void StartGame() => TransitionManager.Instance().Transition(SceneManager.GetActiveScene().name, transitionSettings, loadTime);

    public void GoToOptions() => MainMenuManager.Instance.OpenMenu(Default.Options, MainMenuManager.MainMenu);

    public void GoToControls() => MainMenuManager.Instance.OpenMenu(Default.Controls, MainMenuManager.MainMenu);

    public void GoToCredits() => MainMenuManager.Instance.OpenMenu(Default.Credits, MainMenuManager.MainMenu);

    public void ExitGame() => Application.Quit();
    #endregion
}
