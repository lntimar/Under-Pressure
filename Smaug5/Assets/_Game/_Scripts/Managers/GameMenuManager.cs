using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    #region Variáveis Globais
    // Instância da Classe
    public static GameMenuManager Instance;

    // Referências dos Menus que serão manipulados
    public static GameObject GameMenu, OptionsMenu, ControlsMenu;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        Instance = this;
        Init();
    }
    #endregion

    #region Funções Próprias
    // Coletando os objetos necessários
    private void Init()
    {
        var menuCanvas = GameObject.Find("GameMenu Canvas");
        GameMenu = menuCanvas.transform.Find("GameMenu").gameObject;
        OptionsMenu = menuCanvas.transform.Find("OptionsMenu").gameObject;
        ControlsMenu = menuCanvas.transform.Find("ControlsMenu").gameObject;
    }

    // Abre o menu desejado e fecha o que foi utilizado anteriormente
    public void OpenMenu(InGame menu, GameObject callingMenu)
    {
        // Ativando o menu selecionado
        switch (menu)
        {
            case InGame.GameMenu:
                GameMenu.SetActive(true);
                break;

            case InGame.Options:
                OptionsMenu.SetActive(true);
                break;

            case InGame.Controls:
                ControlsMenu.SetActive(true);
                break;
        }

        // Desativando o anterior
        callingMenu.SetActive(false);
    }
    #endregion
}
