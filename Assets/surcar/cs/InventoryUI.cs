using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryUIPanel;
    public Button CloseButton;
    public GameObject itemPrefab; // ��Ʒ�б����Ԥ����
    public Transform itemListParent; // ��Ʒ�б�ĸ�����
    public Text currentInventoryNameText; // ��ǰ�ֿ������ı�
    public Button switchInventoryButton; // �л��ֿⰴť
    public Text selectedItemNameText; // ѡ����Ʒ����
    public Image selectedItemIconImage; // ѡ����Ʒͼ��
    public Text selectedItemDescriptionText; // ѡ����Ʒ����
    public Button useButton; // ʹ�ð�ť
    public Button setToToolbarButton; // ���õ���������ť
    public Button discardButton; // ������ť
    public Button transferButton; // ת�ư�ť
    public InputPopupUI inputPopup; // �������뵯��

    private string _currentInventoryName; // ��ǰ�ֿ�����
    private string _otherInventoryName; // ��һ���ֿ�����
    private string _currentInventoryTitle; // ��ǰ�ֿ���ʾ����
    private string _otherInventoryTitle; // ��һ���ֿ���ʾ����
    private ItemInstance _selectedItem; // ��ǰѡ�е���Ʒ
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

    // �򿪲ֿ�UI
    public void OpenInventory(string inventoryName, string inventoryTitle, string otherInventoryName = null, string otherInventoryTitle = null)
    {
        _currentInventoryName = inventoryName;
        _otherInventoryName = otherInventoryName;
        _currentInventoryTitle = inventoryTitle;
        _otherInventoryTitle = otherInventoryTitle;
        RefreshUI();
        InventoryUIPanel.SetActive(true);
    }

    // ˢ��UI
    private void RefreshUI()
    {
        // �����Ʒ�б�
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        // ��ʾ��ǰ�ֿ�����
        currentInventoryNameText.text = _currentInventoryTitle;

        // �Ƿ���ʾ�л���ť
        switchInventoryButton.gameObject.SetActive(_otherInventoryName != null);
        if (_otherInventoryName != null)
        {
            switchInventoryButton.GetComponentInChildren<Text>().text = "�л�����" + _otherInventoryTitle;
        }

        // ������Ʒ�б�
        var items = _itemManager.GetAllItemsInInventory(_currentInventoryName);
        foreach (var item in items)
        {
            var itemUI = Instantiate(itemPrefab, itemListParent).GetComponent<InventoryItemUI>();
            itemUI.Init(item, OnItemSelected);
        }

        // ���ѡ����Ʒ
        _selectedItem = null;
        HideSelectedItemInfo();
    }

    // ����ѡ����Ʒ����Ϣ
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

    // ��Ʒѡ���¼�
    private void OnItemSelected(ItemInstance item)
    {
        _selectedItem = item;
        ItemType itemType = _itemManager.GetItemType(item.ItemTypeId);
        selectedItemNameText.text = itemType.Name;
        selectedItemIconImage.sprite = itemType.Icon;
        selectedItemDescriptionText.text = itemType.Description;

        // ��ʾ�Ҳ���Ϣ
        selectedItemIconImage.gameObject.SetActive(true);
        useButton.gameObject.SetActive(itemType.canUse);
        setToToolbarButton.gameObject.SetActive(_currentInventoryName == "PlayerInventory" && itemType.canEquip);
        discardButton.gameObject.SetActive(true);
        transferButton.gameObject.SetActive(_otherInventoryName != null);
    }

    // �л��ֿ�
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

    // ʹ����Ʒ
    private void OnUseClicked()
    {
        if (_selectedItem != null)
        {
            bool useSuccess = ItemTypeManager.Instance.UseItem(_selectedItem.ItemTypeId);
            if (useSuccess)
            {
                // ʹ�óɹ����ӱ����м�ȥ1������
                _itemManager.RemoveItemFromInventory(_currentInventoryName, _selectedItem.ItemTypeId, 1);

                // �����Ʒ�Ƿ���ʣ������
                var remainingItems = _itemManager.GetAllItemsInInventory(_currentInventoryName);
                bool itemStillExists = remainingItems.Exists(i => i.ItemTypeId == _selectedItem.ItemTypeId);

                /*if (itemStillExists)
                {
                    // �����Ʒ����ʣ�࣬������ʾ
                    OnItemSelected(_selectedItem);
                }
                else
                {
                    // �����Ʒû��ʣ�࣬�����Ҳ���Ϣ
                    HideSelectedItemInfo();
                }*/
                RefreshUI();

                Debug.Log("ʹ����Ʒ�ɹ���");
                MessageBox.instance.Show("��Ʒʹ�����!");
            }
            else
            {
                Debug.Log("ʹ����Ʒʧ�ܡ�");
                MessageBox.instance.Show("��Ʒʹ��ʧ��!");
            }
        }
    }

    // װ����Ʒ
    private void OnSetToToolbarClicked()
    {
        if (_selectedItem != null)
        {
            ItemType itemType = _itemManager.GetItemType(_selectedItem.ItemTypeId);
            // ���ȼ��װ�����Ƿ��Ѿ���װ��
            if (!string.IsNullOrEmpty(itemType.equipSlot))
            {
                string equipmentId = PlayerEquipmentManager.Instance.GetEquipment(itemType.equipSlot);
                if (!string.IsNullOrEmpty(equipmentId))
                {
                    // ���Խ���ǰװ�����뱳��
                    bool addSuccess = ItemManager.Instance.AddItemToInventory("PlayerInventory", equipmentId, 1);
                    if (!addSuccess)
                    {
                        MessageBox.instance.Show("�����������޷�����װ����");
                        return;
                    }
                    // ��װ�������Ƴ�
                    PlayerEquipmentManager.Instance.RemoveEquipment(itemType.equipSlot);
                }
                // ����װ������װ����
                PlayerEquipmentManager.Instance.SetEquipment(itemType.equipSlot, _selectedItem.ItemTypeId);
                // �ӱ������Ƴ���װ��
                _itemManager.RemoveItemFromInventory("PlayerInventory", _selectedItem.ItemTypeId, 1);
                MessageBox.instance.Show("װ���ɹ���");
                RefreshUI();
            }
            else
            {
                Debug.Log("����Ʒû��ָ����װ���ۡ�");
                MessageBox.instance.Show("����Ʒû��ָ����װ���ۣ�");
            }
        }
    }

    // ������Ʒ
    private void OnDiscardClicked()
    {
        if (_selectedItem != null)
        {
            // ���������뵯��
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

    // ת����Ʒ
    private void OnTransferClicked()
    {
        if (_selectedItem != null && _otherInventoryName != null)
        {
            // ���������뵯��
            inputPopup.Initialize(
                onConfirm: quantity =>
                {
                    // �ȳ��Խ���Ʒ��ӵ�Ŀ����
                    bool addSuccess = _itemManager.AddItemToInventory(_otherInventoryName, _selectedItem.ItemTypeId, quantity);
                    if (addSuccess)
                    {
                        // �����ӳɹ����ٴ�Դ������Ƴ���Ʒ
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