using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public void GoBackToMainMenu() => MainMenuManager.Instance.OpenMenu(Default.MainMenu, MainMenuManager.MainMenu);
}
