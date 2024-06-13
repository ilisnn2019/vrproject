using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public enum HandType
{
    Left,
    Right
}

public class XRHandController : MonoBehaviour
{
    public HandType handType;

    private Animator animator;
    private InputDevice inputDevice;


    public float thumbMoveSpeed;

    private float indexValue;

    public float IndexValue
    {
        get => indexValue;
    }

    private float thumbValue;
    private float threeFingersValue;

    private bool primary;
    private bool secondary;

    private bool isgrabbing = false;
    public Vector3 grabbedPos;
    public Quaternion grabbedRot;

    private bool isforce = false;

    public Vector3 initPos;
    public Quaternion initRot;

    [SerializeField]
    private XRBaseControllerInteractor e_haptic;

    public XRBaseControllerInteractor E_haptic
    {
        get { return e_haptic; }
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        initPos = transform.localPosition;
        initRot = transform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        if(!isforce)
            AnimationHand();

        if (isgrabbing)
        {
            transform.SetPositionAndRotation(grabbedPos, grabbedRot);
        }
        else
        {
            transform.SetLocalPositionAndRotation(initPos, initRot);
        }
    }

    public void OnGrabbing(Vector3 position, Quaternion rotation) //hand grab fixed positioned object.
    {
        grabbedPos = position;
        grabbedRot = rotation;
        isgrabbing = true;
    }

    public void OnRelease()
    {
        isgrabbing = false;
    }

    public void ReleaseAndLock()
    {
        isforce = true;
        animator.SetFloat("Index", 0);
        animator.SetFloat("3Fingers", 0);
        animator.SetFloat("Thumb", 0);
    }

    public void UnLock()
    {
        isforce = false;
    }




    private InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;
        if(handType == HandType.Left)
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        return inputDevices[0];
    }


    void AnimationHand()
    {
        inputDevice = GetInputDevice();

        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out indexValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out threeFingersValue);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out primary);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondary);

        if (primary || secondary)
        {
            thumbValue += thumbMoveSpeed;

        }
        else
        {
            thumbValue -= thumbMoveSpeed;
        }

        thumbValue = Mathf.Clamp(thumbValue, 0, 1);

        animator.SetFloat("Index", indexValue);
        animator.SetFloat("3Fingers", threeFingersValue);
        animator.SetFloat("Thumb", thumbValue);

    }

}
