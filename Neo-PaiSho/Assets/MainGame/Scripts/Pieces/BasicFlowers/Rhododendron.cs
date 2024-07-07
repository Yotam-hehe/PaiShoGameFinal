using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhododendron : PieceScript
{
    public void Awake()
    {
        //Rhododendron parameters
        basicTile = true;
        moveDistance = 5;
        clash = PieceType.WhiteJade;
        harmony = new PieceType[2];
        harmony[0] = PieceType.Chrysanthemum;
        harmony[1] = PieceType.Jasmine;
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
