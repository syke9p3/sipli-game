//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Random = System.Random;

//public class ISMCTS : MonoBehaviour
//{
//    private Node root;

//    private GameObject sipliBoard;
//    private PieceGenerator pieceGenerator;

//    public string teamColor = "red";

//    private GameObject controller;
//    private Game game;

//    public void RunMCTS(GameState state, int numIterations)
//    {
//        root = new Node();

//        for (int i = 0; i < numIterations; i++)
//        {
//            Node selectedNode = Selection(root);
//            Node expandedNode = Expansion(selectedNode);
//            int playoutResult = Playout(expandedNode);
//            Backpropagation(expandedNode, playoutResult);
//        }

//        Action bestAction = SelectBestAction(root);
//        PerformAction(bestAction);
//    }

//    private Node Selection(Node node)
//    {
//        while (node.HasChildNodes(node))
//        {
//            node = UCB1Selection(node);
//        }

//        return node;
//    }


//    private Node UCB1Selection(Node node)
//    {
//        // Implement the UCB1 selection strategy to choose the most promising child node based on the node's statistics (e.g., visit count, average reward)

//        // Calculate the UCB1 value for each child node
//        List<Node> children = node.childNodes;
//        double explorationFactor = Math.Sqrt(2); // Exploration factor, you can adjust this value
//        double totalVisits = node.visitCount;

//        double bestUCB1 = double.MinValue;
//        Node selectedChild = null;

//        foreach (Node child in children)
//        {
//            double childVisits = child.visitCount;
//            double childScore = child.accumulatedReward;
//            double childUCB1 = (childScore / childVisits) + explorationFactor * Math.Sqrt(Math.Log(totalVisits) / childVisits);

//            if (childUCB1 > bestUCB1)
//            {
//                bestUCB1 = childUCB1;
//                selectedChild = child;
//            }
//        }

//        return selectedChild;
//    }

//    private double CalculateUCB1Value(Node node)
//    {
//        // Calculate the UCB1 value for the given node based on the node's visit count, the parent node's visit count, and the average reward of the node
//        // Return the UCB1 value

//        const double explorationFactor = 1.41; // Exploration factor for UCB1

//        double averageReward = node.accumulatedReward / node.visitCount;
//        double UCB1Value = averageReward + explorationFactor * Mathf.Sqrt(Mathf.Log(node.parentNode.visitCount) / node.visitCount);

//        return UCB1Value;
//    }

//    private Node Expansion(Node node)
//    {
//        // Expand the selected node by adding one or more child nodes representing unexplored game states
//        // Return one of the newly created child nodes

//        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
//        List<GameObject> validMovePlates = GetValidMovePlates(movePlates);

//        if (validMovePlates.Count > 0)
//        {
//            // Select a random move plate from the valid move plates
//            GameObject selectedMovePlate = validMovePlates[Random.Range(0, validMovePlates.Count)];

//            // Get the coordinates of the selected move plate
//            int targetX = selectedMovePlate.GetComponent<MovePlate>().GetX();
//            int targetY = selectedMovePlate.GetComponent<MovePlate>().GetY();

//            // Create a new child node representing the selected move plate
//            Node childNode = new Node();
//            childNode.gameState = node.gameState.ApplyMove(targetX, targetY); // Update the game state with the selected move
//            childNode.action = new Action(targetX, targetY); // Set the action associated with the selected move
//            childNode.parentNode = node; // Set the parent node

//            node.childNodes.Add(childNode); // Add the child node to the parent node's list of child nodes

//            return childNode;
//        }

//        return node;
//    }

//    private List<GameObject> GetValidMovePlates(GameObject[] movePlates)
//    {
//        // Filter out move plates that lead to occupied spaces
//        List<GameObject> validMovePlates = new List<GameObject>();

//        foreach (GameObject movePlate in movePlates)
//        {
//            MovePlate movePlateScript = movePlate.GetComponent<MovePlate>();

//            if (!movePlateScript.attack && movePlateScript.GetReference() == null)
//            {
//                validMovePlates.Add(movePlate);
//            }
//        }

//        return validMovePlates;
//    }

//    private int Playout(Node node)
//    {
//        // Simulate a playout (randomized play) from the given node or one of its child nodes
//        // Play the game using a randomized policy until reaching a terminal state
//        // Return the result or score of the playout

//        GameState gameState = node.gameState.Clone(); // Clone the game state

//        while (!gameState.IsGameOver())
//        {
//            List<Action> validActions = GetValidActions(gameState); // Get the valid actions in the current game state

//            if (validActions.Count > 0)
//            {
//                // Select a random action from the valid actions
//                Action selectedAction = validActions[Random.Range(0, validActions.Count)];

//                // Apply the selected action to the game state
//                gameState = gameState.ApplyMove(selectedAction.x, selectedAction.y);
//            }
//            else
//            {
//                break; // No valid actions, break the playout
//            }
//        }

//        // Calculate the score based on the terminal game state
//        int score = CalculateScore(gameState);

//        return score;
//    }

//    private List<Action> GetValidActions(GameState gameState)
//    {
//        // Get the valid actions (moves) that can be performed in the current game state
//        // Return the list of valid actions

//        List<Action> validActions = new List<Action>();

//        List<GameObject> aiPieces = pieceGenerator.GetPiecesByPlayer(teamColor);

//        foreach (GameObject piece in aiPieces)
//        {
//            int x = piece.GetComponent<Piece>().GetXBoard();
//            int y = piece.GetComponent<Piece>().GetYBoard();

//            if (gameState.IsValidMove(x, y))
//            {
//                validActions.Add(new Action(x, y));
//            }
//        }

//        return validActions;
//    }

//    private int CalculateScore(GameState gameState)
//    {
//        // Calculate the score based on the terminal game state
//        // Return the score

//        int score = 0;

//        // Implement the scoring logic based on the game rules and the terminal game state

//        return score;
//    }

//    private void Backpropagation(Node node, int playoutResult)
//    {
//        // Update the statistics (e.g., visit count, accumulated reward) of the visited nodes along the path
//        // from the selected node to the root node based on the playout result
//        // Increment the visit count and update the accumulated reward of the nodes

//        while (node != null)
//        {
//            node.visitCount++;
//            node.accumulatedReward += playoutResult;

//            node = node.parentNode;
//        }
//    }

//    private Action SelectBestAction(Node node)
//    {
//        // Select the best action to take based on the statistics (e.g., visit count, average reward) of the child nodes
//        // Return the best action

//        Action bestAction = null;
//        double bestAverageReward = double.MinValue;

//        foreach (Node childNode in node.childNodes)
//        {
//            double averageReward = childNode.accumulatedReward / childNode.visitCount;

//            if (averageReward > bestAverageReward)
//            {
//                bestAverageReward = averageReward;
//                bestAction = childNode.action;
//            }
//        }

//        return bestAction;
//    }

//    private void PerformAction(Action action)
//    {
//        // Perform the selected action in the game based on the chosen strategy

//        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

//        foreach (GameObject movePlate in movePlates)
//        {
//            MovePlate movePlateScript = movePlate.GetComponent<MovePlate>();

//            if (movePlateScript.GetX() == action.x && movePlateScript.GetY() == action.y)
//            {
//                // Get the selected piece from the move plate
//                GameObject selectedPiece = movePlateScript.GetReference();

//                if (selectedPiece != null)
//                {
//                    int targetX = action.x;
//                    int targetY = action.y;

//                    if (movePlateScript.attack)
//                    {
//                        // Get the attacked piece
//                        GameObject attackedPiece = pieceGenerator.GetPosition(targetX, targetY);

//                        // Resolve combat between the attacking and defending pieces
//                        controller.GetComponent<CombatManager>().ResolveCombat(selectedPiece, attackedPiece);

//                        // Create a new MoveData object for the move
//                        MoveData moveData = new MoveData(selectedPiece, selectedPiece.GetComponent<Piece>().GetXBoard(), selectedPiece.GetComponent<Piece>().GetYBoard(), targetX, targetY);

//                        // Set the combat data for the move
//                        moveData.SetCombatData(true, attackedPiece);

//                        // Move the attacking piece to the target position
//                        selectedPiece.GetComponent<Piece>().MoveTo(targetX, targetY);
//                        selectedPiece.GetComponent<Piece>().GenerateMovementArrow();

//                        // Push the move data to the move stack
//                        game.moveStack.Push(moveData);
//                    }
//                    else
//                    {
//                        // Create a new MoveData object for the move
//                        MoveData moveData = new MoveData(selectedPiece, selectedPiece.GetComponent<Piece>().GetXBoard(), selectedPiece.GetComponent<Piece>().GetYBoard(), targetX, targetY);

//                        // Make the move by calling the necessary methods
//                        selectedPiece.GetComponent<Piece>().MoveTo(targetX, targetY);

//                        // Generate the movement arrow
//                        selectedPiece.GetComponent<Piece>().GenerateMovementArrow();

//                        // Push the move data to the move stack
//                        game.moveStack.Push(moveData);
//                    }
//                }

//                break;
//            }
//        }
//    }
//}

//public class Node
//{
//    public int visitCount;
//    public double accumulatedReward;
//    public List<Node> childNodes;
//    public GameState gameState;
//    public Action action;
//    public Node parentNode;

//    public Node()
//    {
//        visitCount = 0;
//        accumulatedReward = 0;
//        childNodes = new List<Node>();
//        gameState = null;
//        action = null;
//        parentNode = null;

//    }

//    public bool HasChildNodes(Node node)
//    {
//        return node.childNodes.Count > 0;
//    }
//}

//public class Action
//{
//    public int x;
//    public int y;

//    public Action(int x, int y)
//    {
//        this.x = x;
//        this.y = y;
//    }
//}