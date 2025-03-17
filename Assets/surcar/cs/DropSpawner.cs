using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [Range(0, 1)] public float dropProbability; // ������� (0 �� 1)
    public GameObject dropPrefab; // �����Ԥ����

    public void Drop()
    {
        if (Random.value <= dropProbability)
        {
            // ʵ�����������Ʒ
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
    }
}
