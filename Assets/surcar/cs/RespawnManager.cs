using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance; // ����ģʽ��ȷ��ֻ��һ������������

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="prefab">��Ҫ����������</param>
    /// <param name="position">����λ��</param>
    /// <param name="delay">�����ӳ�</param>
    public void Respawn(GameObject prefab, Vector3 position, float delay)
    {
        Debug.Log("��������");
        StartCoroutine(RespawnCoroutine(prefab, position, delay));
    }

    private IEnumerator RespawnCoroutine(GameObject prefab, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
