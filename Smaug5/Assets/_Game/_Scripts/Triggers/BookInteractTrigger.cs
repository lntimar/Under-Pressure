using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class BookInteractTrigger : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Refer�ncias:")]
    [SerializeField] private Outline outlineEffect;
    [SerializeField] private GameObject book;

    [HideInInspector] public bool CanInteract = false;

    private GameObject _crossHairUI;
    #endregion

    #region Fun��es Unity
    private void Awake() => _crossHairUI = GameObject.FindGameObjectWithTag("CrossHairUI");

    private void Update()
    {
        if (CanInteract)
        {
            outlineEffect.eraseRenderer = false;

            if (Input.GetKeyDown(KeyCode.E) && !book.active)
                Open();
        }
        else
        {
            outlineEffect.eraseRenderer = true;
        }
    }
    #endregion

    private void Open()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _crossHairUI.SetActive(false);
        Camera.main.enabled = false;
        book.SetActive(true);
    }
}
