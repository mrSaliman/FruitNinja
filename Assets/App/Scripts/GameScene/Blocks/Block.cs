using App.GameScene.Physics;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        public PhysicsObject2D physicsObject;
        
        public SpriteRenderer spriteRenderer;

        public float Radius { get; private set; }

        public abstract void OnHit();

        public abstract void OnMiss();

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            var spriteSize = (Vector2)sprite.bounds.size - (new Vector2(sprite.border.x + sprite.border.w, sprite.border.y + sprite.border.z)) / sprite.pixelsPerUnit;
            Radius = Mathf.Min(spriteSize.x, spriteSize.y) / 2f;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity)
        {
            transform.position = position;
            physicsObject.velocity = velocity;
            physicsObject.isFrozen = false;
        }
    }
}