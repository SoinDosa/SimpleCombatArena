using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    public Text playerName;
    // Start is called before the first frame update
    void Start()
    {
        if (LobbyManager.userName != null)
        {
            playerName.GetComponent<Text>().text = LobbyManager.userName;
        }
        else
        {
            playerName.GetComponent<Text>().text = "Guest";
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
