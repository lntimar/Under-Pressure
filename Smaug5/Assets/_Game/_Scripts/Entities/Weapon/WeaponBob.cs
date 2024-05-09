using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Configurações:")]

    [Header("Referências:")]
    [SerializeField] private CameraHeadBob headBob;

    [Header("Ajustes:")]
    [SerializeField, Range(0.001f, 0.01f)] private float amount;
    [SerializeField, Range(1f, 30f)] private float frequency;
    [SerializeField, Range(10f, 100f)] private float smooth;

    private Vector3 _startPos;
    #endregion

    #region Funções Unity
    private void Start() => _startPos = transform.localPosition;

    private void Update()
    {
        // Verifica se a camera pode se mover
        if (headBob.IsActive())
            CheckForMovement(); // Chama a função para verificar se o jogador está se movendo
    }
    #endregion

    #region Funções Próprias
    private void CheckForMovement()
    {
        var inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
            StartWeaponBob();
        else
            StopWeaponBob();
    }

    private void StartWeaponBob()
    {
        var pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);

        transform.localPosition += pos;
    }

    private void StopWeaponBob() => transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, smooth * Time.deltaTime);
    #endregion
}