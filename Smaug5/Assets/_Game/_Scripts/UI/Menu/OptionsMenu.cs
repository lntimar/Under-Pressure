using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Configurações:")]
    // Inspector:
    [Header("Referências:")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    #endregion

    #region Funções Unity
    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
            LoadVolume();
        else
            SetMasterVolume();

        volumeSlider.value = 1f;
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
    #endregion
}
