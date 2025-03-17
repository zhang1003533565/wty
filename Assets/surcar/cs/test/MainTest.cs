using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //创建测试储物箱
        ItemManager itemManager = ItemManager.Instance;
        itemManager.CreateInventory("box-test1", 100);
        itemManager.AddItemToInventory("box-test1", "wood", 8);

        itemManager.CreateInventory("PlayerInventory", 9999);
        //itemManager.AddItemToInventory("PlayerInventory", "wood", 5);
        //itemManager.AddItemToInventory("PlayerInventory", "stone", 10);
        //itemManager.AddItemToInventory("PlayerInventory", "axe", 1);

        //测试物品ui，调起物品UI
        //InventoryUI.Instance.OpenInventory("PlayerInventory", "我的背包", "box-test1", "储物箱");

        //测试装备UI
        //PlayerEquipmentManager.Instance.SetEquipment("Weapon", "wood");
        //PlayerEquipmentManager.Instance.SetEquipment("Shoes", "stone");
        //EquipmentUI.instance.OpenEquipmentPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
