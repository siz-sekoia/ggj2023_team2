using UnityEngine;

namespace App
{
    public class MoveCameraController : MonoBehaviour
    {
        private Transform _targetObj;

        public void SetTarget(Transform transform)
        {
            _targetObj = transform;
        }

        private void LateUpdate()
        {
            if (_targetObj == null) return;

            transform.position = _targetObj.position;
        }
    }
}