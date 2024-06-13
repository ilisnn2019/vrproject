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

    private HandController[] handsAttached2Shaker = new HandController[2]; // 0 : Head, 1 : Body

    private float speed;

    private Vector3 direction;

    private int shake_cnt = 0;

    public Transform shakerHead;

    public Pourable shakerCapPourable;

    public GameObject shakerCapObject;

    public List<AudioClip> clips = new();

    public XRHandController[] hands = new XRHandController[2];//0 -> left;

    SoundFunction soundfunction;

    int state = -1;

    public Transform BodyAttach;
    private Vector3 initialPos;
    private Quaternion initialRot;

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
        hands[0] = GameObject.FindWithTag("LeftHand").GetComponent<XRHandController>();
        hands[1] = GameObject.FindWithTag("RightHand").GetComponent<XRHandController>();
        soundfunction = GetComponent<SoundFunction>();
    }

    void Update()
    {
        if (handsAttached2Shaker[0].hand != null && handsAttached2Shaker[1].hand != null) //두손이 모두 쉐이커를 잡고 있으면,
        {
            GenHaptic(); //진동 발생
        }
        //DebugText.debugText.text = shake_cnt.ToString();

        if(shake_cnt >= 100 && shakerCapObject != null)
        {
            shakerCapObject.GetComponent<XRGrabInteractable>().enabled = true;
        }

        if (shakerHead.gameObject.activeSelf)
        {
            state = 1;
        }
        else
        {
            state = 1;
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

    private XRHandController GetHand(string name)
    {
        if (name == "Left Controller")
        {
            return hands[0];
        }
        else if (name == "Right Controller")
        {
            return hands[1];
        }
        else
        {
            return null;
        }
    }

    private void ShakeHeadOn()
    {
        shaker_Collider.center = new Vector3(-5.99164778e-05f, -4.15828836e-05f, 0.000879664498f);
        shaker_Collider.size = new Vector3(0.00155161147f, 0.00156158721f, 0.00438096002f);

        shakerHead.gameObject.SetActive(true);

        XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
        interactable.selectMode = InteractableSelectMode.Multiple;
        interactable.trackPosition = interactable.trackRotation = true;
    }

    public void SelectShaker(SelectEnterEventArgs args)
    {
        Transform controller = args.interactorObject.transform.parent;

        XRHandController hand = GetHand(controller.name);

        if (hand == null || state == 0)
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


    public void UnSelectShaker(SelectExitEventArgs args)
    {
        Transform controller = args.interactorObject.transform.parent;

        XRHandController hand = GetHand(controller.name);

        if (hand == null || state == 0)
        {
            return;
        }

        if (handsAttached2Shaker[0].hand == hand)
        {
            handsAttached2Shaker[0].hand = handsAttached2Shaker[1].hand;
            handsAttached2Shaker[0].value = handsAttached2Shaker[1].value;
            handsAttached2Shaker[1].hand = null;
            handsAttached2Shaker[1].value = -100;
        }
        else if(handsAttached2Shaker[1].hand == hand)
        {
            handsAttached2Shaker[1].hand = null;
            handsAttached2Shaker[1].value = -100;
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
        //DebugText.debugText.text = speed.ToString();
        if (speed > MINSPEED)
        {

            int rand = UnityEngine.Random.Range(1, 5);
            soundfunction.ClipPlay(clips[rand]);

            if (axis > 0)
            {
                handsAttached2Shaker[0].hand.E_haptic.SendHapticImpulse(1f, 0.3f);



            }
            else 
            {
                handsAttached2Shaker[1].hand.E_haptic.SendHapticImpulse(1f, 0.3f);
            }

            shake_cnt++;
        }
    }
    public void Cap_Off()
    {
        Destroy(shakerCapObject);
        //sourable active
        Liquids liquids = GetComponent<Storable>().Get_Liquids();

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None; //un freeze rotation;

        shakerCapPourable.Capoff();
        shakerCapPourable.Set_Liquids(liquids);
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        Transform controller = args.interactorObject.transform.parent;

        if (controller == null || state == 1)
        {
            return;
        }

        XRHandController hand = GetHand(controller.name);

        if (hand!=null)
        {
            hand.OnGrabbing(hand.transform.position, hand.transform.rotation);
        }
        else
        {
            return;
        }

    }

    public void OnRelease(SelectExitEventArgs args)
    {
        Transform controller = args.interactorObject.transform.parent;

        if (controller == null || state == 1)
        {
            return;
        }

        XRHandController hand = GetHand(controller.name);

        if (hand != null)
        {
            hand.OnRelease();
        }
        else
        {
            return;
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("sink"))
        {
            transform.SetPositionAndRotation(initialPos, initialRot);
        }
    }

}
