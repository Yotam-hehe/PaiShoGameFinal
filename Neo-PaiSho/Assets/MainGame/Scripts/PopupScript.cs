using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupScript : MonoBehaviour
{
    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}
