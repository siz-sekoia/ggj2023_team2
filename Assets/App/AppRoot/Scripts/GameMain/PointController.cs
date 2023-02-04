using UniRx;
using UnityEngine;

namespace App
{
    public class PointController : MonoBehaviour
    {
        // 速度
        [SerializeField] private float Speed = 1f;

        // 移動ベクトル
        public Vector3 vec;

        private readonly float _defultAngle = -90f;
        public Subject<Unit> OnAddPointObserver { get; set; }

        public bool _isStop;
        public bool IsPause;
        public float NowAngle;

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

        // Update is called once per frame
        private void Update()
        {
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
            ItemEntity item = collision.gameObject.GetComponent<ItemEntity>();

            // アイテムじゃなければ無視
            if (item == null)
                return;

            // アイテム獲得処理
            item.ItemGet();
        }
    }
}