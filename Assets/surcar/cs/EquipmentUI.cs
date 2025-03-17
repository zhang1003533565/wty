using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentPanel; // װ���������
    public Button closeButton; // �رհ�ť
    public Dictionary<string, Image> slotIcons = new Dictionary<string, Image>(); // ��λͼ��
    public Dictionary<string, Text> slotNames = new Dictionary<string, Text>(); // ��λ����
    public Dictionary<string, Button> slotButtons = new Dictionary<string, Button>(); // ��λж�ذ�ť

    public static EquipmentUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // ��ʼ��UIԪ��
        InitializeUI();
        // �󶨹رհ�ť�¼�
        closeButton.onClick.AddListener(CloseEquipmentPanel);
        // Ĭ�Ϲر�װ������
        equipmentPanel.SetActive(false);
    }

    private void Start()
    {
        
    }

    // ��ʼ��UIԪ��
    private void InitializeUI()
    {
        // ʾ�����󶨲�λ��ͼ�ꡢ���ƺͰ�ť
        BindSlotUI("Weapon");
        BindSlotUI("Clothes");
        BindSlotUI("Helmet");
        BindSlotUI("Shoes");
        BindSlotUI("Gloves");
        BindSlotUI("Pants");
    }

    // �󶨲�λ��UIԪ��
    private void BindSlotUI(string slot)
    {
        string slotPath = $"{slot}Slot/"; // �����λ��UI·��Ϊ��λ����+Slot
        slotIcons[slot] = equipmentPanel.transform.Find(slotPath + "Icon").GetComponent<Image>();
        slotNames[slot] = equipmentPanel.transform.Find(slotPath + "Name").GetComponent<Text>();
        slotButtons[slot] = equipmentPanel.transform.Find(slotPath + "Button").GetComponent<Button>();

        // ��ж�ذ�ť�¼�
        slotButtons[slot].onClick.AddListener(() => UnEquipItem(slot));
    }

    // ��װ������
    public void OpenEquipmentPanel()
    {
        equipmentPanel.SetActive(true);
        UpdateEquipmentUI();
    }

    // �ر�װ������
    private void CloseEquipmentPanel()
    {
        equipmentPanel.SetActive(false);
    }

    // ����װ������
    private void UpdateEquipmentUI()
    {
        var allEquipments = PlayerEquipmentManager.Instance.GetAllEquipments();
        foreach (var slot in allEquipments.Keys)
        {
            string equipmentId = allEquipments[slot];
            ItemType itemType = ItemManager.Instance.GetItemType(equipmentId);
            if (itemType != null)
            {
                // ��ʾͼ�������
                slotIcons[slot].sprite = itemType.Icon;
                slotNames[slot].text = itemType.Name;
                slotIcons[slot].gameObject.SetActive(true);
                slotNames[slot].gameObject.SetActive(true);
                slotButtons[slot].gameObject.SetActive(true);
            }
        }

        // ����δװ���Ĳ�λ
        var emptySlots = new List<string> { "Weapon", "Clothes", "Helmet", "Shoes", "Gloves", "Pants" };
        foreach (var slot in emptySlots)
        {
            if (!PlayerEquipmentManager.Instance.IsSlotOccupied(slot))
            {
                slotIcons[slot].gameObject.SetActive(false);
                slotNames[slot].gameObject.SetActive(false);
                slotButtons[slot].gameObject.SetActive(false);
            }
        }
    }

    // ж��װ��
    private void UnEquipItem(string slot)
    {
        string equipmentId = PlayerEquipmentManager.Instance.GetEquipment(slot);
        if (!string.IsNullOrEmpty(equipmentId))
        {
            // ���Խ�װ�����뱳��
            bool success = ItemManager.Instance.AddItemToInventory("PlayerInventory", equipmentId, 1);
            if (success)
            {
                // ��װ�������Ƴ�
                PlayerEquipmentManager.Instance.RemoveEquipment(slot);
                UpdateEquipmentUI();
                MessageBox.instance.Show("װ���ɹ�ж�أ�");
            }
            else
            {
                MessageBox.instance.Show("�����������޷�ж�أ�");
            }
        }
    }
}