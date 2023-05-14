using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // instead of color, the Sprite should change instead

    [SerializeField] private Color baseColor, playerColor, aiColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject highlight;

    public void Init(string color)
    {
        switch (color)
        {
            case "playerColor":
                _renderer.color = playerColor;
                break;
            case "aiColor":
                _renderer.color = aiColor;
                break;
            default:
                _renderer.color = baseColor;
                break;
        }
    }

    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }
    
    void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
