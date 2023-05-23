using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;
    private bool movePlatesVisible = false;

    private int xBoard = -1;
    private int yBoard = -1;

    private string player;

    public Sprite ally_infinity, ally_xzero, ally_zero, ally_one, ally_two, ally_three;
    public Sprite enemy_piece, enemy_infinity, enemy_xzero, enemy_one, enemy_two, enemy_three;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "ally_infinity": this.GetComponent<SpriteRenderer>().sprite = ally_infinity; player = "white"; break;
            case "ally_xzero": this.GetComponent<SpriteRenderer>().sprite = ally_xzero; player = "white"; break;
            case "ally_zero": this.GetComponent<SpriteRenderer>().sprite = ally_zero; player = "white"; break;
            case "ally_one": this.GetComponent<SpriteRenderer>().sprite = ally_one; player = "white"; break;
            case "ally_two": this.GetComponent<SpriteRenderer>().sprite = ally_two; player = "white"; break;
            case "ally_three": this.GetComponent<SpriteRenderer>().sprite = ally_three; player = "white"; break;
            case "enemy_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            case "enemy_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            case "enemy_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            case "enemy_one": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            case "enemy_two": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            case "enemy_three": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
        }
    }

    public void SetCoords()
    {
        //Get the board value in order to convert to xy coords
        float x = xBoard;
        float y = yBoard;

        //Set actual unity values
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
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            if (movePlatesVisible)
            {
                // Move plates are already visible, so destroy them
                DestroyMovePlates();
                movePlatesVisible = false;
            }
            else
            {
                // Remove all move plates relating to previously selected piece
                DestroyMovePlates();

                // Create new move plates
                InitiateMovePlates();
                movePlatesVisible = true;
            }
        }
    }

    public void DestroyMovePlates()
    {
        //Destroy old MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); //Be careful with this function "Destroy" it is asynchronous
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
            case "enemy_infinity":
            case "enemy_xzero":
            case "enemy_zero":
            case "enemy_one":
            case "enemy_two":
            case "enemy_three":
                SurroundMovePlate();
                break;
        }
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard + 0);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            Debug.Log("x: " + x + " y: " + y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<PieceManager>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
            else
            {
                // Tile is occupied by allied piece, do not generate move plate
            }
        }
    }


    public void MovePlateSpawn(int matrixX, int matrixY)
    {

        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

}
