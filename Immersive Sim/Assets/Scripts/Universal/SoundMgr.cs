using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SoundMgr : MonoBehaviour
{
    // Start is called before the first frame update

    public string[][] banks;


    void Start()
    {
        GenerateBank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBank()
    {

    }


    void RandomSound(string folderPath)
    {


    }

    void PlaySound(AudioSource src, AudioClip clip)
    {
        
    }

    private static long GetDirectorySize(string folderPath)
    {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        return di.EnumerateFiles("*").Sum(fi => fi.Length);
    }
}
