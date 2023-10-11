using UnityEngine;

namespace App.GameScene.Visualization
{
    public class CameraManager
    {
        private readonly Camera _camera;
        public Rect CameraRect
        {
            get
            {
                Vector2 position = _camera.transform.position;
                var width = _camera.orthographicSize * 2f * _camera.aspect; 
                var height = _camera.orthographicSize * 2f;
                
                var cameraRect = new Rect(position.x - width / 2, position.y - height / 2, width, height);
                
                return cameraRect;
            }
        }

        public CameraManager(Camera camera)
        {
            _camera = camera;
        }
    }
}