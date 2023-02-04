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

            var targetPos = _targetObj.position;
            targetPos.z = transform.position.z;
            transform.position = targetPos;
        }
    }
}