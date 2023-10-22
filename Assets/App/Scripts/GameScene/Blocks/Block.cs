using App.GameScene.Physics;
using JetBrains.Annotations;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public abstract class Block : MonoBehaviour
    {
        public PhysicsObject2D physicsObject;
        
        public SpriteRenderer spriteRenderer;
        private float _radius;

        [SerializeField] [CanBeNull] private ShadowController shadowController;

        public bool IsInteractable { get; protected set; } = true;
        public bool IsHalfable { get; protected set; }

        [SerializeField] [CanBeNull] public DisappearingSprite disappearingSprite;
        [SerializeField] [CanBeNull] public ParticleSystem splashParticle;
        [CanBeNull] public Sprite splash;
        public Color particleColor;
        
        public delegate void BlockHitAction();

        public delegate void BlockMissAction();
        public event BlockHitAction OnBlockHit;
        public event BlockMissAction OnBlockMiss;

        public float Radius
        {
            get => _radius * transform.localScale.x;
            private set => _radius = value;
        }

        public virtual void OnHit()
        {
            OnBlockHit?.Invoke();
        }

        public virtual void OnMiss()
        {
            OnBlockMiss?.Invoke();
        }

        public virtual void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            var spriteSize = (Vector2)sprite.bounds.size - (new Vector2(sprite.border.x + sprite.border.z, sprite.border.y + sprite.border.w)) / sprite.pixelsPerUnit;
            Radius = Mathf.Min(spriteSize.x, spriteSize.y) / 2f;
            SetupShadow();
        }

        protected void SetupShadow()
        {
            if (shadowController is null) return;
            shadowController = Instantiate(shadowController, transform);
            shadowController.mainSpriteRenderer.sprite = spriteRenderer.sprite;
            shadowController.parent = this;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity)
        {
            transform.position = position;
            physicsObject.velocity = velocity;
            physicsObject.isFrozen = false;
        }
    }
}