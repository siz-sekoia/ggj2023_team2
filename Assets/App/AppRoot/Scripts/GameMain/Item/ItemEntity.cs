using kumi0708;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntity : MonoBehaviour
{
    public int uniqueId{ get{ return _uniqueId; } }

    /// <summary>
    /// アイテムの初期設定
    /// </summary>
    /// <param name="paramType"></param>
    /// <param name="paramVal"></param>
    public void Setup(float paramVal, int uniqueId, Action<int> delete)
    {
        _paramVal = paramVal;
        _uniqueId = uniqueId;
        _delete = delete;
        Debug.Log("アイテム生成！ ParamType : " + _paramType + " : ParamVal : " + _paramVal + " : UniqueId : " + _uniqueId);

        // 色変えなし
        //SetImageColor(_paramType);
    }

    /*
    /// <summary>
    /// アイテムの画像色設定（仮）
    /// </summary>
    public void SetImageColor(GameDefine.ItemParamType type)
    {
        switch(type)
        {
            default:
                // 画像色変えテスト
                float r = Random.Range(0f, 1.0f);
                float g = Random.Range(0f, 1.0f);
                float b = Random.Range(0f, 1.0f);

                _itemImage.color = new Color(r, g, b);
                break;
        }    
    }
    */

    /// <summary>
    /// アイテム獲得処理
    /// </summary>
    public void ItemGet()
    {
        // エフェクト再生（なぜか出ない…）
        eff.EffectEvent();
        // SE再生
        AudioManager.Instance.PlaySE("se_pomyu");

        _delete(_uniqueId);

        OnDestroy();
    }

    /// <summary>
    /// アイテム削除処理
    /// </summary>
    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    // アイテムのユニークID（必ず他のアイテムと被らないID）
    private int _uniqueId;
    [SerializeField]
    // パラメータのタイプ（性別とか）
    private GameDefine.ItemParamType _paramType;
    [SerializeField]
    // パラメータの値
    private float _paramVal;
    [SerializeField]
    private effect eff;
    // アイテムの画像
    [SerializeField]
    private Image _itemImage;

    private Action<int> _delete;
}
