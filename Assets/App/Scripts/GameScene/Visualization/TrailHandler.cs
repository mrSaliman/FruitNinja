using UnityEngine;

namespace App.GameScene.Visualization
{
    public class TrailHandler : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [HideInInspector] public CameraInfoProvider cameraInfoProvider;

        public void MoveTo(Vector2 position)
        {
            trailRenderer.emitting = true;
            var worldPosition = cameraInfoProvider.mainCamera.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            transform.position = worldPosition;
        }

        public void TeleportTo(Vector2 position)
        {
            trailRenderer.emitting = false;
            var worldPosition = cameraInfoProvider.mainCamera.ScreenToWorldPoint(position);
            worldPosition.z = 0;
            transform.position = worldPosition;
        }
    }
}