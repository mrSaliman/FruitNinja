using UnityEngine;

namespace App.GameScene.Visualization
{
    public class CameraManager
    {
        private readonly Camera _camera;
        public Rect CameraSize => _camera.rect;
        
        public CameraManager(Camera camera)
        {
            _camera = camera;
        }
    }
}