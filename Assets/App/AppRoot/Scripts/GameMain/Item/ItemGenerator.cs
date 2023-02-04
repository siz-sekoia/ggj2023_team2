using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private void Start()
    {
        Test();
    }

    /// <summary>
    /// �A�C�e����������
    /// </summary>
    public void ItemCreate(int itemParamType, float itemVal, Vector3 pos)
    {
        var entity = Instantiate(_itemObj, Vector3.zero, Quaternion.identity).GetComponent<ItemEntity>();

        entity.transform.SetParent(transform);
        entity.transform.localPosition = pos;

        _itemMaxId++;
        entity.Setup(itemParamType, itemVal, _itemMaxId);

        _itemList.Add(entity);
    }

    /// <summary>
    /// ID�w��ɂ��A�C�e���擾
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemEntity GetItemEntity(int id)
    {
       foreach(ItemEntity entity in _itemList)
        {
            if (entity.uniqueId == id)
                return entity;
        }

        return null;
    }

    /// <summary>
    /// �t�B�[���h�ɂ���A�C�e���̑S�폜
    /// </summary>
    public void ItemAllDestroy()
    {
        foreach(ItemEntity entity in _itemList)
        {
            entity.OnDestroy();
        }

        _itemList.Clear();
    }

    /// <summary>
    /// �A�C�e�������e�X�g
    /// </summary>
    private void Test()
    {
        Vector3 pos = Vector3.zero;

        for (int idx = 0; idx < _initCraeteItemCount + 1; idx++)
        {
            float x = Random.Range(-500.0f, 500.0f);
            float y = Random.Range(-500.0f, 500.0f);

            pos.x = x;
            pos.y = y;
            pos.z = 0.0f;

            ItemCreate(0, 100.0f, pos);
        }

        // �S�폜�e�X�g
        //ItemAllDestroy();
    }

    // ItemEntity��prefab
    [SerializeField]
    private GameObject _itemObj;
    [SerializeField]
    private int _initCraeteItemCount = 0;
    // ���݂���A�C�e���̃��X�g
    private List<ItemEntity> _itemList = new List<ItemEntity>();
    // �A�C�e���ɐU���Ă���ő��ID
    private int _itemMaxId = -1;
}
