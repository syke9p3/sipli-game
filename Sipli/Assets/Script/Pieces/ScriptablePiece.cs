using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Piece", menuName = "Scriptable Piece")]

public class ScriptablePiece : ScriptableObject
{
    public Faction Faction;
    public BasePiece PiecePrefab;

}

public enum Faction
{
    Player = 0,
    Enemy = 1
}
