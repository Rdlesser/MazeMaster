
using System.Collections.Generic;
using UnityEngine;

public static class Misc
{
    public static void ShuffleIntList(ref List<int> list)
    {
        for (var i = 0; i < list.Count; i++) {
            var temp = list[i];
            var randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
