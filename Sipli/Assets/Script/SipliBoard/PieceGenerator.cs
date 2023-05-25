using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    public GameObject piecePrefab;
    public GameObject[,] positions = new GameObject[5, 5];
    private string[] allyPieces = new string[9];
    private string[] enemyPieces = new string[9];
    private bool hide;

    private void Start()
    {
        GeneratePieces();
    }

    private void Update()
    {
    }

    public void GeneratePieces()
    {
        int xMissingAlly = Random.Range(0, 5); // get random number from x = 0 to 4
        int yMissingAlly = Random.Range(0, 2); // get random number from y = 0 to 1
        int xMissingEnemy = Random.Range(0, 5); // get random number from x = 0 to 4
        int yMissingEnemy = Random.Range(3, 5); // get random number from y = 3 to 4

        System.Random rng = new System.Random();

        allyPieces = new string[]
        {
            "blu_infinity",
            "blu_xzero",
            "blu_xzero",
            "blu_zero",
            "blu_zero",
            "blu_zero",
            "blu_one",
            "blu_two",
            "blu_three",
        };

        allyPieces = allyPieces.OrderBy(piece => rng.Next()).ToArray();
        int allyIndex = 0;
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                if (x == xMissingAlly && y == yMissingAlly)
                {
                    continue;
                }

                GeneratePiece(allyPieces[allyIndex], x, y);
                allyIndex++;

                if (allyIndex >= allyPieces.Length)
                {
                    break;
                }
            }
        }

        enemyPieces = new string[]
        {
            "red_infinity",
            "red_xzero",
            "red_xzero",
            "red_zero",
            "red_zero",
            "red_zero",
            "red_one",
            "red_two",
            "red_three",
        };

        enemyPieces = enemyPieces.OrderBy(piece => rng.Next()).ToArray();
        int enemyIndex = 0;

        for (int x = 0; x < 5; x++)
        {
            for (int y = 3; y < 5; y++)
            {
                if (x == xMissingEnemy && y == yMissingEnemy)
                {
                    continue;
                }

                GeneratePiece(enemyPieces[enemyIndex], x, y);
                enemyIndex++;

                if (enemyIndex >= enemyPieces.Length)
                {
                    break;
                }
            }
        }
    }

    private GameObject GeneratePiece(string name, int x, int y)
    {
        GameObject obj = Instantiate(piecePrefab, new Vector3(x, y), Quaternion.identity);
        obj.name = $"Piece {x} {y}";

        Piece pm = obj.GetComponent<Piece>();
        pm.name = name;
        pm.SetXBoard(x);
        pm.SetYBoard(y);
        pm.Activate();

        SetPosition(obj);

        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Piece pm = obj.GetComponent<Piece>();
        positions[pm.GetXBoard(), pm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }

        return true;
    }

    public List<GameObject> GetPiecesByPlayer(string playerColor)
    {
        List<GameObject> pieces = new List<GameObject>();

        for (int x = 0; x < positions.GetLength(0); x++)
        {
            for (int y = 0; y < positions.GetLength(1); y++)
            {
                GameObject piece = positions[x, y];

                if (piece != null && piece.GetComponent<Piece>().GetCurrentPlayer() == playerColor)
                {
                    pieces.Add(piece);
                }
            }
        }

        return pieces;
    }

}

