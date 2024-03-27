using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMenuManager
{
    // Referências dos Menus que serão manipulados
    public static GameObject GameMenu, OptionsMenu, ControlsMenu;

    // Verificando se já temos as referências
    public static bool IsInitialised { get; private set; }

    // Coletando os objetos necessários
    public static void Init()
    {
        var menuCanvas = GameObject.Find("GameMenu Canvas");
        GameMenu = menuCanvas.transform.Find("GameMenu").gameObject;
        OptionsMenu = menuCanvas.transform.Find("OptionsMenu").gameObject;
        ControlsMenu = menuCanvas.transform.Find("ControlsMenu").gameObject;

        IsInitialised = true;
    }

    // Abre o menu desejado e fecha o que foi utilizado anteriormente
    public static void OpenMenu(InGame menu, GameObject callingMenu)
    {
        if (!IsInitialised) Init();

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

        callingMenu.SetActive(false);
    }
}
