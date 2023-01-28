﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(LavaTile))]
public class LavaTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Tile tile = (Tile)target;
        if (tile.sprite != null)
        {
            Texture2D newIcon = new Texture2D(width, height);
            Texture2D spritePreview = AssetPreview.GetAssetPreview(tile.sprite);
            EditorUtility.CopySerialized(spritePreview, newIcon);
            EditorUtility.SetDirty(tile);
            return newIcon;
        }
        
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}