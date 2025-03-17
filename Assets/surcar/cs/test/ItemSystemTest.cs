using UnityEngine;

public class ItemSystemTest : MonoBehaviour
{
    private void Start()
    {
        TestItemManager();
    }

    private void TestItemManager()
    {
        // 初始化 ItemManager
        ItemManager itemManager = ItemManager.Instance;

        // 添加物品类型
        //ItemType wood = new ItemType("wood", "Wood", "A piece of wood.", null, 1.5f);
        //ItemType stone = new ItemType("stone", "Stone", "A piece of stone.", null, 3.0f);
        //itemManager.AddItemType(wood);
        //itemManager.AddItemType(stone);

        Debug.Log("Added item types: Wood and Stone.");

        // 创建仓库
        itemManager.CreateInventory("PlayerInventory", 100);
        Debug.Log("Created inventory 'PlayerInventory' with max weight 100.");

        // 添加物品到仓库
        bool success1 = itemManager.AddItemToInventory("PlayerInventory", "wood", 10);
        Debug.Log(success1 ? "Added 10 Wood to PlayerInventory." : "Failed to add Wood.");

        bool success2 = itemManager.AddItemToInventory("PlayerInventory", "stone", 5);
        Debug.Log(success2 ? "Added 5 Stone to PlayerInventory." : "Failed to add Stone.");

        // 检查仓库总重量
        float weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory: {weight}");

        // 尝试添加超过仓库承重的物品
        bool success3 = itemManager.AddItemToInventory("PlayerInventory", "wood", 100);
        Debug.Log(success3 ? "Added 100 Wood to PlayerInventory." : "Failed to add Wood (exceeds max weight).");

        // 检查仓库总重量
        weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory after failed add: {weight}");

        // 删除物品
        itemManager.RemoveItemFromInventory("PlayerInventory", "wood", 5);
        Debug.Log("Removed 5 Wood from PlayerInventory.");

        // 检查仓库总重量
        weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory after removal: {weight}");

        // 查找物品
        ItemInstance woodItem = itemManager.GetItemFromInventory("PlayerInventory", "wood");
        Debug.Log(woodItem != null ? $"Found Wood in PlayerInventory. Quantity: {woodItem.Quantity}" : "Wood not found in PlayerInventory.");

        // 保存仓库数据
        itemManager.SaveAllInventories();
        Debug.Log("Saved all inventories.");

        DataManager.Instance.SaveDatabase("Default");

        // 清空内存中的仓库数据
        itemManager.LoadAllInventories();
        Debug.Log("Loaded all inventories from saved data.");

        // 重新检查仓库总重量
        weight = itemManager.CalculateInventoryWeight("PlayerInventory");
        Debug.Log($"Current weight of PlayerInventory after loading: {weight}");

        // 测试新仓库
        itemManager.CreateInventory("ChestInventory", 50);
        Debug.Log("Created inventory 'ChestInventory' with max weight 50.");

        bool success4 = itemManager.AddItemToInventory("ChestInventory", "stone", 10);
        Debug.Log(success4 ? "Added 10 Stone to ChestInventory." : "Failed to add Stone to ChestInventory.");

        // 检查 ChestInventory 总重量
        weight = itemManager.CalculateInventoryWeight("ChestInventory");
        Debug.Log($"Current weight of ChestInventory: {weight}");
    }
}