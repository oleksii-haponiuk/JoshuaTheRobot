using System;
using UnityEngine;
public class TerrainVisualisation : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileOffset = 1f;
    
    
    private void Start()
    {
        SpawnTerrain();
    }
    
    private void SpawnTerrain()
    {
        for (int x = 0; x < Terrain.Instance.Length; x++)
        {
            for (int y = 0; y < Terrain.Instance.Width; y++)
            {
                var tile = Terrain.Instance.GetTile(x, y);
                var tileGameObject = Instantiate(tile.tileType.tilePrefab);
                tileGameObject.transform.position = new Vector2(x * tileOffset, y * tileOffset);
            } 
        }
    } 
}