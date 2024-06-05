using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações")]
    
    [Header("Cursor:")]
    [SerializeField] private Texture2D cursorTexture;

    [Header("Transição:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float loadTime;

    private bool _gamePaused;
    #endregion

    #region Funções Unity
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_gamePaused)
                Pause();
            else
                Resume();
        }
    }
    #endregion

    #region Funções Próprias
    private void Pause()
    {
        Time.timeScale = 0f;
        GameMenuManager.MenuCanvas.GetComponent<Canvas>().enabled = true;
        _gamePaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        var cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameMenuManager.MenuCanvas.GetComponent<Canvas>().enabled = false;
        _gamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoToOptions() => GameMenuManager.Instance.OpenMenu(InGame.Options, GameMenuManager.GameMenu);

    public void GoToControls() => GameMenuManager.Instance.OpenMenu(InGame.Controls, GameMenuManager.GameMenu);

    public void GoToMainMenu() 
    {
        Time.timeScale = 1f;
        TransitionManager.Instance().Transition("MainMenu", transitionSettings, loadTime);
    }
    #endregion
}
