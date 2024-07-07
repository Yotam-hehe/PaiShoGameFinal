using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using TMPro;

public class OptionsStart : MonoBehaviour
{
    public UnityEngine.UI.Button myButton;

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Instructions()
    {
        SceneManager.LoadScene(3);
    }

    public void ChangeSoundText()
    {
        if (myButton != null)
        {
            TextMeshProUGUI buttonText = myButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                if(buttonText.text == "Sound: On")
                {
                    buttonText.text = "Sound: Off";
                }
                else
                {
                    buttonText.text = "Sound: On";
                }
            }
            else
            {
                Debug.LogError("No Text component found on the button.");
            }
        }
        else
        {
            Debug.LogError("No Button component found.");
        }
    }
}
