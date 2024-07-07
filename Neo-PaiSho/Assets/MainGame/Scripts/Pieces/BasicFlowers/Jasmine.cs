using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jasmine : PieceScript
{
    public void Awake()
    {
        // Jasmine parameters
        basicTile = true;
        moveDistance = 3;
        clash = PieceType.Rose;
        harmony = new PieceType[2];
        harmony[0] = PieceType.Rhododendron;
        harmony[1] = PieceType.Lily;
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
