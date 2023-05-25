using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;
    public GameObject sipliBoard;

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
        sipliBoard = GameObject.FindGameObjectWithTag("SipliBoard");

        // When two pieces collide, resolve combat
        if (attack)
        {
            GameObject attacker = reference;
            GameObject defender = sipliBoard.GetComponent<PieceGenerator>().GetPosition(matrixX, matrixY);

            controller.GetComponent<CombatManager>().ResolveCombat(attacker, defender);

        }

        //Set the Chesspiece's original location to be empty
        sipliBoard.GetComponent<PieceGenerator>().SetPositionEmpty(reference.GetComponent<Piece>().GetXBoard(),
        reference.GetComponent<Piece>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<Piece>().SetXBoard(matrixX);
        reference.GetComponent<Piece>().SetYBoard(matrixY);
        reference.GetComponent<Piece>().SetCoords();

        //Update the matrix
        sipliBoard.GetComponent<PieceGenerator>().SetPosition(reference);

        //Switch Current Player
        controller.GetComponent<Game>().NextTurn();

        //Destroy the move plates including self
        reference.GetComponent<Piece>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public int GetX()
    {
        return matrixX;
    }

    public int GetY()
    {
        return matrixY;
    }

    public Vector2 GetCoords()
    {
        return new Vector2(matrixX, matrixY);
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