using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IronPython.Hosting;

public class LookItem : MonoBehaviour
{
    private float stopTime=0;
    private float currentTime = 0;
    bool isLookAt=false;
    bool check = true;

    public Text chText;
    private int pg = 1;
    // Start is called before the first frame update
    void Start()
    {
        var engine = Python.CreateEngine();
        var scope = engine.CreateScope();

        string code = "a=1+2";

        var source = engine.CreateScriptSourceFromString(code);
        source.Execute(scope);

        Debug.Log(scope.GetVariable<int>("a"));
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (!isLookAt)
        {
            stopTime = currentTime;
            check = true;
        }
        if (currentTime - stopTime >= 1 && check)
        {
            if (pg++ == 1)
                Text1();
            else 
                Text2();
            check = false;
        }
    }

    public void onLookItemBox(bool look)
    {
        isLookAt = look;
        Debug.Log(isLookAt);
    }

    public void Text1()
    {
        chText.GetComponent<Text>().text = "이제부터 제가 하는 질문에 답변해주세요. 준비가 되면 next버튼을 봐주세요";
    }

    public void Text2()
    {
        chText.GetComponent<Text>().text = "오늘 기분이 어떠신가요??";
    }
}
