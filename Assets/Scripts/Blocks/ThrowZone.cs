using Unity.Mathematics;
using UnityEngine;

namespace Blocks
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
        public Camera mainCamera;

        public void OnDrawGizmos()
        {
            var cameraHeight = 2f * mainCamera.orthographicSize;
            var cameraWidth = cameraHeight * mainCamera.aspect;
            
            Gizmos.color = Color.green;

            var radPlatformAngle = platformAngle * Mathf.Deg2Rad;
            var center = new Vector2(
                xIndentation * cameraWidth - cameraWidth / 2f, 
                yIndentation * cameraHeight - cameraHeight / 2f);
            
            Gizmos.DrawLine(
                new Vector3(center.x - math.cos(radPlatformAngle) * radius, center.y - math.sin(radPlatformAngle) * radius), 
                new Vector3(center.x + math.cos(radPlatformAngle) * radius, center.y + math.sin(radPlatformAngle) * radius));

            Gizmos.color = Color.yellow;
            
            var radStartAngle = startThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
            var radEndAngle = endThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
            
            Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radStartAngle) * radius / 2f, center.y + math.sin(radStartAngle) * radius / 2f));
            Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radEndAngle) * radius / 2f, center.y + math.sin(radEndAngle) * radius / 2f));
        }
    }
}