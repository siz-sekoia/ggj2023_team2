using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace App
{
    [RequireComponent(typeof(UILineRenderer))]
    public class LineController : MonoBehaviour
    {
        [SerializeField] private PointController _pointController;
        [SerializeField] private UILineRenderer _uiLineRenderer;

        public PointController Point => _pointController;
        private readonly List<Vector2> _points = new();
        public bool IsReft;
        private int _nowIndex = 1;
        private bool initEnd;

        public bool _isStop;

        public int Index;

        public void Setup(int index)
        {
            Index = index;
            _points.Add(_pointController.transform.localPosition);
            _points.Add(_pointController.transform.localPosition);
            _uiLineRenderer.Points = _points.ToArray();
            _nowIndex = 1;
        }

        public void MoveStart(float angle)
        {
            // 左右確保
            IsReft = angle > 0f;
            // ポイント移動開始
            _pointController.AddVec(angle);
            initEnd = true;
        }

        public void Stop()
        {
            Debug.Log("Stop");
            _pointController.Stop();
            _isStop = true;
        }

        private void Update()
        {
            if (_isStop) return;
            if (!initEnd) return;
            _uiLineRenderer.Points[_nowIndex] = _pointController.transform.localPosition;
            _uiLineRenderer.SetAllDirty();
        }
    }
}