using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToOptions() => MainMenuManager.Instance.OpenMenu(Default.Options, MainMenuManager.MainMenu);

    public void GoToControls() => MainMenuManager.Instance.OpenMenu(Default.Controls, MainMenuManager.MainMenu);

    public void GoToCredits() => MainMenuManager.Instance.OpenMenu(Default.Credits, MainMenuManager.MainMenu);

    public void ExitGame() => Application.Quit();
}
