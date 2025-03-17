using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringIntPair
{
    public string itemTypeId; // 物品类型 ID
    public int minCount; // 最小数量
    public int maxCount; // 最大数量
    [Range(0, 1)] public float probability; // 掉落概率 (0到1之间)
}

public class DropItem : MonoBehaviour, IInteractable
{
    public string Name; // 掉落物名称
    public string id; // 掉落物的唯一标识符

    public List<StringIntPair> dataList = new List<StringIntPair>(); // 掉落物品列表

    private bool _isInitialized = false; // 是否已初始化

    // Start is called before the first frame update
    void Start()
    {
        id = "TMP_" + System.Guid.NewGuid().ToString();
        Debug.Log("start DropItem " + Name + "id:" + id);

        // 初始化掉落物
        InitializeDropItem();
    }

    // 初始化掉落物
    private void InitializeDropItem()
    {
        ItemManager itemManager = ItemManager.Instance;
        itemManager.CreateInventory(id, 99999); // 创建一个无限负重的临时库存

        foreach (StringIntPair pair in dataList)
        {
            // 根据概率决定是否掉落该物品
            if (UnityEngine.Random.value <= pair.probability)
            {
                // 在数量范围内随机生成物品数量
                int count = UnityEngine.Random.Range(pair.minCount, pair.maxCount + 1);

                if(count > 0)
                    itemManager.AddItemToInventory(id, pair.itemTypeId, count);
            }
        }

        _isInitialized = true; // 标记为已初始化
    }

    // Update is called once per frame
    void Update()
    {
        // 如果未初始化，跳过
        if (!_isInitialized) return;

        // 检查物品是否被拿空
        ItemManager itemManager = ItemManager.Instance;
        if (itemManager.GetAllItemsInInventory(id).Count == 0)
        {
            itemManager.DeleteInventory(id);
            Destroy(gameObject); // 如果物品被拿空，销毁自身
        }
    }

    public bool IsInteractable()
    {
        return true;
    }

    public bool IsAttackable()
    {
        return false;
    }

    public string GetName()
    {
        return Name;
    }

    public void Interact()
    {
        //InventoryUI.Instance.OpenInventory("PlayerInventory", "我的背包", id, Name);
        InventoryUI.Instance.OpenInventory(id, Name, "PlayerInventory", "我的背包");
    }

    public void TakeDamage(string weaponName, int damage)
    {
        // 实现受伤逻辑（如果需要）
    }

    public bool isDeath()
    {
        return false;
    }
}
