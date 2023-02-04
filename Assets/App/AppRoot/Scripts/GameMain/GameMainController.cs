using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace App
{
    public class GameMainController : MonoBehaviour
    {
        [SerializeField] private MoveCameraController _moveCameraController;
        [SerializeField] private LineController _LinePrefab;
        [SerializeField] private Transform _startPoint;

        [SerializeField] private Button _debugResultButton;

        private readonly List<LineController> _allLines = new();

        private readonly int index = 0;
        private readonly Dictionary<int, UILineRenderer> _lineRenderers = new();

        //RaycastAllの引数
        private PointerEventData pointData;


        private LineController _lineRenderer;
        private PointController _point;

        private bool isClicking;

        private void Start()
        {
            //RaycastAllの引数PointerEvenDataを作成
            pointData = new PointerEventData(EventSystem.current);

            // 開始時最初のポイント生成
            PopNewPoint(_startPoint, transform, -45f);
            PopNewPoint(_startPoint, transform, 45f);
            
            SetEvent();
        }

        private void SetEvent()
        {
            _debugResultButton.OnClickAsObservable()
                .Subscribe(_ => { SceneManager.LoadScene("Result"); })
                .AddTo(this);
        }

        private void Update()
        {
            //RaycastAllの結果格納用のリスト作成
            var RayResult = new List<RaycastResult>();
            //PointerEvenDataに、マウスの位置をセット
            pointData.position = Input.mousePosition;
            //RayCast（スクリーン座標）
            EventSystem.current.RaycastAll(pointData, RayResult);

            if (Input.GetMouseButtonUp(0))
            {
                var max = _allLines.Count;
                for (var i = 0; i < max; i++)
                {
                    var line = _allLines[i];
                    // line.AddPoint();
                    PopNewPoint(line.Point.transform, transform, line.IsReft ? -45f : 45f);
                }
            }

            if (Input.GetMouseButtonDown(0)) isClicking = true;
            if (Input.GetMouseButtonUp(0))
            {
                isClicking = false;
            }

            if (Input.GetMouseButtonUp(1)) isClicking = false;
            // _moveCameraController.SetTarget();
        }

        private void PopNewPoint(Transform startTrans, Transform rootTrans, float angle)
        {
            var line = Instantiate(_LinePrefab, startTrans.position, startTrans.rotation, rootTrans);
            line.Setup(angle);
            line.gameObject.SetActive(true);
            line.gameObject.name += Time.realtimeSinceStartup.ToString();
            _allLines.Add(line);
        }
    }
}