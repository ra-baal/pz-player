using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
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

    public void Quit()
    {
        Debug.Log("Quitting player");
        Application.Quit();
    }
}
