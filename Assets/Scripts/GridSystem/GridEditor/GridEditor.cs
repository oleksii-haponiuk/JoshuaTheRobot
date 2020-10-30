using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GridEditor : MonoBehaviour
{
    [SerializeField] private GridView terrainVisualisation;
    
    [SerializeField] private TMP_Dropdown tileTypeDropdown;
    
    [SerializeField] private Camera mainCamera;

    
    private void Awake()
    {
        InputManager.OnTouch += HandleTouch;
    }

    private void Start()
    {
        PopulateTileTypeDropDown();
    }

    private void OnDestroy()
    {
        InputManager.OnTouch -= HandleTouch;
    }
    
    private void PopulateTileTypeDropDown()
    {
        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var tileType in TileDataBase.Instance.TileTypes)
        {
            options.Add(new TMP_Dropdown.OptionData(tileType.name));
        }
        
        tileTypeDropdown.options = options;
        tileTypeDropdown.RefreshShownValue();
    }

    private void HandleTouch(Vector2 touchPosition)
    {
        var wordTouchPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        var tileCoordinates = terrainVisualisation.RealPositionToTileCoordinates(wordTouchPosition);
        Grid.Instance.SetTile(tileCoordinates.x, tileCoordinates.y, TileDataBase.Instance.IdToTileType(tileTypeDropdown.value));
    }
}
