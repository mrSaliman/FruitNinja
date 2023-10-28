using App.GameScene.Blocks.SpecialBlocks;
using App.GameScene.Gameplay_Management.Block_Management.Block_Assignment;
using App.GameScene.Physics;
using JetBrains.Annotations;
using UnityEngine;

namespace App.GameScene.Blocks
{
    public class Block : MonoBehaviour
    {
        public PhysicsObject2D physicsObject;
        
        public SpriteRenderer spriteRenderer;
        private float _radius;

        public BlockType blockType;

        [SerializeField] [CanBeNull] private ShadowController shadowController;

        public float immortalityTimer;
        
        public bool isInteractable;
        public bool isHalfable;
        public bool isDestructible;
        public bool isMissable;
        public bool isMimic;

        public float mimicTimer;

        [SerializeField] [CanBeNull] public DisappearingSprite disappearingSprite;
        [SerializeField] [CanBeNull] public ParticleSystem splashParticle;
        [SerializeField] public bool useDirectionForParticle;
        [HideInInspector] [CanBeNull] public Sprite splash;
        [HideInInspector] public Color particleColor;
        
        public delegate void BlockHitAction();

        public delegate void BlockMissAction();
        public event BlockHitAction OnBlockHit;
        public event BlockMissAction OnBlockMiss;

        public float Radius
        {
            get => _radius * transform.localScale.x;
            private set => _radius = value;
        }

        public void OnHit()
        {
            OnBlockHit?.Invoke();
        }

        public void OnMiss()
        {
            OnBlockMiss?.Invoke();
        }

        public virtual void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            var spriteSize = (Vector2)sprite.bounds.size -
                             (new Vector2(sprite.border.x + sprite.border.z,
                                 sprite.border.y + sprite.border.w)) /
                             sprite.pixelsPerUnit;
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

        public void AddVelocity(Vector2 velocity)
        {
            physicsObject.velocity += velocity;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity)
        {
            transform.position = position;
            physicsObject.velocity = velocity;
            physicsObject.isFrozen = false;
        }

        public void SetSettings(BlockSettings blockSettings)
        {
            blockType = blockSettings.blockType;
            shadowController = blockSettings.shadowController;
            isInteractable = blockSettings.isInteractable;
            isHalfable = blockSettings.isHalfable;
            isDestructible = blockSettings.isDestructible;
            isMissable = blockSettings.isMissable;
            disappearingSprite = blockSettings.disappearingSprite;
            splashParticle = blockSettings.splashParticle;
            useDirectionForParticle = blockSettings.useDirectionForParticle;
        }
    }
}