using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject[,] positions = new GameObject[5, 5];
    private string[] allyPieces = new string[9];
    private string[] enemyPieces = new string[9];

    private int boardWidth;
    private int boardHeight;

    private Game game;
    private void Start()
    {
        boardWidth = GameObject.FindGameObjectWithTag("SipliBoard").GetComponent<BoardGenerator>().GetWidth();
        boardHeight = GameObject.FindGameObjectWithTag("SipliBoard").GetComponent<BoardGenerator>().GetHeight();

        GeneratePieces();


        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    private void Update()
    {
    }

    public void GeneratePieces()
    {
        int xMissingAlly = Random.Range(0, boardWidth); // get random number from x = 0 to 4
        int yMissingAlly = Random.Range(0, 2); // get random number from y = 0 to 1
        int xMissingEnemy = Random.Range(0, boardHeight); // get random number from x = 0 to 4
        int yMissingEnemy = Random.Range(boardHeight - 2, boardHeight); // get random number from y = 3 to 4

        System.Random rng = new System.Random();

        allyPieces = new string[]
        {
            "blu_infinity",
            "blu_xzero",
            "blu_xzero",
            "blu_zero",
            "blu_zero",
            "blu_zero",
            "blu_one",
            "blu_two",
            "blu_three",

        };

        allyPieces = allyPieces.OrderBy(piece => rng.Next()).ToArray();
        int allyIndex = 0;
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                if (x == xMissingAlly && y == yMissingAlly)
                {
                    continue;
                }

                if (allyIndex < allyPieces.Length)
                {
                    GeneratePiece(allyPieces[allyIndex], x, y, "blue");
                    allyIndex++;
                }

                if (allyIndex >= allyPieces.Length)
                {
                    break;
                }
            }
        }

        enemyPieces = new string[]
        {
            "red_infinity",
            "red_xzero",
            "red_xzero",
            "red_zero",
            "red_zero",
            "red_zero",
            "red_one",
            "red_two",
            "red_three",
        };

        enemyPieces = enemyPieces.OrderBy(piece => rng.Next()).ToArray();
        int enemyIndex = 0;

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 3; y < boardHeight; y++)
            {
                if (x == xMissingEnemy && y == yMissingEnemy)
                {
                    continue;
                }

                if (enemyIndex < enemyPieces.Length)
                {
                    GeneratePiece(enemyPieces[enemyIndex], x, y, "red");
                    enemyIndex++;
                }

                if (enemyIndex >= enemyPieces.Length)
                {
                    break;
                }
            }
        }
    }

    private GameObject GeneratePiece(string name, int x, int y, string playerColor)
    {
        GameObject obj = Instantiate(piecePrefab, new Vector3(x, y), Quaternion.identity);
        obj.name = $"Piece {x} {y}";

        Piece p = obj.GetComponent<Piece>();
        p.name = name;
        p.player = playerColor;
        p.SetXBoard(x);
        p.SetYBoard(y);
        p.Activate();

        SetPosition(obj);

        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Piece pm = obj.GetComponent<Piece>();
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
        {
            return false;
        }

        return true;
    }

    public List<GameObject> GetPiecesByPlayer(string playerColor)
    {
        List<GameObject> pieces = new List<GameObject>();

        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                GameObject piece = positions[x, y];

                if (piece != null && piece.GetComponent<Piece>().GetCurrentPlayer() == playerColor)
                {
                    pieces.Add(piece);
                }
            }
        }

        return pieces;
    }

    public GameObject GetPieceAtPosition(int x, int y)
    {
        // Retrieve the piece at the specified position
        GameObject piece = positions[x, y];

        return piece;
    }

    public List<MoveData> GetLegalMoves(string playerColor)
    {
        List<MoveData> legalMoves = new List<MoveData>();

        List<GameObject> currentPlayerPieces = GetPiecesByPlayer(playerColor);
        foreach (GameObject piece in currentPlayerPieces)
        {
            // Generate move plates for the piece
            piece.GetComponent<Piece>().InitiateMovePlates();

            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (GameObject movePlate in movePlates)
            {
                MovePlate movePlateScript = movePlate.GetComponent<MovePlate>();

                if (movePlateScript.GetReference() == piece)
                {
                    int targetX = movePlateScript.GetX();
                    int targetY = movePlateScript.GetY();

                    GameObject targetPiece = GetPieceAtPosition(targetX, targetY);
                    if (targetPiece == null || targetPiece.GetComponent<Piece>().GetCurrentPlayer() != playerColor)
                    {
                        // Create a new MoveData object for the valid move
                        MoveData moveData = new MoveData(piece, piece.GetComponent<Piece>().GetXBoard(), piece.GetComponent<Piece>().GetYBoard(), targetX, targetY);

                        legalMoves.Add(moveData);
                    }
                }
            }
        }

        return legalMoves;
    }

    public List<GameObject> GetAllPieces()
    {
        List<GameObject> allPieces = new List<GameObject>();

        // Iterate over the generated pieces and add them to the list
        foreach (Transform pieceTransform in transform)
        {
            GameObject piece = pieceTransform.gameObject;
            allPieces.Add(piece);
        }

        return allPieces;
    }



}

