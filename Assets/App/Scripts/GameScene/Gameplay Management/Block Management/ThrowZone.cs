using App.Configs.Physics;
using Unity.Mathematics;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    [ExecuteInEditMode]
    public class ThrowZone : MonoBehaviour
    {
        public float xIndentation;
        public float yIndentation;
        public float radius;
        public float platformAngle;
        public float startThrowAngle;
        public float endThrowAngle;
        public float startThrowVelocity;
        public float endThrowVelocity;
        public float probability;
        public bool showTrajectory;
        public Camera mainCamera;

        public void OnDrawGizmos()
        {
            var cameraHeight = 2f * mainCamera.orthographicSize;
            var cameraWidth = cameraHeight * mainCamera.aspect;
            
            var radPlatformAngle = platformAngle * Mathf.Deg2Rad;
            var sinPlatform = math.sin(radPlatformAngle);
            var cosPlatform = math.cos(radPlatformAngle);
                
            var radStartAngle = startThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
            var radEndAngle = endThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
            
            Gizmos.color = Color.green;

            var center = new Vector2(
                xIndentation * cameraWidth - cameraWidth / 2f, 
                yIndentation * cameraHeight - cameraHeight / 2f);
            
            Gizmos.DrawLine(
                new Vector3(center.x - cosPlatform * radius, center.y - sinPlatform * radius), 
                new Vector3(center.x + cosPlatform * radius, center.y + sinPlatform * radius));

            Gizmos.color = Color.yellow;
            
            Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radStartAngle) * radius / 2f, center.y + math.sin(radStartAngle) * radius / 2f));
            Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radEndAngle) * radius / 2f, center.y + math.sin(radEndAngle) * radius / 2f));
            
            if(!showTrajectory) return;
            //DrawMinMaxTrajectory(center, cameraWidth, cameraHeight);
            DrawMinMaxTrajectory(new Vector2(
                center.x - cosPlatform * radius, 
                center.y - sinPlatform * radius), 
                cameraWidth, cameraHeight);
            DrawMinMaxTrajectory(new Vector2(
                    center.x + cosPlatform * radius, 
                    center.y + sinPlatform * radius), 
                cameraWidth, cameraHeight);
        }

        private void DrawMinMaxTrajectory(Vector2 center, float cameraWidth, float cameraHeight)
        {
            Gizmos.color = Color.red;
            DrawThrowTrajectory(startThrowAngle + platformAngle, center, startThrowVelocity, cameraHeight, cameraWidth);
            Gizmos.color = Color.blue;
            DrawThrowTrajectory(startThrowAngle + platformAngle, center, endThrowVelocity, cameraHeight, cameraWidth);
            
            Gizmos.color = Color.red;
            DrawThrowTrajectory(endThrowAngle + platformAngle, center, startThrowVelocity, cameraHeight, cameraWidth);
            Gizmos.color = Color.blue;
            DrawThrowTrajectory(endThrowAngle + platformAngle, center, endThrowVelocity, cameraHeight, cameraWidth);
        }

        private void DrawThrowTrajectory(float angle, Vector2 point, float startVelocity, float cameraHeight, float cameraWidth)
        {
            var radAngle = angle * Mathf.Deg2Rad;
            var initialVelocity = new Vector2(math.cos(radAngle) * startVelocity, math.sin(radAngle) * startVelocity);

            var step = 0.2f;
            var timeInterval = 0f;
            var startPosition = point;
            Vector2 futurePosition;
            do
            {
                timeInterval += step;
                futurePosition = point + initialVelocity * timeInterval + 
                                 0.5f * PhysicsConstants.Gravity * timeInterval * timeInterval;
                Gizmos.DrawLine(startPosition, futurePosition);
                startPosition = futurePosition;
            } while (futurePosition.y <= cameraHeight / 2f &&
                     ((futurePosition.y >= -cameraHeight / 2f &&
                       futurePosition.x >= -cameraWidth / 2f &&
                       futurePosition.x <= cameraWidth / 2f) ||
                      initialVelocity.y + PhysicsConstants.Gravity.y * timeInterval > 0f));

        }
    }
}