using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private GameObject sipliBoard;
    private Game game;
    public string teamColor = "red";

    private void Start()
    {
        sipliBoard = GameObject.FindGameObjectWithTag("SipliBoard");
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    }

    private void Update()
    {
        Game cp = game.GetComponent<Game>();
        if (cp.GetCurrentPlayer() == teamColor && !cp.IsGameOver())
        {
            MakeRandomMove();
        }
    }

    public void MakeRandomMove()
    {
        List<Action> legalMoves = MoveGeneration(game.GetCurrentGameState());
        
        if (legalMoves.Count > 0)
        {
            Action selectedMove = SelectMove(legalMoves);
            MakeMove(selectedMove);
        }
    }

    public List<Action> MoveGeneration(GameState gameState)
    {

        List<Action> legalMoves = new List<Action>();

        List<GameObject> aiPieces = sipliBoard.GetComponent<PieceGenerator>().GetPiecesByPlayer(teamColor);

        foreach (GameObject piece in aiPieces)
        {
            piece.GetComponent<Piece>().InitiateMovePlates();

            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (GameObject movePlate in movePlates)
            {
                MovePlate movePlateScript = movePlate.GetComponent<MovePlate>();

                if (movePlateScript.GetReference() == piece)
                {
                    int targetX = movePlateScript.GetX();
                    int targetY = movePlateScript.GetY();

                    GameObject targetPiece = sipliBoard.GetComponent<PieceGenerator>().GetPosition(targetX, targetY);
                    if (targetPiece == null || targetPiece.GetComponent<Piece>().GetCurrentPlayer() != teamColor)
                    {
                        Action moveData = new Action(piece, piece.GetComponent<Piece>().GetXBoard(), piece.GetComponent<Piece>().GetYBoard(), targetX, targetY);
                        legalMoves.Add(moveData);
                    }
                }
            }
        }

        //Debug.Log("Legal Move count: " + legalMoves.Count + " should be more than 0 | " + game.GetCurrentPlayer());

        return legalMoves;
    }

    public Action SelectMove(List<Action> legalMoves)
    {
        int randomIndex = Random.Range(0, legalMoves.Count);
        return legalMoves[randomIndex];
    }

    public void MakeMove(Action moveData)
    {
        GameObject piece = moveData.GetPiece();
        int targetX = moveData.GetTargetX();
        int targetY = moveData.GetTargetY();

        piece.GetComponent<Piece>().MoveTo(targetX, targetY);
        piece.GetComponent<Piece>().GenerateMovementArrow();

        // Push the move data to the move stack
        game.moveStack.Push(moveData);
    }
}