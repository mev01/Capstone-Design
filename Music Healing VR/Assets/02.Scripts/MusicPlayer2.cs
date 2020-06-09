using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicPlayer2 : MonoBehaviour
{
    AudioSource audio;
    String url = "";
    UnityWebRequest www;

    public String[] urls = new string[2];
    AudioClip[] ad = new AudioClip[10];

    byte[] arrByte;
    int i;

    void Start()
    {
        i = 0;
        arrByte = new byte[5];
        audio = GetComponent<AudioSource>();

        indexSuffle(arrByte);

        StartCoroutine(StartAudio());

    }

    void Update()
    {
        StartCoroutine(PlayOne());
    }
    IEnumerator StartAudio()
    {
        yield return StartCoroutine("GetAudioClip");

        for (int j = 1; j < 2; j++)
        {
            StartCoroutine(GetAudioClip2(j));
        }
    }

    IEnumerator GetAudioClip() //가져온 url로 음악파일 다운 받아서 틀기
    {
        Debug.Log("1-1");
        //yield return new WaitForSeconds(10f);
        using (www = UnityWebRequestMultimedia.GetAudioClip("https://firebasestorage.googleapis.com/v0/b/music-healing-vr.appspot.com/o/1.Cheerful%2C%20Happy%2Ftest.wav?alt=media&token=49562536-ba4e-4600-b381-5b4c50ef739d", AudioType.WAV))
        {
            Debug.Log("1-2");
            ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                ad[0] = DownloadHandlerAudioClip.GetContent(www);
                audio.clip = ad[0];
                //audio.clip = www.downloadHandler.data;
                audio.Play();
            }
        }

    }

    IEnumerator GetAudioClip2(int j)
    {
       
        Debug.Log("2-1");
        using (www = UnityWebRequestMultimedia.GetAudioClip(urls[j], AudioType.WAV))
        {
            Debug.Log("2-2");
            yield return www.SendWebRequest();
            ad[j] = DownloadHandlerAudioClip.GetContent(www);
            audio.clip = ad[j];
            audio.Play();
        }
    }

    IEnumerator PlayOne()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        { //다음 곡으로 넘어가기
            audio.clip = ad[arrByte[i++]];
            audio.Play();
            if (i == 1)
            {
                indexSuffle(arrByte);
                i = 0;
            }
            yield return new WaitForSeconds(audio.clip.length);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button1)) //이전 곡으로 가기
        {
            audio.clip = ad[arrByte[--i]];
            if (i == -1)
            {
                i = 1;
            }
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }
    }

    //arrByte에 랜덤 인덱스값 넣기
    private void indexSuffle(byte[] arrByte)
    {
        System.Random rand = new System.Random();
        rand.NextBytes(arrByte);
    }

    /*  IEnumerator GetAudioClip2()
      {
          string filename = Application.dataPath + "test.mp4";
          using (WWW www = new WWW("https://sample-videos.com/video123/mp4/720/big_buck_bunny_720p_30mb.mp4"))
          {
              while (!www.isDone)
              {
                  await Task.Delay(250);
                  Debug.Log("Downloading");
              }
              using (FileStream sourceStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
              {
                  await sourceStream.WriteAsync(www.bytes, 0, www.bytes.Length);
                  Debug.Log("YAY");
                  sourceStream.Close();
              }
          }
      } */

    IEnumerator GetUrl() //url 가져오기
    {
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://music-healing-vr.appspot.com/1.Cheerful, Happy");
        Firebase.Storage.StorageReference island_ref = storage.GetReferenceFromUrl("test.wav");

        // Fetch the download URL
        island_ref.GetDownloadUrlAsync().ContinueWith(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                url = task.Result.ToString();
                // ... now download the file via WWW or UnityWebRequest.

            }
        });
        yield return null;
    }


}
