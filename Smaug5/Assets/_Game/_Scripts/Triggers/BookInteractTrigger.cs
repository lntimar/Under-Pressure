using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class BookInteractTrigger : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Referências:")]
    [SerializeField] private Outline outlineEffect;
    [SerializeField] private GameObject book;

    [HideInInspector] public bool CanInteract = false;

    // Referências:
    private GameObject _crossHairUI;
    private Rigidbody _playerRb;
    #endregion

    #region Funções Unity

    private void Awake()
    {
        _crossHairUI = GameObject.FindGameObjectWithTag("CrossHairUI");
        _playerRb = FindObjectOfType<PlayerMove>().gameObject.GetComponent<Rigidbody>();
    } 

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
        //FindObjectOfType<Weapon>().enabled = false;
        //FindObjectOfType<SonarScript>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _crossHairUI.SetActive(false);
        Camera.main.enabled = false;
        book.SetActive(true);
    }
}
