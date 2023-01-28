
public enum NodeStatus
{
    Empty = 0,
    Blocked = 1,
    Traversable = 2
}

public class PathNode
{
    private readonly Grid<PathNode> _grid;
    public readonly int X;
    public readonly int Y;

    public int GCost;
    public int HCost;
    public int FCost;

    public NodeStatus NodeStatus;
    public PathNode CameFromNode;
    
    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        _grid = grid;
        X = x;
        Y = y;
        NodeStatus = NodeStatus.Empty;
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

    public void SetNodeStatus(NodeStatus newNodeStatus)
    {
        NodeStatus = newNodeStatus;
        _grid.TriggerGridObjectChanged(X, Y);
    }

    public override string ToString()
    {
        return X + "," + Y;
    }
}
