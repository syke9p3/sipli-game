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

        if (hideRedPieces)
        {
            HidePieces("red");
            hideRedPieces = false;
        }
    }

    public void HidePieces(string player)
    {
        List<GameObject> pieces = sipliBoard.GetPiecesByPlayer(player);

        foreach (GameObject piece in pieces)
        {
            piece.GetComponent<Piece>().SetIsHidden();
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

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer == "blue") ? "red" : "blue";
    }


    public void Winner(string playerWinner)
    {
        gameOver = true;
        playerWinner = (playerWinner == "blue") ? "BLUE" : "RED";

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " WINS";
        Debug.Log(playerWinner + " is the winner");
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}