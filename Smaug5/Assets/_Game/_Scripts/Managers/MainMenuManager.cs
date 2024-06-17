using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Cursor:")] 
    [SerializeField] private Texture2D cursorTexture;

    // Instância da Classe
    public static MainMenuManager Instance;

    // Referências dos Menus que serão manipulados
    public static GameObject MainMenu, OptionsMenu, ControlsMenu, CreditsMenu;

    // Efeitos Sonoros Botões
    private int _curBtnSFXindex = 1;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        Instance = this;
        Init();
        SetCursor();
    }

    private void Start() => Cursor.visible = true;
    #endregion

    #region Funções Próprias
    // Coletando os objetos necessários
    private void Init()
    {
        var menuCanvas = GameObject.Find("MainMenu Canvas");
        MainMenu = menuCanvas.transform.Find("MainMenu").gameObject;
        OptionsMenu = menuCanvas.transform.Find("OptionsMenu").gameObject;
        ControlsMenu = menuCanvas.transform.Find("ControlsMenu").gameObject;
        CreditsMenu = menuCanvas.transform.Find("CreditsMenu").gameObject;
    }

    // Abre o menu desejado e fecha o que foi utilizado anteriormente
    public void OpenMenu(Default menu, GameObject callingMenu)
    {
        // Ativando o menu selecionado
        switch (menu)
        {
            case Default.MainMenu:
                MainMenu.SetActive(true);
                break;

            case Default.Options:
                OptionsMenu.SetActive(true);
                break;

            case Default.Controls:
                ControlsMenu.SetActive(true);
                break;

            case Default.Credits:
                CreditsMenu.SetActive(true);
                break;
        }

        // Desativando o anterior
        callingMenu.SetActive(false);
    }

    public void PlayBtnSFX()
    {
        AudioManager.Instance.PlaySFX("menu confirm " + _curBtnSFXindex);

        if (_curBtnSFXindex == 1) _curBtnSFXindex = 2;
        else _curBtnSFXindex = 1;
    }

    private void SetCursor()
    {
        var cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion
}
