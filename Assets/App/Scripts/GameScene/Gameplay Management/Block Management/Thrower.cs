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
            var launchVelocity = GetRandomVelocity(throwZone, launchAngle);
            
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
        
        private Vector2 GetRandomVelocity(ThrowZone throwZone, float launchAngle)
        {
            var radLaunchAngle = launchAngle * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(radLaunchAngle), Mathf.Sin(radLaunchAngle));
            var velocity = Random.Range(throwZone.startThrowVelocity, throwZone.endThrowVelocity);
            
            return direction * velocity;
        }
    }
}