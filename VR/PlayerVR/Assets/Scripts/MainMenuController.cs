using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject fileHolder;
    private FileReader fileReader;

    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI title;

    string path;
    string folder;
    bool fileValid;

    private void Start()
    {
        fileValid = false;
        playButton.interactable = false;
        fileReader = fileHolder.GetComponent<FileReader>();
        path = Application.persistentDataPath + "/LoopReality/";
        //path = "E:\\Julia\\PCL\\";
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
        bool isPcd = true;
        foreach (string line in System.IO.File.ReadLines(path + folder + @"/kinectv2-settings.vrfilm"))
        {
            if (line == "ply") isPcd = false;
            break;
        }

        Time.timeScale = 1f;
        StartCoroutine(LoadYourAsyncScene(isPcd));
    }

    IEnumerator LoadYourAsyncScene(bool isPcd)
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad;
        if (isPcd)
            asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Additive);
        else
            asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        if (isPcd)
            SceneManager.MoveGameObjectToScene(fileHolder, SceneManager.GetSceneByName("PlayerPCD"));
        else
            SceneManager.MoveGameObjectToScene(fileHolder, SceneManager.GetSceneByName("PlayerPLY"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public void Quit()
    {
        Debug.Log("Quitting player");
        Application.Quit();
    }
}
