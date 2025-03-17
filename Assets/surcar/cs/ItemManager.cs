using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemType
{
    public string Id; // ��Ʒ��Ψһ��ʶ��
    public string Name; // ��Ʒ����
    public string Description; // ��Ʒ����
    public Sprite Icon; // ��Ʒͼ��
    public float Weight; // ��Ʒ�ĵ�λ����
    public bool canUse; //�Ƿ��ʹ��
    public bool canEquip;//�Ƿ��װ��
    public string equipSlot;//�������װ������Ӧ�Ĳ�λ����

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
    public string UUID; // ��Ʒʵ����Ψһ��ʶ��
    public string ItemTypeId; // ��Ʒ���͵� ID
    public int Quantity; // ��Ʒ����

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
    public string InventoryName; // �ֿ�����
    public float MaxWeight; // �ֿ��������
    public List<ItemInstance> Items; // �ֿ��е���Ʒʵ���б�

    public Inventory(string inventoryName, float maxWeight)
    {
        InventoryName = inventoryName;
        MaxWeight = maxWeight;
        Items = new List<ItemInstance>();
    }

    // ����ֿ⵱ǰ������
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

    // �����Ʒ
    public bool AddItem(ItemInstance item, Dictionary<string, ItemType> itemTypes)
    {
        float itemWeight = itemTypes.ContainsKey(item.ItemTypeId) ? itemTypes[item.ItemTypeId].Weight : 0;
        if (CalculateTotalWeight(itemTypes) + itemWeight * item.Quantity > MaxWeight)
        {
            return false; // �����ֿ����
        }

        // �����Ƿ�������ͬ���͵���Ʒ
        var existingItem = Items.Find(i => i.ItemTypeId == item.ItemTypeId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity; // �ѵ�
        }
        else
        {
            Items.Add(item); // �������Ʒ
        }
        return true;
    }

    // ɾ����Ʒ
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

    // ������Ʒ
    public ItemInstance GetItem(string itemTypeId)
    {
        return Items.Find(i => i.ItemTypeId == itemTypeId);
    }
}


public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private Dictionary<string, ItemType> _itemTypes; // ������Ʒ����
    private Dictionary<string, Inventory> _inventories; // ���вֿ�
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

    // �����Ʒ����
    public void AddItemType(ItemType itemType)
    {
        if (!_itemTypes.ContainsKey(itemType.Id))
        {
            _itemTypes.Add(itemType.Id, itemType);
        }
    }

    // ��ȡ��Ʒ����
    public ItemType GetItemType(string itemTypeId)
    {
        return _itemTypes.ContainsKey(itemTypeId) ? _itemTypes[itemTypeId] : null;
    }

    // �����ֿ�
    public void CreateInventory(string inventoryName, float maxWeight)
    {
        if (!_inventories.ContainsKey(inventoryName))
        {
            _inventories.Add(inventoryName, new Inventory(inventoryName, maxWeight));
        }
    }

    // ɾ���ֿ�
    public void DeleteInventory(string inventoryName)
    {
        if (_inventories.ContainsKey(inventoryName))
        {
            _inventories.Remove(inventoryName); // ���ֵ����Ƴ��ֿ�
            Debug.Log($"Inventory '{inventoryName}' has been deleted.");
        }
        else
        {
            Debug.LogWarning($"Inventory '{inventoryName}' does not exist.");
        }
    }

    // ��ֿ������Ʒ
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

        // �����µ���Ʒʵ��
        ItemInstance newItem = new ItemInstance(_dataManager.GenerateUUID(), itemTypeId, quantity);
        return _inventories[inventoryName].AddItem(newItem, _itemTypes);
    }

    // �Ӳֿ�ɾ����Ʒ
    public void RemoveItemFromInventory(string inventoryName, string itemTypeId, int quantity)
    {
        if (_inventories.ContainsKey(inventoryName))
        {
            _inventories[inventoryName].RemoveItem(itemTypeId, quantity);
        }
    }

    // ��ȡ�ֿ��е���Ʒ
    public ItemInstance GetItemFromInventory(string inventoryName, string itemTypeId)
    {
        return _inventories.ContainsKey(inventoryName) ? _inventories[inventoryName].GetItem(itemTypeId) : null;
    }

    // ����ֿ�������
    public float CalculateInventoryWeight(string inventoryName)
    {
        return _inventories.ContainsKey(inventoryName) ? _inventories[inventoryName].CalculateTotalWeight(_itemTypes) : 0;
    }

    // �������вֿ�����
    public void SaveAllInventories()
    {
        foreach (var inventory in _inventories)
        {
            string json = JsonUtility.ToJson(inventory.Value);
            _dataManager.SetValue("Inventories", inventory.Key, json);
        }
    }

    // �������вֿ�����
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

    // ��ȡָ���ֿ��е�������Ʒʵ��
    public List<ItemInstance> GetAllItemsInInventory(string inventoryName)
    {
        if (_inventories.ContainsKey(inventoryName))
        {
            return _inventories[inventoryName].Items; // ���زֿ��е���Ʒ�б�
        }
        else
        {
            Debug.LogWarning($"Inventory '{inventoryName}' does not exist.");
            return new List<ItemInstance>(); // ����ֿⲻ���ڣ����ؿ��б�
        }
    }
}