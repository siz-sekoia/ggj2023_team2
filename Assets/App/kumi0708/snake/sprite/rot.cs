using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot : MonoBehaviour
{
    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        var s = Mathf.Sin(Time.time)*0.05f;

        myTransform.Translate(s, 0,s);

        // ローカル座標基準で、現在の回転量へ加算する
        myTransform.Rotate(1.0f, 1.0f, 1.0f);
    }
}
