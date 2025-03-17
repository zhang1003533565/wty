using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance; // 单例模式，确保只有一个管理器存在

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 处理物体的重生
    /// </summary>
    /// <param name="prefab">需要重生的物体</param>
    /// <param name="position">重生位置</param>
    /// <param name="delay">重生延迟</param>
    public void Respawn(GameObject prefab, Vector3 position, float delay)
    {
        Debug.Log("生成物体");
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
