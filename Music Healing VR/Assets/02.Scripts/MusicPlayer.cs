using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audio;
    int[] idxArr;
    int i;

    void Start()
    {
        i = 0;
        idxArr = new int[10];
        audio = GetComponent<AudioSource>();

        indexSuffle(idxArr);

        audio.clip = MusicDownload.ad[idxArr[i]];
        audio.Play();
    }

    void Update()
    {
        StartCoroutine(PlayOne());
    }

    IEnumerator PlayOne()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick1Button2)){ //다음 곡으로 넘어가기
            audio.clip = MusicDownload.ad[idxArr[++i]];
            audio.Play();
            if(i == 4)
            {
                i = -1;
            }
            yield return new WaitForSeconds(audio.clip.length);   
        }

        if (!audio.isPlaying)
        {
            audio.clip = MusicDownload.ad[idxArr[++i]];
            audio.Play();
            if (i == 4)
            {
                i = -1;
            }
            yield return new WaitForSeconds(audio.clip.length);
        }

    }

    //idxArr에 랜덤 인덱스값 넣기
    private void indexSuffle(int[] idxArr)
    {

        for (int i = 0; i < 10; i++)
        {
            idxArr[i] = i;
        }

        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            for(int i = 0; i < 10; i++)
            {
                idxArr[i] = 10+i;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6)
        {
            for (int i = 0; i < 10; i++)
            {
                idxArr[i] = 20 + i;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7 || SceneManager.GetActiveScene().buildIndex == 8)
        {
            for (int i = 0; i < 10; i++)
            {
                idxArr[i] = 30 + i;
            }
        }

    }

}
