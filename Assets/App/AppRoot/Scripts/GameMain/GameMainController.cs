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
        public bool IsAll;
        
        [SerializeField] private MoveCameraController _moveCameraController;
        [SerializeField] private LineController _LinePrefab;
        [SerializeField] private Transform _startPoint;

        [SerializeField] private Button _debugResultButton;

        public float ForceAngle = -1f;
        public Vector2 AngleRange = new(0, 45);

        private readonly List<LineController> _allLines = new();

        private readonly int index = 0;
        private readonly Dictionary<int, UILineRenderer> _lineRenderers = new();

        //RaycastAllの引数
        private PointerEventData pointData;


        private LineController _lineRenderer;
        private PointController _point;

        private bool isClicking;
        private LineController _nowSelectLine;

        private LineController _select1;
        private LineController _select2;

        // All用
        private readonly List<LineController> transforms = new();

        private int indexCount;
        
        private void Start()
        {
            //RaycastAllの引数PointerEvenDataを作成
            pointData = new PointerEventData(EventSystem.current);

            // 開始時最初のポイント生成
            var line = PopNewPoint(_startPoint, transform);
            line.MoveStart(0f);
            _moveCameraController.SetTarget(line.Point.transform);
            _allLines.Add(line);
            // PopNewPoint(_startPoint, transform, 45f);
            
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
            // var RayResult = new List<RaycastResult>();
            // //PointerEvenDataに、マウスの位置をセット
            // pointData.position = Input.mousePosition;
            // //RayCast（スクリーン座標）
            // EventSystem.current.RaycastAll(pointData, RayResult);

            if (IsAll)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    transforms.Clear();
                    var max = _allLines.Count;
                    Debug.Log("Maxx" + max);
                    for (var i = 0; i < max; i++)
                    {
                        transforms.Add(_allLines[i]);
                    }
                    Debug.Log("transforms:" + transforms.Count);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("transforms c:" + transforms.Count + " al:" + _allLines.Count);
                    foreach (var p in transforms)
                    {
                        p.Stop();
                        _allLines.RemoveAt(p.Index);
                        var angle = ForceAngle > 0f ? ForceAngle : Random.Range(AngleRange.x, AngleRange.y);
                        var select1 = PopNewPoint(p.Point.transform, transform);
                        var select2 = PopNewPoint(p.Point.transform, transform);
                        select1.MoveStart(angle + p.Point.NowAngle);
                        select2.MoveStart(angle * -1f + p.Point.NowAngle);
                        _allLines.Add(select1);
                        _allLines.Add(select2);
                    }
                    //
                    //     Debug.Log("al:" + _allLines.Count);
                    //
                    //     transforms.Clear();
                    //     Debug.Log("_allLines c:" + _allLines.Count + " " + transforms.Count);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) isClicking = true;
                if (Input.GetMouseButtonUp(0))
                {
                    isClicking = false;
                    _nowSelectLine.Stop();
                    var nowTransform = _nowSelectLine.Point.transform;

                    _allLines.RemoveAt(_nowSelectLine.Index);

                    var line1 = PopNewPoint(nowTransform, transform);
                    _allLines.Add(line1);
                    var line2 = PopNewPoint(nowTransform, transform);
                    _allLines.Add(line2);

                    _select1 = line1;
                    _select2 = line2;
                    _nowSelectLine = line1;

                    // ターゲット更新
                    _moveCameraController.SetTarget(line1.Point.transform);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    isClicking = false;
                    var nowIndex = _allLines.FindIndex(x => x == _nowSelectLine);
                    Debug.Log($"b nowIndex:{nowIndex} Count{_allLines.Count}");
                    nowIndex++;
                    if (_allLines.Count <= nowIndex) nowIndex = 0;

                    Debug.Log($"a nowIndex:{nowIndex}");
                    _nowSelectLine = _allLines[nowIndex];
                    _moveCameraController.SetTarget(_nowSelectLine.Point.transform);

                    var angle = ForceAngle > 0f ? ForceAngle : Random.Range(AngleRange.x, AngleRange.y);
                    _select1.MoveStart(angle);
                    _select2.MoveStart(angle * -1f);
                    _select1 = null;
                    _select2 = null;
                }
            }
            // _moveCameraController.SetTarget();
        }

        private LineController PopNewPoint(Transform startTrans, Transform rootTrans)
        {
            Debug.Log("PopNewPoint");
            var line = Instantiate(_LinePrefab, startTrans.position, startTrans.rotation, rootTrans);
            line.Setup(indexCount);
            indexCount++;
            line.gameObject.SetActive(true);
            line.gameObject.name += Time.realtimeSinceStartup.ToString();
            _nowSelectLine = line;
            return line;
        }
    }
}