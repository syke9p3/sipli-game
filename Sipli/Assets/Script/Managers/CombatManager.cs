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
                attacker.GetComponent<Piece>().Deactivate();
                Debug.Log("Same piece battle and not infinity");
            }

            defender.GetComponent<Piece>().Deactivate();
        }

        // Infinity vs Other Piece
        if (attackerName == "blu_infinity" && defenderName != "red_infinity")
        {
            Debug.Log(attackerName.Substring(4));
            attacker.GetComponent<Piece>().Deactivate();
            controller.GetComponent<Game>().Winner("red");
        }
        else if (attackerName == "red_infinity" && defenderName != "blu_infinity")
        {
            attacker.GetComponent<Piece>().Deactivate();
            controller.GetComponent<Game>().Winner("blue");
        }

        // Other Piece vs Infinity
        else if (attackerName != "blu_infinity" && defenderName == "red_infinity")
        {
            defender.GetComponent<Piece>().Deactivate();
            controller.GetComponent<Game>().Winner("blue");

        }
        else if (attackerName != "red_infinity" && defenderName == "blu_infinity")
        {
            defender.GetComponent<Piece>().Deactivate();
            controller.GetComponent<Game>().Winner("red");

        }

        // Infinity vs Infinity
        else if (attackerName == "blu_infinity" && defenderName == "red_infinity")
        {
            defender.GetComponent<Piece>().Deactivate();
            controller.GetComponent<Game>().Winner("blue");
        }
        else if (attackerName == "red_infinity" && defenderName == "blu_infinity")
        {
            defender.GetComponent<Piece>().Deactivate();
            controller.GetComponent<Game>().Winner("red");
        }

        // Scout vs Ninja
        else if ((attackerName.Substring(4) == "zero" && defenderName.Substring(4) == "xzero"))
        {
            defender.GetComponent<Piece>().Deactivate();
        }
        // Scout vs Other Piece
        else if ((attackerName.Substring(4) == "zero" && defenderName.Substring(4) != "xzero"))
        {
            attacker.GetComponent<Piece>().Deactivate();
        }
        // Other Piece vs Scout
        else if ((attackerName.Substring(4) != "xzero" && defenderName.Substring(4) == "zero"))
        {
            defender.GetComponent<Piece>().Deactivate();
        }

        // Ninja vs Scout
        else if ((attackerName.Substring(4) == "xzero" && defenderName.Substring(4) == "zero"))
        {
            attacker.GetComponent<Piece>().Deactivate();
        }
        // Ninja vs Other Piece
        else if ((attackerName.Substring(4) == "xzero" && defenderName.Substring(4) != "zero"))
        {
            defender.GetComponent<Piece>().Deactivate();
        }
        // Other Piece vs Ninja
        else if ((attackerName.Substring(4) != "zero" && defenderName.Substring(4) == "xzero"))
        {
            attacker.GetComponent<Piece>().Deactivate();
        }
        else if (attackerName.Substring(4) != "infinity" && attackerName.Substring(4) != "xzero" && attackerName.Substring(4) != "zero" &&
          defenderName.Substring(4) != "infinity" && defenderName.Substring(4) != "xzero" && defenderName.Substring(4) != "zero")
        {

            int attackerRank = attacker.GetComponent<Piece>().pieceRanks[attackerName];
            int defenderRank = defender.GetComponent<Piece>().pieceRanks[defenderName];

            if (attackerRank == defenderRank)
            {
                // Same rank, destroy both pieces
                attacker.GetComponent<Piece>().Deactivate();
                defender.GetComponent<Piece>().Deactivate();
            }
            else if (attackerRank > defenderRank)
            {
                // Attacker wins, destroy defender
                defender.GetComponent<Piece>().Deactivate();
            }
            else
            {
                // Defender wins, destroy attacker
                attacker.GetComponent<Piece>().Deactivate();
            }
        }
        else
        {
        }


    }
}


