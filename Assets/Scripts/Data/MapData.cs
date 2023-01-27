using System;
using UnityEngine;

[Serializable]
public class MapData
{
        public Vector2Int MapSize;
        public int ExtraWalls;
        public int LavaTileCount;
        public MazeTilesList MazeTilesList;
}