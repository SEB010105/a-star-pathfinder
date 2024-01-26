using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tiles
{
    [CreateAssetMenu(fileName = "TileType", menuName = "TileType", order = 0)]
    public class TileType : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int passCost;

        public static readonly int UnreachablePassCostThreshold = 99;

        public int GetPassCost()
        {
            return passCost;
        }

        private GameObject GetInstanceOfPrefab()
        {
            if (prefab)
                return Instantiate(prefab);
            return null;
        }

        public GameObject InitializeGameObject(GameObject grid, Vector3 position, float tileSize)
        {
            var gameObject = GetInstanceOfPrefab();
            
            if (gameObject)
            {
                gameObject.transform.position = position;
                gameObject.transform.SetParent(grid.transform);
                gameObject.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
            }

            return gameObject;
        }
        
        public void UninitializeGameObject(GameObject gameObject)
        {
            if (gameObject)
                Destroy(gameObject);
        }
    }

    public class Tile : IComparable<Tile>
    {
        private readonly GameObject _grid;
        private readonly Grid _gridComponent;
        private readonly Vector2 _position;
        private readonly float _tileSize;
        private TileType _tileType;
        private GameObject _gameObject;
        public int Distance { get; set; }
        public int DistanceToGoal { get; set; }
        public int PassCost { get; set; }
        public Tile Predecessor { get; set; }
        public TextMeshPro DebugText { get; set; }

        public Tile(GameObject grid, Vector2 position, float tileSize, TileType tileType, TextMeshPro debugText)
        {
            _grid = grid;
            _gridComponent = grid.GetComponent<Grid>();
            _position = new Vector2(
                    Mathf.RoundToInt(position.x),
                    Mathf.RoundToInt(position.y)
                );
            _tileSize = tileSize;
            SetTileType(tileType);

            Distance = Int32.MaxValue;
            DistanceToGoal = Int32.MaxValue;

            DebugText = debugText;
            var debugTextContainer = debugText.transform.parent.transform;
            debugTextContainer.position = GetPositionInWorld();
            debugTextContainer.SetParent(grid.transform);
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public List<Tile> GetReachableNeighbors()
        {
            List<Tile> neighbors = new()
            {
                GetReachableNeighbor(new Vector2(_position.x, _position.y - 1)), // up
                GetReachableNeighbor(new Vector2(_position.x, _position.y + 1)), // down
                GetReachableNeighbor(new Vector2(_position.x-1, _position.y)), // left
                GetReachableNeighbor(new Vector2(_position.x+1, _position.y)) // right
            };

            if (neighbors[0] != null && neighbors[2] != null)
                neighbors.Add(GetReachableNeighbor(new Vector2(_position.x-1, _position.y-1))); // up left
            if (neighbors[1] != null && neighbors[2] != null)
                neighbors.Add(GetReachableNeighbor(new Vector2(_position.x-1, _position.y+1))); // down left
            if (neighbors[0] != null && neighbors[3] != null)
                neighbors.Add(GetReachableNeighbor(new Vector2(_position.x+1, _position.y-1))); // up right
            if (neighbors[1] != null && neighbors[3] != null)
                neighbors.Add(GetReachableNeighbor(new Vector2(_position.x+1, _position.y+1))); // down right

            neighbors.RemoveAll(tile => tile == null);
            
            return neighbors;
        }

        private Tile GetReachableNeighbor(Vector2 position)
        {
            var neighbor = _gridComponent.GetTile(position);
            if (neighbor != null && neighbor.PassCost <= TileType.UnreachablePassCostThreshold)
                return neighbor;
            return null;
        }
        
        public void SetTileType(TileType tileType)
        {
            _tileType = tileType;
            
            _tileType.UninitializeGameObject(_gameObject);
            _gameObject = _tileType.InitializeGameObject(_grid, GetPositionInWorld(), _tileSize);

            PassCost = _tileType.GetPassCost();
        }

        public Vector3 GetPositionInWorld()
        {
            var gridPosition = _grid.transform.position;
            return new Vector3(
                _position.x * _tileSize + gridPosition.x,
                gridPosition.y,
                _position.y * _tileSize + gridPosition.z
            );
        }

        public int CompareTo(Tile other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var fScoreDifference = (Distance + DistanceToGoal).CompareTo(other.Distance + other.DistanceToGoal);
            if (fScoreDifference != 0)
                return fScoreDifference;
            return DistanceToGoal.CompareTo(other.DistanceToGoal);
        }
    }
}