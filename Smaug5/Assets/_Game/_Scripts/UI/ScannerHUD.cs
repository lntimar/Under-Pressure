using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannerHUD : MonoBehaviour
{
    #region Variáveis
    [Header("Configurações:")]
    [SerializeField] private int ammoMaxIndex = 6;
    [SerializeField] private int sonarEnergyMaxIndex = 3;

    [Header("Referências:")]
    [SerializeField] private TextMeshProUGUI txtLife;
    [SerializeField] private MeshRenderer[] ammoBars;
    [SerializeField] private GameObject[] sonarEnergyBars;
    [SerializeField] private PlayerStats playerStats;

    [Header("Alerta:")] 
    [SerializeField] private float mediumDistance;
    [SerializeField] private float highDistance;
    
    [Header("Luz Barras:")]
    [SerializeField] private Material lightMaterialAmmo;
    [SerializeField] private Material unlightMaterialAmmo;

    // Referências:
    private Transform _playerTransform;
    private List<EnemyBehaviour> _enemies = new List<EnemyBehaviour>();
    #endregion

    #region Funções Unity

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerMove>().transform;

        _enemies = FindObjectsOfType<EnemyBehaviour>().ToList();
    }

    private void Start()
    {
        
    }
    #endregion
    
    #region Funções Próprias
    public void SetLifeText(int value) => txtLife.text = value.ToString();

    public void SetAmmoBar() 
    {
        for (int i = 0; i < ammoMaxIndex; i++) 
        {
            if (ammoBars[i].sharedMaterial != unlightMaterialAmmo)
            {
                ammoBars[i].sharedMaterial = unlightMaterialAmmo;
                break;
            }
        }
    }

    public void ResetAmmoBar()
    {
        for (int i = 0; i < ammoMaxIndex; i++) 
            ammoBars[i].sharedMaterial = lightMaterialAmmo;
    }

    public void SetSonarEnergyBar(int soulCount) 
    {
        for (int i = 0; i < sonarEnergyMaxIndex; i++)
        {
            if (i == soulCount - 1)
                sonarEnergyBars[i].SetActive(true);
            else
                sonarEnergyBars[i].SetActive(false);
        }
    }

    public void ResetSonarEnergyBar()
    {
        for (int i = 0; i < sonarEnergyMaxIndex; i++)
            sonarEnergyBars[i].SetActive(false);
    }
    #endregion
}
