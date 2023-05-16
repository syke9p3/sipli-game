using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public static Piece Instance;
    void Awake()
    {
        Instance = this; 
    }


}
