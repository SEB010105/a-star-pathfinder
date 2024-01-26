using System.Collections.Generic;
using PathFinding;
using Tiles;
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private readonly List<Tile> _tiles = new();
    [SerializeField] private Vector2 size;
    [SerializeField] private float tileSize = 1;
    [SerializeField] private TileType clearTileType;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject debugTextPrefab;
    private AStarPathFinder _pathFinder;

    public Vector2 Size => size;

    private void Start()
    {
        Initialize();
        _pathFinder = new AStarPathFinder(this, 2);
        _pathFinder.CalculateDistancesToTarget(GetPositionInGrid(target.transform.position));
    }

    public void Clear()
    {
        foreach (var tile in _tiles)
            tile.SetTileType(clearTileType);
    }

    private void Initialize()
    {
        for (int i = 0; i < size.x * size.y; i++)
        {
            var tile = new Tile(
                gameObject, 
                new Vector2(i % size.x, (int) (i / size.x)), 
                tileSize, 
                clearTileType,
                Instantiate(debugTextPrefab).GetComponentInChildren<TextMeshPro>()
                );
            
            _tiles.Add(tile);
        }
    }

    public Vector2 GetPositionInGrid(Vector3 position)
    {
        var gridPosition = gameObject.transform.position;
        
        return GetRoundedPosition(new Vector2(
            position.x - gridPosition.x,
            position.z - gridPosition.z
        ) / tileSize);
    }
    
    public Tile GetTile(Vector2 position)
    {
        var roundedPosition = GetRoundedPosition(position);
        
        if (CheckIfPositionOutOfBounds(roundedPosition))
            return null;
        
        return _tiles[Mathf.RoundToInt(roundedPosition.y * size.x + roundedPosition.x)];
    }

    public void SetTileTypeAtPosition(TileType tileType, Vector2 position)
    {
        var tile = GetTile(position);
        tile?.SetTileType(tileType);
    }

    private Vector2 GetRoundedPosition(Vector2 position)
    {
        return new Vector2(
            Mathf.RoundToInt(position.x),
            Mathf.RoundToInt(position.y)
        );
    }

    private bool CheckIfPositionOutOfBounds(Vector2 position)
    {
        return !(
            position.x < size.x &&
            position.x >= 0 &&
            position.y < size.y &&
            position.y >= 0
        );
    }

    public List<Tile> GetPath(Vector2 from, Vector2 to)
    {
        return _pathFinder.GetPath(from, to);
    }

    public void ClearDebugTexts()
    {
        foreach (var tile in _tiles)
        {
            tile.DebugText.text = "";
        }
    }

    public List<Tile> GetTiles()
    {
        return _tiles;
    }

    public void SetDebugTextsActive(bool active)
    {
        foreach (var tile in _tiles)
        {
            tile.DebugText.transform.gameObject.SetActive(active);
        }
    }
}