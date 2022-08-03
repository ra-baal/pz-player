using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject fileReader;

    public Button goBackButton;
    public Button unpauseButton;

    public bool isPaused = false;

    private void Start()
    {
        fileReader = FindObjectOfType<FileReader>().gameObject;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    private void OnEnable()
    {
        goBackButton.onClick.AddListener(GoBackToMainMenu);
        unpauseButton.onClick.AddListener(UnPause);
    }
    
    private void OnDisable()
    {
        goBackButton.onClick.RemoveListener(GoBackToMainMenu);
        unpauseButton.onClick.RemoveListener(UnPause);
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnPause()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoBackToMainMenu()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad;
        asyncLoad = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(fileReader, SceneManager.GetSceneByName("MainMenuScene"));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.Escape) == true) && isPaused)
        {
            Debug.Log("unpause");
            UnPause();
        }
        else if((Input.GetKey(KeyCode.Escape)==true) && !isPaused)
        {
            Debug.Log("pause");
            Pause();
        }
    }
}
