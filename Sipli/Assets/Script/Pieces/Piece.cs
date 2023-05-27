using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject controller;
    public GameObject sipliBoard;
    public GameObject movePlate;
    public GameObject arrowPrefab;

    private bool movePlatesVisible = false;

    private int xBoard = -1;
    private int yBoard = -1;
    private Vector3 previousPosition;

    public string player;
    private bool isActive = true; // if buhay pa
    public bool isHidden = false;

    public Sprite ally_infinity, ally_xzero, ally_zero, ally_one, ally_two, ally_three;
    public Sprite enemy_infinity, enemy_xzero, enemy_zero, enemy_one, enemy_two, enemy_three; //enemy_piece

    private int rank;
    public static Dictionary<string, int> pieceRanks;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        sipliBoard = GameObject.FindGameObjectWithTag("SipliBoard");

        SetCoords();

        switch (this.name)
        {
            case "blu_infinity": this.GetComponent<SpriteRenderer>().sprite = ally_infinity; player = "blue"; break;
            case "blu_xzero": this.GetComponent<SpriteRenderer>().sprite = ally_xzero; player = "blue"; break;
            case "blu_zero": this.GetComponent<SpriteRenderer>().sprite = ally_zero; player = "blue"; break;
            case "blu_one": this.GetComponent<SpriteRenderer>().sprite = ally_one; player = "blue"; break;
            case "blu_two": this.GetComponent<SpriteRenderer>().sprite = ally_two; player = "blue"; break;
            case "blu_three": this.GetComponent<SpriteRenderer>().sprite = ally_three; player = "blue"; break;
            case "red_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_infinity; player = "red"; break;
            case "red_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_xzero; player = "red"; break;
            case "red_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_zero; player = "red"; break;
            case "red_one": this.GetComponent<SpriteRenderer>().sprite = enemy_one; player = "red"; break;
            case "red_two": this.GetComponent<SpriteRenderer>().sprite = enemy_two; player = "red"; break;
            case "red_three": this.GetComponent<SpriteRenderer>().sprite = enemy_three; player = "red"; break;
                //case "red_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
                //case "red_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
                //case "red_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
                //case "red_one": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
                //case "red_two": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
                //case "red_three": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
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

    private void Update()
    {
        isActive = controller.GetComponent<Game>().GetCurrentPlayer() == player;

        //HidePiece(isHidden, "red");

    }

    // Method to deactivate the piece
    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    // Method to reactivate the piece
    public void Reactivate()
    {
        isActive = true;
        gameObject.SetActive(true);
    }
    public bool IsActive()
    {
        return isActive;
    }

    public string GetName()
    {
        return this.name;
    }

    public void HidePiece(bool isHidden, string player)
    {
        if (isHidden)
        {
            switch (this.name)
            {
                case "red_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_infinity; player = "red"; break;
                case "red_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_xzero; player = "red"; break;
                case "red_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_zero; player = "red"; break;
                case "red_one": this.GetComponent<SpriteRenderer>().sprite = enemy_one; player = "red"; break;
                case "red_two": this.GetComponent<SpriteRenderer>().sprite = enemy_two; player = "red"; break;
                case "red_three": this.GetComponent<SpriteRenderer>().sprite = enemy_three; player = "red"; break;
            }
        } 
        else
        {
            //switch (this.name)
            //{
            //    case "red_infinity": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
            //    case "red_xzero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
            //    case "red_zero": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
            //    case "red_one": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
            //    case "red_two": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
            //    case "red_three": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; player = "red"; break;
            //}
        }
    }

    public bool IsHidden()
    {
        return isHidden;
    }

    public void SetIsHidden()
    {
        isHidden = (isHidden == false) ? true : false;
    }

    public int GetRank()
    {
        return rank;
    }

    public string GetPlayer()
    {
        return player;
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

    private void OnMouseDown()
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

    public bool InitiateMovePlates()
    {
        if (!isActive)
        {
            return false;
        }

        // Logic to initiate move plates...

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

        return true; // Move plates successfully initiated

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

    public void MoveTo(int x, int y)
    {

        // Store the previous position before moving
        previousPosition = transform.position;


        // Check if the target position is valid
        if (!sipliBoard.GetComponent<PieceGenerator>().PositionOnBoard(x, y))
        {
            Debug.Log("Invalid move: Target position is outside the board.");
            return;
        }

        GameObject targetPiece = sipliBoard.GetComponent<PieceGenerator>().GetPosition(x, y);

        // Check if the target position is occupied by an ally piece
        if (targetPiece != null && targetPiece.GetComponent<Piece>().GetPlayer() == player)
        {
            Debug.Log("Invalid move: Target position is occupied by an ally piece.");
            return;
        }

        // Set the Chesspiece's original location to be empty
        sipliBoard.GetComponent<PieceGenerator>().SetPositionEmpty(GetXBoard(), GetYBoard());

        // Move the piece to the new position
        SetXBoard(x);
        SetYBoard(y);
        SetCoords();

        // Update the matrix
        sipliBoard.GetComponent<PieceGenerator>().SetPosition(gameObject);

        // Switch to the next player's turn
        controller.GetComponent<Game>().NextTurn();

        // Destroy the move plates
        DestroyMovePlates();
    }

    public void GenerateMovementArrow()
    {

        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");

        foreach(GameObject i in arrows)
        {
            Destroy(i);
        }

        // Instantiate the arrow object or sprite at the previous position
        GameObject arrow = Instantiate(arrowPrefab, previousPosition, Quaternion.identity);

        // Determine the direction of movement
        Vector3 movementDirection = transform.position - previousPosition;

        // Rotate the arrow object to match the movement direction
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // Subtract 90 degrees offset
    }


}


