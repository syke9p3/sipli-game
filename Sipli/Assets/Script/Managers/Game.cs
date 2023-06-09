using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject piece;

    public GameObject[,] positions = new GameObject[5, 5];
    private GameObject[] playerPlayer = new GameObject[9];
    private GameObject[] playerEnemy = new GameObject[9];

    private string[] allyPieces = new string[9];
    private string[] enemyPieces = new string[9];

    public Vector2 mouseOver;

    private string currentPlayer = "white";

    private bool gameOver = false;

    public GameObject playerTurnUI;

    void Start()
    {
        GenerateBoard();
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

            //Debug.Log("Mouse Position: " + mousePosition);
        }
    }

    void GenerateBoard()
    {
   

        //tile with missing piece

        int xMissingAlly = Random.Range(0, 4 + 1); //get random number from x = 0 to 4
        int yMissingAlly = Random.Range(0, 1 + 1); //get random number from y = 0 to 1
        int xMissingEnemy = Random.Range(0, 4 + 1); //get random number from x = 0 to 4
        int yMissingEnemy = Random.Range(3, 4 + 1); //get random number from y = 3 to 4

        System.Random rng = new System.Random();

        // reorder pieces order in the array

        allyPieces = new string[]
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

        // Generate Ally pieces in (0,0) to (4,1)
        allyPieces = allyPieces.OrderBy(piece => rng.Next()).ToArray();
        int allyIndex = 0;
        for (int x = 0; x <= 4; x++)
        {
            for (int y = 0; y <= 1; y++)
            {
                if (x == xMissingAlly && y == yMissingAlly)
                {
                    continue;
                }

                GeneratePiece(allyPieces[allyIndex], x, y);
                allyIndex++;

                if (allyIndex >= allyPieces.Length)
                {
                    // All pieces have been placed, exit the loop
                    continue;
                }
            }
        }

        // Generate Enemy Pieces in (0,3) to (4,4)

        enemyPieces = new string[]
      {
            "enemy_infinity",

            "enemy_xzero",
            "enemy_xzero",

            "enemy_zero",
            "enemy_zero",
            "enemy_zero",

            "enemy_one",
            "enemy_two",
            "enemy_three",
      };
        enemyPieces = enemyPieces.OrderBy(piece => rng.Next()).ToArray();
        int enemyIndex = 0;

        for (int x = 0; x <= 4; x++)
        {
            for (int y = 3; y <= 4; y++)
            {
                if (x == xMissingEnemy && y == yMissingEnemy)
                {
                    continue;
                }

                GeneratePiece(enemyPieces[enemyIndex], x, y);
                enemyIndex++;

                if (enemyIndex >= enemyPieces.Length)
                {
                    // All pieces have been placed, exit the loop
                    continue;
                }

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
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        //playerTurnUI = GameObject.FindGameObjectWithTag("PlayerTurnUI");

        //if (this.GetCurrentPlayer() == "white")
        //{
        //    playerTurnUI.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        //} else
        //{
        //    playerTurnUI.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        //}
        

        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
        }
    }

    //public void Winner(string playerWinner)
    //{
    //   gameOver = true;

    //   GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
    //   GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

    //   GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    //}

}