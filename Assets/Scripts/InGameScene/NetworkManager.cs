using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject RespawnPanel;

    public void Awake()
    {
        Spawn();
        Debug.Log("Welcome " + PhotonNetwork.NickName);
    }
    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0), Quaternion.identity);
        RespawnPanel.SetActive(false);
    }
}
