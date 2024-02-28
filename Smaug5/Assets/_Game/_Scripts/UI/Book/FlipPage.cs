using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine.UI;

public class FlipPage : MonoBehaviour
{
    private enum ButtonType
    {
        Next,
        Previous
    }

    [SerializeField] private Button previousBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private OpenBook openBookScript;

    private Vector3 _rotation;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private bool _isClicked = false;

    private DateTime _startTime;
    private DateTime _endTime;

    private float _textsDefaultAlpha;

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if (previousBtn != null)
            previousBtn.onClick.AddListener(() => TurnOnePage(ButtonType.Previous));

        if (nextBtn != null)
            nextBtn.onClick.AddListener(() => TurnOnePage(ButtonType.Next));

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() => ClickCloseBook());

        _textsDefaultAlpha = texts[0].color.a;
    }

    private void Update()
    {
        if (_isClicked)
        {
            for (int i = 0; i < texts.Length; i++)
                texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 1f);

            transform.Rotate(_rotation * Time.deltaTime);
            _endTime = DateTime.Now;

            if ((_endTime - _startTime).TotalSeconds >= 2)
            {
                _isClicked = false;
                transform.rotation = _startRotation;
                transform.position = _startPosition;

                for (int i = 0; i < texts.Length; i++)
                    texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, _textsDefaultAlpha);
            }
        }
    }

    private void TurnOnePage(ButtonType type)
    {
        if (type == ButtonType.Previous)
        {
            //var newRotation = new Vector3(_startRotation.x, 180, _startRotation.z);
            //transform.rotation = Quaternion.Euler(newRotation);
            _rotation = new Vector3(0, -180, 0);
        }
        else // Next
        {
            _rotation = new Vector3(0, 180, 0);
        }

        _isClicked = true;
        _startTime = DateTime.Now;
    }

    private void ClickCloseBook()
    {
        openBookScript.ClickClose();
        print("Foi!");
    }
}
