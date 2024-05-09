using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Rendering;

public class ChangeWeaponPosition : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Referências:")] 
    [SerializeField] private Transform gunTransform;

    [Header("Posições:")]
    [SerializeField] private Vector3[] positions;

    private int _lastIndex;
    #endregion

    #region Funções Próprias
    public void ChangeGunPos(int index)
    {
        if (index != _lastIndex)
            gunTransform.localPosition = positions[index];

        _lastIndex = index;
    }
    #endregion

}
