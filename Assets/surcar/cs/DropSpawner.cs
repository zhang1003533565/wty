using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [Range(0, 1)] public float dropProbability; // 掉落概率 (0 到 1)
    public GameObject dropPrefab; // 怪物的预制体

    public void Drop()
    {
        if (Random.value <= dropProbability)
        {
            // 实例化掉落的物品
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
    }
}
