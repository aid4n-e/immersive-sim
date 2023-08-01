using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBank : MonoBehaviour
{
    public Dialogue dialogue = new Dialogue();

    public string fileName;

    public bool manualSave = false;
    public bool manualLoad = false;

    void Start()
    {

    }

    void Update()
    {
        if(manualSave)
        {
            manualSave = false;
            SavetoJSON(fileName);
        }
        if (manualLoad)
        {
            manualLoad = false;
            LoadfromJSON(fileName);
        }
    }

    public void SavetoJSON(string name)
    {
        string dialogueData = JsonUtility.ToJson(dialogue);
        string filePath = Application.dataPath + "/Data/Dialogue/" + name + "Dialogue.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, dialogueData);
        Debug.Log("Data saved");
    }

    public void LoadfromJSON(string name)
    {
        string filePath = Application.dataPath + "/Data/Dialogue/"+ name +"Dialogue.json";
        string dialogueData = System.IO.File.ReadAllText(filePath);

        dialogue = JsonUtility.FromJson<Dialogue>(dialogueData);
        Debug.Log("Data loaded");
    }
}

[System.Serializable]
public class Dialogue
{

    public TextStruct[] dialogue;


}

[System.Serializable]
public struct TextStruct
{
    public string[] dialogue;
}