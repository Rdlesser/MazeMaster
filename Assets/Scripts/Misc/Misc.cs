
using System.Collections.Generic;
using UnityEngine;

public static class Misc
{
    public static void ShuffleIntList(ref List<int> list)
    {
        for (int i = 0; i < list.Count; i++) {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
