using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject MenuPanel;
    public void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            MenuPop();
        }

    }
    public void MenuPop()
    {
        if (MenuPanel.activeSelf == false) // 메뉴 popup
        {
            //Time.timeScale = 0f; // 먼저 시간을 정지 시킨다.

            GameObject.Find("Canvas").transform.Find("ExitMenuPanel").gameObject.SetActive(false); //나가기메뉴 비활성화

            Camera.main.transform.position += new Vector3(0f, 1f, 0f); //땅 안보이게 카메라 위로 올리기
            this.transform.position = //카메라 앞에 panel 위치시키기
                Camera.main.transform.position 
                + Camera.main.transform.forward * 5.0f
                + new Vector3(0, 0, 2); 
            this.transform.rotation = new Quaternion(
                Camera.main.transform.rotation.x, 
                Camera.main.transform.rotation.y, 
                Camera.main.transform.rotation.z, 
                Camera.main.transform.rotation.w);

            MenuPanel.SetActive(true); //메뉴 활성화

            InvokeRepeating("CameraTracing", 0.0f, 0.1f); //메뉴가 계속 카메라 쪽을 보도록
            Invoke("MenuPopdown", 20f); //20초 후에 메뉴 자동으로 꺼짐
        }

        else //메뉴 popdown
        {
            CancelInvoke();
            MenuPopdown();
        }
    }

    public void MenuPopdown()
    {
        //Time.timeScale = 1f; // 시간을 다시 흘러가게 한다. 

        Camera.main.transform.position -= new Vector3(0f, 1f, 0f); //카메라 내리기
        MenuPanel.SetActive(false);
        GameObject.Find("Canvas").transform.Find("ExitMenuPanel").gameObject.SetActive(false);
    }

    public void CameraTracing()
    {
        this.transform.rotation = new Quaternion(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z, Camera.main.transform.rotation.w);
    }

}
