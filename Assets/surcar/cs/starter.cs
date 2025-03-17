using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starter : MonoBehaviour
{
    private void Awake()
    {
        //再所有awake之后启动,负责启动整个游戏世界
        //DataManager.Instance.LoadDatabase("Default");

        ItemManager itemManager = ItemManager.Instance;
        //添加物品类型
        //addItemTypes(itemManager);

        //载入物品信息
        itemManager.LoadAllInventories();
    }

    private void addItemType(string id, string name, string desp, string icon, float weight, ItemManager itemManager)
    {
        //Sprite theIcon = Resources.Load<Sprite>("Icons/"+ icon);
        //ItemType wood = new ItemType(id, name, desp, theIcon, weight);
        //itemManager.AddItemType(wood);
    }

    private void addItemTypes(ItemManager itemManager)
    {

        // 添加物品类型
        //Sprite woodIcon = Resources.Load<Sprite>("Icons/wood");
        //ItemType wood = new ItemType("wood", "Wood", "A piece of wood.", woodIcon, 1.5f);
        //itemManager.AddItemType(wood);
        //addItemType("wood", "木材", "一块木头。", "wood", 1.5f, itemManager);
        //addItemType("stone", "石头", "一颗石头。", "stone", 3.0f, itemManager);

        //ItemType stone = new ItemType("stone", "Stone", "A piece of stone.", null, 3.0f);
        //itemManager.AddItemType(stone);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
