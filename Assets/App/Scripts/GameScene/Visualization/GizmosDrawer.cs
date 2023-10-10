using System.Collections.Generic;
using App.Configs.Physics;
using App.GameScene.Gameplay_Management.Block_Management;
using Unity.Mathematics;
using UnityEngine;

namespace App.GameScene.Visualization
{
    public static class GizmosDrawer
    {
        public static void DrawThrowZones(List<ThrowZone> throwZones, CameraManager cameraManager)
        {
            if (throwZones is null || cameraManager is null) return;
            foreach (var throwZone in throwZones)
            {
                var cameraSize = cameraManager.CameraSize;
            
                var radPlatformAngle = throwZone.PlatformAngle * Mathf.Deg2Rad;
                var sinPlatform = math.sin(radPlatformAngle);
                var cosPlatform = math.cos(radPlatformAngle);
                
                var radStartAngle = throwZone.StartThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
                var radEndAngle = throwZone.EndThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
            
                var center = new Vector2(
                    (throwZone.XIndentation - 0.5f) * cameraSize.width,
                    (throwZone.YIndentation - 0.5f) * cameraSize.height);
                
                
                Gizmos.color = Color.green;
                
                Gizmos.DrawLine(
                    new Vector3(center.x - cosPlatform * throwZone.Radius, center.y - sinPlatform * throwZone.Radius), 
                    new Vector3(center.x + cosPlatform * throwZone.Radius, center.y + sinPlatform * throwZone.Radius));

                Gizmos.color = Color.yellow;
            
                Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radStartAngle) * throwZone.Radius / 2f, center.y + math.sin(radStartAngle) * throwZone.Radius / 2f));
                Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radEndAngle) * throwZone.Radius / 2f, center.y + math.sin(radEndAngle) * throwZone.Radius / 2f));
            
                
                if(!throwZone.ShowTrajectory) return;
                DrawMinMaxTrajectory(new Vector2(
                        center.x - cosPlatform * throwZone.Radius, 
                        center.y - sinPlatform * throwZone.Radius), 
                    throwZone, cameraSize);
                DrawMinMaxTrajectory(new Vector2(
                        center.x + cosPlatform * throwZone.Radius, 
                        center.y + sinPlatform * throwZone.Radius), 
                    throwZone, cameraSize);
            }

            return;

            void DrawMinMaxTrajectory(Vector2 point, ThrowZone throwZone, Rect cameraSize)
            {
                Gizmos.color = Color.red;
                DrawThrowTrajectory(throwZone.StartThrowAngle + throwZone.PlatformAngle, point, throwZone.StartThrowVelocity, cameraSize.height, cameraSize.width);
                Gizmos.color = Color.blue;
                DrawThrowTrajectory(throwZone.StartThrowAngle + throwZone.PlatformAngle, point, throwZone.EndThrowVelocity, cameraSize.height, cameraSize.width);
            
                Gizmos.color = Color.red;
                DrawThrowTrajectory(throwZone.EndThrowAngle + throwZone.PlatformAngle, point, throwZone.StartThrowVelocity, cameraSize.height, cameraSize.width);
                Gizmos.color = Color.blue;
                DrawThrowTrajectory(throwZone.EndThrowAngle + throwZone.PlatformAngle, point, throwZone.EndThrowVelocity, cameraSize.height, cameraSize.width);
            }

            void DrawThrowTrajectory(float angle, Vector2 point, float startVelocity, float cameraHeight, float cameraWidth)
            {
                var radAngle = angle * Mathf.Deg2Rad;
                var initialVelocity = new Vector2(math.cos(radAngle) * startVelocity, math.sin(radAngle) * startVelocity);

                const float step = 0.2f;
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
}