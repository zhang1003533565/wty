using UnityEngine;

public class ItemSystemTest : MonoBehaviour
{
    private void Start()
    {
        TestItemManager();
    }

    private void TestItemManager()
    {
        // ��ʼ�� ItemManager
        ItemManager itemManager = ItemManager.Instance;

        // �����Ʒ����
        //ItemType wood = new ItemType("wood", "Wood", "A piece of wood.", null, 1.5f);
        //ItemType stone = new ItemType("stone", "Stone", "A piece of stone.", null, 3.0f);
        //itemManager.AddItemType(wood);
        //itemManager.AddItemType(stone);

        Debug.Log("Added item types: Wood and Stone.");

        // �����ֿ�
        itemManager.CreateInventory("PlayerInventory", 100);
        Debug.Log("Created inventory 'PlayerInventory' with max weight 100.");

        // �����Ʒ���ֿ�
        bool success1 = itemManager.AddItemToInventory("PlayerInventory", "wood", 10);
        Debug.Log(success1 ? "Added 10 Wood to PlayerInventory." : "Failed to add Wood.");

        bool success2 = itemManager.AddItemToInventory("PlayerInventory", "stone", 5);
        Debug.Log(success2 ? "Added 5 Stone to PlayerInventory." : "Failed to add Stone.");

        // ���ֿ�������
        float weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory: {weight}");

        // ������ӳ����ֿ���ص���Ʒ
        bool success3 = itemManager.AddItemToInventory("PlayerInventory", "wood", 100);
        Debug.Log(success3 ? "Added 100 Wood to PlayerInventory." : "Failed to add Wood (exceeds max weight).");

        // ���ֿ�������
        weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory after failed add: {weight}");

        // ɾ����Ʒ
        itemManager.RemoveItemFromInventory("PlayerInventory", "wood", 5);
        Debug.Log("Removed 5 Wood from PlayerInventory.");

        // ���ֿ�������
        weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory after removal: {weight}");

        // ������Ʒ
        ItemInstance woodItem = itemManager.GetItemFromInventory("PlayerInventory", "wood");
        Debug.Log(woodItem != null ? $"Found Wood in PlayerInventory. Quantity: {woodItem.Quantity}" : "Wood not found in PlayerInventory.");

        // ����ֿ�����
        itemManager.SaveAllInventories();
        Debug.Log("Saved all inventories.");

        DataManager.Instance.SaveDatabase("Default");

        // ����ڴ��еĲֿ�����
        itemManager.LoadAllInventories();
        Debug.Log("Loaded all inventories from saved data.");

        // ���¼��ֿ�������
        weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory after loading: {weight}");

        // �����²ֿ�
        itemManager.CreateInventory("ChestInventory", 50);
        Debug.Log("Created inventory 'ChestInventory' with max weight 50.");

        bool success4 = itemManager.AddItemToInventory("ChestInventory", "stone", 10);
        Debug.Log(success4 ? "Added 10 Stone to ChestInventory." : "Failed to add Stone to ChestInventory.");

        // ��� ChestInventory ������
        weight = itemManager.CalculateInventoryWeight("ChestInventory");
        Debug.Log($"Current weight of ChestInventory: {weight}");
    }
}