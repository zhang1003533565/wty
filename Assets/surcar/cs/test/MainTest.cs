using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�������Դ�����
        ItemManager itemManager = ItemManager.Instance;
        itemManager.CreateInventory("box-test1", 100);
        itemManager.AddItemToInventory("box-test1", "wood", 8);

        itemManager.CreateInventory("PlayerInventory", 9999);
        //itemManager.AddItemToInventory("PlayerInventory", "wood", 5);
        //itemManager.AddItemToInventory("PlayerInventory", "stone", 10);
        //itemManager.AddItemToInventory("PlayerInventory", "axe", 1);

        //������Ʒui��������ƷUI
        //InventoryUI.Instance.OpenInventory("PlayerInventory", "�ҵı���", "box-test1", "������");

        //����װ��UI
        //PlayerEquipmentManager.Instance.SetEquipment("Weapon", "wood");
        //PlayerEquipmentManager.Instance.SetEquipment("Shoes", "stone");
        //EquipmentUI.instance.OpenEquipmentPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
