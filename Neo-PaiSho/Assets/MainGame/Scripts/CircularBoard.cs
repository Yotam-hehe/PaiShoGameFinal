using JetBrains.Annotations;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngineInternal;
using static UnityEngine.Rendering.DebugUI.Table;

public class CircularBoard : MonoBehaviour
{
    //art
    [Header("Art")]
    public Material cylinderMaterialHover;
    public Material cylinderMaterial;

    [Header("Prefabs & Mats")]
    [SerializeField] private GameObject[] prefabs_Red;
    [SerializeField] private GameObject[] prefabs_White;
    [SerializeField] private Material[] teamMats;

    //game
    [Header("Game")]
    public Camera currentCam;// Camera
    public int cylCountX = 18;// Amount of tiles on x axis
    public int cylCountY = 18;// Amount of tiles on y axis
    public GameObject[,] piecesBoard;// Keeps track of pieces on board
    public GameObject[] red_Pieces;
    public GameObject[] white_Pieces;
    public GameObject[,] cylGameBoard;// Keeps track of board tiles
    public GameObject cylinderPrefab;// Prefab for cyls
    public GameObject DraggedPiece;// save the piece being dragged
    public GameObject linePrefab;
    public Vector3 middle;// Middle of board
    public Vector3 originalPos;
    public Vector2Int hover = -Vector2Int.one;// Vector for hovering function
    public Vector2Int hitPosition;
    public Vector3 Void = new Vector3(-999, -999, -999);
    public bool isDragging;
    public int Turn; // 1 is player 1 and 0 is player 2
    public int turnCounter;

    private Harmony Harmonies;
    private Ui_Manager uiManager;

    // Gates important
    public Vector2Int Gate_17_9 = new Vector2Int(17,9);
    public Vector2Int Gate_9_1 = new Vector2Int(9, 1);
    public Vector2Int Gate_9_17 = new Vector2Int(9, 17);
    public Vector2Int Gate_1_9 = new Vector2Int(1, 9);


    void Start()
    {
        Harmonies = GetComponent<Harmony>();
        uiManager = GetComponent<Ui_Manager>();
        turnCounter = 1;
        transform.position = new Vector3(9,0,9);
        middle = transform.position;
        GenerateBoard(1,1, cylCountX, cylCountY);

        //Gates (important for game) // works
        Vector2Int Gate_17_9 = new Vector2Int((int)cylGameBoard[17, 9].transform.position.x, (int)cylGameBoard[17, 9].transform.position.z);
        Vector2Int Gate_9_1 = new Vector2Int((int)cylGameBoard[9, 1].transform.position.x, (int)cylGameBoard[9, 1].transform.position.z);
        Vector2Int Gate_9_17 = new Vector2Int((int)cylGameBoard[9, 17].transform.position.x, (int)cylGameBoard[9, 17].transform.position.z);
        Vector2Int Gate_1_9 = new Vector2Int((int)cylGameBoard[1, 9].transform.position.x, (int)cylGameBoard[1, 9].transform.position.z);



        SpawningAll();
    }

    private void Update()
    {
        if (!currentCam)
        {
            currentCam = Camera.main;
            return;
        }

        Manager();
    }

    // Input managment
    public void Manager()
    {
        // Highlighting tiles
        RaycastHit info = new();
        Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);
        if (!isDragging)
        {
            TileHighlight(info, ray);
        }

        PieceMovementManager();
    }

    // Function that highlights the tiles that the player's mouse is hovering above
    private void TileHighlight(RaycastHit info, Ray ray)
    {
        // If collides with piece it doesnt highlight the place
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("InsideBoard")))
        {
            //if we already were on the board
            if (hitPosition != null)
            {
                if (cylGameBoard[hitPosition.x, hitPosition.y] != null)
                {
                    cylGameBoard[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Cyl");
                    cylGameBoard[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = cylinderMaterial;
                    cylGameBoard[hitPosition.x, hitPosition.y].transform.position = new Vector3(cylGameBoard[hitPosition.x, hitPosition.y].transform.position.x, -1.06f, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.z);
                }
                //cylGameBoard[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Cyl");
                //cylGameBoard[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = cylinderMaterial;
                //cylGameBoard[hitPosition.x, hitPosition.y].transform.position = new Vector3(cylGameBoard[hitPosition.x, hitPosition.y].transform.position.x, -1.06f, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.z);
            }
        }
        else if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Cyl", "Hover")))
        {

            // Get the indexes of the tile i've hit
            hitPosition =  Index(info.transform.gameObject);

            // If we're hovering a tile after not hovering any tiles
            if (hover == -Vector2Int.one && hitPosition != -Vector2Int.one)
            {
                hover = hitPosition;
                cylGameBoard[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                cylGameBoard[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = cylinderMaterialHover;
                cylGameBoard[hitPosition.x, hitPosition.y].transform.position = new Vector3(cylGameBoard[hitPosition.x, hitPosition.y].transform.position.x, -0.86f, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.z);
            }

            // If we were already hovering a tile, change the previous one
            if (hover != hitPosition)
            {
                cylGameBoard[hover.x, hover.y].layer = LayerMask.NameToLayer("Cyl");
                cylGameBoard[hover.x, hover.y].GetComponent<MeshRenderer>().material = cylinderMaterial;
                cylGameBoard[hover.x, hover.y].transform.position = new Vector3(cylGameBoard[hover.x, hover.y].transform.position.x, -1.06f, cylGameBoard[hover.x, hover.y].transform.position.z);
                hover = hitPosition;
                cylGameBoard[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                cylGameBoard[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = cylinderMaterialHover;
                cylGameBoard[hitPosition.x, hitPosition.y].transform.position = new Vector3(cylGameBoard[hitPosition.x, hitPosition.y].transform.position.x, -0.86f, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.z);
            }
        }
        else
        {
            if (hover != -Vector2Int.one)
            {
                cylGameBoard[hover.x, hover.y].layer = LayerMask.NameToLayer("Cyl");
                cylGameBoard[hover.x, hover.y].GetComponent<MeshRenderer>().material = cylinderMaterial;
                cylGameBoard[hover.x, hover.y].transform.position = new Vector3(cylGameBoard[hover.x, hover.y].transform.position.x, -1.06f, cylGameBoard[hover.x, hover.y].transform.position.z);
                hover = -Vector2Int.one;
            }
        }
    }

    private void PieceMovementManager()
    {
        // If press on pieces
        if (Input.GetMouseButtonDown(0))
        {
            GrabLogic();
        }

        // Dragging itself
        if (isDragging)
        {
            DragLogic();
        }

        // If let go of pieces snaps to place if on gates/board
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseLogic();
        }
    }

    private void GrabLogic()
    {
        int teamTurn = turnCounter % 2;


        RaycastHit info2;
        Ray ray2 = currentCam.ScreenPointToRay(Input.mousePosition);

        // If the piece is outside of the board
        if (Physics.Raycast(ray2, out info2, 100, LayerMask.GetMask("OutSideBoard")))
        {
            if (info2.collider.gameObject.GetComponent<PieceScript>().team == teamTurn)
            {
                isDragging = true;
                int x = (int)math.round(info2.point.x);
                int z = (int)math.round(info2.point.z);
                SaveOriginalPos(new Vector3(x, 0, z));
                DraggedPiece = info2.collider.gameObject;

                Vector2Int hit = new Vector2Int((int)info2.point.x, (int)info2.point.z);

                // Makes the tiles of the gates highlighted as possible placements
                if (hit != Gate_17_9 && hit != Gate_9_1 && hit != Gate_9_17 && hit != Gate_1_9)
                {
                    cylGameBoard[17, 9].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[17, 9].transform.position = new Vector3(cylGameBoard[17, 9].transform.position.x, -0.86f, cylGameBoard[17, 9].transform.position.z);
                    cylGameBoard[9, 1].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[9, 1].transform.position = new Vector3(cylGameBoard[9, 1].transform.position.x, -0.86f, cylGameBoard[9, 1].transform.position.z);
                    cylGameBoard[1, 9].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[1, 9].transform.position = new Vector3(cylGameBoard[1, 9].transform.position.x, -0.86f, cylGameBoard[1, 9].transform.position.z);
                    cylGameBoard[9, 17].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[9, 17].transform.position = new Vector3(cylGameBoard[9, 17].transform.position.x, -0.86f, cylGameBoard[9, 17].transform.position.z);

                }
            }
        }

        // If the piece inside board
        if (Physics.Raycast(ray2, out info2, 100, LayerMask.GetMask("InsideBoard")))
        {
            if (info2.collider.gameObject.GetComponent<PieceScript>().team == teamTurn)
            {
                //something
                isDragging = true;
                DraggedPiece = info2.collider.gameObject;
                int x = (int)math.round(info2.point.x);
                int z = (int)math.round(info2.point.z);
                SaveOriginalPos(new Vector3(x, 0, z));


                ShowMoves(x, z, cylGameBoard, DraggedPiece, cylinderMaterialHover);
            }
        }
    }

    private void DragLogic()
    {
        Ray forpos = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToGround = 0;

        // Ensure the piece is not dragged below the ground
        if (new Plane(Vector3.up, Vector3.zero).Raycast(forpos, out distanceToGround))
        {
            Vector3 newPosition = forpos.GetPoint(distanceToGround);
            DraggedPiece.transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }
    }

    private void ReleaseLogic()
    {
        if (isDragging)
        {
            // unhighlight the gates
            cylGameBoard[17, 9].GetComponent<MeshRenderer>().material = cylinderMaterial;
            cylGameBoard[17, 9].transform.position = new Vector3(cylGameBoard[17, 9].transform.position.x, -1.06f, cylGameBoard[17, 9].transform.position.z);
            cylGameBoard[9, 1].GetComponent<MeshRenderer>().material = cylinderMaterial;
            cylGameBoard[9, 1].transform.position = new Vector3(cylGameBoard[9, 1].transform.position.x, -1.06f, cylGameBoard[9, 1].transform.position.z);
            cylGameBoard[1, 9].GetComponent<MeshRenderer>().material = cylinderMaterial;
            cylGameBoard[1, 9].transform.position = new Vector3(cylGameBoard[1, 9].transform.position.x, -1.06f, cylGameBoard[1, 9].transform.position.z);
            cylGameBoard[9, 17].GetComponent<MeshRenderer>().material = cylinderMaterial;
            cylGameBoard[9, 17].transform.position = new Vector3(cylGameBoard[9, 17].transform.position.x, -1.06f, cylGameBoard[9, 17].transform.position.z);


            isDragging = false;
            Ray forpos = Camera.main.ScreenPointToRay(Input.mousePosition);
            double x = DraggedPiece.transform.position.x;
            double z = DraggedPiece.transform.position.z;
            int roundedX = (int)math.round(DraggedPiece.transform.position.x);
            int roundedZ = (int)math.round(DraggedPiece.transform.position.z);
            Vector2Int hit = new Vector2Int(roundedX, roundedZ);
            bool isLegal;

            // if released over the board
            if ((0 <= x && x < 18) && (0 <= z && z < 18))
            {
                isLegal = LegalMove(DraggedPiece, hit);

                //int orgX = (int)math.round(originalPos.x);
                //int orgZ = (int)math.round(originalPos.z);


                // if piece was inside of the board
                if (DraggedPiece.layer != LayerMask.NameToLayer("OutSideBoard"))
                {
                    //Possiblemoves pos = new Possiblemoves();
                    if (isLegal)
                    {
                        // last place the piece was at becomes none
                        piecesBoard[(int)math.round(originalPos.x), (int)math.round(originalPos.z)] = null;

                        // If lands on piece(move was checked if legal before) so destroy piece, Turn Ends
                        if (piecesBoard[roundedX, roundedZ] != null && piecesBoard[roundedX, roundedZ].GetComponent<PieceScript>().type != PieceType.none)
                        {
                            //DraggedDown(DraggedPiece);
                            GameObject pieceToDelete = piecesBoard[roundedX, roundedZ];
                            pieceToDelete.transform.position = Void;
                            piecesBoard[roundedX, roundedZ] = DraggedPiece;
                            DraggedPiece.transform.position = new Vector3(roundedX, DraggedPiece.transform.position.y, roundedZ);

                            UnShow(cylGameBoard, cylinderMaterial);
                            //pos.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);
                            //Possiblemoves.instance.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);

                            originalPos = Vector3.zero;
                            turnCounter++;
                            CheckHarmoniesNew(DraggedPiece);
                            CheckWin();

                            // Sound
                            FindObjectOfType<AudioManager>().Play("Piece Eat");
                        }
                        else//piece landed on an empty space, Turn Ends
                        {
                            //DraggedDown(DraggedPiece);

                            // new place gets piece
                            piecesBoard[roundedX, roundedZ] = DraggedPiece;
                            DraggedPiece.transform.position = new Vector3(roundedX, DraggedPiece.transform.position.y, roundedZ);


                            UnShow(cylGameBoard, cylinderMaterial);
                            //pos.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);
                            //Possiblemoves.instance.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);

                            originalPos = Vector3.zero;
                            turnCounter++;
                            CheckHarmoniesNew(DraggedPiece);
                            CheckWin();

                            // Sound
                            FindObjectOfType<AudioManager>().Play("Piece Move");
                        }
                    }
                    else //Move isnt Legal, Turn Doesnt End
                    {
                        DraggedPiece.transform.position = originalPos;

                        UnShow(cylGameBoard, cylinderMaterial);
                        //pos.UnShow(orgX, orgZ, cylGameBoard,DraggedPiece, cylinderMaterial);
                        //Possiblemoves.instance.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);

                        originalPos = Vector3.zero;


                        FindObjectOfType<AudioManager>().Play("Error");
                    }
                }

                // if piece was outside board
                else
                {
                    //if piece wasnt released at gates, Turn Doesnt End
                    if (hit != Gate_17_9 && hit != Gate_9_1 && hit != Gate_9_17 && hit != Gate_1_9)
                    {
                        DraggedPiece.transform.position = originalPos;
                        originalPos = Vector3.zero;

                        FindObjectOfType<AudioManager>().Play("Error");
                    }
                    else //if piece was released at gates, Turn Ends
                    {
                        if (piecesBoard[hit.x, hit.y] == null || piecesBoard[hit.x, hit.y].GetComponent<PieceScript>().type == PieceType.none)
                        {
                            piecesBoard[roundedX, roundedZ] = DraggedPiece;
                            DraggedPiece.transform.position = new Vector3(roundedX, DraggedPiece.transform.position.y, roundedZ);
                            DraggedPiece.layer = LayerMask.NameToLayer("InsideBoard");
                            originalPos = Vector3.zero;
                            turnCounter++;
                            CheckHarmoniesNew(DraggedPiece);
                            CheckWin();

                            // Sound
                            FindObjectOfType<AudioManager>().Play("Piece Move");
                        }
                        else
                        {
                            DraggedPiece.transform.position = originalPos;
                            originalPos = Vector3.zero;

                            FindObjectOfType<AudioManager>().Play("Error");
                        }
                    }

                }
            }
            else // if wasnt released over the board (return it to its original position)
            {
                DraggedPiece.transform.position = originalPos;
                originalPos = Vector3.zero;
                FindObjectOfType<AudioManager>().Play("Error");
            }
        }
    }


    // Moves Legality Check
    public bool LegalMove(GameObject DraggedPiece,Vector2Int v)
    {
        bool islegal=true;
        GameObject checkIfLandingOnPiece = piecesBoard[v.x, v.y];
        string tag = cylGameBoard[v.x, v.y].tag;
        PieceType pieceType = DraggedPiece.GetComponent<PieceScript>().type;

        // Is moving the right distance
        if (Vector3.Distance(originalPos, DraggedPiece.transform.position) > DraggedPiece.GetComponent<PieceScript>().moveDistance)
        {    
            islegal = false;
        }

        // Check if moving to an allowed square (the piece's color square or a neutral square)
        if(pieceType == PieceType.WhiteLotus || pieceType == PieceType.Orchid)
        {
            //can land anywhere
        }
        else if(DraggedPiece.GetComponent<PieceScript>().color == "Red")
        {
            if (tag == "White")
            {
                islegal = false;
            }
        }
        else if(DraggedPiece.GetComponent<PieceScript>().color == "White")
        {
            if (tag == "Red")
            {
                islegal = false;
            }
        }

        // Check if there is a piece in the landing space
        if(checkIfLandingOnPiece!=null)
        {
            if(DraggedPiece.GetComponent<PieceScript>().type == PieceType.Orchid)
            {
                islegal = DraggedPiece.GetComponent<Orchid>().IsClashing(checkIfLandingOnPiece);
            }
            else if(checkIfLandingOnPiece.GetComponent<PieceScript>().type == PieceType.Orchid)
            {
                if(DraggedPiece.GetComponent<PieceScript>().type == PieceType.WhiteLotus)
                {
                    islegal = false;
                }
            }
            // Checks if the piece is a piece that clashes with the dragged piece or not (if doesnt clash, goes into the if. if does clash, doesnt go into if)
            else if (DraggedPiece.GetComponent<PieceScript>().clash != checkIfLandingOnPiece.GetComponent<PieceScript>().type && checkIfLandingOnPiece.GetComponent<PieceScript>().type!=PieceType.none)
            {
                islegal = false;
            }
        }
        return islegal;
    }

    // Harmonies
    public void CheckHarmoniesNew(GameObject piece)
    {
        if (piece.GetComponent<PieceScript>().team == 0)
        {
            Harmonies.UpdateHarmonies(piece, white_Pieces, red_Pieces);
        }
        else
        {
            Harmonies.UpdateHarmonies(piece, red_Pieces, white_Pieces);
        }
    }
    public void CheckWin()
    {
        GameObject winningPiece = Harmonies.Win();
        if (winningPiece != null)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    // Possible Move Showing, Show and Unshow
    public void ShowMoves(int x, int y, GameObject[,] matrix, GameObject piece, Material hov)
    {
        if (matrix == null)
        {
            Debug.LogError("Matrix is null.");
            return;
        }
        int radius = piece.GetComponent<PieceScript>().moveDistance;
        Vector2 v = new Vector2(x, y);
        Vector2 right = new Vector2(x+radius, y);
        Vector2 top = new Vector2(x, y+radius);
        Vector2 left = new Vector2(x-(radius), y);
        Vector2 bottom = new Vector2(x, y-radius);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Vector2 vector2 = new Vector2(i, j);
                if(piece.GetComponent<PieceScript>().type == PieceType.WhiteLotus || piece.GetComponent<PieceScript>().type == PieceType.Orchid) 
                {
                    if (IsPointInsideOrOnRotatedSquare(bottom, left, top, right, vector2) && matrix[i, j] != null)
                    {
                        Debug.Log("Hi");
                        matrix[i, j].layer = LayerMask.NameToLayer("PosMoves");

                        MeshRenderer renderer = matrix[i, j].GetComponent<MeshRenderer>();
                        if (renderer != null)
                        {
                            renderer.material = hov;
                            matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, -0.86f, matrix[i, j].transform.position.z);
                        }
                        else
                        {
                            Debug.LogWarning("MeshRenderer not found on GameObject at (" + i + ", " + j + ").");
                        }
                    }
                }
                else if (IsPointInsideOrOnRotatedSquare(bottom, left, top, right, vector2) && matrix[i, j] != null && (matrix[i, j].tag == piece.GetComponent<PieceScript>().color || matrix[i, j].tag == "Neutral"))
                {
                    Debug.Log("Hi");
                    matrix[i, j].layer = LayerMask.NameToLayer("PosMoves");

                    MeshRenderer renderer = matrix[i, j].GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.material = hov;
                        matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, -0.86f, matrix[i, j].transform.position.z);
                    }
                    else
                    {
                        Debug.LogWarning("MeshRenderer not found on GameObject at (" + i + ", " + j + ").");
                    }
                }
            }
        }
    }

    // helper for show moves
    // Method Gets 4 diffrent points that form a square and another point,
    // and checks if that point is within or on the square
    public bool IsPointInsideOrOnRotatedSquare(Vector2 bottom, Vector2 left, Vector2 top, Vector2 right, Vector2 point)
    {
        // Calculate vectors for square
        Vector2 vectorBetweenBottomLeft = left - bottom;
        Vector2 vectorBetweenLeftTop = top - left;
        Vector2 vectorBetweenTopRight = right - top;
        Vector2 vectorBetweenRightBottom = bottom - right;

        // Check if the point is on or inside the square
        bool isInside = Vector2.Dot(point - bottom, vectorBetweenBottomLeft) >= 0 && Vector2.Dot(point - left, vectorBetweenLeftTop) >= 0 && Vector2.Dot(point - top, vectorBetweenTopRight) >= 0 && Vector2.Dot(point - right, vectorBetweenRightBottom) >= 0;

        //Check point is on the edges
        if (!isInside)
        {
            isInside = Mathf.Approximately(Vector2.Dot(point - bottom, vectorBetweenBottomLeft.normalized), 0f) && Vector2.Dot(point - bottom, vectorBetweenBottomLeft) <= vectorBetweenBottomLeft.sqrMagnitude && Mathf.Approximately(Vector2.Dot(point - left, vectorBetweenLeftTop.normalized), 0f) && Vector2.Dot(point - left, vectorBetweenLeftTop) <= vectorBetweenLeftTop.sqrMagnitude && Mathf.Approximately(Vector2.Dot(point - top, vectorBetweenTopRight.normalized), 0f) && Vector2.Dot(point - top, vectorBetweenTopRight) <= vectorBetweenTopRight.sqrMagnitude && Mathf.Approximately(Vector2.Dot(point - right, vectorBetweenRightBottom.normalized), 0f) && Vector2.Dot(point - right, vectorBetweenRightBottom) <= vectorBetweenRightBottom.sqrMagnitude;
        }

        return isInside;
    }

    // UnHighlighting the possible moves that were shown by the "ShowMoves" Function
    public void UnShow(GameObject[,] matrix,Material reg)
    {
        Debug.Log("heyo");
        if (matrix == null)
        {
            Debug.LogError("Matrix is null.");
            return;
        }
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] != null)
                {
                    if (matrix[i, j].layer == LayerMask.NameToLayer("PosMoves") && (matrix[i, j] != null))
                    {
                        matrix[i, j].layer = LayerMask.NameToLayer("Cyl");
                        matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, -1.06f, matrix[i, j].transform.position.z);
                        MeshRenderer renderer = matrix[i, j].GetComponent<MeshRenderer>();
                        if (renderer != null)
                        {
                            renderer.material = reg;
                        }
                    }
                }
            }
        }
    }

    // Saves original pos of piece, if move isnt legal the piece goes back to its original position
    public void SaveOriginalPos(Vector3 pos)
    {
        if (originalPos == Vector3.zero)
        {
            originalPos = pos;
        }
    }
    
    // Returns index of tile (x,y) on board 
    public Vector2Int Index(GameObject hit)
    {
        for (int i = 0; i < cylCountX; i++)
        {
            for (int j = 0; j < cylCountY; j++)
            {
                if (cylGameBoard[i, j] != null)
                {
                    if (cylGameBoard[i, j] == hit)
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }
        }

        // Invalid
        return -Vector2Int.one;
    }



    // Board

    // Operation that generates the board
    private void GenerateBoard(float tileSize, float height, int tileCountX, int tileCountY)
    {
        cylGameBoard = new GameObject[tileCountX, tileCountY];
        piecesBoard = new GameObject[tileCountX, tileCountY];

        for (int i = 0; i < tileCountX; i++)
        {
            for (int j = 0; j < tileCountY; j++)
            {
                Vector3 position = new Vector3(i, 0, j);

                // Adjust the radius value according to your circle's requirements
                float radius = 8.75f;

                if (Vector3.Distance(position, middle) <= radius)
                {
                    cylGameBoard[i, j] = GenerateTile(tileSize, height, i, j);
                    piecesBoard[i, j] = new GameObject();
                    piecesBoard[i,j].transform.position = new Vector3(i,0,j);
                    piecesBoard[i, j].transform.parent = transform;
                    piecesBoard[i, j].AddComponent<PieceScript>();
                    piecesBoard[i, j].GetComponent<PieceScript>().type = PieceType.none;

                    // assign red and white board tiles
                    Vector2Int V2_9 = new Vector2Int(2, 9);
                    Vector2Int V9_2 = new Vector2Int(9, 2);
                    Vector2Int V9_16 = new Vector2Int(9, 16);
                    Vector2Int V16_9 = new Vector2Int(16, 9);
                    Vector2Int mid = new Vector2Int(9, 9);
                    Vector2Int p = new Vector2Int(i, j);

                    bool IsRed = IsPointInTriangle(mid, V9_2, V2_9, p) || IsPointInTriangle(mid, V9_16, V16_9, p);
                    bool IsWhite = IsPointInTriangle(mid,V2_9,V9_16,p) || IsPointInTriangle(mid,V16_9,V9_2,p);
                    
                    if (IsRed) // Check if piece is supposed to be red
                    {
                        cylGameBoard[i, j].tag = "Red";
                    }
                    else if(IsWhite) // Check if piece is supposed to be white
                    {
                        cylGameBoard[i, j].tag = "White";
                    }
                    else
                    {
                        cylGameBoard[i, j].tag = "Neutral";
                    }
                }
            }
        }
    }
    
    // Operation that generates a single tile // part of board ops
    private GameObject GenerateTile(float tileSize, float height, int x, int y)
    {
        GameObject cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        cyl.transform.localScale = new Vector3(tileSize, height, tileSize);
        cyl.transform.position = new Vector3(x, -1.06f, y);

        cyl.name = string.Format("X:{0}, Y:{1}", x, y);
        cyl.transform.parent = transform;

        cyl.AddComponent<BoxCollider>();
        cyl.layer = LayerMask.NameToLayer("Cyl");
        cyl.GetComponent<MeshRenderer>().material = cylinderMaterial;
        cyl.tag = "Neutral";

        return cyl;
    }
    
    // Operation to check if point is within a triangle by vertexes and point // part of board ops
    bool IsPointInTriangle(Vector2Int A, Vector2Int B, Vector2Int C, Vector2Int P)
    {
        float tolerance = 0.01f;
        // Convert Vector2Int to Vector2 for calculations
        Vector2 AFloat = A;
        Vector2 BFloat = B;
        Vector2 CFloat = C;
        Vector2 PFloat = P;

        // Calculate the vectors AB, BC, and CA
        Vector2 AB = BFloat - AFloat;
        Vector2 BC = CFloat - BFloat;
        Vector2 CA = AFloat - CFloat;

        // Calculate the vectors AP, BP, and CP
        Vector2 AP = PFloat - AFloat;
        Vector2 BP = PFloat - BFloat;
        Vector2 CP = PFloat - CFloat;

        // Calculate the cross products
        float crossProductABP = Vector3.Cross(AB, AP).z;
        float crossProductBCP = Vector3.Cross(BC, BP).z;
        float crossProductCAP = Vector3.Cross(CA, CP).z;

        // Check if the signs of the cross products are consistent
        bool isInside = (crossProductABP > tolerance && crossProductBCP > tolerance && crossProductCAP > tolerance) ||
                        (crossProductABP < -tolerance && crossProductBCP < -tolerance && crossProductCAP < -tolerance);

        return isInside;
    }



    // Game Pieces

    // Operation that spawns all pieces (player and enemy)
    private void SpawningAll()
    {
        SpawnRed();
        SpawnWhite();
    }
    private void SpawnRed()
    {
        red_Pieces = new GameObject[20];
        int count=0;
        int piecetype = 0;
        int i; int j;
        for (i = -1; i>-4;i--)
        {
            piecetype = 0;
            for(j =-2; j<4; j++)
            {
                piecetype++;
                //SpawnPiece((PieceType)piecetype, 1, new Vector3(j, 0, i));
                red_Pieces[count] = SpawnPiece((PieceType)piecetype, 1, new Vector3(j, 0, i));
                count++;
            }
        }
        i += 1;
        piecetype = 10;
        for (j = 2; j < 4; j++)
        {
            piecetype++;
            //SpawnPiece((PieceType)piecetype, 1, new Vector3(j, 0, -4));
            red_Pieces[count] = SpawnPiece((PieceType)piecetype, 1, new Vector3(j, 0, -4));
            count++;
        }
    }
    private void SpawnWhite()
    {
        white_Pieces = new GameObject[20];
        int count = 0;
        int piecetype = 0;
        int i; int j;
        for (i = 19 ; i < 22; i++)
        {
            piecetype = 0;
            for (j = 20; j > 14; j--)
            {
                piecetype++;
                //SpawnPiece((PieceType)piecetype, 1, new Vector3(j, 0, i));
                white_Pieces[count] = SpawnPiece((PieceType)piecetype, 0, new Vector3(j, 0, i));
                white_Pieces[count].transform.rotation = Quaternion.Euler(270f, 180f, 0f);
                count++;
            }
        }
        i += 1;
        piecetype = 10;
        for (j = 16; j > 14; j--)
        {
            piecetype++;
            //SpawnPiece((PieceType)piecetype, 1, new Vector3(j, 0, -4));
            white_Pieces[count] = SpawnPiece((PieceType)piecetype, 0, new Vector3(j, 0, 22));
            white_Pieces[count].transform.rotation = Quaternion.Euler(270f, 180f, 0f);
            count++;
        }
    }

    // Operation to spawn a single piece by the type, the team and a vector of the place
    private GameObject SpawnPiece(PieceType pieceType, int team, Vector3 place)
    {
        GameObject piece = new GameObject(pieceType.ToString());
        if (team == 0)
        {
            piece = Instantiate(prefabs_White[(int)pieceType - 1], transform);
        }
        if(team == 1)
        {
            piece = Instantiate(prefabs_Red[(int)pieceType - 1], transform);
        }
        piece.transform.position = place;

        switch((int)pieceType) 
        {
            case 1:
                piece.AddComponent<Rose>();
                break;
            case 2:
                piece.AddComponent<Chrysanthemum>();
                break;
            case 3:
                piece.AddComponent<Rhododendron>();
                break;
            case 4:
                piece.AddComponent<Jasmine>();
                break;
            case 5:
                piece.AddComponent<Lily>();
                break;
            case 6:
                piece.AddComponent<WhiteJade>();
                break;
            case 11:
                piece.AddComponent<WhiteLotus>();
                break;
            case 12:
                piece.AddComponent<Orchid>();
                break;
        }

        piece.AddComponent<PieceScript>();
        piece.AddComponent<BoxCollider>().enabled = true;

        piece.GetComponent<PieceScript>().SetXnY(place.x, place.z);

        //piece.GetComponent<PieceScript>().x = place.x;
        //piece.GetComponent<PieceScript>().y = place.z;
        piece.GetComponent<PieceScript>().type = pieceType;
        piece.GetComponent<PieceScript>().team = team;
        piece.layer = LayerMask.NameToLayer("OutSideBoard");
        piece.transform.localScale = new Vector3(8,8,8);

        return piece;
    }
}
