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
    [SerializeField] private GameObject controllerMenu;

    [Header("XR Rig")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private XRRayInteractor rightRay;
    [SerializeField] private XRRayInteractor leftRay;

    [HideInInspector] public bool Pause;
    [HideInInspector] public bool menuActive;

    private void Start()
    {
        menuActive = true;
        rightRay.enabled = true;
        leftRay.enabled = true;

        controllerMenu.SetActive(false);
    }
    public void ToggleMenuToMovie()
    {
        menu.SetActive(false);
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        movie.SetActive(true);
        controllerMenu.SetActive(true);
        leftRay.enabled = false;
        rightRay.enabled = false;
        menuActive = false;
    }
    public void ToggleMovieToMenu()
    {
        movie.SetActive(false);
        controllerMenu.SetActive(false);
        mainCamera.clearFlags = CameraClearFlags.Skybox;
        menu.SetActive(true);
        leftRay.enabled = true;
        rightRay.enabled = true;
        menuActive = true;
    }
}
