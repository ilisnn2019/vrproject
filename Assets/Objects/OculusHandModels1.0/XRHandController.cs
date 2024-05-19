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

    [SerializeField]
    private XRBaseControllerInteractor e_haptic;

    public XRBaseControllerInteractor E_haptic
    {
        get { return e_haptic; }
    }

    // Define the positions of the finger joints
    public Transform thumbJoint;
    public Transform indexFingerJoint;
    public Transform middleFingerJoint;
    public Transform ringFingerJoint;
    public Transform pinkyFingerJoint;

    // Adjust these values based on your model and preferences
    public float fingerIKWeight = 1.0f;
    public float fingerIKPositionWeight = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        AnimationHand();
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

        //ApplyFingerIK();
    }

    //void ApplyFingerIK()
    //{
    //    // Calculate target positions for the finger joints
    //    Vector3 thumbTargetPosition = CalculateFingerTargetPosition(thumbJoint);
    //    Vector3 indexTargetPosition = CalculateFingerTargetPosition(indexFingerJoint);
    //    Vector3 middleTargetPosition = CalculateFingerTargetPosition(middleFingerJoint);
    //    Vector3 ringTargetPosition = CalculateFingerTargetPosition(ringFingerJoint);
    //    Vector3 pinkyTargetPosition = CalculateFingerTargetPosition(pinkyFingerJoint);

    //    // Apply IK positions
    //    ApplyFingerIKPosition(thumbJoint, thumbTargetPosition);
    //    ApplyFingerIKPosition(indexFingerJoint, indexTargetPosition);
    //    ApplyFingerIKPosition(middleFingerJoint, middleTargetPosition);
    //    ApplyFingerIKPosition(ringFingerJoint, ringTargetPosition);
    //    ApplyFingerIKPosition(pinkyFingerJoint, pinkyTargetPosition);
    //}

    //Vector3 CalculateFingerTargetPosition(Transform joint)
    //{
    //    // Get the current rotation of the joint
    //    Quaternion jointRotation = joint.localRotation;

    //    // Calculate the bend direction based on the joint's z-local axis
    //    Vector3 bendDirection = jointRotation * Vector3.forward;

    //    // Define the desired bend angle (in radians) - adjust this based on your needs
    //    float desiredBendAngle = Mathf.PI / 4.0f; // 45 degrees in radians

    //    // Calculate the target position based on the desired bend angle
    //    Vector3 targetPosition = joint.position + (bendDirection * Mathf.Tan(desiredBendAngle) * joint.localScale.z);

    //    return targetPosition;
    //}

    //void ApplyFingerIKPosition(Transform joint, Vector3 targetPosition)
    //{
    //    // Apply IK position for the finger joint
    //    joint.position = Vector3.Lerp(joint.position, targetPosition, fingerIKPositionWeight);
    //}

}
