using System.Collections.Generic;
using App.Configs.Physics;
using App.GameScene.Gameplay_Management.Block_Management;
using App.GameScene.Gameplay_Management.Block_Management.Block_Throw;
using Unity.Mathematics;
using UnityEngine;

namespace App.GameScene.Visualization
{
    public static class GizmosDrawer
    {
        public static void DrawThrowZones(List<ThrowZone> throwZones, CameraInfoProvider cameraInfoProvider)
        {
            if (throwZones is null || cameraInfoProvider is null) return;
            foreach (var throwZone in throwZones)
            {
                var cameraSize = cameraInfoProvider.CameraRect;
            
                var radPlatformAngle = throwZone.platformAngle * Mathf.Deg2Rad;
                var sinPlatform = math.sin(radPlatformAngle);
                var cosPlatform = math.cos(radPlatformAngle);
                
                var radStartAngle = throwZone.startThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
                var radEndAngle = throwZone.endThrowAngle * Mathf.Deg2Rad + radPlatformAngle;
            
                var center = new Vector2(
                    (throwZone.xIndentation - 0.5f) * cameraSize.width,
                    (throwZone.yIndentation - 0.5f) * cameraSize.height);
                
                
                Gizmos.color = Color.green;
                
                Gizmos.DrawLine(
                    new Vector3(center.x - cosPlatform * throwZone.radius, center.y - sinPlatform * throwZone.radius), 
                    new Vector3(center.x + cosPlatform * throwZone.radius, center.y + sinPlatform * throwZone.radius));

                Gizmos.color = Color.yellow;
            
                Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radStartAngle) * throwZone.radius / 2f, center.y + math.sin(radStartAngle) * throwZone.radius / 2f));
                Gizmos.DrawLine(center, new Vector3(center.x + math.cos(radEndAngle) * throwZone.radius / 2f, center.y + math.sin(radEndAngle) * throwZone.radius / 2f));
            
                
                if(!throwZone.showTrajectory) continue;
                DrawMinMaxTrajectory(new Vector2(
                        center.x - cosPlatform * throwZone.radius, 
                        center.y - sinPlatform * throwZone.radius), 
                    throwZone, cameraSize);
                DrawMinMaxTrajectory(new Vector2(
                        center.x + cosPlatform * throwZone.radius, 
                        center.y + sinPlatform * throwZone.radius), 
                    throwZone, cameraSize);
            }

            return;

            void DrawMinMaxTrajectory(Vector2 point, ThrowZone throwZone, Rect cameraSize)
            {
                Gizmos.color = Color.red;
                DrawThrowTrajectory(throwZone.startThrowAngle + throwZone.platformAngle, point, throwZone.startThrowVelocity, cameraSize.height, cameraSize.width);
                Gizmos.color = Color.blue;
                DrawThrowTrajectory(throwZone.startThrowAngle + throwZone.platformAngle, point, throwZone.endThrowVelocity, cameraSize.height, cameraSize.width);
            
                Gizmos.color = Color.red;
                DrawThrowTrajectory(throwZone.endThrowAngle + throwZone.platformAngle, point, throwZone.startThrowVelocity, cameraSize.height, cameraSize.width);
                Gizmos.color = Color.blue;
                DrawThrowTrajectory(throwZone.endThrowAngle + throwZone.platformAngle, point, throwZone.endThrowVelocity, cameraSize.height, cameraSize.width);
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