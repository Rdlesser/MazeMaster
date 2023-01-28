using System;
using UnityEngine;

[Serializable]
public class MapData
{
        public Vector2Int MapSize;
        [Range(0,3)]
        public int StarsAmount;
        public int ExtraWalls;
        public int LavaTileCount;
        public MazeTilesList MazeTilesList;
}