using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteJade : PieceScript
{
    public void Awake()
    {
        //WhiteJade parameters
        basicTile = true;
        moveDistance = 5;
        type = PieceType.WhiteJade;
        clash = PieceType.Rhododendron;
        harmony = new PieceType[2];
        harmony[0] = PieceType.Lily;
        harmony[1] = PieceType.Rose;
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
