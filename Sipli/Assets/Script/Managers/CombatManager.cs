using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject controller;

    private GameObject destroyedPiece;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    public void ResolveCombat(GameObject attacker, GameObject defender)
    {
        // Perform combat resolution here based on your game's rules
        // You can access the attacker and defender GameObjects to determine their properties and strengths


        string attackerName = attacker.GetComponent<Piece>().name;
        string defenderName = defender.GetComponent<Piece>().name;

        Debug.Log(attackerName + " vs. " + defenderName);

        // Same piece battle
        if (attackerName.Substring(4) == defenderName.Substring(4))
        {
            // Destroy both pieces unless it's an attacker infinity
            if (!(attackerName.Substring(4) == "infinity"))
            {
                destroyedPiece = attacker;
            }
            destroyedPiece = defender;
        }

        // Infinity vs Other Piece
        if (attackerName == "blu_infinity" && defenderName != "red_infinity")
        {
            destroyedPiece = attacker;
            controller.GetComponent<Game>().Winner("red");
        }
        else if (attackerName == "red_infinity" && defenderName != "blu_infinity")
        {
            destroyedPiece = attacker;
            controller.GetComponent<Game>().Winner("blue");
        }

        // Other Piece vs Infinity
        if (attackerName != "blu_infinity" && defenderName == "red_infinity")
        {
            destroyedPiece = defender;
            controller.GetComponent<Game>().Winner("blue");

        }
        else if (attackerName != "red_infinity" && defenderName == "blu_infinity")
        {
            destroyedPiece = defender;
            controller.GetComponent<Game>().Winner("red");

        }

        // Infinity vs Infinity
        if (attackerName == "blu_infinity" && defenderName == "red_infinity")
        {
            destroyedPiece = defender;
            controller.GetComponent<Game>().Winner("blue");
        }
        else if (attackerName == "red_infinity" && defenderName == "blu_infinity")
        {
            destroyedPiece = defender;
            controller.GetComponent<Game>().Winner("red");
        }

        // Scout vs Ninja
        if ((attackerName.Substring(4) == "zero" && defenderName.Substring(4) == "xzero"))
        {
            destroyedPiece = defender;
        }
        // Scout vs Other Piece
        else if ((attackerName.Substring(4) == "zero" && defenderName.Substring(4) != "xzero"))
        {
            destroyedPiece = attacker;
        }
        // Other Piece vs Scout
        else if ((attackerName.Substring(4) != "xzero" && defenderName.Substring(4) == "zero"))
        {
            destroyedPiece = defender;
        }

        // Ninja vs Scout
        if ((attackerName.Substring(4) == "xzero" && defenderName.Substring(4) == "zero"))
        {
            destroyedPiece = attacker;
        }
        // Ninja vs Other Piece
        else if ((attackerName.Substring(4) == "xzero" && defenderName.Substring(4) != "zero"))
        {
            destroyedPiece = defender;
        }
        // Other Piece vs Ninja
        else if ((attackerName.Substring(4) != "zero" && defenderName.Substring(4) == "xzero"))
        {
            destroyedPiece = attacker;
        }


        if (attackerName.Substring(4) != "infinity" && attackerName.Substring(4) != "xzero" && attackerName.Substring(4) != "zero" &&
            defenderName.Substring(4) != "infinity" && defenderName.Substring(4) != "xzero" && defenderName.Substring(4) != "zero")
        {

            int attackerRank = attacker.GetComponent<Piece>().pieceRanks[attackerName];
            int defenderRank = defender.GetComponent<Piece>().pieceRanks[defenderName];

            if (attackerRank == defenderRank)
            {
                // Same rank, destroy both pieces
                destroyedPiece = attacker;
                destroyedPiece = defender;
            }
            else if (attackerRank > defenderRank)
            {
                // Attacker wins, destroy defender
                destroyedPiece = defender;
            }
            else
            {
                // Defender wins, destroy attacker
                destroyedPiece = attacker;
            }
        }
        else
        {
        }

        destroyedPiece.GetComponent<Piece>().Deactivate();


    }
}


