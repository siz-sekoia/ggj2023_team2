using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private void Start()
    {
        PhaseItemCreate(0);
    }

    /// <summary>
    /// アイテム生成処理
    /// </summary>
    public void ItemCreate(GameObject item, float itemVal, Vector3 pos)
    {
        var entity = Instantiate(item, Vector3.zero, Quaternion.identity).GetComponent<ItemEntity>();

        entity.transform.SetParent(transform);
        entity.transform.localPosition = pos;

        _itemMaxId++;
        entity.Setup(itemVal, _itemMaxId, ListRemove);

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

    public void PhaseItemCreate(int phase)
    {
        List<GameObject> list = new List<GameObject>();
        float posY_min = 0.0f;
        float posY_max = 0.0f;

        switch(phase)
        {
            case 0:
                list = _firstItemObjList;
                posY_min = 360.0f;
                posY_max = 169.0f;
                break;
            case 1:
                list = _secondItemObjList;
                posY_min = 169.0f;
                posY_max = -22.0f;
                break;
            case 2:
                list = _thirdItemObjList;
                posY_min = -22.0f;
                posY_max = -213.0f;
                break;
            case 3:
                list = _fourthItemObjList;
                posY_min = -213.0f;
                posY_max = -404.0f;
                break;
            default:
                list = _firstItemObjList;
                Debug.LogWarning("意図しないフェーズのアイテムを生成しようとしているので、第1フェーズのアイテムを生成します");
                break;
        }

        foreach(var obj in list)
        {
            for(int count = 0; count < _initCraeteItemCount; count++)
            {
                float val = Random.Range(_increaseMinNum, _increaseMaxNum);
                float pos_x = Random.Range(-384.0f, 348.0f);
                float pos_y = Random.Range(posY_min, posY_max);
                
                Vector3 pos = Vector3.zero;
                pos.x = pos_x;
                pos.y = pos_y;
                pos.z = 0.0f;
                
                ItemCreate(obj, val, pos);
            }
        }
    }

    /// <summary>
    /// アイテム生成テスト（使えません）
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

            //ItemCreate(0, 100.0f, pos);
        }

        // 全削除テスト
        //ItemAllDestroy();
    }

    private void ListRemove(int id)
    {
        var entity = GetItemEntity(id);
        if(entity != null)
        {
            _itemList.Remove(entity);
        }

    }

    // ItemEntityのprefab
    [SerializeField]
    private GameObject _itemObj;
    [SerializeField]
    private int _initCraeteItemCount = 0;
    [SerializeField]
    private List<GameObject> _firstItemObjList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _secondItemObjList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _thirdItemObjList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _fourthItemObjList = new List<GameObject>();
    // パラメータ増加の最大値
    [SerializeField]
    private float _increaseMaxNum;
    // パラメータ増加の最小値
    [SerializeField]
    private float _increaseMinNum;
    // 存在するアイテムのリスト
    private List<ItemEntity> _itemList = new List<ItemEntity>();
    // アイテムに振っている最大のID
    private int _itemMaxId = -1;
}
