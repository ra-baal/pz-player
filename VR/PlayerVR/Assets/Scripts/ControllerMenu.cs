using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerMenu : MonoBehaviour
{
    private new GameObject particleSystem;
    private GameObject menu;
    private MainMenuController mmc;

    [SerializeField] private TextMeshProUGUI pausePlayText;
    [SerializeField] private Button pausePlayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private GameObject rightController;

    private float alpha;
    private bool visible;

    private new Camera camera;

    private void OnValidate()
    {
        Assert.IsNotNull(pausePlayButton);
        Assert.IsNotNull(mainMenuButton);
        Assert.IsNotNull(quitButton);
    }

    private void Start()
    {
        rightController = GameObject.Find("RightHand Controller");
        particleSystem = GameObject.Find("Particle System");
        menu = GameObject.Find("MenuController");
        mmc = menu.GetComponent<MainMenuController>();

        mainMenuButton.onClick.AddListener(() => {
            particleSystem.SetActive(false);
            camera.clearFlags = CameraClearFlags.Skybox;
            menu.SetActive(true);
        });
        quitButton.onClick.AddListener(() => { Application.Quit(); });
        pausePlayButton.onClick.AddListener(PausePlay);
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        float angle = Vector3.Angle(transform.forward, camera.transform.forward);
        if (angle < 60.0f && !mmc.Pause)
        {
            alpha = 1.0f;
            if (!visible)
            {
                visible = true;
                rightController.GetComponent<XRRayInteractor>().enabled = true;
            }
        }
        else
        {
            alpha = 0.0f;
            if (visible)
            {
                visible = false;
                rightController.GetComponent<XRRayInteractor>().enabled = false;
            }
        }
        GetComponent<CanvasGroup>().alpha = alpha;
    }
    private void PausePlay()
    {
        if (mmc.Pause)
        {
            mmc.Pause = false;
            Time.timeScale = 1f;
            pausePlayText.text = "Play";
        }
        else
        {
            mmc.Pause = true;
            Time.timeScale = 0f;
            pausePlayText.text = "Pause";
        }
    }
    void OnDestroy()
    {
        pausePlayButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }
}