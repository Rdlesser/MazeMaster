#if UNITY_EDITOR
using UnityEditor;
#endif

public class StarTile : MazeTile
{
    public override void Interact()
    {
        var position = gameObject.transform.position;
        GameEventDispatcher.DispatchPlayerReachedStarEvent(position.x, position.y);
    }
    
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/Star")]
    public new static void CreateMyTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "StarTile", "Asset", "Save Tile", "Assets/Tiles");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(CreateInstance<StarTile>(), path);
    }
#endif
}
