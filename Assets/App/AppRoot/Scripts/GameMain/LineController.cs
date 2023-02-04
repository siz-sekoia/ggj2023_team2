using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace App
{
    [RequireComponent(typeof(UILineRenderer))]
    public class LineController : MonoBehaviour
    {
        [SerializeField] private PointController _pointController;
        [SerializeField] private UILineRenderer _uiLineRenderer;

        [SerializeField] private Canvas _canvas;

        public PointController Point => _pointController;
        
        private readonly List<Vector2> _points = new();

        private int _nowIndex = 1;

        public void Setup()
        {
            _points.Add(_pointController.transform.localPosition);
            _points.Add(_pointController.transform.localPosition);
            _uiLineRenderer.Points = _points.ToArray();
            _nowIndex = 1;

            // ポイント移動開始
            _pointController.AddVec();

            SetEvent();
        }

        private void SetEvent()
        {
            _pointController.OnAddPointObserver
                .Subscribe(_ => { AddPoint(); })
                .AddTo(this);
        }

        public void AddPoint()
        {
            // 分岐ポイント確保
            _points.Add(_pointController.transform.localPosition);
            _uiLineRenderer.Points = _points.ToArray();
            _nowIndex++;

            // 角度変更
            // _pointController.AddVec(Random.Range(-30f, 30f));
            _pointController.AddVec(-30f);
        }


        private void Update()
        {
            _uiLineRenderer.Points[_nowIndex] = _pointController.transform.localPosition;
            _uiLineRenderer.SetAllDirty();
        }

        private Vector2 WorldToCanvasPositionConvert(Transform transform)
        {
            return _canvas.worldCamera.WorldToScreenPoint(transform.position);
        }
    }
}