using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject controller;
    public GameObject sipliBoard;
    public GameObject movePlate;
    private bool movePlatesVisible = false;

    private int xBoard = -1;
    private int yBoard = -1;

    public string player;
    private bool isHidden;

    public Sprite ally_infinity, ally_xzero, ally_zero, ally_one, ally_two, ally_three;
    public Sprite enemy_infinity, enemy_xzero, enemy_zero, enemy_one, enemy_two, enemy_three, enemy_piece;

    private int rank;
    public static Dictionary<string, int> pieceRanks;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        sipliBoard = GameObject.FindGameObjectWithTag("SipliBoard");

        SetCoords();

        switch (this.name)
        {
            case "blu_infinity": this.GetComponent<SpriteRenderer>().sprite = ally_infinity; player = "white"; break;
            case "blu_xzero": this.GetComponent<SpriteRenderer>().sprite = ally_xzero; player = "white"; break;
            case "blu_zero": this.GetComponent<SpriteRenderer>().sprite = ally_zero; player = "white"; break;
            case "blu_one": this.GetComponent<SpriteRenderer>().sprite = ally_one; player = "white"; break;
            case "blu_two": this.GetComponent<SpriteRenderer>().sprite = ally_two; player = "white"; break;
            case "blu_three": this.GetComponent<SpriteRenderer>().sprite = ally_three; player = "white"; break;

            //case "red_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            //case "red_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            //case "red_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            //case "red_one": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            //case "red_two": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;
            //case "red_three": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "black"; break;

            // uncomment if you want the pieces to be shown

            case "red_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_infinity; player = "black"; break;
            case "red_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_xzero; player = "black"; break;
            case "red_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_zero; player = "black"; break;
            case "red_one": this.GetComponent<SpriteRenderer>().sprite = enemy_one; player = "black"; break;
            case "red_two": this.GetComponent<SpriteRenderer>().sprite = enemy_two; player = "black"; break;
            case "red_three": this.GetComponent<SpriteRenderer>().sprite = enemy_three; player = "black"; break;
        }

        pieceRanks = new Dictionary<string, int>
        {
            { "blu_one", 3 },
            { "blu_two", 4 },
            { "blu_three", 5 },
            { "red_one", 3 },
            { "red_two", 4 },
            { "red_three", 5 }
        };
    }


    public int GetRank()
    {
        return rank;
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
    public string GetCurrentPlayer()
    {
        return player;
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
            case "blu_infinity":
            case "blu_xzero":
            case "blu_zero":
            case "blu_one":
            case "blu_two":
            case "blu_three":
            case "red_infinity":
            case "red_xzero":
            case "red_zero":
            case "red_one":
            case "red_two":
            case "red_three":
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
        PieceGenerator sc = sipliBoard.GetComponent<PieceGenerator>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Piece>().player != player)
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

    public void GenerateMovePlatesForAIPlayer()
{
    if (controller.GetComponent<Game>().GetCurrentPlayer() == player)
    {
        InitiateMovePlates();
        movePlatesVisible = true;
    }
}
}


