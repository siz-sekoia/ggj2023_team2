using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace App
{
    [RequireComponent(typeof(UILineRenderer))]
    public class LineController : MonoBehaviour
    {
        [SerializeField] private PointController _pointController;
        [SerializeField] private UILineRenderer _uiLineRenderer;

        [SerializeField] private TextMeshProUGUI _text;

        public PointController Point => _pointController;
        private readonly List<Vector2> _points = new();
        public bool IsReft;
        private int _nowIndex = 1;
        private bool initEnd;

        public bool IsOver;
        public bool IsStop;
        public bool IsPause;

        public int Index;

        public void SetText(int num)
        {
            // テキストはもう使わない
            _text.SetText(num.ToString());
            _text.gameObject.SetActive(false);
        }

        public void Setup(int index, Action<int> nextPhseAction)
        {
            Index = index;
            _points.Add(_pointController.transform.localPosition);
            _points.Add(_pointController.transform.localPosition);
            _uiLineRenderer.Points = _points.ToArray();
            _nowIndex = 1;
            _pointController.Setup(nextPhseAction);
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
            IsStop = true;
        }

        public void SetPause(bool enable)
        {
            _pointController.SetPause(enable);
            IsPause = enable;
        }


        public void SetOver(bool enable)
        {
            _pointController.SetOver(enable);
            IsOver = enable;
        }

        private void Update()
        {
            if (IsOver) return;
            if (IsPause) return;
            if (IsStop) return;
            if (!initEnd) return;
            _uiLineRenderer.Points[_nowIndex] = _pointController.transform.localPosition;
            _uiLineRenderer.SetAllDirty();
        }
    }
}