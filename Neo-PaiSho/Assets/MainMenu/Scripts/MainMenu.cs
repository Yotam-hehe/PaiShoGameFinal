using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Camera Camera;
    public Button button;
    GameObject menu;
    GameObject loadingInterface;
    Image loadingProgressBar;

    public void Play()
    {
        FindObjectOfType<AudioManager>().UnLoop("Theme");
        FindObjectOfType<AudioManager>().Play("GameStart");
        SceneManager.LoadScene(1);
    }

    public void OpenOptions()
    {
        //SceneManager.LoadScene("OptionsScene");
    }
    public void Back()
    {
        button.buttonText.text = "Quit";
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
