using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

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

        private readonly Vector2 _judgeCenter = new(0f, -24f);
        private readonly float _judgeR = 530;

        [SerializeField] private Button _startButton;
        [SerializeField] private BranchCounter branchCounter;

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
        private bool selectNumTurn;
        public int AngleLimit = 90;

        private float timer;
        public float checkTime = 0.1f;


        public float coolTimer;
        public float coolCheckTime = 5f;

        public int nowClickCount;
        public int clickCountLimit = 8;
        

        public bool IsWaitClick = true;

        private int _currentPhase = 0;

        public bool IsStart;
        public bool IsGameOver;

        private readonly float[] _getItemParamArray = { -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f };
        public int AliveCount;

        [SerializeField]
        GameObject resultView;

        [SerializeField]
        private InputField _apiInput;

        private void Start()
        {
            //RaycastAllの引数PointerEvenDataを作成
            pointData = new PointerEventData(EventSystem.current);

            SetEvent();

            IsStart = false;
            IsGameOver = false;

            // BGM再生
            AudioManager.Instance.PlayBGM("New_Horizon_2", volume: 0.2f);
            branchCounter.UpdateCounter(clickCountLimit);
        }

        private void SetEvent()
        {
            _debugResultButton.OnClickAsObservable()
                .Subscribe(_ => { SceneManager.LoadScene("Result"); })
                .AddTo(this);

            // スタートボタン
            _startButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    IsStart = true;
                    IsGameOver = false;

                    nowClickCount = 0;
                    coolTimer = 0f;
                    
                    _startButton.gameObject.SetActive(false);
                    _apiInput.gameObject.SetActive(false);

                    // 開始時最初のポイント生成
                    var line = PopNewPoint(_startPoint, transform);
                    line.MoveStart(0f);
                    _moveCameraController.SetTarget(line.Point.transform);
                    _allLines.Add(line);

                    _itemGenerator.Setup(CalcItemParam);
                    branchCounter.gameObject.SetActive(true);
                })
                .AddTo(this);

            _apiInput.onValueChanged.AddListener(delegate {
                GGJ2023APIController.Instance.chatGptApiKey = _apiInput.text;
            });
        }

        private void Update()
        {
            //RaycastAllの結果格納用のリスト作成
            // var RayResult = new List<RaycastResult>();
            // //PointerEvenDataに、マウスの位置をセット
            // pointData.position = Input.mousePosition;
            // //RayCast（スクリーン座標）
            // EventSystem.current.RaycastAll(pointData, RayResult);
            SiroinabaDebug();
            if (!IsStart)
                // 未スタート時処理
                return;

            if (IsGameOver)
                // ゲームオーバー時処理
                return;

            // 範囲外判定
            var allActive = true;
            foreach (var line in _allLines)
            {
                if (line.IsOver)
                    continue;

                if (!InCircle(_judgeCenter, _judgeR, line.Point.transform.position))
                {
                    Debug.Log("<color=red>Over Hit!</color>");
                    line.SetOver(true);
                }
            }

            AliveCount = _allLines.Count > 0 ? _allLines.Count(x => !x.IsOver) : -1;
            if (_allLines.Count > 0 && AliveCount <= 0)
            {
                Debug.Log("<color=red>GameOver</color>");
                ReplaceItemParam();
                IsGameOver = true;
                branchCounter.gameObject.SetActive(false);
                return;
            }

            if (IsAll)
            {
                if (!isClicking)
                {
                    coolTimer -= Time.deltaTime;
                    if (coolTimer > 0f) Debug.Log($"CoolTime:{coolTimer}");
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (coolTimer <= 0f)
                    {
                        if (nowClickCount >= clickCountLimit)
                        {
                            Debug.Log("Over Click");
                        }
                        else
                        {
                            Debug.Log($"Click!! {nowClickCount}");
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

                            coolTimer = coolCheckTime;
                            nowClickCount++;

                            // タップ開始時表示有効
                            foreach (var t in _allLines)
                            {
                                if (t.IsOver)
                                    continue;

                                t.Point.TapActive(true);
                            }

                            selectNumTurn = true;
                        }
                    }
                }

                if (isClicking && IsWaitClick)
                {
                    timer += Time.deltaTime;
                    if (timer > checkTime)
                    {
                        timer = 0f;
                        if (selectNumTurn)
                            selectNum++;
                        else
                            selectNum--;

                        if (selectNum < 0)
                        {
                            selectNumTurn = !selectNumTurn;
                            selectNum = 0;
                        }
                        else if (AngleLimit <= selectNum)
                        {
                            selectNumTurn = !selectNumTurn;
                            selectNum = AngleLimit;
                        }

                        foreach (var t in _allLines)
                            if (!t.IsStop && !t.IsOver)
                            {
                                t.SetText(selectNum);
                                var per = selectNum / 360f;
                                t.Point.Tapping(per);
                            }
                    }
                }

                if (Input.GetMouseButtonUp(0) && isClicking)
                {
                    // タップ開始時表示無効
                    foreach (var t in _allLines)
                    {
                        if (t.IsOver)
                            continue;

                        t.Point.TapActive(false);
                    }
                    
                    isClicking = false;
                    Debug.Log("transforms c:" + transforms.Count + " al:" + _allLines.Count);
                    var angle = ForceAngle > 0f ? ForceAngle : Random.Range(AngleRange.x, AngleRange.y);
                    if (IsWaitClick) angle = selectNum;
                    foreach (var p in transforms)
                    {
                        p.Stop();
                        p.SetOver(true);
                        var select1 = PopNewPoint(p.Point.transform, transform);
                        var select2 = PopNewPoint(p.Point.transform, transform);
                        select1.MoveStart(angle + p.Point.NowAngle);
                        select2.MoveStart(angle * -1f + p.Point.NowAngle);
                        _allLines.Add(select1);
                        _allLines.Add(select2);
                    }
                    branchCounter.UpdateCounter(clickCountLimit - nowClickCount);
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
            line.gameObject.hideFlags = HideFlags.HideInHierarchy;
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

        /// <summary>
        ///     円の内側か
        ///     (x - a)^2 + (y - b)^2 <= r^2
        /// </summary>
        /// <param name="p">円の中心座標</param>
        /// <param name="r">半径</param>
        /// <param name="c">対象となる点</param>
        /// <returns></returns>
        public static bool InCircle(Vector2 p, float r, Vector2 c)
        {
            var sum = 0f;
            for (var i = 0; i < 2; i++)
                sum += Mathf.Pow(p[i] - c[i], 2);
            var res = sum <= Mathf.Pow(r, 2f);
            Debug.Log($"<color=cyan>p:{p} c:{c} r:{r}</color> res:{res}");
            return res;
        }

        private void CalcItemParam(int itemType, float val)
        {
            _getItemParamArray[itemType] += val;
        }

        private void SiroinabaDebug()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReplaceItemParam();
            }
        }

        private void CheckMaxParamArrayNum()
        {
            for(int idx = 0; idx < _getItemParamArray.Length; idx++)
            {
                _getItemParamArray[idx] = Math.Max(_getItemParamArray[idx], 1.0f);
            }
        }

        private string ReplaceItemParam()
        {
            string ret = GameDefine.chatGptDefaultText;
            resultView.SetActive(true);

            if (string.IsNullOrEmpty(GGJ2023APIController.Instance.chatGptApiKey))
            {
                resultView.GetComponent<Result>().TextSetting(GameDefine.default_story);

            }
            else
            {
                CheckMaxParamArrayNum();

                ret = ret.Replace("[#0]", _getItemParamArray[0].ToString());
                ret = ret.Replace("[#1]", _getItemParamArray[1].ToString());
                ret = ret.Replace("[#2]", _getItemParamArray[2].ToString());
                ret = ret.Replace("[#3]", _getItemParamArray[3].ToString());
                ret = ret.Replace("[#4]", _getItemParamArray[4].ToString());
                ret = ret.Replace("[#5]", _getItemParamArray[5].ToString());
                ret = ret.Replace("[#6]", _getItemParamArray[6].ToString());
                ret = ret.Replace("[#7]", _getItemParamArray[7].ToString());

                Debug.Log(ret);

                ResultChatGPT(ret);
            }



            return ret;
        }

        private async void ResultChatGPT(string prompt)
        {


            // ChatGPTのAPIを叩いて、レスポンス取得
            var response = await GGJ2023APIController.GetChatGPTAPIResponse(prompt, GGJ2023APIController.Instance.chatGptApiKey);
            // レスポンスからテキスト取得
            string outputText = response.Choices.FirstOrDefault().Text;

            string result = "";
            result = outputText.TrimStart('\n');
            Debug.Log("CHAT_GPT  :  " + outputText);

            resultView.GetComponent<Result>().TextSetting(outputText);
        }
    }
}