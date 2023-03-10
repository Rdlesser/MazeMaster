

using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private readonly Grid<PathNode> _grid;
    // TODO: Improvement - create a class for priority queue and use that instead
    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public Pathfinding(int width, int height)
    {
        _grid = new Grid<PathNode>(width, height, 10f, Vector3.zero,
            (grid, x, y) => new PathNode(grid, x, y));
    }

    public Grid<PathNode> GetGrid()
    {
        return _grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        _grid.GetXY(startWorldPosition, out var startX, out var startY);
        _grid.GetXY(endWorldPosition, out var endX, out var endY);

        var path = FindPath(startX, startY, endX, endY);

        if (path == null)
        {
            return null;    
        }

        var vectorPath = new List<Vector3>();
        foreach (var pathNode in path)
        {
            vectorPath.Add(new Vector3(pathNode.X, pathNode.Y) * _grid.GetCellSize() + Vector3.one * _grid.GetCellSize() * 0.5f);
        }

        return vectorPath;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var startNode = _grid.GetGridObject(startX, startY);
        var endNode = _grid.GetGridObject(endX, endY);
        
        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        for (var x = 0; x < _grid.GetWidth(); x++)
        {
            for (var y = 0; y < _grid.GetHeight(); y++)
            {
                var pathNode = _grid.GetGridObject(x, y);
                pathNode.GCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.CameFromNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count > 0)
        {
            var currentNode = GetLowestFCostNode(_openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode))
            {
                if (_closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (neighbourNode.NodeStatus == NodeStatus.Blocked)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                var tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.CameFromNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    // TODO: improvement - instead of dynamically identifying neighbours - precalculate neighbours as soon as we make the grid
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        var neighboursList = new List<PathNode>();

        if (currentNode.X - 1 >= 0)
        {
            neighboursList.Add(GetNode(currentNode.X - 1, currentNode.Y));
        }

        if (currentNode.X + 1 < _grid.GetWidth())
        {
            neighboursList.Add(GetNode(currentNode.X + 1, currentNode.Y)); 
        }

        if (currentNode.Y - 1 >= 0)
        {
            neighboursList.Add(GetNode(currentNode.X, currentNode.Y - 1));
        }

        if (currentNode.Y + 1 < _grid.GetHeight())
        {
            neighboursList.Add(GetNode(currentNode.X, currentNode.Y + 1));
        }

        return neighboursList;
    }

    public PathNode GetNode(int x, int y)
    {
        return _grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        var path = new List<PathNode> { endNode };
        var currentNode = endNode;

        while (currentNode.CameFromNode != null)
        {
            path.Add(currentNode.CameFromNode);
            currentNode = currentNode.CameFromNode; 
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        var xDistance = Mathf.Abs(a.X - b.X);
        var yDistance = Mathf.Abs(a.Y - b.Y);
        return MOVE_STRAIGHT_COST * (xDistance + yDistance);
    }
    
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        var lowestFCostNode = pathNodeList[0];
        for (var i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].FCost < lowestFCostNode.FCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}