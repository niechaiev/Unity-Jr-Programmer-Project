using System;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Color TeamColor;
    private void Awake()
    {
        LoadColor();
    }

    [Serializable]
    class SaveData
    {
        public Color TeamColor;
    }

    public void SaveColor()
    {
        SaveData saveData = new()
        {
            TeamColor = TeamColor
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        
    }
    public void LoadColor()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            TeamColor = saveData.TeamColor;
        }
    }
}
