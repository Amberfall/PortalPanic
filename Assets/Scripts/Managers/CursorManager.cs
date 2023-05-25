using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : Singleton<CursorManager>
{
    public float YValueNotAllowedZoneValue => _yValueNotAllowedZoneValue;

    public event EventHandler<OnCursorChangedEventArgs> OnCursorChanged;
    public class OnCursorChangedEventArgs : EventArgs
    {
        public CursorType _cursorType;
    }

    [SerializeField] private LayerMask _interactableLayer = new LayerMask();
    [SerializeField] private float _yValueNotAllowedZoneValue = -10f;
    [SerializeField] private List<CursorAnimation> _cursorAnimationList;

    private int _currentFrame;
    private float _frameTimer;
    private int _frameCount;
    
    private bool _disableCursor = false;

    private CursorAnimation _cursorAnimation;
    private Camera _mainCam;

    protected override void Awake() {
        base.Awake();

        _mainCam = Camera.main;
    }

    public enum CursorType
    {
        Open,
        Closed,
        NotAllowed,
        Arrow,
    }

    private void Update()
    {
        DetectCursorType();
        DisableCursorEditorOnly();

        if (_disableCursor) { return; }

        _frameTimer -= Time.deltaTime;
        if (_frameTimer <= 0f)
        {
            _frameTimer += _cursorAnimation.frameRate;
            _currentFrame = (_currentFrame + 1) % _frameCount;
            Cursor.SetCursor(_cursorAnimation.textureArray[_currentFrame], _cursorAnimation.offset, CursorMode.Auto);
        }
    }

    private void DisableCursorEditorOnly() {
        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                _disableCursor = !_disableCursor;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        } 
        #endif
    }

    private void DetectCursorType()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            SetActiveCursorType(CursorType.Arrow);
            return;
        }

        if (InputManager.Instance.CurrentHeldObject) {
            SetActiveCursorType(CursorType.Closed);
            return;
        }

        if (!IsInValidZone())
        {
            SetActiveCursorType(CursorType.NotAllowed);
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, _interactableLayer);

        if (hit) {
            if (Input.GetMouseButton(0))
            {
                SetActiveCursorType(CursorType.Closed);
            } else {
                SetActiveCursorType(CursorType.Open);
            }
        } else {
            SetActiveCursorType(CursorType.Arrow);
        }

        if (Slingshot.Instance.CurrentThrowableItem) {
            SetActiveCursorType(CursorType.Closed);
        }

    }

    public bool IsInValidZone() {
        if (_mainCam.ScreenToWorldPoint(Input.mousePosition).y < _yValueNotAllowedZoneValue)
        {
            return true;
        }

        return false;
    }

    public void SetActiveCursorType(CursorType cursorType)
    {
        if (_disableCursor) { return; }

        SetActiveCursorAnimation(GetCursorAnimation(cursorType));
        OnCursorChanged?.Invoke(this, new OnCursorChangedEventArgs { _cursorType = cursorType });
    }

    private CursorAnimation GetCursorAnimation(CursorType cursorType)
    {
        foreach (CursorAnimation cursorAnimation in _cursorAnimationList)
        {
            if (cursorAnimation.cursorType == cursorType)
            {
                return cursorAnimation;
            }
        }
        // Couldn't find this CursorType!
        return null;
    }

    private void SetActiveCursorAnimation(CursorAnimation cursorAnimation)
    {
        this._cursorAnimation = cursorAnimation;
        _currentFrame = 0;
        _frameTimer = 0f;
        _frameCount = cursorAnimation.textureArray.Length;
    }


    [System.Serializable]
    public class CursorAnimation
    {

        public CursorType cursorType;
        public Texture2D[] textureArray;
        public float frameRate;
        public Vector2 offset;

    }

}