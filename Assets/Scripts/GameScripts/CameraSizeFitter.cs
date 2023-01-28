
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
                
                var screenRatio = (float)Screen.width / Screen.height;

                var targetRatio = mapSize.x / (float) mapSize.y;

                if (screenRatio >= targetRatio)
                {
                        Camera.main.orthographicSize = mapSize.y / 2f;
                }
                else
                {
                        var differenceInSize = targetRatio / screenRatio;
                        Camera.main.orthographicSize = mapSize.y / 2f * differenceInSize;
                }
        }
}