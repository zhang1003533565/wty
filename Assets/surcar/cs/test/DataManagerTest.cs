using UnityEngine;

public class DataManagerTest : MonoBehaviour
{
    private void Start()
    {
        TestDataManager();
    }

    private void TestDataManager()
    {
        // 初始化数据管理器
        DataManager dataManager = DataManager.Instance;

        // 生成一个 UUID
        string uuid = dataManager.GenerateUUID();
        Debug.Log($"Generated UUID: {uuid}");

        // 测试设置和获取值
        TestSetAndGetValue(dataManager, uuid);

        // 测试删除键
        TestDeleteKey(dataManager, uuid);

        // 测试保存和加载数据库
        TestSaveAndLoadDatabase(dataManager, uuid);

        Debug.Log("All tests completed.");
    }

    private void TestSetAndGetValue(DataManager dataManager, string uuid)
    {
        Debug.Log("Testing SetValue and GetValue...");

        // 设置值
        dataManager.SetValue(uuid, "PlayerName", "John Doe");
        dataManager.SetValue(uuid, "Health", 100);
        dataManager.SetValue(uuid, "Stamina", 75.5f);

        // 获取值
        string playerName = dataManager.GetValue<string>(uuid, "PlayerName", "Unknown");
        int health = dataManager.GetValue<int>(uuid, "Health", 0);
        float stamina = dataManager.GetValue<float>(uuid, "Stamina", 0.0f);

        // 验证结果
        Debug.Assert(playerName == "John Doe", $"Name should be 'John Doe', but got '{playerName}'");
        Debug.Assert(health == 100, $"Health should be 100, but got {health}");
        Debug.Assert(Mathf.Approximately(stamina, 75.5f), $"Stamina should be 75.5, but got {stamina}");

        Debug.Log("SetValue and GetValue tests passed.");
    }



    private void TestDeleteKey(DataManager dataManager, string uuid)
    {
        Debug.Log("Testing DeleteKey...");

        // 删除键
        dataManager.DeleteKey(uuid, "Health");

        // 尝试获取已删除的键
        int health = dataManager.GetValue<int>(uuid, "Health", -1);

        // 验证结果
        Debug.Assert(health == -1, $"Health should be -1 (default), but got {health}");

        Debug.Log("DeleteKey test passed.");
    }

    private void TestSaveAndLoadDatabase(DataManager dataManager, string uuid)
    {
        Debug.Log("Testing SaveDatabase and LoadDatabase...");

        // 设置值
        dataManager.SetValue(uuid, "Level", 5);

        // 保存数据库
        dataManager.SaveDatabase("Default");

        // 清空内存中的数据
        dataManager.LoadDatabase("NonExistentDatabase");

        // 加载数据库
        dataManager.LoadDatabase("Default");

        // 获取值
        int level = dataManager.GetValue<int>(uuid, "Level", 0);

        // 验证结果
        Debug.Assert(level == 5, $"Level should be 5, but got {level}");

        Debug.Log("SaveDatabase and LoadDatabase tests passed.");
    }
}