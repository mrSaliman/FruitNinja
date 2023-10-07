using Physics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blocks
{
    public abstract class Block : MonoBehaviour
    {
        [SerializeReference]
        private PhysicsObject2D physicsObject;

        public BlockData blockData;
        [SerializeReference]
        private SpriteRenderer spriteRenderer;
        
        [SerializeReference]
        private Camera mainCamera;
        private float _cameraHeight;
        private float _cameraWidth;
        
        private void Awake()
        {
            SetSprite();
            
            _cameraHeight = 2f * mainCamera.orthographicSize;
            _cameraWidth = _cameraHeight * mainCamera.aspect;
        }

        public abstract void OnHit();

        protected abstract void OnMiss();

        private void Update()
        {
            if (!(physicsObject.velocity.y < 0) ||
                (!(transform.position.y + blockData.radius < -_cameraHeight / 2f) &&
                 !(transform.position.x + blockData.radius < -_cameraWidth / 2f) &&
                 !(transform.position.x - blockData.radius > _cameraWidth / 2f))) return;
            Destroy(gameObject);
            //OnMiss();
        }

        private void SetSprite()
        {
            spriteRenderer.sprite = blockData.sprite;
        }

        public void ThrowItself(Vector3 position, Vector2 velocity, float angularVelocity)
        {
            transform.position = position;
            physicsObject.SetMainAndAngularVelocity(velocity, angularVelocity);
            physicsObject.isFrozen = false;
        }
    }
}