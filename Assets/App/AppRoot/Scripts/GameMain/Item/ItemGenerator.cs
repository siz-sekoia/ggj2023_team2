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
    /// アイテム生成処理
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
    /// ID指定によるアイテム取得
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
    /// フィールドにあるアイテムの全削除
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
    /// アイテム生成テスト
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

        // 全削除テスト
        //ItemAllDestroy();
    }

    // ItemEntityのprefab
    [SerializeField]
    private GameObject _itemObj;
    [SerializeField]
    private int _initCraeteItemCount = 0;
    // 存在するアイテムのリスト
    private List<ItemEntity> _itemList = new List<ItemEntity>();
    // アイテムに振っている最大のID
    private int _itemMaxId = -1;
}
