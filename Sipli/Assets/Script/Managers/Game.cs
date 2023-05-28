using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public PieceGenerator sipliBoard;
    public GameObject playerTurnUI;

    private string currentPlayer = "blue";
    public string playerWinner;
    private bool gameOver = false;

    public bool hideRedPieces = false;

    public Stack<MoveData> moveStack = new Stack<MoveData>();

    private void Start()
    {
        sipliBoard = GameObject.FindGameObjectWithTag("SipliBoard").GetComponent<PieceGenerator>();
    }

    private void Update()
    {

        if (gameOver && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("vsAIGame");
        }

        
        //HidePieces("red", hideRedPieces);
        //hideRedPieces = true;

        if (IsInfinityPieceAtFarthestSide("red"))
        {
            playerWinner = "red";
            gameOver = true;
        }
        else if (IsInfinityPieceAtFarthestSide("blue"))
        {
            playerWinner = "blue";
            gameOver = true;
        }

    }

    public GameState GetCurrentGameState()
    {
        GameState gameState = new GameState();

        gameState.currentPlayer = currentPlayer;
        gameState.totalVisits = 0;
        gameState.pieces = new List<GameObject>(sipliBoard.GetAllPieces());

        for (int x = 0; x < sipliBoard.GetComponent<BoardGenerator>().GetWidth(); x++)
        {
            for (int y = 0; y < sipliBoard.GetComponent<BoardGenerator>().GetHeight(); y++)
            {
                gameState.positions[x, y] = sipliBoard.GetPosition(x, y);
            }
        }

        return gameState;
    }

    public void PrintCurrentGameState()
    {
        GameState gameState = GetCurrentGameState();

        Debug.Log("=== Current Game State ===");
        Debug.Log("Current Player: " + gameState.GetCurrentPlayer());
        Debug.Log("Total Visits: " + gameState.GetTotalVisits());
        Debug.Log("Piece Count: " + gameState.GetAllPieces().Count);
        List<GameObject> redPieces = gameState.GetPiecesByPlayer("red");
        List<GameObject> bluePieces = gameState.GetPiecesByPlayer("blue");

        Debug.Log("Red Pieces:");
        foreach (GameObject redPiece in redPieces)
        {
            Debug.Log(redPiece.name);
        }

        Debug.Log("Blue Pieces:");
        foreach (GameObject bluePiece in bluePieces)
        {
            Debug.Log(bluePiece.name);
        }
        Debug.Log("Board Status:");

        int width = sipliBoard.GetComponent<BoardGenerator>().GetWidth();
        int height = sipliBoard.GetComponent<BoardGenerator>().GetHeight();

        // Piece representation dictionary
        Dictionary<string, string> pieceRepresentations = new Dictionary<string, string>
        {
            { "red_infinity", "RI" },
            { "red_xzero", "RX" },
            { "red_three", "RH" },
            { "red_two", "RT" },
            { "red_one", "RO" },
            { "red_zero", "RZ" },
            { "blu_infinity", "BI" },
            { "blu_xzero", "BX" },
            { "blu_three", "BH" },
            { "blu_two", "BT" },
            { "blu_one", "BO" },
            { "blu_zero", "BZ" }
        };

        for (int y = height - 1; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < width; x++)
            {
                GameObject piece = gameState.GetPieceAtPosition(x, y);
                string pieceRepresentation = (piece != null && pieceRepresentations.ContainsKey(piece.name))
                    ? pieceRepresentations[piece.name]
                    : "--";
                row += "[" + pieceRepresentation + "]";
            }
            Debug.Log(row);
        }
    }

    public void UndoMove()
    {

        if (moveStack.Count > 0)
        {
            MoveData lastMove = moveStack.Pop();

            GameObject piece = lastMove.piece;
            int startX = lastMove.startX;
            int startY = lastMove.startY;
            int targetX = lastMove.targetX;
            int targetY = lastMove.targetY;

            // Revert the move by moving the piece back to its original position
            piece.GetComponent<Piece>().MoveTo(startX, startY);

            // Check if combat occurred during the move
            if (lastMove.hasCombat)
            {
                GameObject attackedPiece = lastMove.attackedPiece;

                // Reactivate the attacked piece
                attackedPiece.GetComponent<Piece>().Reactivate();

                // Move the attacked piece back to its original position
                attackedPiece.GetComponent<Piece>().MoveTo(targetX, targetY);
            }

            // Additional logic to update game state, variables, or any other relevant data
        }
    }

    public void HidePieces(string player, bool hide)
    {

        if (hide)
        {
            List<GameObject> pieces = sipliBoard.GetPiecesByPlayer(player);

            foreach (GameObject piece in pieces)
            {
                piece.GetComponent<Piece>().SetIsHidden();
            }
        }
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public string GetPlayerWinner()
    {
        return playerWinner;
    }

    public void SetPlayerWinner(string player)
    {
        playerWinner = player;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer == "blue") ? "red" : "blue";
        //PrintCurrentGameState();

        GameState gameState = GetCurrentGameState();
        //gameState.PrintCurrentGameState();
    }

    private bool IsInfinityPieceAtFarthestSide(string player)
    {
        List<GameObject> pieces = sipliBoard.GetPiecesByPlayer(player);
        foreach (GameObject piece in pieces)
        {
            if (piece.name == player + "_infinity")
            {
                int y = piece.GetComponent<Piece>().GetYBoard();
                int maxY = sipliBoard.GetComponent<BoardGenerator>().GetHeight() - 1;
                if (player == "blue" && y == maxY)
                {
                    Winner(player);
                    return true;
                }
                else if (player == "red" && y == 0)
                {
                    Winner(player);
                    return true;
                }
            }
        }
        return false;
    }

    public void Winner(string winner)
    {
        gameOver = true;
        playerWinner = winner;
        
        Tile sipliBoardTile = GameObject.FindGameObjectWithTag("SipliBoard").GetComponent<Tile>();
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;

        if (playerWinner == "blue")
        {
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().color = sipliBoardTile.playerColor;
        } else
        {
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().color = sipliBoardTile.aiColor;
        }

        playerWinner = (playerWinner == "blue") ? "BLUE" : "RED";
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " WINS";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }


}

