using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot : MonoBehaviour
{
    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;

        var s = Mathf.Sin(Time.time)*0.05f;

        myTransform.Translate(s, 0,s);

        // ���[�J�����W��ŁA���݂̉�]�ʂ։��Z����
        myTransform.Rotate(1.0f, 1.0f, 1.0f);
    }
}
