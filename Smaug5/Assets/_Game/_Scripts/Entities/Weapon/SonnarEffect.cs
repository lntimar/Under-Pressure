using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonnarEffect : MonoBehaviour
{
    // Inspector:
    [Header("Referências:")] 
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private Transform originalParent;
    [SerializeField] private Vector3 initialPosition;

    private void OnEnable()
    {
        transform.parent = null;
        EnableParticles();
        Invoke("DisableObject", 1.2f);
    }

    private void EnableParticles()
    {
        for (int i = 0; i < particles.Length; i++)
            particles[i].Play();
    }

    private void DisableObject()
    {
        transform.parent = originalParent;
        transform.localPosition = initialPosition;
        transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        gameObject.SetActive(false);
    }
}
