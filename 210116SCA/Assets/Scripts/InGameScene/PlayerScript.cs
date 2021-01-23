using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D rB;
    public Animator aN;
    public PhotonView pV;

    public SpriteRenderer sR;
    public Image sRCircle;
    public Text playerText;
    public Image hpBar;
    public AudioSource aS;

    public RectTransform uIRT;

    [SerializeField]
    private static float speed = 2.0f;
    Vector2 mousePos;
    float angle;

    private Weapon weaponValue = Weapon.Pistol;
    enum Weapon
    {
        Hand, // 0
        Pistol // 1
    }
    public Transform attackPos;

    public bool check = true;
    Vector3 curPos;
    Vector3 vecMove;

    public void Awake()
    {
        playerText.text = pV.IsMine ? PhotonNetwork.NickName : pV.Owner.NickName;
        playerText.color = pV.IsMine ? Color.green : Color.red;
        sRCircle.color = pV.IsMine ? Color.green : Color.red;

        if (pV.IsMine)
        {
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }
    }

    public void FixedUpdate()
    {
        uIRT.transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, -transform.rotation.z);
        aN.SetBool("walk", false);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        if (pV.IsMine)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            if(hor != 0 || ver != 0){
                aN.SetBool("walk", true);
            }
            rB.velocity = new Vector2(speed * hor, speed * ver);
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            if (Input.GetMouseButton(0) && check)
            {
                Attack();
                Debug.Log("End Attack");
            }
        }
    }


    public void Hit()
    {
        hpBar.fillAmount -= 0.1f;
        if (hpBar.fillAmount <= 0.0f)
        {
            GameObject.Find("Canvas").transform.Find("RespawnPanel").gameObject.SetActive(true);
            pV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }

    public void Attack()
    {
        if (weaponValue == Weapon.Pistol)
        {
            check = false;
            aS.Play();
            PhotonNetwork.Instantiate("Bullet", attackPos.position, transform.rotation);
            StartCoroutine(WaitForIt());


        }
    }
    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    IEnumerator WaitForIt()
    {
        aN.ResetTrigger("idle");
        aN.SetTrigger("attack");
        Debug.Log("WaitFotIt");
        yield return new WaitForSeconds(0.5f);

        aN.SetTrigger("idle");
        aN.ResetTrigger("attack");
        check = true;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpBar.fillAmount);
        }
        else
        {
            hpBar.fillAmount = (float)stream.ReceiveNext();
        }
    }
}