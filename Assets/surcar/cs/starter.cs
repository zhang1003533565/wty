using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starter : MonoBehaviour
{
    private void Awake()
    {
        //������awake֮������,��������������Ϸ����
        //DataManager.Instance.LoadDatabase("Default");

        ItemManager itemManager = ItemManager.Instance;
        //�����Ʒ����
        //addItemTypes(itemManager);

        //������Ʒ��Ϣ
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

        // �����Ʒ����
        //Sprite woodIcon = Resources.Load<Sprite>("Icons/wood");
        //ItemType wood = new ItemType("wood", "Wood", "A piece of wood.", woodIcon, 1.5f);
        //itemManager.AddItemType(wood);
        //addItemType("wood", "ľ��", "һ��ľͷ��", "wood", 1.5f, itemManager);
        //addItemType("stone", "ʯͷ", "һ��ʯͷ��", "stone", 3.0f, itemManager);

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
