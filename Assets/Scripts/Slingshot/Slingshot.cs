using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Singleton<Slingshot>
{
    public Throwable CurrentThrowableItem => _currentThrowableItem;
    public bool IsSlinging => _isSlinging;

    [SerializeField] private float _slingElasticityStrength = 1.5f;
    [SerializeField] private float _slingElasticitySpeed = 10f;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _maxStetchLength;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _idlePosition;

    private Throwable _currentThrowableItem;
    private Vector3 _currentPosition, _slingStartPosition, _throwableForce;
    private TrajectoryLine _trajectoryLine;
    private float _slingShotForce;
    private bool _isSlinging = false;
    private bool _isAttached = false;

    protected override void Awake() {
        base.Awake();
        _trajectoryLine = GetComponent<TrajectoryLine>();
    }

    private void Start() {
        SetStrips(_idlePosition.position);
        _center.position = _idlePosition.position;
    }

    private void Update() {
        // have sling follow throwable item (mouse) that is currently attached
        if (_currentThrowableItem && !_isSlinging && _isAttached) {
            float stretchForceToAdd = Vector3.Distance(_currentPosition, _idlePosition.position);
            _slingShotForce = _slingElasticityStrength * stretchForceToAdd;
            _throwableForce = (_currentPosition - _idlePosition.position) * -_slingShotForce;

            _trajectoryLine.Velocity = _throwableForce;
            // _projection.SimulateTrajectory(_currentThrowableItem, _idlePosition.position, _throwableForce);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentPosition = mousePosition;
            _currentPosition = _idlePosition.position + Vector3.ClampMagnitude(_currentPosition - _idlePosition.position, _maxStetchLength);
            _center.position = _currentPosition;
            _currentThrowableItem.transform.position = _center.position;
            SetStrips(_center.position);
        } 

        // shoot sling if throwable item attached
        if (_currentThrowableItem && Input.GetMouseButtonUp(0) ) {
            _slingStartPosition = _center.position;
            _currentThrowableItem.IsActive = false;
            StartCoroutine(ShootThrowableRoutine());
        }

        // Detatch with right click
        if (_currentThrowableItem && Input.GetMouseButtonDown(1)) {
            _slingStartPosition = _center.position;
            _currentThrowableItem.AttachToSlingShot(false);
            _currentThrowableItem.DetachFromSlingShot();
            _currentThrowableItem = null;

            StartCoroutine(ShootThrowableRoutine());
            // _currentThrowableItem.IsActive = true;
        }
    }

    public void SetCurrentThrowableItem(Throwable throwable) {
        StopAllCoroutines();
        _isSlinging = false;
        _currentThrowableItem = throwable;
        StartCoroutine(SlingToThrowableRoutine());
    }

    private void Throw() {
        if (!_currentThrowableItem) { return; }

        float stretchForceToAdd = Vector3.Distance(_currentPosition, _idlePosition.position);
        _slingShotForce = _slingElasticityStrength * stretchForceToAdd;
        _throwableForce = (_currentPosition - _idlePosition.position) * -_slingShotForce;
        _currentThrowableItem.GetComponent<Rigidbody2D>().velocity = _throwableForce;
        _currentThrowableItem.AttachToSlingShot(false);
        _currentThrowableItem.IsInAirFromSlingshot = true;
        _currentThrowableItem.DetachFromSlingShot();
        _currentThrowableItem = null;
    }

    private IEnumerator SlingToThrowableRoutine() {
        // magic number atm but duration float is so the center quickly attatches to the thrown object but not instant.  Subtle but feels less jerky this way. 
        float duration = .1f;
        float time = 0.0f;

        while (time < duration)
        {
            if (_currentThrowableItem == null) { yield return null; }

            time += _slingElasticitySpeed * Time.deltaTime;
            float linearT = time / duration;
            _center.position = Vector3.Lerp(_idlePosition.position, _currentThrowableItem.transform.position, linearT);
            SetStrips(_center.position);
            yield return null;
        }

        _isAttached = true;
    }

    private IEnumerator ShootThrowableRoutine() {
        _isSlinging = true;
        float firstKeyTime = _curve.keys[1].time;
        float duration = 1.0f;
        float slingTime = 0.0f;
        bool hasThrown = false;

        while (slingTime < duration)
        {
            float t = slingTime / duration;
            float curveValue = _curve.Evaluate(t);
            _center.position = Vector3.LerpUnclamped(_slingStartPosition, _idlePosition.position, curveValue);

            if (_currentThrowableItem != null) {
                _currentThrowableItem.transform.position = _center.position;
            }

            SetStrips(_center.position);

            if (t >= firstKeyTime && !hasThrown)
            {
                Throw();
                hasThrown = true;
            }

            slingTime += _slingElasticitySpeed * Time.deltaTime;
            yield return null;
        }

        _center.position = _idlePosition.position;
        _isSlinging = false;
    }

    private void SetStrips(Vector3 position) {
        float offsetValue = .8f;

        _lineRenderers[0].SetPosition(0, position + new Vector3(-offsetValue, 0 , 0));
        _lineRenderers[1].SetPosition(0, position + new Vector3(offsetValue, 0, 0));
    }
}
