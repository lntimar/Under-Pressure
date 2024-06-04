using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Transform _playerTransform;

    private void Awake() => _playerTransform = FindObjectOfType<PlayerMove>().transform;

    private void Update() => transform.position = _playerTransform.transform.position;
}