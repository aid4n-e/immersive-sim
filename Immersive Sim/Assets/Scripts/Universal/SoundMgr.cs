using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class SoundMgr : MonoBehaviour
{
    // Start is called before the first frame update

    // Change this to start using enums instead
    public SoundBank soundBankList;

    public SoundBank selectedBank;




    //[SerializeField, ButtonInvoke(nameof(BuildSoundBank), ButtonInvoke.DisplayIn.PlayAndEditModes)] private bool buildSoundBank;  // Button for running BuildSoundBank function

    // Play a specific audio clip from a bank at a source
    public void PlaySound(AudioSource src, int bank, int clip)
    {
        src.PlayOneShot(soundBankList.soundBanks[bank].clips[clip]);
    }

    // Play a random audio clip from a bank at a source
    public void PlayRandomSound(AudioSource src, int bank)
    {
        int clip = Random.Range(0, soundBankList.soundBanks[bank].clips.Length);
        src.PlayOneShot(soundBankList.soundBanks[bank].clips[clip]);
    }















    /*// Allows the AutoAssign function to be called on the current selectedBank (NOT WORKING) 
    public void BuildSoundBank()
    {
        selectedBank.AutoAssign();
    }*/












    /*
        public string[][][] banks;
        public string[] topDir;



        void Start()
        {
            topDir = System.IO.Directory.GetDirectories(Application.dataPath + "/Sounds", "*", System.IO.SearchOption.TopDirectoryOnly);

            for (int n = 0; n < topDir.Length; n++)
            {
                string[] midDir = System.IO.Directory.GetDirectories(topDir[n], "*", System.IO.SearchOption.TopDirectoryOnly);


                for (int i = 0; i < midDir.Length; i++)
                {
                    banks[n][i] = 
                }

                banks[n][] = System.IO.Directory.GetDirectories(topDir[n], "*", System.IO.SearchOption.TopDirectoryOnly);

                for (int i = 0; i < fileEntries.Length; i++)
                {
                    banks[n][i][] fileEntries = Directory.GetFiles(topDir[n]);


                }

            }

            GenerateBank();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GenerateBank()
        {
            for(int i = 0; i < topDir.Length; i++)
                Debug.Log(topDir[i]);
        }


*/
}
