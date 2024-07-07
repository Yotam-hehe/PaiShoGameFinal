using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private CircularBoard CircularBoard;
    private Camera currentCam;// Camera
    public bool isDragging;
    public Vector2Int hitPosition;
    public GameObject[,] cylGameBoard;// Keeps track of board tiles
    public Material cylinderMaterial;
    public Material cylinderMaterialHover;
    public Vector2Int hover = -Vector2Int.one;// Vector for hovering function
    public int turnCounter;
    public GameObject DraggedPiece;// save the piece being dragged
    public Vector3 originalPos;
    public GameObject[,] piecesBoard;// Keeps track of pieces on board
    public Vector3 Void = new Vector3(-999, -999, -999);

    // Gates important
    public Vector2Int Gate_17_9 = new Vector2Int(17, 9);
    public Vector2Int Gate_9_1 = new Vector2Int(9, 1);
    public Vector2Int Gate_9_17 = new Vector2Int(9, 17);
    public Vector2Int Gate_1_9 = new Vector2Int(1, 9);


    void Start()
    {
        CircularBoard = GetComponent<CircularBoard>();
        currentCam = CircularBoard.currentCam;
        isDragging = CircularBoard.isDragging;
        hitPosition = CircularBoard.hitPosition;
        cylGameBoard = CircularBoard.cylGameBoard;
        cylinderMaterial = CircularBoard.cylinderMaterial;
        cylinderMaterialHover = CircularBoard.cylinderMaterialHover;
        turnCounter = CircularBoard.turnCounter;
        DraggedPiece = CircularBoard.DraggedPiece;
        originalPos = CircularBoard.originalPos;
        piecesBoard = CircularBoard.piecesBoard;
    }

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
            hitPosition = CircularBoard.Index(info.transform.gameObject);

            // If we're hovering a tile after not hovering any tiles
            if (hover == -Vector2Int.one && hitPosition != -Vector2Int.one)
            {
                hover = hitPosition;
                cylGameBoard[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                cylGameBoard[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = cylinderMaterialHover;
                cylGameBoard[hitPosition.x, hitPosition.y].transform.position = new Vector3(cylGameBoard[hitPosition.x, hitPosition.y].transform.position.x, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.y + 0.2f, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.z);
            }

            // If we were already hovering a tile, change the previous one
            if (hover != hitPosition)
            {
                cylGameBoard[hover.x, hover.y].layer = LayerMask.NameToLayer("Cyl");
                cylGameBoard[hover.x, hover.y].GetComponent<MeshRenderer>().material = cylinderMaterial;
                cylGameBoard[hover.x, hover.y].transform.position = new Vector3(cylGameBoard[hover.x, hover.y].transform.position.x, cylGameBoard[hover.x, hover.y].transform.position.y - 0.2f, cylGameBoard[hover.x, hover.y].transform.position.z);
                hover = hitPosition;
                cylGameBoard[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                cylGameBoard[hitPosition.x, hitPosition.y].GetComponent<MeshRenderer>().material = cylinderMaterialHover;
                cylGameBoard[hitPosition.x, hitPosition.y].transform.position = new Vector3(cylGameBoard[hitPosition.x, hitPosition.y].transform.position.x, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.y + 0.2f, cylGameBoard[hitPosition.x, hitPosition.y].transform.position.z);
            }
        }
        else
        {
            if (hover != -Vector2Int.one)
            {
                cylGameBoard[hover.x, hover.y].layer = LayerMask.NameToLayer("Cyl");
                cylGameBoard[hover.x, hover.y].GetComponent<MeshRenderer>().material = cylinderMaterial;
                cylGameBoard[hover.x, hover.y].transform.position = new Vector3(cylGameBoard[hover.x, hover.y].transform.position.x, cylGameBoard[hover.x, hover.y].transform.position.y - 0.2f, cylGameBoard[hover.x, hover.y].transform.position.z);
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
                CircularBoard.SaveOriginalPos(new Vector3(x, 0, z));
                DraggedPiece = info2.collider.gameObject;

                Vector2Int hit = new Vector2Int((int)info2.point.x, (int)info2.point.z);

                // Makes the tiles of the gates highlighted as possible placements
                if (hit != Gate_17_9 && hit != Gate_9_1 && hit != Gate_9_17 && hit != Gate_1_9)
                {
                    cylGameBoard[17, 9].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[17, 9].transform.position = new Vector3(cylGameBoard[17, 9].transform.position.x, cylGameBoard[17, 9].transform.position.y + 0.2f, cylGameBoard[17, 9].transform.position.z);
                    cylGameBoard[9, 1].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[9, 1].transform.position = new Vector3(cylGameBoard[9, 1].transform.position.x, cylGameBoard[9, 1].transform.position.y + 0.2f, cylGameBoard[9, 1].transform.position.z);
                    cylGameBoard[1, 9].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[1, 9].transform.position = new Vector3(cylGameBoard[1, 9].transform.position.x, cylGameBoard[1, 9].transform.position.y + 0.2f, cylGameBoard[1, 9].transform.position.z);
                    cylGameBoard[9, 17].GetComponent<MeshRenderer>().material = cylinderMaterialHover; cylGameBoard[9, 17].transform.position = new Vector3(cylGameBoard[9, 17].transform.position.x, cylGameBoard[9, 17].transform.position.y + 0.2f, cylGameBoard[9, 17].transform.position.z);

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
                CircularBoard.SaveOriginalPos(new Vector3(x, 0, z));


                CircularBoard.ShowMoves(x, z, cylGameBoard, DraggedPiece, cylinderMaterialHover);
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
            isDragging = false;
            Ray forpos = Camera.main.ScreenPointToRay(Input.mousePosition);
            double x = DraggedPiece.transform.position.x;
            double z = DraggedPiece.transform.position.z;
            int roundedX = (int)math.round(DraggedPiece.transform.position.x);
            int roundedZ = (int)math.round(DraggedPiece.transform.position.z);
            Vector2Int hit = new Vector2Int(roundedX, roundedZ);
            bool isLegal;

            // if released over the board
            if ((0 <= x && x <= 18) && (0 <= z && z <= 18))
            {
                isLegal = CircularBoard.LegalMove(DraggedPiece, hit);

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
                        if (piecesBoard[roundedX, roundedZ] != null)
                        {
                            //DraggedDown(DraggedPiece);
                            GameObject pieceToDelete = piecesBoard[roundedX, roundedZ];
                            pieceToDelete.transform.position = Void;
                            piecesBoard[roundedX, roundedZ] = DraggedPiece;
                            DraggedPiece.transform.position = new Vector3(roundedX, DraggedPiece.transform.position.y, roundedZ);

                            CircularBoard.UnShow(cylGameBoard, cylinderMaterial);
                            //pos.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);
                            //Possiblemoves.instance.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);

                            originalPos = Vector3.zero;
                            turnCounter++;
                            CircularBoard.CheckHarmoniesNew(DraggedPiece);
                            CircularBoard.CheckWin();
                        }
                        else//piece landed on an empty space, Turn Ends
                        {
                            //DraggedDown(DraggedPiece);

                            // new place gets piece
                            piecesBoard[roundedX, roundedZ] = DraggedPiece;
                            DraggedPiece.transform.position = new Vector3(roundedX, DraggedPiece.transform.position.y, roundedZ);


                            CircularBoard.UnShow(cylGameBoard, cylinderMaterial);
                            //pos.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);
                            //Possiblemoves.instance.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);

                            originalPos = Vector3.zero;
                            turnCounter++;
                            CircularBoard.CheckHarmoniesNew(DraggedPiece);
                            CircularBoard.CheckWin();
                        }
                    }
                    else //Move isnt Legal, Turn Doesnt End
                    {
                        DraggedPiece.transform.position = originalPos;

                        CircularBoard.UnShow(cylGameBoard, cylinderMaterial);
                        //pos.UnShow(orgX, orgZ, cylGameBoard,DraggedPiece, cylinderMaterial);
                        //Possiblemoves.instance.UnShow(orgX, orgZ, cylGameBoard, DraggedPiece, cylinderMaterial);

                        originalPos = Vector3.zero;
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
                    }
                    else //if piece was released at gates, Turn Ends
                    {
                        piecesBoard[roundedX, roundedZ] = DraggedPiece;
                        DraggedPiece.transform.position = new Vector3(roundedX, DraggedPiece.transform.position.y, roundedZ);
                        DraggedPiece.layer = LayerMask.NameToLayer("InsideBoard");
                        originalPos = Vector3.zero;
                        turnCounter++;
                        CircularBoard.CheckHarmoniesNew(DraggedPiece);
                        CircularBoard.CheckWin();
                    }

                }
            }
            else // if wasnt released over the board (return it to its original position)
            {
                DraggedPiece.transform.position = originalPos;
                originalPos = Vector3.zero;
            }

            // unhighlight the gates
            cylGameBoard[17, 9].GetComponent<MeshRenderer>().material = cylinderMaterial; cylGameBoard[17, 9].transform.position = new Vector3(cylGameBoard[17, 9].transform.position.x, -1.06f, cylGameBoard[17, 9].transform.position.z);
            cylGameBoard[9, 1].GetComponent<MeshRenderer>().material = cylinderMaterial; cylGameBoard[9, 1].transform.position = new Vector3(cylGameBoard[9, 1].transform.position.x, -1.06f, cylGameBoard[9, 1].transform.position.z);
            cylGameBoard[1, 9].GetComponent<MeshRenderer>().material = cylinderMaterial; cylGameBoard[1, 9].transform.position = new Vector3(cylGameBoard[1, 9].transform.position.x, -1.06f, cylGameBoard[1, 9].transform.position.z);
            cylGameBoard[9, 17].GetComponent<MeshRenderer>().material = cylinderMaterial; cylGameBoard[9, 17].transform.position = new Vector3(cylGameBoard[9, 17].transform.position.x, -1.06f, cylGameBoard[9, 17].transform.position.z);
        }
    }
}
