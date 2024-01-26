using System;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

namespace PathFinding
{
    public abstract class PathFinder
    {
        protected Grid Grid { get; }

        protected PathFinder(Grid grid)
        {
            Grid = grid;
        }

        public abstract List<Tile> GetPath(Vector2 from, Vector2 to);
    }

    public class SimplePathFinder : PathFinder
    {
        public SimplePathFinder(Grid grid) : base(grid)
        {
        }
        
        public override List<Tile> GetPath(Vector2 from, Vector2 to)
        {
            List<Tile> path = new();
            var currentTile = Grid.GetTile(from);
            path.Add(currentTile);
            
            while (currentTile != null && !currentTile.GetPosition().Equals(to))
            {
                var currentTilePosition = currentTile.GetPosition();
                var step = new Vector2();

                if (currentTilePosition.x > to.x)
                    step.x = -1;
                else if (currentTilePosition.x < to.x)
                    step.x = 1;
                else if (currentTilePosition.y > to.y)
                    step.y = -1;
                else if (currentTilePosition.y < to.y)
                    step.y = 1;
                
                currentTile = Grid.GetTile(new Vector2(
                    currentTilePosition.x + step.x,
                    currentTilePosition.y + step.y
                ));

                if (currentTile != null)
                {
                    if (currentTile.PassCost > TileType.UnreachablePassCostThreshold)
                        return path;
                    
                    path.Add(currentTile);
                }
            }

            return path;
        }
    }

    public class AStarPathFinder : PathFinder
    {
        private readonly int _diagonalExtraCost;
        
        public AStarPathFinder(Grid grid, int diagonalExtraCost) : base(grid)
        {
            _diagonalExtraCost = diagonalExtraCost;
        }
        
        public override List<Tile> GetPath(Vector2 from, Vector2 to)
        {
            if (from == to)
                return new List<Tile>();
            
            ResetTiles();
            
            var startTile = Grid.GetTile(from);
            List<Tile> toVisit = new()
            {
                startTile
            };
            List<Tile> visited = new();
            startTile.Distance = 0;
            startTile.DebugText.text = 
                "<b><size=3>" + (startTile.Distance + startTile.DistanceToGoal) + "</size></b>\n" + 
                startTile.Distance + " " + startTile.DistanceToGoal;

            while (toVisit.Count > 0)
            {
                toVisit.Sort(CompareTiles);
                
                var currentTile = toVisit[0];
                toVisit.RemoveAt(0);
                visited.Add(currentTile);

                var neighbors = currentTile.GetReachableNeighbors();
                
                foreach (var neighbor in neighbors)
                {
                    var newDistance = 
                        currentTile.Distance + 
                        neighbor.PassCost +
                        (IsDiagonalOf(currentTile, neighbor) ? _diagonalExtraCost : 0);
                    
                    if (newDistance < neighbor.Distance)
                    {
                        neighbor.Distance = newDistance;
                        neighbor.Predecessor = currentTile;
                        neighbor.DebugText.text = 
                            "<b><size=3>" + (neighbor.Distance + neighbor.DistanceToGoal) + "</size></b>\n" + 
                            neighbor.Distance + " " + neighbor.DistanceToGoal;
                    }
                    
                    if (!toVisit.Contains(neighbor) && !visited.Contains(neighbor))
                        toVisit.Add(neighbor);
                }
                
                currentTile.DebugText.text = "<color=black>" + currentTile.DebugText.text;

                if (currentTile.GetPosition().Equals(to))
                    return GetPathFromTargetToStart(Grid.GetTile(to));
            }

            Grid.ClearDebugTexts();
            
            return new List<Tile>();
        }

        public void CalculateDistancesToTarget(Vector2 to)
        {
            var currentTile = Grid.GetTile(to);
            List<Tile> toVisit = new()
            {
                currentTile
            };
            List<Tile> visited = new();
            currentTile.DistanceToGoal = 0;

            while (toVisit.Count > 0)
            {
                currentTile = toVisit[0];
                toVisit.RemoveAt(0);
                visited.Add(currentTile);
            
                var neighbors = currentTile.GetReachableNeighbors();
            
                foreach (var neighbor in neighbors)
                {
                    var newDistanceToGoal = 
                        currentTile.DistanceToGoal + 
                        neighbor.PassCost +
                        (IsDiagonalOf(currentTile, neighbor) ? _diagonalExtraCost : 0);
                    
                    if (newDistanceToGoal < neighbor.DistanceToGoal)
                        neighbor.DistanceToGoal = newDistanceToGoal;

                    if (!toVisit.Contains(neighbor) && !visited.Contains(neighbor))
                        toVisit.Add(neighbor);
                }
            }
        }

        private bool IsDiagonalOf(Tile current, Tile other)
        {
            var positionOffset = other.GetPosition() - current.GetPosition();
            var absOffsetSum = Mathf.Abs((int) positionOffset.x + (int) positionOffset.y);

            return absOffsetSum is 0 or 2;
        }

        private List<Tile> GetPathFromTargetToStart(Tile target)
        {
            List<Tile> path = new()
            {
                target
            };
            Tile currentTile = target;
            
            while (currentTile.Predecessor != null)
            {
                path.Add(currentTile.Predecessor);
                currentTile = currentTile.Predecessor;
            }

            path.Reverse();
                    
            return path;
        }
        
        private void ResetTiles()
        {
            foreach (var tile in Grid.GetTiles())
            {
                tile.Distance = Int32.MaxValue;
                tile.Predecessor = null;
            }
        }

        private int CompareTiles(Tile tile1, Tile tile2)
        {
            return tile1.CompareTo(tile2);
        }
    }
}