using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Assertions;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text LogText;
    [SerializeField] private TextMeshProUGUI FPS;
    [SerializeField] private TextMeshProUGUI Clock;
    [SerializeField] private TextMeshProUGUI Temperature;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button saveButton;

    private float alpha;
    private bool visible;

    private new Camera camera;

    private void OnValidate()
    {
        Assert.IsNotNull(LogText);
        Assert.IsNotNull(FPS);
        Assert.IsNotNull(Clock);
        Assert.IsNotNull(Temperature);
        Assert.IsNotNull(clearButton);
        Assert.IsNotNull(saveButton);
    }

    private void Start()
    {
        //clearButton.onClick.AddListener(() => { LogText.text = ""; });
        //saveButton.onClick.AddListener(SaveLogs);
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
            }
        }
        else
        {
            alpha = 0.0f;
            if (visible)
            {
                visible = false;
            }
        }
        GetComponent<CanvasGroup>().alpha = alpha;
    }
    void OnDestroy()
    {
        saveButton.onClick.RemoveAllListeners();
    }
}