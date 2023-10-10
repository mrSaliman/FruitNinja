using App.GameScene.Physics;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        public PhysicsObject2D physicsObject;
        
        public SpriteRenderer spriteRenderer;

        private float _radius;

        public float Radius => _radius;

        public abstract void OnHit();

        public abstract void OnMiss();

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            Vector2 spriteSize = spriteRenderer.bounds.size;
            _radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity)
        {
            transform.position = position;
            physicsObject.velocity = velocity;
            physicsObject.isFrozen = false;
        }
    }
}