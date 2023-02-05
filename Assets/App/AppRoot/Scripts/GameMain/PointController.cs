using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class PointController : MonoBehaviour
    {
        [SerializeField] private Image _tapImage1;
        [SerializeField] private Image _tapImage2;
        
        // 速度
        [SerializeField] private float Speed = 1f;

        // 移動ベクトル
        public Vector3 vec;

        private readonly float _defultAngle = -90f;
        public Subject<Unit> OnAddPointObserver { get; set; }

        public bool _isStop;
        public bool IsOver;
        public bool IsPause;
        public float NowAngle;

        private Action<int> _nextPhaseAction;
        public float imageColorDuration = 1f; 

        public void Setup(Action<int> nextPhaseAction)
        {
            _nextPhaseAction = nextPhaseAction;
        }

        public void TapActive(bool enable)
        {
            // _tapImage1.gameObject.SetActive(enable);
            // _tapImage2.gameObject.SetActive(enable);
            // var c = _tapImage.color;
            // c.a = 1f;
            // _tapImage.color = c;
            var c = _tapImage1.color;
            c.a = enable ? 0.0f : 1.0f; // 初期値
            _tapImage1.color = c;
            _tapImage2.color = c;
            var endValue = enable ? 1.0f : 0.0f;
            DOTween.ToAlpha(
                () => _tapImage1.color,
                color => _tapImage1.color = color,
                endValue, // 目標値
                imageColorDuration // 所要時間
            );
            DOTween.ToAlpha(
                () => _tapImage2.color,
                color => _tapImage2.color = color,
                endValue, // 目標値
                imageColorDuration // 所要時間
            );
        }

        public void Tapping(float fill)
        {
            var x = NowAngle;
            Debug.Log($"NowAngle : {NowAngle}");
            var normalizedAngle = Mathf.Repeat(x, 360);

            _tapImage1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, normalizedAngle);
            _tapImage2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, normalizedAngle);
            _tapImage1.fillAmount = fill;
            _tapImage2.fillAmount = fill;
        }

        /// <summary>
        ///     角度指定して、移動速度設定 (0 = 下)
        /// </summary>
        public void AddVec(float angle = 0f)
        {
            var tangle = _defultAngle + angle;
            NowAngle = angle;
            Debug.Log(tangle);
            // 角度をラジアンに変換
            var rad = tangle * Mathf.Deg2Rad;
            // ラジアンから進行方向を設定
            var direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            // 方向に速度を掛け合わせて移動ベクトルを求める
            // vec = direction * Speed * Time.deltaTime;
            vec = direction * Speed;
        }

        public void Stop()
        {
            _isStop = true;
            gameObject.SetActive(false);
        }

        public void SetPause(bool enable)
        {
            IsPause = enable;
        }

        public void SetOver(bool enable)
        {
            IsOver = enable;
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsOver) return;
            if (IsPause) return;
            if (_isStop) return;
            // 物体を移動する
            transform.position += vec;
        }

        /// <summary>
        /// ラインの当たり判定処理
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            NextPhaseLine next = collision.gameObject.GetComponent<NextPhaseLine>();
            if(next != null)
            {
                Destroy(collision.gameObject);
                _nextPhaseAction(next.nextPhase);
            }

            ItemEntity item = collision.gameObject.GetComponent<ItemEntity>();
            // アイテムじゃなければ無視
            if (item != null)
            {
                // アイテム獲得処理
                item.ItemGet();
            }
        }
    }
}