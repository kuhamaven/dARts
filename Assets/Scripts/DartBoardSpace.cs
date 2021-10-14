using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DartBoardSpace : MonoBehaviour
{
    public int score;

    public enum RingType
    {
        Single,
        Double,
        Triple,
        Bull,
        BullsEye
    }

    public RingType type;

    public void ResolveScore(Dart dart)
    {
        int multiplier;
        switch (type)
        {
            case RingType.Double:
                multiplier = 2;
                break;
            case RingType.Triple:
                multiplier = 3;
                break;
            default:
                multiplier = 1;
                break;
        }

        int scoreToAdd = score * multiplier;

        GameObject.FindGameObjectWithTag("Score Controller").GetComponent<ScoreController>().AddScore(scoreToAdd, dart);

        //if Bull/Bull's Eye we could play SFX here.
    }
}