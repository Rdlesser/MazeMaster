
public enum NodeStatus
{
    Empty = 0,
    Blocked = 1,
    Traversable = 2
}

public class PathNode
{
    private Grid<PathNode> _grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public NodeStatus nodeStatus;
    public PathNode cameFromNode;
    
    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        _grid = grid;
        this.x = x;
        this.y = y;
        nodeStatus = NodeStatus.Empty;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetNodeStatus(NodeStatus newNodeStatus)
    {
        nodeStatus = newNodeStatus;
        _grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
