using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chrysanthemum : PieceScript
{
    public void Awake()
    {
        //Chrysanthemum parameters
        basicTile = true;
        moveDistance = 4;
        clash = PieceType.Lily;
        harmony = new PieceType[2];
        harmony[0] = PieceType.Rose;
        harmony[1] = PieceType.Rhododendron;
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
