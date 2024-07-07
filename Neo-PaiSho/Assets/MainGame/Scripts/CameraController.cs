using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 1.0f;
    private Camera _camera;
    public bool canRotate;
    private string teamTurn;
    Vector3 piv = new Vector3 (9, 0, 9);
    private bool flag;

    //camera positions
    private Vector3 Red;//position of red camera
    private Vector3 White;//position of white camera

    // Variables for camera smooth movement
    private Quaternion targetRotationRed = Quaternion.Euler(60f, 0f, 0f); //The target rotation for the start of the red turn
    private Quaternion targetRotationWhite = Quaternion.Euler(60f, 180f, 0f); //The target rotation for the start of the white turn
    private float Duration = 1f; //Total time of camera transition
    private float elapsedTime = 0f; //Variable to keep track of the elapsed time

    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        _camera = Camera.main;
        
        Red = new Vector3(9,20,-6);
        White = new Vector3(9, 20, 24);

        canRotate = false; 


        teamTurn = GetTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }


        // Camera rotation around pivot point (middle of the board) 
        Vector3 pivotPoint = piv;
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();

            //// Rotate to the left
            //float rotationSpeed = sensitivity * Time.deltaTime*10f;
            //transform.RotateAround(pivotPoint, Vector3.up, rotationSpeed);
            //_camera.transform.RotateAround(pivotPoint, Vector3.up, rotationSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            RotateRight();

            //// Rotate to the right
            //float rotationSpeed = sensitivity * Time.deltaTime*10f;
            //transform.RotateAround(pivotPoint, Vector3.up, -rotationSpeed);
            //_camera.transform.RotateAround(pivotPoint, Vector3.up, -rotationSpeed);
        }

        // Defult Camera location
        if (Input.GetKeyDown(KeyCode.F))
        {
            Defult();
        }

        //Change turns
        if (teamTurn != GetTurn())
        {
            teamTurn = GetTurn();
            StartTurnChange();
        }

        // Smooth turn change
        if (flag)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the fraction of the rotation completed
            float t = elapsedTime / Duration;

            // Smoothly transition the rotation of the camera to the desired rotation
            _camera.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            // Smoothly transition the movement of the camera to the desired position
            _camera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // If the rotation and movement is complete, stop updating
            if (t >= 1.0f)
            {
                flag = false;
                elapsedTime = 0f;
            }
        }
    }

    public void RotateLeft()
    {
        Vector3 pivotPoint = piv;
        // Rotate to the left
        float rotationSpeed = sensitivity * Time.deltaTime * 10f;
        transform.RotateAround(pivotPoint, Vector3.up, rotationSpeed);
        _camera.transform.RotateAround(pivotPoint, Vector3.up, rotationSpeed);
    }
    public void RotateRight()
    {
        Vector3 pivotPoint = piv;
        // Rotate to the right
        float rotationSpeed = sensitivity * Time.deltaTime * 10f;
        transform.RotateAround(pivotPoint, Vector3.up, -rotationSpeed);
        _camera.transform.RotateAround(pivotPoint, Vector3.up, -rotationSpeed);
    }
    public void StartTurnChange()
    {
        flag = true;
        elapsedTime = 0f;

        if(teamTurn == "White") 
        {
            initialRotation = _camera.transform.rotation;
            initialPosition = _camera.transform.position;
            targetRotation = Quaternion.Euler(60f, 180f, 0f);
            targetPosition = White;
        }
        else if (teamTurn == "Red")
        {
            initialRotation = _camera.transform.rotation;
            initialPosition = _camera.transform.position;
            targetRotation = Quaternion.Euler(60f, 0f, 0f);
            targetPosition = Red;
        }
    }

    //Camera rotation based on mouse movement
    public void Rotate()
    {
        if (canRotate)
        {
            canRotate = false;
        }
        else
        {
            canRotate = true;
        }
    }

    //Set camera to defult position
    public void Defult()
    {
        string turn = GetTurn();
        if(turn == "Red") 
        {
            RedCam();
        }
        else if(turn == "White")
        {
            WhiteCam();
        }
    }

    //Camera for player 1 and 2 logic

    //Returns the name of the team that is playing in the current turn
    private string GetTurn()
    {
        CircularBoard turnCounterScript = FindObjectOfType<CircularBoard>();
        if (turnCounterScript != null)
        {
            int currentTurn = turnCounterScript.turnCounter;
            if (currentTurn % 2 == 0)
            {
                return "White";
            }
            else
            {
                return "Red";
            }
        }
        return "Error";
    }
    public void RedCam()
    {
        _camera.transform.position = Red;
        _camera.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
    }
    public void WhiteCam()
    {
        _camera.transform.position = White;
        _camera.transform.rotation = Quaternion.Euler(60f, 180f, 0f);
    }
}
