//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AIPlayer : MonoBehaviour
//{
//    private Game gameController;
//    private string currentPlayer;

//    private void Start()
//    {
//        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
//        currentPlayer = gameController.GetCurrentPlayer();

//        if (currentPlayer != "black")
//        {
//            enabled = false; // Disable the AI player if it's not the black player's turn
//        }
//    }

//    private void Update()
//    {
//        if (gameController.IsGameOver())
//        {
//            enabled = false; // Disable the AI player if the game is over
//            return;
//        }

//        if (currentPlayer == "black")
//        {
//            // AI player's turn
//            GameObject selectedPiece = ChooseRandomPiece();

//            // Generate move plates for the selected piece
//            selectedPiece.GetComponent<PieceManager>().GenerateMovePlatesForAIPlayer();

//            // Select a random move from the available move plates
//            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
//            int randomIndex = Random.Range(0, movePlates.Length);

//            // Get the selected move from the chosen move plate
//            Vector2 randomMove = movePlates[randomIndex].GetComponent<MovePlate>().GetCoords();

//            // Simulate the movement by calling the OnMouseUp method of the chosen move plate
//            movePlates[randomIndex].GetComponent<MovePlate>().OnMouseUp();

//            // Destroy the move plates
//            foreach (GameObject movePlate in movePlates)
//            {
//                Destroy(movePlate);
//            }

//            // End the turn
//            gameController.NextTurn();
//            currentPlayer = gameController.GetCurrentPlayer();
//        }
//    }

//    private GameObject ChooseRandomPiece()
//    {
//        GameObject[] blackPieces = GameObject.FindGameObjectsWithTag("BlackPiece");
//        int randomIndex = Random.Range(0, blackPieces.Length);

//        return blackPieces[randomIndex];
//    }

//    private Vector2 ChooseRandomMove(GameObject selectedPiece)
//    {
//        PieceManager pieceManager = selectedPiece.GetComponent<PieceManager>();

//        Vector2 currentPos = new Vector2(pieceManager.GetXBoard(), pieceManager.GetYBoard());

//        // Generate a random move within the valid range of the piece
//        int randomX = Random.Range(-1, 2);
//        int randomY = Random.Range(-1, 2);
//        Vector2 randomMove = currentPos + new Vector2(randomX, randomY);

//        while (!gameController.PositionOnBoard((int)randomMove.x, (int)randomMove.y) ||
//               gameController.GetPosition((int)randomMove.x, (int)randomMove.y) != null)
//        {
//            // Keep generating random moves until a valid move is found
//            randomX = Random.Range(-1, 2);
//            randomY = Random.Range(-1, 2);
//            randomMove = currentPos + new Vector2(randomX, randomY);
//        }

//        return randomMove;
//    }
//}
