using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public PieceGenerator sipliBoard;
    public GameObject playerTurnUI;

    private string currentPlayer = "blue";
    private string playerWinner;
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

        if (playerWinner != null)
        {
            Winner(playerWinner);
        }

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

    public void UndoMove()
    {

        Debug.Log("movestack in UndoMove");
        Debug.Log(this.moveStack.Count);


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
                    return true;
                }
                else if (player == "red" && y == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        
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

