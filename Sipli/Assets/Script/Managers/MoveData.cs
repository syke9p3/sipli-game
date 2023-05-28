using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData
{
    public GameObject piece;
    public int startX;
    public int startY;
    public int targetX;
    public int targetY;
    public bool hasCombat;
    public GameObject attackedPiece;

    public MoveData(GameObject piece, int startX, int startY, int targetX, int targetY)
    {
        this.piece = piece;
        this.startX = startX;
        this.startY = startY;
        this.targetX = targetX;
        this.targetY = targetY;
        this.hasCombat = false;
        this.attackedPiece = null;
    }

    public void SetCombatData(bool hasCombat, GameObject attackedPiece)
    {
        this.hasCombat = hasCombat;
        this.attackedPiece = attackedPiece;
    }

    public GameObject GetPiece()
    {
        return piece;
    }

    public int GetStartX()
    {
        return startX;
    }

    public int GetStartY()
    {
        return startY;
    }

    public int GetTargetX()
    {
        return targetX;
    }

    public int GetTargetY()
    {
        return targetY;
    }

    public bool HasCombat()
    {
        return hasCombat;
    }

    public GameObject GetAttackedPiece()
    {
        return attackedPiece;
    }
}