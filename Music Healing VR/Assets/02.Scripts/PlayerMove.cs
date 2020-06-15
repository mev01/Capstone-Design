using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3.0f; //이동 속도

    private Transform camTr;
    private CharacterController cc;

    private int key = 0;
    /*카메라 처음 시작 위치 세팅
    public float xpos = 0.0f;
    public float ypos = 1.0f;
    public float zpos = 0.0f;*/

    void Start()
    {
        camTr = Camera.main.GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
        /* 카메라 처음 시작 위치 세팅
        Vector3 vector; //새 변수 만듬
        vector = transform.position; //새 변수에 현재 위치 넣어줌
        vector.x = xpos; //변수.x 이런식으로 원하는 값 넣음
        vector.y = ypos;
        vector.z = zpos;
        camTr.position = vector; //현재 위치에 변수를 넣어줌 */
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //메인카메라가 바라보는 방향
            Vector3 dir = camTr.TransformDirection(Vector3.forward);
            cc.Move(dir * Time.deltaTime * speed);
        }
    }

    void JoistickMove()
    {
        if (key == 0)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.W))
            {
                Debug.Log("move forward");
                key = 1;
                //메인카메라가 바라보는 방향
                Vector3 dir = camTr.TransformDirection(Vector3.forward);
                cc.Move(dir * Time.deltaTime * speed);
            }
            /* else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.A))
             {
                 key = 2;
                 Vector3 dir = camTr.TransformDirection(Vector3.left);
                 cc.Move(dir * Time.deltaTime * speed);
             }
             else if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.D))
             {
                 key = 3;
                 Vector3 dir = camTr.TransformDirection(Vector3.right);
                 cc.Move(dir * Time.deltaTime * speed);
             }
             else if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.S))
             {
                 key = 4;
                 Vector3 dir = camTr.TransformDirection(Vector3.back);
                 cc.Move(dir * Time.deltaTime * speed);
             } */
        }

        else if (key != 0 && Input.anyKeyDown)
        {
            key = 0;
        }
        else if (key == 1)
        {
            Vector3 dir = camTr.TransformDirection(Vector3.forward);
            cc.Move(dir * Time.deltaTime * speed);
        }
        /* else if (key == 2)
         {
             Vector3 dir = camTr.TransformDirection(Vector3.left);
             cc.Move(dir * Time.deltaTime * speed);
         }
         else if (key == 3)
         {
             Vector3 dir = camTr.TransformDirection(Vector3.right);
             cc.Move(dir * Time.deltaTime * speed);
         }
         else if (key == 4)
         {
             Vector3 dir = camTr.TransformDirection(Vector3.back);
             cc.Move(dir * Time.deltaTime * speed);
         } */
    }

}
