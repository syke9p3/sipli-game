//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class AI : MonoBehaviour
//{
//    private string aiPlayer; // The player controlled by the AI

//    public AI(string player)
//    {
//        aiPlayer = player;
//    }

//    public List<Move> GeneratePossibleMoves(Board board)
//    {
//        List<Move> possibleMoves = new List<Move>();

//        // Iterate through all the pieces on the board
//        foreach (Piece piece in board.GetPieces())
//        {
//            // Check if the piece belongs to the AI player
//            if (piece.GetCurrentPlayer() == aiPlayer)
//            {
//                // Generate possible moves for the current piece
//                List<Move> movesForPiece = GenerateMovesForPiece(piece, board);
//                possibleMoves.AddRange(movesForPiece);
//            }
//        }

//        return possibleMoves;
//    }

//    private List<Move> GenerateMovesForPiece(Piece piece, Board board)
//    {
//        List<Move> moves = new List<Move>();

//        // Get the position of the piece on the board
//        int x = piece.GetXBoard();
//        int y = piece.GetYBoard();

//        // Generate possible moves based on the piece type
//        switch (piece.name)
//        {
//            case "blu_infinity":
//            case "blu_xzero":
//            case "blu_zero":
//            case "blu_one":
//            case "blu_two":
//            case "blu_three":
//            case "red_infinity":
//            case "red_xzero":
//            case "red_zero":
//            case "red_one":
//            case "red_two":
//            case "red_three":
//                moves.AddRange(GenerateMovesForGenericPiece(piece, x, y, board));
//                break;
//                // Add cases for other piece types if needed
//        }

//        return moves;
//    }

//    private List<Move> GenerateMovesForGenericPiece(Piece piece, int x, int y, PieceGenerator board)
//    {
//        List<Move> moves = new List<Move>();

//        // Check the possible destination positions for the piece
//        int[][] possiblePositions = new int[][] {
//            new int[] { x, y + 1 }, // Up
//            new int[] { x, y - 1 }, // Down
//            new int[] { x - 1, y }, // Left
//            new int[] { x + 1, y }  // Right
//        };

//        foreach (int[] pos in possiblePositions)
//        {
//            int destX = pos[0];
//            int destY = pos[1];

//            // Check if the destination position is valid and empty
//            if (board.IsValidPosition(destX, destY) && board.IsPositionEmpty(destX, destY))
//            {
//                // Create a new move object and add it to the list of moves
//                Move move = new Move(piece, destX, destY);
//                moves.Add(move);
//            }
//        }

//        return moves;
//    }
//}