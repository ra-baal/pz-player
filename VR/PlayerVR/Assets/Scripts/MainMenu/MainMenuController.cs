using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //public bool fileValid = false; //bedzie trzeba wywalic blad jak plik bedzie zly yadayada
    //public Button playButton;
    public GameObject fileHolder = null;
    //private FileReader fileReader;
    //string path;

    bool fileValid = false; //bedzie trzeba wywalic blad jak plik bedzie zly yadayada
    public Button playButton;

    private void Start()
    {
        fileValid = false;
        playButton.interactable = false;
    }
    public void LoadFile()
    {
        Debug.Log("File loaded and can be transfered to the next scene");
        fileValid = true;
        playButton.interactable = true;
    }
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
            SceneManager.MoveGameObjectToScene(fileHolder, SceneManager.GetSceneByName("scenePCD"));
        else
            SceneManager.MoveGameObjectToScene(fileHolder, SceneManager.GetSceneByName("scenePLY"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public void Quit()
    {
        Debug.Log("Quitting player");
        Application.Quit();
    }
}
