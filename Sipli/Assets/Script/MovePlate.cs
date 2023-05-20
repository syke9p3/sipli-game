using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    GameObject reference = null;

    // Board positions, not world positions
    int matrixX;
    int matrixY;

    // false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f); 
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack)
        {
            GameObject piece = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            Destroy(piece);
        }
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<PieceManager>().GetXBoard(), reference.GetComponent<PieceManager>().GetYBoard());
        reference.GetComponent<PieceManager>().SetXBoard(matrixX);
        reference.GetComponent<PieceManager>().SetYBoard(matrixY);
        reference.GetComponent<PieceManager>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);
        reference.GetComponent<PieceManager>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y) {
            matrixX = x;
            matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

}
