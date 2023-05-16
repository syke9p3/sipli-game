using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private string player;

    public Sprite ally_infinity, ally_xzero, ally_zero, ally_one, ally_two, ally_three;
    public Sprite enemy_piece;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //SetCoords();

        switch (this.name)
        {
            case "ally_infinity": this.GetComponent<SpriteRenderer>().sprite = ally_infinity; break;
            case "ally_xzero": this.GetComponent<SpriteRenderer>().sprite = ally_xzero; break;
            case "ally_zero": this.GetComponent<SpriteRenderer>().sprite = ally_zero; break;
            case "ally_one": this.GetComponent<SpriteRenderer>().sprite = ally_one; break;
            case "ally_two": this.GetComponent<SpriteRenderer>().sprite = ally_two; break;
            case "ally_three": this.GetComponent<SpriteRenderer>().sprite = ally_three; break;
            case "enemy_piece": this.GetComponent<SpriteRenderer>().sprite = enemy_piece; break;
        }
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

}
