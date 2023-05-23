using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject defender = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            GameObject attacker = reference;

            string attackerName = attacker.GetComponent<PieceManager>().name;
            string defenderName = defender.name;

            // ResolveCombat(attacker, defender)

            // Same piece battle
            if (attackerName.Substring(5) == defenderName.Substring(6) || attackerName.Substring(6) == defenderName.Substring(5))
            {

                if (!(attackerName == "ally_infinity" || attackerName == "enemy_infinity")){
                    Destroy(attacker);
                }
                    Destroy(defender);
            }

            // Infinity vs Other Piece
            if (attackerName == "ally_infinity" && defenderName != "enemy_infinity")
            {
                Destroy(attacker);
                controller.GetComponent<Game>().Winner("black");
            }
            else if (attackerName == "enemy_infinity" && defenderName != "ally_infinity")
            {
                Destroy(attacker);
                controller.GetComponent<Game>().Winner("white");
            }

            // Other Piece vs Infinity
            if (attackerName != "ally_infinity" && defenderName == "enemy_infinity")
            {
                Destroy(defender);
                controller.GetComponent<Game>().Winner("white");

            }
            else if (attackerName != "enemy_infinity" && defenderName == "ally_infinity")
            {
                Destroy(defender);
                controller.GetComponent<Game>().Winner("black");

            }

            // Infinity vs Infinity
            if (attackerName == "ally_infinity" && defenderName == "enemy_infinity")
            {
                Destroy(defender);
                controller.GetComponent<Game>().Winner("white");
            }
            else if (attackerName == "enemy_infinity" && defenderName == "ally_infinity")
            {
                Destroy(defender);
                controller.GetComponent<Game>().Winner("black");
            }

            // Scout vs Ninja
            if ((attackerName == "ally_zero" && defenderName == "enemy_xzero") || (attackerName == "enemy_zero" && defenderName == "ally_xzero"))
            {
                Destroy(defender);
            }
            // Scout vs Other Piece
            else if ((attackerName == "ally_zero" && defenderName != "enemy_xzero") || (attackerName == "enemy_zero" && defenderName != "ally_xzero"))
            {
                Destroy(attacker);
            }
            // Other Piece vs Scout
            else if ((attackerName != "ally_xzero" && defenderName == "enemy_zero") || (attackerName != "enemy_xzero" && defenderName == "ally_zero"))
            {
                Destroy(defender);
            }

            // Ninja vs Scout
            if ((attackerName == "ally_xzero" && defenderName == "enemy_zero") || (attackerName == "enemy_xzero" && defenderName == "ally_zero"))
            {
                Destroy(attacker);
            }
            // Ninja vs Other Piece
            else if ((attackerName == "ally_xzero" && defenderName != "enemy_zero") || (attackerName == "enemy_xzero" && defenderName != "ally_zero"))
            {
                Destroy(defender);
            }
            // Other Piece vs Ninja
            else if ((attackerName != "ally_xzero" && defenderName == "enemy_zero") || (attackerName != "enemy_xzero" && defenderName == "ally_zero"))
            {
                Destroy(defender);
            }
        }

        //Set the Chesspiece's original location to be empty
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<PieceManager>().GetXBoard(),
        reference.GetComponent<PieceManager>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<PieceManager>().SetXBoard(matrixX);
        reference.GetComponent<PieceManager>().SetYBoard(matrixY);
        reference.GetComponent<PieceManager>().SetCoords();

        //Update the matrix
        controller.GetComponent<Game>().SetPosition(reference);

        //Switch Current Player
        controller.GetComponent<Game>().NextTurn();

        //Destroy the move plates including self
        reference.GetComponent<PieceManager>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}