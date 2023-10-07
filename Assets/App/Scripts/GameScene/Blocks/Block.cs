using App.GameScene.Physics;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        [SerializeReference]
        private PhysicsObject2D physicsObject;
        
        [SerializeReference]
        private SpriteRenderer spriteRenderer;

        private float _radius;
        
        [HideInInspector] public float cameraHeight;
        [HideInInspector] public float cameraWidth;

        public abstract void OnHit();

        protected abstract void OnMiss();

        private void Update()
        {
            if (!(physicsObject.velocity.y < 0) ||
                (!(transform.position.y + _radius < -cameraHeight / 2f) &&
                 !(transform.position.x + _radius < -cameraWidth / 2f) &&
                 !(transform.position.x - _radius > cameraWidth / 2f))) return;
            Destroy(gameObject);
            //OnMiss();
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            Vector2 spriteSize = spriteRenderer.bounds.size;
            _radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity, float angularVelocity)
        {
            transform.position = position;
            physicsObject.SetMainAndAngularVelocity(velocity, angularVelocity);
            physicsObject.isFrozen = false;
        }
    }
}