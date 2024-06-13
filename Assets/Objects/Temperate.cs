using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Temperate : MonoBehaviour
{
    [SerializeField]
    private XRHandController[] hands = new XRHandController[2];//0 -> left;

    private XRHandController grap_hand;

    private bool isgrab = false;

    public GameObject ice;

    public GameObject ices;

    private Vector3 originPos;
    private Quaternion originRot;

    float grap_time = 0;


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        originRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isgrab)
        {
            if (grap_time <= 0)
            {
                grap_hand.E_haptic.SendHapticImpulse(1f, 4f);
                grap_hand.ReleaseAndLock();
                ices.SetActive(false);
                DropIce();
                isgrab = false;
            }
            else
            {
                grap_time -= Time.deltaTime;
            }
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

    public void Grab_Ice(SelectEnterEventArgs args)
    {
        Transform controller = args.interactorObject.transform.parent;

        grap_hand = GetHand(controller.name);

        isgrab = true;

        grap_time = 5;
    }

    public void UnGrab_Ice(SelectExitEventArgs args)
    {
        isgrab = false;
        grap_time = -1;
        ices.SetActive(true);
        grap_hand.UnLock();
        grap_hand = null;
        ResetIce();
    }

    private void ResetIce()
    {
        transform.SetPositionAndRotation(originPos, originRot);
    }
    private void DropIce()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(ice);
        }
    }
}
