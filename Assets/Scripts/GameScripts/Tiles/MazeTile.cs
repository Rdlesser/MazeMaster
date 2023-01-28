
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeTile : Tile
{
    public virtual void Interact()
    {
        
    }
    
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }
}
