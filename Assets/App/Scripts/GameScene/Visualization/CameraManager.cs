using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Visualization
{
    [CreateAssetMenu(menuName = "CameraManager", fileName = "New CameraManager")]
    public class CameraManager : ScriptableObject
    {
        [SerializeReference] public Camera camera;
        public Rect CameraRect
        {
            get
            {
                Vector2 position = camera.transform.position;
                var width = camera.orthographicSize * 2f * camera.aspect; 
                var height = camera.orthographicSize * 2f;
                
                var cameraRect = new Rect(position.x - width / 2, position.y - height / 2, width, height);
                
                return cameraRect;
            }
        }
    }
}