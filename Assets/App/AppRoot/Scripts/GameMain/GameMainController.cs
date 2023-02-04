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

        [SerializeField] private ItemGenerator _itemGenerator;

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

        private int selectNum;
        public int AngleLimit = 90;

        private float timer;
        public float checkTime = 0.1f;

        public bool IsWaitClick = true;

        private int _currentPhase = 0;

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

            // BGM再生
            AudioManager.Instance.PlayBGM("New_Horizon_2", volume: 0.2f);
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
                        if (_allLines[i].IsStop) continue;
                        transforms.Add(_allLines[i]);
                        _allLines[i].SetPause(true);
                    }
                    Debug.Log("transforms:" + transforms.Count);
                    isClicking = true;
                    selectNum = 0;
                }

                if (isClicking && IsWaitClick)
                {
                    timer += Time.deltaTime;
                    if (timer > checkTime)
                    {
                        timer = 0f;
                        selectNum++;
                        if (AngleLimit <= selectNum) selectNum = 0;

                        foreach (var t in _allLines)
                            if (!t.IsStop)
                                t.SetText(selectNum);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    isClicking = false;
                    Debug.Log("transforms c:" + transforms.Count + " al:" + _allLines.Count);
                    var angle = ForceAngle > 0f ? ForceAngle : Random.Range(AngleRange.x, AngleRange.y);
                    if (IsWaitClick) angle = selectNum;
                    foreach (var p in transforms)
                    {
                        p.Stop();
                        var select1 = PopNewPoint(p.Point.transform, transform);
                        var select2 = PopNewPoint(p.Point.transform, transform);
                        select1.MoveStart(angle + p.Point.NowAngle);
                        select2.MoveStart(angle * -1f + p.Point.NowAngle);
                        _allLines.Add(select1);
                        _allLines.Add(select2);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("LDown");
                    isClicking = true;
                    selectNum = 0;
                    _nowSelectLine.SetPause(true);
                }

                if (isClicking && IsWaitClick)
                {
                    timer += Time.deltaTime;
                    if (timer > checkTime)
                    {
                        timer = 0f;
                        selectNum++;
                        if (AngleLimit <= selectNum) selectNum = 0;

                        foreach (var t in _allLines)
                            if (!t.IsStop)
                                t.SetText(selectNum);
                    }
                }
                
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("LUp");
                    isClicking = false;
                    _nowSelectLine.Stop();
                    var nowTransform = _nowSelectLine.Point.transform;

                    var line1 = PopNewPoint(nowTransform, transform);
                    _allLines.Add(line1);
                    var line2 = PopNewPoint(nowTransform, transform);
                    _allLines.Add(line2);

                    _select1 = line1;
                    _select2 = line2;

                    var angle = ForceAngle > 0f ? ForceAngle : Random.Range(AngleRange.x, AngleRange.y);
                    if (IsWaitClick) angle = selectNum;
                    line1.MoveStart(angle);
                    line2.MoveStart(angle * -1f);

                    // line1選択しておく
                    _nowSelectLine = line1;

                    // ターゲット更新
                    _moveCameraController.SetTarget(line1.Point.transform);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    isClicking = false;
                    var nowIndex = _allLines.FindIndex(x => x == _nowSelectLine);
                    Debug.Log($"b nowIndex:{nowIndex} Count{_allLines.Count}");
                    LineController hit = null;
                    for (var i = nowIndex + 1; i < _allLines.Count; i++)
                        if (!_allLines[i].IsStop)
                        {
                            hit = _allLines[i];
                            break;
                        }

                    if (!hit)
                        for (var i = 0; i < nowIndex; i++)
                            if (!_allLines[i].IsStop)
                            {
                                hit = _allLines[i];
                                break;
                            }

                    if (!hit) Debug.LogError("ない！！！！！！！！！！！！！！！！！！！！");
                    _nowSelectLine = hit;
                    _moveCameraController.SetTarget(_nowSelectLine.Point.transform);
                }
            }
            // _moveCameraController.SetTarget();
        }

        private LineController PopNewPoint(Transform startTrans, Transform rootTrans)
        {
            Debug.Log("PopNewPoint");
            var line = Instantiate(_LinePrefab, startTrans.position, startTrans.rotation, rootTrans);
            line.Setup(indexCount, NextPhase);
            indexCount++;
            line.gameObject.SetActive(true);
            line.gameObject.name += Time.realtimeSinceStartup.ToString();
            _nowSelectLine = line;
            return line;
        }

        private void NextPhase(int phase)
        {
            if (_currentPhase >= phase)
                return;

            // 今あるフェーズのアイテム全削除
            _itemGenerator.ItemAllDestroy();
            _currentPhase = phase;
            // 次のフェーズのアイテム出現
            _itemGenerator.PhaseItemCreate(_currentPhase);
            Debug.Log(_currentPhase);
        }
    }
}