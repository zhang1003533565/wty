using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryItemUI : MonoBehaviour
{
    public Image itemIcon; // ��Ʒͼ��
    public Text itemNameText; // ��Ʒ����
    public Text itemQuantityText; // ��Ʒ����

    private ItemInstance _item; // ��������Ʒʵ��
    private Action<ItemInstance> _onSelected; // ѡ��ص�

    // ��ʼ��
    public void Init(ItemInstance item, Action<ItemInstance> onSelected)
    {
        _item = item;
        _onSelected = onSelected;

        // ��ȡ��Ʒ����
        var itemType = ItemManager.Instance.GetItemType(item.ItemTypeId);

        // ����UI
        itemIcon.sprite = itemType.Icon;
        itemNameText.text = itemType.Name;
        itemQuantityText.text = item.Quantity.ToString() + "��";//item.Quantity > 1 ? item.Quantity.ToString() + "��": "";

        // �󶨵���¼�
        GetComponent<Button>().onClick.AddListener(OnItemClicked);
    }

    // ����¼�
    private void OnItemClicked()
    {
        _onSelected?.Invoke(_item);
    }
}