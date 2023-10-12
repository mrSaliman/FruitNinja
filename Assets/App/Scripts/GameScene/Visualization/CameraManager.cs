using UnityEngine;

namespace App.GameScene.Visualization
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeReference] public Camera mainCamera;
        public Rect CameraRect
        {
            get
            {
                Vector2 position = mainCamera.transform.position;
                var width = mainCamera.orthographicSize * 2f * mainCamera.aspect; 
                var height = mainCamera.orthographicSize * 2f;
                
                var cameraRect = new Rect(position.x - width / 2, position.y - height / 2, width, height);
                
                return cameraRect;
            }
        }
    }
}