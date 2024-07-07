using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDisplay : MonoBehaviour
{
    int turnCount;
    // Start is called before the first frame update
    void Start()
    {
        CircularBoard circularBoard = GetComponent<CircularBoard>();
        turnCount = circularBoard.turnCounter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
