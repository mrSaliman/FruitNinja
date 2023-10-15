using App.GameScene.Visualization;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    public class TrailHandler : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [HideInInspector] public CameraManager cameraManager;

        public void MoveTo(Vector2 position)
        {
            trailRenderer.emitting = true;
            var worldPosition = cameraManager.mainCamera.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            transform.position = worldPosition;
        }

        public void TeleportTo(Vector2 position)
        {
            trailRenderer.emitting = false;
            var worldPosition = cameraManager.mainCamera.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            transform.position = worldPosition;
        }
    }
}