using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 50f;

    [Header("Map Boundaries")]
    [SerializeField] private Vector2 _mapMinBounds = new Vector2(-50, -50);
    [SerializeField] private Vector2 _mapMaxBounds = new Vector2(50, 50);

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampCameraPosition();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * vertical + right * horizontal) * _moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            float zoomAmount = scroll * _zoomSpeed;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - zoomAmount, _minZoom, _maxZoom);
        }
    }

    private void ClampCameraPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _mapMinBounds.x, _mapMaxBounds.x);
        pos.z = Mathf.Clamp(pos.z, _mapMinBounds.y, _mapMaxBounds.y);
        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((_mapMinBounds.x + _mapMaxBounds.x) / 2, 0, (_mapMinBounds.y + _mapMaxBounds.y) / 2);
        Vector3 size = new Vector3(_mapMaxBounds.x - _mapMinBounds.x, 0.1f, _mapMaxBounds.y - _mapMinBounds.y);
        Gizmos.DrawWireCube(center, size);
    }
}