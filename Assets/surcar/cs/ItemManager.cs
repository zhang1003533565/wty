using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemType
{
    public string Id; // 物品的唯一标识符
    public string Name; // 物品名称
    public string Description; // 物品描述
    public Sprite Icon; // 物品图标
    public float Weight; // 物品的单位重量
    public bool canUse; //是否可使用
    public bool canEquip;//是否可装备
    public string equipSlot;//如果可以装备，对应的槽位名称

    public ItemType(string id, string name, string description, Sprite icon, float weight, bool canUse, bool canEquip, string equipSlot)
    {
        Id = id;
        Name = name;
        Description = description;
        Icon = icon;
        Weight = weight;
        this.canUse = canUse;
        this.canEquip = canEquip;
        this.equipSlot = equipSlot;
    }
}

[System.Serializable]
public class ItemInstance
{
    public string UUID; // 物品实例的唯一标识符
    public string ItemTypeId; // 物品类型的 ID
    public int Quantity; // 物品数量

    public ItemInstance(string uuid, string itemTypeId, int quantity)
    {
        UUID = uuid;
        ItemTypeId = itemTypeId;
        Quantity = quantity;
    }
}

[System.Serializable]
public class Inventory
{
    public string InventoryName; // 仓库名称
    public float MaxWeight; // 仓库的最大承重
    public List<ItemInstance> Items; // 仓库中的物品实例列表

    public Inventory(string inventoryName, float maxWeight)
    {
        InventoryName = inventoryName;
        MaxWeight = maxWeight;
        Items = new List<ItemInstance>();
    }

    // 计算仓库当前总重量
    public float CalculateTotalWeight(Dictionary<string, ItemType> itemTypes)
    {
        float totalWeight = 0;
        foreach (var item in Items)
        {
            if (itemTypes.ContainsKey(item.ItemTypeId))
            {
                totalWeight += itemTypes[item.ItemTypeId].Weight * item.Quantity;
            }
        }
        return totalWeight;
    }

    // 添加物品
    public bool AddItem(ItemInstance item, Dictionary<string, ItemType> itemTypes)
    {
        float itemWeight = itemTypes.ContainsKey(item.ItemTypeId) ? itemTypes[item.ItemTypeId].Weight : 0;
        if (CalculateTotalWeight(itemTypes) + itemWeight * item.Quantity > MaxWeight)
        {
            return false; // 超出仓库承重
        }

        // 查找是否已有相同类型的物品
        var existingItem = Items.Find(i => i.ItemTypeId == item.ItemTypeId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity; // 堆叠
        }
        else
        {
            Items.Add(item); // 添加新物品
        }
        return true;
    }

    // 删除物品
    public void RemoveItem(string itemTypeId, int quantity)
    {
        var item = Items.Find(i => i.ItemTypeId == itemTypeId);
        if (item != null)
        {
            item.Quantity -= quantity;
            if (item.Quantity <= 0)
            {
                Items.Remove(item);
            }
        }
    }

    // 查找物品
    public ItemInstance GetItem(string itemTypeId)
    {
        return Items.Find(i => i.ItemTypeId == itemTypeId);
    }
}


public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private Dictionary<string, ItemType> _itemTypes; // 所有物品类型
    private Dictionary<string, Inventory> _inventories; // 所有仓库
    private DataManager _dataManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _dataManager = DataManager.Instance;
        _itemTypes = new Dictionary<string, ItemType>();
        _inventories = new Dictionary<string, Inventory>();
    }

    // 添加物品类型
    public void AddItemType(ItemType itemType)
    {
        if (!_itemTypes.ContainsKey(itemType.Id))
        {
            _itemTypes.Add(itemType.Id, itemType);
        }
    }

    // 获取物品类型
    public ItemType GetItemType(string itemTypeId)
    {
        return _itemTypes.ContainsKey(itemTypeId) ? _itemTypes[itemTypeId] : null;
    }

    // 创建仓库
    public void CreateInventory(string inventoryName, float maxWeight)
    {
        if (!_inventories.ContainsKey(inventoryName))
        {
            _inventories.Add(inventoryName, new Inventory(inventoryName, maxWeight));
        }
    }

    // 删除仓库
    public void DeleteInventory(string inventoryName)
    {
        if (_inventories.ContainsKey(inventoryName))
        {
            _inventories.Remove(inventoryName); // 从字典中移除仓库
            Debug.Log($"Inventory '{inventoryName}' has been deleted.");
        }
        else
        {
            Debug.LogWarning($"Inventory '{inventoryName}' does not exist.");
        }
    }

    // 向仓库添加物品
    public bool AddItemToInventory(string inventoryName, string itemTypeId, int quantity)
    {
        if (!_itemTypes.ContainsKey(itemTypeId))
        {
            Debug.LogWarning($"Item type {itemTypeId} does not exist.");
            return false;
        }

        if (!_inventories.ContainsKey(inventoryName))
        {
            Debug.LogWarning($"Inventory {inventoryName} does not exist.");
            return false;
        }

        // 创建新的物品实例
        ItemInstance newItem = new ItemInstance(_dataManager.GenerateUUID(), itemTypeId, quantity);
        return _inventories[inventoryName].AddItem(newItem, _itemTypes);
    }

    // 从仓库删除物品
    public void RemoveItemFromInventory(string inventoryName, string itemTypeId, int quantity)
    {
        if (_inventories.ContainsKey(inventoryName))
        {
            _inventories[inventoryName].RemoveItem(itemTypeId, quantity);
        }
    }

    // 获取仓库中的物品
    public ItemInstance GetItemFromInventory(string inventoryName, string itemTypeId)
    {
        return _inventories.ContainsKey(inventoryName) ? _inventories[inventoryName].GetItem(itemTypeId) : null;
    }

    // 计算仓库总重量
    public float CalculateInventoryWeight(string inventoryName)
    {
        return _inventories.ContainsKey(inventoryName) ? _inventories[inventoryName].CalculateTotalWeight(_itemTypes) : 0;
    }

    // 保存所有仓库数据
    public void SaveAllInventories()
    {
        foreach (var inventory in _inventories)
        {
            string json = JsonUtility.ToJson(inventory.Value);
            _dataManager.SetValue("Inventories", inventory.Key, json);
        }
    }

    // 加载所有仓库数据
    public void LoadAllInventories()
    {
        var inventoryKeys = _dataManager.GetAllKeys("Inventories");
        foreach (var key in inventoryKeys)
        {
            string json = _dataManager.GetValue<string>("Inventories", key, "");
            if (!string.IsNullOrEmpty(json))
            {
                Inventory inventory = JsonUtility.FromJson<Inventory>(json);
                _inventories[key] = inventory;
            }
        }
    }

    // 获取指定仓库中的所有物品实例
    public List<ItemInstance> GetAllItemsInInventory(string inventoryName)
    {
        if (_inventories.ContainsKey(inventoryName))
        {
            return _inventories[inventoryName].Items; // 返回仓库中的物品列表
        }
        else
        {
            Debug.LogWarning($"Inventory '{inventoryName}' does not exist.");
            return new List<ItemInstance>(); // 如果仓库不存在，返回空列表
        }
    }
}