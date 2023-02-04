using UnityEngine;
using UnityEngine.UI.Extensions;

namespace App
{
    public class PointController : MonoBehaviour
    {
        [SerializeField] private UILineRenderer _uiLineRenderer;

        // 速度
        public float Speed;

        // 移動ベクトル
        private Vector3 vec;

        private float _angle = -90f;

        // Start is called before the first frame update
        public void AddVec(float angle = 0f)
        {
            _angle += angle;
            // 角度をラジアンに変換
            var rad = _angle * Mathf.Deg2Rad;
            // ラジアンから進行方向を設定
            var direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            // 方向に速度を掛け合わせて移動ベクトルを求める
            vec = direction * Speed * Time.deltaTime;
        }

        // Update is called once per frame
        private void Update()
        {
            // 物体を移動する
            transform.position += vec;
        }
    }
}