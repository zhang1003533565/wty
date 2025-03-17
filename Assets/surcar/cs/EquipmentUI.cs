using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentPanel; // 装备界面面板
    public Button closeButton; // 关闭按钮
    public Dictionary<string, Image> slotIcons = new Dictionary<string, Image>(); // 槽位图标
    public Dictionary<string, Text> slotNames = new Dictionary<string, Text>(); // 槽位名称
    public Dictionary<string, Button> slotButtons = new Dictionary<string, Button>(); // 槽位卸载按钮

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

        // 初始化UI元素
        InitializeUI();
        // 绑定关闭按钮事件
        closeButton.onClick.AddListener(CloseEquipmentPanel);
        // 默认关闭装备界面
        equipmentPanel.SetActive(false);
    }

    private void Start()
    {
        
    }

    // 初始化UI元素
    private void InitializeUI()
    {
        // 示例：绑定槽位的图标、名称和按钮
        BindSlotUI("Weapon");
        BindSlotUI("Clothes");
        BindSlotUI("Helmet");
        BindSlotUI("Shoes");
        BindSlotUI("Gloves");
        BindSlotUI("Pants");
    }

    // 绑定槽位的UI元素
    private void BindSlotUI(string slot)
    {
        string slotPath = $"{slot}Slot/"; // 假设槽位的UI路径为槽位名称+Slot
        slotIcons[slot] = equipmentPanel.transform.Find(slotPath + "Icon").GetComponent<Image>();
        slotNames[slot] = equipmentPanel.transform.Find(slotPath + "Name").GetComponent<Text>();
        slotButtons[slot] = equipmentPanel.transform.Find(slotPath + "Button").GetComponent<Button>();

        // 绑定卸载按钮事件
        slotButtons[slot].onClick.AddListener(() => UnEquipItem(slot));
    }

    // 打开装备界面
    public void OpenEquipmentPanel()
    {
        equipmentPanel.SetActive(true);
        UpdateEquipmentUI();
    }

    // 关闭装备界面
    private void CloseEquipmentPanel()
    {
        equipmentPanel.SetActive(false);
    }

    // 更新装备界面
    private void UpdateEquipmentUI()
    {
        var allEquipments = PlayerEquipmentManager.Instance.GetAllEquipments();
        foreach (var slot in allEquipments.Keys)
        {
            string equipmentId = allEquipments[slot];
            ItemType itemType = ItemManager.Instance.GetItemType(equipmentId);
            if (itemType != null)
            {
                // 显示图标和名称
                slotIcons[slot].sprite = itemType.Icon;
                slotNames[slot].text = itemType.Name;
                slotIcons[slot].gameObject.SetActive(true);
                slotNames[slot].gameObject.SetActive(true);
                slotButtons[slot].gameObject.SetActive(true);
            }
        }

        // 隐藏未装备的槽位
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

    // 卸载装备
    private void UnEquipItem(string slot)
    {
        string equipmentId = PlayerEquipmentManager.Instance.GetEquipment(slot);
        if (!string.IsNullOrEmpty(equipmentId))
        {
            // 尝试将装备放入背包
            bool success = ItemManager.Instance.AddItemToInventory("PlayerInventory", equipmentId, 1);
            if (success)
            {
                // 从装备槽中移除
                PlayerEquipmentManager.Instance.RemoveEquipment(slot);
                UpdateEquipmentUI();
                MessageBox.instance.Show("装备成功卸载！");
            }
            else
            {
                MessageBox.instance.Show("背包已满，无法卸载！");
            }
        }
    }
}