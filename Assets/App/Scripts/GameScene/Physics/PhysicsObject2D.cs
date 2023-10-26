using App.Configs.Physics;
using UnityEngine;

namespace App.GameScene.Physics
{
    public class PhysicsObject2D : MonoBehaviour
    {
        [HideInInspector] public Vector2 velocity;

        public float AngularVelocity { get; set; }
        public float ScaleSpeed { get; set; }

        [HideInInspector] public bool isFrozen;
        [HideInInspector] public TimeController timeController;

        private void Awake()
        {
            isFrozen = true;
        }

        private void Update()
        {
            if (isFrozen) return;
            var deltaTime = timeController.DeltaTime;
            velocity += PhysicsConstants.Gravity * deltaTime;
            var mainTransform = transform;
            mainTransform.position += (Vector3)velocity * deltaTime;
            mainTransform.Rotate(0, 0, AngularVelocity * deltaTime); 
            mainTransform.localScale += Vector3.one * (ScaleSpeed * deltaTime);
        }

    }
}