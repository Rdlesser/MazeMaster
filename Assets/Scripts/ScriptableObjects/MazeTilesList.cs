

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MazeTilesList", menuName = "ScriptableObjects/MazeTilesList", order = 1)]
public class MazeTilesList : ScriptableObject
{
        public List<MazeTileData> TileDatas;
}