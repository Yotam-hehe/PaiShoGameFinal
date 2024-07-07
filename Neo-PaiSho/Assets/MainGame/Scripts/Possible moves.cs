using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Possiblemoves : MonoBehaviour
//{
//    CircularBoard circular;
//    public static Possiblemoves instance;
//    // Start is called before the first frame update
//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else
//        {
//            Debug.LogWarning("Multiple instances of HelperScript found. Destroying this instance.");
//            Destroy(this);
//        }
//    }

//    public void ShowMoves(int x, int y, GameObject[,] matrix, GameObject piece, Material hov)
//    {
//        int radius = piece.GetComponent<PieceScript>().moveDistance;
//        Vector2 v = new Vector2(x, y);
//        Vector2 vector2;
//        for (int i = 0; i < 18; i++)
//        {
//            for (int j = 0; j < 18; j++)
//            {
//                vector2 = new Vector2(i, j);
//                if (CalculateDistance(v, vector2) <= radius)
//                {
//                    matrix[i, j].layer = LayerMask.NameToLayer("PosMoves");
//                    matrix[i, j].GetComponent<MeshRenderer>().material = hov;
//                    matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, matrix[i, j].transform.position.y - 0.2f, matrix[i, j].transform.position.z);
//                }


//                //if (Math.Sqrt( Math.Pow((double)x-i,2) + Math.Pow((double)y - j, 2)) <= radius)
//                //{
//                //    matrix[i,j].layer = LayerMask.NameToLayer("PosMoves");
//                //    matrix[i, j].GetComponent<MeshRenderer>().material = hov;
//                //    matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, matrix[i, j].transform.position.y+0.2f, matrix[i, j].transform.position.z);
//                //}
//            }
//        }
//    }


//    public void UnShow(int x, int y, GameObject[,] matrix,GameObject piece, Material reg)
//    {
//        Vector2 v = new Vector2(x, y);
//        Vector2 vector2;
//        int radius = piece.GetComponent<PieceScript>().moveDistance;
//        for (int i = 0; i < 18; i++)
//        {
//            for (int j = 0; j < 18; j++)
//            {
//                vector2 = new Vector2(i,j);
//                if (CalculateDistance(v, vector2) <= radius)
//                {
//                    matrix[i, j].layer = LayerMask.NameToLayer("Cyl");
//                    matrix[i, j].GetComponent<MeshRenderer>().material = reg;
//                    matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, matrix[i, j].transform.position.y - 0.2f, matrix[i, j].transform.position.z);
//                }



//                //if (Math.Sqrt(Math.Pow((double)x - i, 2) + Math.Pow((double)y - j, 2)) <= radius)
//                //{
//                //    matrix[i, j].layer = LayerMask.NameToLayer("Cyl");
//                //    matrix[i, j].GetComponent<MeshRenderer>().material = reg;
//                //    matrix[i, j].transform.position = new Vector3(matrix[i, j].transform.position.x, matrix[i, j].transform.position.y - 0.2f, matrix[i, j].transform.position.z);

//                //}
//            }
//        }
//    }

//    private float CalculateDistance(Vector2 a, Vector2 b)
//    {
//        float deltaX = b.x - a.x;
//        float deltaY = b.y - a.y;
//        float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
//        return distance;
//    }
//}
