using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeCameraPosition : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Refer�ncias:")]
    [SerializeField] private CameraHeadBob cameraHeadBobScript;
    [SerializeField] private SkinnedMeshRenderer playerTop;

    [Header("Posi��es:")]
    [SerializeField] private Vector3[] positions;

    private int _lastIndex = 0;

    private bool _isFirstTime = true;
    #endregion

    #region Fun��es Pr�prias
    public void ChangePos(int index)
    {
        cameraHeadBobScript.ChangeStartPos(positions[index]);

        if (index != _lastIndex && !_isFirstTime)
        {
            _isFirstTime = false;
            playerTop.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            Invoke("ResetPlayerTop", 0.1f);
        }

        _lastIndex = index;
    }

    private void ResetPlayerTop() => playerTop.shadowCastingMode = ShadowCastingMode.On;
    #endregion
}
