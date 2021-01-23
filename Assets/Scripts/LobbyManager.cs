using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "0.1";
    public GameObject loadingScreen;
    public Text loadingText;
    public Text helloUser;

    public Button hostButton;
    public Button joinButton;

    //joinScreen 관련
    public GameObject joinScreen;
    public Text joinText;
    public Button okButton;
    public static string userName;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        userName = GameObject.Find("/StartCanvas/StartPanel/NameInput/Text").GetComponent<Text>().text;
        Destroy(GameObject.Find("StartCanvas"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        loadingScreen.SetActive(false);
        helloUser.text = $"Hello {userName}!";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        loadingScreen.SetActive(true);
        loadingText.text = $"연결 실패\n 사유 : { cause.ToString()}";
    }

    public void CreateRoom()
    {
        hostButton.interactable = false;
        joinButton.interactable = false;
        joinScreen.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            joinText.text = "Create new room...";
            okButton.interactable = false;
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
        }
        else
        {
            joinText.text = "Offline입니다";
            okButton.interactable = true;
            hostButton.interactable = true;
            joinButton.interactable = true;
        }
    }

    public void JoinRoom()
    {
        hostButton.interactable = false;
        joinButton.interactable = false;
        joinScreen.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            joinText.text = "Join Random Room...";
            okButton.interactable = false;
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            joinText.text = "Offline입니다";
            okButton.interactable = true;
            hostButton.interactable = true;
            joinButton.interactable = true;
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        joinText.text = "No empty room";
        okButton.interactable = true;
        hostButton.interactable = true;
        joinButton.interactable = true;
    }

    public void OKButton()
    {
        joinScreen.SetActive(false);
    }

    public override void OnJoinedRoom()
    {

        PhotonNetwork.LoadLevel("InGame");
    }
}
