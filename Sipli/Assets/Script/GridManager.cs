using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform cam;

    private void Start()
    {
        GenerateGrid();

       
    }

    void GenerateGrid()
    {
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
            }
        }

        cam.transform.position = new Vector3((float) width/2 -0.5f, (float) height/2 -0.5f, -10);
    }

    

}
