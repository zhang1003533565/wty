using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance;

    // װ����λ����
    private const string WEAPON_SLOT = "Weapon";
    private const string CLOTHES_SLOT = "Clothes";
    private const string HELMET_SLOT = "Helmet";
    private const string SHOES_SLOT = "Shoes";
    private const string GLOVES_SLOT = "Gloves";
    private const string PANTS_SLOT = "Pants";

    // �̶������װ�� UUID
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
        // ��ʼ��ʱ����װ������
        Debug.Log($"Player Equipment UUID: {PLAYER_EQUIPMENT_UUID}");
    }

    // ��ȡ��ǰװ��
    public string GetEquipment(string slot)
    {
        return DataManager.Instance.GetValue(PLAYER_EQUIPMENT_UUID, slot, string.Empty);
    }

    // ����װ��
    public void SetEquipment(string slot, string equipmentId)
    {
        DataManager.Instance.SetValue(PLAYER_EQUIPMENT_UUID, slot, equipmentId);
        Debug.Log($"Equipped {equipmentId} in slot {slot}");
    }

    // �Ƴ�װ��
    public void RemoveEquipment(string slot)
    {
        DataManager.Instance.DeleteKey(PLAYER_EQUIPMENT_UUID, slot);
        Debug.Log($"Removed equipment from slot {slot}");
    }

    // ��ȡ����װ����Ϣ
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

    // ���ĳ����λ�Ƿ��Ѿ�װ��
    public bool IsSlotOccupied(string slot)
    {
        return !string.IsNullOrEmpty(GetEquipment(slot));
    }
}