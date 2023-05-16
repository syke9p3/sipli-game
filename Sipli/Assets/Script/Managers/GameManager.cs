using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);    
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

    }
}

public enum GameState
{
    GenerateGrid,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Lose
}
