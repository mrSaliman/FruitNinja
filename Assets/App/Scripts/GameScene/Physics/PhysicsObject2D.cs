using UnityEngine;

namespace App.GameScene.Physics
{
    public class PhysicsObject2D : MonoBehaviour
    {
        public Vector2 velocity;
        public float angularVelocity;
        public Vector2 gravity;
        public bool isFrozen;

        private void Awake()
        {
            isFrozen = true;
            gravity = new Vector2(0, -9.81f);
        }

        private void FixedUpdate()
        {
            if (isFrozen) return;
            var deltaTime = Time.fixedDeltaTime;
            velocity += gravity * deltaTime;
            transform.position += (Vector3)velocity * deltaTime;
            transform.rotation *= Quaternion.Euler(0f, 0f, angularVelocity * deltaTime);
        }

        public void SetMainAndAngularVelocity(Vector2 newVelocity, float newAngularVelocity)
        {
            this.velocity = newVelocity;
            this.angularVelocity = newAngularVelocity;
        }
    }
}