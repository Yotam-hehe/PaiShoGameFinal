using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchid : PieceScript
{
    public PieceType[] clash;
    public void Awake()
    {
        clash = new PieceType[8];

        //Orchid parameters
        moveDistance = 6;
        clash[0] = PieceType.Rose;
        clash[1] = PieceType.Chrysanthemum;
        clash[2] = PieceType.Rhododendron;
        clash[3] = PieceType.Jasmine;
        clash[4] = PieceType.Lily;
        clash[5] = PieceType.WhiteJade;
        clash[6] = PieceType.WhiteLotus;
    }

    public bool IsClashing(GameObject gameObject)
    {
        if (gameObject != null && gameObject.GetComponent<PieceScript>() != null)
        {
            PieceType t = gameObject.GetComponent<PieceScript>().type;
            for (int i =0; i < clash.Length; i++)
            {
                if (clash[i] == t)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool HarmonyCheck(GameObject gameObject)
    {
        return false;
    }
}
