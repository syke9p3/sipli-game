using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public PieceGenerator pieceGenerator;
    public GameObject playerTurnUI;

    private string currentPlayer = "white";
    private string playerWinner;
    private bool gameOver = false;

    private void Start()
    {
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
        currentPlayer = (currentPlayer == "white") ? "black" : "white";
    }

    public void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("vsAIGame");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        playerWinner = (playerWinner == "white") ? "BLUE" : "RED";

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " WINS";
        Debug.Log(playerWinner + " is the winner");

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}