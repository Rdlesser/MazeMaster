
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraSizeFitter : MonoBehaviour
{
        [SerializeField] private Tilemap _tilemap;
        
        
        public void Init(MapData _mapData)
        {
                FitCamera(_mapData.MapSize);
        }
        
        private void FitCamera(Vector2Int mapSize)
        {
                if (Camera.main == null)
                {
                        return;
                }
                
                float screenRatio = (float)Screen.width / Screen.height;

                var targetRatio = mapSize.x / (float) mapSize.y;

                if (screenRatio >= targetRatio)
                {
                        Camera.main.orthographicSize = mapSize.y / 2f;
                }
                else
                {
                        float differenceInSize = targetRatio / screenRatio;
                        Camera.main.orthographicSize = mapSize.y / 2f * differenceInSize;
                }
        }
}