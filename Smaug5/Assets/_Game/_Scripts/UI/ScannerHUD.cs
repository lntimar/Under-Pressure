using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannerHUD : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private int ammoMaxIndex = 4;
    [SerializeField] private int sonarEnergyMaxIndex = 3;

    [Header("Referências:")]
    [SerializeField] private TextMeshProUGUI txtLife;
    [SerializeField] private MeshRenderer[] ammoBars;
    [SerializeField] private MeshRenderer[] sonarEnergyBars;
    [SerializeField] private PlayerStats playerStats;

    [Header("Luz Barras:")]
    [SerializeField] private Material lightMaterialAmmo;
    [SerializeField] private Material unlightMaterialAmmo;
    [SerializeField] private Material lightMaterialSonar;
    [SerializeField] private Material unlightMaterialSonar;
    #endregion

    #region Funções Próprias
    public void SetLifeText(int value) => txtLife.text = value.ToString();

    public void SetAmmoBar(int value) 
    {
        var targetIndex = value;

        for (int i = 0; i < ammoMaxIndex; i++) 
        {
            if (i <= targetIndex)
                ammoBars[i].sharedMaterial = lightMaterialAmmo;
            else
                ammoBars[i].sharedMaterial = unlightMaterialAmmo;
        }
    }

    public void SetSonarEnergyBar() 
    {
        var targetIndex = PlayerStats.Souls;

        for (int i = 0; i < sonarEnergyMaxIndex; i++) 
        {
            if (i <= targetIndex)
                sonarEnergyBars[i].sharedMaterial = lightMaterialSonar;
            else
                sonarEnergyBars[i].sharedMaterial = unlightMaterialSonar;
        }
    }
    #endregion
}
