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

        // When two pieces collide, resolve combat
        if (attack)
        {
            GameObject attacker = reference;
            GameObject defender = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            controller.GetComponent<CombatManager>().ResolveCombat(attacker, defender);

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