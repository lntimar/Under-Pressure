using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeCameraPosition : MonoBehaviour
{
    [Header("Referências:")] 
    [SerializeField] private CameraHeadBob cameraHeadBobScript;
    [SerializeField] private SkinnedMeshRenderer playerTop;

    [Header("Posições:")] 
    [SerializeField] private Vector3[] positions;

    private int _lastIndex;

    public void ChangePos(int index)
    {
        cameraHeadBobScript.ChangeStartPos(positions[index]);

        if (index != _lastIndex)
        {
            playerTop.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            Invoke("ResetPlayerTop", 0.1f);
        }

        _lastIndex = index;
    }

    private void ResetPlayerTop() => playerTop.shadowCastingMode = ShadowCastingMode.On;
}
