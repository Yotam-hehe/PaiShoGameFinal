using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvasScript : MonoBehaviour
{
    public GameObject canvas;
    private void Awake()
    {
        canvas.SetActive(false);
        canvas.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
