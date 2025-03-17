using UnityEngine;

public class DataManagerTest : MonoBehaviour
{
    private void Start()
    {
        TestDataManager();
    }

    private void TestDataManager()
    {
        // ��ʼ�����ݹ�����
        DataManager dataManager = DataManager.Instance;

        // ����һ�� UUID
        string uuid = dataManager.GenerateUUID();
        Debug.Log($"Generated UUID: {uuid}");

        // �������úͻ�ȡֵ
        TestSetAndGetValue(dataManager, uuid);

        // ����ɾ����
        TestDeleteKey(dataManager, uuid);

        // ���Ա���ͼ������ݿ�
        TestSaveAndLoadDatabase(dataManager, uuid);

        Debug.Log("All tests completed.");
    }

    private void TestSetAndGetValue(DataManager dataManager, string uuid)
    {
        Debug.Log("Testing SetValue and GetValue...");

        // ����ֵ
        dataManager.SetValue(uuid, "PlayerName", "John Doe");
        dataManager.SetValue(uuid, "Health", 100);
        dataManager.SetValue(uuid, "Stamina", 75.5f);

        // ��ȡֵ
        string playerName = dataManager.GetValue<string>(uuid, "PlayerName", "Unknown");
        int health = dataManager.GetValue<int>(uuid, "Health", 0);
        float stamina = dataManager.GetValue<float>(uuid, "Stamina", 0.0f);

        // ��֤���
        Debug.Assert(playerName == "John Doe", $"Name should be 'John Doe', but got '{playerName}'");
        Debug.Assert(health == 100, $"Health should be 100, but got {health}");
        Debug.Assert(Mathf.Approximately(stamina, 75.5f), $"Stamina should be 75.5, but got {stamina}");

        Debug.Log("SetValue and GetValue tests passed.");
    }



    private void TestDeleteKey(DataManager dataManager, string uuid)
    {
        Debug.Log("Testing DeleteKey...");

        // ɾ����
        dataManager.DeleteKey(uuid, "Health");

        // ���Ի�ȡ��ɾ���ļ�
        int health = dataManager.GetValue<int>(uuid, "Health", -1);

        // ��֤���
        Debug.Assert(health == -1, $"Health should be -1 (default), but got {health}");

        Debug.Log("DeleteKey test passed.");
    }

    private void TestSaveAndLoadDatabase(DataManager dataManager, string uuid)
    {
        Debug.Log("Testing SaveDatabase and LoadDatabase...");

        // ����ֵ
        dataManager.SetValue(uuid, "Level", 5);

        // �������ݿ�
        dataManager.SaveDatabase("Default");

        // ����ڴ��е�����
        dataManager.LoadDatabase("NonExistentDatabase");

        // �������ݿ�
        dataManager.LoadDatabase("Default");

        // ��ȡֵ
        int level = dataManager.GetValue<int>(uuid, "Level", 0);

        // ��֤���
        Debug.Assert(level == 5, $"Level should be 5, but got {level}");

        Debug.Log("SaveDatabase and LoadDatabase tests passed.");
    }
}