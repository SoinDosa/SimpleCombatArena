using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField nameInputField;
    public GameObject blankError;
    public GameObject canvas;
    public void Start()
    {
        DontDestroyOnLoad(canvas);
    }

    public void SignIn()
    {
        if(nameInputField.text == "")
        {
            Debug.Log("아이디를 입력해주세요");
            blankError.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
