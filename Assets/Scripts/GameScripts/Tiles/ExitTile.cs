#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitTile : MazeTile
{
    public override void Interact()
    {
        GameEventDispatcher.DispatchPlayerAtExitEvent();
    }
    
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/Exit")]
    public new static void CreateMyTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "ExitTile", "Asset", "Save Tile", "Assets/Tiles");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<ExitTile>(), path);
    }
#endif
}
