using Physics;
using Unity.Mathematics;
using UnityEngine;

namespace Blocks
{
    public abstract class Block : MonoBehaviour
    {
        private PhysicsObject2D _physicsObject;

        public BlockData blockData;
        private SpriteRenderer _spriteRenderer;
        
        private Camera _mainCamera;
        private float _cameraHeight;
        private float _cameraWidth;
        
        private void Awake()
        {
            _physicsObject = GetComponent<PhysicsObject2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            SetSprite();
            
            _mainCamera = Camera.main;
            _cameraHeight = 2f * _mainCamera.orthographicSize;
            _cameraWidth = _cameraHeight * _mainCamera.aspect;
        }

        public abstract void OnHit();

        protected abstract void OnMiss();

        private void Update()
        {
            if (!(_physicsObject.velocity.y < 0) ||
                (!(transform.position.y + blockData.radius < -_cameraHeight / 2f - 1f) &&
                 !(transform.position.x + blockData.radius < -_cameraWidth / 2f - 1f) &&
                 !(transform.position.x - blockData.radius > _cameraWidth / 2f + 1f))) return;
            Destroy(gameObject);
            //OnMiss();
        }

        private void SetSprite()
        {
            _spriteRenderer.sprite = blockData.sprite;
        }

        public void ThrowItself(Vector3 position, float angle, float velocity, float angularVelocity)
        {
            transform.position = position;
            _physicsObject.SetMainAndAngularVelocity(
                new Vector2(
                    velocity * math.cos(angle), 
                    velocity * math.sin(angle)), 
                angularVelocity);
            _physicsObject.isFrozen = false;
        }
    }
}