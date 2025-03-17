using System.Collections.Generic;
using UnityEngine;

public class ItemTypeManager : MonoBehaviour
{
    private static ItemTypeManager _instance;
    public static ItemTypeManager Instance => _instance;

    private Dictionary<string, ItemType> _itemTypes = new Dictionary<string, ItemType>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            InitializeItemTypes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeItemTypes()
    {
        AddItemType("wood", "木材", "一块木头。", "wood", 1.5f, true, false, "");
        AddItemType("stone", "石头", "一颗石头。", "stone", 3.0f, false, false, "");
        AddItemType("axe", "石斧", "一把很粗糙的石头斧子，非常不好用。", "stoneAxe", 3.0f, false, true, "Weapon");
        // 可以继续添加更多物品类型
    }

    private void AddItemType(string id, string name, string description, string iconPath, float weight, bool canUse, bool canEquip, string equipSlot)
    {
        Sprite icon = Resources.Load<Sprite>("Icons/" + iconPath);
        ItemType itemType = new ItemType(id, name, description, icon, weight, canUse, canEquip, equipSlot);
        _itemTypes[id] = itemType;

        ItemManager.Instance.AddItemType(itemType);
    }

    public bool UseItem(string itemId)
    {
        if (_itemTypes.TryGetValue(itemId, out ItemType itemType))
        {
            if (itemType.canUse)
            {
                // 处理使用物品的逻辑
                Debug.Log($"物品 {itemId} 已被使用");
                return true;
            }
            else
            {
                Debug.Log($"物品 {itemId} 不能使用");
                return false;
            }
        }
        else
        {
            Debug.Log($"物品 {itemId} 不存在");
            return false;
        }
    }

    public bool CanUseItem(string itemId)
    {
        if (_itemTypes.TryGetValue(itemId, out ItemType itemType))
        {
            return itemType.canUse;
        }
        else
        {
            Debug.Log($"物品 {itemId} 不存在");
            return false;
        }
    }

    public bool CanEquipItem(string itemId)
    {
        if (_itemTypes.TryGetValue(itemId, out ItemType itemType))
        {
            return itemType.canEquip;
        }
        else
        {
            Debug.Log($"物品 {itemId} 不存在");
            return false;
        }
    }
}