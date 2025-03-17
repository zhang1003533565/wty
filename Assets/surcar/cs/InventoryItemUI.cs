using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryItemUI : MonoBehaviour
{
    public Image itemIcon; // 物品图标
    public Text itemNameText; // 物品名称
    public Text itemQuantityText; // 物品数量

    private ItemInstance _item; // 关联的物品实例
    private Action<ItemInstance> _onSelected; // 选择回调

    // 初始化
    public void Init(ItemInstance item, Action<ItemInstance> onSelected)
    {
        _item = item;
        _onSelected = onSelected;

        // 获取物品类型
        var itemType = ItemManager.Instance.GetItemType(item.ItemTypeId);

        // 更新UI
        itemIcon.sprite = itemType.Icon;
        itemNameText.text = itemType.Name;
        itemQuantityText.text = item.Quantity.ToString() + "个";//item.Quantity > 1 ? item.Quantity.ToString() + "个": "";

        // 绑定点击事件
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
    }

    // 点击事件
    private void OnItemClicked()
    {
        _onSelected?.Invoke(_item);
    }
}