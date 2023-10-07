using App.GameScene.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    public class Thrower : MonoBehaviour
    {
        [HideInInspector] public float cameraHeight;
        [HideInInspector] public float cameraWidth;

        
        public void Throw(Block block, ThrowZone throwZone)
        {
            var randomPoint = GetRandomPoint(throwZone);
            var launchAngle = GetRandomLaunchAngle(throwZone);
            var launchVelocity = GetRandomVelocity(throwZone, randomPoint, launchAngle);
            
            block.ThrowItself(randomPoint, launchVelocity, 30f);
        }
        
        private Vector2 GetRandomPoint(ThrowZone throwZone)
        {   
            var center = new Vector2(
                throwZone.xIndentation * cameraWidth - cameraWidth / 2f, 
                throwZone.yIndentation * cameraHeight - cameraHeight / 2f);
            
            var randomT = Random.Range(-1f, 1f);
            var radians = throwZone.platformAngle * Mathf.Deg2Rad;
            var offset = new Vector2(Mathf.Cos(radians) * randomT * throwZone.radius, Mathf.Sin(radians) * randomT * throwZone.radius);
            
            return center + offset;
        }

        private float GetRandomLaunchAngle(ThrowZone throwZone)
        {
            var minLaunchAngle = throwZone.startThrowAngle + throwZone.platformAngle;
            var maxLaunchAngle = throwZone.endThrowAngle + throwZone.platformAngle;
            return Random.Range(minLaunchAngle, maxLaunchAngle);
        }
        
        private Vector2 GetRandomVelocity(ThrowZone throwZone, Vector2 point, float launchAngle)
        {
            var g = Physics2D.gravity.magnitude;
            
            float angleInRadians = launchAngle * Mathf.Deg2Rad;

            float initialVelocityX = Mathf.Sqrt((2 * g * cameraWidth) / Mathf.Sin(2 * angleInRadians));
            float initialVelocityY = (cameraHeight / 2f - point.y) / Mathf.Sin(angleInRadians);

            return new Vector2(initialVelocityX, initialVelocityY);
            
            var verticalVelocity = Random.Range(
                Mathf.Sqrt(2 * g * -point.y), 
                Mathf.Sqrt(2 * g * (cameraHeight / 2f - point.y)));
            
            var radLaunchAngle = launchAngle * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(radLaunchAngle), Mathf.Sin(radLaunchAngle));
            
            return direction * (verticalVelocity / direction.y);
        }
    }
}