using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pausePlayText;
    [SerializeField] private Button pausePlayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    [Header("Setup")]
    [SerializeField] private ControllerSystem cs;
    [SerializeField] private XRRayInteractor rightController;

    private float alpha;
    private bool visible;

    private Camera mainCamera;

    private void OnValidate()
    {
        Assert.IsNotNull(pausePlayButton);
        Assert.IsNotNull(mainMenuButton);
        Assert.IsNotNull(quitButton);
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => { cs.ToggleMovieToMenu(); });;
        quitButton.onClick.AddListener(() => { Application.Quit(); });
        pausePlayButton.onClick.AddListener(PausePlay);

        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var controller = GameObject.Find("[LeftHand Controller] Model Parent").transform;
        gameObject.transform.SetParent(controller, false);
    }
    void Update()
    {
        float angle = Vector3.Angle(transform.forward, mainCamera.transform.forward);
        if (angle < 60.0f && !cs.menuActive)
        {
            alpha = 1.0f;
            if (!visible)
            {
                visible = true;
                rightController.enabled = true;
            }
        }
        else
        {
            alpha = 0.0f;
            if (visible)
            {
                visible = false;
                rightController.enabled = false;
            }
        }
        GetComponent<CanvasGroup>().alpha = alpha;
    }
    private void PausePlay()
    {
        if (cs.Pause)
        {
            cs.Pause = false;
            Time.timeScale = 1f;
            pausePlayText.text = "Play";
        }
        else
        {
            cs.Pause = true;
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