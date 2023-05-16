using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    public static GridManager Instance;
    private Dictionary<Vector2, Tile> tiles;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();

    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                string color = "";
                if (x >= 0 && y >=3)
                {
                    color = "aiColor";
                }
                else if (x >= 0 && y <= 1)
                {
                    color = "playerColor";
                }

                spawnedTile.Init(color);

                tiles[new Vector2(x, y)] = spawnedTile;
            }

        }

        cam.transform.position = new Vector3((float) width/2 -0.5f, (float) height/2 -0.5f, -10);
   
        GameManager.Instance.ChangeState(GameState.PlayerTurn);

    }

    

}
