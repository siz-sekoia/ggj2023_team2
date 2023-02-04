using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private UILineRenderer _uiLineRendererPrefab;
        [SerializeField] private PointController _plinePrefab;
        [SerializeField] private Transform _startPoint;

        [SerializeField] private Button _debugResultButton;

        private readonly int index = 0;
        private readonly Dictionary<int, UILineRenderer> _lineRenderers = new();

        //RaycastAllの引数
        private PointerEventData pointData;


        private LineController _lineRenderer;
        private PointController _point;

        private void Start()
        {
            //RaycastAllの引数PointerEvenDataを作成
            pointData = new PointerEventData(EventSystem.current);
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
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // _uiLineRenderer = Instantiate(_uiLineRendererPrefab, _startPoint.position, _startPoint.rotation,
                //     transform);
                // _uiLineRenderer.Points = new[] { Vector2.zero, Vector2.zero };
                // _uiLineRenderer.gameObject.SetActive(true);
                // _point = Instantiate(_plinePrefab, _startPoint.position, _startPoint.rotation, transform);
                // _point.AddVec();
                // _point.gameObject.SetActive(true);
            }

            // if (_point != null) _uiLineRenderer.Points[1] = _point.transform.position;

            //RaycastAllの結果格納用のリスト作成
            var RayResult = new List<RaycastResult>();
            //PointerEvenDataに、マウスの位置をセット
            pointData.position = Input.mousePosition;
            //RayCast（スクリーン座標）
            EventSystem.current.RaycastAll(pointData, RayResult);

            if (Input.GetMouseButtonUp(0))
            {
                var raycastResult = RayResult.FirstOrDefault(x => x.gameObject.tag == "Player");
                if (!raycastResult.Equals(default(RaycastResult)))
                {
                    var pointController = raycastResult.gameObject.GetComponent<PointController>();
                    pointController.AddVec(Random.Range(-30, 30));
                    // _uiLineRenderer.Points.AddRange(new[] { _point.transform.position });
                }
                // raycastResult.gameObject.GetComponent<PointController>();
                // if (pointObj is PointController)
                // {
                //     // pointController
                //     // var uiLineRenderer =
                //     //     Instantiate(_uiLineRenderer, _startPoint.position, _startPoint.rotation, transform);
                //     // uiLineRenderer.gameObject.SetActive(true);
                //     // var pointController =
                //     //     Instantiate(_plinePrefab, _startPoint.position, _startPoint.rotation, transform);
                //     // pointController.SetVec(-90 + Random.Range(-30, 30));
                //     // pointController.gameObject.SetActive(true);
                //     // Debug.Log("Point!");
                // }
            }
        }

        private void PopNewPoint()
        {
            var uiLineRenderer = Instantiate(_uiLineRendererPrefab, _startPoint.position, _startPoint.rotation,
                transform);
            // _uiLineRenderer.Points = new[] { Vector2.zero, Vector2.zero };
            // _uiLineRenderer.gameObject.SetActive(true);
            _point = Instantiate(_plinePrefab, _startPoint.position, _startPoint.rotation, transform);
            _point.AddVec();
            _point.gameObject.SetActive(true);
        }
    }
}