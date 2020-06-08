using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicPlayer2 : MonoBehaviour
{
    AudioSource audio;
    String url;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCoroutine(GetAudioClip());
    }

    IEnumerator GetAudioClip() //가져온 url로 음악파일 다운 받아서 틀기
    {
        //yield return new WaitForSeconds(10f);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://firebasestorage.googleapis.com/v0/b/music-healing-vr.appspot.com/o/1.Cheerful%2C%20Happy%2F(%EA%B2%BD%EC%BE%8C%ED%95%9C)All%20For%20You_converted.mp3?alt=media&token=017da223-fe36-4146-82be-f6fc4aa596ea", AudioType.MPEG))
        {
            yield return www.Send();
            Debug.Log("4");
            if (www.isError)
            {
                Debug.Log("err");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www);
                audio.clip = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("6");
                //audio.clip = www.downloadHandler.data;
                audio.Play();
            }
        }

    }

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
