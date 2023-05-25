using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private PieceGenerator pieceGenerator;
    private Game game;
    private GameObject controller;
    private GameObject sipliBoard;

    private void Start()
    {
        pieceGenerator = GetComponent<PieceGenerator>();
        controller = GameObject.FindGameObjectWithTag("GameController");
        sipliBoard = GameObject.FindGameObjectWithTag("SipliBoard");
        game = GetComponent<Game>();
    }

    private void Update()
    {

        Game cp = controller.GetComponent<Game>();
        if (cp.GetCurrentPlayer() == "red" && cp.IsGameOver() == false)
        {
            MakeMove();
        }
    }

    public void MakeMove()
{
    // Get all pieces controlled by the AI player
    List<GameObject> aiPieces = sipliBoard.GetComponent<PieceGenerator>().GetPiecesByPlayer("red");

    // Filter out pieces without valid moves
    List<GameObject> piecesWithMoves = aiPieces.Where(piece => piece.GetComponent<Piece>().InitiateMovePlates()).ToList();

    if (piecesWithMoves.Count > 0)
    {
        // Select a random piece from the pieces with valid moves
        GameObject selectedPiece = piecesWithMoves[Random.Range(0, piecesWithMoves.Count)];

        // Get the move plates for the selected piece
        selectedPiece.GetComponent<Piece>().InitiateMovePlates();

        // Get the move plates generated by the selected piece
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        // Filter out move plates that lead to occupied spaces
        List<GameObject> validMovePlates = new List<GameObject>();

        foreach (GameObject movePlate in movePlates)
        {
            MovePlate movePlateScript = movePlate.GetComponent<MovePlate>();

            if (!movePlateScript.attack && movePlateScript.GetReference() == selectedPiece)
            {
                int targetX = movePlateScript.GetX();
                int targetY = movePlateScript.GetY();

                // Check if the target position is empty
                if (sipliBoard.GetComponent<PieceGenerator>().GetPosition(targetX, targetY) == null)
                {
                    validMovePlates.Add(movePlate);
                }
            }
        }

        if (validMovePlates.Count > 0)
        {
            // Select a random valid move plate
            GameObject selectedMovePlate = validMovePlates[Random.Range(0, validMovePlates.Count)];

            // Get the coordinates of the selected move plate
            MovePlate movePlateScript = selectedMovePlate.GetComponent<MovePlate>();
            int targetX = movePlateScript.GetX();
            int targetY = movePlateScript.GetY();

            // Make the move by calling the necessary methods
            selectedPiece.GetComponent<Piece>().MoveTo(targetX, targetY);

            // Generate the movement arrow
            selectedPiece.GetComponent<Piece>().GenerateMovementArrow();
            }
        else
        {
            // No valid non-attack moves, try to find attack move plates
            List<GameObject> attackPlates = new List<GameObject>();

            foreach (GameObject movePlate in movePlates)
            {
                MovePlate movePlateScript = movePlate.GetComponent<MovePlate>();

                if (movePlateScript.attack && movePlateScript.GetReference() == selectedPiece)
                {
                    attackPlates.Add(movePlate);
                }
            }

            if (attackPlates.Count > 0)
            {
                // Select a random attack move plate
                GameObject selectedMovePlate = attackPlates[Random.Range(0, attackPlates.Count)];

                // Get the coordinates of the selected move plate
                MovePlate movePlateScript = selectedMovePlate.GetComponent<MovePlate>();
                int targetX = movePlateScript.GetX();
                int targetY = movePlateScript.GetY();

                // Get the attacked piece
                GameObject attackedPiece = sipliBoard.GetComponent<PieceGenerator>().GetPosition(targetX, targetY);

                // Resolve combat between the attacking and defending pieces
                controller.GetComponent<CombatManager>().ResolveCombat(selectedPiece, attackedPiece);

                // Move the attacking piece to the target position
                selectedPiece.GetComponent<Piece>().MoveTo(targetX, targetY);
                selectedPiece.GetComponent<Piece>().GenerateMovementArrow();

                }
            }
    }
}

}