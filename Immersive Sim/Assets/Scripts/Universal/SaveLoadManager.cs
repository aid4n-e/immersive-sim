using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    public SaveData save = new SaveData();

    public bool manualSave = false;
    public bool manualLoad = false;

    private void Start()
    {
    }

    private void Update()
    {   
        if(manualSave)
        {
            manualSave = false;
            SavetoJSON();
        }
        
        if(manualLoad)
        {
            manualLoad = false;
            LoadfromJSON();
        }
    }

    public void SavetoJSON()
    {
        string saveData = JsonUtility.ToJson(save);
        string filePath = Application.persistentDataPath + "/SaveData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath,saveData);
        Debug.Log("Data saved");
    }

    public void LoadfromJSON()
    {
        string filePath = Application.persistentDataPath + "/SaveData.json";
        string saveData = System.IO.File.ReadAllText(filePath);

        save = JsonUtility.FromJson<SaveData>(saveData);
        Debug.Log("Data loaded");
    }
}


[System.Serializable]
public class SaveData
{
    public float moralValue;
}
