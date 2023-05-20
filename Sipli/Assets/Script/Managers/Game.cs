using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject piece;

    public GameObject[,] positions = new GameObject[5, 5];
    private GameObject[] playerPlayer = new GameObject[9];
    private GameObject[] playerEnemy = new GameObject[9];

    private string[] playerPieces = new string[9];
    private string[] enemyPieces = new string[9];

    public Vector2 mouseOver;

    private string currentPlayer = "player";

    private bool gameOver = false;

    void Start()
    {
        GenerateBoard();
    }

    private void Update()
    {
        updateMouseOver();
    }

    private void updateMouseOver()
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Set the z-coordinate to 0 in 2D

            Debug.Log("Mouse Position: " + mousePosition);
        }
    }

    void GenerateBoard()
    {
        playerPieces = new string[]
        {
            "ally_infinity",

            "ally_xzero",
            "ally_xzero",

            "ally_zero",
            "ally_zero",
            "ally_zero",

            "ally_one",
            "ally_two",
            "ally_three",
        };

        //tile with missing piece

        int xMissingAlly = Random.Range(0, 4 + 1); //get random number from x = 0 to 4
        int yMissingAlly = Random.Range(0, 1 + 1); //get random number from y = 0 to 1
        int xMissingEnemy = Random.Range(0, 4 + 1); //get random number from x = 0 to 4
        int yMissingEnemy = Random.Range(3, 4 + 1); //get random number from y = 3 to 4

        System.Random rng = new System.Random();
        playerPieces = playerPieces.OrderBy(piece => rng.Next()).ToArray();

        // Generate Ally pieces in (0,0) to (4,1)
        int index = 0;
        for (int x = 0; x <= 4; x++)
        {
            for (int y = 0; y <= 1; y++)
            {
                if (x == xMissingAlly && y == yMissingAlly)
                {
                    continue;
                }

                GeneratePiece(playerPieces[index], x, y);
                index++;

                if (index >= playerPieces.Length)
                {
                    // All pieces have been placed, exit the loop
                    continue;
                }
            }
        }

        // Generate Enemy Pieces in (0,3) to (4,4)


        for (int x = 0; x <= 4; x++)
        {
            for (int y = 3; y <= 4; y++)
            {
                if (x == xMissingEnemy && y == yMissingEnemy)
                {
                    continue;
                }

                GeneratePiece("enemy_piece", x, y);
                Debug.Log("enemy piece");

            }
        }

    }

    public GameObject GeneratePiece(string name, int x, int y)
    {
        GameObject obj = Instantiate(piece, new Vector3(x, y), Quaternion.identity);
        obj.name = $"Piece {x} {y}";

        PieceManager pm = obj.GetComponent<PieceManager>();
        pm.name = name;
        pm.SetXBoard(x);
        pm.SetYBoard(y);
        pm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        PieceManager pm = obj.GetComponent<PieceManager>();
        positions[pm.GetXBoard(), pm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) 
            return false;
        return true;
    }


}