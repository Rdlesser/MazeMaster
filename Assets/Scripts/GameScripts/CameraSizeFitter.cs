
using UnityEngine;

public class CameraSizeFitter : MonoBehaviour
{

        public void Init(MapData mapData)
        {
                FitCamera(mapData.MapSize);
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