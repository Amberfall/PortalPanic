using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem2D : MonoBehaviour
{
    // [SerializeField] private PolygonCollider2D _cameraConfiner2D;
    [SerializeField] private float _camMoveSpeed = 10f;
    [SerializeField] private float _camMoveSpeedShiftMultiplier = 2f;
    // [SerializeField] private bool _useEdgeScrolling = false;
    [SerializeField] private bool _useDragPan = false;
    [SerializeField] private float _orthographicSizeMin = 10f;
    [SerializeField] private float _orthographicSizeMax = 18f;
    [SerializeField] private float _targetOrthographicSize = 10f;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private bool _dragPanMoveActive;
    private Vector2 _lastMousePosition;
    private Camera _cam;
    private float _startingMoveSpeed;

    private float _mapMinX, _mapMaxX, _mapMinY, _mapMaxY;

    private void Awake()
    {
        _cam = Camera.main;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        // _mapMinX = _cameraConfiner2D.bounds.center.x - _cameraConfiner2D.bounds.size.x / 2;
        // _mapMaxX = _cameraConfiner2D.bounds.center.x + _cameraConfiner2D.bounds.size.x / 2;

        // _mapMinY = _cameraConfiner2D.bounds.center.y - _cameraConfiner2D.bounds.size.y / 2;
        // _mapMaxY = _cameraConfiner2D.bounds.center.y + _cameraConfiner2D.bounds.size.y / 2;
    }

    private void Start()
    {
        _startingMoveSpeed = _camMoveSpeed;
    }

    private void Update()
    {
        // if (_useEdgeScrolling)
        // {
        //     HandleCameraMovementEdgeScrolling();
        // }

        if (_useDragPan)
        {
            HandleCameraMovementDragPan();
        }

        HandleCameraZoom_OrthographicSize();
    }

    private void FixedUpdate()
    {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.y = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.y = -1f;
        // if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        // if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        // Vector3 moveVector = transform.up * inputDir.y + transform.right * inputDir.x;
        Vector3 moveVector = transform.up * inputDir.y;

        float currentMoveSpeed = _camMoveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed *= _camMoveSpeedShiftMultiplier;
        }

        Vector3 targetPosition = transform.position + moveVector * currentMoveSpeed * Time.deltaTime;

        // transform.position = ClampCamera(targetPosition);
        transform.position = targetPosition;
    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize)
        {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollSize)
        {
            inputDir.y = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollSize)
        {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollSize)
        {
            inputDir.y = +1f;
        }

        Vector3 moveVector = transform.up * inputDir.y + transform.right * inputDir.x;

        transform.position += moveVector * _camMoveSpeed * Time.deltaTime;

        // transform.position = ClampCamera(transform.position);
    }

    private void HandleCameraMovementDragPan()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(2))
        {
            _dragPanMoveActive = true;
            _lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2))
        {
            _dragPanMoveActive = false;
        }

        if (_dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - _lastMousePosition;

            float dragPanSpeed = 1f;
            inputDir.x = -mouseMovementDelta.x * dragPanSpeed;
            inputDir.y = -mouseMovementDelta.y * dragPanSpeed;

            _lastMousePosition = Input.mousePosition;
        }

        Vector3 moveVector = transform.up * inputDir.y;

        transform.position += moveVector * _camMoveSpeed * Time.deltaTime;

        // transform.position = ClampCamera(transform.position);
    }

    private void HandleCameraZoom_OrthographicSize()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetOrthographicSize -= 1;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            _targetOrthographicSize += 1;
        }

        _targetOrthographicSize = Mathf.Clamp(_targetOrthographicSize, _orthographicSizeMin, _orthographicSizeMax);

        float zoomSpeed = 10f;

        _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(_cinemachineVirtualCamera.m_Lens.OrthographicSize, _targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float camWidth = _cinemachineVirtualCamera.m_Lens.OrthographicSize * _cam.aspect;

        float minX = _mapMinX + camWidth;
        float maxX = _mapMaxX - camWidth;
        float minY = _mapMinY + camHeight;
        float maxY = _mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}