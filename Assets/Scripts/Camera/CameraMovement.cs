using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    private Camera _mainCamera;

    
    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        InputManager.OnMove += HandleMove;
    }

    private void OnDestroy()
    {
        InputManager.OnTouch -= HandleMove;
    }

    private void HandleMove(Vector2 moveVector)
    {
        _mainCamera.gameObject.transform.position += (Vector3)moveVector;
    }
}
