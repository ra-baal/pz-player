using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public bool fileValid = false; //bedzie trzeba wywalic blad jak plik bedzie zly yadayada
    public Button playButton;
    public GameObject fileHolder;
    private FileReader fileReader;
    public TMP_InputField inputField;
    string path;

    private void Start()
    {
        fileValid = false;
        playButton.interactable = true;
        fileReader = fileHolder.GetComponent<FileReader>();
    }
    public void LoadFile()
    {
        Apply();
        Debug.Log("Folder loaded and can be transfered to the next scene");
        fileValid = true;
        playButton.interactable = true;
    }
    public void Apply()
    {
        //path = EditorUtility.OpenFolderPanel("Load video files", "", "");
        path = inputField.text;
        fileReader.SetVariables(path);
    }

    public void Play()
    {
        path = inputField.text;
        fileReader.SetVariables(path);

        bool isPcd = true;
        foreach (string line in System.IO.File.ReadLines(path + @"/frame-settings.vrfilm"))
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
            SceneManager.MoveGameObjectToScene(fileHolder, SceneManager.GetSceneByName("PlayScene"));
        else
            SceneManager.MoveGameObjectToScene(fileHolder, SceneManager.GetSceneByName("PlayScenePLY"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public void Quit()
    {
        Debug.Log("Quitting player");
        Application.Quit();
    }
}
