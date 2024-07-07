using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    //art
    [Header("Art")]
    [SerializeField] private Material tileMat;

    [Header("Prefabs & Mats")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMats;

    //Game
    private PieceScript[,] PiecesOnBoard; // saves the board live basically
    private const int tileCountX = 18;
    private const int tileCountY = 18;
    private GameObject[,] tiles;
    private Camera currentCam;
    private Vector2Int hover = -Vector2Int.one;
    private Vector3 v3 = new Vector3((float)8.5, 0, (float)8.5);  

    private void Awake()
    {
        GenerateBoard(1,tileCountX,tileCountY);
        //SpawningSingle(PieceType.Lily, 0);
    }
    //private void Update()
    //{
    //    //mouse drag
    //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


    //    //highlighting tiles
    //    if (!currentCam)
    //    {
    //        currentCam = Camera.main;
    //        return;
    //    }
    //    RaycastHit hit;
    //    Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);
    //    if(Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Tile","Hover"))) 
    //    {

    //        //get index of tile being hit
    //        Vector2Int pos = TileIndex(hit.transform.gameObject);

    //        //if hovering after not hovering over tile
    //        if(hover == -Vector2Int.one)
    //        {
    //            hover = pos;
    //            tiles[pos.x, pos.y].layer = LayerMask.NameToLayer("Hover");
    //        }

    //        //if hovering after hovering over another tile
    //        if (hover != pos)
    //        {
    //            tiles[hover.x, hover.y].layer = LayerMask.NameToLayer("Tile");
    //            hover = pos;
    //            tiles[pos.x, pos.y].layer = LayerMask.NameToLayer("Hover");
    //        }
    //    }
    //    else
    //    {
    //        if(hover != -Vector2Int.one)
    //        {
    //            //int retTile = LayerMask.NameToLayer("Tile");
    //            tiles[hover.x, hover.y].layer = LayerMask.NameToLayer("Tile");
    //            hover = -Vector2Int.one;
    //        }
    //    }
    //}

    //Add For Update
    private Vector2Int TileIndex(GameObject hit)
    {
        for(int i = 0; i < tileCountX; i++)
        {
            for(int j = 0; j < tileCountY; j++)
            {
                if (tiles[i, j] == hit)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        // Invalid
        return -Vector2Int.one;
    }

    //Board Generation
    private void GenerateBoard(float tileSize, int tileCountX, int tileCountY)
    {
        tiles = new GameObject[tileCountX, tileCountY];
        for(int i = 0; i < tileCountX; i++)
        {
            for(int j = 0; j < tileCountY; j++)
            {
                Vector3 vcheck = new Vector3(i+(1/2), 0, j+(1/2));
                if (Vector3.Distance(vcheck, v3) <= 9)
                {
                    tiles[i, j] = GenerateTile(tileSize, i, j);
                }
            }
        }
    }
    private GameObject GenerateTile(float tileSize, int x, int y)
    {
        GameObject tile = new GameObject(string.Format("X:{0}, Y:{1}",x,y));
        tile.transform.parent = transform;
        Mesh mesh = new Mesh();
        tile.AddComponent<MeshFilter>().mesh = mesh;
        tile.AddComponent<MeshRenderer>().material = tileMat;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x*tileSize,0,y*tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y+1) * tileSize);
        vertices[2] = new Vector3((x+1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, 0, (y + 1) * tileSize);

        int[] tris = new int[] {0,1,2,1,3,2};
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        tile.AddComponent<BoxCollider>();
        tile.layer = LayerMask.NameToLayer("Tile");
        return tile;
    }

    // Spawning Pieces
    //private PieceScript SpawningSingle(PieceType pieceType, int team)
    //{
    //    PieceScript piece = Instantiate(prefabs[(int)pieceType-1], transform).GetComponent<PieceScript>();

    //    piece.AddComponent<DragObject>();
    //    piece.AddComponent<BoxCollider>().enabled = true;
        
    //    piece.type = pieceType;
    //    piece.team = team;
        
    //    return piece;
    //}
}
