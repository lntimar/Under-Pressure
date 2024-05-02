using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadBob : MonoBehaviour
{
    #region Variáveis 
    // Unity Inspector:
    [SerializeField, Range(0.001f, 0.01f)] private float Amount = 0.002f;
    [SerializeField, Range(1f, 30f)] private float Frequency = 10.0f;

    [SerializeField, Range(10f, 100f)] private float Smooth = 10.0f;

    private Vector3 _startPos;

    private bool _canMove = true;
    #endregion

    #region Funções Unity
    private void Start() => _startPos = transform.localPosition;

    private void Update()
    {
        if (_canMove)
        {
            print("Teste");
            CheckForHeadbobTrigger();
            StopHeadbob();
        }
    }
    #endregion

    #region Funções Próprias
    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }

    private void StopHeadbob()
    {
        if (transform.localPosition == _startPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, 1 * Time.deltaTime);
    }

    public void Stop()
    {
        _canMove = false;
        transform.localPosition = _startPos;
    }

    public void Enable() => _canMove = true;

    public bool IsActive()
    {
        return _canMove;
    }
    #endregion
}