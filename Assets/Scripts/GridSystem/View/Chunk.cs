using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public GameObject[,] Tiles { get; }


    public Chunk(GameObject[,] tiles)
    {
        Tiles = tiles;
    }
}