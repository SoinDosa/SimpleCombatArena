using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;

public class MovingController : MonoBehaviourPun, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform m_rectBack;
    public RectTransform m_rectJoy;

    float m_radius;
    float m_Speed = 10.0f;
    bool m_isTouch = false;
    Vector3 m_vecMove;

    private GameObject playerObject;
    private Animator playerAnimator;
    private PhotonView playerPV;
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        m_radius = m_rectBack.rect.width * 0.5f;
        playerPV = playerObject.GetPhotonView();
        playerAnimator = playerObject.GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(playerPV.IsMine){
            playerObject.transform.position += m_vecMove;
        }
    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 joyVec = new Vector2(vecTouch.x - m_rectBack.position.x, vecTouch.y - m_rectBack.position.y);
        joyVec = Vector2.ClampMagnitude(joyVec, m_radius - 100f);
        m_rectJoy.localPosition = joyVec;

        float fSqr = (m_rectBack.position - m_rectJoy.position).sqrMagnitude / (m_radius * m_radius);
        Vector2 vecNormal = joyVec.normalized;
        m_vecMove = new Vector3(vecNormal.x * m_Speed * Time.deltaTime * fSqr, vecNormal.y * m_Speed * Time.deltaTime * fSqr, 0f);
        playerAnimator.SetBool("walk", true);
        if (playerPV.IsMine)
        {
            playerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg * -1);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_isTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_rectJoy.localPosition = Vector2.zero;
        playerAnimator.SetBool("walk", false);
        m_vecMove = Vector3.zero;
        m_isTouch = false;
    }
}
