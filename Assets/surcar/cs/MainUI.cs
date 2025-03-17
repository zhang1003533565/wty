using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button InventoryBtn;
    public Button EquipmentBtn;

    // Start is called before the first frame update
    void Start()
    {
        InventoryBtn.onClick.AddListener(OnInventoryBtn);
        EquipmentBtn.onClick.AddListener(OnEquipmentBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInventoryBtn()
    {
        //InventoryUI.Instance.OpenInventory("PlayerInventory", "我的背包", "box-test1", "储物箱");
        InventoryUI.Instance.OpenInventory("PlayerInventory", "我的背包");
    }

    public void OnEquipmentBtn()
    {
        EquipmentUI.instance.OpenEquipmentPanel();
    }
}
