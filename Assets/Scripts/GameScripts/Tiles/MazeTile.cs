#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.Tilemaps;

public class MazeTile : Tile
{
    public virtual void Interact()
    {
        
    }
    
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/MazeTile")]
    public static void CreateMyTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "MazeTile", "Asset", "Save Tile", "Assets/Tiles");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<LavaTile>(), path);
    }
#endif
}
