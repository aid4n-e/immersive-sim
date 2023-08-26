using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using static Unity.VisualScripting.Member;

[CreateAssetMenu(fileName = "new-sound-bank", menuName = "Sound Bank")]
public class SoundBank : ScriptableObject
{
    public bool isList = false;
    public string soundName = "click01";
    public string soundType = "General";
    public AudioClip[] clips;
    public SoundBank[] soundBanks;

    public AudioClip randomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }









    /*
    public void AutoAssign()
    {
        Debug.Log("Assigning sound clips to bank...");
        string path = "/Sounds/" + soundType + "/" + soundName + "/";
        string[] filePaths = Directory.GetFiles(Application.dataPath + path, "*.wav", SearchOption.TopDirectoryOnly);
        clips = new AudioClip[filePaths.Length];

        for(int i = 1; i < filePaths.Length; i++)
        {
            string newSoundPath = path + soundName + "-" + i.ToString("00") + ".wav";
            clips[i] = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Sounds/General/click-03/click03-01.wav", typeof(AudioClip));

            Debug.Log(newSoundPath);
            EditorUtility.SetDirty(this);

        }
    }*/
}
