using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartLogin : MonoBehaviour
{
    public InputField id;
    public GameObject canvas;
    public void Start()
    {
        DontDestroyOnLoad(canvas);
    }

    public void SignIn()
    {
        if(id.text == "")
        {
            Debug.Log("아이디를 입력해주세요");
        }
        else
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
