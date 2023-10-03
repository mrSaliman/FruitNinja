using UnityEngine;

namespace Scripts.Physics
{
    public class PhysicsObject2D : MonoBehaviour
    {
        private Vector2 _velocity;
        private float _angularVelocity = 30f;
        private float _gravity;

        private void Awake()
        {
            _gravity = -9.80665f;
        }

        private void Update()
        {
            _velocity.y += _gravity * Time.deltaTime;
            transform.Translate(_velocity * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward, _angularVelocity * Time.deltaTime);
        }

        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        public void SetAngularVelocity(float angularVelocity)
        {
            _angularVelocity = angularVelocity;
        }

        public void SetVelocityAndAngularVelocity(Vector2 velocity, float angularVelocity)
        {
            _velocity = velocity;
            _angularVelocity = angularVelocity;
        }
    }
}