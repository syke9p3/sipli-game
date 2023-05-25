using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject controller;

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
                Destroy(attacker);
            }
            Destroy(defender);
        }
        // Infinity vs Other Piece
        else if (attackerName == "blu_infinity" && defenderName != "red_infinity")
        {
            Destroy(attacker);
            controller.GetComponent<Game>().SetPlayerWinner("red");
        }
        else if (attackerName == "red_infinity" && defenderName != "blu_infinity")
        {
            Destroy(attacker);
            controller.GetComponent<Game>().SetPlayerWinner("blue");
        }
        // Other Piece vs Infinity
        else if (attackerName != "blu_infinity" && defenderName == "red_infinity")
        {
            Destroy(defender);
            controller.GetComponent<Game>().SetPlayerWinner("blue");
        }
        else if (attackerName != "red_infinity" && defenderName == "blu_infinity")
        {
            Destroy(defender);
            controller.GetComponent<Game>().SetPlayerWinner("red");
        }
        // Infinity vs Infinity
        else if (attackerName == "blu_infinity" && defenderName == "red_infinity")
        {
            Destroy(defender);
            controller.GetComponent<Game>().SetPlayerWinner("blue");
        }
        else if (attackerName == "red_infinity" && defenderName == "blu_infinity")
        {
            Destroy(defender);
            controller.GetComponent<Game>().SetPlayerWinner("red");
        }
        // Scout vs Ninja
        else if ((attackerName.Substring(4) == "zero" && defenderName.Substring(4) == "xzero"))
        {
            Destroy(defender);
        }
        // Scout vs Other Piece
        else if ((attackerName.Substring(4) == "zero" && defenderName.Substring(4) != "xzero"))
        {
            Destroy(attacker);
        }
        // Other Piece vs Scout
        else if ((attackerName.Substring(4) != "xzero" && defenderName.Substring(4) == "zero"))
        {
            Destroy(defender);

        }
        // Ninja vs Scout
        else if ((attackerName.Substring(4) == "xzero" && defenderName.Substring(4) == "zero"))
        {
            Destroy(attacker);

        }
        // Ninja vs Other Piece
        else if ((attackerName.Substring(4) == "xzero" && defenderName.Substring(4) != "zero"))
        {
            Destroy(defender);

        }
        // Other Piece vs Ninja
        else if ((attackerName.Substring(4) != "zero" && defenderName.Substring(4) == "xzero"))
        {
            Destroy(attacker);

        }
        // Compare ranks of non-special pieces
        else if (Piece.pieceRanks.ContainsKey(attackerName) && Piece.pieceRanks.ContainsKey(defenderName))
        {
            int attackerRank = Piece.pieceRanks[attackerName];
            int defenderRank = Piece.pieceRanks[defenderName];

            if (attackerRank == defenderRank)
            {
                // Same rank, destroy both pieces
                Destroy(attacker);
                Destroy(defender);

            }
            else if (attackerRank > defenderRank)
            {
                // Attacker wins, destroy defender
                Destroy(defender);

            }
            else
            {
                // Defender wins, destroy attacker
                Destroy(attacker);

            }
        }
    }
}



