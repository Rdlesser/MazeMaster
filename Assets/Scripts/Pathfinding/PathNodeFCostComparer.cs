
using System;
using System.Collections.Generic;

public class PathNodeFCostComparer : IComparer<PathNode>
{
    public int Compare(PathNode x, PathNode y)
    {
        if (x == null)
        {
            if (y == null)
            {
                return 0;
            }

            return -1;
        }

        if (y == null)
        {
            return 1;
        }

        return x.FCost.CompareTo(y.FCost);
    }
}
