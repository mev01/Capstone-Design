using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class MusicDownload : MonoBehaviour
{
    public String[] urls = new string[40];
    public static AudioClip[] ad = new AudioClip[40];

    UnityWebRequest www ;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(StartAudio());
        }

    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator StartAudio()
    {
        for(int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return StartCoroutine(GetAudioClip(10*i+j));
            }
            Debug.Log("asdf");
        }

    }

    IEnumerator GetAudioClip(int i) //url로 음악파일 다운 받기
    {
        using (www = UnityWebRequestMultimedia.GetAudioClip(urls[i], AudioType.MPEG))
        {           
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }

            else
            {
                ad[i] = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log(i);
            }
        }

    }
}
