using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StringIntPair
{
    public string itemTypeId; // ��Ʒ���� ID
    public int minCount; // ��С����
    public int maxCount; // �������
    [Range(0, 1)] public float probability; // ������� (0��1֮��)
}

public class DropItem : MonoBehaviour, IInteractable
{
    public string Name; // ����������
    public string id; // �������Ψһ��ʶ��

    public List<StringIntPair> dataList = new List<StringIntPair>(); // ������Ʒ�б�

    private bool _isInitialized = false; // �Ƿ��ѳ�ʼ��

    // Start is called before the first frame update
    void Start()
    {
        id = "TMP_" + System.Guid.NewGuid().ToString();
        Debug.Log("start DropItem " + Name + "id:" + id);

        // ��ʼ��������
        InitializeDropItem();
    }

    // ��ʼ��������
    private void InitializeDropItem()
    {
        ItemManager itemManager = ItemManager.Instance;
        itemManager.CreateInventory(id, 99999); // ����һ�����޸��ص���ʱ���

        foreach (StringIntPair pair in dataList)
        {
            // ���ݸ��ʾ����Ƿ�������Ʒ
            if (UnityEngine.Random.value <= pair.probability)
            {
                // ��������Χ�����������Ʒ����
                int count = UnityEngine.Random.Range(pair.minCount, pair.maxCount + 1);

                if(count > 0)
                    itemManager.AddItemToInventory(id, pair.itemTypeId, count);
            }
        }

        _isInitialized = true; // ���Ϊ�ѳ�ʼ��
    }

    // Update is called once per frame
    void Update()
    {
        // ���δ��ʼ��������
        if (!_isInitialized) return;

        // �����Ʒ�Ƿ��ÿ�
        ItemManager itemManager = ItemManager.Instance;
        if (itemManager.GetAllItemsInInventory(id).Count == 0)
        {
            itemManager.DeleteInventory(id);
            Destroy(gameObject); // �����Ʒ���ÿգ���������
        }
    }

    public bool IsInteractable()
    {
        return true;
    }

    public bool IsAttackable()
    {
        return false;
    }

    public string GetName()
    {
        return Name;
    }

    public void Interact()
    {
        //InventoryUI.Instance.OpenInventory("PlayerInventory", "�ҵı���", id, Name);
        InventoryUI.Instance.OpenInventory(id, Name, "PlayerInventory", "�ҵı���");
    }

    public void TakeDamage(string weaponName, int damage)
    {
        // ʵ�������߼��������Ҫ��
    }

    public bool isDeath()
    {
        return false;
    }
}
