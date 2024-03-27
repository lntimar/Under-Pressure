using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    #region Vari�veis Globais
    // Inst�ncia da Classe
    public static GameMenuManager Instance;

    // Refer�ncias dos Menus que ser�o manipulados
    public GameObject GameMenu, OptionsMenu, ControlsMenu;
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        Instance = this;
        Init();
    }
    #endregion

    #region Fun��es Pr�prias
    // Coletando os objetos necess�rios
    private void Init()
    {
        var menuCanvas = GameObject.Find("GameMenu Canvas");
        GameMenu = menuCanvas.transform.Find("GameMenu").gameObject;
        OptionsMenu = menuCanvas.transform.Find("OptionsMenu").gameObject;
        ControlsMenu = menuCanvas.transform.Find("ControlsMenu").gameObject;
    }

    // Abre o menu desejado e fecha o que foi utilizado anteriormente
    public static void OpenMenu(InGame menu, GameObject callingMenu)
    {
        // Ativando o menu selecionado
        switch (menu)
        {
            case InGame.GameMenu:
                Instance.GameMenu.SetActive(true);
                break;

            case InGame.Options:
                Instance.OptionsMenu.SetActive(true);
                break;

            case InGame.Controls:
                Instance.ControlsMenu.SetActive(true);
                break;
        }

        // Desativando o anterior
        callingMenu.SetActive(false);
    }
    #endregion
}
