using App.Configs.Physics;
using App.GameScene.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.GameScene.Gameplay_Management.Block_Management.Block_Throw
{
    public class RandomBlockThrower
    {
        public Rect CameraSize;

        public void Throw(Block block, ThrowZone throwZone)
        {
            var randomPoint = GetRandomPoint(throwZone);
            var launchAngle = GetRandomLaunchAngle(throwZone);
            var launchVelocity = GetRandomVelocity(throwZone, launchAngle);
            var timeToDrown = 
                CalculateTimeToDrown(launchVelocity.y, randomPoint.y, -CameraSize.height / 2f);
            
            SetUpRandomAnimations(block, timeToDrown);
            
            block.ThrowItself(randomPoint, launchVelocity);
        }

        private static void SetUpRandomAnimations(Block block, float timeToEnd)
        {
            var scaleSpeed = (Random.Range(0.66f, 1.55f) - 1) / timeToEnd;
            var angularVelocity = Random.Range(-360, 360) / timeToEnd;
            block.physicsObject.ScaleSpeed = scaleSpeed;
            block.physicsObject.ScaleRange = new Vector2(0.66f, 1.55f);
            block.physicsObject.AngularVelocity = angularVelocity;
        }

        private static float CalculateTimeToDrown(float initialVelocityY, float startY, float targetY)
        {
            return (initialVelocityY +
                    Mathf.Sqrt(initialVelocityY * initialVelocityY +
                               2 * -PhysicsConstants.Gravity.y * (startY - targetY))) /
                   -PhysicsConstants.Gravity.y;
        }
        
        private Vector2 GetRandomPoint(ThrowZone throwZone)
        {   
            var randomT = Random.Range(-1f, 1f);
            var radians = throwZone.platformAngle * Mathf.Deg2Rad;
            var offset = new Vector2(Mathf.Cos(radians) * randomT * throwZone.radius, Mathf.Sin(radians) * randomT * throwZone.radius);
            
            var center = new Vector2(
                (throwZone.xIndentation - 0.5f) * CameraSize.width,
                (throwZone.yIndentation - 0.5f) * CameraSize.height);
            
            return center + offset;
        }

        private static float GetRandomLaunchAngle(ThrowZone throwZone)
        {
            var minLaunchAngle = throwZone.startThrowAngle + throwZone.platformAngle;
            var maxLaunchAngle = throwZone.endThrowAngle + throwZone.platformAngle;
            return Random.Range(minLaunchAngle, maxLaunchAngle);
        }
        
        private static Vector2 GetRandomVelocity(ThrowZone throwZone, float launchAngle)
        {
            var radLaunchAngle = launchAngle * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(radLaunchAngle), Mathf.Sin(radLaunchAngle));
            var velocity = Random.Range(throwZone.startThrowVelocity, throwZone.endThrowVelocity);
            
            return direction * velocity;
        }
    }
}