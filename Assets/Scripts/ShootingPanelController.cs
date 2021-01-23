using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEditor;

public class ShootingPanelController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    GameObject playerObject;
    Animator playerAnimator;

    //moving joystick
    public RectTransform s_rectBack;
    public RectTransform s_rectJoy;
    float s_radius;
    bool s_isTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        s_radius = s_rectBack.rect.width * 0.5f;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = playerObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 joyVec = new Vector2(vecTouch.x - s_rectBack.position.x, vecTouch.y - s_rectBack.position.y);
        joyVec = Vector2.ClampMagnitude(joyVec, s_radius - 100f);
        s_rectJoy.localPosition = joyVec;

        float fSqr = (s_rectBack.position - s_rectJoy.position).sqrMagnitude / (s_radius * s_radius);
        Vector2 vecNormal = joyVec.normalized;

        playerObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg * -1);
        playerAnimator.SetBool("isAttack", true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        s_isTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        s_isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        s_rectJoy.localPosition = Vector2.zero;
        playerAnimator.SetBool("isAttack", false);
        s_isTouch = false;
    }
}
