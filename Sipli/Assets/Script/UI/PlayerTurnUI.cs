using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnUI : MonoBehaviour
{

    private GameObject controller;
    private GameObject playerTurnUI;
    private Tile tile;

    void Start()
    {
        tile = GetComponent<Tile>();
    }

    void Update()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        playerTurnUI = GameObject.FindGameObjectWithTag("PlayerTurnUI");

        string currentPlayer = controller.GetComponent<Game>().GetCurrentPlayer();
        Color playerColor = tile.playerColor;
        Color aiColor = tile.aiColor;

        if (controller.GetComponent<Game>().IsGameOver() == false)
        {

            if (currentPlayer == "blue")
            {
                playerTurnUI.GetComponent<Image>().color = playerColor;

            }
            else
            {
                playerTurnUI.GetComponent<Image>().color = aiColor;
            }
        } else
        {
            string winner = controller.GetComponent<Game>().GetPlayerWinner();

            if (winner == "blue")
            {
                playerTurnUI.GetComponent<Image>().color = playerColor;
            }
            else
            {
                playerTurnUI.GetComponent<Image>().color = aiColor;
            }

        }
    }
}
