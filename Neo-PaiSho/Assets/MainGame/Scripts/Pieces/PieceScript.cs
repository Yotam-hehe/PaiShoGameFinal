using System;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum PieceType
{
    none = 0,

    //Basic Tiles
    Rose = 1,
    Chrysanthemum = 2,
    Rhododendron = 3,
    Jasmine = 4,
    Lily = 5,
    WhiteJade = 6,

    //Accent Tiles
    Rock = 7,
    Wheel = 8,
    Knotweed = 9,
    Boat = 10,

    //Special Tiles
    WhiteLotus = 11,
    Orchid = 12,
}
public class PieceScript : MonoBehaviour
{
    public int moveDistance; // The maximum distance the piece can inherently move
    public float x; // Cords
    public float y; // Cords
    public int team; //0 is white, 1 is red
    public PieceType type; // What Piece Is it
    public PieceType clash; // Basic flower tiles have pieces they clash with (meaning they can eat them and be eaten by them)
    public PieceType[] harmony; // The pieces that the current piece harmonizes with (when they are on the same x/y cord there is a line that will light up between them)
    public string color; // Basic flower tiles have a color either red or white or Neutral
    public bool basicTile;

    public void SetXnY(float x,float y)
    {
        this.x = x;
        this.y = y;
    }
    
    public virtual bool HarmonyCheck(GameObject gameObject)
    {
        if (gameObject != null && gameObject.GetComponent<PieceScript>() != null && this.harmony!=null)
        {
            for(int i = 0;i< harmony.Length;i++)
            {
                if (this.harmony[i]== gameObject.GetComponent<PieceScript>().type)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
