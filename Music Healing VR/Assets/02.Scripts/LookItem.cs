using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookItem : MonoBehaviour
{
    private float stopTime=0;
    private float currentTime = 0;
    bool isLookAt=false;
    bool check = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(!isLookAt)
        {
            stopTime = currentTime;
        }
        if (currentTime - stopTime >= 2&&check)
        {
            Debug.Log("ch");
            check = false;
        }
        //Debug.Log(currentTime);
        //Debug.Log(stopTime);
    }

    public void onLookItemBox(bool look)
    {
        isLookAt = look;
        Debug.Log(isLookAt);
    }
}
