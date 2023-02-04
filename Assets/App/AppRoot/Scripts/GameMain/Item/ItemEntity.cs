using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntity : MonoBehaviour
{
    public int uniqueId{ get{ return _uniqueId; } }

    /// <summary>
    /// �A�C�e���̏����ݒ�
    /// </summary>
    /// <param name="paramType"></param>
    /// <param name="paramVal"></param>
    public void Setup(int paramType, float paramVal, int uniqueId)
    {
        _paramType = (GameDefine.ItemParamType)paramType;
        _paramVal = paramVal;
        _uniqueId = uniqueId;

        Debug.Log("�A�C�e�������I ParamType : " + _paramType + " : ParamVal : " + _paramVal + " : UniqueId : " + _uniqueId);

        SetImageColor(_paramType);
    }

    /// <summary>
    /// �A�C�e���̉摜�F�ݒ�i���j
    /// </summary>
    public void SetImageColor(GameDefine.ItemParamType type)
    {
        switch(type)
        {
            default:
                // �摜�F�ς��e�X�g
                float r = Random.Range(0f, 1.0f);
                float g = Random.Range(0f, 1.0f);
                float b = Random.Range(0f, 1.0f);

                _itemImage.color = new Color(r, g, b);
                break;
        }    
    }

    /// <summary>
    /// �A�C�e���l������
    /// </summary>
    public void ItemGet()
    {
        OnDestroy();
    }

    /// <summary>
    /// �A�C�e���폜����
    /// </summary>
    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    // �A�C�e���̃��j�[�NID�i�K�����̃A�C�e���Ɣ��Ȃ�ID�j
    private int _uniqueId;
    // �p�����[�^�̃^�C�v�i���ʂƂ��j
    private GameDefine.ItemParamType _paramType;
    // �p�����[�^�̒l
    private float _paramVal;
    // �A�C�e���̉摜
    [SerializeField]
    private Image _itemImage;
}
