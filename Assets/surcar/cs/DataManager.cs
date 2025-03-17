using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ValueType { String, Float, Int }

[System.Serializable]
public class Database
{
    public string databaseName;
    public List<DataSet> dataSetList = new List<DataSet>(); // 使用 List 替代 Dictionary
}

[System.Serializable]
public class DataSet
{
    public string uuid;
    public List<DataEntry> entries = new List<DataEntry>(); // 使用 List 替代 Dictionary
}

[System.Serializable]
public class DataEntry
{
    public string key;
    public DataValue value;
}

[System.Serializable]
public class DataValue
{
    public ValueType valueType;
    public string stringValue;
    public float floatValue;
    public int intValue;
}


public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance => _instance;

    private Dictionary<string, Database> _databases = new Dictionary<string, Database>();
    private string _currentDatabase = "Default";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDatabase(_currentDatabase);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 核心接口
    public T GetValue<T>(string uuid, string key, T defaultValue)
    {
        if (!_databases[_currentDatabase].dataSetList.Exists(d => d.uuid == uuid))
            return defaultValue;

        var dataSet = _databases[_currentDatabase].dataSetList.Find(d => d.uuid == uuid);
        var entry = dataSet.entries.Find(e => e.key == key);

        if (entry == null)
            return defaultValue;

        try
        {
            return typeof(T) switch
            {
                Type t when t == typeof(string) => (T)(object)entry.value.stringValue,
                Type t when t == typeof(int) => (T)(object)entry.value.intValue,
                Type t when t == typeof(float) => (T)(object)entry.value.floatValue,
                _ => defaultValue
            };
        }
        catch
        {
            return defaultValue;
        }
    }

    public void SetValue<T>(string uuid, string key, T value)
    {
        if (!_databases.ContainsKey(_currentDatabase))
        {
            _databases[_currentDatabase] = new Database { databaseName = _currentDatabase };
        }

        var database = _databases[_currentDatabase];
        var dataSet = database.dataSetList.Find(d => d.uuid == uuid);

        if (dataSet == null)
        {
            dataSet = new DataSet { uuid = uuid };
            database.dataSetList.Add(dataSet);
        }

        var entry = dataSet.entries.Find(e => e.key == key);

        if (entry == null)
        {
            entry = new DataEntry { key = key, value = new DataValue() };
            dataSet.entries.Add(entry);
        }

        if (value is string str)
        {
            entry.value.valueType = ValueType.String;
            entry.value.stringValue = str;
        }
        else if (value is int i)
        {
            entry.value.valueType = ValueType.Int;
            entry.value.intValue = i;
        }
        else if (value is float f)
        {
            entry.value.valueType = ValueType.Float;
            entry.value.floatValue = f;
        }
        else
        {
            throw new ArgumentException("Unsupported value type");
        }
    }

    public void DeleteKey(string uuid, string key)
    {
        var database = _databases[_currentDatabase];
        var dataSet = database.dataSetList.Find(d => d.uuid == uuid);

        if (dataSet != null)
        {
            dataSet.entries.RemoveAll(e => e.key == key);
        }
    }

    public void SaveDatabase(string dbName)
    {
        if (_databases.TryGetValue(dbName, out Database database))
        {
            string json = JsonUtility.ToJson(database, true);
            string path = GetDatabasePath(dbName);
            File.WriteAllText(path, json);
            Debug.Log($"Saved database {dbName} to {path}");
        }
        else
        {
            Debug.LogWarning($"Database {dbName} does not exist.");
        }
    }

    public void LoadDatabase(string dbName)
    {
        string path = GetDatabasePath(dbName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Database database = JsonUtility.FromJson<Database>(json);
            _databases[dbName] = database;
            Debug.Log($"Loaded database {dbName} from {path}");
        }
        else
        {
            _databases[dbName] = new Database { databaseName = dbName };
            Debug.Log($"Created new database {dbName}");
        }
    }

    private string GetDatabasePath(string dbName)
    {
        return Path.Combine(Application.persistentDataPath, $"{dbName}.json");
    }

    public string GenerateUUID()
    {
        return Guid.NewGuid().ToString();
    }

    public List<string> GetAllKeys(string uuid)
    {
        if (!_databases.TryGetValue(_currentDatabase, out Database database))
        {
            Debug.LogWarning($"Database {_currentDatabase} does not exist.");
            return new List<string>();
        }

        var dataSet = database.dataSetList.Find(d => d.uuid == uuid);
        if (dataSet == null)
        {
            Debug.LogWarning($"DataSet with UUID {uuid} does not exist in database {_currentDatabase}.");
            return new List<string>();
        }

        return dataSet.entries.ConvertAll(e => e.key);
    }
}