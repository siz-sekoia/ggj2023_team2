using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace App
{
    [RequireComponent(typeof(UILineRenderer))]
    public class LineController : MonoBehaviour
    {
        private UILineRenderer _uiLineRenderer;
        private PointController _targetPoint;
        private readonly List<Vector2> _points = new();

        private int _nowIndex = 1;

        private void Awake()
        {
            _uiLineRenderer = GetComponent<UILineRenderer>();
        }

        public void Setup(PointController pointController)
        {
            _targetPoint = pointController;
            _points.Add(_targetPoint.transform.position);
            _points.Add(_targetPoint.transform.position);
            _nowIndex = 1;
        }


        private void Update()
        {
            _uiLineRenderer.Points[_nowIndex] = _targetPoint.transform.position;
        }
    }
}