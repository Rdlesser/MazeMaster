using System;
using UnityEngine;

[Serializable]
public class MapData
{
        public Vector2Int MapSize;
        public int ExtraWalls; 
        public MazeTilesList MazeTilesList;
}