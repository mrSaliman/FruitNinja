﻿using App.GameScene.Physics;
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

        public float Radius
        {
            get => _radius * transform.localScale.x;
            private set => _radius = value;
        }

        public abstract void OnHit();

        public abstract void OnMiss();

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            var spriteSize = (Vector2)sprite.bounds.size - (new Vector2(sprite.border.x + sprite.border.z, sprite.border.y + sprite.border.w)) / sprite.pixelsPerUnit;
            Radius = Mathf.Min(spriteSize.x, spriteSize.y) / 2f;
            if (shadowController is null) return;
            shadowController = Instantiate(shadowController, transform);
            shadowController.mainSpriteRenderer.sprite = sprite;
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