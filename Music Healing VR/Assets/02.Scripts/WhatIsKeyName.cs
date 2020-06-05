using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatIsKeyName : MonoBehaviour
{
    KeyCode[] keyCodes = null;
    List<KeyCode> keys = new List<KeyCode>();

    void Start()
    {
        // 키코드를 한번 받아옵니다. 이렇게 전체를 받아도되지만 원하는것만 추가해도 되겠지요.
        if (keyCodes == null)
            keyCodes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];
    }

    void Update()
    {
        // 매 프레임마다 눌렸는지 검사해서 리스트에 누른 키를 추가합니다.
        foreach (KeyCode keyCode in keyCodes)
            if (Input.GetKeyDown(keyCode))
                keys.Add(keyCode);

        // 출력을 위해 담아버립니다.
        string text = string.Empty;
        foreach (KeyCode keyCode in keys)
            text += keyCode.ToString() + ", ";

        // 출력해봅니다.
        // A,
        // A,S
        // A,S,D 순으로 눌렀을때 그대로 잘 들어옵니다.
        Debug.Log(text);
    }
}
