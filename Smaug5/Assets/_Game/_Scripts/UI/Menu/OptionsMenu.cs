using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações:")]
    
    [Header("Referências:")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI txtBtnFullscren;
    #endregion

    #region Funções Unity
    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
            LoadVolume();
        else
            SetMasterVolume();
    }
    #endregion

    #region Funções Próprias
    public void SetMasterVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");

        SetMasterVolume();
    }

    public void GoBackToMainMenu() => MainMenuManager.Instance.OpenMenu(Default.MainMenu, MainMenuManager.OptionsMenu);

    public void GoBackToGameMenu() => GameMenuManager.Instance.OpenMenu(InGame.GameMenu, GameMenuManager.OptionsMenu);

    public void SetFullScreen()
    {
        if (!Screen.fullScreen)
        {
            Screen.fullScreen = true;
            txtBtnFullscren.text = "X";
        }
        else
        {
            Screen.fullScreen = false;
            txtBtnFullscren.text = " ";
        }
    }
    #endregion
}
