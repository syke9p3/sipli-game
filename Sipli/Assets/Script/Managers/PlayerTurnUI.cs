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
        controller = GameObject.FindGameObjectWithTag("GameController");
        playerTurnUI = GameObject.FindGameObjectWithTag("PlayerTurnUI");
        tile = GetComponent<Tile>();
    }

    void Update()
    {
        string currentPlayer = controller.GetComponent<Game>().GetCurrentPlayer();
        Color playerColor = tile.playerColor;
        Color aiColor = tile.aiColor;

        if (controller.GetComponent<Game>().IsGameOver() == false)
        {

            if (currentPlayer == "white")
            {
                playerTurnUI.GetComponent<Image>().color = playerColor;

            }
            else
            {
                playerTurnUI.GetComponent<Image>().color = aiColor;
            }
        } else
        {
            
        }
    }
}
