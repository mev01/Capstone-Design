using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] ad = new AudioClip[5];

    AudioSource audio;
    byte[] arrByte;
    int i;

    void Start()
    {
        i = 0;
        arrByte = new byte[5];
        audio = GetComponent<AudioSource>();

        indexSuffle(arrByte);
        audio.clip = ad[arrByte[i++]];
        audio.Play();
        StartCoroutine(PlayOne());
    }

    IEnumerator PlayOne()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button2)){ //다음 곡으로 넘어가기
            audio.clip = ad[arrByte[i++]];
            audio.Play();
            if(i == 4)
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
                i = 4;
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

}
