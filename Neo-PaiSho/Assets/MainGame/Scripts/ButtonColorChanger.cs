using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public void ChangeButtonColor(Button button, Color newColor)
    {
        // Change the color of the button's image (if it has one)
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = newColor;
        }

        // Change the color of the button's text (if it has one)
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.color = newColor;
        }
    }
}
