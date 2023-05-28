using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    public List<GameObject> pieces{ get; set; }
    public string currentPlayer { get; set; }
    public int totalVisits { get; set; }

    public GameObject[,] positions = new GameObject[5, 5];

    private Dictionary<string, int> pieceRanks = new Dictionary<string, int>
        {
            { "blu_infinity", 100 },
            { "blu_xzero", 50 },
            { "blu_three", 40 },
            { "blu_two", 30 },
            { "blu_one", 20 },
            { "blu_zero", 10 },
            { "red_infinity", -100 },
            { "red_xzero", -50 },
            { "red_three", -40 },
            { "red_two", -30 },
            { "red_one", -20 },
            { "red_zero", -10 },

        };

    public GameState Clone()
    {
        Debug.Log("Monte Carlo AI: Cloning Game State");
        // Create a new instance of the GameState class
        GameState clonedGameState = new GameState();

        // Copy the values of the member variables
        clonedGameState.currentPlayer = currentPlayer;
        clonedGameState.totalVisits = totalVisits;
        clonedGameState.pieces = new List<GameObject>(pieces);

        clonedGameState.positions = (GameObject[,])positions.Clone();

        int width = clonedGameState.positions.GetLength(0);
        int height = clonedGameState.positions.GetLength(1);

        //clonedGameState.PrintCurrentGameState();
       
        return clonedGameState;
    }

    public void PrintCurrentGameState()
    {
        Debug.Log("=== Current Game State in GS class===");
        Debug.Log("Current Player: " + GetCurrentPlayer());
        Debug.Log("Total Visits: " + GetTotalVisits());
        Debug.Log("Piece Count: " + GetAllPieces().Count);
        List<GameObject> redPieces = GetPiecesByPlayer("red");
        List<GameObject> bluePieces = GetPiecesByPlayer("blue");

        Debug.Log("Red Pieces:" + redPieces.Count);
        Debug.Log("Blue Pieces:" + bluePieces.Count);


        int width = GameObject.FindGameObjectWithTag("SipliBoard").GetComponent<BoardGenerator>().GetWidth();
        int height = GameObject.FindGameObjectWithTag("SipliBoard").GetComponent<BoardGenerator>().GetHeight();

        // Piece representation dictionary
        Dictionary<string, string> pieceRepresentations = new Dictionary<string, string>
        {
            { "red_infinity", "R8" },
            { "red_xzero", "RX" },
            { "red_three", "R3" },
            { "red_two", "R2" },
            { "red_one", "R1" },
            { "red_zero", "R0" },
            { "blu_infinity", "B8" },
            { "blu_xzero", "BX" },
            { "blu_three", "B3" },
            { "blu_two", "B2" },
            { "blu_one", "B1" },
            { "blu_zero", "B0" }
        };

        for (int y = height - 1; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < width; x++)
            {
                GameObject piece = GetPieceAtPosition(x, y);
                string pieceRepresentation = (piece != null && pieceRepresentations.ContainsKey(piece.name))
                    ? pieceRepresentations[piece.name]
                    : "--";
                row += "[" + pieceRepresentation + "]";
            }
            Debug.Log(row);
        }
    }

    public GameState()
    {
        pieces = new List<GameObject>();
        currentPlayer = "blue";
        totalVisits = 0;
    }

    private int GetPieceValue(string pieceName)
    {
        // Retrieve the piece value from the pieceRanks dictionary
        if (pieceRanks.TryGetValue(pieceName, out int value))
        {
            return value;
        }

        // Default value if the piece name is not found in the dictionary
        return 0;
    }

    public GameObject[,] GetBoard()
    {
        // Return a copy of the board to prevent direct modifications
        GameObject[,] copy = new GameObject[positions.GetLength(0), positions.GetLength(1)];
        Array.Copy(positions, copy, positions.Length);
        return copy;
    }

    public GameObject GetPieceAtPosition(int x, int y)
{
    // Retrieve the piece at the specified position
    GameObject piece = positions[x, y];

    return piece;
}

    public List<GameObject> GetPiecesByPlayer(string player)
    {
        List<GameObject> pieces = new List<GameObject>();

        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                GameObject piece = positions[x, y];

                if (piece != null && piece.GetComponent<Piece>().GetCurrentPlayer() == player)
                {
                    pieces.Add(piece);
                }
            }
        }

        return pieces;
    }

    public List<GameObject> GetAllPieces()
    {
        List<GameObject> pieces = new List<GameObject>();

        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                GameObject piece = positions[x, y];

                if (piece != null)
                {
                    pieces.Add(piece);
                }
            }
        }

        return pieces;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void NextPlayerTurn()
    {
        currentPlayer = (currentPlayer == "blue") ? "red" : "blue";
    }

    public List<GameObject> GetLegalMoves()
    {
        List<GameObject> legalMoves = new List<GameObject>();

        List<GameObject> currentPlayerPieces = GetPiecesByPlayer(currentPlayer);
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
                    if (targetPiece == null || targetPiece.GetComponent<Piece>().GetCurrentPlayer() != currentPlayer)
                    {
                        legalMoves.Add(movePlate);
                    }
                }
            }
        }

        return legalMoves;
    }

    public bool IsGameOver()
    {
        //Check if the blue infinity piece is captured
        GameObject blueInfinityPiece = GetPieceByName("blu_infinity");
        if (blueInfinityPiece == null)
        {
            Debug.Log("Game Over: Blue infinity piece is captured.");
            return true;
        }

        // Check if the red infinity piece is captured
        GameObject redInfinityPiece = GetPieceByName("red_infinity");
        if (redInfinityPiece == null)
        {
            Debug.Log("Game Over: Red infinity piece is captured.");
            return true;
        }

        return false; // Game is not over
    }

    private GameObject GetPieceByName(string name)
    {
        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                if (positions[x, y] != null && positions[x, y].name == name)
                {
                    return positions[x, y];
                }
            }
        }
        return null; // Piece not found
    }

    public void MakeMove(GameObject piece, int targetX, int targetY)
    {
        // Implement the necessary logic to make the move in your game
        piece.GetComponent<Piece>().MoveTo(targetX, targetY);
    }

    public GameObject GetPosition(int x, int y)
    {
        // Implement the necessary logic to retrieve the piece at the specified position in your game
        return pieces.FirstOrDefault(piece => piece.GetComponent<Piece>().GetXBoard() == x && piece.GetComponent<Piece>().GetYBoard() == y);
    }

    public int GetTotalVisits()
    {
        return totalVisits;
    }

    public void IncrementTotalVisits()
    {
        totalVisits++;
    }
}