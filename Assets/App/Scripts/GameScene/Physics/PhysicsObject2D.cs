using App.Configs.Physics;
using UnityEngine;

namespace App.GameScene.Physics
{
    public class PhysicsObject2D : MonoBehaviour
    {
        [HideInInspector] public Vector2 velocity;
        [HideInInspector] public bool isFrozen;

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