using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine.UI;

public class FlipPage : MonoBehaviour
{
    #region Variáveis Globais
    private enum ButtonType
    {
        Next,
        Previous
    }

    // Unity Inspector:
    [Header("Referências:")]

    [Header("Botões:")]
    [SerializeField] private Button previousBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button closeBtn;

    [Header("Textos:")]
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private TextMeshProUGUI[] textsFlipPages;

    [Header("Scripts:")]
    [SerializeField] private OpenBook openBookScript;
    [SerializeField] private PageEditor pageEditorScript;

    private Vector3 _rotation;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private bool _isClicked = false;

    private DateTime _startTime;
    private DateTime _endTime;

    private float _textsDefaultAlpha;

    private int _curPageIndex = 0;
    #endregion

    #region Funções Unity
    private void Start()
    {
        previousBtn.interactable = true;
        nextBtn.interactable = true;
        closeBtn.interactable = true;

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if (previousBtn != null)
            previousBtn.onClick.AddListener(() => TurnOnePage(ButtonType.Previous));

        if (nextBtn != null)
            nextBtn.onClick.AddListener(() => TurnOnePage(ButtonType.Next));

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() => ClickCloseBook());

        _textsDefaultAlpha = texts[0].color.a;

        for (int i = 0; i < texts.Length; i++)
            texts[i].text = pageEditorScript.BookPages[_curPageIndex].Texts[i];
    }

    private void Update()
    {
        PageClicked();

        VerifyMoveButtons();
    }
    #endregion

    #region Funções Próprias
    private void TurnOnePage(ButtonType type)
    {
        if (type == ButtonType.Previous)
        {
            //var newRotation = new Vector3(_startRotation.x, 180, _startRotation.z);
            //transform.rotation = Quaternion.Euler(newRotation);
            _curPageIndex--;

            _rotation = new Vector3(0, -180, 0);

            StartCoroutine(TempChangePage(type, 0.02f));
            StartCoroutine(TempChangePage(ButtonType.Next, 1f)); // Ajuste

            textsFlipPages[0].text = pageEditorScript.BookPages[_curPageIndex + 1].Texts[0];
            textsFlipPages[1].text = pageEditorScript.BookPages[_curPageIndex].Texts[1];
        }
        else // Next
        {
            _curPageIndex++;
            _rotation = new Vector3(0, 180, 0);

            StartCoroutine(TempChangePage(type, 1f));

            textsFlipPages[0].text = pageEditorScript.BookPages[_curPageIndex].Texts[0];
            textsFlipPages[1].text = pageEditorScript.BookPages[_curPageIndex - 1].Texts[1];
        }

        

        _isClicked = true;
        _startTime = DateTime.Now;
    }

    private void ClickCloseBook()
    {
        string[] texts = new string[2];
        for (int i = 0; i < texts.Length; i++)
            texts[i] = pageEditorScript.BookPages[_curPageIndex].Texts[i];

        openBookScript.LastPageIndex = _curPageIndex;

        openBookScript.ClickClose();
    }
    private void PageClicked()
    {
        if (_isClicked)
        {
            previousBtn.interactable = false;
            nextBtn.interactable = false;
            closeBtn.interactable = false;

            for (int i = 0; i < texts.Length; i++)
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 1f);

            transform.Rotate(_rotation * Time.deltaTime);
            _endTime = DateTime.Now;

            if ((_endTime - _startTime).TotalSeconds >= 2)
            {
                previousBtn.interactable = true;
                nextBtn.interactable = true;
                closeBtn.interactable = true;

                _isClicked = false;
                transform.rotation = _startRotation;
                transform.position = _startPosition;

                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, _textsDefaultAlpha);
                    texts[i].text = pageEditorScript.BookPages[_curPageIndex].Texts[i];
                }
            }
        }
    }

    private void VerifyMoveButtons()
    {
        if (_curPageIndex == 0)
            previousBtn.gameObject.SetActive(false);
        else
            previousBtn.gameObject.SetActive(true);

        if (_curPageIndex == pageEditorScript.BookPages.Length - 1)
            nextBtn.gameObject.SetActive(false);
        else
            nextBtn.gameObject.SetActive(true);
    }

    private IEnumerator TempChangePage(ButtonType type, float t)
    {
        yield return new WaitForSeconds(t);
        if (type == ButtonType.Previous)
            texts[0].text = pageEditorScript.BookPages[_curPageIndex].Texts[0];
        else
            texts[1].text = pageEditorScript.BookPages[_curPageIndex].Texts[1];
    }
    #endregion
}
