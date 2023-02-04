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
        [SerializeField] private LineController _LinePrefab;
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

            // 開始時最初のポイント生成
            PopNewPoint(_startPoint, transform);
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
                var raycastResult = RayResult.FirstOrDefault(x => x.gameObject.tag == "Player");
                if (!raycastResult.Equals(default(RaycastResult)))
                {
                    var pointController = raycastResult.gameObject.GetComponent<PointController>();
                    // 分岐ポイント追加
                    pointController.AddPoint();
                    // 新規追加
                    PopNewPoint(pointController.transform, transform);
                }
            }
        }

        private void PopNewPoint(Transform startTrans, Transform rootTrans)
        {
            var line = Instantiate(_LinePrefab, startTrans.position, startTrans.rotation, rootTrans);
            line.Setup();
            line.gameObject.SetActive(true);
        }
    }
}