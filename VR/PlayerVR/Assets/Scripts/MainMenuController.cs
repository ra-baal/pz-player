using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Control")]
    [SerializeField] private ControllerSystem cs;
    [SerializeField] private FileReader fileReader;
    [SerializeField] private Camera mainCamera;

    [Header("UI")]
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI title;

    private string path;
    private string folder;

    private void Awake()
    {
        mainCamera.clearFlags = CameraClearFlags.Skybox;
    }
    private void Start()
    {
        playButton.interactable = false;
        path = Application.persistentDataPath + "/LoopReality/";
    }
    public void LoadFile(string selectedTitle)
    {
        folder = selectedTitle;
        if (System.IO.File.Exists(path + folder + "/settings.vrfilm"))
        {
            fileReader.SetVariables(path + folder);
            Debug.Log("Folder loaded and can be transfered to the next scene");
            if (!playButton.interactable)
                playButton.interactable = true;
        }
        else
        {
            Debug.Log("Invalid folder");
            playButton.interactable = false;
        }
    }
    public void Play()
    {
        cs.ToggleMenuToMovie();
    }

    public void Quit()
    {
        Debug.Log("Quitting player");
        Application.Quit();
    }
}
