using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryUIPanel;
    public Button CloseButton;
    public GameObject itemPrefab; // 物品列表项的预制体
    public Transform itemListParent; // 物品列表的父对象
    public Text currentInventoryNameText; // 当前仓库名称文本
    public Button switchInventoryButton; // 切换仓库按钮
    public Text selectedItemNameText; // 选中物品名称
    public Image selectedItemIconImage; // 选中物品图标
    public Text selectedItemDescriptionText; // 选中物品描述
    public Button useButton; // 使用按钮
    public Button setToToolbarButton; // 设置到工具栏按钮
    public Button discardButton; // 丢弃按钮
    public Button transferButton; // 转移按钮
    public InputPopupUI inputPopup; // 数量输入弹框

    private string _currentInventoryName; // 当前仓库名称
    private string _otherInventoryName; // 另一个仓库名称
    private string _currentInventoryTitle; // 当前仓库显示名称
    private string _otherInventoryTitle; // 另一个仓库显示名称
    private ItemInstance _selectedItem; // 当前选中的物品
    private ItemManager _itemManager;

    public static InventoryUI Instance;

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

        _itemManager = ItemManager.Instance;
        CloseButton.onClick.AddListener(OnCloseBtn);
        switchInventoryButton.onClick.AddListener(SwitchInventory);
        useButton.onClick.AddListener(OnUseClicked);
        setToToolbarButton.onClick.AddListener(OnSetToToolbarClicked);
        discardButton.onClick.AddListener(OnDiscardClicked);
        transferButton.onClick.AddListener(OnTransferClicked);
    }

    // 打开仓库UI
    public void OpenInventory(string inventoryName, string inventoryTitle, string otherInventoryName = null, string otherInventoryTitle = null)
    {
        _currentInventoryName = inventoryName;
        _otherInventoryName = otherInventoryName;
        _currentInventoryTitle = inventoryTitle;
        _otherInventoryTitle = otherInventoryTitle;
        RefreshUI();
        InventoryUIPanel.SetActive(true);
    }

    // 刷新UI
    private void RefreshUI()
    {
        // 清空物品列表
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        // 显示当前仓库名称
        currentInventoryNameText.text = _currentInventoryTitle;

        // 是否显示切换按钮
        switchInventoryButton.gameObject.SetActive(_otherInventoryName != null);
        if (_otherInventoryName != null)
        {
            switchInventoryButton.GetComponentInChildren<Text>().text = "切换至：" + _otherInventoryTitle;
        }

        // 加载物品列表
        var items = _itemManager.GetAllItemsInInventory(_currentInventoryName);
        foreach (var item in items)
        {
            var itemUI = Instantiate(itemPrefab, itemListParent).GetComponent<InventoryItemUI>();
            itemUI.Init(item, OnItemSelected);
        }

        // 清空选中物品
        _selectedItem = null;
        HideSelectedItemInfo();
    }

    // 隐藏选中物品的信息
    private void HideSelectedItemInfo()
    {
        selectedItemNameText.text = "";
        selectedItemIconImage.sprite = null;
        selectedItemDescriptionText.text = "";
        selectedItemIconImage.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);
        setToToolbarButton.gameObject.SetActive(false);
        discardButton.gameObject.SetActive(false);
        transferButton.gameObject.SetActive(false);
    }

    // 物品选中事件
    private void OnItemSelected(ItemInstance item)
    {
        _selectedItem = item;
        ItemType itemType = _itemManager.GetItemType(item.ItemTypeId);
        selectedItemNameText.text = itemType.Name;
        selectedItemIconImage.sprite = itemType.Icon;
        selectedItemDescriptionText.text = itemType.Description;

        // 显示右侧信息
        selectedItemIconImage.gameObject.SetActive(true);
        useButton.gameObject.SetActive(itemType.canUse);
        setToToolbarButton.gameObject.SetActive(_currentInventoryName == "PlayerInventory" && itemType.canEquip);
        discardButton.gameObject.SetActive(true);
        transferButton.gameObject.SetActive(_otherInventoryName != null);
    }

    // 切换仓库
    private void SwitchInventory()
    {
        var temp = _currentInventoryName;
        var temptitle = _currentInventoryTitle;
        _currentInventoryName = _otherInventoryName;
        _currentInventoryTitle = _otherInventoryTitle;
        _otherInventoryName = temp;
        _otherInventoryTitle = temptitle;
        RefreshUI();
    }

    // 使用物品
    private void OnUseClicked()
    {
        if (_selectedItem != null)
        {
            bool useSuccess = ItemTypeManager.Instance.UseItem(_selectedItem.ItemTypeId);
            if (useSuccess)
            {
                // 使用成功，从背包中减去1个数量
                _itemManager.RemoveItemFromInventory(_currentInventoryName, _selectedItem.ItemTypeId, 1);

                // 检查物品是否还有剩余数量
                var remainingItems = _itemManager.GetAllItemsInInventory(_currentInventoryName);
                bool itemStillExists = remainingItems.Exists(i => i.ItemTypeId == _selectedItem.ItemTypeId);

                /*if (itemStillExists)
                {
                    // 如果物品还有剩余，更新显示
                    OnItemSelected(_selectedItem);
                }
                else
                {
                    // 如果物品没有剩余，隐藏右侧信息
                    HideSelectedItemInfo();
                }*/
                RefreshUI();

                Debug.Log("使用物品成功。");
                MessageBox.instance.Show("物品使用完毕!");
            }
            else
            {
                Debug.Log("使用物品失败。");
                MessageBox.instance.Show("物品使用失败!");
            }
        }
    }

    // 装备物品
    private void OnSetToToolbarClicked()
    {
        if (_selectedItem != null)
        {
            ItemType itemType = _itemManager.GetItemType(_selectedItem.ItemTypeId);
            // 首先检查装备槽是否已经有装备
            if (!string.IsNullOrEmpty(itemType.equipSlot))
            {
                string equipmentId = PlayerEquipmentManager.Instance.GetEquipment(itemType.equipSlot);
                if (!string.IsNullOrEmpty(equipmentId))
                {
                    // 尝试将当前装备放入背包
                    bool addSuccess = ItemManager.Instance.AddItemToInventory("PlayerInventory", equipmentId, 1);
                    if (!addSuccess)
                    {
                        MessageBox.instance.Show("背包已满，无法更换装备！");
                        return;
                    }
                    // 从装备槽中移除
                    PlayerEquipmentManager.Instance.RemoveEquipment(itemType.equipSlot);
                }
                // 将新装备放入装备槽
                PlayerEquipmentManager.Instance.SetEquipment(itemType.equipSlot, _selectedItem.ItemTypeId);
                // 从背包中移除新装备
                _itemManager.RemoveItemFromInventory("PlayerInventory", _selectedItem.ItemTypeId, 1);
                MessageBox.instance.Show("装备成功！");
                RefreshUI();
            }
            else
            {
                Debug.Log("该物品没有指定的装备槽。");
                MessageBox.instance.Show("该物品没有指定的装备槽！");
            }
        }
    }

    // 丢弃物品
    private void OnDiscardClicked()
    {
        if (_selectedItem != null)
        {
            // 打开数量输入弹框
            inputPopup.Initialize(
                onConfirm: quantity =>
                {
                    _itemManager.RemoveItemFromInventory(_currentInventoryName, _selectedItem.ItemTypeId, quantity);
                    RefreshUI();
                },
                onCancel: () => Debug.Log("Discard operation cancelled."),
                defaultQuantity: _selectedItem.Quantity
            );
        }
    }

    // 转移物品
    private void OnTransferClicked()
    {
        if (_selectedItem != null && _otherInventoryName != null)
        {
            // 打开数量输入弹框
            inputPopup.Initialize(
                onConfirm: quantity =>
                {
                    // 先尝试将物品添加到目标库存
                    bool addSuccess = _itemManager.AddItemToInventory(_otherInventoryName, _selectedItem.ItemTypeId, quantity);
                    if (addSuccess)
                    {
                        // 如果添加成功，再从源库存中移除物品
                        _itemManager.RemoveItemFromInventory(_currentInventoryName, _selectedItem.ItemTypeId, quantity);
                        Debug.Log("Transfer successful.");
                    }
                    else
                    {
                        Debug.Log("Failed to add item to the target inventory. Transfer aborted.");
                    }
                    RefreshUI();
                },
                onCancel: () => Debug.Log("Transfer operation cancelled."),
                defaultQuantity: _selectedItem.Quantity
            );
        }
    }

    public void OnCloseBtn()
    {
        InventoryUIPanel.SetActive(false);
    }
}