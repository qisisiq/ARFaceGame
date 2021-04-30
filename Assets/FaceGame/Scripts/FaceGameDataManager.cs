using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FaceGameDataManager : MonoBehaviour
{
    private static FaceGameDataManager s_DataManager = null;
    private List<FaceGameSongData> m_dataList;
    
    void Awake()
    {
        if (s_DataManager == null)
        {
            s_DataManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        // add the song data objects here. 
    }

    private void Save(FaceGameDataManager data, string fileName)
    {
        string jsonData = JsonUtility.ToJson(data);
        WriteToFile(fileName + ".txt", jsonData);
    }

    private void Load(string fileName)
    {
        var dataToOverwrite = new FaceGameSongData();
        string jsonData = ReadFromFile(fileName);
        JsonUtility.FromJsonOverwrite(jsonData, dataToOverwrite);
    }

    private void WriteToFile(string fileName, string data)
    {
        var path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(data);
        }
    }

    private string ReadFromFile(string fileName)
    {
        var path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                string res = streamReader.ReadToEnd();
                return res;
            }
        }
        else
        {
            Debug.LogWarning(fileName + " was not found");
            return "";
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
