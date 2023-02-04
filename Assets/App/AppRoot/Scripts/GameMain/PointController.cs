using UniRx;
using UnityEngine;

namespace App
{
    public class PointController : MonoBehaviour
    {
        // 速度
        [SerializeField] private float Speed = 1f;

        // 移動ベクトル
        private Vector3 vec;

        private float _angle = -90f;
        public Subject<Unit> OnAddPointObserver { get; set; }

        // Start is called before the first frame update
        public void AddVec(float angle = 0f)
        {
            var tangle = _angle + angle;
            // 角度をラジアンに変換
            var rad = tangle * Mathf.Deg2Rad;
            // ラジアンから進行方向を設定
            var direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            // 方向に速度を掛け合わせて移動ベクトルを求める
            // vec = direction * Speed * Time.deltaTime;
            vec = direction * Speed;
        }

        // Update is called once per frame
        private void Update()
        {
            // 物体を移動する
            transform.position += vec;
        }
    }
}