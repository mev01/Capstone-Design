using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IronPython.Hosting;
using System.Threading;
using System.IO;
using IronPython.Runtime;
using System.Text;

public class LookItem : MonoBehaviour
{
    private float stopTime=0;
    private float currentTime = 0;
    bool isLookAt=false;
    bool check = true;

    public Text chText;
    private int pg = 1;
    private List<string> Libs; //라이브러리의 경로
    private string _code; //실행할 파이썬의 코드

    // Start is called before the first frame update
    void Start()
    {
        string url = Application.dataPath + "/hello2/hello.py"; //경로 불러오기
        _code = File.ReadAllText(url); //코드 불러오기
        Libs = new List<string> //라이브러리 경로 지정
        {
            Application.dataPath + "/hello2/venv/Lib",
        };
        //주의! 반드시 스레드로 할 것을 추천한다. 단일스레드 유니티 특성상 실행동안 굳는데, 시간이 길어지면 오류로 인식하고 종료된다.
        new Thread(new ThreadStart(Run)).Start();

        /*var engine = Python.CreateEngine();
        var scope = engine.CreateScope();

        string code = "a=1+2";

        var source = engine.CreateScriptSourceFromString(code);
        source.Execute(scope);

        Debug.Log(scope.GetVariable<int>("a"));*/
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

    private void Run()
    {
        //Python에서 sys.argv로 실행인자를 받도록 하는 것을 알것이다. 그것을 .Net에서 입력해주는 것이다.
        var argv = new List();  //리스트 생성
        argv.Add("__main__.py"); //파이썬 명. 왜냐면 sys.argv[0]은 실행명이라서
        argv.Add("test");  //문자열 입력
        var engine = UnityPython.CreateEngine(); //엔진생성. 원래는 Python.CreateEngine(); 인데, 이게 위에서 언급한 것이다.
        var scope = engine.CreateScope(); //스코프 생성
        engine.Runtime.GetSysModule().SetVariable("argv", argv); //sys.argv 입력

        var paths = engine.GetSearchPaths(); //라이브러리들의 위치를 입력해줘야한다.
        foreach (string s in Libs)
            paths.Add(s);

        engine.SetSearchPaths(paths); //라이브러리 등록

        StringBuilder code = new StringBuilder(); //문자열 처리할 것이 많으면 StringBuilder가 좋다.
        code.Append("import UnityEngine\n"); //이렇게 하면 유니티의 메소드를 Python에서 구동할 수 있다.
        code.Append(_code); //코드 추가
        code = code.Replace("print", "UnityEngine.Debug.Log"); //이렇게 하면 결과를 출력받을 수 있다. cmd에서는 print로 되니 문제도 발생안하고 좋다.

        var script = engine.CreateScriptSourceFromString(code.ToString()); // 코드 등록
        script.Execute(); //실행
    }
}
