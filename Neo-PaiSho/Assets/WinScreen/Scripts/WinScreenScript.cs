using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class WinScreenScript : MonoBehaviour
{
    public Image imgRight;
    public Image imgLeft;
    public GameObject WinText;
    public GameObject WinScreen;
    private bool isActiveScene = false;
    public Camera camera;
    private float time = 0f;
    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        light.SetActive(false);
        WinText.SetActive(false);
        WinScreen.SetActive(false);
    }

    private void CheckActiveScene()
    {
        // Set the flag based on whether the current scene is the active scene
        isActiveScene = SceneManager.GetActiveScene() == gameObject.scene;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActiveScene)
        {
            CheckActiveScene();
            if(light.activeInHierarchy)
            {
                light.SetActive(false);
            }
            ResetCurtains();
        }
        else
        {

            UniversalAdditionalCameraData camData = camera.GetComponent<UniversalAdditionalCameraData>();
            if(camData.renderType!= CameraRenderType.Base)
            {
                light.SetActive(true);
                camData.renderType = CameraRenderType.Base;
                camera = Camera.main;
                WinScreen.SetActive(true);
            }
            time += Time.time;
            if(time > 2f) 
            {
                Curtains();
            }
        }

    }

    // Black Screen proccess aka black curtains
    public void Curtains()
    {
        if (imgLeft.fillAmount < 0.7 && imgRight.fillAmount < 0.7)
        {
            imgLeft.fillAmount = Mathf.PingPong(Time.time * 0.3f, 1.0f);
            imgRight.fillAmount = Mathf.PingPong(Time.time * 0.3f, 1.0f);
        }
        else
        {
            WinText.SetActive(true);
        }
    }
    public void ResetCurtains()
    {
        imgLeft.fillAmount = 0f; 
        imgRight.fillAmount = 0f;
    }
}
