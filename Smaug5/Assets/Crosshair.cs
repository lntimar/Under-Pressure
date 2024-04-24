using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    #region Variáveis Globais
    // Referências:
    private GameObject _imgCrosshair;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        _imgCrosshair = transform.Find("Img Crosshair").gameObject;
        Show();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_imgCrosshair.active) Show();
            else Hide();
        }
    }
    #endregion

    #region Funções Próprias
    private void Show()
    {
        _imgCrosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Hide()
    {
        _imgCrosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion
}
