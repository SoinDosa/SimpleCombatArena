using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "0.1";
    public GameObject connectScreen;
    public Text connectText;
    public GameObject loadingScreen;
    public Text loadingText;
    public Button okButton;
    //스크린 관련
    public static string userName;
    //유저 ID
    public Text helloID;
    public Button hostButton;
    public Button joinButton;
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        userName = GameObject.Find("/LoginCanvas/NameInputField/Text").GetComponent<Text>().text;
        PhotonNetwork.NickName = userName;
        Destroy(GameObject.Find("LoginCanvas"));
        helloID.text = $"Hello {PhotonNetwork.NickName}!";
        joinButton.interactable = false;
        hostButton.interactable = false;
    }

    // Update is called once per frame


    public override void OnConnectedToMaster(){
        connectScreen.SetActive(false);
        joinButton.interactable = true;
        hostButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectScreen.SetActive(true);
        connectText.text = $"연결 실패\n사유 : {cause.ToString()}";
        joinButton.interactable = false;
        hostButton.interactable = false;
    }

    public void CreateRoom(){
        hostButton.interactable = false;
        joinButton.interactable = false;
        loadingScreen.SetActive(true);
        if(PhotonNetwork.IsConnected){
            okButton.interactable = false;
            loadingText.text = "Create new room...";
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers=20});
        }
        else{
            loadingText.text = "Create Fail";
            okButton.interactable = true;
            hostButton.interactable = true;
            joinButton.interactable = true;
        }
    }

    public void JoinRoom(){
        hostButton.interactable = false;
        joinButton.interactable = false;
        loadingScreen.SetActive(true);
        if(PhotonNetwork.IsConnected){
            okButton.interactable = false;
            loadingText.text = "Join Random room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else{
            loadingText.text = "Join Fail";
            okButton.interactable = true;
            hostButton.interactable = true;
            joinButton.interactable = true;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        loadingText.text = "No empty room";
        okButton.interactable = true;
        hostButton.interactable = true;
        joinButton.interactable = true;
    }

    public void OKButton(){
        loadingScreen.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("InGame");
    }
}
