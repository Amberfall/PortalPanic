using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private float _maxDistance;
    [SerializeField] private int _linePoints;

    private Vector2 _velocity;
    private Slingshot _slingShot;

    private void Awake() {
        _slingShot = GetComponent<Slingshot>();
    }

    private void Start()
    {
        if (_lineRenderer == null)
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }
    }

    private void Update()
    {
        if (_slingShot.IsSlinging) { 
            _lineRenderer.enabled = false;
            return;
        }
        
        if (_slingShot.CurrentThrowableItem) { 
            DrawTrajectory();
        } else {
            _lineRenderer.enabled = false;
        }
    }

    private void DrawTrajectory()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = _linePoints + 1;
        _lineRenderer.SetPositions(CalculateLineArray());
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[_linePoints + 1];

        for (int i = 0; i <= _linePoints; i++)
        {
            float t = i / (float)_linePoints;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = _velocity.x * t;
        float y = _velocity.y * t - 0.5f * -Physics2D.gravity.y * Mathf.Pow(t, 2);
        return new Vector3(x, y, 0) + _startTransform.position;
    }
}
