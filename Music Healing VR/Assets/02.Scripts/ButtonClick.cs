using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject ExitMenuPanel;

    private float stopTime = 0;
    private float currentTime = 0;
    bool isLookAt = false;
    bool check = true;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (!isLookAt)
        {
            stopTime = currentTime;
            check = true;
        }
        if (currentTime - stopTime >= 1.5f && check) //응시 1.5초 지나면 버튼 클릭
        {
            BtClick();
            check = false;
        }
    }

    public void onLookItemBox(bool look)
    {
        isLookAt = look;
    }

    public void BtClick()
    {
        if (this.gameObject.name == "Transition button")
        {
            if (SceneManager.GetActiveScene().buildIndex != 8)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
                SceneManager.LoadScene(1);
        }
        else if (this.gameObject.name == "Lobby botton")
        {
            SceneManager.LoadScene("Lobby");
        }
        else if (this.gameObject.name == "My Music botton")
        {

        }
        else if (this.gameObject.name == "Volume button")
        {

        }
        else if (this.gameObject.name == "Exit button")
        {
            ExitMenuPanel.SetActive(true); //나가기메뉴 활성화
            GameObject.Find("MenuPanel").SetActive(false); //메뉴 비활성화
        }
        else if(this.gameObject.name == "Yes button")
        {
            Application.Quit();
        }
        else if (this.gameObject.name == "No button")
        {
            ExitMenuPanel.SetActive(false); //나가기메뉴 비활성화
        }
    }
}
