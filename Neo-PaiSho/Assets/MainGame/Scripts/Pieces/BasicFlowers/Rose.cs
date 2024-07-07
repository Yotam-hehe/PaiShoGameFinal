using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : PieceScript
{
    public void Awake()
    {
        // Rose parameters
        basicTile = true;
        moveDistance = 3;
        clash = PieceType.Jasmine;
        harmony = new PieceType[2];
        harmony[0] = PieceType.WhiteJade;
        harmony[1] = PieceType.Chrysanthemum;
        color = "Red";
    }
    public override bool HarmonyCheck(GameObject gameObject)
    {
        if (gameObject != null && gameObject.GetComponent<PieceScript>() != null && gameObject.GetComponent<PieceScript>().harmony != null)
        {
            for (int i = 0; i < this.harmony.Length; i++)
            {
                if (this.harmony[i] == gameObject.GetComponent<PieceScript>().type)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
