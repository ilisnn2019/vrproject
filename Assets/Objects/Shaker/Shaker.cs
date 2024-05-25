using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public struct HandController
{
    public XRHandController hand;
    public float value;    
}

public class Shaker : MonoBehaviour, IListener
{
    private const float MINSPEED = 3.5f;

    private bool eventManagerInitialized = false;

    private BoxCollider shaker_Collider;

    public HandController[] handsAttached2Shaker = new HandController[2]; // 0 : Head, 1 : Body

    private float speed;

    private Vector3 direction;

    private int shake_cnt = 0;

    public Transform shakerHead;

    public GameObject shakerCapPourable;

    public GameObject shakerCapObject;

    public GameObject ice;

    void Awake()
    {
        StartCoroutine(WaitForEventManagerInitialization());
    }

    IEnumerator WaitForEventManagerInitialization()
    {
        // EventManager가 초기화될 때까지 대기합니다.
        while (!eventManagerInitialized)
        {
            EventManager eventManager = EventManager.Instance;
            if (eventManager != null)
            {
                eventManagerInitialized = true;
            }
            yield return null;
        }

        // 이제 EventManager가 초기화되었으므로 초기화 작업을 수행합니다.
        Initialize();
    }

    private void Initialize()
    {
        // EventManager에 대한 초기화 코드를 이곳에 추가합니다.
        EventManager.Instance.AddListener(EVENT_TYPE.SHAKER_HEADON, this);

        shaker_Collider = GetComponent<BoxCollider>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (handsAttached2Shaker[0].hand != null) //두손이 모두 쉐이커를 잡고 있으면,
        {
            GenHaptic(); //진동 발생
        }
        DebugText.debugText.text = shake_cnt.ToString();

        if(shake_cnt >= 200 && shakerCapObject != null)
        {
            shakerCapObject.GetComponent<XRGrabInteractable>().enabled = true;
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SHAKER_HEADON:
                Destroy(Sender.gameObject);
                ShakeHeadOn();
                break;
        }
    }

    private void ShakeHeadOn()
    {
        Transform shaker_head = shakerHead.transform;

        shaker_Collider.center = new Vector3(-5.99164778e-05f, -4.15828836e-05f, 0.000879664498f);
        shaker_Collider.size = new Vector3(0.00155161147f, 0.00156158721f, 0.00438096002f);

        shaker_head.gameObject.SetActive(true);
    }

    public void SelectShaker(XRHandController hand)
    {
        if(hand == null)
        {
            return;
        }

        Vector3 pivot = transform.GetChild(0).transform.up;

        Vector3 handVector = hand.transform.position - pivot;

        float dot = Vector3.Dot(pivot, handVector);

        if (handsAttached2Shaker[0].hand == null)
        {
            handsAttached2Shaker[0].hand = hand;
            handsAttached2Shaker[0].value = dot;
        }
        else { 
            if(handsAttached2Shaker[0].value < dot)
            {
                handsAttached2Shaker[1].hand = handsAttached2Shaker[0].hand;
                handsAttached2Shaker[1].value = handsAttached2Shaker[0].value;
                handsAttached2Shaker[0].hand = hand;
                handsAttached2Shaker[0].value = dot;
            }
            else
            {
                handsAttached2Shaker[1].hand = hand;
                handsAttached2Shaker[1].value = dot;
            }
        }
    }


    public void UnSelectShaker(XRHandController hand)
    {
        if (hand == null)
        {
            return;
        }

        if (handsAttached2Shaker[0].hand == hand)
        {
            handsAttached2Shaker[0].hand = handsAttached2Shaker[1].hand;
            handsAttached2Shaker[0].value = handsAttached2Shaker[1].value;
            handsAttached2Shaker[1].hand = null;
            handsAttached2Shaker[1].value = 0;
        }
        else
        {
            handsAttached2Shaker[1].hand = null;
            handsAttached2Shaker[1].value = 0;
        }
    }
    

    IEnumerator CalculateSpeed()
    {
        Vector3 lastPos = transform.position;
        yield return new WaitForFixedUpdate();
        direction = (lastPos - transform.position);  
        speed = direction.magnitude / Time.deltaTime;
    }

    private void GenHaptic()
    {
        StartCoroutine(CalculateSpeed());
        //Haptic 발생
        Vector3 axisShaker = handsAttached2Shaker[0].hand.transform.position - handsAttached2Shaker[1].hand.transform.position;
        axisShaker = axisShaker.normalized;

        float axis = Vector3.Dot(axisShaker, direction);
        DebugText.debugText.text = speed.ToString();
        if (speed > MINSPEED)
        {
            if (axis > 0)
            {
                handsAttached2Shaker[0].hand.E_haptic.SendHapticImpulse(0.5f, 0.5f);

            }
            else 
            {
                handsAttached2Shaker[1].hand.E_haptic.SendHapticImpulse(0.5f, 0.5f);
            }

            shake_cnt++;
        }
    }
    public void Cap_Off(Liquid liquid)
    {
        Destroy(shakerCapObject);
        //sourable active
        shakerCapPourable.SetActive(true);
        shakerCapPourable.GetComponent<Pourable>().Set_Color(liquid.l_color);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ice"))
        {
            if (ice.activeSelf == false)
                ice.SetActive(true);
        }
    }
}
