using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.NetworkInformation;
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

    private Vector3 _rotation;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private bool _isClicked;

    private DateTime _startTime;
    private DateTime _endTime;

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if (previousBtn != null)
            previousBtn.onClick.AddListener(() => TurnOnePage(ButtonType.Previous));

        if (nextBtn != null)
            nextBtn.onClick.AddListener(() => TurnOnePage(ButtonType.Next));
    }

    private void Update()
    {
        if (_isClicked)
        {
            transform.Rotate(_rotation * Time.deltaTime);
            _endTime = DateTime.Now;

            if ((_endTime - _startTime).TotalSeconds >= 1)
            {
                _isClicked = false;
                transform.rotation = _startRotation;
                transform.position = _startPosition;
            }
        }
    }

    private void TurnOnePage(ButtonType type)
    {
        _isClicked = true;
        _startTime = DateTime.Now;

        if (type == ButtonType.Previous)
        {
            var newRotation = new Vector3(_startRotation.x, -180, _startRotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
            _rotation = new Vector3(0, 180, 0);
        }
        else // Next
        {
            _rotation = new Vector3(0, 180, 0);
        }
    }
}
