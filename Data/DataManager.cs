using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string _savePath;
    private byte[] encryptionKey; // 暗号化キー

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/save.json";
    }

    public void SaveData(Data data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(_savePath, json);
    }

    public Data LoadData()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<Data>(json);
        }
        else
        {
            Debug.LogWarning("セーブデータが見つかりません。");
            return null;
        }
    }
}
