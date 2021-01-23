using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BulletScript : MonoBehaviourPunCallbacks, IPunObservable
{
    private float bulletSpeed = 1.0f;

    public PhotonView pV;
    private Vector2 initPos;

    private float x;
    private float y;
    void Start()
    {
        initPos = new Vector2(transform.position.x, transform.position.y);
    }

    void FixedUpdate()
    {
        transform.Translate(0,bulletSpeed,0);
        x = initPos.x - transform.position.x;
        y = initPos.y - transform.position.y;

        if(Mathf.Sqrt(x*x + y * y) > 10.0f)
        {
            pV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Wall")
        {
            pV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        if(!pV.IsMine && col.tag == "Player" && col.GetComponent<PhotonView>().IsMine)
        {
            col.GetComponent<PlayerScript>().Hit();
            pV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
