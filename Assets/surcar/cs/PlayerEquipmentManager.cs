using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance;

    // 装备槽位常量
    private const string WEAPON_SLOT = "Weapon";
    private const string CLOTHES_SLOT = "Clothes";
    private const string HELMET_SLOT = "Helmet";
    private const string SHOES_SLOT = "Shoes";
    private const string GLOVES_SLOT = "Gloves";
    private const string PANTS_SLOT = "Pants";

    // 固定的玩家装备 UUID
    private const string PLAYER_EQUIPMENT_UUID = "playerweapons";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayerEquipment();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayerEquipment()
    {
        // 初始化时加载装备数据
        Debug.Log($"Player Equipment UUID: {PLAYER_EQUIPMENT_UUID}");
    }

    // 获取当前装备
    public string GetEquipment(string slot)
    {
        return DataManager.Instance.GetValue(PLAYER_EQUIPMENT_UUID, slot, string.Empty);
    }

    // 设置装备
    public void SetEquipment(string slot, string equipmentId)
    {
        DataManager.Instance.SetValue(PLAYER_EQUIPMENT_UUID, slot, equipmentId);
        Debug.Log($"Equipped {equipmentId} in slot {slot}");
    }

    // 移除装备
    public void RemoveEquipment(string slot)
    {
        DataManager.Instance.DeleteKey(PLAYER_EQUIPMENT_UUID, slot);
        Debug.Log($"Removed equipment from slot {slot}");
    }

    // 获取所有装备信息
    public Dictionary<string, string> GetAllEquipments()
    {
        var equipments = new Dictionary<string, string>();
        var slots = new List<string> { WEAPON_SLOT, CLOTHES_SLOT, HELMET_SLOT, SHOES_SLOT, GLOVES_SLOT, PANTS_SLOT };

        foreach (var slot in slots)
        {
            string equipmentId = GetEquipment(slot);
            if (!string.IsNullOrEmpty(equipmentId))
            {
                equipments[slot] = equipmentId;
            }
        }

        return equipments;
    }

    // 检查某个槽位是否已经装备
    public bool IsSlotOccupied(string slot)
    {
        return !string.IsNullOrEmpty(GetEquipment(slot));
    }
}