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
    [SerializeField] private TransitionSettings firstTransitionSettings;
    [SerializeField] private float firstLoadTime;
    [SerializeField] private TransitionSettings playTransitionSettings;
    [SerializeField] private float playLoadTime;

    private static bool firstTime = true;
    #endregion

    #region Fun��es Pr�prias

    private void Start()
    {
        if (firstTime)
        {
            firstTime = false;
            TransitionManager.Instance().Transition(firstTransitionSettings, firstLoadTime);
        }
    }

    public void StartGame() => TransitionManager.Instance().Transition("NPC Dialogue", playTransitionSettings, playLoadTime);

    public void GoToOptions() => MainMenuManager.Instance.OpenMenu(Default.Options, MainMenuManager.MainMenu);

    public void GoToControls() => MainMenuManager.Instance.OpenMenu(Default.Controls, MainMenuManager.MainMenu);

    public void GoToCredits() => MainMenuManager.Instance.OpenMenu(Default.Credits, MainMenuManager.MainMenu);

    public void ExitGame() => Application.Quit();
    #endregion
}
