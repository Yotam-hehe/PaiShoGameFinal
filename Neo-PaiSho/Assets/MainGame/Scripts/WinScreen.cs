using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreenCanvas;// Unity canvas the includes the win screen object
    public TextMeshProUGUI text;// Win

    public void Start()
    {
        HideWinScreen();
        text = null;
    }
    // Call this method to show the win screen
    public void ShowWinScreen(int team)
    {
        winScreenCanvas.SetActive(true);
        if (team == 0)
        {
            text.text = "P2 Wins!";
        }
        else if(team==1)
        {
            text.text = "P1 Wins!";
        }

    }

    // Call this method to hide the win screen
    public void HideWinScreen()
    {
        winScreenCanvas.SetActive(false);
    }

    // Example method attached to a button to restart the game
    public void RestartGame()
    {
        // Add logic here to restart the game
        Debug.Log("Restarting game...");
    }
}
