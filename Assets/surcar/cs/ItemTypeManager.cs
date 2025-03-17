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
        AddItemType("wood", "ľ��", "һ��ľͷ��", "wood", 1.5f, true, false, "");
        AddItemType("stone", "ʯͷ", "һ��ʯͷ��", "stone", 3.0f, false, false, "");
        AddItemType("axe", "ʯ��", "һ�Ѻֲܴڵ�ʯͷ���ӣ��ǳ������á�", "stoneAxe", 3.0f, false, true, "Weapon");
        // ���Լ�����Ӹ�����Ʒ����
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
                // ����ʹ����Ʒ���߼�
                Debug.Log($"��Ʒ {itemId} �ѱ�ʹ��");
                return true;
            }
            else
            {
                Debug.Log($"��Ʒ {itemId} ����ʹ��");
                return false;
            }
        }
        else
        {
            Debug.Log($"��Ʒ {itemId} ������");
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
            Debug.Log($"��Ʒ {itemId} ������");
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
            Debug.Log($"��Ʒ {itemId} ������");
            return false;
        }
    }
}