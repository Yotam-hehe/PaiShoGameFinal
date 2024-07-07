using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLotus : PieceScript 
{
    public void Awake()
    {
        //WhiteLotus parameters
        moveDistance = 2;
        harmony = new PieceType[6];
        harmony[0] = PieceType.Chrysanthemum;
        harmony[1] = PieceType.Jasmine;
        harmony[2] = PieceType.Lily;
        harmony[3] = PieceType.Rhododendron;
        harmony[4] = PieceType.Rose;
        harmony[5] = PieceType.WhiteJade;
    }

    public override bool HarmonyCheck(GameObject gameObject)
    {
        for (int i = 0; i < this.harmony.Length; i++)
        {
            if (this.harmony[i] == gameObject.GetComponent<PieceScript>().type)
            {
                    return true;
            }
        }
        return false;
    }
}
