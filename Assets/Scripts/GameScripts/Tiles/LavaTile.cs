#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LavaTile : MazeTile
{

    public override void Interact()
    {
        GameEventDispatcher.DispatchPlayerOnLavaEvent();
    }
    
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/Lava")]
    public new static void CreateMyTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "LavaTile", "Asset", "Save Tile", "Assets/Tiles");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<LavaTile>(), path);
    }
#endif
}
