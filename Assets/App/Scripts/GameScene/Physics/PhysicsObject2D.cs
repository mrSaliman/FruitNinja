using App.Configs.Physics;
using UnityEngine;

namespace App.GameScene.Physics
{
    public class PhysicsObject2D : MonoBehaviour
    {
        public Vector2 velocity;
        public float angularVelocity;
        public bool isFrozen;

        private void Awake()
        {
            isFrozen = true;
        }

        private void Update()
        {
            if (isFrozen) return;
            var deltaTime = Time.deltaTime;
            velocity += PhysicsConstants.Gravity * deltaTime;
            transform.position += (Vector3)velocity * deltaTime;
        }

    }
}