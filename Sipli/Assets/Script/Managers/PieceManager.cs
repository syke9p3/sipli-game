using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private string player;

    public Sprite ally_infinity, ally_xzero, ally_zero, ally_one, ally_two, ally_three;
    public Sprite enemy_piece;

    private Vector3 offset;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Take the instantiated location and adjust the transform
        //SetCoords();

        switch (this.name)
        {
            case "ally_infinity": this.GetComponent<SpriteRenderer>().sprite = ally_infinity; break;
            case "ally_xzero": this.GetComponent<SpriteRenderer>().sprite = ally_xzero; break;
            case "ally_zero": this.GetComponent<SpriteRenderer>().sprite = ally_zero; break;
            case "ally_one": this.GetComponent<SpriteRenderer>().sprite = ally_one; break;
            case "ally_two": this.GetComponent<SpriteRenderer>().sprite = ally_two; break;
            case "ally_three": this.GetComponent<SpriteRenderer>().sprite = ally_three; break;
            case "enemy_piece": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();
        InitiateMovePlates();
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "ally_infinity":
            case "ally_xzero":
            case "ally_zero":
            case "ally_one":
            case "ally_two":
            case "ally_three":
            case "enemy_piece":
                SurroundMovePlate();
                break;
        }
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject piece = sc.GetPosition(x, y);
            
            if(piece != null)
            {
                MovePlateSpawn(x, y);
            } else if (piece.GetComponent<PieceManager>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        GameObject mp = Instantiate(movePlate, new Vector3(matrixX, matrixY), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        GameObject mp = Instantiate(movePlate, new Vector3(matrixX, matrixY), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

}
