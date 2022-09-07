using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerSystem : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject movie;

    [Header("XR Rig")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private XRRayInteractor rightRay;
    [SerializeField] private XRRayInteractor leftRay;

    [HideInInspector] public bool Pause;
    [HideInInspector] public bool menuActive;

    private InputDevice rightController;
    private InputDevice leftController;

    private void Start()
    {
        menuActive = true;
        rightRay.enabled = true;
        leftRay.enabled = false;

        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics character = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(character, devices);
        foreach (var device in devices)
        {
            Debug.Log(device.name + device.characteristics);
        }
        if (devices.Count > 0) rightController = devices[0];

        devices = new List<InputDevice>();
        character = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(character, devices);
        foreach (var device in devices)
        {
            Debug.Log(device.name + device.characteristics);
        }
        if (devices.Count > 0) leftController = devices[0];
    }
    public void ToggleMenuToMovie()
    {
        menu.SetActive(false);
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        movie.SetActive(true);
        leftRay.enabled = false;
        rightRay.enabled = false;
        menuActive = false;
    }
    public void ToggleMovieToMenu()
    {
        movie.SetActive(false);
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        menu.SetActive(true);
        leftRay.enabled = false;
        rightRay.enabled = true;
        menuActive = true;
    }
    private void Update()
    {
        if (menuActive)
        {
            rightController.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerValue);
            if (rightTriggerValue >= 0.1f)
            {
                if (!rightRay.enabled)
                {
                    leftRay.enabled = false;
                    rightRay.enabled = true;
                }
                Debug.Log("Right trigger pressed: " + rightTriggerValue);
            }
            leftController.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerValue);
            if (leftTriggerValue >= 0.1f)
            {
                if (rightRay.enabled)
                {
                    leftRay.enabled = true;
                    rightRay.enabled = false;
                }
                Debug.Log("Left trigger pressed: " + leftTriggerValue);
            }
        }
    }
}
