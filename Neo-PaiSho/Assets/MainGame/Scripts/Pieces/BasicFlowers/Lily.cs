using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lily : PieceScript
{
    public void Awake()
    {
        //Lily parameters
        basicTile = true;
        moveDistance = 4;
        clash = PieceType.Chrysanthemum;
        harmony = new PieceType[2];
        harmony[0] = PieceType.Jasmine;
        harmony[1] = PieceType.WhiteJade;
        color = "White";
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
