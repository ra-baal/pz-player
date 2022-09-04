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
    [SerializeField] private TextMeshProUGUI pausePlayText;
    [SerializeField] private Button pausePlayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private GameObject rightController;

    private float alpha;
    private bool visible;
    private bool pause;

    private new Camera camera;

    private void OnValidate()
    {
        Assert.IsNotNull(pausePlayButton);
        Assert.IsNotNull(mainMenuButton);
        Assert.IsNotNull(quitButton);
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => { SceneManager.LoadScene("MainMenu"); });
        quitButton.onClick.AddListener(() => { Application.Quit(); });
        pausePlayButton.onClick.AddListener(PausePlay);
        camera = Camera.main;
    }
    void Update()
    {
        float angle = Vector3.Angle(transform.forward, camera.transform.forward);
        if (angle < 80.0f)
        {
            alpha = angle <= 50.0f ? 1.0f : (angle - 50.0f) / 30.0f;
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
        if (pause)
        {
            pausePlayText.text = "Play";
        }
        else
        {
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