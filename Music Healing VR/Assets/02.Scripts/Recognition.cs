using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition;
using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recognition : MonoBehaviour
{
    public GCSpeechRecognition gc;
    public FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples.GCSR_Example ex;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("record");
            StartRecord();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            StopRecord();
            gc.RecognizeSuccessEvent += Gc_RecognizeSuccessEvent;
        }
    }

    private void Gc_RecognizeSuccessEvent(RecognitionResponse obj)
    {
        throw new NotImplementedException();
    }

    void StartRecord()
    {
        gc.StartRecord(false);
    }

    void StopRecord()
    {
        gc.StopRecord();
    }
}
