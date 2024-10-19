using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannerHUD : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private int lifeMaxIndex = 4;
    [SerializeField] private int sonarEnergyMaxIndex = 3;

    [Header("Referências:")]
    [SerializeField] private TextMeshProUGUI txtAmmo;
    [SerializeField] private Image[] imgsLife;
    [SerializeField] private Image[] imgsSonarEnergy;
    [SerializeField] private PlayerStats playerStats;
    #endregion

    #region Funções Próprias
    public void SetAmmoText(int value) => txtAmmo.text = value.ToString();

    public void SetHealthBar() 
    {
        var maxIndex = (PlayerStats.Health * lifeMaxIndex / playerStats.MaxHealth) - 1;

        for (int i = 0; i < lifeMaxIndex; i++) 
        {
            if (i <= maxIndex)
                imgsLife[i].enabled = true;
            else
                imgsLife[i].enabled = false;
        }
    }

    public void SetSonarEnergyBar() 
    {
        var maxIndex = PlayerStats.Souls;

        for (int i = 0; i < sonarEnergyMaxIndex; i++) 
        {
            if (i <= maxIndex)
                imgsSonarEnergy[i].enabled = true;
            else
                imgsSonarEnergy[i].enabled = false;
        }
    }
    #endregion
}
