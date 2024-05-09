using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Rendering;

public class ChangeWeaponPosition : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Refer�ncias:")] 
    [SerializeField] private Transform gunTransform;

    [Header("Posi��es:")]
    [SerializeField] private Vector3[] positions;

    private int _lastIndex;
    #endregion

    #region Fun��es Pr�prias
    public void ChangeGunPos(int index)
    {
        if (index != _lastIndex)
            gunTransform.localPosition = positions[index];

        _lastIndex = index;
    }
    #endregion

}
