﻿
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(MazeTile))]
public class MazeTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Tile Target = (Tile)target;
        if (Target.sprite != null)
        {
            Texture2D newIcon = new Texture2D(width, height);
            Texture2D spritePreview = AssetPreview.GetAssetPreview(Target.sprite);
            EditorUtility.CopySerialized(spritePreview, newIcon);
            EditorUtility.SetDirty(Target);
            return newIcon;
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}