using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public bool Pause;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject movie;

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Camera mainCamera;

    public GameObject fileHolder;
    private FileReader fileReader;

    private string path;
    private string folder;


    private void Awake()
    {
        mainCamera.clearFlags = CameraClearFlags.Skybox;
    }
    private void Start()
    {
        Pause = false;
        playButton.interactable = false;
        fileReader = fileHolder.GetComponent<FileReader>();
        path = Application.persistentDataPath + "/LoopReality/";
    }
    public void LoadFile(string selectedTitle)
    {
        folder = selectedTitle;
        if (System.IO.File.Exists(path + folder + "/kinectv2-settings.vrfilm"))
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
        menu.SetActive(false);
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        movie.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quitting player");
        Application.Quit();
    }
}
