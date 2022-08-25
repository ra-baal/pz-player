using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ControllerLogs : MonoBehaviour
{
    [SerializeField] private Button clearButton;
    [SerializeField] private Button saveButton;

    private bool visible;
    private float currentAlpha;
    [SerializeField] private GameObject camera;
    private Transform cameraTransform;

    private void OnValidate()
    {
        Assert.IsNotNull(clearButton);
        Assert.IsNotNull(saveButton);
    }

    private void Start()
    {
        //clearButton.onClick.AddListener(() => { LogText.text = ""; });
        //saveButton.onClick.AddListener(SaveLogs);
        //Debug.Log("LOG AWAKE");
        cameraTransform = camera.transform;
    }

    void Update()
    {
        float angle = Vector3.Angle(transform.forward, cameraTransform.forward);
        if (angle < 80.0f)
        {
            currentAlpha = angle <= 50.0f ? 1.0f : (angle - 50.0f) / 30.0f;
            if (!visible)
            {
                visible = true;
            }
        }
        else
        {
            currentAlpha = 0.0f;
            if (visible)
            {
                visible = false;
            }
        }
        GetComponent<CanvasGroup>().alpha = currentAlpha;
    }
    void OnDestroy()
    {
        clearButton.onClick.RemoveAllListeners();
        saveButton.onClick.RemoveAllListeners();
    }
}