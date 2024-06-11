using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class OpenBook : MonoBehaviour
{
    #region Variáveis Globais
    // Unity Inspector:
    [Header("CrossHair:")] 
    [SerializeField] private GameObject crossHairUI;

    [Header("Botões:")]
    [SerializeField] private Button openBtn;

    [Header("Game Objects:")]
    [SerializeField] private GameObject openedBook;
    [SerializeField] private GameObject insideBackCover;

    [Header("Scripts:")] 
    [SerializeField] private PageEditor pageEditorScript;

    [Header("Textos:")]
    [SerializeField] private TextMeshProUGUI[] tempTexts;

    private Vector3 _rotation;

    private bool _isOpenClicked = false;
    private bool _isCloseClicked = false;

    private DateTime _startTime;
    private DateTime _endTime;

    [HideInInspector] public int LastPageIndex = 0;
    #endregion

    #region Funções Unity
    private void Start()
    {
        /*
        if (openBtn != null)
            openBtn.onClick.AddListener(() => ClickOpen());
        */

        Invoke("ClickOpen", 0.75f);
    }

    private void Update()
    {
        if (_isOpenClicked || _isCloseClicked)
        {
            transform.Rotate(_rotation * Time.deltaTime * 0.8f);
            _endTime = DateTime.Now;

            if (_isOpenClicked)
            {
                if ((_endTime - _startTime).TotalSeconds >= 1)
                {
                    for (int i = 0; i < tempTexts.Length; i++)
                        tempTexts[i].transform.parent.gameObject.SetActive(false);

                    _isOpenClicked = false;
                    openedBook.SetActive(true);
                    insideBackCover.SetActive(false);
                    gameObject.SetActive(false);
                }
            }

            if (_isCloseClicked)
            {
                if ((_endTime - _startTime).TotalSeconds >= 1)
                {
                    _isCloseClicked = false;

                    for (int i = 0; i < tempTexts.Length; i++)
                        tempTexts[i].transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnDisable()
    {
        LastPageIndex = 0;
        for (int i = 0; i < tempTexts.Length; i++)
            tempTexts[i].text = pageEditorScript.BookPages[LastPageIndex].Texts[i];
    }
    #endregion

    #region Funções Próprias
    private void ClickOpen()
    {
        _isOpenClicked = true;
        _startTime = DateTime.Now;
        _rotation = new Vector3(0, 180, 0);

        for (int i = 0; i < tempTexts.Length; i++)
        {
            tempTexts[i].transform.parent.gameObject.SetActive(true);
            tempTexts[i].text = pageEditorScript.BookPages[LastPageIndex].Texts[i];
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("book open");
    }

    public void ClickClose()
    {
        gameObject.SetActive(true);
        openedBook.SetActive(false);
        insideBackCover.SetActive(true);

        _isCloseClicked = true;
        _startTime = DateTime.Now;
        _rotation = new Vector3(0, -180, 0);

        for (int i = 0; i < tempTexts.Length; i++)
        {
            tempTexts[i].transform.parent.gameObject.SetActive(true);
            tempTexts[i].text = pageEditorScript.BookPages[LastPageIndex].Texts[i];
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("book close");

        Invoke("CloseBook", 0.5f);
    }

    private void CloseBook()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        crossHairUI.SetActive(false);
        Camera.main.enabled = true;
        gameObject.SetActive(false);
    }
    #endregion
}
